using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PerlinNoise.Transformers {
    /// <summary>
    /// Takes in a NoiseField and outputs a Texture2D
    /// </summary>
    public class Texture2DTransformer : INoiseTransformer<float, Texture2D>, INoiseTransformer<Color, Texture2D> {
        /// <summary>
        /// Gets or sets the Graphics Device.  Required to create a new Texture2D.
        /// </summary>
        public GraphicsDevice Graphics { get; set; }

        public Texture2DTransformer(GraphicsDevice graphics) {
            Graphics = graphics;
        }

        /// <summary>
        /// Takes a field of color data and renders it to a Texture2D.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public Texture2D Transform(NoiseField<Color> field) {
            Color[] data = field.Flatten();
            Texture2D texture = new Texture2D(Graphics, field.Width, field.Height);

            texture.SetData<Color>(data);
            return texture;
        }

        /// <summary>
        /// Performs a transformation on a float field to a Texture2D.  This will render as
        /// a grayscale image.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public Texture2D Transform(NoiseField<float> field) {
            float[] data = field.Flatten();
            Color[] colorData = new Color[data.Length];

            for(int i = 0; i < data.Length; i++) {
                colorData[i] = new Color(data[i], data[i], data[i]);
            }

            Texture2D texture = new Texture2D(Graphics, field.Width, field.Height);

            texture.SetData<Color>(colorData);
            return texture;
        }
    }
}
