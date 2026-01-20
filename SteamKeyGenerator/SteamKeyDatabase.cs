using System.Text.Json.Serialization;

namespace SteamKeyGenerator;

/// <summary>
/// Represents the complete Steam key database, organized by format.
/// </summary>
public record SteamKeyDatabase
{
    /// <summary>Gets or initializes the collection of Format 1 keys (8-part format).</summary>
    [JsonPropertyName("Format1")]
    public required List<KeyEntry> Format1 { get; init; }

    /// <summary>Gets or initializes the collection of Format 2 keys (5-part format).</summary>
    [JsonPropertyName("Format2")]
    public required List<KeyEntry> Format2 { get; init; }

    /// <summary>Gets or initializes the collection of Format 3 keys (alphanumeric format).</summary>
    [JsonPropertyName("Format3")]
    public required List<KeyEntry> Format3 { get; init; }
}
