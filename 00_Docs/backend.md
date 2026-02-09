# Backend Architektur & Design Entscheidungen

Dieses Dokument beschreibt die Architektur, die Datenflüsse und die zugrunde liegenden Design-Entscheidungen.

---
## Fragen aus der Aufgabenstellung
### Wie schneidest du das Projekt – was entwickelst du zuerst?
Die Lösung ist in vertikale **Module** (Fachlichkeiten) und horizontale **Schichten** (Technik) geschnitten. ( siehe [ADR-002](#ADR-002))
#### Voschlag für Projektphasen
Wir entwickeln Top Down in vier Phasen:

1.  **Recording:**
    Daten sind das Herzstück des Systems. Ohne Daten kein System.

2.  **Playback:**
    Visualisierung der Daten.

3.  **Analysis:**
    Hinzufügen der KI-Logik.
4.  **DeviceMgmt:**
    Funktionen für Administratoren.

### Welches Projekt referenziert welches? Wie verhinderst du enge Kopplung?
Api referenziert alle Projekte.
Module referenzieren nur Contracts und Infrastructure. Kein Modul kennt ein anderes Modul. (siehe [ADR-001](#ADR-001), [Architekturüberblick](architectureoverview.md))
### Wie interagieren die Module? (z.B. Wie erfährt Intelligence, dass Recording ein neues Video hat?)
Module interagieren über definierte Schnittstellen und entsprechende Events. Intelligence/Analysis erfährt über einen Background worker, der die Outbox Tabelle verwendet über neue Videos. (siehe [ADR-004](#ADR-004),[ADR-005](#ADR-005), [sequenceDiagram](sequenceDiagram.md))
### Wo und wie werden die massiven Videodaten (Blobs) vs. Metadaten gespeichert?
Videodateien werden in Blobstorage gespeichert. Metadaten in relationaler Datenbank. (siehe [ADR-003](#ADR-003), [sequenceDiagram](sequenceDiagram.md))

## 1. Architecture Decision Records (ADR)


### ADR-001: Modular Monolith vs Microservices
* **Kontext:** Die Systemarchitektur sollte verständlich und leicht wartbar sein. Ein Wechsel zu Microservices soll in der Zukunft möglich sein.
* **Entscheidung:** Wir entwickeln einen **Modularen Monolithen** als Prototypen mit klar definierten Schnittstellen.
* **Trade-offs:**
    *  **Pro:** Single Deployment Unit (`VideoSecurity.Api`), In-Memory Kommunikation (High Performance), einfaches Debugging.
    *  **Contra:** Skalierung erfolgt für die gesamte App, nicht granular pro Modul. 

### ADR-002: Domain Driven Design
* **Kontext:** Kombination aus vertical Slicing und Schichten.
* **Entscheidung:** **Vertikaler Schnitt nach Fachlichkeit (DDD)**. Jedes Modul (`Recording`, `Analysis`, `Playback`) ist ein separates Projekt. Die Infrastruktur wird über Interfaces/Contracts geteilt.
* **Trade-offs:**
    * **Pro:** Änderungen betreffen meist nur ein Projekt. Parallele Teams können autark arbeiten.
    *  **Contra:** Hohe Diszipilin im Infrastructure Projekt notwendig, damit diese keine Business Logic enthält.

### ADR-003: Hybrid Storage Strategy
* **Kontext:** Videodaten sind groß, Metadaten sind klein und strukturiert. Relationale Datenbanken skalieren schlecht mit BLOBs.
* **Entscheidung:** **Trennung von Payload und Metadaten**.
    * **Blob Storage (File System/Azure Blobstorage/S3):** Speichert rohe Video-Dateien.
    * **relationale Datenbank:** Speichert Pfade und Meta-Daten.
* **Trade-offs:**
    * **Pro:** Datenbank bleibt performant und Suche ist strukturiert parametrisierbar. Separierter Zugriff auf Video-Streams.
    * **Contra:** Risiko von verwaisten Dateien, wenn DB-Commit fehlschlägt (Lösung: Outbox).

### ADR-004: Transactional Outbox Pattern
* **Kontext:** Wir müssen ein Video speichern und ein Event senden damit keine inkonsistenten Daten entstehen.
* **Entscheidung:** Nutzung einer **Outbox-Tabelle**. Das Event wird in derselben DB-Transaktion gespeichert wie die Metadaten. Ein Background-Worker arbeitet die Events asynchron ab.
* **Alternatives:** Interne Eventstruktur(MediatR, noch nicht benutzt) oder Message Queue (aws sqs, azure service bus)
* **Trade-offs:**
    * **Pro:** Garantiert **At-Least-Once Delivery**. Datenintegrität gesichert.
    * **Contra:** Leichte Latenz bis zur Event-Verarbeitung (Polling).

### ADR-005: Interface-Based Contracts
* **Kontext:** Module müssen kommunizieren, dürfen sich aber nicht direkt referenzieren (Zirkelbezug).
* **Entscheidung:** Extraktion von **`*.Contracts` Bibliotheken**. Module kennen nur Interfaces, keine Implementierungen.
* **Trade-offs:**
    * ✅ **Pro:** Strikte Entkopplung, saubere Dependency Injection.
    * ⚠️ **Contra:** Mehr Projekte im Solution Explorer.

---

