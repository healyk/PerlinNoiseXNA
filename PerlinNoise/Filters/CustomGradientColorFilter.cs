using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PerlinNoise.Filters {
    /// <summary>
    /// Produces a custom gradient from a range of Colors.  This can be used to 
    /// add gradient ranges to an image.  A example would be taking all values between 0.4 and 0.5 and
    /// applying a red graident to them.
    /// </summary>
    public class CustomGradientColorFilter : INoiseFilter<float, Color> {
        public struct ColorPair {
            public float Start;
            public float End;
            public Color Color;

            public ColorPair(float start, float end, Color color) {
                this.Start = start;
                this.End = end;
                this.Color = color;
            }

            public bool InRange(float val) {
                return val < End && val >= Start;
            }

            public override string ToString() {
                return Color + "[" + Start + ", " + End + ")";
            }
        }

        /// <summary>
        /// Gets or sets the list of colors.
        /// </summary>
        public List<ColorPair> Colors { get; private set; }

        /// <summary>
        /// Constructs a new CustomGradientColorFilter.
        /// </summary>
        public CustomGradientColorFilter() {
            Colors = new List<ColorPair>();
        }

        /// <summary>
        /// Filters the NoiseField by applying the Gradient values.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public NoiseField<Color> Filter(NoiseField<float> field) {
            NoiseField<Color> result = new NoiseField<Color>(field.Width, field.Height);

            for(int x = 0; x < field.Width; x++) {
                for(int y = 0; y < field.Height; y++) {
                    float fieldValue = field.Field[x, y];

                    foreach(ColorPair pair in Colors) {
                        if(pair.InRange(fieldValue)) {
                            float colorMultipler = fieldValue / pair.End;

                            result.Field[x, y] =
                                new Color((int)(pair.Color.R * colorMultipler),
                                          (int)(pair.Color.G * colorMultipler),
                                          (int)(pair.Color.B * colorMultipler));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Adds a new Gradient value.
        /// </summary>
        /// <param name="start">The beginning of the field values to color.</param>
        /// <param name="end">The end of the field values to color.</param>
        /// <param name="color">The color of the gradient</param>
        public void AddColorPoint(float start, float end, Color color) {
            ColorPair pair = new ColorPair(start, end, color);

            Colors.Add(pair);
        }
    }
}