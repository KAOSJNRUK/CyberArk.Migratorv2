using CyberArk.Migrator.Core.Models;
using CyberArk.Migrator.Core.Abstractions;

namespace CyberArk.Migrator.Infra;

public static class ImportOrchestrator
{
    public static async Task RunBatchedParallelAsync(
        ITargetProvider target,
        IEnumerable<Account> allAccounts,
        int batchSize,
        int maxDegreeOfParallelism,
        CancellationToken ct)
    {
        var batches = new List<List<Account>>();
        var current = new List<Account>(batchSize);
        foreach (var a in allAccounts)
        {
            current.Add(a);
            if (current.Count >= batchSize)
            {
                batches.Add(current);
                current = new List<Account>(batchSize);
            }
        }
        if (current.Count > 0) batches.Add(current);

        var throttler = new SemaphoreSlim(maxDegreeOfParallelism);
        var tasks = new List<Task>();
        foreach (var batch in batches)
        {
            await throttler.WaitAsync(ct);
            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    await target.ImportAccountsAsync(batch, ct);
                }
                finally { throttler.Release(); }
            }, ct));
        }
        await Task.WhenAll(tasks);
    }
}
