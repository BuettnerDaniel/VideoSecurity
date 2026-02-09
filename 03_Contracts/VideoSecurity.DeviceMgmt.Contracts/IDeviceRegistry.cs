using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSecurity.DeviceMgmt.Contracts
{
    public interface IDeviceRegistry
    {
        /// <summary>
        /// Registriert Kameras via DTO
        /// TODO: Autodiscovery?
        /// </summary>        
        Task<Guid> RegisterCameraAsync(CameraConfigDto config);
        /// <summary>
        /// Ändert die Konfiguation einer Kamera
        /// </summary>                
        Task UpdateConfigurationAsync(Guid id, CameraConfigDto config);
        /// <summary>
        /// Liefert Konfigurationsdaten einer Kamera.
        /// </summary>
        /// <param name="id">Die Kamera ID</param>
        /// <returns>Das DTO oder null, wenn keine Kamera mit dieser Id vorhanden</returns>
        Task<CameraDto?> GetCameraAsync(Guid id);
        /// <summary>
        /// Aktualisiert den Online/Offline Status (z.B. durch Health-Checks).
        /// </summary>
        Task UpdateStatusAsync(Guid id, bool isOnline);
    }
    /// <summary>
    /// Kamerainformationen
    /// </summary>    
    public record CameraDto(Guid Id, string Name, string IPAddress, bool IsOnline);
    public record CameraConfigDto(
        string Name,
        string IpAddress,
        int Port
        );
    

}

