using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PerlinNoise {
    /// <summary>
    /// Delegate for an algorithm that interpolates two points using t as the interpolation value.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public delegate float InterpolationAlgorithm(float a, float b, float t);

    /// <summary>
    /// The PerlinNoiseGenerator is a collection of algorithms that can be used to generate
    /// 'smooth' fields of noise.  It can also generate random noise as well.
    /// 
    /// The code in this class is based on the post here: http://devmag.org.za/2009/04/25/perlin-noise/
    /// The general strategy to use this code is to:
    ///   (1) Generate a white noise field
    ///   (2) Smooth that field using the PerlinNoiseField method.
    ///   (3) Optionally apply filters and/or transformers to the field.
    /// </summary>
    public class PerlinNoiseGenerator {
        /// <summary>
        /// Gets or sets the number of octaves to use
        /// </summary>
        public int OctaveCount { get; set; }

        /// <summary>
        /// Gets or sets the persistence factor
        /// </summary>
        public float Persistence { get; set; }

        /// <summary>
        /// Gets or sets the algorithm to use when interpolating.
        /// </summary>
        public InterpolationAlgorithm Interpolation { get; set; }

        /// <summary>
        /// Gets or sets the random number generator to use.
        /// </summary>
        public Random Random { get; set; }

        public PerlinNoiseGenerator() {
            OctaveCount = 4;
            Persistence = 0.5f;
            Interpolation = InterpolationAlgorithms.LinearInterpolation;
            Random = new Random();
        }

        /// <summary>
        /// Generates a field of white noise (random noise).  This uses the class's random.
        /// </summary>
        /// <param name="width">Width of the field</param>
        /// <param name="height">Height of the field</param>
        /// <returns>A new field of white noise</returns>
        public NoiseField<float> GenerateWhiteNoise(int width, int height) {
            return GenerateWhiteNoise(width, height, Random);
        }

        /// <summary>
        /// Generates wa field of white noise (random noise)
        /// </summary>
        /// <param name="width">Width of the field</param>
        /// <param name="height">Height of the field</param>
        /// <param name="random">Random number generator to use.  Can be pre-seeded.</param>
        /// <returns></returns>
        public NoiseField<float> GenerateWhiteNoise(int width, int height, Random random) {
            NoiseField<float> field = new NoiseField<float>(width, height);

            for(int x = 0; x < width; x++) {
                for(int y = 0; y < height; y++) {
                    field.Field[x, y] = (float)random.NextDouble() % 1;
                }
            }

            return field;
        }

        /// <summary>
        /// Smooths a noise field.
        /// </summary>
        /// <param name="whiteNoise">Noise field to smooth.</param>
        /// <param name="octave">The current octave.</param>
        /// <returns></returns>
        public NoiseField<float> SmoothNoiseField(NoiseField<float> whiteNoise, int octave) {
            NoiseField<float> smooth = new NoiseField<float>(whiteNoise.Width, whiteNoise.Height);

            int samplePeriod = 1 << octave;
            float sampleFrequency = 1.0f / samplePeriod;

            for(int x = 0; x < smooth.Width; x++) {
                int sampleX1 = (x / samplePeriod) * samplePeriod;
                int sampleX2 = (sampleX1 + samplePeriod) % smooth.Width;

                float horizontalBlend = (x - sampleX1) * sampleFrequency;

                for(int y = 0; y < smooth.Height; y++) {
                    int sampleY1 = (y / samplePeriod) * samplePeriod;
                    int sampleY2 = (sampleY1 + samplePeriod) % smooth.Height;

                    float verticalBlend = (y - sampleY1) * sampleFrequency;

                    float top = Interpolation(whiteNoise.Field[sampleX1, sampleY1], whiteNoise.Field[sampleX2, sampleY1], horizontalBlend);
                    float bottom = Interpolation(whiteNoise.Field[sampleX1, sampleY2], whiteNoise.Field[sampleX2, sampleY2], horizontalBlend);

                    smooth.Field[x, y] = Interpolation(top, bottom, verticalBlend);
                }
            }

            return smooth;
        }

        /// <summary>
        /// Generates a new Perlin Noise field.
        /// </summary>
        /// <param name="baseNoise">Base noise to use.  Should be white noise.</param>
        /// <param name="octaveCount">Number of octaves to go over (number of fields to average)</param>
        /// <param name="persistence">Persistence factor</param>
        /// <returns></returns>
        public NoiseField<float> PerlinNoiseField(NoiseField<float> baseNoise) {
            NoiseField<float>[] smoothNoise = new NoiseField<float>[OctaveCount];

            for(int i = 0; i < OctaveCount; i++) {
                smoothNoise[i] = SmoothNoiseField(baseNoise, i);
            }

            NoiseField<float> perlinNoise = new NoiseField<float>(baseNoise.Width, baseNoise.Height);
            float amplitude = 1.0f;
            float totalAmplitude = 0.0f;

            for(int octave = OctaveCount - 1; octave >= 0; octave--) {
                amplitude *= Persistence;
                totalAmplitude += amplitude;

                for(int x = 0; x < baseNoise.Width; x++) {
                    for(int y = 0; y < baseNoise.Height; y++) {
                        perlinNoise.Field[x, y] += smoothNoise[octave].Field[x, y] * amplitude;
                    }
                }
            }

            // Normalize the fields
            for(int x = 0; x < baseNoise.Width; x++) {
                for(int y = 0; y < baseNoise.Height; y++) {
                    perlinNoise.Field[x, y] /= totalAmplitude;
                }
            }

            return perlinNoise;
        }

        /// <summary>
        /// Performs all steps using cosineInterpolation for a new perlin noise.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="octaveCount"></param>
        /// <param name="persistence"></param>
        /// <returns></returns>
        public NoiseField<float> GeneratePerlinNoise(int width, int height) {
            NoiseField<float> whiteNoise = GenerateWhiteNoise(width, height);
            return PerlinNoiseField(whiteNoise);
        }
    }
}
