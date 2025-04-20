<x-layouts.app>
    <x-slot:title>Browse Months for {{ $year }}</x-slot:title>

    <h1 class="text-2xl font-semibold mb-4">Browse Months for {{ $year }}</h1>

     @if($months->count() > 0)
        <div class="dynamic-grid grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-4">
            @foreach($months as $monthInfo)
                {{-- Use the Blade component --}}
                <x-month-card :monthInfo="$monthInfo" />
            @endforeach
        </div>
    @else
        <p class="text-gray-600 text-center mt-6">No months found for {{ $year }}.</p>
    @endif

    <div class="mt-6">
         <a href="{{ route('browse.years') }}" class="text-blue-600 hover:underline">« Back to Years</a>
    </div>
</x-layouts.app>
