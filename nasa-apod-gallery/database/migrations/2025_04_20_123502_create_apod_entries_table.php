<?php
use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up(): void {
        Schema::create('apod_entries', function (Blueprint $table) {
            $table->string('date')->primary(); // YYYY-MM-DD
            $table->string('title')->index();
            $table->text('explanation')->nullable();
            $table->string('url')->nullable();
            $table->string('hdurl')->nullable();
            $table->string('media_type', 50);
            $table->string('copyright')->nullable();
            $table->string('service_version', 20)->nullable();
            $table->timestamps();
        });
    }
    public function down(): void { Schema::dropIfExists('apod_entries'); }
};
