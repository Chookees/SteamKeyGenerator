namespace SteamKeyGenerator;

/// <summary>
/// Main entry point for the Steam Game Key Generator application.
/// </summary>
class Program
{
    /// <summary>
    /// Application entry point. Displays the main menu and handles user navigation.
    /// </summary>
    static int Main(string[] args)
    {
        CliMenu.MainMenu();
        return 0;
    }
}
