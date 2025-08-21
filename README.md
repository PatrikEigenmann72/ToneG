# ToneG

## About

ToneG was built for those moments in live sound when you need a signal generator now — not after googling console layers or digging through vendor-specific logic. Whether the desk buries its tools or you simply want something reliable on your own terms, ToneG has your back.
It’s your fallback when gear doesn’t cooperate. Plug in a 3.5mm to dual XLR, designate a tone and noise channel, and you're ready. No menus, no manuals, no second-guessing. This isn’t about not knowing the console—it’s about working smarter when time matters.
Tone3 lets you say: "I’ve got this."

## Folder Structure

The folders bin/ and obj/ with their artifacts *.jar, *.exe, *.dll, *.so, etc will not be tracked, except libraries in the lib/ folder.

```
Tone3/  
├── bin/
├── obj/  
├── .gitignore                       ← Contains the folders and files that will not be tracked.
├── AGenerator.cs                    ← Apstract Class as Prototyp for other Audio Generators.
├── AudioPlayer.cs                   ← The Player that makes the sonic generator audible.
├── LICENSE                          ← GNU General Public License
├── MainForm.cs                      ← MainForm logic and event handling.
├── MainForm.Designer.cs             ← MainForm GUI logic.
├── PinkNoiseGenerator.cs            ← An Audio Generator for Pink Noise.
├── Program.cs                       ← The Programs entry point.
├── Samael.HuginAndMunin.Config.cs   ← The Config class from the Samael.HuginAndMunin library.
├── Samael.HuginAndMunin.Debug.cs    ← The Debug class from the Samael.HuginAndMunin library.
├── Samael.HuginAndMunin.Log.cs      ← The Log class from the Samael.HuginAndMunin library.
├── SineWaveGenerator.cs             ← An Audio Generator for a Sine Wave.
└── README.md                        ← Project mission, usage, build instructions
```

## Author

My name is Patrik Eigenmann. After nine years as a professional software engineer, I switched careers to live sound. I still code on the side—projects like Tone3 keep me curious, challenged, and mentally sharp.
These tools aren’t polished products backed by teams. They’re personal explorations, built to deepen my understanding and share something useful with others. If you’d like to support what I do, you can donate to p.eigenmann@gmx.net via PayPal. I’d truly appreciate it.
Tone3 is and always will be free—licensed under the GNU Public License v3.0. Feel free to modify it, break it, rebuild it, or run it just as it is. Thanks for checking it out.
