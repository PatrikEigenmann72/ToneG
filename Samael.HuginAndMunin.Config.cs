// File: Samael.HuginAndMunin.Config.cs
// This file is part of the Samael.HuginAndMunin library and provides a configuration utility.
// It's a classic key=value container compiled in your project for easy access and safety
// reasons. This singleton pattern ensures that there is only one instance of the configuration
// available throughout the application.
// --------------------------------------------------------------------------------------------
// Author:          Patrik Eigenmann
// eMail:           p.eigenmann72@gmail.com
// GitHub:          https://github.com/PatrikEigenmann/HelloWorld
// --------------------------------------------------------------------------------------------
// Change Log:
// Sun 2025-08-10 File created and initial version implemented.                 Version: 00.01
// Tue 2025-08-12 Added method GetBool.                                         Version: 00.02
// Tue 2025-08-12 Added method GetInt.                                          Version: 00.03
// Tue 2025-08-12 Added method GetFloat.                                        Version: 00.04
// Tue 2025-08-12 Added method GetDouble.                                       Version: 00.05
// Thu 2025-08-21 BugFix: A misplaced } caused a compilation error.             Version: 00.06
// --------------------------------------------------------------------------------------------
namespace Samael.HuginAndMunin;

/// <summary>
/// Configuration class for a C# application. As part of the Samael.HuginAndMunin library,
/// it provides a centralized way to manage application settings. Compiled into your
/// application, it offers easy access to configuration values throughout the codebase, and
/// a certain safety by not having plain text files with passwords or sensitive information
/// laying around. I sealed this class to ensure that it cannot be inherited, providing a
/// more robust and reliable configuration management solution.
/// </summary>
public sealed class Config
{
    /// <summary>
    /// This ensures the singleton pattern and provides global access to the configuration
    /// throughout the application.
    /// </summary>
    private static Config? _instance;

    /// <summary>
    /// This dictionary holds the key-value pairs for the configuration settings.
    /// </summary>
    private readonly Dictionary<string, string> _map;

    /// <summary>
    /// Private constructor to prevent instantiation from outside the class.
    /// </summary>
    private Config()
    {
        _map = new Dictionary<string, string>
        {
            ["App.Title"] = "ToneG",
            ["App.Name"] = "ToneG",
            ["App.Version"] = "00.04",
            ["Btn.ToggleOn"] = "Stop",
            ["Btn.ToggleOff"] = "Run",
            ["Btn.Toggle.Name"] = "toggleButton"
            // Extend as needed
        };
    }

    /// <summary>
    /// Retrieves the value associated with the specified key from the configuration.
    /// </summary>
    /// <param name="key">The key of the configuration setting to retrieve.</param>
    /// <returns>The value associated with the specified key, or an empty string if the key is not found.</returns>
    public static string Get(string key)
    {
        if (_instance == null)
        {
            _instance = new Config();
        }

        return _instance._map.TryGetValue(key, out var value) ? value : string.Empty;
    }

    /// <summary>
    /// Retrieves the boolean value associated with the specified key from the configuration.
    /// </summary>
    /// <param name="key">The key of the configuration setting to retrieve.</param>
    /// <returns>The parsed boolean value.</returns>
    /// <exception cref="FormatException">Thrown if the value cannot be parsed as a boolean.</exception>
    public static bool GetBool(string key)
    {
        var value = Get(key);
        return bool.TryParse(value, out var result)
            ? result
            : throw new FormatException($"Value '{value}' for key '{key}' is not a valid bool.");
    }

    /// <summary>
    /// Retrieves the integer value associated with the specified key from the configuration.
    /// </summary>
    /// <param name="key">The key of the configuration setting to retrieve.</param>
    /// <returns>The parsed integer value.</returns>
    /// <exception cref="FormatException">Thrown if the value cannot be parsed as an integer.</exception>
    public static int GetInt(string key)
    {
        var value = Get(key);
        return int.TryParse(value, out var result)
            ? result
            : throw new FormatException($"Value '{value}' for key '{key}' is not a valid int.");
    }

    /// <summary>
    /// Retrieves the float value associated with the specified key from the configuration.
    /// </summary>
    /// <param name="key">The key of the configuration setting to retrieve.</param>
    /// <returns>The parsed float value.</returns>
    /// <exception cref="FormatException">Thrown if the value cannot be parsed as a float.</exception>
    public static float GetFloat(string key)
    {
        var value = Get(key);
        return float.TryParse(value, out var result)
            ? result
            : throw new FormatException($"Value '{value}' for key '{key}' is not a valid float.");
    }

    /// <summary>
    /// Retrieves the double value associated with the specified key from the configuration.
    /// </summary>
    /// <param name="key">The key of the configuration setting to retrieve.</param>
    /// <returns>The parsed double value.</returns>
    /// <exception cref="FormatException">Thrown if the value cannot be parsed as a double.</exception>
    public static double GetDouble(string key)
    {
        var value = Get(key);
        return double.TryParse(value, out var result)
            ? result
            : throw new FormatException($"Value '{value}' for key '{key}' is not a valid double.");
    }
}