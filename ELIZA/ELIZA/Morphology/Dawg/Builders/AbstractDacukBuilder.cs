using System;
using System.Collections.Generic;
using System.Linq;
using ELIZA.Morphology.Dawg.Utils;

namespace ELIZA.Morphology.Dawg.Builders
{
    [Serializable]
    public abstract class AbstractDacukBuilder<TKey, TValue>: IIncrementalDawgBuilder<Dawg<TKey, TValue>, TKey, TValue>
        where TKey: IComparable
    {
        protected Dictionary<IDawgNode<TKey, TValue>, IDawgNode<TKey, TValue>> registry; //state registry
        protected Dawg<TKey, TValue> instance; //constructed instance
        protected IDawgNode<TKey, TValue> confluxState; //first conflux state encountered
        protected IDawgNode<TKey, TValue> lastState; //last state in longest common prefix path
        protected int prefixLenght; //lenght of longest common prefix
        protected Stack<IDawgNode<TKey, TValue>> prefixPath; //prefix path
        protected IEqualityComparer<TValue> comparer; //optional value comparer 

        public Dawg<TKey, TValue> Instance
        {
            get
            {
                //we need to clone registry and registered flags
                var registryCopy = new Dictionary<IDawgNode<TKey, TValue>, IDawgNode<TKey, TValue>>();
                foreach (var node in registry)
                    registryCopy.Add(node.Key, node.Value);
                var registeredCopy = (RegisteredPropertyHandler)Registered.Clone();
                var instanceCopy = (Dawg<TKey, TValue>)instance.Clone();               
                //call finalizer
                ReplaceOrRegister(instance.Root);
                var instanceToReturn = instance;
                //then revert back
                registry = registryCopy;
                Registered = registeredCopy;
                instance = instanceCopy;
                return instanceToReturn;
            }
        }

        //indexed property implementation; just syntax sugar
        protected RegisteredPropertyHandler Registered { get; private set; }

        protected AbstractDacukBuilder()
        {
            registry = new Dictionary<IDawgNode<TKey, TValue>, IDawgNode<TKey, TValue>>();
            instance = new Dawg<TKey, TValue>();
            confluxState = null;
            lastState = null;
            comparer = null;
            prefixLenght = 0;
            prefixPath = new Stack<IDawgNode<TKey, TValue>>();
            Registered = new RegisteredPropertyHandler();
        }

        public virtual Dawg<TKey, TValue> Build(IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> data)
        {
            registry = new Dictionary<IDawgNode<TKey, TValue>, IDawgNode<TKey, TValue>>();
            instance = new Dawg<TKey, TValue>();
            foreach(var pair in data)
                Append(pair.Key, pair.Value);
            return instance;
        }
        public abstract void Append(IEnumerable<TKey> key, TValue value);

        /// <summary>
        /// Finds the longest common prefix of the specified key. Sets values of 
        /// <see cref="confluxState"/>, <see cref="lastState"/>, <see cref="prefixPath"/> and <see cref="prefixLenght"/>.
        /// <see cref="confluxState"/> gets the first conflux state encountered (null if no such state exists).
        /// <see cref="lastState"/> gets the last state encountered (null if no prefix exists).
        /// <see cref="prefixPath"/> gets the values of the states encountered.
        /// <see cref="prefixLenght"/> gets the lenght of longest common prefix (0 if no prefix exists).
        /// </summary>
        /// <param name="key">The key to search longest common preifix of.</param>
        protected void CommonPrefix(IEnumerable<TKey> key)
        {
            prefixLenght = 0;
            lastState = instance.Root;
            confluxState = null;
            var confluxFound = false;
            prefixPath = new Stack<IDawgNode<TKey, TValue>>();
            foreach (var k in key)
            {
                //prefix can be continued
                if (lastState.Contains(k))
                {
                    lastState = lastState.Get(k);
                    prefixPath.Push(lastState);
                    //increment prefix lenght
                    prefixLenght++;
                    if (!confluxFound) //if conflux state was not found yet
                    {
                        //if lastState is conflux
                        if (lastState.Children.Count() > 1)
                        {
                            confluxFound = true;
                            confluxState = lastState;
                        }
                    }
                }
                else break;
            }         
        }
        /// <summary>
        /// Creates the branch for the specified key and value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns the root of newly created branch.</returns>
        protected IDawgNode<TKey, TValue> CreateBranch(IEnumerable<TKey> key, TValue value)
        {           
            key = key.Reverse();
            var keys = key as TKey[] ?? key.ToArray();
            IDawgNode<TKey, TValue> current = new EndLinkedDawgNode<TKey, TValue>(keys.First(), value);
            return keys.Skip(1).Aggregate(current,
                (current1, k) => new InnerDawgNode<TKey, TValue>(k, current1, null));
        }
        /// <summary>
        /// Replaces or registers newly created state.
        /// </summary>
        /// <param name="state">The state.</param>
        protected abstract void ReplaceOrRegister(IDawgNode<TKey, TValue> state);

        protected class RegisteredPropertyHandler: ICloneable
        {
            protected Dictionary<IDawgNode<TKey, TValue>, bool> registred;

            public bool this[IDawgNode<TKey, TValue>state]
            {
                get { return registred.ContainsKey(state) && registred[state]; }
                set
                {
                    if (value)
                    {
                        if (!registred.ContainsKey(state))
                            registred.Add(state, true);
                        else
                            registred[state] = true;
                    }
                    else
                    {
                        if (registred.ContainsKey(state))
                            registred.Remove(state);
                    }
                }
            }

            public RegisteredPropertyHandler()
            {
                registred = new Dictionary<IDawgNode<TKey, TValue>, bool>
                    (new StateReferenceComparer<TKey, TValue>());
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:System.Object"/> class.
            /// </summary>
            protected RegisteredPropertyHandler(Dictionary<IDawgNode<TKey, TValue>, bool> registred)
            {
                this.registred = registred;
            }

            /// <summary>
            /// Creates a new object that is a copy of the current instance.
            /// </summary>
            /// <returns>
            /// A new object that is a copy of this instance.
            /// </returns>
            public object Clone()
            {
                var copyRegistred = registred.ToDictionary(pair => pair.Key, pair => pair.Value);
                return new RegisteredPropertyHandler(copyRegistred);
            }
        }
    }
}
