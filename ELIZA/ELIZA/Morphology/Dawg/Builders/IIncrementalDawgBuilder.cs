using System;
using System.Collections.Generic;

namespace ELIZA.Morphology.Dawg.Builders
{
    public interface IIncrementalDawgBuilder<out TDawg, TKey, TValue>: IDawgBuilder<TDawg, TKey, TValue>
        where TDawg: IDawg<TKey, TValue>
        where TKey : IComparable
    {
        void Append(IEnumerable<TKey> key, TValue value);
        //instance can be returned at any time
        TDawg Instance { get; }
    }
}
