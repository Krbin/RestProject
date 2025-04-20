<?php
namespace App\Services;

use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;
use Carbon\Carbon;

class NasaApiService
{
    protected string $apiKey;
    protected string $baseUri;

    public function __construct() {
        $this->apiKey = config('nasaapi.api_key');
        $this->baseUri = config('nasaapi.base_uri');
    }

    public function getApodData(Carbon $startDate, Carbon $endDate): ?array {
        $response = Http::baseUrl($this->baseUri)
            ->timeout(30)->retry(3, 1000)
            ->get('planetary/apod', [
                'api_key' => $this->apiKey,
                'start_date' => $startDate->toDateString(),
                'end_date' => $endDate->toDateString(),
                'thumbs' => 'true',
            ]);

        if ($response->failed()) {
            Log::error('NASA API request failed', [/*...*/]); // Add details as before
            return null;
        }
        $data = $response->json();
        if ($response->ok() && is_array($data)) {
           if (isset($data['date']) && $startDate->equalTo($endDate)) return [$data];
           return array_filter($data, fn($item) => is_array($item) && isset($item['date']));
        } elseif ($response->ok() && is_object($data) && isset($data->date)) {
             return [(array)$data];
        }
        Log::warning('Unexpected NASA API response format', ['response' => $data]);
        return null;
    }
}
