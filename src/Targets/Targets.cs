using System.Linq;
using CyberArk.Migrator.Core.Abstractions;
using CyberArk.Migrator.Core.Models;
using CyberArk.Migrator.Infra;

namespace CyberArk.Migrator.Targets;

public class PCloudTarget : ITargetProvider
{
    public string Name => "PCloud";

    public Task AuthenticateAsync(CancellationToken ct)
    {
        Log.Info("Authenticated to P-Cloud (stub).");
        return Task.CompletedTask;
    }

    public Task EnsureSafesAsync(IEnumerable<Safe> safes, CancellationToken ct)
    {
        foreach (var s in safes.DistinctBy(s => s.Name))
            Log.Info($"Ensure Safe: {s.Name}");
        return Task.CompletedTask;
    }

    public Task EnsurePlatformsAsync(IEnumerable<Platform> platforms, CancellationToken ct)
    {
        foreach (var p in platforms.DistinctBy(p => p.Name))
            Log.Info($"Ensure Platform: {p.Name}");
        return Task.CompletedTask;
    }

    public async Task ImportAccountsAsync(IEnumerable<Account> accounts, CancellationToken ct)
    {
        await Task.Delay(50, ct); // simulate network
        foreach (var a in accounts)
            Log.Info($"Import Account: {a.Safe}/{a.Username}@{a.Address}:{a.Port} [{a.Platform}]");
    }
}

public class SelfHostedTarget : ITargetProvider
{
    public string Name => "SelfHosted";

    public Task AuthenticateAsync(CancellationToken ct)
    {
        Log.Info("Authenticated to Self-Hosted (stub).");
        return Task.CompletedTask;
    }

    public Task EnsureSafesAsync(IEnumerable<Safe> safes, CancellationToken ct)
    {
        foreach (var s in safes.DistinctBy(s => s.Name))
            Log.Info($"Ensure Safe: {s.Name}");
        return Task.CompletedTask;
    }

    public Task EnsurePlatformsAsync(IEnumerable<Platform> platforms, CancellationToken ct)
    {
        foreach (var p in platforms.DistinctBy(p => p.Name))
            Log.Info($"Ensure Platform: {p.Name}");
        return Task.CompletedTask;
    }

    public async Task ImportAccountsAsync(IEnumerable<Account> accounts, CancellationToken ct)
    {
        await Task.Delay(50, ct); // simulate network
        foreach (var a in accounts)
            Log.Info($"Import Account: {a.Safe}/{a.Username}@{a.Address}:{a.Port} [{a.Platform}]");
    }
}
