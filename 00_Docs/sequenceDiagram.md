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
    participant Cam as Kamera
    
    box rgb(245, 245, 245) 02_Modules
        participant Rec as VideoSecurity.Recording
        participant Ana as VideoSecurity.Analysis
    end
    
    box rgb(235, 235, 235) 04_Infrastructure
        participant Blob as IBlobStorage
        participant Outbox as IOutboxService
    end

    Note over Cam, Blob: Phase 1: Recording Flow
    Cam->>Rec: 1. VideoStream 
    activate Rec
    
    Note right of Rec: Via IStreamIngestor
    Rec->>Blob: 2. SaveStreamAsync(Stream, ...)
    Blob-->>Rec: return BlobPath
    
    rect rgb(220, 255, 220)
    Note right of Rec: Transactional Outbox
    Rec->>Outbox: 3. PublishAsync<VideoSegmentCreatedEvent>(...)
    Note right of Outbox: Commit to DB
    end
    
    Rec-->>Cam: Ack
    deactivate Rec

    Note over Outbox, Ana: Phase 2: Analysis Flow
    loop Background Service
        Outbox->>Ana: 4. Dispatch Event (VideoSegmentCreatedEvent)
        activate Ana
        
        Note right of Ana: Via IAnalysisCoordinator
        Ana->>Blob: 5. OpenReadStreamAsync(BlobPath)
        Blob-->>Ana: return Stream
        
        Ana->>Ana: AI Inference Logic
        
        Ana->>Outbox: 6. Mark as Processed (Idempotent)
        deactivate Ana
    end