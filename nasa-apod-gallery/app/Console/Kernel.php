<?php

namespace App\Console;

use Illuminate\Console\Scheduling\Schedule;
use Illuminate\Foundation\Console\Kernel as ConsoleKernel;
// Import your Job class if you plan to schedule it
use App\Jobs\SyncApodEntries;

class Kernel extends ConsoleKernel
{
    /**
     * The Artisan commands provided by your application.
     *
     * @var array
     */
    protected $commands = [
        // Register any custom Artisan commands here if you create them
        // Example: \App\Console\Commands\MyCustomCommand::class,
    ];

    /**
     * Define the application's command schedule.
     *
     * This method is called by Laravel when the schedule:run command is executed.
     *
     * @param  \Illuminate\Console\Scheduling\Schedule  $schedule
     * @return void
     */
    protected function schedule(Schedule $schedule): void
    {
        // --- THIS IS WHERE YOU DEFINE SCHEDULED TASKS ---

        // Example: Schedule the APOD Sync job to run daily at 2 AM
        // ->job() is used to schedule Queuable Jobs
        $schedule->job(new SyncApodEntries)
                 ->dailyAt('02:00') // Run once a day at 2:00 AM server time
                 ->withoutOverlapping(1440); // Prevent job running again if previous one took > 24h (optional but good)

        // Example: Run the built-in inspire command hourly
        // $schedule->command('inspire')->hourly();

        // Example: Schedule a different job on a specific queue
        // $schedule->job(new AnotherJob)->onQueue('processing')->everyFiveMinutes();
    }

    /**
     * Register the commands for the application.
     *
     * Loads commands from the Commands directory and registers console routes.
     *
     * @return void
     */
    protected function commands(): void
    {
        // Loads commands from the app/Console/Commands directory
        $this->load(__DIR__.'/Commands');

        // Requires the routes defined in routes/console.php
        require base_path('routes/console.php');
    }

protected $middlewareGroups = [
    'web' => [
        // ... other middleware ...
        \App\Http\Middleware\EnsureInitialApodSync::class, // Add this line
    ],
    // ...
];

}
