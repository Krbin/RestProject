<?php


return [
    /*
    |--------------------------------------------------------------------------
    | NASA API Configuration
    |--------------------------------------------------------------------------
    |
    | Settings for interacting with the NASA APIs, particularly APOD.
    | Values are typically pulled from the .env file for security and
    | environment-specific settings.
    |
    */

    // Your NASA API Key (Get one from https://api.nasa.gov/)
    'api_key' => env('NASA_API_KEY', 'DEMO_KEY'),

    // The base URI for NASA API requests
    'base_uri' => 'https://api.nasa.gov/',

    // Default start date for the APOD API if none is found during sync
    'start_date' => env('NASA_API_START_DATE', '1995-06-16'),

    // Number of days to fetch in a single API request during sync
    'sync_batch_size' => env('NASA_SYNC_BATCH_SIZE', 30),

    // Delay in seconds between sync batches to respect API rate limits
    'sync_delay_seconds' => env('NASA_SYNC_DELAY_SECONDS', 60),

];
