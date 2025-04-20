<?php

namespace App\Jobs;

use App\Models\ApodEntry;
use App\Services\NasaApiService;
use Illuminate\Bus\Queueable;
use Illuminate\Contracts\Queue\ShouldQueue;
use Illuminate\Foundation\Bus\Dispatchable;
use Illuminate\Queue\InteractsWithQueue;
use Illuminate\Queue\SerializesModels;
use Illuminate\Support\Carbon;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Cache; // Use Cache for last sync date state
use Throwable; // For catching exceptions

class SyncApodEntries implements ShouldQueue
{
    use Dispatchable, InteractsWithQueue, Queueable, SerializesModels;

    /**
     * The number of seconds the job can run before timing out.
     * Set higher for potentially long sync processes. (e.g., 30 minutes)
     * @var int
     */
    public int $timeout = 1800;

    /**
     * The number of times the job may be attempted.
     * @var int
     */
    public int $tries = 3;

    /**
     * The number of seconds to wait before retrying the job. Exponential backoff.
     * @var array<int>
     */
    public array $backoff = [60, 180, 300]; // Retry after 1, 3, 5 minutes

    /**
     * The service instance for interacting with the NASA API.
     * Declared here but initialized in handle() via dependency injection.
     */
    // protected NasaApiService $nasaApiService; // Not strictly needed here if injected in handle

    /**
     * An optional date to force the sync to start from.
     * Useful for testing or re-syncing specific ranges.
     */
    protected ?Carbon $forceStartDate;

    /**
     * Create a new job instance.
     *
     * @param Carbon|null $forceStartDate If provided, sync will start from this date.
     */
    public function __construct(?Carbon $forceStartDate = null)
    {
         $this->forceStartDate = $forceStartDate;
         // Optionally set higher priority or specific queue
         // $this->onQueue('sync');
    }

    /**
     * Execute the job.
     * Fetches APOD data in batches and saves/updates entries in the database.
     *
     * @param NasaApiService $nasaApiService Automatically injected by Laravel's service container.
     * @return void
     */
    public function handle(NasaApiService $nasaApiService): void
    {
        // $this->nasaApiService = $nasaApiService; // Assign if needed elsewhere in the class
        $cacheKey = 'apod_last_sync_date'; // Cache key for tracking progress
        $startDate = $this->determineStartDate($cacheKey);
        $today = Carbon::today();
        $batchSize = config('nasaapi.sync_batch_size', 30);
        $delaySeconds = config('nasaapi.sync_delay_seconds', 60);
        $apiStartDate = Carbon::parse(config('nasaapi.start_date', '1995-06-16'));

        // Exit early if already up-to-date
        if ($startDate->gt($today)) {
            Log::info('APOD Sync: Database is already up-to-date.');
            return;
        }

        Log::info("APOD Sync: Starting sync from {$startDate->toDateString()}");

        while ($startDate->lte($today)) {
            // Determine the end date for the current batch
            $endDate = $startDate->copy()->addDays($batchSize - 1);
            if ($endDate->gt($today)) {
                $endDate = $today->copy();
            }

            Log::info("APOD Sync: Fetching batch {$startDate->toDateString()} to {$endDate->toDateString()}");

            // Fetch data from the API service
            $entries = $nasaApiService->getApodData($startDate, $endDate);

            // Use the end date of the batch as the default marker in case of fetch errors/empty results
            $lastDateInBatchMarker = $endDate->copy();

            if (is_array($entries) && count($entries) > 0) {
                $savedCount = 0;
                $latestDateSuccessfullyProcessed = null;

                foreach ($entries as $entryData) {
                    // Basic validation of essential fields from API response
                    if (empty($entryData['date']) || empty($entryData['title']) || empty($entryData['media_type'])) {
                         Log::warning('APOD Sync: Skipping entry with missing essential data.', ['data' => $entryData]);
                         continue;
                    }

                    try {
                        // Use updateOrCreate to handle both inserts and updates based on the date (primary key)
                        $apodEntry = ApodEntry::updateOrCreate(
                            ['date' => $entryData['date']], // Conditions to find existing record
                            [ // Data to insert or update with
                                'title' => $entryData['title'],
                                'explanation' => $entryData['explanation'] ?? null,
                                'url' => $entryData['url'] ?? null,
                                'hdurl' => $entryData['hdurl'] ?? null,
                                'media_type' => $entryData['media_type'],
                                'copyright' => $entryData['copyright'] ?? null,
                                'service_version' => $entryData['service_version'] ?? null,
                            ]
                        );

                        if ($apodEntry->wasRecentlyCreated || $apodEntry->wasChanged()) {
                           $savedCount++;
                        }

                        // Track the latest date successfully processed in this batch
                        $currentEntryDate = Carbon::parse($entryData['date']);
                        if ($latestDateSuccessfullyProcessed === null || $currentEntryDate->gt($latestDateSuccessfullyProcessed)) {
                            $latestDateSuccessfullyProcessed = $currentEntryDate;
                        }

                    } catch (\Exception $e) {
                        // Log error if saving a specific entry fails
                        Log::error("APOD Sync: Failed to save entry for date {$entryData['date']}", [
                            'message' => $e->getMessage(),
                            'trace' => $e->getTraceAsString() // Optional: for detailed debugging
                        ]);
                        // Depending on severity, you might want to:
                        // - Continue to the next entry (current behavior)
                        // - Throw the exception to fail the job and trigger retries
                        // - Implement specific error handling
                    }
                } // End foreach entry

                Log::info("APOD Sync: Processed/Saved {$savedCount} entries for batch ending {$endDate->toDateString()}.");

                 // Update the cache marker ONLY if we successfully processed entries in this batch
                 if ($latestDateSuccessfullyProcessed !== null) {
                     $lastDateInBatchMarker = $latestDateSuccessfullyProcessed;
                 } else {
                     // If no entries were successfully processed (e.g., all invalid),
                     // still use the intended end date of the batch to advance.
                     Log::warning("APOD Sync: No entries successfully processed in batch ending {$endDate->toDateString()}. Advancing marker based on batch end date.");
                     $lastDateInBatchMarker = $endDate->copy();
                 }

            } else {
                 // Log if the API returned no valid entries for the range
                 Log::info("APOD Sync: No valid entries received from API for batch ending {$endDate->toDateString()}.");
                 // Keep $lastDateInBatchMarker as the batch end date
                 $lastDateInBatchMarker = $endDate->copy();
            }

            // --- IMPORTANT: Update Cache and Prepare for Next Loop ---
            // Always update the cache key to the determined marker date for this batch.
            // This ensures progress even if a batch had issues or was empty, preventing infinite loops.
            Cache::forever($cacheKey, $lastDateInBatchMarker->toDateString());

            // Set the start date for the *next* iteration
            $startDate = $lastDateInBatchMarker->addDay();

            // Rate limiting delay *before* potentially starting the next batch fetch
            if ($startDate->lte($today)) {
                 Log::info("APOD Sync: Waiting {$delaySeconds} seconds before next batch...");
                 sleep($delaySeconds); // Simple delay; consider more robust rate limiting if needed
            }

        } // End while loop

        Log::info('APOD Sync: Synchronization process finished.');
    }

