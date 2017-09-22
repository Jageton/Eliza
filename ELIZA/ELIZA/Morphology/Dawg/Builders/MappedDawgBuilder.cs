using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELIZA.Morphology.Dawg.Utils;

namespace ELIZA.Morphology.Dawg.Builders
{
    public class MappedDawgBuilder<TKey, TValue, TCKey, TCValue>:
        DawgBuilderDecorator<Dawg<TCKey, TCValue>, TCKey, TCValue>,
        IDawgBuilder<MappedDawg<TKey, TValue, TCKey, TCValue>, TKey, TValue> 
        where TKey: IComparable
        where TCKey: IComparable
    {
        protected readonly IMappingStrategy<TKey, TCKey> keyMapper;
        protected readonly IMappingStrategy<TValue, TCValue> valueMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MappedDawgBuilder(IDawgBuilder<Dawg<TCKey, TCValue>, TCKey, TCValue> innerBuilder,
            IMappingStrategy<TKey, TCKey> keyMapper,
            IMappingStrategy<TValue, TCValue> valueMapper) : base(innerBuilder)
        {
            this.keyMapper = keyMapper;
            this.valueMapper = valueMapper;
        }

        /// <summary>
        /// Builds the specified type of dawg from the specified data set.
        /// </summary>
        /// <param name="data">The data set.</param>
        /// <returns>Returns the builded dafsa.</returns>
        public MappedDawg<TKey, TValue, TCKey, TCValue> Build(IEnumerable<KeyValuePair<IEnumerable<TKey>,
            TValue>> data)
        {
            return new MappedDawg<TKey, TValue, TCKey, TCValue>(
                innerBuilder.Build(from pair in data 
                                   select new KeyValuePair<IEnumerable<TCKey>, TCValue>
                                       (from k in pair.Key select keyMapper.Map(k),
                                       valueMapper.Map(pair.Value))),
                                       keyMapper, valueMapper);
        }
    }
}
