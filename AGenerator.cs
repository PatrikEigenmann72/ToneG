// --------------------------------------------------------------------------------------
// AGenerator.cs - Abstract base class for audio generators.
// Defines a contract for 16-bit PCM signal sources (sine, pink, etc.).
// --------------------------------------------------------------------------------------
// Author:  Patrik Eigemann
// GitHub:  www.github.com/PatrikEigemann/Tone3
// --------------------------------------------------------------------------------------
// Change Log:
// 2025-08-02 Sat File translated to C# from Java.                        Version 00.01
// --------------------------------------------------------------------------------------

namespace ToneG.Audio
{
    /// <summary>
    /// Abstract audio generator class. Test
    /// Produces 16-bit PCM samples for playback, decoupled from signal logic.
    /// </summary>
    public abstract class AGenerator
    {
        /// <summary>
        /// Audio playback sample rate in Hz. Default is 44100.
        /// </summary>
        protected int sampleRate = 44100;

        /// <summary>
        /// Set the internal sample rate for this generator.
        /// </summary>
        /// <param name="rate">Sample rate in Hz</param>
        public void SetSampleRate(int rate)
        {
            sampleRate = rate;
        }

        /// <summary>
        /// Generate the next 16-bit PCM audio sample.
        /// Subclasses must override this method.
        /// </summary>
        /// <returns>Next audio sample (range: -32768 to 32767)</returns>
        public abstract short NextSample();
    }
}