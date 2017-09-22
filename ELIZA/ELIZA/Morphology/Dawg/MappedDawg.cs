using System;
using System.Collections.Generic;
using System.Linq;
using ELIZA.Morphology.Dawg.Utils;

namespace ELIZA.Morphology.Dawg
{
    public class MappedDawg<TKey, TValue, TCKey, TCValue>:
        AbstractDawgDecorator<TCKey, TCValue>, IDawg<TKey, TValue>
        where TKey: IComparable
        where TCKey: IComparable
    {
        protected IMappingStrategy<TKey, TCKey> keyMapper;
        protected IMappingStrategy<TValue, TCValue> valueMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractDawgDecorator{TCKey, TCValue}"/> class.
        /// </summary>
        /// <param name="inner">The inner dawg.</param>
        /// <param name="keyMapper">The object mapper from <see cref="TKey"/> to <see cref="TCKey"/>.</param>
        /// <param name="valueMapper">The object mapper from <see cref="TValue"/> to <see cref="TCValue"/>.</param>
        public MappedDawg(IDawg<TCKey, TCValue> inner, IMappingStrategy<TKey, TCKey> keyMapper,
            IMappingStrategy<TValue, TCValue> valueMapper) : base(inner)
        {
            this.keyMapper = keyMapper;
            this.valueMapper = valueMapper;
        }

        protected IEnumerable<TCKey> Map(IEnumerable<TKey> key)
        {
            return from k in key select keyMapper.Map(k);
        }
        protected IEnumerable<TKey> Map(IEnumerable<TCKey> key)
        {
            return from k in key select keyMapper.Map(k);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<IEnumerable<TKey>, TValue>> IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>>.GetEnumerator()
        {
            return (from pair in inner
                select new KeyValuePair<IEnumerable<TKey>, TValue>(Map(pair.Key),
                    valueMapper.Map(pair.Value))).GetEnumerator();
        }
        /// <summary>
        /// Determines whether the read-only dictionary contains an element that has the specified key.
        /// </summary>
        /// <returns>
        /// true if the read-only dictionary contains an element that has the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey(IEnumerable<TKey> key)
        {
            return inner.ContainsKey(Map(key));
        }
        /// <summary>
        /// Gets the value that is associated with the specified key.
        /// </summary>
        /// <returns>
        /// true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2"/> interface contains an element that has the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate.</param><param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool TryGetValue(IEnumerable<TKey> key, out TValue value)
        {
            TCValue outV;
            value = default(TValue);
            var result = inner.TryGetValue(Map(key), out outV);
            if (result)
            {
                value = valueMapper.Map(outV);
            }
            return result;
        }
        /// <summary>
        /// Gets the element that has the specified key in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// The element that has the specified key in the read-only dictionary.
        /// </returns>
        /// <param name="key">The key to locate.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found. </exception>
        public TValue this[IEnumerable<TKey> key]
        {
            get { return valueMapper.Map(inner[Map(key)]); }
        }
        /// <summary>
        /// Gets an enumerable collection that contains the keys in the read-only dictionary. 
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the keys in the read-only dictionary.
        /// </returns>
        public new IEnumerable<IEnumerable<TKey>> Keys
        {
            get { return from k in inner.Keys select Map(k); }
        }
        /// <summary>
        /// Gets an enumerable collection that contains the values in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the values in the read-only dictionary.
        /// </returns>
        public new IEnumerable<TValue> Values
        {
            get { return from v in inner.Values select valueMapper.Map(v); }
        }
        /// <summary>
        /// Finds all the <see cref="KeyValuePair{TKey,TValue}"/> whose key starts with the specified sequence.
        /// In another words, performs key* search.
        /// </summary>
        /// <param name="startsWith">The begining of the key.</param>
        /// <returns>Returns the enumeration that contains <see cref="KeyValuePair{TKey,TValue}"/> whose keys 
        /// start with the specified sequence.</returns>
        public IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> StartsWith(IEnumerable<TKey> startsWith)
        {
            return from pair in inner.StartsWith(Map(startsWith))
                   select new KeyValuePair<IEnumerable<TKey>, TValue>(Map(pair.Key),
                       valueMapper.Map(pair.Value));
        }
        /// <summary>
        /// Finds all the <see cref="KeyValuePair{TKey,TValue}"/> whose key starts and ends with sepcified sequences.
        /// </summary>
        /// <param name="startsWith">The beginning of the key.</param>
        /// <param name="endsWith">The ending of the key.</param>
        /// <returns>Returns the enumeration that contains <see cref="KeyValuePair{TKey,TValue}"/> whose keys 
        /// start and end with specified sequences.</returns>
        public IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> StartsAndEndsWith(IEnumerable<TKey> startsWith, IEnumerable<TKey> endsWith)
        {
            return from pair in inner.StartsAndEndsWith(Map(startsWith), Map(endsWith))
                   select new KeyValuePair<IEnumerable<TKey>, TValue>(Map(pair.Key),
                       valueMapper.Map(pair.Value));
        }
        /// <summary>
        /// Performs the fuzzy search of the sepcified key.
        /// </summary>
        /// <param name="key">The key to perform to search.</param>
        /// <param name="maxDiff">The maximum difference lenght. The good practice is to set this parameter equal to 
        /// half of the <see cref="key"/> lenght.</param>
        /// <returns>Returns the enumeration that contains all search results.</returns>
        public IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> FuzzySearch(IEnumerable<TKey> key, int maxDiff)
        {
            return from pair in inner.FuzzySearch(Map(key), maxDiff)
                select new KeyValuePair<IEnumerable<TKey>, TValue>(Map(pair.Key),
                    valueMapper.Map(pair.Value));
        }
    }
}
