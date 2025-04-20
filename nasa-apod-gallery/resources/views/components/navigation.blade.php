<nav class="bg-blue-700 text-white shadow-md sticky top-0 z-50"> {{-- Added sticky --}}
    <div class="container mx-auto px-4 py-3">
        <div class="flex justify-between items-center">
            <a href="{{ route('search') }}" class="text-xl font-semibold hover:text-blue-200 transition duration-150 ease-in-out">
                NASA APOD Gallery
            </a>
            <div class="space-x-1"> {{-- Add space between links --}}
                <a href="{{ route('search') }}"
                   class="px-3 py-2 rounded hover:bg-blue-600 transition duration-150 ease-in-out {{ request()->routeIs('search') ? 'bg-blue-800 font-semibold' : '' }}">
                   Search
                </a>
                <a href="{{ route('browse.years') }}"
                   class="px-3 py-2 rounded hover:bg-blue-600 transition duration-150 ease-in-out {{ request()->routeIs('browse.*') ? 'bg-blue-800 font-semibold' : '' }}">
                   Browse
                </a>
                {{-- Add other links if needed --}}
            </div>
        </div>
    </div>
</nav>
