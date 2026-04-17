# Atc.UndoRedo

A production-grade, platform-agnostic undo/redo framework for .NET.

[![NuGet Version](https://img.shields.io/nuget/v/Atc.UndoRedo.svg)](https://www.nuget.org/packages/Atc.UndoRedo)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Atc.UndoRedo.svg)](https://www.nuget.org/packages/Atc.UndoRedo)
[![Build Status](https://github.com/atc-net/atc-undoredo/actions/workflows/build.yml/badge.svg)](https://github.com/atc-net/atc-undoredo/actions)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4.svg)](https://dotnet.microsoft.com/)

## 📖 Table of contents

- [✨ Features](#-features)
- [📦 Installation](#-installation)
- [🚀 Quick start](#-quick-start)
- [🧩 Core concepts](#-core-concepts)
- [🛠️ Usage examples](#️-usage-examples)
- [🧪 Samples](#-samples)
- [✅ Testing](#-testing)
- [🗂️ Project structure](#️-project-structure)
- [📝 Changelog](#-changelog)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)

## ✨ Features

Core undo/redo:

- Dual-stack undo/redo with configurable history limit
- Thread-safe via `ReaderWriterLockSlim`
- Pure .NET with zero platform dependencies

Command composition:

- Command grouping for atomic multi-command transactions
- Command merging to coalesce rapid changes (slider drags, typing)
- Property tracking via delegate-based `PropertyChangeCommand<T>`
- Rich commands carrying metadata, timestamps, and contexts

Advanced history control:

- Named snapshots for save points in history
- History serialization to persist and restore undo/redo state
- Audit logging with timestamps for all operations
- Non-linear history with branching redo paths
- Operation approval to veto undo/redo operations
- Memory-aware trimming with a configurable memory budget

## 📦 Installation

Install the NuGet package:

```bash
dotnet add package Atc.UndoRedo
```

Or add it manually to your `.csproj`:

```xml
<PackageReference Include="Atc.UndoRedo" Version="1.0.0" />
```

Target framework: `net10.0`.

## 🚀 Quick start

```csharp
using Atc.UndoRedo.Commands;
using Atc.UndoRedo.Services;

var list = new List<string>();
var service = new UndoRedoService();

service.Execute(new UndoCommand(
    description: "Add item",
    execute: () => list.Add("Hello"),
    unExecute: () => list.Remove("Hello")));

service.Undo(); // list is empty
service.Redo(); // list contains "Hello"
```

## 🧩 Core concepts

### `IUndoCommand` / `UndoCommand`

The fundamental command contract (`Description`, `Execute()`, `UnExecute()`). `UndoCommand` is a delegate-based implementation for the common case. See `src/Atc.UndoRedo/Interfaces/IUndoCommand.cs` and `src/Atc.UndoRedo/Commands/UndoCommand.cs`.

### `PropertyChangeCommand<T>`

A pre-built command for reverting a single property change from `oldValue` to `newValue`. See `src/Atc.UndoRedo/Commands/PropertyChangeCommand.cs`.

### `UndoCommandGroup`

Wraps a list of commands into a single atomic undo unit. Forward order on execute, reverse order on un-execute. See `src/Atc.UndoRedo/Commands/UndoCommandGroup.cs`.

### `IMergeableUndoCommand`

Extends `IUndoCommand` with a `MergeId` and `TryMergeWith(...)` so consecutive compatible commands (typing, slider drags) collapse into one undo step. See `src/Atc.UndoRedo/Interfaces/IMergeableUndoCommand.cs`.

### `IRichUndoCommand` / `RichUndoCommand`

Commands that carry extra metadata (id, timestamp, parameter, data, image, user-action flag, contexts). See `src/Atc.UndoRedo/Commands/RichUndoCommand.cs`.

### `UndoRedoService`

The orchestrator. Exposes `Execute`, `Undo`, `Redo`, `Clear`, `BeginGroup`, `SuspendRecording`, snapshots, serialization, approvers, events, and `CanUndo`/`CanRedo` state. See `src/Atc.UndoRedo/Services/UndoRedoService.cs`.

## 🛠️ Usage examples

### Command grouping

`BeginGroup` returns an `IDisposable` scope. Any command executed inside the scope is collapsed into a single undo step when the scope disposes.

```csharp
using (service.BeginGroup("Rename and move"))
{
    service.Execute(renameCommand);
    service.Execute(moveCommand);
}

service.Undo(); // reverts both in one step
```

### Property tracking

```csharp
var person = new Person { Name = "Ada" };
var oldName = person.Name;
var newName = "Grace";

service.Execute(new PropertyChangeCommand<string>(
    description: "Rename person",
    setter: v => person.Name = v,
    oldValue: oldName,
    newValue: newName));
```

### Command merging

Implement `IMergeableUndoCommand`; successive commands with the same `MergeId` can fold into one.

```csharp
public sealed class SliderChangeCommand : IMergeableUndoCommand
{
    public string Description => "Slider change";
    public int MergeId => 42;
    public bool IsObsolete => false;

    public void Execute() { /* apply newValue */ }
    public void UnExecute() { /* restore oldValue */ }

    public bool TryMergeWith(IUndoCommand other)
        => other is SliderChangeCommand next && /* update newValue from next */ true;
}
```

### Named snapshots

```csharp
var snapshot = service.CreateSnapshot("Before refactor");
// ... user performs several actions ...
service.RestoreSnapshot(snapshot); // jumps history back to the snapshot
```

### History serialization

Commands that should survive a save must implement `ISerializableUndoCommand`. Provide an `IUndoCommandDeserializer` on load.

```csharp
using var fs = File.Create("history.bin");
service.SaveHistory(fs);

using var fsIn = File.OpenRead("history.bin");
service.LoadHistory(fsIn, myDeserializer);
```

### Audit logging

```csharp
var logger = new UndoRedoAuditLogger();
var service = new UndoRedoService { AuditLogger = logger };

service.Execute(command);
foreach (var entry in logger.Entries)
{
    Console.WriteLine($"{entry.Timestamp:O} {entry.ActionType} {entry.Description}");
}
```

### Operation approval

```csharp
public sealed class ConfirmDestructive : IUndoOperationApprover
{
    public bool ApproveUndo(IUndoCommand command) => Prompt(command);
    public bool ApproveRedo(IUndoCommand command) => Prompt(command);

    private static bool Prompt(IUndoCommand c) => /* show dialog */ true;
}

service.RegisterApprover(new ConfirmDestructive());
```

### Memory budget

`MaxHistorySize` caps command count; `MaxHistoryMemory` (bytes) caps estimated memory. Commands that implement `IMemoryAwareUndoCommand` contribute their `EstimatedMemoryBytes` to the budget.

```csharp
var service = new UndoRedoService
{
    MaxHistorySize = 500,
    MaxHistoryMemory = 64 * 1024 * 1024, // 64 MiB
};
```

## 🧪 Samples

Three sample projects live under `sample/`:

- `Atc.UndoRedo.Sample.BlazorServer` — Blazor Server host. Run with `dotnet run --project sample/Atc.UndoRedo.Sample.BlazorServer` and open [https://localhost:18774](https://localhost:18774).
- `Atc.UndoRedo.Sample.BlazorWasm` — Blazor WebAssembly host. Run with `dotnet run --project sample/Atc.UndoRedo.Sample.BlazorWasm` and open [https://localhost:18775](https://localhost:18775).
- `Atc.UndoRedo.Sample.SharedDemo` — Razor class library shared by both hosts; contains the demo pages.

The demo pages cover:

- `BasicUndoRedo` — core `Execute`/`Undo`/`Redo` over a list.
- `CommandGrouping` — multi-command atomic transactions.
- `MergeableCommands` — coalescing rapid changes into one undo step.
- `PropertyTracking` — property-level change recording.
- `HistoryViewer` — inspecting the undo and redo stacks live.

## ✅ Testing

Tests use **xUnit v3** and live under `test/Atc.UndoRedo.Tests`. Run the full suite from the repository root:

```bash
dotnet test
```

## 🗂️ Project structure

```text
atc-undoredo/
├── src/
│   └── Atc.UndoRedo/              # The library
├── sample/
│   ├── Atc.UndoRedo.Sample.BlazorServer/
│   ├── Atc.UndoRedo.Sample.BlazorWasm/
│   └── Atc.UndoRedo.Sample.SharedDemo/
├── test/
│   └── Atc.UndoRedo.Tests/        # xUnit v3 test suite
├── Atc.UndoRedo.slnx
├── Directory.Build.props          # Shared build config, analyzers, versioning
├── CHANGELOG.md
└── README.md
```

## 📝 Changelog

See [`CHANGELOG.md`](CHANGELOG.md). The project follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and [Semantic Versioning](https://semver.org/), with releases driven by [release-please](https://github.com/googleapis/release-please).

## 🤝 Contributing

Issues and pull requests are welcome. Please open a [GitHub issue](https://github.com/atc-net/atc-undoredo/issues) to discuss larger changes before submitting a PR.

## 📄 License

Licensed under the [MIT License](LICENSE).
