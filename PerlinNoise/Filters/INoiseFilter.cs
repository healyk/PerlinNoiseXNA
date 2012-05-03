using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerlinNoise {
    /// <summary>
    /// Defines an algorithm used to filter a PerlinNoise.  This algorithm takes in
    /// a NoiseField and outputs a NoiseField.  Examples involve translating noise values to
    /// color values, normalizing values, dropping out values and so forth.
    /// 
    /// <typeparam name="T">Type of the noise field entering the filter</typeparam>
    /// <typeparam name="U">Type of the noise field the filter produces</typeparam>
    /// </summary>
    public interface INoiseFilter<T, U> {
        /// <summary>
        /// Performs the filter.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        NoiseField<U> Filter(NoiseField<T> field);
    }
}
