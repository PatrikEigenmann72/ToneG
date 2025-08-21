// File: MainForm.Designer.cs
// This is the partial designer class for the MainForm.cs. The Designer class is usually produced
// by a Wysiwyg Designer, but because I am using Visual Studio Code, I'll do it by hand
// and try to be as close to a Designer generated code as possible.
// -------------------------------------------------------------------------------------------------
// Author:     Patrik Eigenmann
// eMail:      p.eigenmann72@gmail.com
// GitHub:     https://github.com/PatrikEigenmann72/HelloWorld
// -------------------------------------------------------------------------------------------------
// Change Log:
// Sun 2025-07-27 File created.                                                       Version: 00.01
// Sat 2025-08-10 File updated to include debug functionality.                        Version: 00.02
// Sat 2025-08-10 File updated to include logging functionality.                      Version: 00.03
// Sun 2025-08-10 File updated to include Config functionality.                       Version: 00.04
// -------------------------------------------------------------------------------------------------
namespace ToneG.Gui;

// Samael framework usings
using SHM = Samael.HuginAndMunin;

/// <summary>
/// This is the partial designer class for the MainForm.cs. The Designer class is usually produced
/// by a Wysiwyg Designer, but because I am using Visual Studio Code, I'll do it by hand and try to
/// be as close to a Designer generated code as possible.
/// </summary>
partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Slider for selecting frequencies between 20 Hz and 20 KHz.
    /// </summary>
    /// 
    private TrackBar frequencySlider;

    /// <summary>
    /// Text label that shows the current frequency in Hz
    /// </summary>    
    private Label frequencyDisplay;

    /// <summary>
    /// Radio button to select Sine wave playback mode
    /// </summary>
    private RadioButton sineButton;

    /// <summary>
    /// Radio button to select Pink noise playback mode
    /// </summary>
    private RadioButton pinkButton;

    /// <summary>
    /// Radio button for Split mode: Sine Left / Pink Right
    /// </summary>
    private RadioButton splitButton;

    /// <summary>
    /// Big toggle button to start/stop audio playback
    /// </summary>
    private Button toggleButton;

    /// <summary>
    /// Panel that contains radio buttons and the toggle button
    /// </summary>
    private Panel controlPanel;

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "WinForm components are being initialized.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "WinForm components are being initialized.", "MainForm.Designer");
        this.components = new System.ComponentModel.Container();
        this.frequencySlider = new System.Windows.Forms.TrackBar();
        this.frequencyDisplay = new System.Windows.Forms.Label();
        this.sineButton = new System.Windows.Forms.RadioButton();
        this.pinkButton = new System.Windows.Forms.RadioButton();
        this.splitButton = new System.Windows.Forms.RadioButton();
        this.toggleButton = new System.Windows.Forms.Button();
        this.controlPanel = new System.Windows.Forms.Panel();

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Panels and Layouts are being suspended.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Panels and Layouts are being suspended.", "MainForm.Designer");
        ((System.ComponentModel.ISupportInitialize)(this.frequencySlider)).BeginInit();
        this.controlPanel.SuspendLayout();
        this.SuspendLayout();

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Frequency label initialized.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Frequency label initialized.", "MainForm.Designer");
        // frequencyDisplay – taller for full font visibility
        this.frequencyDisplay.Location = new System.Drawing.Point(10, 10);
        this.frequencyDisplay.Size = new System.Drawing.Size(580, 60); // bumped from 40
        this.frequencyDisplay.Text = "Frequency: 440 Hz";
        this.frequencyDisplay.TextAlign = ContentAlignment.MiddleCenter;
        this.frequencyDisplay.Name = "frequencyDisplay";
        this.frequencyDisplay.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        this.frequencyDisplay.BackColor = Color.Yellow;

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Frequency slider initialized.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Frequency slider initialized.", "MainForm.Designer");
        // frequencySlider – unchanged
        this.frequencySlider.Location = new System.Drawing.Point(10, 85);
        this.frequencySlider.Minimum = 20;
        this.frequencySlider.Maximum = 20000;
        this.frequencySlider.TickFrequency = 1000;
        this.frequencySlider.Value = 440;
        this.frequencySlider.Size = new System.Drawing.Size(580, 45);
        this.frequencySlider.Name = "frequencySlider";

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Control panel initialized.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Control panel initialized.", "MainForm.Designer");
        // controlPanel – now taller
        this.controlPanel.Location = new System.Drawing.Point(10, 180);
        this.controlPanel.Size = new System.Drawing.Size(580, 90); // bumped from 50
        this.controlPanel.Name = "controlPanel";

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Radio button sine created.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Radio button sine created.", "MainForm.Designer");
        // Buttons inside controlPanel – re-centered vertically
        this.sineButton.Location = new System.Drawing.Point(0, 25);
        this.sineButton.Size = new System.Drawing.Size(100, 45);
        this.sineButton.Text = "Sine";
        this.sineButton.Name = "sineButton";

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Radio button pink created.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Radio button pink created.", "MainForm.Designer");
        this.pinkButton.Location = new System.Drawing.Point(120, 25);
        this.pinkButton.Size = new System.Drawing.Size(100, 45);
        this.pinkButton.Text = "Pink";
        this.pinkButton.Name = "pinkButton";

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Radio button split created.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Radio button split created.", "MainForm.Designer");
        this.splitButton.Location = new System.Drawing.Point(245, 25);
        this.splitButton.Size = new System.Drawing.Size(230, 45);
        this.splitButton.Text = "Sine L / Pink R";
        this.splitButton.Name = "splitButton";

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Toggle button created.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Toggle button created.", "MainForm.Designer");
        this.toggleButton.Location = new System.Drawing.Point(480, 25);
        this.toggleButton.Size = new System.Drawing.Size(100, 45);
        this.toggleButton.Text = SHM.Config.Get("Btn.ToggleOff");
        this.toggleButton.Name = SHM.Config.Get("Btn.Toggle.Name");

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Components are getting added to the control panel.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Components are getting added to the control panel.", "MainForm.Designer");
        this.controlPanel.Controls.Add(this.sineButton);
        this.controlPanel.Controls.Add(this.pinkButton);
        this.controlPanel.Controls.Add(this.splitButton);
        this.controlPanel.Controls.Add(this.toggleButton);

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Window layout created.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Window layout created.", "MainForm.Designer");
        // Window height adjusted to fit new layout
        this.ClientSize = new Size(600, 280); // enough to clear bottom
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog; // 👈 disables resizing
        this.MaximizeBox = false; // 👈 disables maximize button
        string text = $"{SHM.Config.Get("App.Title")} - Version: {SHM.Config.Get("App.Version")}";
        this.Text = text;
        this.Name = SHM.Config.Get("App.Name");

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Controls are being added to the controls array.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Controls are being added to the controls array.", "MainForm.Designer");
        this.Controls.Add(this.frequencyDisplay);
        this.Controls.Add(this.frequencySlider);
        this.Controls.Add(this.controlPanel);

        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Finish updating layout.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Finish updating layout.", "MainForm.Designer");
        ((System.ComponentModel.ISupportInitialize)(this.frequencySlider)).EndInit();
        this.controlPanel.ResumeLayout(false);
        this.ResumeLayout(false);
    }
    #endregion

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Disposing resources.", "MainForm.Designer");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Disposing resources.", "MainForm.Designer");
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

}
