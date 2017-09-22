using System;
using System.Collections.Generic;
using System.Linq;

namespace ELIZA.Syntax
{
    [Serializable]
    public class Tree<TKey, TDep> : IEnumerable<Tree<TKey, TDep>>
    {
        protected TKey key;
        protected Tree<TKey, TDep> leftChild;
        protected Tree<TKey, TDep> rightSibling;
        protected TDep dependencyType;

        public IEnumerable<Tree<TKey, TDep>> Children
        {
            get
            {
                var current = leftChild;
                while (current != null)
                {
                    yield return current;
                    current = current.rightSibling;
                }
            }
        }
        public IEnumerable<KeyValuePair<TDep, Tree<TKey, TDep>>> Dependencies
        {
            get
            {
                var current = leftChild;
                while (current != null)
                {
                    yield return new KeyValuePair<TDep, Tree<TKey, TDep>>(current.dependencyType, leftChild);
                    current = current.rightSibling;
                }
            }
        }
        public TKey Key
        {
            get { return this.key; }
            set { key = value; }
        }

        public TDep DependencyType
        {
            get { return dependencyType; }
            set { dependencyType = value; }
        }

        public Tree(TKey key = default(TKey), TDep dependency = default(TDep))
        {
            this.key = key;
            this.leftChild = null;
            this.rightSibling = null;
            dependencyType = dependency;
        }

        public void AddChild(TKey key = default(TKey), TDep dependency = default(TDep))
        {
            if (this.leftChild != null)
            {
                var child = new Tree<TKey, TDep>(key, dependency);
                Children.Last().rightSibling = child;
            }
            else
            {
                this.leftChild = new Tree<TKey, TDep>(key, dependency);
            }
        }

        public void AddChild(Tree<TKey, TDep> child)
        {
            if (this.leftChild != null)
            {
                Children.Last().rightSibling = child;
            }
            else
            {
                this.leftChild = child;
            }
        }
        public void AddChild(Tree<TKey, TDep> child, TDep dependency)
        {
            child.dependencyType = dependency;
            AddChild(child);
        }
        public void AddSibling(TKey key =  default(TKey), TDep dependency = default(TDep))
        {
            if (this.rightSibling != null)
            {
                var temp = this.rightSibling;
                this.rightSibling = new Tree<TKey, TDep>(key, dependency) {rightSibling = temp};
            }
            else
            {
                this.rightSibling = new Tree<TKey, TDep>(key, dependency);
            }
        }

        public void RemoveChild(Tree<TKey, TDep> child)
        {
            if (leftChild.Equals(child))
            {
                leftChild = leftChild.rightSibling;
            }
            else
            {
                var currChild = leftChild.rightSibling;
                var prevChild = leftChild;
                while (currChild != null && !currChild.Equals(child))
                {
                    
                }
            }
        }

        public override string ToString()
        {
            return this.TreeToString("", true, false);
        }

        private string TreeToString(string intent, bool last, bool writeDependency = true)
        {
            string result = intent;
            if (last)
            {
                result += "\\- ";
                intent += "  ";
            }
            else
            {
                result += "|- ";
                intent += "|  ";
                
            }
            if (writeDependency)
                result += string.Format("({0}) ", dependencyType) + " ";
            result += key.ToString();
            var childList = Children.ToList();
            for (var i = 0; i < childList.Count; i++)
            {
                result += Environment.NewLine + childList[i].TreeToString(intent, i == childList.Count - 1);
            }         
            return result;
        }

        public IEnumerator<Tree<TKey, TDep>> GetEnumerator()
        {
            var stack = new Stack<Tree<TKey, TDep>>();
            stack.Push(this);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                if(current.leftChild != null)
                    stack.Push(current.leftChild);
                if(current.rightSibling != null)
                    stack.Push(current.rightSibling);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
