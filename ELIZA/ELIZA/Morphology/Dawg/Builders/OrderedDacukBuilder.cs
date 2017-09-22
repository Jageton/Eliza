using System;
using System.Collections.Generic;
using System.Linq;

namespace ELIZA.Morphology.Dawg.Builders
{
    public class OrderedDacukBuilder<TKey, TValue>: AbstractDacukBuilder<TKey, TValue> where TKey: IComparable
    {
        protected TKey[] prevKey;

        public OrderedDacukBuilder()
        {
            prevKey = new TKey[0];
        }

        public override Dawg<TKey, TValue> Build(IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> data)
        {
            //the data for this method must be ordered
            var ordered = data.OrderBy((c) => c.Key);
            base.Build(ordered);
            ReplaceOrRegister(instance.Root);
            return instance;
        }

        public override void Append(IEnumerable<TKey> key, TValue value)
        {
            var keys = key as TKey[] ?? key.ToArray(); //enumerate to array
            if(!CheckOrderedCondition(keys)) //check ordered condition
                throw new DawgException(string.Format("The key {0} violates ordered condition.", key));
            CommonPrefix(keys); //find the biggest common prefix
            if (lastState.Children.Any()) //last state is the last state in the biggest common prefix
                ReplaceOrRegister(lastState); //replace or register states
            var suffix = CreateBranch(keys.Skip(prefixLenght), value); //create new branch
            lastState.Add(suffix); //append new branch to the last state
            prevKey = keys; //remember the last key
        }

        protected bool CheckOrderedCondition(TKey[] key)
        {
            for (var i = 0; i < prevKey.Length; i++)
            {
                //the key is obviosly lesser
                if (key[i].CompareTo(prevKey[i]) < 0) return false;
                //the key is obviosly bigger
                if (key[i].CompareTo(prevKey[i]) > 0) return true;
                //else continue investigation
            }
            return key.Length > prevKey.Length;
        }

        protected override void ReplaceOrRegister(IDawgNode<TKey, TValue> state)
        {
            var child = state.Children.Last(); //last child
            if (!Registered[child])
            {
                //if child has children
                if (child.Children.Any())
                    ReplaceOrRegister(child);
                //if such q exist that q is in registry and q == child
                if (registry.ContainsKey(child))
                {
                    //not registered state is replaced by equal registred one
                    //replacement is reference replacement
                    //states are equal, but in .Net world they are different objects
                    state.Remove(child.Key);
                    state.Add(registry[child]);
                }
                else
                {
                    //else this state becomes the registred one,
                    //meaning that it may be used later as a replacement
                    registry.Add(child, child);
                    Registered[child] = true;
                }
            }
        }
    }
}
