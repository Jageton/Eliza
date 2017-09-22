using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIZA.Morphology.Dawg.Builders
{
    public class IncrementalDawgBuilderDecorator<TDawg, TKey, TValue>:
        DawgBuilderDecorator<TDawg, TKey, TValue>, IIncrementalDawgBuilder<TDawg, TKey, TValue>
        where TDawg : IDawg<TKey, TValue>
        where TKey : IComparable
    {
        protected new IIncrementalDawgBuilder<TDawg, TKey, TValue> innerBuilder;
        public new IIncrementalDawgBuilder<TDawg, TKey, TValue> InnerBuilder
        {
            get
            {
                return innerBuilder;
            }
            private set
            {
                if (value == null) throw new ArgumentNullException("value");
                innerBuilder = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public IncrementalDawgBuilderDecorator(IIncrementalDawgBuilder<TDawg, TKey, TValue> innerBuilder) : base(innerBuilder)
        {

        }

        public void Append(IEnumerable<TKey> key, TValue value)
        {
            innerBuilder.Append(key, value);
        }
        public TDawg Instance
        {
            get { return innerBuilder.Instance; }
        }
    }
}
