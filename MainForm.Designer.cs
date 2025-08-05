namespace ToneG.Gui;

/// <summary>
/// 
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


    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.frequencySlider = new System.Windows.Forms.TrackBar();
        this.frequencyDisplay = new System.Windows.Forms.Label();
        this.sineButton = new System.Windows.Forms.RadioButton();
        this.pinkButton = new System.Windows.Forms.RadioButton();
        this.splitButton = new System.Windows.Forms.RadioButton();
        this.toggleButton = new System.Windows.Forms.Button();
        this.controlPanel = new System.Windows.Forms.Panel();

        ((System.ComponentModel.ISupportInitialize)(this.frequencySlider)).BeginInit();
        this.controlPanel.SuspendLayout();
        this.SuspendLayout();

        // frequencyDisplay – taller for full font visibility
        this.frequencyDisplay.Location = new System.Drawing.Point(10, 10);
        this.frequencyDisplay.Size = new System.Drawing.Size(580, 60); // bumped from 40
        this.frequencyDisplay.Text = "Frequency: 440 Hz";
        this.frequencyDisplay.TextAlign = ContentAlignment.MiddleCenter;
        this.frequencyDisplay.Name = "frequencyDisplay";
        this.frequencyDisplay.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        this.frequencyDisplay.BackColor = Color.Yellow;

        // frequencySlider – unchanged
        this.frequencySlider.Location = new System.Drawing.Point(10, 85);
        this.frequencySlider.Minimum = 20;
        this.frequencySlider.Maximum = 20000;
        this.frequencySlider.TickFrequency = 1000;
        this.frequencySlider.Value = 440;
        this.frequencySlider.Size = new System.Drawing.Size(580, 45);
        this.frequencySlider.Name = "frequencySlider";

        // controlPanel – now taller
        this.controlPanel.Location = new System.Drawing.Point(10, 180);
        this.controlPanel.Size = new System.Drawing.Size(580, 90); // bumped from 50
        this.controlPanel.Name = "controlPanel";

        // Buttons inside controlPanel – re-centered vertically
        this.sineButton.Location = new System.Drawing.Point(0, 25);
        this.sineButton.Size = new System.Drawing.Size(100, 45);
        this.sineButton.Text = "Sine";
        this.sineButton.Name = "sineButton";

        this.pinkButton.Location = new System.Drawing.Point(120, 25);
        this.pinkButton.Size = new System.Drawing.Size(100, 45);
        this.pinkButton.Text = "Pink";
        this.pinkButton.Name = "pinkButton";

        this.splitButton.Location = new System.Drawing.Point(245, 25);
        this.splitButton.Size = new System.Drawing.Size(230, 45);
        this.splitButton.Text = "Sine L / Pink R";
        this.splitButton.Name = "splitButton";

        this.toggleButton.Location = new System.Drawing.Point(480, 25);
        this.toggleButton.Size = new System.Drawing.Size(100, 45);
        this.toggleButton.Text = "Run";
        this.toggleButton.Name = "toggleButton";

        this.controlPanel.Controls.Add(this.sineButton);
        this.controlPanel.Controls.Add(this.pinkButton);
        this.controlPanel.Controls.Add(this.splitButton);
        this.controlPanel.Controls.Add(this.toggleButton);

        // Window height adjusted to fit new layout
        this.ClientSize = new Size(600, 280); // enough to clear bottom
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog; // 👈 disables resizing
        this.MaximizeBox = false; // 👈 disables maximize button
        this.Text = "Tone Generator";
        this.Name = "MainForm";

        this.Controls.Add(this.frequencyDisplay);
        this.Controls.Add(this.frequencySlider);
        this.Controls.Add(this.controlPanel);

        ((System.ComponentModel.ISupportInitialize)(this.frequencySlider)).EndInit();
        this.controlPanel.ResumeLayout(false);
        this.ResumeLayout(false);
    }
    #endregion
}
