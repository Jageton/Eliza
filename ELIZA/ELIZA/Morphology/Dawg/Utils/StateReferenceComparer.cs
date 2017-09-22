using System;
using System.Collections.Generic;

namespace ELIZA.Morphology.Dawg.Utils
{
    /// <summary>
    /// Reference comparer for <see cref="InnerDawgNode{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="Dawg{TKey,TValue}.IDawgNode{TKey,TValue}}" />
    public class StateReferenceComparer<TKey, TValue> :
        IEqualityComparer<IDawgNode<TKey, TValue>> where TKey : IComparable
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.
        /// </param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
        public bool Equals(IDawgNode<TKey, TValue> x, IDawgNode<TKey, TValue> y)
        {
            return ReferenceEquals(x, y);
        }
        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(IDawgNode<TKey, TValue> obj)
        {
            return EqualityComparer<object>.Default.GetHashCode(obj);
        }
    }
}
