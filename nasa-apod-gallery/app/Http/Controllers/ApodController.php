<?php

namespace App\Http\Controllers;

use App\Models\ApodEntry;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB; // For raw queries/grouping
use Illuminate\Support\Facades\Storage; // If saving downloads locally first
use Illuminate\Support\Facades\Http; // For downloading image from source URL
use Illuminate\Support\Facades\Response; // For creating download response
use Illuminate\Support\Facades\Log; // For logging errors
use Illuminate\Support\Str; // For string manipulation (slug, etc.)
use Carbon\Carbon; // For date manipulation/formatting

class ApodController extends Controller
{
    /**
     * Display the search page and handle search queries.
     * Shows recent entries if no query is provided.
     *
     * @param Request $request
     * @return \Illuminate\View\View
     */
    public function search(Request $request)
    {
        $searchTerm = $request->input('query');
        // Number of items per page for pagination
        $perPage = 20; // Adjust as needed

        // Start query builder
        $query = ApodEntry::query();

        if ($searchTerm) {
            // Basic LIKE search (case-insensitive for SQLite using LOWER)
            // For other DBs like MySQL/PostgreSQL, you might use ILIKE or adjust collation
            $query->where(function ($q) use ($searchTerm) {
                $lowerTerm = strtolower($searchTerm);
                $q->where(DB::raw('LOWER(title)'), 'like', "%{$lowerTerm}%")
                  ->orWhere(DB::raw('LOWER(explanation)'), 'like', "%{$lowerTerm}%");
            });
            // TODO: Implement a more sophisticated fuzzy search service if desired.
            // Example using a simple Levenshtein distance (requires DB function or PHP implementation):
            // ->orderByRaw('LEVENSHTEIN(LOWER(title), ?) ASC', [$lowerTerm]) // Example, may not be efficient
        }

        // Always order by date descending (most recent first)
        $query->orderBy('date', 'desc');

        // Paginate the results
        $entries = $query->paginate($perPage)->withQueryString(); // withQueryString preserves search term in pagination links

        return view('search', compact('entries', 'searchTerm'));
    }

    /**
     * Display a list of years for browsing.
     * Each year includes a preview of the latest entry.
     *
     * @return \Illuminate\View\View
     */
    public function browseYears()
    {
        // Get distinct years and the latest date within each year.
        // Using a subquery or window function might be more performant on large datasets
        // but this is generally okay for SQLite unless the table is huge.
        $yearsData = ApodEntry::query()
             ->selectRaw('substr(date, 1, 4) as year, MAX(date) as latest_date') // SQLite specific date extraction
             ->groupBy('year')
             ->orderBy('year', 'desc') // Show most recent years first
             ->get();

         // Eager load preview entries to avoid N+1 query problem
         $latestDates = $yearsData->pluck('latest_date')->filter()->unique();
         $previews = ApodEntry::query()
             ->whereIn('date', $latestDates)
             ->get()
             ->keyBy('date'); // Key by date for easy lookup

         // Map data for the view
         $years = $yearsData->map(function ($yearInfo) use ($previews) {
             // Find the preview entry using the fetched map
             $preview = $previews->get($yearInfo->latest_date);
             return (object)[ // Using stdClass for simplicity in the view
                 'year' => $yearInfo->year,
                 'preview_entry' => $preview,
             ];
         })->filter(fn($y) => $y->year !== null); // Filter out potential null years if date format was bad

        return view('browse-years', compact('years'));
    }

     /**
      * Display a list of months for a given year.
      * Each month includes a preview of the latest entry.
      *
      * @param int $year The year to browse.
      * @return \Illuminate\View\View
      */
     public function browseMonths(int $year)
     {
         // Validate year input (basic)
         if ($year < 1995 || $year > Carbon::now()->year) {
             abort(404, 'Invalid year requested.');
         }

          // Get distinct months and the latest date within each month for the given year.
         $monthsData = ApodEntry::query()
             ->selectRaw('substr(date, 6, 2) as month, MAX(date) as latest_date') // SQLite specific date extraction
             ->whereRaw('substr(date, 1, 4) = ?', [$year]) // Filter by year
             ->groupBy('month')
             ->orderBy('month', 'asc') // Order months chronologically
             ->get();

         // Eager load preview entries
         $latestDates = $monthsData->pluck('latest_date')->filter()->unique();
         $previews = ApodEntry::query()
             ->whereIn('date', $latestDates)
             ->get()
             ->keyBy('date');

         // Map data for the view
         $months = $monthsData->map(function ($monthInfo) use ($year, $previews) {
             $preview = $previews->get($monthInfo->latest_date);
             return (object)[
                 'year' => $year,
                 'month' => (int)$monthInfo->month, // Cast month to integer
                 'preview_entry' => $preview,
             ];
         })->filter(fn($m) => $m->month >= 1 && $m->month <= 12); // Ensure valid month number


        return view('browse-months', compact('year', 'months'));
     }

