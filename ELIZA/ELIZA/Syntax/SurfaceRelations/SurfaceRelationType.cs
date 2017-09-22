namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Тип поверхностно-синтаксического отношения.
    /// </summary>
    public enum SurfaceRelationType
    {
        /// <summary>
        /// "Сильное отношение". Легко формализуется. Определеяется в предложении в явном виде.
        /// </summary>
        Strong,
        /// <summary>
        /// "Слабое отношение". Сложно формализуется.
        /// Определяется с использованием различных эвристик.
        /// </summary>
        Weak
    }
}
