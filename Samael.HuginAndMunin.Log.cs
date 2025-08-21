// -----------------------------------------------------------------------------------------------
// File: Samael.HuginAndMunin.Log.cs
// This file is part of the Samael.HuginAndMunin library and provides a designated logging utility.
// It includes methods for Writing Log messages on the levels of errors and exceptions, warnings,
// and informational or verbose messages. With this utility, developers can easily track their
// application's behavior after development and debugging phases.
// -----------------------------------------------------------------------------------------------
// Author:          Patrik Eigenmann
// eMail:           p.eigenmann72@gmail.com
// GitHub:          https://github.com/PatrikEigenmann/HelloWorld
// -----------------------------------------------------------------------------------------------
// Change Log:
// Thu 2025-08-07 File created and initial version implemented.                     Version: 00.01
// Sat 2025-08-09 File updated to include console and MessageBox output handling.   Version: 00.02
// Sun 2025-08-10 WriteLine added parameter string component.                       Version: 00.03
// -----------------------------------------------------------------------------------------------
namespace Samael.HuginAndMunin;

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// LogLevel enumerations are based on a bitmask to control the output of the log messages.
/// Each level corresponeds to a specific type of message that can be displayed.
/// This allows for flexible control over what messages are shown in the log file.
/// </summary>
[Flags]
public enum LogLevel
{
    None = 0,                                 // No log output                0000
    Error = 1 << 0,                            // Error messages               0001
    Warning = 1 << 1,                            // Warning messages             0010
    Info = 1 << 2,                            // Informational messages       0100
    Verbose = 1 << 3,                            // Verbose messages             1000
    All = Error | Warning | Info | Verbose   // All messages                 1111
}

/// <summary>
/// The class Log provides static methods for writing log messages. It includes
/// methods for writing messages at different log levels, handling errors and exceptions,
/// warnings, and informational or verbose messages. The log output is controlled by a
/// bitmask that can be set to different levels depending on the build or configuration.
/// </summary>
public static class Log
{
    /// <summary>
    /// The big question I was asking myself was how do I know if the log file is empty or has
    /// already entries in it? Because, when do I append without to overload the file with too
    /// much information. My solution was to rewrite the file on each application start. That
    /// way, I ensure a clean log file for each session (Application runtime). On initialization
    /// this flag will be set to false, that triggers the file to be rewritten.
    /// </summary>
    private static bool _initialized = false;

    /// <summary>
    /// The current log level bitmask. This controls which log message are written or which are
    /// disregarded. Since the LogLevel are on bit level, you can combine multiple levels using
    /// the bitwise OR operator. As example: Log.Bitmask = LogLevel.Error | LogLevel.Warning will
    /// track only errors, esxeptioonc
    /// </summary>
    private static LogLevel _bitmask = LogLevel.All;

    /// <summary>
    /// Setter / Getter for the private property _bitmask. Not to be redundant, but the bitmask
    /// concept for log types is pretty satisfying and elegant. Bitmask operations are so self-
    /// explanatory and easy to use.
    /// </summary>
    public static LogLevel Bitmask
    {
        get => _bitmask;
        set => _bitmask = value;
    }

    /// <summary>
    /// My thoughts about the actual log file are the follows. I want to ensure that the log file
    /// is always in a clean state when the application starts. This means I need to handle the
    /// log file carefully to avoid appending too much information and potentially overwhelming
    /// the file with unnecessary data. And it should be in a place, where end users and computer
    /// proficients can easily find the file and have write/read access to it. What better place
    /// than $HOME\Documents\Logs is there? In case I ever have to give support to because one
    /// of my programs is misbehaving, I can tell the person please email me the log file
    /// {AppName}.log in the folder Logs in your Documents.
    /// </summary>
    private static readonly string _logFilePath = GetLogFilePath();

