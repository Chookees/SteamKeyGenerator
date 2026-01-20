namespace SteamKeyGenerator;

/// <summary>
/// Manages the command-line interface menus and user interactions.
/// </summary>
public class CliMenu
{
    /// <summary>Current generator configuration options.</summary>
    private static GeneratorOptions _options = new();

    /// <summary>
    /// Displays the main application menu with options to generate keys, configure settings, or exit.
    /// </summary>
    public static void MainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Steam Game Key Generator");
            Console.WriteLine("========================\n");
            Console.WriteLine("1 - Generate Key");
            Console.WriteLine("2 - Options");
            Console.WriteLine("3 - Exit");
            Console.Write("\nSelect option (1/2/3): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    GenerateKeyMenu();
                    break;
                case "2":
                    OptionsMenu();
                    break;
                case "3":
                    Console.WriteLine("? Goodbye!");
                    return;
                default:
                    Console.WriteLine("? Invalid selection");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    /// <summary>
    /// Displays the key generation menu where users can generate keys and validate them.
    /// Continuous generation loop until user enters input other than Y or N.
    /// </summary>
    private static void GenerateKeyMenu()
    {
        Console.Clear();
        Console.WriteLine("Steam Game Key Generator");
        Console.WriteLine("========================\n");
        Console.WriteLine($"Format: {_options.Format}");
        Console.WriteLine($"Save to Database: {(_options.SaveToDatabase ? "Yes" : "No")}");
        Console.WriteLine("(Any input other than Y or N will return to menu)\n");

        while (true)
        {
            var random = new Random();
            var generatedKey = KeyGenerator.GenerateUniqueKey(random, _options.Format);
            Console.WriteLine($"Generated Key: {generatedKey}");

            // Attempt to copy the key to clipboard
            ClipboardManager.CopyToClipboard(generatedKey);
            Console.WriteLine("(copied to clipboard)");

            Console.Write("Is this key valid? (Y/y for yes, N/n for no): ");

            var validationInput = Console.ReadLine();

            // Exit generation loop if invalid response
            if (!IsValidResponse(validationInput))
            {
                Console.WriteLine("\n? Returning to menu...");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            // Parse validation response
            var isValid = validationInput?.Equals("Y", StringComparison.OrdinalIgnoreCase) ?? false;

            // Save key to database if configured
            if (_options.SaveToDatabase)
            {
                KeyDatabaseManager.SaveKeyToDatabase(generatedKey, _options.Format, isValid);
                Console.WriteLine($"? Key saved to database (Valid: {(isValid ? "Yes" : "No")})\n");
            }
            else
            {
                Console.WriteLine($"? Key not saved (Valid: {(isValid ? "Yes" : "No")})\n");
            }
        }
    }

    /// <summary>
    /// Displays the options menu for configuring generator settings.
    /// Allows users to change the key format and database save preference.
    /// </summary>
    private static void OptionsMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Options");
            Console.WriteLine("=======\n");
            Console.WriteLine($"1 - Format: {_options.Format}");
            Console.WriteLine($"2 - Save to Database: {(_options.SaveToDatabase ? "Yes" : "No")}");
            Console.WriteLine("3 - Back to Menu");
            Console.Write("\nSelect option (1/2/3): ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SetFormatOption();
                    break;
                case "2":
                    SetSaveToDatabaseOption();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("? Invalid selection");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    /// <summary>
    /// Prompts user to select a key generation format (1, 2, or 3).
    /// Updates the global options with the selected format.
    /// </summary>
    private static void SetFormatOption()
    {
        Console.Clear();
        Console.WriteLine("Select Format");
        Console.WriteLine("=============\n");
        Console.WriteLine("1 - Format 1 (AAAAA-BBBBB-CCCCC-AAAAA-BBBBB-CCCCC-DDDDD-EEEEE)");
        Console.WriteLine("2 - Format 2 (AAAAA-BBBBB-CCCCC-DDDDD-EEEEE)");
        Console.WriteLine("3 - Format 3 (237ABCDGHJLPRST 23)");
        Console.Write("\nEnter format (1/2/3): ");

        var format = Console.ReadLine() switch
        {
            "1" => 1,
            "2" => 2,
            "3" => 3,
            _ => -1
        };

        if (format is > 0 and <= 3)
        {
            _options = _options with { Format = format };
            Console.WriteLine($"? Format set to {format}");
        }
        else
        {
            Console.WriteLine("? Invalid format selection");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    /// <summary>
    /// Prompts user to enable or disable automatic saving of generated keys to the database.
    /// </summary>
    private static void SetSaveToDatabaseOption()
    {
        Console.Clear();
        Console.WriteLine("Save to Database");
        Console.WriteLine("================\n");
        Console.WriteLine("1 - Yes");
        Console.WriteLine("2 - No");
        Console.Write("\nEnter choice (1/2): ");

        var choice = Console.ReadLine() switch
        {
            "1" => true,
            "2" => false,
            _ => _options.SaveToDatabase
        };

        _options = _options with { SaveToDatabase = choice };
        Console.WriteLine($"? Save to Database set to {(choice ? "Yes" : "No")}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    /// <summary>
    /// Validates whether the user input is either 'Y'/'y' or 'N'/'n'.
    /// </summary>
    /// <param name="input">The user input to validate.</param>
    /// <returns>True if input is valid (Y, y, N, or n); false otherwise.</returns>
    private static bool IsValidResponse(string? input)
        => input is not null &&
           (input.Equals("Y", StringComparison.OrdinalIgnoreCase) ||
            input.Equals("N", StringComparison.OrdinalIgnoreCase));
}
