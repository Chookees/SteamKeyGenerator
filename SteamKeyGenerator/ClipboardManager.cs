using System.Diagnostics;

namespace SteamKeyGenerator;

/// <summary>
/// Manages copying text to the system clipboard across different operating systems.
/// </summary>
public class ClipboardManager
{
    /// <summary>
    /// Copies the specified text to the system clipboard using the appropriate method for the operating system.
    /// Silently fails if the clipboard operation is not available.
    /// </summary>
    /// <param name="text">The text to copy to clipboard.</param>
    public static void CopyToClipboard(string text)
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                CopyToClipboardWindows(text);
            }
            else if (OperatingSystem.IsLinux())
            {
                CopyToClipboardLinux(text);
            }
            else if (OperatingSystem.IsMacOS())
            {
                CopyToClipboardMacOS(text);
            }
        }
        catch
        {
            // Silently fail if clipboard operation not available
        }
    }

    /// <summary>
    /// Copies text to clipboard on Windows using PowerShell.
    /// </summary>
    /// <param name="text">The text to copy.</param>
    private static void CopyToClipboardWindows(string text)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = $"-Command Set-Clipboard -Value '{text.Replace("'", "''")}'",
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        process.WaitForExit();
    }

    /// <summary>
    /// Copies text to clipboard on Linux using xclip command.
    /// </summary>
    /// <param name="text">The text to copy.</param>
    private static void CopyToClipboardLinux(string text)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"echo '{text.Replace("'", "'\\''")}' | xclip -selection clipboard\"",
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        process.WaitForExit();
    }

    /// <summary>
    /// Copies text to clipboard on macOS using pbcopy command.
    /// </summary>
    /// <param name="text">The text to copy.</param>
    private static void CopyToClipboardMacOS(string text)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"echo '{text.Replace("'", "'\\''")}' | pbcopy\"",
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        process.WaitForExit();
    }
}
