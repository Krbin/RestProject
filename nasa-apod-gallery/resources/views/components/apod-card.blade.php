@props(['entry']) {{-- Define 'entry' as a required property --}}

<div {{ $attributes->merge(['class' => 'image-card border border-gray-200 rounded-lg shadow-sm bg-white overflow-hidden transition duration-200 ease-in-out hover:shadow-md hover:-translate-y-1 flex flex-col']) }}> {{-- Added flex flex-col --}}
    <a href="{{ route('apod.detail', $entry) }}" class="block flex flex-col flex-grow"> {{-- Pass model directly, added flex --}}
        {{-- Image / Placeholder Section --}}
        <div class="relative"> {{-- Relative positioning for potential overlays --}}
            @if ($entry->is_image && $entry->thumbnail_url)
                <img src="{{ $entry->thumbnail_url }}"
                     alt="{{ $entry->title }}"
                     loading="lazy"
                     class="w-full h-48 object-cover border-b border-gray-200 bg-gray-200" {{-- Consistent height --}}
                     {{-- Simple inline onerror is okay, or use JS for more complex fallback --}}
                     onerror="this.style.display='none'; this.closest('a').querySelector('.placeholder-or-error').style.display='flex';"
                     >
                {{-- Placeholder div hidden by default, shown on error --}}
                 <div class="placeholder-or-error card-placeholder h-48 hidden flex-col items-center justify-center bg-red-100 border-b border-red-200 text-red-700">
                     <span class="placeholder-icon text-4xl mb-2 opacity-60">⚠️</span>
                     <span class="placeholder-text text-xs font-semibold">Load Failed</span>
                 </div>
            @else
                <div class="placeholder-or-error card-placeholder h-48 flex flex-col items-center justify-center bg-gray-100 border-b border-gray-200 text-gray-500">
                    <span class="placeholder-icon text-4xl mb-2 opacity-60">
                        @if($entry->media_type == 'video') 🎬 @else 🖼️ @endif
                    </span>
                    <span class="placeholder-text text-sm font-medium">{{ Str::ucfirst($entry->media_type) }}</span>
                </div>
            @endif
        </div>

        {{-- Info Section --}}
        <div class="card-info p-3 text-center mt-auto"> {{-- Added mt-auto to push info down --}}
            <h3 class="card-title font-semibold text-sm text-gray-800 mb-1 truncate" title="{{ $entry->title }}">
                {{ $entry->title ?: 'Untitled' }}
            </h3>
            <p class="card-date text-xs text-gray-500">{{ $entry->date }}</p>
        </div>
    </a>
</div>
