using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace ELIZA.Morphology
{
    /// <summary>
    /// Модель n-грамм.
    /// </summary>
    /// <typeparam name="TKey">Тип элементов n-граммы.</typeparam>
    [Serializable]
    [ProtoContract]
    public class NGramm<TKey> where TKey: IComparable
    {
        [ProtoMember(1, IsRequired=true)]
        private ulong count;
        [ProtoMember(2, IsRequired=true)]
        private Trie<TKey, ulong> trie; //префиксное дерево для хранения N-грамм
        
        /// <summary>
        /// Получает количество n-грамм.
        /// </summary>
        public ulong Count
        {
            get { return count; }
        }
        public Trie<TKey, ulong> Trie
        {
            get { return trie; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NGramm{TKey}"/> class.
        /// </summary>
        public NGramm()
        {
            trie = new Trie<TKey, ulong>();
            count = 0;
        }

        /// <summary>
        /// Добавляет новую n-грамму.
        /// </summary>
        /// <param name="key">n-грамма.</param>
        public void AddNGramm(IEnumerable<TKey> key)
        {
            count++;
            int n = key.Count();
           
            for(int i = n; i >= 1; i--) //заполним (1...n) граммы
            {
                IEnumerable<TKey> currKey = key.Skip(n - i).Take(i);
                if (trie.Contains(currKey))
                {
                    ulong number = trie.Get(currKey);
                    trie.Set(currKey, number + 1);
                }
                else
                {
                    trie.Set(currKey, 1);
                }
            }
        }
        /// <summary>
        /// Возвращает вероятность появления данной n-граммы.
        /// </summary>
        /// <param name="key">n-грамма.</param>
        /// <returns>Возвращает вероятность появления данной n-граммы.</returns>
        public double Compute(IEnumerable<TKey> key)
        {
            double number = trie.Contains(key) ? trie.Get(key) : 0;
            return number / count;
        }
    }
}
