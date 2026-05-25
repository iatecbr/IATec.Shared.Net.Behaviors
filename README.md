# IATec.Shared.Net.Behaviors

Reusable MediatR pipeline behaviors for IATec .NET projects.

## Overview

This library provides cross-cutting pipeline behaviors that integrate with [MediatR](https://github.com/jbogard/MediatR) to handle common concerns such as request validation and exception handling. It is designed to be consumed by multiple internal IATec services, promoting consistency and reducing boilerplate across the organization.

## Features

- **ValidatorPipelineBehavior** – Automatically validates incoming requests using [FluentValidation](https://docs.fluentvalidation.net/) before they reach the request handler. Validation failures are collected and returned as a `FluentResults` response.
- **ExceptionPipelineBehavior** – Catches unhandled exceptions during request processing, logs the error details, and returns a standardized localized error response.

## Installation

The package is distributed through the IATec internal NuGet feed:

```bash
dotnet add package IATec.Shared.Behaviors
```

## Getting Started

Register the behaviors in your `Program.cs` or `Startup.cs` alongside MediatR:

```csharp
using IATec.Shared.Behaviors;

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<YourEntryType>();
});

// Register FluentValidation validators (required for ValidatorPipelineBehavior)
services.AddValidatorsFromAssemblyContaining<YourEntryType>();

// Register pipeline behaviors
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorPipelineBehavior<,>));
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionPipelineBehavior<,>));
```

> Ensure `ExceptionPipelineBehavior` is registered **after** `ValidatorPipelineBehavior` (or as the outermost behavior) so that validation errors are returned before any generic exception wrapping occurs.

## Requirements

- .NET 8, .NET 9, or .NET 10

## Dependencies

- [FluentResults](https://www.nuget.org/packages/FluentResults/) (≥ 4.0.0)
- [MediatR](https://www.nuget.org/packages/MediatR/) (≥ 14.0.0)
- [Microsoft.Extensions.Localization.Abstractions](https://www.nuget.org/packages/Microsoft.Extensions.Localization.Abstractions/) (≥ 10.0.8)
- [IATec.Shared.Domain](https://github.com/iatecbr/IATec.Shared.Net.Domain) (≥ 2.0.0)

## Contributing

This is an internal IATec library. For bug reports or feature requests, please open an issue in the company's issue tracker.

## See Also

- [CHANGELOG.md](CHANGELOG.md)

## License

© IATec Solutions – All rights reserved.
