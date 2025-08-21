// File: Program.cs
// The main entry point for the application. Handles initialization, configuration,
// and execution of the core logic. This class is typically responsible for invoking
// services, managing dependencies, and coordinating application startup in a .NET
// console or WinForm environment.
// --------------------------------------------------------------------------------
// Author:     Patrik Eigenmann
// eMail:      p.eigenmann72@gmail.com
// GitHub:     https://github.com/PatrikEigenmann72/HelloWorld
// --------------------------------------------------------------------------------
// Change Log:
// Sun 2025-07-27 File created.                                                 Version: 00.01
// Sun 2025-08-10 Samael.HuginAndMunin.Debug.cs included into the Project.      Version: 00.02
// Sun 2025-08-10 Samael.HuginAndMunin.Log.cs included into the Project.        Version: 00.03
// Sun 2025-08-10 Samael.HuginAndMunin.Config.cs included into the Project.     Version: 00.04
// Thu 2025-08-21 Reorganized project structure and namespaces.                 Version: 00.05
// --------------------------------------------------------------------------------
using SHM = Samael.HuginAndMunin;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Initialize Samael Components
        SHM.Debug.Bitmask = SHM.DebugLevel.Error | SHM.DebugLevel.Verbose | SHM.DebugLevel.Info;
        SHM.Log.Bitmask = SHM.LogLevel.Error | SHM.LogLevel.Verbose;

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new ToneG.Gui.MainForm());
    }
}
