using CyberArk.Migrator.Core.Models;

namespace CyberArk.Migrator.Core.Abstractions;

public interface ISourceProvider
{
    string Name { get; }
    Task AuthenticateAsync(CancellationToken ct);
    IAsyncEnumerable<Safe> ExportSafesAsync(CancellationToken ct);
    IAsyncEnumerable<Platform> ExportPlatformsAsync(CancellationToken ct);
    IAsyncEnumerable<Account> ExportAccountsAsync(CancellationToken ct);
}

public interface ITargetProvider
{
    string Name { get; }
    Task AuthenticateAsync(CancellationToken ct);
    Task EnsureSafesAsync(IEnumerable<Safe> safes, CancellationToken ct);
    Task EnsurePlatformsAsync(IEnumerable<Platform> platforms, CancellationToken ct);
    Task ImportAccountsAsync(IEnumerable<Account> accounts, CancellationToken ct);
}
