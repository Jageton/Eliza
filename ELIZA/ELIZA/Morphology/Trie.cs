using System;
using System.Collections.Generic;
using ProtoBuf;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Предствляет префиксное дерево.
    /// </summary>
    /// <typeparam name="TKey">Тип ключа.</typeparam>
    /// <typeparam name="TValue">Тип значения.</typeparam>
    [ProtoContract]
    [Serializable]
    [ProtoInclude(12, typeof(SparseStringTrie<ulong>))]
    [ProtoInclude(13, typeof(SparseStringTrie<ushort>))]
    public class Trie<TKey, TValue> where TKey: IComparable
    {
        [ProtoMember(1)]
        protected Node<TKey, TValue> root;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Trie{TKey, TValue}"/>.
        /// </summary>
        public Trie()
        {
            root = new Node<TKey, TValue>();
        }
        
        /// <summary>
        /// Устаналивает значение для заданного ключа.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        public void Set(IEnumerable<TKey> key, TValue value)
        {
            root.Set(key, value);
        }
        /// <summary>
        /// Проверяет, соедржит ли дерево заданный ключ.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Возвращает значение, показывающее, содержит ли текущее дерево заданный ключ.</returns>
        public bool Contains(IEnumerable<TKey> key)
        {
            return root.Contains(key);
        }
        /// <summary>
        /// Получает значение по ключу.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Возвращает значение с заданным ключём.</returns>
        public TValue Get(IEnumerable<TKey> key)
        {
            return root.Get(key);
        }
        /// <summary>
        /// Получает коллецию ключей, дочерних по отношению к заданному ключу.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Возвращает коллецию ключей, дочерних по отношению к заданному ключу.</returns>
        public IEnumerable<Node<TKey, TValue>> GetChildNodes(IEnumerable<TKey> key)
        {
            var node = root.GetNode(key);
            if (node != null)
                return node.ChildCollection;
            else return new List<Node<TKey, TValue>>();
        }
    }
}
