# Bentley Configuration Explorer

Bentley Configuration Explorer (BCE) is a Windows desktop tool for inspecting, analyzing, and comparing MicroStation-style configuration workspaces.

## Current status

This repository currently targets `.NET 10` and includes the WinForms Configuration Explorer application. The longer-term direction is to separate the parsing and analysis engine from the desktop UI so it can be reused by other applications and aligned with Python-based analysis tooling.

## Build requirements

- Visual Studio with .NET 10 SDK and Windows desktop development tools

## Recommended project direction

The recommended future split is:

- Core parser/model library: tokenizer, parser, evaluator, configuration/variable/file models
- Analysis library: comparison, validation, trace/state export, diagnostics, shared JSON output
- Desktop UI: WinForms user interface and desktop workflows

## Shared C# / Python output

A portable JSON workspace analysis export is being introduced so C# and Python tools can share comparable output. The shared artifact should include workspace metadata, final variables, file provenance, processing events, and diagnostics.

## Public data guidance

Do not commit private debug logs, generated `msdebug` output, real customer/user paths, local machine names, private ProjectWise samples, binary workspace exports, or vendor binaries.
