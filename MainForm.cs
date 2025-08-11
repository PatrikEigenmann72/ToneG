// File: MainForm.cs
// The MainForm is the primary user interface for this audio application. ToneG is a small Tone
// generator born out of the need to get a fast result. Instead of digging deep into the software
// of a mixing board, ToneG provides a straight-forward interface for generating a sine wave, pink
// noise, or both at the same time. Run ToneG on a computer and hook up an 1/8" to dual xlr on the
// computers audio output, and label the left channel as Sine and right channel as Pink. You're
// good to go! This is a must audio tool for the engineer without compromise. Have fun on your show.
// -------------------------------------------------------------------------------------------------
// Author:     Patrik Eigenmann
// eMail:      p.eigenmann72@gmail.com
// GitHub:     https://github.com/PatrikEigenmann72/HelloWorld
// -------------------------------------------------------------------------------------------------
// Change Log:
// Sun 2025-07-27 File created.                                                       Version: 00.01
// Sat 2025-08-09 File updated to include debug functionality.                        Version: 00.02
// Sat 2025-08-09 File updated to include logging functionality.                      Version: 00.03
// Sun 2025-08-10 File updated to include Config functionality.                       Version: 00.04
// -------------------------------------------------------------------------------------------------
namespace ToneG.Gui;

// Importing Samael framework
using SHM = Samael.HuginAndMunin;

// Importing ToneG proprietary class library
using ToneG.Audio;

///<summary>
/// The MainForm class serves as the primary user interface for this application. This partial
/// class is responsible for managing the event handling and user interactions. With version 00.04,
/// debugging, logging and config functionality have been integrated.
///</summary>
public partial class MainForm : Form
{
    /// <summary>
    /// With this nestled enumeration the modes of the application are defined. We can switch between
    /// different audio generation modes easily. The available modes are:
    /// Sine - which means the software generates a sine wave as a stereo signal.
    /// Pink - which means the software generates pink noise as a stereo signal.
    /// Split - which means the software generates both signals, with sine on the left and pink on the right.
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Pink noise generator mode. The software generates pink noise as a stereo signal. This mode
        /// is useful for testing speakers and creating ambient soundscapes.
        /// </summary>
        PINK = 1,

        /// <summary>
        /// Sine wave generator mode. The software generates a sine wave as a stereo signal. This mode
        /// is useful to verify the speakers frequency range. As an example, a subwoofer should response
        /// to the low frequencies below 200 Hz. Top usually should respond to frequencies above 60 Hz.
        /// </summary>
        SINE = 2,

