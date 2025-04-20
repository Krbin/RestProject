<?php
namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Carbon;

class ApodEntry extends Model
{
    use HasFactory;
    protected $table = 'apod_entries';
    protected $primaryKey = 'date';
    public $incrementing = false;
    protected $keyType = 'string';

    protected $fillable = [ /* Fields allowed for mass assignment */
        'date', 'title', 'explanation', 'url', 'hdurl',
        'media_type', 'copyright', 'service_version',
    ];

    // Accessor for is_image check
    public function getIsImageAttribute(): bool {
        return strtolower($this->media_type) === 'image';
    }
    // Accessor for year (optional)
    public function getYearAttribute(): ?int {
        try { return Carbon::parse($this->date)->year; } catch (\Exception $e) { return null; }
    }
    // Accessor for month (optional)
    public function getMonthAttribute(): ?int {
        try { return Carbon::parse($this->date)->month; } catch (\Exception $e) { return null; }
    }
    // Accessor for thumbnail URL (basic)
    public function getThumbnailUrlAttribute(): ?string {
        return $this->is_image ? $this->url : null;
    }
}
