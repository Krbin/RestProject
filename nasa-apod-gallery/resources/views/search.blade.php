<x-layouts.app>
    <x-slot:title>Search APOD Entries</x-slot:title>

    <h1 class="text-2xl font-semibold mb-4">Search APOD</h1>

    <form action="{{ route('search') }}" method="GET" class="mb-6">
        <div class="search-bar bg-gray-200 p-4 rounded-md border border-gray-300">
            <input type="search" name="query" value="{{ $searchTerm ?? '' }}" placeholder="Search by title or keyword..." class="w-full px-3 py-2 border border-gray-400 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent" autofocus>
        </div>
    </form>

    {{-- TODO: Add Sync Status Display if needed --}}

    @if($entries->count() > 0)
        <div class="dynamic-grid grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4">
            @foreach($entries as $entry)
                {{-- Use the Blade component --}}
                <x-apod-card :entry="$entry" />
            @endforeach
        </div>

        {{-- Pagination Links --}}
        <div class="mt-6">
            {{ $entries->appends(['query' => $searchTerm])->links() }}
        </div>
    @else
        @if($searchTerm)
            <p class="text-gray-600 text-center mt-6">No results found for "{{ $searchTerm }}".</p>
        @else
            <p class="text-gray-600 text-center mt-6">No recent entries found. The database might be empty or syncing.</p>
        @endif
    @endif

</x-layouts.app> {{-- Close the layout component --}}
