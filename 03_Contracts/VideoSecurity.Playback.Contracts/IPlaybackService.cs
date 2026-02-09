using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSecurity.Playback.Contracts
{
    public interface IPlaybackService
    {
        /// <summary>
        /// Durchsucht das Archiv nach verfügbaren Video-Segmenten.
        /// Wird vom Frontend genutzt
        /// </summary>
        /// <param name="cameraId">Die Kamera</param>
        /// <param name="from">Startzeitraum der Suche</param>
        /// <param name="to">Endzeitraum der Suche</param>
        /// <returns>Liste der verfügbaren Segmente</returns>
        Task<IEnumerable<RecordedSegmentDto>> SearchSegmentsAsync(Guid cameraId, DateTime from, DateTime to, CancellationToken ct);


        //TODO: Security hinsichtlich Zugriffsberechtigung bedenken, Rollenkonzept für User, Admins etc
        /// <summary>
        /// Lädt den tatsächlichen Videostream für ein Segment.
        /// Das Playback-Modul holt sich hierfür den Pfad aus der DB und streamt via Infrastructure vom Blob.
        /// </summary>
        /// <param name="segmentId">Die ID des Segments (aus der Search-Methode)</param>
        /// <returns>Der rohe Videostream (z.B. MP4)</returns>
        Task<Stream?> GetSegmentStreamAsync(Guid segmentId, CancellationToken ct);
    }
    /// <summary>
    /// Repräsentiert einen Zeitabschnitt, für den eine Videoaufnahme existiert.
    /// </summary>
    public record RecordedSegmentDto(
        Guid Id,
        Guid CameraId,
        DateTime StartTime,
        TimeSpan Duration,
        long SizeInBytes
    );
}
