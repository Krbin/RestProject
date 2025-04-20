@props(['monthInfo']) {{-- Expects object with ->year, ->month, ->preview_entry --}}

@php
    // Calculate month name within the component
    try {
        $monthName = \Carbon\Carbon::create()->month($monthInfo->month)->format('F');
        $monthNameShort = \Carbon\Carbon::create()->month($monthInfo->month)->format('M');
    } catch (\Exception $e) {
        $monthName = 'Invalid';
        $monthNameShort = 'Inv';
    }
@endphp

<div class="month-card border border-gray-200 rounded-lg shadow-sm bg-white overflow-hidden transition duration-200 ease-in-out hover:shadow-md hover:-translate-y-1 flex flex-col">
    <a href="{{ route('browse.days', ['year' => $monthInfo->year, 'month' => $monthInfo->month]) }}" class="block flex flex-col flex-grow">
        {{-- Image / Placeholder Section --}}
        <div class="relative">
            @if ($monthInfo->preview_entry?->is_image && $monthInfo->preview_entry?->thumbnail_url) {{-- Use nullsafe operator --}}
                <img src="{{ $monthInfo->preview_entry->thumbnail_url }}"
                     alt="Preview for {{ $monthName }}"
                     loading="lazy"
                     class="w-full h-48 object-cover border-b border-gray-200 bg-gray-200"
                     onerror="this.style.display='none'; this.closest('a').querySelector('.placeholder-or-error').style.display='flex';"
                     >
                 <div class="placeholder-or-error card-placeholder h-48 hidden flex-col items-center justify-center bg-red-100 border-b border-red-200 text-red-700">
                     <span class="placeholder-icon text-4xl mb-2 opacity-60">⚠️</span>
                     <span class="placeholder-text text-xs font-semibold">Load Failed</span>
                 </div>
            @else
                <div class="placeholder-or-error card-placeholder h-48 flex flex-col items-center justify-center bg-gray-100 border-b border-gray-200 text-gray-500">
                    <span class="placeholder-icon text-4xl mb-2 opacity-60">🗓️</span>
                    <span class="placeholder-text text-base font-medium">{{ $monthName }}</span>
                </div>
            @endif
        </div>

        {{-- Info Section --}}
        <div class="card-info p-3 text-center mt-auto">
            <h3 class="card-title font-semibold text-base text-gray-800">
                {{ $monthName }}
            </h3>
             {{-- Optionally add date of preview entry --}}
             @if($monthInfo->preview_entry)
                <p class="card-date text-xs text-gray-500">Preview: {{ $monthInfo->preview_entry->date }}</p>
             @endif
        </div>
    </a>
</div>
