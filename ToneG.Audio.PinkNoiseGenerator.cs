// ----------------------------------------------------------------------------------------------
// PinkNoiseGenerator.cs - Sample-accurate pink noise generator using filtered white
// noise. This class implements a computationally efficient approximation of true 1/f
// noise, adapted from Paul Kellet’s method. Unlike white noise, which distributes equal
// energy per hertz, pink noise rolls off -3 dB per octave—aligning with human perception.
//
// This class is part of the polymorphic generator system driven by AGenerator.java.
// AudioPlayer doesn’t care what kind of sound it's playing—as long as nextSample() 
// returns valid PCM shorts, it’s happy. That design keeps signal logic isolated and
// extendable, with zero coupling to the playback engine.
//
// PinkNoiseGenerator is entirely self-contained and non-blocking. It’s optimized for
// real-time sample synthesis with no pre-buffering or lookup tables—just one lightweight
// method call per frame inside the audio thread.
// ----------------------------------------------------------------------------------------------
// Author:  Patrik Eigemann  
// eMail:   p.eigenmann@gmx.net  
// GitHub:  www.github.com/PatrikEigemann/ToneG  
// ----------------------------------------------------------------------------------------------
// Change Log:
// Sat 2025-08-02 File translated to C# from Java.                                  Version 00.01
// ----------------------------------------------------------------------------------------------
using System;

namespace ToneG.Audio
{
    /// <summary>
    /// Sample-accurate pink noise generator using filtered white noise.
    /// Based on Paul Kellet’s algorithm for 1/f noise.
    /// </summary>
    public class PinkNoiseGenerator : AGenerator
    {
        private readonly Random random = new Random();

        private double b0 = 0, b1 = 0, b2 = 0, b3 = 0, b4 = 0, b5 = 0, b6 = 0;

        /// <summary>
        /// Generate the next 16-bit PCM pink noise sample.
        /// </summary>
        /// <returns>Sample value in range [-32768, 32767]</returns>
        public override short NextSample()
        {
            double white = random.NextDouble() * 2.0 - 1.0;

            b0 = 0.99886 * b0 + white * 0.0555179;
            b1 = 0.99332 * b1 + white * 0.0750759;
            b2 = 0.96900 * b2 + white * 0.1538520;
            b3 = 0.86650 * b3 + white * 0.3104856;
            b4 = 0.55000 * b4 + white * 0.5329522;
            b5 = -0.7616 * b5 - white * 0.0168980;

            double pink = b0 + b1 + b2 + b3 + b4 + b5 + b6 + white * 0.5362;
            b6 = white * 0.115926;

            pink *= 0.11;
            pink = Math.Clamp(pink, -1.0, 1.0);

            return (short)(pink * short.MaxValue);
        }
    }
}