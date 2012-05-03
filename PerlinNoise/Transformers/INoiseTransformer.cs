using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerlinNoise.Transformers {
    /// <summary>
    /// Interface for classes that perform a transformation on a NoiseField, changing it into
    /// another object.  To go from NoiseField to NoiseField, use a INoiseFilter instead.
    /// 
    /// <typeparam name="T">Type of the field.</typeparam>
    /// <typeparam name="U">Object that this transformation produces</typeparam>
    /// </summary>
    public interface INoiseTransformer<T, U> {
        /// <summary>
        /// Performs a transformation on a NoiseField.
        /// </summary>
        /// <param name="field">Field to return</param>
        /// <returns>Result of the transformation</returns>
        U Transform(NoiseField<T> field);
    }
}
