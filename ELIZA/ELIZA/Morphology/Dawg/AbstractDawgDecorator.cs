using System;
using System.Collections;
using System.Collections.Generic;

namespace ELIZA.Morphology.Dawg
{
    /// <summary>
    /// Abstract dawg decorator. Adds additional functionality to the already existing dawg.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="IDawg{TKey,TValue}" />
    public class AbstractDawgDecorator<TKey, TValue>:
        IDawg<TKey, TValue> where TKey: IComparable
    {
        protected IDawg<TKey, TValue> inner;

        /// <summary>
        /// Gets the inner dawg.
        /// </summary>
        public IDawg<TKey, TValue> InnerDawg { get { return inner; }
            private set { inner = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDawgDecorator{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="inner">The inner dawg.</param>
        public AbstractDawgDecorator(IDawg<TKey, TValue> inner)
        {
            InnerDawg = inner;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<IEnumerable<TKey>, TValue>> GetEnumerator()
        {
            return inner.GetEnumerator();
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) inner).GetEnumerator();
        }
        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <returns>
        /// The number of elements in the collection. 
        /// </returns>
        public virtual int Count
        {
            get { return inner.Count; }
        }
        /// <summary>
        /// Determines whether the read-only dictionary contains an element that has the specified key.
        /// </summary>
        /// <returns>
        /// true if the read-only dictionary contains an element that has the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public virtual bool ContainsKey(IEnumerable<TKey> key)
        {
            return inner.ContainsKey(key);
        }
        /// <summary>
        /// Gets the value that is associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2"/> interface contains an element that has the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate.</param><param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public virtual bool TryGetValue(IEnumerable<TKey> key, out TValue value)
        {
            return inner.TryGetValue(key, out value);
        }
        /// <summary>
        /// Gets the element that has the specified key in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// The element that has the specified key in the read-only dictionary.
        /// </returns>
        /// <param name="key">The key to locate.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found. </exception>
        public virtual TValue this[IEnumerable<TKey> key]
        {
            get { return inner[key]; }
        }
        /// <summary>
        /// Gets an enumerable collection that contains the keys in the read-only dictionary. 
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the keys in the read-only dictionary.
        /// </returns>
        public virtual IEnumerable<IEnumerable<TKey>> Keys
        {
            get { return inner.Keys; }
        }
        /// <summary>
        /// Gets an enumerable collection that contains the values in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the values in the read-only dictionary.
        /// </returns>
        public virtual IEnumerable<TValue> Values
        {
            get { return inner.Values; }
        }
        /// <summary>
        /// Finds all the <see cref="KeyValuePair{TKey,TValue}"/> whose key starts with the specified sequence.
        /// In another words, performs key* search.
        /// </summary>
        /// <param name="startsWith">The begining of the key.</param>
        /// <returns>Returns the enumeration that contains <see cref="KeyValuePair{TKey,TValue}"/> whose keys 
        /// start with the specified sequence.</returns>
        public virtual IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> StartsWith(IEnumerable<TKey> startsWith)
        {
            return inner.StartsWith(startsWith);
        }
        /// <summary>
        /// Finds all the <see cref="KeyValuePair{TKey,TValue}"/> whose key starts and ends with sepcified sequences.
        /// </summary>
        /// <param name="startsWith">The beginning of the key.</param>
        /// <param name="endsWith">The ending of the key.</param>
        /// <returns>Returns the enumeration that contains <see cref="KeyValuePair{TKey,TValue}"/> whose keys 
        /// start and end with specified sequences.</returns>
        public virtual IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> StartsAndEndsWith(IEnumerable<TKey> startsWith, IEnumerable<TKey> endsWith)
        {
            return inner.StartsAndEndsWith(startsWith, endsWith);
        }
        /// <summary>
        /// Performs the fuzzy search of the sepcified key.
        /// </summary>
        /// <param name="key">The key to perform to search.</param>
        /// <param name="maxDiff">The maximum difference lenght. The good practice is to set this parameter equal to 
        /// half of the <see cref="key"/> lenght.</param>
        /// <returns>Returns the enumeration that contains all search results.</returns>
        public virtual IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> FuzzySearch(IEnumerable<TKey> key, int maxDiff)
        {
            return inner.FuzzySearch(key, maxDiff);
        }
    }
}
