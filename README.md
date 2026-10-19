# Bentley Configuration Explorer

Bentley Configuration Explorer (BCE) is a Windows desktop tool for inspecting, analyzing, and comparing MicroStation-style configuration workspaces.

## Current status

This repository currently targets `.NET 10` and includes the WinForms Configuration Explorer application. The longer-term direction is to separate the parsing and analysis engine from the desktop UI so it can be reused by other applications and aligned with Python-based analysis tooling.

## Warnings

Bentley Configuration Explorer was migrated to .NET 10 and dropped usage of the insecure BinaryFormatter. Any binary configuration files (*.bcf) from old versions need to be re-created from the workspace files going forward.

JSON workspace files use an explicit allowlist of persisted model and collection types. Unknown `$type` metadata is rejected without resolving or loading the named assembly. Object references are preserved for compatibility with existing JSON `.bcf` files, and JSON nesting is limited to 128 levels. Do not open untrusted `.bcf` files with older builds that lack this restriction.

Saved file graphs are validated before activation and before File History renders them. Child relationships must form a forest with unique ownership, consistent depths, at most 10,000 files, and at most 64 child levels. Normal parent/workspace back-references remain supported. Include processing uses the same file-count/depth limits and stops active include cycles using normalized, case-insensitive paths. Files may still be included again after their earlier processing completes. Limit failures record an error and abort processing.

Reload reads product metadata from embedded string resources using a data-only PE reader. It never loads the product DLL or invokes its code, even when the workspace supplies the executable path. Non-string resources are not deserialized. Missing or invalid DLLs are skipped; product metadata implemented only by computed getters is no longer evaluated.

"Open in Editor" tries Notepad++, Visual Studio Code, then Windows Notepad. Only executable paths in their standard Program Files or per-user Programs installation directories are considered; Notepad is located in the Windows system directory. Editor lookup never uses `PATH` or the current directory. Files are passed as separate arguments to the editor, never launched through file associations or executed directly. Nonstandard or portable editor installations are not automatically discovered.

Spreadsheet XML exports encode cell values, column headers, and worksheet names as literal text/attributes. Workspace values cannot add spreadsheet markup or formula attributes.

## Build requirements

- Visual Studio with .NET 10 SDK and Windows desktop development tools

## Persistence regression checks

From a Visual Studio Developer PowerShell, build and run the dependency-free regression runner against the real application:

```powershell
msbuild tests/WorkspacePersistence/WorkspacePersistence.csproj /restore /p:Configuration=Debug /p:Platform=x64
dotnet tests/WorkspacePersistence/bin/x64/Debug/net10.0-windows/WorkspacePersistence.dll
```

An optional argument supplies a malicious `.bcf` fixture to verify rejection. Run malicious fixtures only against the patched build. The checks cover existing-format compatibility, save/load round-trips, shared and circular references, unauthorized types at all three ArrayList entry points, assembly spoofing, and excessive nesting.

Reload checks also use a temporary product DLL with a marker-writing initializer to verify that opening and reprocessing a saved workspace reads string metadata without loading or executing that DLL. Non-string resources, short versions, missing/corrupt images, and the public metadata accessors are covered as well.

Editor checks inspect launch commands without opening editor windows, including line arguments, executable-looking filenames, and lookalike editor executables in an untrusted current directory and `PATH`.

Additional checks cover child-file cycles, shared-child amplification, actual reference-graph depth, file-count limits, XML/formula injection in both spreadsheet exporters, and real parser processing of self-includes, mutual cycles, deep include chains, and legitimate repeated includes. No Excel application is launched.

## Recommended project direction

The recommended future split is:

- Core parser/model library: tokenizer, parser, evaluator, configuration/variable/file models
- Analysis library: comparison, validation, trace/state export, diagnostics, shared JSON output
- Desktop UI: WinForms user interface and desktop workflows

## Public data guidance

Do not commit private debug logs, generated `msdebug` output, real customer/user paths, local machine names, private ProjectWise samples, binary workspace exports, or vendor binaries.