      /**
       * Display a list of APOD entries (days) for a given year and month.
       *
       * @param int $year The year.
       * @param int $month The month.
       * @return \Illuminate\View\View
       */
      public function browseDays(int $year, int $month)
      {
            // Validate year/month input
            if ($year < 1995 || $year > Carbon::now()->year || $month < 1 || $month > 12) {
                abort(404, 'Invalid year or month requested.');
            }

            $perPage = 18; // Items per page
            $monthPadded = sprintf('%02d', $month); // Ensure month has leading zero (e.g., '01', '12')

            // Query entries for the specific year and month
            $entries = ApodEntry::query()
                ->whereRaw('substr(date, 1, 4) = ?', [$year]) // Filter by year
                ->whereRaw('substr(date, 6, 2) = ?', [$monthPadded]) // Filter by padded month
                ->orderBy('date', 'desc') // Show most recent first
                ->paginate($perPage);

            return view('browse-days', compact('year', 'month', 'entries'));
      }

     /**
      * Display the detail page for a specific APOD entry.
      * Uses Route Model Binding if possible, otherwise finds by date.
      *
      * @param string $date The date in YYYY-MM-DD format.
      * @return \Illuminate\View\View
      */
     public function showDetail(string $date) // Using string date from route constraint
     {
          // Find the entry by its primary key (date)
          // Using findOrFail will automatically throw a 404 if not found.
          $entry = ApodEntry::findOrFail($date);

          return view('show-detail', compact('entry'));
     }

     /**
      * Handle the download request for an APOD image.
      * Streams the image from its source URL.
      *
      * @param string $date The date of the entry to download.
      * @return \Symfony\Component\HttpFoundation\StreamedResponse|\Illuminate\Http\Response
      */
      public function downloadImage(string $date)
      {
          $entry = ApodEntry::query()->find($date); // Find without failing immediately

          // Validate entry exists, is an image, and has a URL
          if (!$entry || !$entry->is_image || empty($entry->url)) {
              abort(404, 'Image not found or entry is not an image.');
          }

          try {
              // Attempt to fetch the image content
              $response = Http::timeout(60)->get($entry->url); // Increase timeout for potentially large images

              if ($response->failed()) {
                  Log::error("APOD Download: Failed to fetch image from source URL.", [
                      'date' => $date,
                      'url' => $entry->url,
                      'status' => $response->status()
                  ]);
                  abort(502, 'Failed to download image from source server.'); // Bad Gateway might be appropriate
              }

              // Determine a safe filename
              $filename = Str::slug($entry->title ?: 'apod-' . $entry->date);
              // Try to get a reasonable extension from the URL or content type
              $extension = pathinfo(parse_url($entry->url, PHP_URL_PATH), PATHINFO_EXTENSION);
              if (empty($extension) || strlen($extension) > 5) { // Basic sanity check on extension
                 $contentType = $response->header('Content-Type');
                 $extension = Str::after($contentType, 'image/') ?: 'jpg'; // Default to jpg if detection fails
              }
              $safeFilename = $filename . '.' . Str::limit($extension, 4, ''); // Limit extension length

              // Return the download response
              return Response::make($response->body(), 200, [
                  'Content-Type' => $response->header('Content-Type') ?: 'application/octet-stream',
                  'Content-Disposition' => 'attachment; filename="' . $safeFilename . '"',
                  'Content-Length' => $response->header('Content-Length') ?: strlen($response->body()), // Provide length if available
              ]);

          } catch (\Exception $e) {
              Log::error("APOD Download: An error occurred while processing image download for entry {$date}", [
                  'message' => $e->getMessage(),
                  'exception' => get_class($e),
              ]);
              abort(500, 'An error occurred while processing the image download.');
          }
      }
}
