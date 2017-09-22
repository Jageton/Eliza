using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELIZA.Morphology.Dawg.Builders
{
    public abstract class DawgBuilderDecorator<TDawg, TKey, TValue>:
        IDawgBuilder<TDawg, TKey, TValue> 
        where TKey: IComparable 
        where TDawg : IDawg<TKey, TValue>
    {
        protected IDawgBuilder<TDawg, TKey, TValue> innerBuilder;
        public IDawgBuilder<TDawg, TKey, TValue> InnerBuilder
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
        protected DawgBuilderDecorator(IDawgBuilder<TDawg, TKey, TValue> innerBuilder)
        {
            this.innerBuilder = innerBuilder;
        }
        /// <summary>
        /// Builds the specified type of dawg from the specified data set.
        /// </summary>
        /// <param name="data">The data set.</param>
        /// <returns>Returns the builded dafsa.</returns>
        public virtual TDawg Build(IEnumerable<KeyValuePair<IEnumerable<TKey>, TValue>> data)
        {
            return innerBuilder.Build(data);
        }
    }
}
