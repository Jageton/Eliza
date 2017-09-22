namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Возможность использования поверхностно-синтаксических связей.
    /// </summary>
    public enum SurfaceRelationUsage
    {
        /// <summary>
        /// Только внутрисегментное.
        /// </summary>
        Inner,
        /// <summary>
        /// Только межсегментное.
        /// </summary>
        External,
        /// <summary>
        /// Любое.
        /// </summary>
        Any
    }
}
