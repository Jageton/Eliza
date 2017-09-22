using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Элемент префиксного дерева.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа.</typeparam>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    [ProtoContract]
    [Serializable]
    [ProtoInclude(111, typeof(SparseNode<ulong>))]
    [ProtoInclude(112, typeof(SparseNode<ushort>))]
    public class Node<TKey, TValue> where TKey : IComparable
    {
        
        private TKey key;
        
        private TValue value;
        
        private Node<TKey, TValue> leftChild;
        
        private Node<TKey, TValue> rightSibling;
        
        private bool hasValue;

        [ProtoMember(1)]
        /// <summary>
        /// Получает или задёт ключ.
        /// </summary>
        public TKey Key
        {
            get { return key; }
            set { key = value; }
        }
        [ProtoMember(2)]
        /// <summary>
        /// Получает или задаёт значение.
        /// </summary>
        public TValue Value
        {
            get { return this.value; }
            set
            {
                this.hasValue = true;
                this.value = value;
            }
        }
        [ProtoMember(3)]
        /// <summary>
        /// Получает или задаёт левого сына.
        /// </summary>       
        public Node<TKey, TValue> LeftChild
        {
            get { return leftChild; }
            set { leftChild = value; }
        }
        [ProtoMember(4)]
        /// <summary>
        /// Получает или задаёт правого брата.
        /// </summary>        
        public Node<TKey, TValue> RightSibling
        {
            get { return rightSibling; }
            set { rightSibling = value; }
        }
        [ProtoMember(5)]
        /// <summary>
        /// Получает значение, показывающее, содержит ли текущий элемент значение.
        /// </summary>
        public bool HasValue
        {
            get { return hasValue; }
            set { hasValue = value; }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Node{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        /// <param name="leftChild">Левый сын.</param>
        /// <param name="rightSibling">Правый брат.</param>
        public Node(TKey key, TValue value, Node<TKey, TValue> leftChild = null,
            Node<TKey, TValue> rightSibling = null)
        {
            this.key = key;
            this.value = value;
            this.leftChild = leftChild;
            this.rightSibling = rightSibling;
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Node{TKey, TValue}"/>.
        /// </summary>
        public Node()
            : this(default(TKey), default(TValue), null, null)
        {

        }

        public void Set(IEnumerable<TKey> key, TValue value)
        {
            Set(key, value, -1);
        }
        /// <summary>
        /// Устаналивает значение для заданного ключа.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        /// <param name="keyPos">Позиция в ключе.</param>
        public virtual void Set(IEnumerable<TKey> key, TValue value, int keyPos)
        {
            keyPos++;
            if (keyPos == key.Count())
            {
                this.Value = value;
                this.hasValue = true;
                return;
            }
            TKey currKey = key.Skip(keyPos).First();
            if (leftChild == null)
            {
                leftChild = new Node<TKey, TValue>();
                leftChild.key = currKey;
                leftChild.Set(key, value, keyPos);
            }
            else
            {
                var currNode = this.leftChild;
                Node<TKey, TValue> prevNode = null;
                while (currNode != null && currNode.key.CompareTo(currKey) < 0)
                {
                    prevNode = currNode;
                    currNode = currNode.rightSibling;
                }
                if (currNode != null && currNode.key.CompareTo(currKey) == 0)
                {
                    currNode.Set(key, value, keyPos);
                }
                else
                {
                    if (prevNode == null)
                    {
                        var temp = this.leftChild;
                        this.leftChild = new Node<TKey, TValue>();
                        this.leftChild.key = currKey;
                        this.leftChild.rightSibling = temp;
                        this.leftChild.Set(key, value, keyPos);
                    }
                    else
                    {
                        prevNode.rightSibling = new Node<TKey, TValue>();
                        prevNode.rightSibling.key = currKey;
                        prevNode.rightSibling.Set(key, value, keyPos);
                        prevNode.rightSibling.rightSibling = currNode;
                    }
                }
            }
        }
        /// <summary>
        /// Получает значение по ключу.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Возвращает значение с заданным ключём.</returns>
        public virtual TValue Get(IEnumerable<TKey> key)
        {
            return Get(key, -1);
        }
        /// <summary>
        /// Получает значение по ключу.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="keyPos">Позиция в ключе.</param>
        /// <returns>Возвращает значение с заданным ключём.</returns>
        public virtual TValue Get(IEnumerable<TKey> key, int keyPos)
        {
            keyPos++;
            if (keyPos != 0 && keyPos == key.Count())
            {
                if (this.hasValue) return this.value;
                else return default(TValue);
            }
            TKey currKey = key.Skip(keyPos).Take(1).First();
            if (leftChild == null)
                return default(TValue);
            else
            {
                var currNode = this.leftChild;
                Node<TKey, TValue> prevNode = null;
                while (currNode != null && currNode.key.CompareTo(currKey) < 0)
                {
                    prevNode = currNode;
                    currNode = currNode.rightSibling;
                }
                if (currNode != null && currNode.key.CompareTo(currKey) == 0)
                    return currNode.Get(key, keyPos);
                else return default(TValue);
            }
        }
        /// <summary>
        /// Проверяет, соедржит ли дерево заданный ключ.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Возвращает значение, показывающее, содержит ли текущее дерево заданный ключ.</returns>
        public virtual bool Contains(IEnumerable<TKey> key)
        {
            return this.Contains(key, -1);
        }
        /// <summary>
        /// Проверяет, соедржит ли дерево заданный ключ.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="keyPos">Позиция в ключе.</param>
        /// <returns>Возвращает значение, показывающее, содержит ли текущее дерево заданный ключ.</returns>
        public virtual bool Contains(IEnumerable<TKey> key, int keyPos)
        {
            keyPos++;
            if (keyPos != 0 && keyPos == key.Count())
            {
                if (this.hasValue) return true;
                else return false;
            }
            TKey currKey = key.Skip(keyPos).Take(1).First();
            if (leftChild == null)
                return false;
            else
            {
                var currNode = this.leftChild;
                Node<TKey, TValue> prevNode = null;
                while (currNode != null && currNode.key.CompareTo(currKey) < 0)
                {
                    prevNode = currNode;
                    currNode = currNode.rightSibling;
                }
                if (currNode != null && currNode.key.CompareTo(currKey) == 0)
                    return currNode.Contains(key, keyPos);
                else return false;
            }
        }
        /// <summary>
        /// Получает коллекцию потомков заданного элемента дерева.
        /// </summary>
        public IEnumerable<Node<TKey, TValue>> ChildCollection
        {
            get
            {
                var currChild = leftChild;
                while (currChild != null)
                {
                    yield return currChild;
                    currChild = currChild.rightSibling;
                }
            }
        }
        /// <summary>
        /// Ищет элемент по заданному ключу.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Возвращает элемент по заданному ключу.</returns>
        public Node<TKey, TValue> GetNode(IEnumerable<TKey> key)
        {
            return GetNode(key, -1);
        }
        /// <summary>
        /// Ищет элемент по заданному ключу.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="keyPos">Позиция в ключе.</param>
        /// <returns>Возвращает элемент по заданному ключу.</returns>
        private Node<TKey, TValue> GetNode(IEnumerable<TKey> key, int keyPos)
        {
            keyPos++;
            if (keyPos == key.Count())
            {
                return this;
            }
            TKey currKey = key.Skip(keyPos).Take(1).First();
            if (leftChild == null)
                return null;
            else
            {
                var currNode = this.leftChild;
                Node<TKey, TValue> prevNode = null;
                while (currNode != null && currNode.key.CompareTo(currKey) < 0)
                {
                    prevNode = currNode;
                    currNode = currNode.rightSibling;
                }
                if (currNode != null && currNode.key.CompareTo(currKey) == 0)
                    return currNode.GetNode(key, keyPos);
                else return null;
            }
        }
    }
}
