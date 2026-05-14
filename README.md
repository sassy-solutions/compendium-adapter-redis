# `compendium-adapter-redis`

Redis adapter for the [Compendium](https://github.com/sassy-solutions/compendium) event-sourcing framework. Distributed idempotency store + projection checkpoint store backed by [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis).

Extracted from `sassy-solutions/compendium` per [ADR-0006](https://github.com/sassy-solutions/compendium/blob/main/docs/adr/0006-multi-repo-adapter-split.md) (multi-repo adapter split). Built from [`template-compendium-adapter-dotnet`](https://github.com/sassy-solutions/template-compendium-adapter-dotnet).

## What's in this package

| Component | Implements | Purpose |
|---|---|---|
| `RedisIdempotencyStore` | `IIdempotencyStore` | TTL-bound key/value with `SET NX` semantics for at-most-once command processing |
| `RedisProjectionCheckpointStore` | `IProjectionCheckpointStore` | Distributed `(projection, aggregate) → position` checkpoint |

## Install

```bash
dotnet add package Compendium.Adapters.Redis
```

```csharp
services.AddRedisIdempotency(builder.Configuration.GetSection("Redis"));
```

See [`docs/README.md`](docs/README.md) for full configuration (connection string, key prefix, TTL defaults).

## Versioning

This package continues the version sequence of `Compendium.Adapters.Redis` originally published from the framework monorepo (last framework-published version: `1.0.0-preview.8`). The first release from this repo is `v1.0.0-preview.9`. Versions are driven by git tags via [MinVer](https://github.com/adamralph/minver) — see [`docs/RELEASE.md`](docs/RELEASE.md).

## Repository conventions

| Aspect | Choice |
|---|---|
| Target | .NET 9, C# 13 |
| Driver | [StackExchange.Redis 2.9.x](https://www.nuget.org/packages/StackExchange.Redis) |
| Test framework | xUnit 2.9.3 + FluentAssertions 6.12.1 + NSubstitute 5.1.0 |
| Integration tests | [Testcontainers](https://dotnet.testcontainers.org) 4.11.0 (Docker required) |
| Result pattern | `Result<T>` from `Compendium.Core` |

## Build & test locally

```bash
# Unit tests — no Docker.
dotnet test --filter "FullyQualifiedName!~IntegrationTests"

# Integration tests — Docker spins up Redis via TestContainers.
dotnet test --filter "FullyQualifiedName~IntegrationTests"
```

Integration tests cover Redis-specific semantics: TTL expiration timing, `SET NX` race-condition behaviour, key-prefix isolation, connection-pool concurrency.

## License

[MIT](LICENSE) — Copyright © 2026 Sassy Solutions.
