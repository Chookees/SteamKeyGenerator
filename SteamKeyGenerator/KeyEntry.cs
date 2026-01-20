using System.Text.Json.Serialization;

namespace SteamKeyGenerator;

/// <summary>
/// Represents a single Steam game key entry with its validity status.
/// </summary>
public record KeyEntry(string Key, bool IsValid);
