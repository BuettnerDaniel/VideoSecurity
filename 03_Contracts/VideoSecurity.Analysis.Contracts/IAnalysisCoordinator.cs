using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSecurity.Analysis.Contracts
{
    /// <summary>
    /// Wird vom Background-Worker aufgerufen, sobald ein neues Video in der Outbox gefunden wurde.
    /// idealerweise verhindern, dass Analyse mehrfach für das gleiche Segment aufgerufen wird.
    /// </summary>
    /// <param name="segmentId">Die ID des Video-Segments</param>
    /// <param name="blobPath">Der Pfad zum Video im BlobStorage (zum Nachladen)</param>
    /// <param name="ct">Cancellation Token</param>
    public interface IAnalysisCoordinator
    {
        Task ProcessNewSegmentAsync(Guid SegmentId, string blobPath, CancellationToken ct);
    }
    /// <summary>
    /// Repräsentiert das Ergebnis einer KI-Erkennung (z.B. "Person erkannt mit 95% Sicherheit").
    /// </summary>
    public record AnalysisResult(string ObjectType, double Confidence, TimeSpan OccuredAt);
}
