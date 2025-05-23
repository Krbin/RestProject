# NASA APOD Gallery - Laravel Edition

This is a web application built with Laravel that displays images and information from NASA's Astronomy Picture of the Day (APOD) API. It allows users to search entries, browse them by year and month, view details, download images, and share entries.

The application runs inside Docker containers managed by Laravel Sail for a consistent development environment. It uses SQLite for the database and Laravel's database queue driver for background synchronization with the NASA APOD API.

## Features

*   **Search:** Search APOD entries by title or keywords in the explanation (basic LIKE search).
*   **Browse:** Navigate entries hierarchically:
    *   View by Year (showing preview of latest entry).
    *   View by Month within a Year (showing preview of latest entry).
    *   View all entries for a specific Month/Year.
*   **Detail View:** Displays the full image (or video link), title, date, explanation, and copyright information. HD image link provided if available.
*   **Download:** Download button for image entries.
*   **Share:** Uses the browser's native Web Share API (if available) or falls back to copying the link.
*   **Background Sync:** A queued job periodically fetches data from the NASA APOD API and stores it locally in an SQLite database to minimize API calls and improve performance. Handles rate limiting.
*   **Responsive Design:** Uses Tailwind CSS for basic responsiveness across different screen sizes.
*   **Dockerized:** Runs within Docker containers using Laravel Sail for easy setup and environment consistency.

## Requirements

*   Docker Desktop (or Docker Engine + Docker Compose)
*   Composer (for project setup if not using the Docker method)
*   A modern Web Browser

## Setup & Installation

1.  **Clone the Repository:**
    ```bash
    git clone <your-repository-url> nasa-apod-laravel-gallery
    cd nasa-apod-laravel-gallery
    ```

2.  **Copy Environment File:**
    ```bash
    cp .env.example .env
    ```

3.  **Configure `.env`:**
    *   Open the `.env` file.
    *   **Crucially, obtain a NASA API Key:** Go to [https://api.nasa.gov/](https://api.nasa.gov/), sign up for a key, and replace `DEMO_KEY` with your actual key:
        ```dotenv
        NASA_API_KEY=YOUR_ACTUAL_NASA_API_KEY
        ```
    *   Adjust `NASA_SYNC_DELAY_SECONDS` if needed (default `60` is safe for `DEMO_KEY`, can be lower for personal keys).
    *   Ensure `DB_CONNECTION=sqlite` and `QUEUE_CONNECTION=database` are set.
    *   You can optionally change `APP_NAME` and `APP_URL`.

4.  **Build & Start Docker Containers (using Sail):**
    *   *(Optional - Only if Sail wasn't included via `composer create-project`)* Install Sail: `composer require laravel/sail --dev` and `php artisan sail:install` (choose `sqlite`).
    *   Start the containers:
        ```bash
        ./vendor/bin/sail build --no-cache # Optional: Force rebuild if needed
        ./vendor/bin/sail up -d
        ```
    *   *(Troubleshooting: If you get permission errors on Linux/macOS, run `sudo chown -R $USER:$USER .` in the project root)*

5.  **Install Composer Dependencies:**
    ```bash
    ./vendor/bin/sail composer install
    ```

6.  **Generate Application Key:**
    ```bash
    ./vendor/bin/sail php artisan key:generate
    ```

7.  **Create SQLite Database File:**
    ```bash
    ./vendor/bin/sail touch database/database.sqlite
    ```

8.  **Run Database Migrations:** This creates the `apod_entries` and `jobs` tables.
    ```bash
    ./vendor/bin/sail php artisan migrate
    ```

9.  **Install Frontend Dependencies:**
    ```bash
    ./vendor/bin/sail npm install
    ```

10. **Compile Frontend Assets:**
    ```bash
    ./vendor/bin/sail npm run build # For production build
    # OR during development:
    # ./vendor/bin/sail npm run dev  # Keep this running in a separate terminal
    ```

**Step 11: Run Queue Worker & Initial Sync**

1.  **Run Queue Worker:** Open a **new terminal window**, navigate to the project directory, and start the queue worker. This process handles the background API synchronization.
    ```bash
    ./vendor/bin/sail php artisan queue:work --tries=3 --timeout=1800
    ```
    *   **Keep this terminal window open** while you want the sync job to be processed. For production, use a process manager like Supervisor (configured within your Docker setup).

2.  **Trigger Initial Sync:** The database starts empty. You need to trigger the first sync job.
    *   **Option A (Web Route - If enabled):** Visit `http://localhost/admin/sync-apod` (or your configured `APP_URL`) in your browser. (Note: This route should be protected in a real application).
    *   **Option B (Tinker):** In your *first* terminal window (where you ran `sail up`), execute:
        ```bash
        ./vendor/bin/sail php artisan tinker
        ```
        Then inside Tinker:
        ```php
        >>> App\Jobs\SyncApodEntries::dispatch()
        >>> exit
        ```
    *   **Monitor:** Watch the output in the `queue:work` terminal. The initial sync will take a significant amount of time due to fetching historical data and API rate limits. Check `storage/logs/laravel.log` for detailed logs and potential errors.

**Step 12: Access the Application**

*   Once the queue worker has processed some sync batches (it might take several minutes to see the first entries), open your web browser and navigate to:
    `http://localhost` (or your configured `APP_URL`).

You should now be able to search, browse, and view the NASA APOD entries!

