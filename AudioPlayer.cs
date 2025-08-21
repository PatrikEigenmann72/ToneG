// ----------------------------------------------------------------------------------------------
// AGenerator.cs - Abstract base class for audio generators.
// Defines a contract for 16-bit PCM signal sources (sine, pink, etc.).
// ----------------------------------------------------------------------------------------------
// Author:  Patrik Eigemann
// GitHub:  www.github.com/PatrikEigemann/Tone3
// ----------------------------------------------------------------------------------------------
// Change Log:
// Sat 2025-08-02 File translated to C# from Java.                                  Version 00.01
// Tue 2025-08-12 Samael.HuginAndMunin.Debug & Log implemented.                     Version 00.02
// Tue 2025-08-12 Introducing Getter Property ClassName and local variable msg.     Version 00.03
// ----------------------------------------------------------------------------------------------
namespace ToneG.Audio;

// .NET using directives
using System;
using System.Runtime.InteropServices;
using System.Threading;

// Samael.HuginAndMunin using directives
using SHM = Samael.HuginAndMunin;

/// <summary>
/// Audio player class for playback of generated audio signals.
/// </summary>
public class AudioPlayer
{
    /// <summary>
    /// Audio playback sample rate in Hz.
    /// </summary>
    private const int SAMPLE_RATE = 44100;

    /// <summary>
    /// Number of audio channels (1 = mono, 2 = stereo).
    /// </summary>
    private const int CHANNELS = 2;

    /// <summary>
    /// Bits per sample (16-bit PCM).
    /// </summary>
    private const int BITS_PER_SAMPLE = 16;

    /// <summary>
    /// Block alignment (bytes per sample * number of channels).
    /// </summary>
    private const int BLOCK_ALIGN = CHANNELS * (BITS_PER_SAMPLE / 8);

    /// <summary>
    /// Buffer size for audio playback (in bytes).
    /// </summary>
    private const int BUFFER_SIZE = 4096;

    /// <summary>
    /// Handle for the wave output device.
    /// </summary>
    private IntPtr waveOutHandle = IntPtr.Zero;

    /// <summary>
    /// Thread for audio playback.
    /// </summary>
    private Thread? audioThread;

    /// <summary>
    /// Flag indicating whether audio playback is running.
    /// </summary>
    private volatile bool running = false;

    /// <summary>
    /// Audio generators for the left channel.
    /// </summary>
    private AGenerator? leftGenerator;

    /// <summary>
    /// Audio generators for the right channel.
    /// </summary>
    private AGenerator? rightGenerator;

    /// <summary>
    /// Flag indicating whether the audio is mono (single channel).
    /// </summary>
    private bool isMono = false;

    /// <summary>
    /// This getter is just for the class name.
    /// </summary>
    public string ClassName
    {
        get { return "AudioPlayer"; }
    }

