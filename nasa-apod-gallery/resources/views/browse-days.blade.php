@php
    // Get month name using Carbon for the title
    try {
        $monthName = \Carbon\Carbon::create()->month($month)->format('F');
    } catch (\Exception $e) {
        $monthName = 'Month ' . $month;
    }
@endphp

<x-layouts.app>
    <x-slot:title>APOD Entries for {{ $monthName }} {{ $year }}</x-slot:title>

    <h1 class="text-2xl font-semibold mb-4">APOD Entries for {{ $monthName }} {{ $year }}</h1>

     @if($entries->count() > 0)
        <div class="dynamic-grid grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4">
            @foreach($entries as $entry)
                 {{-- Use the Blade component --}}
                <x-apod-card :entry="$entry" />
            @endforeach
        </div>

        {{-- Pagination Links --}}
        <div class="mt-6">
            {{ $entries->links() }}
        </div>
    @else
        <p class="text-gray-600 text-center mt-6">No entries found for {{ $monthName }} {{ $year }}.</p>
    @endif

     <div class="mt-6">
         <a href="{{ route('browse.months', ['year' => $year]) }}" class="text-blue-600 hover:underline">« Back to {{ $year }} Months</a>
    </div>
</x-layouts.app>