        /// <summary>
        /// The split mode is one of the strengths of this application. It allows the sine wave signal to
        /// be on the left channel and the pink noise on the right channel. Very useful to have both signals
        /// available at the same time for comparison and testing.
        /// </summary>
        SPLIT = 3
    }

    /// <summary>
    /// Defines in which mode the software is currently operating. I decided to start the software in
    /// split mode, that allows for more flexibility in audio testing. So, we start off on the left
    /// channel with sine wave and on the right channel with pink noise.
    /// </summary>
    private Mode whatIsPlaying = Mode.SPLIT;

    /// <summary>
    /// Indicates whether the audio generation is currently running. We start off with the no signal
    /// at the moment. If I need noise generated, I will press the button to run the audio.
    /// </summary>
    private bool isRunning = false;

    /// <summary>
    /// The current frequency setting for the audio generation. Only relevant if sine wave mode is active.
    /// We start off with an A4 note which means 440 Hz. Maybe I change that later, or have it running at
    /// a lower frequency, not to damage a subwoofer, if the test requires it. Or maybe I create a hover
    /// label over the button "Run" to alert with a message: "Please make sure your frequency setting is
    /// appropriate for your type of speaker!"
    /// </summary>
    private int frequency = 440;

    /// <summary>
    /// Audio generator for the left channel.
    /// </summary>
    private AGenerator? leftChannel;

    /// <summary>
    /// Audio generator for the right channel.
    /// </summary>
    private AGenerator? rightChannel;

    /// <summary>
    /// Audio player for the generated audio signals.
    /// </summary>
    private AudioPlayer audioPlayer = new AudioPlayer();


    ///<summary>
    /// Constructor for the MainForm class. Here is where the magic happens. In C# in IDE's
    /// with a WYSIWYG designer, the MainForm is separated into a form and designer class. The form
    /// handles the user interactions and the designer handles the layout and appearance.
    ///</summary>
    public MainForm()
    {
        InitializeComponent();

        SHM.Debug.WriteLine(SHM.DebugLevel.Info, "Frequency Slider initialized!", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Info, "Frequency Slider initialized!", "MainForm");
        // Set the Slider to 440 hz, because of undetectable behavior.
        frequencySlider.Value = frequency;
        frequencyDisplay.Text = $"Frequency: {frequency} Hz";
        FrequencySlider_ValueChanged(null, EventArgs.Empty);

        SHM.Debug.WriteLine(SHM.DebugLevel.Info, "Make sure Sine Button is checked!", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Info, "Make sure Sine Button is checked!", "MainForm");
        sineButton.Checked = true;

        SHM.Debug.WriteLine(SHM.DebugLevel.Info, "Create all the event handlers!", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Info, "Create all the event handlers!", "MainForm");
        // After initialize let's take care of some event handling.
        toggleButton.Click += ToggleButton_Click;
        frequencySlider.ValueChanged += FrequencySlider_ValueChanged;
        sineButton.CheckedChanged += sineButton_CheckedChanged;
        pinkButton.CheckedChanged += pinkButton_CheckedChanged;
        splitButton.CheckedChanged += splitButton_CheckedChanged;

    }

    /// <summary>
    /// Overrides the default command key processing to intercept keyboard input
    /// before it's handed off to the focused control. This allows the form to handle
    /// global shortcuts and custom keystroke logic across the UI.
    /// </summary>
    /// <param name="msg">
    /// The Windows message encapsulating the input event. Typically represents a key press
    /// such as WM_KEYDOWN or WM_SYSKEYDOWN.
    /// </param>
    /// <param name="keyData">
    /// A bitwise combination of <see cref="Keys"/> values that identifies the specific key
    /// and any modifiers (Ctrl, Alt, Shift) involved in the input.
    /// </param>
    /// <returns>
    /// <c>true</c> if the key event was handled and should not continue to other controls;
    /// otherwise, <c>false</c> to allow normal key processing behavior.
    /// </returns>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Key {keyData} is pressed!", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Info, $"Key {keyData} is pressed!", "MainForm");
        if (keyData == (Keys.Control | Keys.Q))
        {
            SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Application will close.", "MainForm");
            SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Application will close.", "MainForm");
            this.Close();
            return true;
        }

        // Skip frequency changes if slider is disabled
        if (frequencySlider.Enabled)
        {
            SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Calculating frequency step.", "MainForm");
            SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Calculating frequency step.", "MainForm");
            int step = keyData.HasFlag(Keys.Shift) ? 300 : 10;

            if (keyData == Keys.Right || keyData == (Keys.Shift | Keys.Right))
            {
                SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Increasing frequency.", "MainForm");
                SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Increasing frequency.", "MainForm");
                frequencySlider.Value = Math.Min(frequencySlider.Maximum, frequencySlider.Value + step);
                frequencyDisplay.Text = $"Frequency: {frequencySlider.Value} Hz";
                FrequencySlider_ValueChanged(null, EventArgs.Empty);
                return true;
            }

            if (keyData == Keys.Left || keyData == (Keys.Shift | Keys.Left))
            {
                SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Decreasing frequency.", "MainForm");
                SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Decreasing frequency.", "MainForm");
                frequencySlider.Value = Math.Max(frequencySlider.Minimum, frequencySlider.Value - step);
                frequencyDisplay.Text = $"Frequency: {frequencySlider.Value} Hz";
                FrequencySlider_ValueChanged(null, EventArgs.Empty);
                return true;
            }

        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    /// This event is triggered when the form is closing. In here we can dispose of all resources
    /// allocated, stopping any ongoing playbacks. Making sure everything is cleaned up properly
    /// before we close the application for good.
    /// </summary>
    /// <param name="e">The event argument on form closing.</param>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        SHM.Debug.WriteLine(SHM.DebugLevel.Info, "Form is closing, stopping audio.", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Info, "Form is closing, stopping audio.", "MainForm");
        // Stop audio and clean up resources
        StopAudio();

        base.OnFormClosing(e); // Don't forget to call the base!
    }

    /// <summary>
    /// With this event, we signalize that the Run/Stop button is clicked. Every click toggles the
    /// playback state. And resets the text within the button accordingly.
    /// </summary>
    /// <param name="sender">Who was sending the event.</param>
    /// <param name="e">Event argument for the button click.</param>
    private void ToggleButton_Click(object? sender, EventArgs e)
    {
        isRunning = !isRunning;
        SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Toggle button clicked. Current state: {isRunning}", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Info, $"Toggle button clicked. Current state: {isRunning}", "MainForm");

        toggleButton.Text = isRunning ? "Stop" : "Run";

        SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Toggle button text changed to: {toggleButton.Text}", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Info, $"Toggle button text changed to: {toggleButton.Text}", "MainForm");

        // Optional: trigger audio start/stop logic
        if (isRunning)
        {

            SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Starting audio playback.", "MainForm");
            SHM.Log.WriteLine(SHM.LogLevel.Info, $"Starting audio playback.", "MainForm");
            StartAudio(); // Your custom method for playback
        }
        else
        {
            SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Stopping audio playback.", "MainForm");
            SHM.Log.WriteLine(SHM.LogLevel.Info, $"Stopping audio playback.", "MainForm");
            StopAudio();  // Your custom method to halt playback
        }
    }

    /// <summary>
    /// This event is triggered when the frequency slider changes. This visual change has a bunch
    /// of implications and chain reactions in the audio playback. A change of the slider means,
    /// a change in the pitch of the audio signal if sine mode is active. It means also that the
    /// displayed frequency needs to be updated accordingly.
    /// </summary>
    /// <param name="sender">Who was sending the event.</param>
    /// <param name="e">Event argument for the slider value change.</param>
    private void FrequencySlider_ValueChanged(object? sender, EventArgs e)
    {
        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, $"Frequency slider value changed: {frequencySlider.Value}", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, $"Frequency slider value changed: {frequencySlider.Value}", "MainForm");
        frequency = frequencySlider.Value;
        frequencyDisplay.Text = $"Frequency: {frequency} Hz";
    }

    /// <summary>
    /// A click on the sine radio button triggers this event. The event ensures that the whole
    /// application will switch to sine mode.
    /// </summary>
    /// <param name="sender">Who was triggering the event.</param>
    /// <param name="e">Event argument for the radio button state change.</param>
    private void sineButton_CheckedChanged(object? sender, EventArgs e)
    {
        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Sine button checked state changed.", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Sine button checked state changed.", "MainForm");
        if (sineButton.Checked)
        {
            whatIsPlaying = Mode.SINE;
            frequencySlider.Enabled = true;
        }
    }

    /// <summary>
    /// This event is triggered when the pink radio button changes its checked state. The event ensures that the whole
    /// application will switch to pink noise mode.
    /// </summary>
    /// <param name="sender">Who was triggering the event.</param>
    /// <param name="e">Event argument for the radio button state change.</param>
    private void pinkButton_CheckedChanged(object? sender, EventArgs e)
    {
        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Pink button checked state changed.", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Pink button checked state changed.", "MainForm");
        if (pinkButton.Checked)
        {
            whatIsPlaying = Mode.PINK;
            frequencySlider.Enabled = false;
        }

        // 👇 Force focus elsewhere
        this.ActiveControl = frequencySlider;
    }

    /// <summary>
    /// This event is triggered when the split radio button changes its checked state. The event ensures that the whole
    /// application will switch to split mode.
    /// </summary>
    /// <param name="sender">Who was triggering the event.</param>
    /// <param name="e">Event argument for the radio button state change.</param>
    private void splitButton_CheckedChanged(object? sender, EventArgs e)
    {
        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, "Split button checked state changed.", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, "Split button checked state changed.", "MainForm");
        if (splitButton.Checked)
        {
            whatIsPlaying = Mode.SPLIT;
            frequencySlider.Enabled = true;
        }
    }

    /// <summary>
    /// This method is a tricky part of the application. Starting the playback of audio signal is
    /// a multithreaded operation and needs to be handled carefully. If the run button is clicked,
    /// the audio playback must be started in a separate thread to avoid blocking the UI.
    /// </summary>
    private void StartAudio()
    {
        SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Starting audio playback.", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Info, $"Starting audio playback.", "MainForm");
        switch (whatIsPlaying)
        {
            case Mode.PINK:
                SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Starting pink noise playback.", "MainForm");
                SHM.Log.WriteLine(SHM.LogLevel.Info, $"Starting pink noise playback.", "MainForm");
                leftChannel = new PinkNoiseGenerator();
                rightChannel = leftChannel;
                break;

            case Mode.SINE:
                SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Starting sine wave playback.", "MainForm");
                SHM.Log.WriteLine(SHM.LogLevel.Info, $"Starting sine wave playback.", "MainForm");
                var sineGen = new SineWaveGenerator();
                sineGen.SetFrequency(frequency);
                leftChannel = sineGen;
                rightChannel = sineGen;
                break;

            case Mode.SPLIT:
                SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Starting split audio playback.", "MainForm");
                SHM.Log.WriteLine(SHM.LogLevel.Info, $"Starting split audio playback.", "MainForm");
                leftChannel = new SineWaveGenerator();
                ((SineWaveGenerator)leftChannel).SetFrequency(frequency);
                rightChannel = new PinkNoiseGenerator();
                break;
        }

        SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Audio player set with generators.", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Info, $"Audio player set with generators.", "MainForm");
        audioPlayer.SetGenerators(leftChannel, rightChannel);

        SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Starting audio playback.", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Info, $"Starting audio playback.", "MainForm");
        audioPlayer.Start();
        isRunning = true;
    }

    /// <summary>
    /// Stopping the audio playback happens on two occasions: when the stop button is clicked
    /// or when the application is closed, and the audio playback is still running. Because
    /// of the multithreaded nature of audio playback, we need to ensure that we stop the playback
    /// gracefully, and also set the isRunning flag back to false.
    /// </summary>
    private void StopAudio()
    {
        SHM.Debug.WriteLine(SHM.DebugLevel.Info, $"Stopping audio playback.", "MainForm");
        SHM.Log.WriteLine(SHM.LogLevel.Info, $"Stopping audio playback.", "MainForm");
        audioPlayer.Stop();
        isRunning = false;
    }
}