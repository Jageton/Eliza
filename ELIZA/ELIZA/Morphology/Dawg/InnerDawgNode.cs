using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace ELIZA.Morphology.Dawg
{
    /// <summary>
    /// Represents inner dawg node. A dawg node is inner if it does not contain data, otherwise it is 
    /// <see cref="EndLinkedDawgNode{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="IDawgNode{TKey,TValue}" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{PracticeLib.DAWG.IDawgNode{TKey,TValue}}" />
    [ProtoContract]
    [Serializable]
    public class InnerDawgNode<TKey, TValue>: IDawgNode<TKey, TValue> where TKey: IComparable
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public virtual long Count
        {
            get 
            { 
                return this.Aggregate<IDawgNode<TKey, TValue>, long>(HasValue? 1 : 0, (l, node) =>
                    l + node.Count);
            }
        }
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public virtual TValue Value
        {
            get { return default(TValue); }
            set 
            {
                //do nothing for memory purposese
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance has value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has value; otherwise, <c>false</c>.
        /// </value>
        public virtual bool HasValue {
            get { return false; } }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [ProtoMember(1)]
        public TKey Key { get; private set; }
        /// <summary>
        /// Left child in "left child - right sibling" scheme.
        /// </summary>
        /// <value>
        /// The left child.
        /// </value>
        [ProtoMember(2)]
        public IDawgNode<TKey, TValue> LeftChild { get; set; }
        /// <summary>
        /// Right sibling in "left child - right sibling" scheme.
        /// </summary>
        /// <value>
        /// The right sibling child.
        /// </value>
        [ProtoMember(3)]
        public IDawgNode<TKey, TValue> RightSibling { get; set; }
        /// <summary>
        /// Gets the children of current node.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public IEnumerable<IDawgNode<TKey, TValue>> Children
        {
            get { return this; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public InnerDawgNode(TKey key)
        {
            Key = key;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public InnerDawgNode()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public InnerDawgNode(TKey key, IDawgNode<TKey, TValue> leftChild, IDawgNode<TKey, TValue> rightSibling)
        {
            Key = key;
            LeftChild = leftChild;
            RightSibling = rightSibling;
        }

        /// <summary>
        /// Determines whether current tree contains child with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   Returns <c>true</c> if current tree contains child with the specified key;
        ///  otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(TKey key)
        {
            return Get(key) != null;
        }
        /// <summary>
        /// Gets the child with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns the child with the specified key.</returns>
        public IDawgNode<TKey, TValue> Get(TKey key)
        {
            return this.FirstOrDefault((node) => node.Key.CompareTo(key) == 0);
        }
        /// <summary>
        /// Adds the the child node with the cpecified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <exception cref="DawgException">When the node with the specified key already exists.</exception>
        public void Add(TKey key)
        {
            Add(new InnerDawgNode<TKey, TValue>{Key = key});
        }
        /// <summary>
        /// Adds the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <exception cref="DawgException">When the specified node already exists.</exception>
        public void Add(IDawgNode<TKey, TValue> node)
        {
            if (LeftChild == null)
                LeftChild = node;
            else
            {
                var current = LeftChild;
                if (node.Key.CompareTo(current.Key) < 0)
                {
                    node.RightSibling = LeftChild;
                    LeftChild = node;
                }
                else
                {
                    if(LeftChild.Key.CompareTo(node.Key) == 0)
                        throw new DawgException("The specified key already exists.");
                    var prev = current;
                    current = current.RightSibling;
                    if (current == null)
                        prev.RightSibling = node;
                    else
                    {
                        while (current != null && node.Key.CompareTo(current.Key) <= 0)
                        {
                            if (current.Key.CompareTo(node.Key) == 0)
                                throw new DawgException("The specified key already exists.");
                            prev = current;
                            current = current.RightSibling;
                        }
                        node.RightSibling = current;
                        prev.RightSibling = node;
                    }
                }
            }
        }
        /// <summary>
        /// Adds the node with specified key and value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="DawgException">When the node with the specified key already exists.</exception>
        public void Add(TKey key, TValue value)
        {
            Add(new EndLinkedDawgNode<TKey, TValue>{Key = key, Value = value});
        }
        /// <summary>
        /// Removes the node with the specified key. It is guaranteed that there is no child with the 
        /// specified key after execution.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(TKey key)
        {
            var prev = this.FirstOrDefault((node) =>
                    node.RightSibling != null && node.RightSibling.Key.CompareTo(key) == 0);
            if (prev == null)
            {
                if (LeftChild != null && LeftChild.Key.CompareTo(key) == 0)
                    LeftChild = LeftChild.RightSibling;
            }
            else
            {
                prev.RightSibling = prev.RightSibling.RightSibling;
            }
        }


        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public virtual object Clone()
        {
            return new InnerDawgNode<TKey, TValue>()
            {
                Key = Key,
                LeftChild = LeftChild == null ? null : (IDawgNode<TKey, TValue>) LeftChild.Clone(),
                RightSibling = RightSibling == null ? null : (IDawgNode<TKey, TValue>) RightSibling.Clone(),
            };
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
            return Equals((InnerDawgNode<TKey, TValue>) obj);
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
                var hashCode = EqualityComparer<TKey>.Default.GetHashCode(Key);
                hashCode += (hashCode*397) ^ (LeftChild != null ? LeftChild.GetHashCode() : 0);
                hashCode += (hashCode*397) ^ (RightSibling != null ? RightSibling.GetHashCode() : 0);
                return hashCode;
            }
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return SubtreeToString();
        }
        /// <summary>
        /// Returns formatted string for current subtree.
        /// </summary>
        /// <param name="indent">The indent. Default is empty string.</param>
        /// <param name="last">Should be set to <c>true</c> if. Default is <c>true</c> for root.</param>
        /// <returns>Returns string that represents current subtree.</returns>
        public virtual string SubtreeToString(string indent = "", bool last = true)
        {
            var result = indent;
            if (last)
            {
                result += "\\-";
                indent += " ";
            }
            else
            {
                result += "|-";
                indent += "| ";
            }
            result += GetNodeString();
            var children = Children.ToArray();
            for (var i = 0; i < children.Length; i++)
            {
                result += string.Format("\n{0}", children[i].SubtreeToString(indent, i == children.Length - 1));
            }
            return result;
        }
        /// <summary>
        /// Gets the subtree hash code.
        /// </summary>
        /// <returns>A hash code for this tree, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public virtual int GetSubtreeHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<TKey>.Default.GetHashCode(Key);
                hashCode += (hashCode * 397) ^ (LeftChild != null ? LeftChild.GetSubtreeHashCode() : 0);
                hashCode += (hashCode * 397) ^ (RightSibling != null ? RightSibling.GetSubtreeHashCode() : 0);
                return hashCode;
            }
        }
        protected bool Equals(InnerDawgNode<TKey, TValue> other)
        {
            return EqualityComparer<TKey>.Default.Equals(Key, other.Key) && Equals(LeftChild, other.LeftChild) && Equals(RightSibling, other.RightSibling);
        }
        protected virtual string GetNodeString()
        {
            return "Key = " + Key;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IDawgNode<TKey, TValue>> GetEnumerator()
        {
            var current = LeftChild;
            while (current != null)
            {
                yield return current;
                current = current.RightSibling;
            }
            yield break;
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
