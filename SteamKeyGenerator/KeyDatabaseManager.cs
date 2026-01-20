using System.Text.Json;

namespace SteamKeyGenerator;

/// <summary>
/// Manages persistence and retrieval of Steam game keys from the database.
/// </summary>
public class KeyDatabaseManager
{
    /// <summary>Path to the JSON database file storing generated keys.</summary>
    private static readonly string DatabasePath = "steam_keys_database.json";

    /// <summary>JSON serialization options with pretty-printing enabled.</summary>
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    /// <summary>
    /// Saves a generated key to the database with its validity status.
    /// </summary>
    /// <param name="key">The key to save.</param>
    /// <param name="format">Format type (1, 2, or 3).</param>
    /// <param name="isValid">Whether the key was validated as valid.</param>
    public static void SaveKeyToDatabase(string key, int format, bool isValid)
    {
        var database = LoadKeysFromDatabase();
        var entry = new KeyEntry(key, isValid);

        // Add entry to appropriate format collection using immutable update
        var newDatabase = database with
        {
            Format1 = format == 1 ? [..database.Format1, entry] : database.Format1,
            Format2 = format == 2 ? [..database.Format2, entry] : database.Format2,
            Format3 = format == 3 ? [..database.Format3, entry] : database.Format3
        };

        // Persist database to JSON file
        using var stream = File.Create(DatabasePath);
        JsonSerializer.Serialize(stream, newDatabase, JsonOptions);
    }

    /// <summary>
    /// Loads the Steam key database from the JSON file.
    /// Creates an empty database if the file does not exist.
    /// </summary>
    /// <returns>The loaded or newly created database.</returns>
    public static SteamKeyDatabase LoadKeysFromDatabase()
    {
        if (!File.Exists(DatabasePath))
        {
            return new SteamKeyDatabase
            {
                Format1 = [],
                Format2 = [],
                Format3 = []
            };
        }

        using var stream = File.OpenRead(DatabasePath);
        var database = JsonSerializer.Deserialize<SteamKeyDatabase>(stream);

        // Return empty database if deserialization fails
        return database ?? new SteamKeyDatabase { Format1 = [], Format2 = [], Format3 = [] };
    }

    /// <summary>
    /// Checks if a key already exists in the database for the specified format.
    /// </summary>
    /// <param name="database">The Steam key database.</param>
    /// <param name="key">The key to check.</param>
    /// <param name="format">Format type (1, 2, or 3).</param>
    /// <returns>True if the key exists in the database, false otherwise.</returns>
    public static bool KeyExistsInDatabase(SteamKeyDatabase database, string key, int format) => format switch
    {
        1 => database.Format1.Any(k => k.Key == key),
        2 => database.Format2.Any(k => k.Key == key),
        3 => database.Format3.Any(k => k.Key == key),
        _ => false
    };
}
