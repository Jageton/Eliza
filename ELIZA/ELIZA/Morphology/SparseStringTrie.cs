using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace ELIZA.Morphology
{
    [Serializable]
    [ProtoContract]
    [ProtoInclude(101010, typeof(TagTrie))]
    /// <summary>
    /// Разреженное префиксное дерево со строковыми ключами.
    /// </summary>
    /// <typeparam name="TValue">Тип значения элементов..</typeparam>
    /// <seealso cref="Morphology.Trie{System.String,TValue}" />
    public class SparseStringTrie<TValue>: Trie<string, TValue>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SparseStringTrie{TValue}"/>.
        /// </summary>
        public SparseStringTrie()
        {
            this.root = new SparseNode<TValue>();
            this.root.Key = ".";
        }

        /// <summary>
        /// Сжимает заданное дерево за счёт удаления элементов, не соответствующих ключам.
        /// </summary>
        public void Reduce()
        {
            foreach(var child in this.root.ChildCollection)
            {
                ((SparseNode<TValue>)child).Reduce();
            }
        }
        /// <summary>
        /// Устаналивает заданное значения для заданного строкового ключа.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(string key, TValue value)
        {
            IEnumerable<string> strKey = key.ToCharArray().Select(a => a.ToString());
            Set(strKey, value);
        }
        /// <summary>
        /// Проверяет, соедржит ли дерево заданный ключ.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Возвращает значение, показывающее, содержит ли текущее дерево заданный ключ.</returns>
        public bool Contains(string key)
        {
            return this.Contains(key.Select(a => a.ToString()));
        }
        /// <summary>
        /// Получает значение по заданному ключу.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <returns>Возвращает значение по заданному ключу.</returns>
        public TValue Get(string key)
        {
            return root.Get(key.ToCharArray().Select(c => c.ToString()));
        }
    }
}
