<x-layouts.app>
    <x-slot:title>{{ $entry->title ?: 'APOD Detail' }}</x-slot:title>

    <div class="image-detail-container bg-white p-6 rounded-lg shadow border border-gray-200 max-w-4xl mx-auto"> {{-- Added max-width and centering --}}
        <h1 class="text-3xl font-bold mb-1">{{ $entry->title }}</h1>
        <p class="text-gray-500 text-sm mb-4"><em>{{ $entry->date }}</em></p>

        @if ($entry->is_image)
             <div class="mb-4 text-center">
                <img src="{{ $entry->url }}"
                     alt="{{ $entry->title }}"
                     class="detail-image max-w-full h-auto rounded border border-gray-300 inline-block shadow-md"
                     onerror="this.style.display='none'; document.getElementById('img-load-error').style.display='block';"
                     >
                 <p id="img-load-error" class="text-red-600 mt-2 hidden">Failed to load image.</p>

                @if ($entry->hdurl && $entry->hdurl !== $entry->url)
                    <a href="{{ $entry->hdurl }}" target="_blank" rel="noopener noreferrer" class="block mt-2 text-blue-600 hover:underline text-sm">View HD Image</a>
                @endif
             </div>
        @elseif ($entry->media_type == 'video')
             <div class="mb-4 p-4 border rounded bg-gray-50">
                 <p><strong>Video Content</strong></p>
                 <a href="{{ $entry->url }}" target="_blank" rel="noopener noreferrer" class="text-blue-600 hover:underline break-words">{{ $entry->url }}</a> {{-- Added break-words --}}
                  <p class="text-xs text-gray-500 mt-1">(Video playback may occur in an external application or browser tab)</p>
             </div>
        @else
              <div class="mb-4 p-4 border rounded bg-gray-50">
                  <p><strong>Media Type:</strong> {{ $entry->media_type }}</p>
                 <p>URL: <a href="{{ $entry->url }}" target="_blank" rel="noopener noreferrer" class="text-blue-600 hover:underline break-words">{{ $entry->url }}</a></p> {{-- Added break-words --}}
              </div>
        @endif

        {{-- Use Tailwind prose for nice explanation formatting --}}
        <div class="prose prose-sm sm:prose lg:prose-lg xl:prose-xl max-w-none mt-6 mb-6 text-gray-700">
            @foreach(explode("\n", $entry->explanation ?? '') as $paragraph)
                <p>{{ $paragraph }}</p> {{-- Render paragraphs correctly --}}
            @endforeach
        </div>

        @if ($entry->copyright)
            <p class="text-sm text-gray-600 border-t pt-4 mt-4"><strong>Copyright:</strong> {{ $entry->copyright }}</p>
        @endif

        <div class="action-buttons mt-6 flex flex-wrap gap-3 border-t pt-4">
            @if ($entry->is_image)
                <a href="{{ route('apod.download', $entry) }}" {{-- Pass model directly --}}
                   class="inline-flex items-center px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700 text-sm font-medium transition duration-150 ease-in-out"
                   download>
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-1.5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" /></svg>
                    Download
                </a>
            @endif
            <button id="shareButton" class="inline-flex items-center px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 text-sm font-medium transition duration-150 ease-in-out" type="button">
                 <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-1.5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M8.684 13.342C8.886 12.938 9 12.482 9 12s-.114-.938-.316-1.342m0 2.684a3 3 0 110-2.684m0 2.684l6.632 3.316m-6.632-6l6.632-3.316m0 0a3 3 0 105.367-2.684 3 3 0 00-5.367 2.684zm0 9.316a3 3 0 105.367 2.684 3 3 0 00-5.367-2.684z" /></svg>
                Share
            </button>
        </div>
    </div>

    <div class="mt-6 text-center"> {{-- Centered back link --}}
         <a href="{{ url()->previous(route('search')) }}" class="text-blue-600 hover:underline">« Back</a> {{-- Provide fallback URL --}}
    </div>

    {{-- Add script slot for JS --}}
    <x-slot:scripts>
        <script>
            // Keep the existing JS for Web Share API / Clipboard fallback
            const shareButton = document.getElementById('shareButton');
            if (shareButton) { // Ensure button exists
                const shareData = {
                    title: @json($entry->title),
                    text: @json($entry->title . ' - NASA APOD (' . $entry->date . ')'),
                    url: @json(route('apod.detail', $entry))
                };

                if (navigator.share) {
                    shareButton.addEventListener('click', async () => {
                        try {
                            await navigator.share(shareData);
                            console.log('APOD shared successfully');
                        } catch (err) {
                            console.error('Error sharing APOD:', err);
                        }
                    });
                } else {
                     shareButton.addEventListener('click', () => {
                         if(navigator.clipboard) {
                             navigator.clipboard.writeText(shareData.url).then(() => {
                                 alert('Link copied to clipboard!');
                             }).catch(err => {
                                 console.error('Failed to copy link: ', err);
                                 alert('Sharing not supported. Please copy the link manually.');
                             });
                         } else {
                             alert('Sharing and clipboard not supported. Please copy the link manually.');
                             // Optionally disable button if clipboard is also unavailable
                             // shareButton.disabled = true;
                         }
                     });
                      shareButton.title = 'Copy link (Sharing not supported)'; // Update tooltip
                }
            }
        </script>
    </x-slot:scripts>

</x-layouts.app>
