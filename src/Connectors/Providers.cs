using CyberArk.Migrator.Core.Abstractions;
using CyberArk.Migrator.Core.Models;

namespace CyberArk.Migrator.Connectors;

public class DelineaProvider : ISourceProvider
{
    public string Name => "Delinea";
    public Task AuthenticateAsync(CancellationToken ct) => Task.CompletedTask;

    public async IAsyncEnumerable<Safe> ExportSafesAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        yield return new Safe { Name = "CORP-UNIX", Description = "Sample from Delinea" };
        await Task.CompletedTask;
    }

    public async IAsyncEnumerable<Platform> ExportPlatformsAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        yield return new Platform { Name = "UnixSSH", Technology = "SSH" };
        await Task.CompletedTask;
    }

    public async IAsyncEnumerable<Account> ExportAccountsAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        yield return new Account { Safe="CORP-UNIX", Platform="UnixSSH", Address="server01", Port=22, Username="root", Secret="dummy", Rotatable=true };
        await Task.CompletedTask;
    }
}

public class BeyondTrustProvider : ISourceProvider
{
    public string Name => "BeyondTrust";
    public Task AuthenticateAsync(CancellationToken ct) => Task.CompletedTask;

    public async IAsyncEnumerable<Safe> ExportSafesAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        yield return new Safe { Name = "BT-WIN", Description = "Sample from BeyondTrust" };
        await Task.CompletedTask;
    }

    public async IAsyncEnumerable<Platform> ExportPlatformsAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        yield return new Platform { Name = "WinLocal", Technology = "Windows" };
        await Task.CompletedTask;
    }

    public async IAsyncEnumerable<Account> ExportAccountsAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        yield return new Account { Safe="BT-WIN", Platform="WinLocal", Address="win01", Port=3389, Username="Administrator", Secret="dummy", Rotatable=true };
        await Task.CompletedTask;
    }
}

public class OsiriumProvider : ISourceProvider
{
    public string Name => "Osirium";
    public Task AuthenticateAsync(CancellationToken ct) => Task.CompletedTask;

    public async IAsyncEnumerable<Safe> ExportSafesAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        yield return new Safe { Name = "OSI-NET", Description = "Sample from Osirium" };
        await Task.CompletedTask;
    }

    public async IAsyncEnumerable<Platform> ExportPlatformsAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        yield return new Platform { Name = "NetworkDevice", Technology = "SSH" };
        await Task.CompletedTask;
    }

    public async IAsyncEnumerable<Account> ExportAccountsAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        yield return new Account { Safe="OSI-NET", Platform="NetworkDevice", Address="switch01", Port=22, Username="admin", Secret="dummy", Rotatable=false };
        await Task.CompletedTask;
    }
}
