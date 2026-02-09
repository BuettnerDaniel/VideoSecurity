namespace VideoSecurity.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using VideoSecurity.Playback.Contracts;

    [ApiController]
    [Route("api/[controller]")]
    public class PlaybackController : ControllerBase
    {
        private readonly IPlaybackService _playbackService;

        public PlaybackController(IPlaybackService playbackService)
        {
            _playbackService = playbackService;
        }

        // GET /api/playback/segments?cameraId=...&from=...&to=...
        [HttpGet("segments")]
        public async Task<IActionResult> SearchSegments(
            [FromQuery] Guid cameraId,
            [FromQuery] DateTime from,
            [FromQuery] DateTime to,
            CancellationToken ct)
        {
            var segments = await _playbackService.SearchSegmentsAsync(cameraId, from, to, ct);
            return Ok(segments);
        }

        // 2. Video Streamen (Bytes)
        // GET /api/playback/stream/{segmentId}
        [HttpGet("stream/{segmentId}")]
        public async Task<IActionResult> StreamVideo(Guid segmentId, CancellationToken ct)
        {
            // Holt den rohen Stream aus der Infrastruktur (via Module)
            var stream = await _playbackService.GetSegmentStreamAsync(segmentId, ct);

            if (stream == null)
                return NotFound("Video segment not found or file missing.");
            
            return File(stream, "video/mp4");
        }
    }
}
