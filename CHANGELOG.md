# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.3](https://github.com/atc-net/atc-undoredo/compare/v1.1.2...v1.1.3) (2026-04-17)


### Bug Fixes

* **docs:** move readme in solution-explore ([3d51921](https://github.com/atc-net/atc-undoredo/commit/3d51921e62def81d92ac42ecc161ab46347f7e56))

## [1.1.2](https://github.com/atc-net/atc-undoredo/compare/v1.1.1...v1.1.2) (2026-04-17)


### Bug Fixes

* **build:** produce symbols package and silence pack warning ([486b2cb](https://github.com/atc-net/atc-undoredo/commit/486b2cb1d803fa7226ff6c9d01014dcd19d14642))

## [1.1.1](https://github.com/atc-net/atc-undoredo/compare/v1.1.0...v1.1.1) (2026-04-17)


### Bug Fixes

* **ci:** mark sample and test projects as non-packable ([8f9ad73](https://github.com/atc-net/atc-undoredo/commit/8f9ad730d73e343425b3f2c8ef7b6c7092a12f10))

## [1.1.0](https://github.com/atc-net/atc-undoredo/compare/v1.0.0...v1.1.0) (2026-04-17)


### Features

* add Blazor sample applications ([0f393d0](https://github.com/atc-net/atc-undoredo/commit/0f393d067d08e7300a331676eeb73baee50228b6))
* extract undo/redo framework from atc-wpf ([1337dc1](https://github.com/atc-net/atc-undoredo/commit/1337dc16a3c29057c4e9ec021cedf38b2325f65f))


### Bug Fixes

* add StateChanged subscription to all demo pages ([c2e1143](https://github.com/atc-net/atc-undoredo/commit/c2e1143b4618801125876f6c0e782107de4a7bd1))
* **ci:** gate NuGet push on release-please release creation ([a802d3e](https://github.com/atc-net/atc-undoredo/commit/a802d3e4bb1158741eac756536fd51cc336be20b))
* remove trailing newlines from project and manifest files ([288a922](https://github.com/atc-net/atc-undoredo/commit/288a922b8473ac7aeef9119a357dd0e184e9d3eb))

## [Unreleased]

### Added

- Initial extraction of undo/redo framework from atc-wpf
- Core interfaces: `IUndoCommand`, `IUndoRedoService`, `IRichUndoCommand`, and more
- Command implementations: `UndoCommand`, `PropertyChangeCommand`, `UndoCommandGroup`
- `UndoRedoService` with thread-safe dual-stack implementation
- `UndoRedoPropertyTracker` for automatic property change recording
- Snapshot, serialization, and audit logging support
