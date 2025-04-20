{{-- resources/views/components/year-card.blade.php --}}

@props(['yearInfo']) {{-- Expects object with ->year and ->preview_entry --}}

<div class="year-card border border-gray-200 rounded-lg shadow-sm bg-white overflow-hidden transition duration-200 ease-in-out hover:shadow-md hover:-translate-y-1 flex flex-col">
    {{-- Link wraps the entire card content --}}
    <a href="{{ route('browse.months', ['year' => $yearInfo->year]) }}" class="block flex flex-col flex-grow" title="Browse months in {{ $yearInfo->year }}">

        {{-- Image / Placeholder Section - Use relative for potential overlay/error states --}}
        <div class="relative">
            @if ($yearInfo->preview_entry?->is_image && $yearInfo->preview_entry?->thumbnail_url) {{-- Use nullsafe operator --}}
                {{-- The actual image --}}
                <img src="{{ $yearInfo->preview_entry->thumbnail_url }}"
                     alt="Preview image for year {{ $yearInfo->year }}"
                     loading="lazy"
                     class="w-full h-48 object-cover border-b border-gray-200 bg-gray-200"
                     {{-- Simple JS to hide broken image and show the error div below --}}
                     onerror="this.style.display='none'; this.closest('div.relative').querySelector('.placeholder-or-error').style.display='flex';"
                     >
                 {{-- Error placeholder - hidden by default, shown by onerror JS --}}
                 <div class="placeholder-or-error card-placeholder h-48 hidden flex-col items-center justify-center bg-red-100 border-b border-red-200 text-red-700">
                     <span class="placeholder-icon text-4xl mb-2 opacity-60">⚠️</span>
                     <span class="placeholder-text text-xs font-semibold">Load Failed</span>
                 </div>
            @else
                {{-- Default placeholder (no image or not an image type) --}}
                <div class="placeholder-or-error card-placeholder h-48 flex flex-col items-center justify-center bg-gray-100 border-b border-gray-200 text-gray-500">
                    <span class="placeholder-icon text-4xl mb-2 opacity-60">📅</span>
                    {{-- Display the year prominently in the placeholder --}}
                    <span class="placeholder-text text-lg font-medium">{{ $yearInfo->year }}</span>
                </div>
            @endif
        </div>

        {{-- Info Section - Pushed to bottom if image exists, centered otherwise --}}
        <div class="card-info p-3 text-center mt-auto">
            {{-- Main title is the year --}}
            <h3 class="card-title font-semibold text-lg text-gray-800">
                {{ $yearInfo->year }}
            </h3>
             {{-- Optionally add date of preview entry for context --}}
             @if($yearInfo->preview_entry)
                <p class="card-date text-xs text-gray-500">Preview: {{ $yearInfo->preview_entry->date }}</p>
             @endif
        </div>
    </a>
</div>
