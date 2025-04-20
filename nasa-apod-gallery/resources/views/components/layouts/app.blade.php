<!DOCTYPE html>
<html lang="{{ str_replace('_', '-', app()->getLocale()) }}">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    {{-- Use the $title slot or a default value --}}
    <title>{{ $title ?? 'NASA APOD Gallery' }}</title>
    <!-- Fonts -->
    <link rel="preconnect" href="https://fonts.bunny.net">
    <link href="https://fonts.bunny.net/css?family=open-sans:400,600&display=swap" rel="stylesheet" />
    <!-- Vite Assets -->
    @vite(['resources/css/app.css', 'resources/js/app.js'])
    <!-- Page specific styles -->
    {{ $styles ?? '' }}
</head>
<body class="font-sans antialiased bg-gray-100 text-gray-800 min-h-screen flex flex-col">
    {{-- Navigation Component --}}
    <x-navigation />

    {{-- Main Content Area --}}
    <main class="container mx-auto px-4 py-6 flex-grow">
        {{ $slot }} {{-- This is where the main page content will go --}}
    </main>

    {{-- Footer Component --}}
    <x-footer />

    <!-- Page specific scripts -->
    {{ $scripts ?? '' }}
</body>
</html>
