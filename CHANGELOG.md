# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- Initial extraction of undo/redo framework from atc-wpf
- Core interfaces: `IUndoCommand`, `IUndoRedoService`, `IRichUndoCommand`, and more
- Command implementations: `UndoCommand`, `PropertyChangeCommand`, `UndoCommandGroup`
- `UndoRedoService` with thread-safe dual-stack implementation
- `UndoRedoPropertyTracker` for automatic property change recording
- Snapshot, serialization, and audit logging support