    /// <summary>
    /// Set the audio generator for mono playback.
    /// </summary>
    /// <param name="monoGenerator">The audio generator to use for both channels.</param>
    public void SetGenerator(AGenerator monoGenerator)
    {
        // Local messaging System.
        string msg = "";

        try
        {
            msg = "Setting up the Audio Player to mono generator!";
            SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, msg, ClassName);
            SHM.Log.WriteLine(SHM.LogLevel.Verbose, msg, ClassName);
            leftGenerator = monoGenerator;
            rightGenerator = monoGenerator;
            isMono = true;
        }
        catch (ArgumentNullException ex)
        {
            SHM.Debug.WriteException(ex);
            SHM.Log.WriteException(ex);
        }
    }

    /// <summary>
    /// Set the audio generators for stereo playback.
    /// </summary>
    /// <param name="left">The audio generator for the left channel.</param>
    /// <param name="right">The audio generator for the right channel.</param>
    public void SetGenerators(AGenerator left, AGenerator right)
    {
        string msg = "";
        try
        {
            msg = "Setting up the Audio Player to stereo generators!";
            SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, msg, ClassName);
            SHM.Log.WriteLine(SHM.LogLevel.Verbose, msg, ClassName);

            leftGenerator = left;
            rightGenerator = right;
            isMono = false;
        }
        catch (ArgumentNullException ex)
        {
            SHM.Debug.WriteException(ex);
            SHM.Log.WriteException(ex);
        }
    }

    /// <summary>
    /// Start audio playback.
    /// </summary>
    public void Start()
    {
        string msg = "";
        if (running || leftGenerator == null || rightGenerator == null)
        {
            msg = "Something went wrong. Either running is set to true, or one of the generators is null.";
            SHM.Debug.WriteLine(SHM.DebugLevel.Error, msg, ClassName);
            SHM.Log.WriteLine(SHM.LogLevel.Error, msg, ClassName);
            return;
        }

        msg = "Setting running flag to true.";
        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, msg, ClassName);
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, msg, ClassName);
        running = true;

        WAVEFORMATEX format = new WAVEFORMATEX
        {
            wFormatTag = 1, // PCM
            nChannels = CHANNELS,
            nSamplesPerSec = SAMPLE_RATE,
            wBitsPerSample = BITS_PER_SAMPLE,
            nBlockAlign = (ushort)BLOCK_ALIGN,
            nAvgBytesPerSec = SAMPLE_RATE * BLOCK_ALIGN,
            cbSize = 0
        };

        msg = "Opening waveOut device.";
        SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, msg, ClassName);
        SHM.Log.WriteLine(SHM.LogLevel.Verbose, msg, ClassName);
        waveOutOpen(out waveOutHandle, 0xFFFFFFFF, ref format, IntPtr.Zero, IntPtr.Zero, 0x00000000);

        audioThread = new Thread(() =>
        {
            int bufferSize = SAMPLE_RATE * 4 / 10; // 100ms stereo buffer
            int numBuffers = 4;
            IntPtr[] bufferPtrs = new IntPtr[numBuffers];
            WAVEHDR[] headers = new WAVEHDR[numBuffers];

            for (int i = 0; i < numBuffers; i++)
            {
                bufferPtrs[i] = Marshal.AllocHGlobal(bufferSize);
                headers[i] = new WAVEHDR
                {
                    lpData = bufferPtrs[i],
                    dwBufferLength = (uint)bufferSize,
                    dwFlags = 0,
                    dwLoops = 0
                };

                msg = $"Preparing header {i + 1} with buffer size {bufferSize} bytes.";
                SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, msg, ClassName);
                SHM.Log.WriteLine(SHM.LogLevel.Verbose, msg, ClassName);

                waveOutPrepareHeader(waveOutHandle, ref headers[i], Marshal.SizeOf(typeof(WAVEHDR)));
            }

            int currentBuffer = 0;

            while (running)
            {
                msg = $"Generating audio data for buffer {currentBuffer + 1}.";
                SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, msg, ClassName);
                SHM.Log.WriteLine(SHM.LogLevel.Verbose, msg, ClassName);

                byte[] audioData = new byte[bufferSize];

                for (int i = 0; i < bufferSize; i += 4)
                {
                    short leftSample = leftGenerator.NextSample();
                    short rightSample = isMono ? leftSample : rightGenerator.NextSample();
                    msg = $"Generated audio samples - Left: {leftSample}, Right: {rightSample}.";
                    SHM.Debug.WriteLine(SHM.DebugLevel.Verbose, msg, ClassName);
                    SHM.Log.WriteLine(SHM.LogLevel.Verbose, msg, ClassName);

                    audioData[i] = (byte)(leftSample & 0xFF);
                    audioData[i + 1] = (byte)((leftSample >> 8) & 0xFF);
                    audioData[i + 2] = (byte)(rightSample & 0xFF);
                    audioData[i + 3] = (byte)((rightSample >> 8) & 0xFF);
                }

                Marshal.Copy(audioData, 0, bufferPtrs[currentBuffer], bufferSize);
                waveOutWrite(waveOutHandle, ref headers[currentBuffer], Marshal.SizeOf(typeof(WAVEHDR)));

                currentBuffer = (currentBuffer + 1) % numBuffers;
                Thread.Sleep(5); // allows audio to flush without overfilling
            }

            // Cleanup
            for (int i = 0; i < numBuffers; i++)
            {
                waveOutUnprepareHeader(waveOutHandle, ref headers[i], Marshal.SizeOf(typeof(WAVEHDR)));
                Marshal.FreeHGlobal(bufferPtrs[i]);
            }

            waveOutClose(waveOutHandle);
        });

        // Set thread properties
        audioThread.Name = "ToneG-AudioEngine";
        audioThread.IsBackground = true;
        audioThread.Priority = ThreadPriority.AboveNormal;
        audioThread.Start();
    }

    /// <summary>
    /// Stop audio playback.
    /// </summary>
    public void Stop()
    {
        //MessageBox.Show("Stopping Audio!", "Debug Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        running = false;

        if (audioThread != null && audioThread.IsAlive)
        {
            audioThread.Join(); // Wait for the thread to finish
            audioThread = null; // Cleanup
        }

        // You can also close waveOutHandle here if needed
    }

    /// <summary>
    /// Wave format information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    private struct WAVEFORMATEX
    {
        public ushort wFormatTag;
        public ushort nChannels;
        public uint nSamplesPerSec;
        public uint nAvgBytesPerSec;
        public ushort nBlockAlign;
        public ushort wBitsPerSample;
        public ushort cbSize;
    }

    /// <summary>
    /// Wave header information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    private struct WAVEHDR
    {
        public IntPtr lpData;
        public uint dwBufferLength;
        public uint dwBytesRecorded;
        public uint dwUser;
        public uint dwFlags;
        public uint dwLoops;
        public IntPtr lpNext;
        public uint reserved;
    }

    /// <summary>
    /// Wave header flags.
    /// </summary>
    private const uint WHDR_DONE = 0x00000001;

    /// <summary>
    /// Open a waveform output device.
    /// </summary>
    [DllImport("winmm.dll", SetLastError = true)]
    private static extern int waveOutOpen(out IntPtr hWaveOut, uint uDeviceID, ref WAVEFORMATEX lpFormat, IntPtr dwCallback, IntPtr dwInstance, uint dwFlags);

    /// <summary>
    /// Prepare a waveform header for playback.
    /// </summary>
    [DllImport("winmm.dll", SetLastError = true)]
    private static extern int waveOutPrepareHeader(IntPtr hWaveOut, ref WAVEHDR lpWaveHdr, int uSize);

    /// <summary>
    /// Write audio data to the output device.
    /// </summary>
    [DllImport("winmm.dll", SetLastError = true)]
    private static extern int waveOutWrite(IntPtr hWaveOut, ref WAVEHDR lpWaveHdr, int uSize);

    /// <summary>
    /// Unprepare a waveform header for playback.
    /// </summary>
    [DllImport("winmm.dll", SetLastError = true)]
    private static extern int waveOutUnprepareHeader(IntPtr hWaveOut, ref WAVEHDR lpWaveHdr, int uSize);

    /// <summary>
    /// Close a waveform output device.
    /// </summary>
    [DllImport("winmm.dll", SetLastError = true)]
    private static extern int waveOutClose(IntPtr hWaveOut);
}
