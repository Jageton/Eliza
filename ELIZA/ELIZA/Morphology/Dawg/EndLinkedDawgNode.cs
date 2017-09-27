using System;
using System.Collections.Generic;
using ProtoBuf;

namespace ELIZA.Morphology.Dawg
{
    /// <summary>
    /// Represents node linked with ending node. This node Actually has data.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="InnerDawgNode{TKey,TValue}" />
    [ProtoContract]
    public class EndLinkedDawgNode<TKey, TValue>: InnerDawgNode<TKey, TValue> where TKey: IComparable
    {
        [ProtoMember(1)]
        protected TValue value;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public override TValue Value
        {
            get { return value; }
            set { this.value = value; }
        }
        /// <summary>
        /// Gets a value indicating whether this instance has value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has value; otherwise, <c>false</c>.
        /// </value>
        public override bool HasValue
        {
            get { return true; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EndLinkedDawgNode(TKey key) : base(key)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EndLinkedDawgNode()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EndLinkedDawgNode(TKey key, IDawgNode<TKey, TValue> leftChild,
            IDawgNode<TKey, TValue> rightSibling) : base(key, leftChild, rightSibling)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EndLinkedDawgNode(TKey key, TValue value) : base(key)
        {
            this.value = value;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public EndLinkedDawgNode(TKey key, IDawgNode<TKey, TValue> leftChild,
            IDawgNode<TKey, TValue> rightSibling, TValue value) : 
            base(key, leftChild, rightSibling)
        {
            this.value = value;
        }

        protected override string GetNodeString()
        {
            return base.GetNodeString() + "; Value = " + Value;
        }

        protected bool Equals(EndLinkedDawgNode<TKey, TValue> other)
        {
            return base.Equals(other) && EqualityComparer<TValue>.Default.Equals(value, other.value);
        }
        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EndLinkedDawgNode<TKey, TValue>) obj);
        }
        /// <summary>
        /// Serves as the default hash function. 
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ EqualityComparer<TValue>.Default.GetHashCode(value);
            }
        }
        /// <summary>
        /// Gets the subtree hash code.
        /// </summary>
        /// <returns>A hash code for this tree, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetSubtreeHashCode()
        {
            return (base.GetSubtreeHashCode()*397) ^ EqualityComparer<TValue>.Default.GetHashCode(value);
        }
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            return new EndLinkedDawgNode<TKey, TValue>(Key,
                LeftChild == null? null: (IDawgNode<TKey, TValue>)LeftChild.Clone(),
                RightSibling == null? null: (IDawgNode<TKey, TValue>)RightSibling.Clone(),
                Value);
        }
    }
}
