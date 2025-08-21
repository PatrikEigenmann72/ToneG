using System;

namespace ToneG.Audio
{
    /// <summary>
    /// Real-time sine wave generator for test tone output.
    /// Stateless except for phase tracking; sample-accurate and polymorphic.
    /// </summary>
    public class SineWaveGenerator : AGenerator
    {
        /// <summary>
        /// Target frequency in Hz. Volatile to support thread-safe updates.
        /// </summary>
        private double frequency = 440.0;

        /// <summary>
        /// Internal oscillator phase in radians.
        /// Advances with each sample frame and wraps around at 2π.
        /// </summary>
        private double phase = 0.0;

        /// <summary>
        /// Sets the frequency in Hz (e.g., 60.0 for sub test, 440.0 for A4).
        /// </summary>
        /// <param name="frequency">Desired playback frequency</param>
        public void SetFrequency(double frequency)
        {
            this.frequency = frequency;
        }

        /// <summary>
        /// Generates the next 16-bit PCM sine sample.
        /// </summary>
        /// <returns>Sample value between -32768 and 32767</returns>
        public override short NextSample()
        {
            double sample = Math.Sin(phase) * short.MaxValue;

            phase += 2 * Math.PI * frequency / sampleRate;

            if (phase >= 2 * Math.PI)
            {
                phase -= 2 * Math.PI;
            }

            return (short)sample;
        }
    }
}