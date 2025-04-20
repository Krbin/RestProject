<?php

namespace App\Http\Middleware;

use App\Jobs\SyncApodEntries;
use Closure;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Cache;
use Illuminate\Support\Facades\Log;
use Symfony\Component\HttpFoundation\Response;

class EnsureInitialApodSync
{
    /**
     * Handle an incoming request.
     *
     * @param  \Closure(\Illuminate\Http\Request): (\Symfony\Component\HttpFoundation\Response)  $next
     */
    public function handle(Request $request, Closure $next): Response
    {
        $cacheKey = 'apod_initial_sync_dispatched';

        // Check only once per application lifecycle if possible (using static variable)
        // This minimizes cache checks on subsequent requests within the same process.
        static $alreadyChecked = false;

        if (!$alreadyChecked && !Cache::has($cacheKey)) {
            try {
                Log::info('Middleware: Dispatching initial SyncApodEntries job...');
                SyncApodEntries::dispatch(); // Dispatch to default queue
                Cache::put($cacheKey, true, now()->addYears(10)); // Flag as dispatched
                Log::info('Middleware: Initial SyncApodEntries job dispatched successfully.');
            } catch (\Throwable $e) {
                // Log error but don't block the request
                Log::error('Middleware: Failed to dispatch initial APOD sync job.', ['exception' => $e]);
            } finally {
                $alreadyChecked = true; // Mark as checked for this process lifecycle
            }
        }

        return $next($request); // Continue the request
    }
}
