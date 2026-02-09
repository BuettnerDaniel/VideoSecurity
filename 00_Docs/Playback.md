```mermaid
%%{init: {
  'theme': 'base',
  'themeVariables': {
    'background': '#ffffff',
    'primaryColor': '#ffffff',
    'primaryTextColor': '#000000',
    'primaryBorderColor': '#000000',
    'lineColor': '#000000',
    'signalColor': '#000000',
    'signalTextColor': '#000000',
    'noteBkgColor': '#fff9c4',
    'noteTextColor': '#000000'
  }
}}%%
sequenceDiagram
    participant User as ?? Frontend App
    
    box rgb(245, 245, 245) ?? 02_Modules
        participant Play as VideoSecurity.Playback
    end
    
    box rgb(235, 235, 235) ?? 04_Infrastructure
        participant DB as PostgreSQL (Metadata)
        participant Blob as IBlobStorage
    end

    Note over User, DB: Schritt 1: Suche (IPlaybackService)
    User->>Play: 1. SearchSegmentsAsync(cameraId,...)
    activate Play
    Play->>DB: Query Metadata
    DB-->>Play: return Rows
    Play-->>User: return List<RecordedSegmentDto>
    deactivate Play

    Note over User, Blob: Schritt 2: Streaming (IPlaybackService)
    User->>Play: 2. GetSegmentStreamAsync(segmentId)
    activate Play
    Play->>DB: Lookup BlobPath
    
    rect rgb(220, 255, 220)
    Note right of Play: Direct Stream Copy
    Play->>Blob: 3. OpenReadStreamAsync(path)
    Blob-->>Play: return Stream
    end
    
    Play-->>User: return FileStream
    deactivate Play