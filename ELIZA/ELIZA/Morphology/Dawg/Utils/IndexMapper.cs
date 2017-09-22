using System;
using System.Collections.Generic;

namespace ELIZA.Morphology.Dawg.Utils
{
    /// <summary>
    /// This mapper maps instances of type <see cref="TSource"/> to their index of type <see cref="TTarget"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target that should be one of the numeric types.
    /// Maximum of values allowed is based upon numeric types property "MaxValue". If <see cref="TTarget"/> 
    /// is not a numberic type then mapper will not allow any instances and will alwasy throw 
    /// <see cref="IndexOutOfRangeException"/>.</typeparam>
    /// <seealso cref="IMappingStrategy{TSource,TTarget}" />
    public class IndexMapper<TSource, TTarget>: IMappingStrategy<TSource, TTarget>
        where TTarget: struct, IConvertible
    {
        protected ulong valuesAllowed;
        protected ulong count;
        protected Dictionary<TSource, TTarget> instances;
        protected Dictionary<TTarget, TSource> revInstances;


        /// <summary>
        /// Initializes a new instance of the <see cref="IndexMapper{TSource, TTarget}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public IndexMapper(IEqualityComparer<TSource> comparer = null)
        {
            switch (Type.GetTypeCode(typeof(TTarget)))
            {
                case TypeCode.Byte:
                    valuesAllowed = byte.MaxValue;
                    break;
                case TypeCode.Char:
                    valuesAllowed = char.MaxValue;
                    break;
                case TypeCode.Int32:
                    valuesAllowed = int.MaxValue;
                    break;
                case TypeCode.Int64:
                    valuesAllowed = long.MaxValue;
                    break;
                case TypeCode.UInt16:
                    valuesAllowed = ushort.MaxValue;
                    break;
                case TypeCode.UInt32:
                    valuesAllowed = uint.MaxValue;
                    break;
                case TypeCode.UInt64:
                    valuesAllowed = ulong.MaxValue;
                    break;
                default:
                    valuesAllowed = 0; // don't allow any values
                    break;
            }
            instances = new Dictionary<TSource, TTarget>();
            revInstances = new Dictionary<TTarget, TSource>();
            count = 0;
        }

        /// <summary>
        /// Maps the specified object to another object of <see cref="TTarget"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Returns the corresponding instance of <see cref="TTarget"/>.</returns>
        public virtual TTarget Map(TSource obj)
        {
            if (!instances.ContainsKey(obj))
            {
                if(count == valuesAllowed)
                    throw new IndexOutOfRangeException(string.Format("This mapper allows only {0} unique instances.", valuesAllowed));
                var conv = Convert(count);
                instances.Add(obj, conv);
                revInstances.Add(conv, obj);
                count++;
            }
            return instances[obj];
        }

        public virtual TSource Map(TTarget obj)
        {
            return revInstances[obj];
        }

        /// <summary>
        /// Converts the ulong index to instance of type <see cref="TTarget"/>. The <see cref="TTarget"/> must be 
        /// one of the base numberic types.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Returns the corresponding instance of <see cref="TTarget"/>.</returns>
        protected TTarget Convert(ulong index)
        {
            return (TTarget)System.Convert.ChangeType(index, typeof (TTarget));
        }
    }
}
