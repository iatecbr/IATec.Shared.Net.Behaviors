# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [1.3.0] – 2026-05-21

### ADDED
- XML documentation comments in English for all public classes and methods.
- Expanded `README.md` with usage examples, installation instructions, and dependency overview.

### CHANGED
- Updated package references to newer versions for this release.
  - Bumped `IATec.Shared.Domain` to `2.0.0`.
  - Bumped `Microsoft.Extensions.Localization.Abstractions` to `10.0.8`.

---

## [1.2.0] – 2026-01-12

### CHANGED
- Added support for .NET 10 alongside existing .NET 8 and .NET 9 target frameworks.
- Updated package dependencies to their latest compatible versions.

---

## [1.1.0] – 2025-08-28

### FIXED
- Removed unnecessary `AspNetCore.Mvc.Core` library reference to reduce dependency footprint.

---

## [1.0.4] – 2025-08-14

### FIXED
- Removed explicit `FluentValidation` package reference; project now relies on transitive dependencies only.

---

## [1.0.3] – 2025-05-08

### ADDED
- Added `ExceptionPipelineBehavior` to centralize exception handling in MediatR pipelines.
- Introduced strongly-typed `Messages` resource for localized error strings (en, es, pt-BR).

### CHANGED
- Updated exception pipeline to support responses with any result type.

---

## [1.0.2] – 2025-04-24

### ADDED
- Introduced `ValidatorPipelineBehavior` for FluentValidation-based request validation.

---

## [1.0.1] – 2024-09-23

### FIXED
- Fixed NuGet packaging issues and corrected project versioning metadata.

---

## [1.0.0] – 2024-04-29

### ADDED
- Initial project setup with multi-targeting for .NET 8.
- Base solution structure and CI/CD pipeline configuration.
