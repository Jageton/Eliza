using System;
using System.Collections.Generic;
using ProtoBuf;
using JetBrains.Annotations;

namespace ELIZA.Morphology.Dawg
{
    [ProtoContract]
    public interface IDawgNode<TKey, TValue>: ICloneable, IEnumerable<IDawgNode<TKey, TValue>> 
        where TKey: IComparable
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        TKey Key { get; }
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        TValue Value { get; }
        /// <summary>
        /// Gets a value indicating whether this instance has value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has value; otherwise, <c>false</c>.
        /// </value>
        bool HasValue { get; }
        /// <summary>
        /// Gets the element count in dawg with this node as root.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        long Count { get; }
        /// <summary>
        /// Left child in "left child - right sibling" scheme/
        /// </summary>
        /// <value>
        /// The left child.
        /// </value>
        IDawgNode<TKey, TValue> LeftChild { get; set; }
        /// <summary>
        /// Right sibling in "left child - right sibling" scheme/
        /// </summary>
        /// <value>
        /// The right sibling child.
        /// </value>
        IDawgNode<TKey, TValue> RightSibling { get; set; }
        /// <summary>
        /// Gets the children of current node.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        IEnumerable<IDawgNode<TKey, TValue>> Children { get; }

        /// <summary>
        /// Determines whether current tree contains child with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   Returns <c>true</c> if current tree contains child with the specified key;
        ///  otherwise, <c>false</c>.
        /// </returns>
        bool Contains([NotNull]TKey key);
        /// <summary>
        /// Gets the child with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the child with the specified key.</returns>
        IDawgNode<TKey, TValue> Get([NotNull]TKey key);
        /// <summary>
        /// Adds the the child node with the cpecified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <exception cref="DawgException">When the node with the specified key already exists.</exception>
        void Add([NotNull]TKey key);
        /// <summary>
        /// Adds the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <exception cref="DawgException">When the specified node already exists.</exception>
        void Add([NotNull]IDawgNode<TKey, TValue> node);
        /// <summary>
        /// Adds the node with specified key and value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="DawgException">When the node with the specified key already exists.</exception>
        void Add([NotNull]TKey key, TValue value);
        /// <summary>
        /// Removes the node with the specified key. It is guaranteed that there is no child with the 
        /// specified key after execution.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove([NotNull]TKey key);

        /// <summary>
        /// Returns formatted string for current subtree.
        /// </summary>
        /// <param name="indent">The indent. Default is empty string.</param>
        /// <param name="last">Should be set to <c>true</c> if. Default is <c>true</c> for root.</param>
        /// <returns>Returns string that represents current subtree.</returns>
        string SubtreeToString(string indent = "", bool last = true);
        /// <summary>
        /// Gets the subtree hash code.
        /// </summary>
        /// <returns>A hash code for this tree, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        int GetSubtreeHashCode();
    }
}
