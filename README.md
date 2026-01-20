# SteamKeyGenerator

A modern, cross-platform CLI tool for generating and managing Steam game keys in various formats with optional persistence in a JSON database.

## Features

- **Multi-Format Key Generation**: Supports three different Steam key formats
  - **Format 1**: 8-part format (AAAAA-BBBBB-CCCCC-AAAAA-BBBBB-CCCCC-DDDDD-EEEEE)
  - **Format 2**: 5-part format (AAAAA-BBBBB-CCCCC-DDDDD-EEEEE)
  - **Format 3**: Alphanumeric format (237ABCDGHJLPRST 23)

- **Uniqueness Guarantee**: Generated keys are validated against the database to exclude duplicates

- **Persistence**: Optional storage of generated keys with validity status in a JSON database

- **Cross-Platform Clipboard**: Automatic copying of generated keys to the clipboard on Windows, Linux, and macOS

- **Interactive CLI**: User-friendly menu navigation with real-time configuration

- **Configurable Options**: 
  - Format selection
  - Enable/disable database persistence

## Installation

### Requirements

- **.NET 10** or higher
- Windows, Linux, or macOS

### Build from Source

```bash
git clone https://github.com/Chookees/SteamKeyGenerator.git
cd SteamKeyGenerator
dotnet build -c Release
dotnet run --project SteamKeyGenerator/SteamKeyGenerator.csproj
```

### Direct Execution (without Build)

```bash
dotnet run
```

## Usage

### Main Menu

When starting, you will be greeted with the following menu:

```
Steam Game Key Generator
========================

1 - Generate Key
2 - Options
3 - Exit

Select option (1/2/3):
```

### Key Generation

1. Select **Option 1** from the main menu
2. A new key will be generated and automatically copied to the clipboard
3. Confirm the validity of the key:
   - **Y/y**: Mark key as valid
   - **N/n**: Mark key as invalid
   - **Any other input**: Return to main menu
4. The key will optionally be saved to the database (depending on settings)

### Options

1. Select **Option 2** from the main menu
2. Configure:
   - **Format**: Choose between Format 1, 2, or 3
   - **Save to Database**: Enable/disable persistence

The selected settings will remain for the current session.

## Architecture

The project follows the **Single Responsibility Principle** with clear separation of concerns:

```
Program.cs (Entry-Point)
    ⬇
CliMenu (UI & User Interaction)
    - KeyGenerator (Key Generation)
    - KeyDatabaseManager (Persistence)
    - ClipboardManager (System Integration)
```

### Components

| Class | Responsibility |
|-------|-----------------|
| **Program.cs** | Application entry point |
| **CliMenu** | Menu logic and user interface |
| **KeyGenerator** | Generation of keys in all formats |
| **KeyDatabaseManager** | JSON persistence and duplicate checking |
| **ClipboardManager** | OS-specific clipboard operations |
| **KeyEntry** | Data model for individual keys |
| **SteamKeyDatabase** | Data model for the database |
| **GeneratorOptions** | Configuration data model |

### Data Flow

```
[User Input] 
    ⬇
[CliMenu validates]
    ⬇
[KeyGenerator creates unique key]
    ⬇
[KeyDatabaseManager checks uniqueness]
    ⬇
[ClipboardManager copies to clipboard]
    ⬇
[Optional: KeyDatabaseManager persists to JSON]
```

## Database Format

Keys are stored in JSON format (`steam_keys_database.json`):

```json
{
  "Format1": [
    {"key": "AAAAA-BBBBB-CCCCC-DDDDD-EEEEE-FFFFF-GGGGG-HHHHH", "isValid": true},
    {"key": "...", "isValid": false}
  ],
  "Format2": [...],
  "Format3": [...]
}
```

## Technical Details

### Key Generation

- **Stack Allocation**: Uses `string.Create()` for efficient memory usage
- **Random Instances**: New instance per generation for optimal randomness
- **Character Sets**: Format-specific, pre-defined character sets
- **Format 1 & 2**: Alphanumeric characters (A-Z, 0-9)
- **Format 3**: Limited set (237ABCDGHJLPRST) + 2-digit number

### Database Operations

- **JSON Serialization**: Using `System.Text.Json` for performance
- **Immutable Records**: Use of C# records with `with` syntax for safe data manipulation
- **Error Tolerance**: Automatic creation of an empty database on errors

### Clipboard Management

Implements OS-specific mechanisms:

- **Windows**: PowerShell `Set-Clipboard` cmdlet
- **Linux**: `xclip` with Bash integration
- **macOS**: `pbcopy` with Bash integration

Failed clipboard operations are silently ignored and do not interrupt the workflow.

## Development

### Project Structure

```
SteamKeyGenerator/
- Program.cs                  (Entry-Point)
- CliMenu.cs                  (UI Logic)
- KeyGenerator.cs             (Generation)
- KeyDatabaseManager.cs        (Persistence)
- ClipboardManager.cs          (System Integration)
- KeyEntry.cs                 (Model)
- SteamKeyDatabase.cs          (Model)
- GeneratorOptions.cs          (Model)
- SteamKeyGenerator.csproj
```

### Build and Test

```bash
# Debug build
dotnet build

# Release build
dotnet build -c Release

# Clean
dotnet clean
```

## License

This project is licensed under the **MIT License**. You are free to use, modify, and distribute it.

```
MIT License

Copyright (c) 2024

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

## Contributing

Contributions are welcome! Please create a fork and submit your improvements as pull requests.

## Support

If you have any questions or issues, please open an [Issue](https://github.com/Chookees/SteamKeyGenerator/issues) on GitHub.

---

**Note**: This tool is for demonstration purposes only. Generating real Steam keys requires proper authorization from Valve/Steam.
