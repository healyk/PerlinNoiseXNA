using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PerlinNoise.Filters {
    /// <summary>
    /// Filters a perlin noise by applying a gradient to the filter.
    /// </summary>
    public class LinearGradientColorFilter : INoiseFilter<float, Color> {
        /// <summary>
        /// Gets the start color for this gradient.
        /// </summary>
        public Color StartColor { get; set; }

        /// <summary>
        /// Gets the end color for this gradient.
        /// </summary>
        public Color EndColor { get; set; }

        /// <summary>
        /// Gets or sets how much of the Start color to use percentage-wise.  Needs to be between 1.0 and 0.0.
        /// 1.0 is the default.
        /// </summary>
        public float StartPercentage { get; set; }

        public LinearGradientColorFilter() {
            StartColor = Color.Black;
            EndColor = Color.Red;

            StartPercentage = 1.0f;
        }

        /// <summary>
        /// Performs the gradient.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public NoiseField<Color> Filter(NoiseField<float> field) {
            NoiseField<Color> result = new NoiseField<Color>(field.Width, field.Height);

            for(int x = 0; x < field.Width; x++) {
                for(int y = 0; y < field.Height; y++) {
                    float t = field.Field[x, y];
                    float u = StartPercentage - t;
                    t *= StartPercentage;

                    result.Field[x, y] = 
                        new Color((int)(u * StartColor.R + t * EndColor.R),
                                  (int)(u * StartColor.G + t * EndColor.G),
                                  (int)(u * StartColor.B + t * EndColor.B));
                }
            }

            return result;
        }
    }
}
