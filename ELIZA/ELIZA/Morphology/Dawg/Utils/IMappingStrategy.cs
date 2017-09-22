namespace ELIZA.Morphology.Dawg.Utils
{
    /// <summary>
    /// Represents the functionallity to map object of one type to object of another type.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public interface IMappingStrategy<TSource, TTarget>
    {
        /// <summary>
        /// Maps the specified object to another object of <see cref="TTarget"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Returns the corresponding instance of <see cref="TTarget"/>.</returns>
        TTarget Map(TSource obj);
        /// <summary>
        /// Maps the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Returns the corresponding instance of <see cref="TSource"/></returns>
        TSource Map(TTarget obj);
    }
}
