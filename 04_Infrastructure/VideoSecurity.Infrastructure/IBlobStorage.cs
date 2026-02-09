using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSecurity.Infrastructure
{
    public interface IBlobStorage
    {
        /// <summary>
        /// Streamt eingehende Videodaten performant in den Blob-Storage (Write-Only).
        /// </summary>
        /// <param name="source">Der eingehende Videostream (z.B. von RTSP)</param>
        /// <param name="fileName">Der gewünschte Dateiname/Pfad im Blob</param>
        /// <param name="ct">Cancellation Token für sauberes Beenden</param>
        /// <returns>Die URI oder der Pfad zum gespeicherten Blob</returns>
        Task<string> SaveStreamAsync(Stream source, string fileName, CancellationToken ct);
        /// <summary>
        /// Öffnet einen Lesestream für ein gespeichertes Video (Read-Only).
        /// Wird vom Analysis-Modul oder Playback-Modul verwendet.
        /// </summary>
        /// <param name="blobPath">Der Pfad, der beim Speichern zurückgegeben wurde</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>Ein stream-fähiges Objekt des Videos</returns>
        Task<Stream> OpenReadStreamAsync(string blobPath, CancellationToken ct);
    }
}
