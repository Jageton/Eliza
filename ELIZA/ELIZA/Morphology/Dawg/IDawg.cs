using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ELIZA.Morphology.Dawg
{
    /// <summary>
    /// Represents the functionality of the directed acyclic finite state automation (DAFSA/DAWG) as readonly 
    /// dictionary with special purpose functionality.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys to be stored.</typeparam>
    /// <typeparam name="TValue">The type of the values to be stored.</typeparam>
    /// <seealso cref="System.Collections.Generic.IReadOnlyDictionary{System.Collections.Generic.IEnumerable{TKey},TValue}" />
    /// <seealso cref="System.Linq.IOrderedEnumerable{System.Collections.Generic.KeyValuePair{System.Collections.Generic.IEnumerable{TKey},TValue}}" />
    public interface IDawg<TKey, TValue> : IReadOnlyDictionary<IEnumerable<TKey>, TValue>
        where TKey : IComparable
    {
        /// <summary>
        /// Finds all the <see cref="KeyValuePair{TKey,TValue}"/> whose key starts with the specified sequence.
        /// In another words, performs key* search.
        /// </summary>
        /// <param name="startsWith">The begining of the key.</param>
        /// <returns>Returns the enumeration that contains <see cref="KeyValuePair{TKey,TValue}"/> whose keys 
        /// start with the specified sequence.</returns>
        IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> StartsWith([NotNull]IEnumerable<TKey> startsWith);
        /// <summary>
        /// Finds all the <see cref="KeyValuePair{TKey,TValue}"/> whose key starts and ends with sepcified sequences.
        /// </summary>
        /// <param name="startsWith">The beginning of the key.</param>
        /// <param name="endsWith">The ending of the key.</param>
        /// <returns>Returns the enumeration that contains <see cref="KeyValuePair{TKey,TValue}"/> whose keys 
        /// start and end with specified sequences.</returns>
        IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> StartsAndEndsWith([NotNull]IEnumerable<TKey> startsWith,
            IEnumerable<TKey> endsWith);
        /// <summary>
        /// Performs the fuzzy search of the sepcified key.
        /// </summary>
        /// <param name="key">The key to perform to search.</param>
        /// <param name="maxDiff">The maximum difference lenght. The good practice is to set this parameter equal to 
        /// half of the <see cref="key"/> lenght.</param>
        /// <returns>Returns the enumeration that contains all search results.</returns>
        IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> FuzzySearch([NotNull]IEnumerable<TKey> key, int maxDiff);
    }
}
