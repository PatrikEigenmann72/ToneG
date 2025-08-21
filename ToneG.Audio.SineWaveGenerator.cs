// --------------------------------------------------------------------------------------
// SineWaveGenerator.cs - Real-time sine wave sample generator for test tone output. This
// class produces continuous sine tones based on a set frequency and shared sample rate.
// It is stateless apart from phase tracking and is designed for live synthesis inside the 
// Tone3 audio thread without needing pre-buffered lookup tables.
//
// SineWaveGenerator extends AGenerator to remain polymorphically compatible with the audio 
// backend. AudioPlayer doesn’t care what generator is in use—as long as nextSample() returns 
// a valid 16-bit PCM frame. This pattern minimizes interdependence and makes the signal chain 
// extensible for future tone types without modifying playback logic.
//
// Phase accumulation is performed in double-precision to ensure smooth cycling and prevent 
// perceptual drift over long runtimes. When 2π is exceeded, phase wraps around to preserve
// continuity without overflow.
// --------------------------------------------------------------------------------------
// Author:  Patrik Eigemann  
// eMail:   p.eigenmann@gmx.net  
// GitHub:  www.github.com/PatrikEigemann/ToneG  
// --------------------------------------------------------------------------------------
// Change Log:
// Sun 2025-06-29 File created and sine oscillator implemented.             Version 00.01
// --------------------------------------------------------------------------------------
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