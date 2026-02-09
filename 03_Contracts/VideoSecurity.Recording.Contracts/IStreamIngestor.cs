using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSecurity.Recording.Contracts
{
    public interface IStreamIngestor
    {
        /// <summary>
        /// Startet die asynchrone Aufnahmepipeline für eine spezifische Kamera.
        /// Verbindet sich zum RTSP-Stream und schreibt in den BlobStorage.
        /// </summary>
        /// <param name="cameraId">Die ID der Kamera</param>
        /// <param name="ct">Token zum Abbrechen der Aufnahme</param>
        Task StartRecordingAsync(Guid cameraId, CancellationToken ct);
        /// <summary>
        /// Stoppt eine laufende Aufnahme.
        /// </summary>
        Task StopRecordingAsync(Guid cameraId);
    }
    /// <summary>
    /// Das zentrale Integrations-Event.
    /// Wird gefeuert, sobald ein Video-Segment sicher im BlobStorage liegt.
    /// </summary>
    public record VideoSegmentCreatedEvent(
        Guid SegmentId,
        Guid CameraId,
        string BlobPath,
        DateTime Timestamp
        );
}
