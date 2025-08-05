using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ToneG.Audio
{
    public class AudioPlayer
    {
        private const int SAMPLE_RATE = 44100;
        private const int CHANNELS = 2;
        private const int BITS_PER_SAMPLE = 16;
        private const int BLOCK_ALIGN = CHANNELS * (BITS_PER_SAMPLE / 8);
        private const int BUFFER_SIZE = 4096;

        private IntPtr waveOutHandle = IntPtr.Zero;
        private Thread? audioThread;
        private volatile bool running = false;

        private AGenerator? leftGenerator;
        private AGenerator? rightGenerator;
        private bool isMono = false;

        public void SetGenerator(AGenerator monoGenerator)
        {
            leftGenerator = monoGenerator;
            rightGenerator = monoGenerator;
            isMono = true;
        }

        public void SetGenerators(AGenerator left, AGenerator right)
        {
            leftGenerator = left;
            rightGenerator = right;
            isMono = false;
        }

        public void Start()
        {
            if (running || leftGenerator == null || rightGenerator == null)
            {
                return;
            }

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
                    waveOutPrepareHeader(waveOutHandle, ref headers[i], Marshal.SizeOf(typeof(WAVEHDR)));
                }

                int currentBuffer = 0;

                while (running)
                {
                    byte[] audioData = new byte[bufferSize];

                    for (int i = 0; i < bufferSize; i += 4)
                    {
                        short leftSample = leftGenerator.NextSample();
                        short rightSample = isMono ? leftSample : rightGenerator.NextSample();

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

            audioThread.Name = "ToneG-AudioEngine";
            audioThread.IsBackground = true;
            audioThread.Priority = ThreadPriority.AboveNormal;
            audioThread.Start();
        }

        /// <summary>
        /// 
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

        private const uint WHDR_DONE = 0x00000001;

        [DllImport("winmm.dll", SetLastError = true)]
        private static extern int waveOutOpen(out IntPtr hWaveOut, uint uDeviceID, ref WAVEFORMATEX lpFormat, IntPtr dwCallback, IntPtr dwInstance, uint dwFlags);

        [DllImport("winmm.dll", SetLastError = true)]
        private static extern int waveOutPrepareHeader(IntPtr hWaveOut, ref WAVEHDR lpWaveHdr, int uSize);

        [DllImport("winmm.dll", SetLastError = true)]
        private static extern int waveOutWrite(IntPtr hWaveOut, ref WAVEHDR lpWaveHdr, int uSize);

        [DllImport("winmm.dll", SetLastError = true)]
        private static extern int waveOutUnprepareHeader(IntPtr hWaveOut, ref WAVEHDR lpWaveHdr, int uSize);

        [DllImport("winmm.dll", SetLastError = true)]
        private static extern int waveOutClose(IntPtr hWaveOut);
    }
}
