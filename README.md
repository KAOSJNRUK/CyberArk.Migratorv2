# CyberArk Migrator â€” v2 (Windows-only)

New in this build:
- Select existing CSV (vendor export) â†’ Encrypt â†’ Import
- Multi-threaded import with configurable Batch Size and Max Degree of Parallelism
- AES-GCM + DPAPI-wrapped DEK; no plaintext secrets on disk

Run:
1) Open CyberArk.Migrator.sln in Visual Studio 2022.
2) Restore NuGet if prompted.
3) Set **CyberArk.Migrator.App** as Startup Project.
4) Press **F5**.

Flow:
1) Create sample CSV or Select existing CSV...
2) Encrypt selected CSV (creates <yourfile>.csv.enc)
3) Set Batch Size and Max Degree of Parallelism
4) Run Import (P-Cloud) or Run Import (Self-Hosted)

Targets are stubs; replace methods in src/Targets/Targets.cs with real REST calls.
