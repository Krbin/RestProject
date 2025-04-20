<?php

use Illuminate\Support\Facades\Route;
use App\Http\Controllers\ApodController;
use App\Jobs\SyncApodEntries; // For manual sync trigger route

/*
|--------------------------------------------------------------------------
| Web Routes
|--------------------------------------------------------------------------
|
| Here is where you can register web routes for your application. These
| routes are loaded by the RouteServiceProvider and all of them will
| be assigned to the "web" middleware group. Make something great!
|
*/

// --- Search Route ---
// Handles the root URL and search queries via GET parameters
Route::get('/', [ApodController::class, 'search'])
    ->name('search'); // Named 'search'

// --- Browse Routes (Hierarchical) ---

// 1. Browse all available years
Route::get('/browse', [ApodController::class, 'browseYears'])
    ->name('browse.years'); // Named 'browse.years'

// 2. Browse months within a specific year
Route::get('/browse/{year}', [ApodController::class, 'browseMonths'])
    ->where('year', '[0-9]{4}') // Ensure {year} parameter is exactly 4 digits
    ->name('browse.months'); // Named 'browse.months' <-- This was missing

// 3. Browse days (entries) within a specific month and year
Route::get('/browse/{year}/{month}', [ApodController::class, 'browseDays'])
    // Ensure parameters are digits (basic validation)
    ->where(['year' => '[0-9]{4}', 'month' => '[0-9]{1,2}'])
    ->name('browse.days'); // Named 'browse.days' <-- This was missing

// --- Detail View Route ---
// Show details for a specific APOD entry by date
Route::get('/apod/{date}', [ApodController::class, 'showDetail'])
    // Enforce YYYY-MM-DD format for the {date} parameter
    ->where('date', '[0-9]{4}-[0-9]{2}-[0-9]{2}')
    ->name('apod.detail'); // Named 'apod.detail'

// --- Download Route ---
// Allow downloading the image for a specific entry
Route::get('/apod/{date}/download', [ApodController::class, 'downloadImage'])
    ->where('date', '[0-9]{4}-[0-9]{2}-[0-9]{2}') // Enforce YYYY-MM-DD format
    ->name('apod.download'); // Named 'apod.download'

// --- Admin/Utility Route ---
// Optional: Route to manually trigger sync (for testing/debugging)
// IMPORTANT: Protect this route in a real application (e.g., with auth middleware)
Route::get('/admin/sync-apod', function () {
    try {
        SyncApodEntries::dispatch();
        return response("APOD Sync Job Dispatched successfully!", 200);
    } catch (\Throwable $e) {
        // Log the error if dispatch fails immediately
        Log::error("Failed to dispatch SyncApodEntries job: " . $e->getMessage());
        return response("Failed to dispatch APOD Sync Job. Check logs.", 500);
    }
})->name('admin.sync'); // Named 'admin.sync'


// --- Default Laravel Welcome Route (Should be removed or commented out) ---
// Route::get('/', function () {
//     return view('welcome');
// });
