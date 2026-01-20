namespace SteamKeyGenerator;

/// <summary>
/// Configuration options for the Steam key generator.
/// </summary>
public record GeneratorOptions
{
    /// <summary>Gets or initializes the format to use for key generation (1, 2, or 3). Default: 1.</summary>
    public int Format { get; init; } = 1;

    /// <summary>Gets or initializes whether generated keys should be saved to the database. Default: true.</summary>
    public bool SaveToDatabase { get; init; } = true;
}
