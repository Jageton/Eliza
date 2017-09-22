using System;
using System.Collections.Generic;

namespace ELIZA.Morphology.Dawg.Builders
{
    /// <summary>
    /// Represents the functionality to build the DAFSA from the sepcified data set.
    /// </summary>
    /// <typeparam name="TDawg">The type of the DAFSA that will be built..</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IDawgBuilder<out TDawg, TKey, TValue> 
        where TDawg: IDawg<TKey, TValue> 
        where TKey:IComparable
    {
        /// <summary>
        /// Builds the specified type of dawg from the specified data set.
        /// </summary>
        /// <param name="data">The data set.</param>
        /// <returns>Returns the builded dafsa.</returns>
        TDawg Build(IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> data);
    }
}
