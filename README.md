# Atc.UndoRedo

A production-grade, platform-agnostic undo/redo framework for .NET.

[![NuGet Version](https://img.shields.io/nuget/v/Atc.UndoRedo.svg)](https://www.nuget.org/packages/Atc.UndoRedo)
[![Build Status](https://github.com/atc-net/atc-undoredo/actions/workflows/ci.yml/badge.svg)](https://github.com/atc-net/atc-undoredo/actions)

## Features

- **Dual-stack undo/redo** with configurable history limit
- **Command grouping** for atomic multi-command transactions
- **Property tracking** via `INotifyPropertyChanged` for automatic change recording
- **Command merging** to coalesce rapid changes (slider drags, typing)
- **Memory-aware trimming** with configurable memory budget
- **Named snapshots** for save points in history
- **History serialization** to persist and restore undo/redo state
- **Audit logging** with timestamps for all operations
- **Non-linear history** with branching redo paths
- **Operation approval** to veto undo/redo operations
- **Thread-safe** via `ReaderWriterLockSlim`
- **Pure .NET** with zero platform dependencies

## Installation

```bash
dotnet add package Atc.UndoRedo
```

## Quick Start

```csharp
using Atc.UndoRedo;

var service = new UndoRedoService();

// Execute a command
service.Execute(new UndoCommand(
    "Add item",
    execute: () => list.Add("Hello"),
    unExecute: () => list.Remove("Hello")));

// Undo it
service.Undo();

// Redo it
service.Redo();
```

## License

MIT
