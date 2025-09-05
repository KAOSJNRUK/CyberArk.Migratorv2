namespace CyberArk.Migrator.Core.Models;

public record Safe
{
    public string Name { get; init; } = "";
    public string? Description { get; init; }
    public string? ManagingTeam { get; init; }
    public Dictionary<string, string>? Tags { get; init; }
}

public record Platform
{
    public string Name { get; init; } = "";
    public string Technology { get; init; } = "";
    public Dictionary<string, string>? Attributes { get; init; }
}

public record Account
{
    public string Safe { get; init; } = "";
    public string Platform { get; init; } = "";
    public string Address { get; init; } = "";
    public int Port { get; init; }
    public string Username { get; init; } = "";
    public string Secret { get; init; } = ""; // kept in-memory only
    public bool Rotatable { get; init; }
    public Dictionary<string,string>? Meta { get; init; }
}
