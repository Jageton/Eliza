using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace ELIZA.Morphology
{
    [Serializable]
    [ProtoContract]
    /// <summary>
    /// Элемент разреженного префиксного дерева со строковыми ключами.
    /// </summary>
    /// <typeparam name="TValue">Тип элемента дерева.</typeparam>
    /// <seealso cref="Morphology.Node{System.String,TValue}" />
    public class SparseNode<TValue>: Node<string, TValue>
    {
        /// <summary>
        /// По возможности уменьшает дерево с корнем в данном элементе.
        /// </summary>
        public void Reduce()
        {
            if(!this.HasValue && this.LeftChild != null) //не содержит значения, и один сын => можно слить
            {
                if(this.LeftChild.RightSibling == null)
                {
                    ((SparseNode<TValue>)this.LeftChild).Reduce(this);
                }
            }
            else //сыновей много, попробуем слияние для каждого из них
            {
                foreach(var child in ChildCollection)
                {
                    ((SparseNode<TValue>)child).Reduce();
                }
            }
        }
        /// <summary>
        /// Уменьшает дерево, путём слияния текущего элемента с его родителем.
        /// </summary>
        /// <param name="parent">Родитель.</param>
        private void Reduce(SparseNode<TValue> parent)
        {
            this.Reduce(); //пытаемся слить с сыновьями
            parent.Key += this.Key; //сливаем ключи
            if(this.HasValue) //если содержит значение
            {
                parent.Value = this.Value; //результат слияния будет содержать значение
            }
            parent.LeftChild = this.LeftChild; //текущей элемент больше не нужен
            
        }
        /// <summary>
        /// Устаналивает значение для заданного ключа.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        /// <param name="keyPos">Позиция в ключе.</param>
        public override void Set(IEnumerable<string> key, TValue value, int keyPos = -1)
        {
            //идентичный алгоритм вставки,за исключением того, что вставляются экземпляры класса SparseNode
            keyPos++;
            if (keyPos == key.Count())
            {
                this.Value = value;
                return;
            }
            string currKey = key.Skip(keyPos).Take(1).First();
            if (this.LeftChild == null)
            {
                this.LeftChild = new SparseNode<TValue>();
                this.LeftChild.Key = currKey;
                this.LeftChild.Set(key, value, keyPos);
            }
            else
            {
                var currNode = this.LeftChild;
                Node<string, TValue> prevNode = null;
                while (currNode != null && !((SparseNode<TValue>)currNode).CheckKey(key, keyPos))
                {
                    prevNode = currNode;
                    currNode = currNode.RightSibling;
                }
                if (currNode != null && ((SparseNode<TValue>)currNode).CheckKey(key, keyPos))
                {
                    currNode.Set(key, value, keyPos);
                }
                else
                {
                    if (prevNode == null)
                    {
                        var temp = this.LeftChild;
                        this.LeftChild = new SparseNode<TValue>();
                        this.LeftChild.Key = currKey;
                        this.LeftChild.RightSibling = temp;
                        this.LeftChild.Set(key, value, keyPos);
                    }
                    else
                    {
                        prevNode.RightSibling = new SparseNode<TValue>();
                        prevNode.RightSibling.Key = currKey;
                        prevNode.RightSibling.Set(key, value, keyPos);
                        prevNode.RightSibling.RightSibling = currNode;
                    }
                }
            }
        }
        /// <summary>
        /// Проверяет, соедржит ли дерево заданный ключ.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="keyPos"></param>
        /// <returns>
        /// Возвращает значение, показывающее, содержит ли текущее дерево заданный ключ.
        /// </returns>
        public override bool Contains(IEnumerable<string> key, int keyPos)
        {
            keyPos++;
            if (keyPos == key.Count())
            {
                if (this.HasValue) return true;
                else return false;
            }
            string currKey = key.Skip(keyPos).Take(1).First();
            if (this.LeftChild == null)
                return false;
            else
            {
                var currNode = this.LeftChild;
                Node<string, TValue> prevNode = null;
                while (currNode != null && !((SparseNode<TValue>)currNode).CheckKey(key, keyPos))
                {
                    prevNode = currNode;
                    currNode = currNode.RightSibling;
                }
                if (currNode != null && currNode.Key.CompareTo(currKey) == 0)
                    return currNode.Contains(key, keyPos);
                else return false;
            }
        }
        /// <summary>
        /// Получает значение по ключу.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="keyPos"></param>
        /// <returns>
        /// Возвращает значение с заданным ключём.
        /// </returns>
        public override TValue Get(IEnumerable<string> key, int keyPos)
        {
            keyPos++;
            if (keyPos == key.Count())
            {
                if (this.HasValue) return this.Value;
                else return default(TValue);
            }
            string currKey = key.Skip(keyPos).Take(1).First();
            if (this.LeftChild == null)
                return default(TValue);
            else
            {
                var currNode = this.LeftChild;
                Node<string, TValue> prevNode = null;
                while (currNode != null && !((SparseNode<TValue>)currNode).CheckKey(key, keyPos))
                {
                    prevNode = currNode;
                    currNode = currNode.RightSibling;
                }
                if (currNode != null && ((SparseNode<TValue>)currNode).CheckKey(key, keyPos))
                {
                    keyPos += currNode.Key.Length - 1;
                    return currNode.Get(key, keyPos);
                }
                else return default(TValue);
            }
        }
        /// <summary>
        /// Проверяет, содержится ли ключ текущего элемента в заданном ключе, начиная с заданной позиции.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="keyPos">Позиция в ключе.</param>
        /// <returns>Возвращает <c>true</c>, если ключ найден, иначе - <c>false</c>.</returns>
        private bool CheckKey(IEnumerable<string> key, int keyPos)
        {
            for (int i = 1; i <= this.Key.Length; i++)
            {
                char c = key.Skip(keyPos).Take(i).Last()[0];
                if (this.Key[i - 1] != c) return false;
            }
            return true;
        }
    }
}