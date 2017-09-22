using System;

namespace ELIZA.Syntax.SurfaceRelations
{
    /// <summary>
    /// Задаёт вариант использования поверхностно-синтаксического отношения.
    /// Применяется к элементам перечисления <see cref="SurfaceRelationName"/>.
    /// Задаёт тип отношения, ситуацию, когда оно используется, и правило для построения.
    /// Отсутствие данного аттрибута у элемента перечисления означает, что данное ПСО не 
    /// реализовано или объеденино с другим ПСО по различным соображениям.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    internal sealed class SurfaceRelationUseCase : Attribute
    {
        private SurfaceRelationUsage usage;
        private SurfaceRelationType type;
        private ISurfaceRelation relation;

        /// <summary>
        /// Получает правило построения отношения..
        /// </summary>
        public ISurfaceRelation Relation
        {
            get { return relation; }
            set { relation = value; }
        }
        /// <summary>
        /// Получает или задаёт тип поверхностно-синтаксического отношения.
        /// </summary>
        public SurfaceRelationType Type
        {
            get { return type; }
            set { type = value; }
        }
        /// <summary>
        /// Получает или задаёт использование поверхностно-синтаксического отношения..
        /// </summary>
        public SurfaceRelationUsage Usage
        {
            get { return usage; }
            set { usage = value; }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SurfaceRelationUseCase"/>.
        /// </summary>
        /// <param name="t">Тип, реализующий потроение ПСО.</param>
        /// <param name="type">Тип ПСО.</param>
        /// <param name="usage">Использование ПСО.</param>
        public SurfaceRelationUseCase (Type t,
            SurfaceRelationType type = SurfaceRelationType.Strong,
            SurfaceRelationUsage usage = SurfaceRelationUsage.Any)
        {
            Relation = (ISurfaceRelation)Activator.CreateInstance(t);
            Type = type;
            Usage = usage;
        }
    }
}
