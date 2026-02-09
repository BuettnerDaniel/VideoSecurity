```mermaid
%%{init: {'theme': 'base', 'themeVariables': { 'background': '#ffffff', 'primaryColor': '#ffffff', 'primaryTextColor': '#000000', 'primaryBorderColor': '#000000', 'lineColor': '#000000'}}}%%
graph TD
    %% --- Styling für Solution-Struktur ---
    classDef container fill:#ffffff,stroke:#000000,stroke-width:2px,color:#000000;
    classDef folder fill:#f5f5f5,stroke:#333333,stroke-width:1px,stroke-dasharray: 5 5;
    
    %% --- Component Styling ---
    classDef host fill:#e3f2fd,stroke:#1565c0,stroke-width:2px,color:#000000,rx:5,ry:5;
    classDef contract fill:#f3e5f5,stroke:#7b1fa2,stroke-width:2px,color:#000000,rx:5,ry:5;
    classDef module fill:#fff8e1,stroke:#fbc02d,stroke-width:2px,color:#000000,rx:5,ry:5;
    classDef infra fill:#e8f5e9,stroke:#2e7d32,stroke-width:2px,color:#000000,rx:5,ry:5;
    classDef db fill:#eceff1,stroke:#37474f,stroke-width:2px,color:#000000,rx:5,ry:5;

    subgraph Wrapper ["VideoSecurity.sln"]
        direction TB

        %% --- 01_Host ---
        subgraph HostFolder ["01_Host"]
            API["Project: VideoSecurity.Api<br/>(Startup & Controllers)"]:::host
        end

        %% --- 03_Contracts ---
        subgraph ContractsFolder ["03_Contracts"]
            IC_Rec["VideoSecurity.Recording.Contracts"]:::contract
            IC_Ana["VideoSecurity.Analysis.Contracts"]:::contract
            IC_Play["VideoSecurity.Playback.Contracts"]:::contract
            IC_Dev["VideoSecurity.DeviceMgmt.Contracts"]:::contract
        end

        %% --- 02_Modules ---
        subgraph ModulesFolder ["02_Modules"]
            direction LR
            DM["Project:<br/>VideoSecurity.DeviceMgmt"]:::module
            REC["Project:<br/>VideoSecurity.Recording"]:::module
            ANA["Project:<br/>VideoSecurity.Analysis"]:::module
            PLAY["Project:<br/>VideoSecurity.Playback"]:::module
        end

        %% --- 04_Infrastructure ---
        subgraph InfraFolder ["04_Infrastructure"]
            INF["Project: VideoSecurity.Infrastructure<br/>(IBlobStorage, IOutboxService)"]:::infra
        end

        %% --- External Systems ---
        DB[("PostgreSQL")]:::db
        BLOB[("Blob Storage")]:::db
    end

    %% Apply Container Styles
    class Wrapper container
    class HostFolder,ContractsFolder,ModulesFolder,InfraFolder folder

    %% --- Abhängigkeiten (Pfeile zeigen Referenzen) ---
    %% API referenziert Module
    API ====> DM & REC & ANA & PLAY
    
    %% Module implementieren/nutzen Contracts
    REC -.-> IC_Rec
    ANA -.-> IC_Ana
    PLAY -.-> IC_Play
    DM -.-> IC_Dev
    
    %% Module nutzen Infrastructure
    DM & REC & ANA & PLAY ====> INF
    
    %% Infrastructure nutzt Datenbanken
    INF ====> DB & BLOB

    linkStyle default stroke-width:2px,fill:none,stroke:black;