     /**
      * Determine the start date for synchronization.
      * Prioritizes forced start date, then cache, then database, then API config start date.
      *
      * @param string $cacheKey The cache key used to store the last sync date.
      * @return Carbon The calculated start date.
      */
     protected function determineStartDate(string $cacheKey): Carbon
     {
         // 1. Check for a forced start date passed to the constructor
         if ($this->forceStartDate) {
              Log::info("APOD Sync: Using forced start date: {$this->forceStartDate->toDateString()}");
             return $this->forceStartDate->copy();
         }

         $apiStartDate = Carbon::parse(config('nasaapi.start_date', '1995-06-16'));

         // 2. Check the cache for the last successfully processed date
         $lastSyncDateStr = Cache::get($cacheKey);
         if ($lastSyncDateStr) {
             try {
                 $lastSyncDate = Carbon::parse($lastSyncDateStr);
                 // Calculate next day, ensuring it's not before the absolute API start date
                 $nextDate = $lastSyncDate->addDay();
                 return $nextDate->greaterThan($apiStartDate) ? $nextDate : $apiStartDate;
             } catch (\Exception $e) {
                  Log::error('APOD Sync: Invalid date found in cache, clearing cache and checking DB.', ['date_str' => $lastSyncDateStr]);
                  Cache::forget($cacheKey); // Clear bad cache entry
             }
         }

         // 3. Cache empty or invalid, check the database for the latest entry date as a fallback
          Log::info('APOD Sync: Cache empty or invalid, checking database for last entry date.');
          $lastDbEntry = ApodEntry::query()->orderBy('date', 'desc')->value('date'); // Get only the date value
          if ($lastDbEntry) {
             try {
                  $lastDbDate = Carbon::parse($lastDbEntry);
                  Cache::forever($cacheKey, $lastDbDate->toDateString()); // Prime the cache with the DB date
                  Log::info("APOD Sync: Primed cache with last DB date: {$lastDbDate->toDateString()}");
                  $nextDate = $lastDbDate->addDay();
                  return $nextDate->greaterThan($apiStartDate) ? $nextDate : $apiStartDate;
             } catch (\Exception $e) {
                  Log::error('APOD Sync: Invalid date found in database.', ['date_str' => $lastDbEntry]);
                  // Fall through to using the API start date if DB date is bad
             }
          }

         // 4. No cache, no valid DB entry, start from the very beginning defined in config
         Log::info('APOD Sync: No valid sync progress found, starting from API config start date.');
         return $apiStartDate;
     }

      /**
       * Handle a job failure.
       * Log the exception details.
       *
       * @param Throwable $exception The exception that caused the failure.
       * @return void
       */
      public function failed(Throwable $exception): void
      {
          // Log critical error when the job ultimately fails after retries
          Log::critical('APOD Sync Job Failed!', [
              'message' => $exception->getMessage(),
              'exception' => get_class($exception),
              'trace' => $exception->getTraceAsString(), // Limit trace length in production if needed
          ]);
          // Optionally: Send notifications (e.g., email, Slack)
          // Notification::route('mail', 'admin@example.com')->notify(new JobFailedNotification($exception));
      }
}
