using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerlinNoise {
    /// <summary>
    /// Collection of algorithms used for interpolations.
    /// </summary>
    public class InterpolationAlgorithms {
        /// <summary>
        /// Performs linear interpolation on two points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float LinearInterpolation(float a, float b, float t) {
            return a * (1 - t) + t * b;
        }

        /// <summary>
        /// Performs a cosine-based interpolation on two points.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float CosineInterpolation(float a, float b, float t) {
            float mu = (1.0f - (float)Math.Cos(t * Math.PI)) / 2.0f;
            return a * (1 - mu) + b * mu;
        }
    }
}
