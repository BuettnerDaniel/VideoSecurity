namespace VideoSecurity.Api.Controllers
{
    using global::VideoSecurity.Recording.Contracts;
    using Microsoft.AspNetCore.Mvc;  

    [ApiController]
    [Route("api/[controller]")]
    public class RecordingController : ControllerBase
    {
        private readonly IStreamIngestor _ingestor;

        public RecordingController(IStreamIngestor ingestor)
        {
            _ingestor = ingestor;
        }

        [HttpPost("{cameraId}/start")]
        public async Task<IActionResult> StartRecording(Guid cameraId, CancellationToken ct)
        {       

            await _ingestor.StartRecordingAsync(cameraId, ct);
            return Accepted(new { Message = "Recording started in background" });
        }

        [HttpPost("{cameraId}/stop")]
        public async Task<IActionResult> StopRecording(Guid cameraId)
        {
            await _ingestor.StopRecordingAsync(cameraId);
            return Ok(new { Message = "Recording stopped" });
        }
    }
}
