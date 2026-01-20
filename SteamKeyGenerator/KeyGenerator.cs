namespace SteamKeyGenerator;

/// <summary>
/// Manages the generation of Steam game keys in different formats.
/// </summary>
public class KeyGenerator
{
    /// <summary>
    /// Generates a unique key in the specified format, ensuring it does not already exist in the database.
    /// </summary>
    /// <param name="random">Random instance for key generation.</param>
    /// <param name="format">Format type (1, 2, or 3).</param>
    /// <returns>A unique generated key string.</returns>
    public static string GenerateUniqueKey(Random random, int format)
    {
        var database = KeyDatabaseManager.LoadKeysFromDatabase();

        string key;
        do
        {
            key = GenerateKeyByFormat(random, format);
        } while (KeyDatabaseManager.KeyExistsInDatabase(database, key, format));

        return key;
    }

    /// <summary>
    /// Generates a key based on the specified format type.
    /// </summary>
    /// <param name="random">Random instance for generation.</param>
    /// <param name="format">Format type (1, 2, or 3).</param>
    /// <returns>A generated key string.</returns>
    private static string GenerateKeyByFormat(Random random, int format) => format switch
    {
        1 => GenerateFormat1Key(random),
        2 => GenerateFormat2Key(random),
        3 => GenerateFormat3Key(random),
        _ => string.Empty
    };

    /// <summary>
    /// Generates a Format 1 key: AAAAA-BBBBB-CCCCC-AAAAA-BBBBB-CCCCC-DDDDD-EEEEE (8 parts).
    /// </summary>
    /// <param name="random">Random instance for generation.</param>
    /// <returns>A Format 1 key string.</returns>
    private static string GenerateFormat1Key(Random random)
        => $"{GenerateKeyPart(random)}-{GenerateKeyPart(random)}-{GenerateKeyPart(random)}-" +
           $"{GenerateKeyPart(random)}-{GenerateKeyPart(random)}-{GenerateKeyPart(random)}-" +
           $"{GenerateKeyPart(random)}-{GenerateKeyPart(random)}";

    /// <summary>
    /// Generates a Format 2 key: AAAAA-BBBBB-CCCCC-DDDDD-EEEEE (5 parts).
    /// </summary>
    /// <param name="random">Random instance for generation.</param>
    /// <returns>A Format 2 key string.</returns>
    private static string GenerateFormat2Key(Random random)
        => $"{GenerateKeyPart(random)}-{GenerateKeyPart(random)}-{GenerateKeyPart(random)}-" +
           $"{GenerateKeyPart(random)}-{GenerateKeyPart(random)}";

    /// <summary>
    /// Generates a Format 3 key: 237ABCDGHJLPRST 23 (14 alphanumeric chars + space + 2 digit number).
    /// </summary>
    /// <param name="random">Random instance for generation.</param>
    /// <returns>A Format 3 key string.</returns>
    private static string GenerateFormat3Key(Random random)
    {
        const string chars = "237ABCDGHJLPRST";

        // Generate 14-character alphanumeric string using stack allocation
        var firstPart = string.Create(14, random, (span, rng) =>
        {
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = chars[rng.Next(chars.Length)];
            }
        });

        // Generate 2-digit number suffix
        var secondPart = random.Next(10, 100).ToString();

        return $"{firstPart} {secondPart}";
    }

    /// <summary>
    /// Generates a single 5-character key part using letters and digits.
    /// </summary>
    /// <param name="random">Random instance for generation.</param>
    /// <returns>A 5-character alphanumeric string.</returns>
    private static string GenerateKeyPart(Random random)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        // Use stack allocation for efficient memory usage
        return string.Create(5, random, (span, rng) =>
        {
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = chars[rng.Next(chars.Length)];
            }
        });
    }
}