    /// <summary>
    /// This method will stitches the path/file name together. Endresult is a string with
    /// $HOME\Documents\Logs\{AppName}.log.
    /// </summary>
    /// <returns>String with $HOME\Documents\Logs\{AppName}.log</returns>
    private static string GetLogFilePath()
    {
        var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var logDir = Path.Combine(documents, "Logs");

        if (!Directory.Exists(logDir))
            Directory.CreateDirectory(logDir);

        var appName = Process.GetCurrentProcess().ProcessName;
        return Path.Combine(logDir, $"{appName}.log");
    }

    /// <summary>
    /// Here is where the magic is happening. This method will write a well formatted log entry.
    /// In this method, I'll format the message with a timestamp and the log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    private static void WriteToFile(string message)
    {
        try
        {
            var mode = _initialized ? FileMode.Append : FileMode.Create;
            using var stream = new FileStream(_logFilePath, mode, FileAccess.Write, FileShare.Read);
            using var writer = new StreamWriter(stream);
            writer.WriteLine(message);
            _initialized = true;
        }
        catch (Exception ex)
        {
            // A simple fallback mechanism, just in case logging falils.
            FallbackReport("WriteToFile", ex);
        }
    }

    /// <summary>
    /// This method will write the actual log entry to the log file. It appends the message
    /// to the log file with a timestamp and log level. Here the bitmask will really shine
    /// with fast and efficient filtering of log messages on a bit level.
    /// </summary>
    /// <param name="level">This is the LogLevel that needs to be checked.</param>
    /// <param name="message">The message is a string that will be written with application context.</param>
    /// /// <param name="component">The component is the name of the component that is logging the message.</param>
    public static void WriteLine(LogLevel level, string message, string component)
    {
        if ((_bitmask & level) != 0)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            WriteToFile($"{timestamp} [{level}] [{component}] {message}");
        }
    }

    /// <summary>
    /// This method will write an exception entry to the log file. It appends the exception details
    /// to the log file with a timestamp and log level.
    /// </summary>
    /// <param name="ex">The exception that occurred.</param>
    public static void WriteException(Exception ex)
    {
        if ((_bitmask & LogLevel.Error) != 0)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            WriteToFile($"{timestamp} [Exception] {ex.GetType().Name}: {ex.Message}");
            WriteToFile(ex.StackTrace ?? "No stack trace available.");
        }
    }

    /// <summary>
    /// The FallbackReport method is an insurance policy, just in case the logging system fails
    /// for some reasons. What you don't want is that the application silently shuts down without
    /// any indication of what went wrong, and it turns out it's a logging issue. But create a
    /// logging system to log a logging system makes no sense. So I implemented a simple fallback
    /// strategy that the application doesn't disappear in the either without a trace.
    /// </summary>
    /// <param name="operation">The operation that was being performed.</param>
    /// <param name="ex">The exception that occurred.</param>
    private static void FallbackReport(string operation, Exception ex)
    {
        var message = $"Logging failed during '{operation}': {ex.Message}";

        if (IsConsoleApp())
        {
            try { Console.Error.WriteLine(message); }
            catch { /* swallow */ }
        }
        else
        {
            try
            { MessageBox.Show(message, "Logging Issue", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            catch { /* swallow */ }
        }
    }

    /// <summary>
    /// This method checks if the application is running as a console application or not. This is
    /// important to know, because the output channel is either a console or a MessageBox.
    /// </summary>
    /// <returns>True or false if it is a console application.</returns>
    private static bool IsConsoleApp()
    {
        try
        {
            // Check if the standard output is a console
            return Console.OpenStandardOutput(1) != Stream.Null;
        }
        catch
        {
            // It is not a console application
            return false;
        }
    }

    /// <summary>
    /// Attempts to release any lingering file handles. This is a no-op unless future changes introduce persistent streams.
    /// </summary>
    public static void Release()
    {
        try
        {
            // Defensive: touch the file with a dummy open/close to ensure no locks remain
            using var stream = new FileStream(_logFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        }
        catch (Exception ex)
        {
            FallbackReport("Release", ex);
        }
    }
}
