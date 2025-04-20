<x-layouts.app>
    <x-slot:title>Browse by Year</x-slot:title>

    <h1 class="text-2xl font-semibold mb-4">Browse by Year</h1>

    @if($years->count() > 0)
        <div class="dynamic-grid grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4">
            @foreach($years as $yearInfo)
                {{-- Use the Blade component --}}
                <x-year-card :yearInfo="$yearInfo" />
            @endforeach
        </div>
    @else
        <p class="text-gray-600 text-center mt-6">No years found. The database might be empty or syncing.</p>
    @endif
</x-layouts.app>
