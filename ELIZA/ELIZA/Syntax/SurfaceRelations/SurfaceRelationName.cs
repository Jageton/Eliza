namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Тип поверхностно-синтаксического отношения.
    /// </summary>
    public enum SurfaceRelationName
    {
        /// <summary>
        /// Предикативное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(PredicativeSSR),
            SurfaceRelationType.Strong, SurfaceRelationUsage.Inner)]
        [SurfaceRelationUseCase(typeof(PredicativeWeakSSR),
            SurfaceRelationType.Weak, SurfaceRelationUsage.Inner)]
        Predicative,
        /// <summary>
        /// Первое комплетивное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(FirstCompletiveSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Inner)]
        [SurfaceRelationUseCase(typeof(FormulaSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Inner)]
        Completive1,
        /// <summary>
        /// Второе комплетивное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(AdditionalComplementarySSR), SurfaceRelationType.Weak,
            SurfaceRelationUsage.Inner)]
        Completive2,
        /// <summary>
        /// Агентивное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(AgentiveSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Inner)]
        Agentive,
        /// <summary>
        /// Определительное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(AttributiveSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Any)]
        Attributive,
        /// <summary>
        /// Аттрибутивное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(Attributive1SSR), SurfaceRelationType.Weak,
            SurfaceRelationUsage.Any)]
        Attributive1,
        /// <summary>
        /// Первое апозитивное ПСО.
        /// </summary>
        Appositive1,
        /// <summary>
        /// Второе апозитивное ПСО.
        /// </summary>
        Appositive2,
        /// <summary>
        /// Количественное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(QuantitiveSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Inner)]
        Quantitive,
        /// <summary>
        /// Ограничительное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(RestrictiveSSR), SurfaceRelationType.Weak,
            SurfaceRelationUsage.Inner)]
        Restictive,
        /// <summary>
        /// Обстоятельственное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(AdverbalSSR), SurfaceRelationType.Weak,
            SurfaceRelationUsage.Any)]
        Adverbial,
        /// <summary>
        /// Вводное ПСО.
        /// </summary>
        Introductory,
        /// <summary>
        /// Сравнительное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(ComparativeSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Any)]
        Comparative,
        /// <summary>
        /// Сравнительно-субъективное ПСО.
        /// </summary>
        ComparativeSubjective,
        /// <summary>
        /// Сравнительно-комплетивное ПСО.
        /// </summary>
        ComparativeCompletive1,
        /// <summary>
        /// Сравнительно-комплетивное ПСО 2.
        /// </summary>
        ComparativeCompletive2,
        /// <summary>
        /// Сравнительно-комплетивное ПСО 3.
        /// </summary>
        ComparativeCompletive3,
        /// <summary>
        /// Сравнительно-обстоятельственное ПСО.
        /// </summary>
        ComparativeAdverbal,
        /// <summary>
        /// Элективное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(ElectiveSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Inner)]
        Elective,
        /// <summary>
        /// Субъектно-копредикативное ПСО.
        /// </summary>
        SubjectiveCopredicative,
        /// <summary>
        /// Комплетивно-предикативное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(CompletivePredicativeSSR), SurfaceRelationType.Weak,
            SurfaceRelationUsage.Inner)]
        CompletivePredicative,
        /// <summary>
        /// Отпредложное (отсоюзное) ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(PrepositionalSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Any)]
        Prepositional,
        /// <summary>
        /// Однородное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(HomogeniousSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Any)]
        Homogenious,
        /// <summary>
        /// Присвязочно-генитивное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(CopularGenitiveSSR), SurfaceRelationType.Weak,
            SurfaceRelationUsage.Inner)]
        CopularGenitive,
        /// <summary>
        /// Союзно-инфинитивное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(ConjunctiveInfinitiveSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Inner)]
        ConjunctiveInfinitive,
        /// <summary>
        /// Целевое ПСО.
        /// </summary>
        Target,
        /// <summary>
        /// Аппроксимативное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(ApproximativeSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Inner)]
        Approximative,
        /// <summary>
        /// Кратное ПСО.
        /// </summary>
        Multiple,
        /// <summary>
        /// Служебное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(ServiceSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Any)]
        Service,
        /// <summary>
        /// Первое вспомогательное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(Accessory1SSR), SurfaceRelationType.Weak,
            SurfaceRelationUsage.Any)]
        Accessory1,
        /// <summary>
        /// Второе вспомогательое ПСО.
        /// </summary>
        Accessory2,
        /// <summary>
        /// Инфинитивно-модальное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(InfinitiveModalSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.Any)]
        InfinitiveModal,
        /// <summary>
        /// Изъяснительное.
        /// </summary>
        [SurfaceRelationUseCase(typeof(IndicativeSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.External)]
        Indicatvie,
        /// <summary>
        /// Сопоставительное.
        /// </summary>
        [SurfaceRelationUseCase(typeof(CorrelativeSSR), SurfaceRelationType.Strong,
            SurfaceRelationUsage.External)]
        Correlative,
        /// <summary>
        /// Адресатное ПСО.
        /// </summary>
        [SurfaceRelationUseCase(typeof(TargetedSSR), SurfaceRelationType.Weak,
            SurfaceRelationUsage.Inner)]
        Targeted
    }
}
