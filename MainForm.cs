namespace ToneG.Gui;

using ToneG.Audio;

///<summary>
///
///</summary>
public partial class MainForm : Form
{
    /// <summary>
    /// Nestled enumeration
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// 
        /// </summary>
        PINK = 1,

        /// <summary>
        /// 
        /// </summary>
        SINE = 2,

        /// <summary>
        /// 
        /// </summary>
        SPLIT = 3
    }

    /// <summary>
    /// 
    /// </summary>
    private Mode whatIsPlaying = Mode.SINE;

    /// <summary>
    /// 
    /// </summary>
    private bool isRunning = false;

    /// <summary>
    /// 
    /// </summary>
    private int frequency = 440;

    /// <summary>
    /// 
    /// </summary>
    private AGenerator? leftChannel;

    /// <summary>
    /// 
    /// </summary>
    private AGenerator? rightChannel;

    /// <summary>
    /// 
    /// </summary>
    private AudioPlayer audioPlayer = new AudioPlayer();


    ///<summary>
    ///
    ///</summary>
    public MainForm()
    {
        InitializeComponent();

        // Set the Slider to 440 hz, because of undetectable behavior.
        frequencySlider.Value = frequency;
        frequencyDisplay.Text = $"Frequency: {frequency} Hz";
        FrequencySlider_ValueChanged(null, EventArgs.Empty);

        sineButton.Checked = true;

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
        if (keyData == (Keys.Control | Keys.Q))
        {
            this.Close();
            return true;
        }

        // ⛔ Skip frequency changes if slider is disabled
        if (frequencySlider.Enabled)
        {

            int step = keyData.HasFlag(Keys.Shift) ? 300 : 10;

            if (keyData == Keys.Right || keyData == (Keys.Shift | Keys.Right))
            {
                frequencySlider.Value = Math.Min(frequencySlider.Maximum, frequencySlider.Value + step);
                frequencyDisplay.Text = $"Frequency: {frequencySlider.Value} Hz";
                FrequencySlider_ValueChanged(null, EventArgs.Empty);
                return true;
            }

            if (keyData == Keys.Left || keyData == (Keys.Shift | Keys.Left))
            {
                frequencySlider.Value = Math.Max(frequencySlider.Minimum, frequencySlider.Value - step);
                frequencyDisplay.Text = $"Frequency: {frequencySlider.Value} Hz";
                FrequencySlider_ValueChanged(null, EventArgs.Empty);
                return true;
            }
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        //MessageBox.Show("Shutting down Application!", "Debug Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Stop audio and clean up resources
        StopAudio();

        // Optional: log exit or trigger other shutdown steps here

        base.OnFormClosing(e); // Don't forget to call the base!
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ToggleButton_Click(object? sender, EventArgs e)
    {
        isRunning = !isRunning;

        toggleButton.Text = isRunning ? "Stop" : "Run";

        // Optional: trigger audio start/stop logic
        if (isRunning)
        {
            StartAudio(); // Your custom method for playback
        }
        else
        {
            StopAudio();  // Your custom method to halt playback
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FrequencySlider_ValueChanged(object? sender, EventArgs e)
    {
        frequency = frequencySlider.Value;
        frequencyDisplay.Text = $"Frequency: {frequency} Hz";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void sineButton_CheckedChanged(object? sender, EventArgs e)
    {
        if (sineButton.Checked)
        {
            whatIsPlaying = Mode.SINE;
            frequencySlider.Enabled = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void pinkButton_CheckedChanged(object? sender, EventArgs e)
    {
        if (pinkButton.Checked)
        {
            whatIsPlaying = Mode.PINK;
            frequencySlider.Enabled = false;
        }

        // 👇 Force focus elsewhere
        this.ActiveControl = frequencySlider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void splitButton_CheckedChanged(object? sender, EventArgs e)
    {
        if (splitButton.Checked)
        {
            whatIsPlaying = Mode.SPLIT;
            frequencySlider.Enabled = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void StartAudio()
    {
        switch (whatIsPlaying)
        {
            case Mode.PINK:
                leftChannel = new PinkNoiseGenerator();
                rightChannel = leftChannel;
                break;

            case Mode.SINE:
                var sineGen = new SineWaveGenerator();
                sineGen.SetFrequency(frequency);
                leftChannel = sineGen;
                rightChannel = sineGen;
                break;

            case Mode.SPLIT:
                leftChannel = new SineWaveGenerator();
                ((SineWaveGenerator)leftChannel).SetFrequency(frequency);
                rightChannel = new PinkNoiseGenerator();
                break;
        }

        audioPlayer.SetGenerators(leftChannel, rightChannel);
        audioPlayer.Start();
        isRunning = true;
    }

    /// <summary>
    /// 
    /// </summary>
    private void StopAudio()
    {
        audioPlayer.Stop();
        isRunning = false;
    }

}
