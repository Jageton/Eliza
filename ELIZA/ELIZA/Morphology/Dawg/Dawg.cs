using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;

namespace ELIZA.Morphology.Dawg
{
    [SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
    public class Dawg<TKey, TValue>: ICloneable, IDawg<TKey, TValue> where TKey : IComparable
    {
        public IDawgNode<TKey, TValue> Root { get; private set; }
        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <returns>
        /// The number of elements in the collection. 
        /// </returns>
        public int Count
        {
            get { return (int)Root.Count; }
        }

        public Dawg()
        {
            Root = new InnerDawgNode<TKey, TValue>();
        }
        internal Dawg(IDawgNode<TKey, TValue> root)
        {
            this.Root = root;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return new Dawg<TKey, TValue>((InnerDawgNode<TKey, TValue>)Root.Clone());
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<IEnumerable<TKey>, TValue>> GetEnumerator()
        {
            return Traverse(Root, new List<TKey>()).GetEnumerator();
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Determines whether the read-only dictionary contains an element that has the specified key.
        /// </summary>
        /// <returns>
        /// true if the read-only dictionary contains an element that has the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey([NotNull]IEnumerable<TKey> key)
        {
            return GetNode(key) != null;
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
            var node = GetNode(key);
            value = default(TValue);
            if (node == null)
                return false;
            if (node.HasValue)
            {
                value = node.Value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Gets the element that has the specified key in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// The element that has the specified key in the read-only dictionary.
        /// </returns>
        /// <param name="key">The key to locate.</param><exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception><exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key"/> is not found. </exception>
        public TValue this[[NotNull]IEnumerable<TKey> key]
        {
            get
            {
                TValue value;
                if(TryGetValue(key, out value))
                    return value;
                throw new KeyNotFoundException("The following key was not found");
            }
        }
        /// <summary>
        /// Gets an enumerable collection that contains the keys in the read-only dictionary. 
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the keys in the read-only dictionary.
        /// </returns>
        public IEnumerable<IEnumerable<TKey>> Keys 
        {
            get { return from pair in Traverse(Root, new List<TKey>()) select pair.Key; } 
        }
        /// <summary>
        /// Gets an enumerable collection that contains the values in the read-only dictionary.
        /// </summary>
        /// <returns>
        /// An enumerable collection that contains the values in the read-only dictionary.
        /// </returns>
        public IEnumerable<TValue> Values
        {
            get { return from pair in Traverse(Root, new List<TKey>()) select pair.Value; }
        }

        IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> Traverse(IDawgNode<TKey, TValue> node, IList<TKey> currentKey)
        {
            while (true)
            {
                if (Root != node)
                {
                    //append node key
                    currentKey.Add(node.Key);
                    //if node has value then return new pair
                    if (node.HasValue)
                        yield return new KeyValuePair<IEnumerable<TKey>, TValue>(currentKey, node.Value);
                }
                if (node.LeftChild != null)
                    foreach (var child in Traverse(node.LeftChild, currentKey))
                        yield return child;
                currentKey.RemoveAt(currentKey.Count - 1);
                if (node.RightSibling != null)
                {
                    node = node.RightSibling;
                    continue;
                }
                break;
            }
        }

        private IDawgNode<TKey, TValue> GetNode(IEnumerable<TKey> key)
        {
            var current = Root;
            foreach (var k in key)
            {
                var next = current.Get(k);
                if (next == null)
                    return null;
                current = next;
            }
            return current;
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
            var currKey = new List<TKey>();
            var currentNode = Root;
            foreach (var k in startsWith)
            {
                currentNode = currentNode.Get(k);
                if(currentNode == null)
                    throw new KeyNotFoundException("The specified key was not fond");
            }
            return Traverse(currentNode, currKey);
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
            var chk = endsWith.Reverse().ToArray(); //the key to check
            foreach (var pair in StartsWith(startsWith))
            {
                var key = pair.Key.Reverse().ToArray(); //current pair reverse key
                if (key.Length > chk.Length) //if key is long enough to contain check
                {
                    if (!chk.Where((t, i) => t.CompareTo(key[i]) != 0).Any()) //check for equality
                        yield return pair;
                }
            }
        }

        /// <summary>
        /// Performs the fuzzy search of the sepcified key.
        /// </summary>
        /// <param name="key">The key to perform to search.</param>
        /// <param name="maxDiff">The maximum difference lenght. The good practice is to set this parameter equal to 
        /// half of the <see cref="key"/> lenght.</param>
        /// <returns>Returns the enumeration that contains all search results.</returns>
        public virtual IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>>
            FuzzySearch(IEnumerable<TKey> key, int maxDiff)
        {
            return FuzzySearch(Root, new List<TKey>(), 0, maxDiff, key.ToArray(), 0);
        }

        protected IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> FuzzySearch(IDawgNode<TKey, TValue> node,
            List<TKey> currentKey, int currentDiff, int maxDiff, TKey[] key, int keyIndex)
        {
            if (currentDiff <= maxDiff)
            {
                if (Root != node)
                {
                    //append node key
                    currentKey.Add(node.Key);
                    //if node has value then return new pair
                    if (node.HasValue)
                        yield return new KeyValuePair<IEnumerable<TKey>, TValue>(currentKey, node.Value);
                }
                if (node.LeftChild != null)
                    foreach (var child in FuzzySearch(node.LeftChild, currentKey,
                        currentDiff + ((keyIndex >= key.Length || key[keyIndex].CompareTo(node.LeftChild.Key) != 0)? 1 : 0),
                        maxDiff, key, keyIndex + 1))
                        yield return child;
                currentKey.RemoveAt(currentKey.Count - 1);
                if (node.RightSibling != null)
                    foreach (var child in FuzzySearch(node.RightSibling, currentKey,
                        currentDiff + ((keyIndex >= key.Length || key[keyIndex].CompareTo(node.RightSibling.Key) != 0) ? 1 : 0),
                        maxDiff, key, keyIndex + 1))
                        yield return child;
            }
        }
    }
}
