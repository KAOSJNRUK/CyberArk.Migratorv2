using CsvHelper.Configuration.Attributes;

namespace CyberArk.Migrator.Core.Csv;

public class AccountCsvRow
{
    [Name("safe")] public string Safe { get; set; } = "";
    [Name("platform")] public string Platform { get; set; } = "";
    [Name("address")] public string Address { get; set; } = "";
    [Name("port")] public int Port { get; set; }
    [Name("username")] public string Username { get; set; } = "";
    [Name("secret_enc")] public string SecretEnc { get; set; } = "";
    [Name("rotatable")] public bool Rotatable { get; set; }
    [Name("json_meta")] public string? JsonMeta { get; set; }
}
