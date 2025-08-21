// --------------------------------------------------------------------------------------------
// File: Samael.HuginAndMunin.Debug.cs
// This file is part of the Samael.HuginAndMunin library and provides a debugging utility.
// It includes methods for displaying debug messages on the levels of errors and exceptions,
// warnings, and informational or verbose messages. With this utility, developers can easily
// track their application's behavior during development and debugging phases. The debug
// messaging will be shut off in release builds to avoid performance overhead and cluttering
// the console output. The debug messages are color-coded for better visibility in the console.
// --------------------------------------------------------------------------------------------
// Author:          Patrik Eigenmann
// eMail:           p.eigenmann72@gmail.com
// GitHub:          https://github.com/PatrikEigenmann/HelloWorld
// --------------------------------------------------------------------------------------------
// Change Log:
// Thu 2025-08-07 File created and initial version implemented.                 Version: 00.01
// Thu 2025-08-07 Cleaned up the code and added AttachConsole(dwProcessId).     Version: 00.02
// Sat 2025-08-09 Cleaned up the output formatting of the debug message.        Version: 00.03
// Sun 2025-08-10 WriteLine added parameter string component.                   Version: 00.04
// --------------------------------------------------------------------------------------------
namespace Samael.HuginAndMunin;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// DebugLevel enumerations are based on a bitmask to control the output of the debug messages.
/// Each level corresponeds to a specific type of message that can be displayed.
/// This allows for flexible control over what messages are shown in the console.
/// </summary>
[Flags]
public enum DebugLevel
{
    None = 0,                                // No debug output              0000
    Error = 1 << 0,                          // Error messages               0001
    Warning = 1 << 1,                        // Warning messages             0010
    Info = 1 << 2,                           // Informational messages       0100
    Verbose = 1 << 3,                        // Verbose debugging messages   1000
    All = Error | Warning | Info | Verbose   // All messages                 1111
}

/// <summary>
/// The class Debug provides static methods for displaying debug messages. It includes
/// methods for writing messages at different debug levels, handling errors and exceptions,
/// warnings, and informational or verbose messages. The debug output is controlled by a
/// bitmask that can be set to different levels depending on the build or configuration.
/// In debug mode, the bitmask is set to allow messages, and in release mode no messages are shown.
/// </summary>
public static class Debug
{

#if DEBUG
    /// <summary>
    /// The current debug level bitmask. In debug mode, the bitmask is set to all,
    /// allowing all debug outputs to be displayed. Very useful if the project is 
    /// being actively developed and you want to see debug messages in the console.
    /// </summary>
    private static DebugLevel _bitmask = DebugLevel.All;
#else
    /// <summary>
    /// The current debug level bitmask. In release mode the bitmask is set to None,
    /// to avoid unnecessary debug outputs. This is useful for performance reasons
    /// and to prevent cluttering the console.
    /// </summary>
    private static DebugLevel _bitmask = DebugLevel.None;
#endif

    /// <summary>
    /// The Bitmask property allows you to get or set the current debug level bitmask.
    /// This property can be used to control which debug messages are displayed
    /// based on the current configuration of the application. Bitmask can be easily
    /// set with bitwise operations to include or exclude specific debug levels.
    /// </summary>
    public static DebugLevel Bitmask
    {
        get => _bitmask;
        set
        {
#if DEBUG
            _bitmask = value;   // Set the bitmask to the desired level for debug output.
            AttachConsole();    // Ensure a console is attached for debug output.
#endif
        }
    }

    /// <summary>
    /// Writes a debug message to the console with the specified level.
    /// This method checks the current bitmask to determine if the message should be displayed.
    /// It formats the message with a timestamp and the debug level, and colors the output
    /// based on the level of the message.
    /// </summary>
    /// /// <param name="level">The debug level of the message.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="component">The component that is debugging the message.</param>
    public static void WriteLine(DebugLevel level, string message, string component)
    {
#if DEBUG
        if ((_bitmask & level) != 0)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            var color = GetColorForLevel(level);
            Console.ForegroundColor = color;
            Console.WriteLine($"{timestamp} [{level}] [{component}] {message}");
            Console.ResetColor();
        }
#endif
    }

    /// <summary>
    /// Writes an error message to the console.
    /// This method is specifically for logging error messages
    /// and will only output if the Error level is included in the bitmask.
    /// </summary>
    public static void WriteException(Exception ex)
    {
#if DEBUG
        if ((_bitmask & DebugLevel.Error) != 0)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{timestamp} [Exception] {ex.GetType().Name}: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.ResetColor();
        }
#endif
    }

    /// <summary>
    /// Attaches a console window to the process.
    /// This is useful for applications that do not have a console window by default,
    /// allowing them to output debug information to a console.
    /// </summary>
    public static void AttachConsole()
    {
#if DEBUG
        if (!IsConsoleAttached)
        {
            int parentProcess = -1; // Use -1 to attach to the parent process console.
            AllocConsole(parentProcess);
        }
#endif
    }

    /// <summary>
    /// AllocConsole is a P/Invoke method that allocates a new console for the calling process.
    /// This is necessary for applications that do not have a console window by default,
    /// allowing them to output debug information to a console.
    /// </summary>
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    /// <summary>
    /// AllocConsole is a P/Invoke method that allocates a new console for the calling process.
    /// This is necessary for applications that do not have a console window by default,
    /// allowing them to output debug information to a console.
    /// </summary>
    /// <param name="dwProcessId">The process ID of the console to attach to, or -1 to create a new console.</param>
    [DllImport("kernel32.dll")]
    private static extern bool AllocConsole(int dwProcessId);


    /// <summary>
    /// IsConsoleAttached checks if a console window is currently attached to the process.
    /// This is useful to determine if debug mode is already in an active console environment,
    /// or if a new console needs to be allocated becuase the application is in Windows Forms
    /// or WPF mode where no console is attached by default.
    /// </summary>
    private static bool IsConsoleAttached => GetConsoleWindow() != IntPtr.Zero;

#if DEBUG
    /// <summary>
    /// GetColorForLevel is a helper method that returns the console color
    /// associated with a specific debug level. This method is used to color-code
    /// the debug output for better visibility in the console.
    /// </summary>
    private static ConsoleColor GetColorForLevel(DebugLevel level)
    {
        return level switch
        {
            DebugLevel.Error   => ConsoleColor.Red,
            DebugLevel.Warning => ConsoleColor.Yellow,
            DebugLevel.Info    => ConsoleColor.Cyan,
            DebugLevel.Verbose => ConsoleColor.Gray,
            _                  => ConsoleColor.White
        };
    }
#endif
}
