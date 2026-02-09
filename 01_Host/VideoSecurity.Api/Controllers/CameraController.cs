using Microsoft.AspNetCore.Mvc;
using VideoSecurity.DeviceMgmt.Contracts;

namespace VideoSecurity.Api.Controllers{

[ApiController]
    [Route("api/[controller]")]
    public class CamerasController : ControllerBase
    {
        private readonly IDeviceRegistry _deviceRegistry;
        public CamerasController(IDeviceRegistry deviceRegistry)
        {
            _deviceRegistry = deviceRegistry;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CameraConfigDto request)
        {
            // Hier würde man in einer echten App Validierung machen (IP Format, etc.)
            var newId = await _deviceRegistry.RegisterCameraAsync(request);

            // 201 Created + Location Header
            return CreatedAtAction(nameof(GetCamera), new { id = newId }, new { id = newId });
        }
        [HttpPut("{id}/config")]
        public async Task<IActionResult> UpdateConfiguration(Guid id, [FromBody] CameraConfigDto config)
        {
            try
            {
                await _deviceRegistry.UpdateConfigurationAsync(id, config);
                               
                return NoContent();
            }
            catch (KeyNotFoundException)
            {                
                return NotFound($"Camera with ID {id} not found.");
            }
            catch (Exception ex)
            {
             
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCamera(Guid id)
        {
            var camera = await _deviceRegistry.GetCameraAsync(id);
            if (camera == null) return NotFound();
            return Ok(camera);
        }

        [HttpPost("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] bool isOnline)
        {
            await _deviceRegistry.UpdateStatusAsync(id, isOnline);
            return NoContent();
        }
    }
}
