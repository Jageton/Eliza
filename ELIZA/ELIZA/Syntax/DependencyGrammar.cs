using System;
using System.Collections.Generic;
using ELIZA.Morphology;
using System.Reflection;
using ELIZA.Syntax.DeepRelations;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax
{
    //возможно сделать статическим
    //(из соображений, что грамматика существует в единственном экземпляре)
    public static class DependencyGrammar
    {
        private static List<ISurfaceRelation> internalStrongRelations;
        private static List<ISurfaceRelation> internalWeakRelations;
        private static List<ISurfaceRelation> externalStrongRelations;
        private static List<ISurfaceRelation> externalWeakRelations;
        private static Dictionary<SurfaceRelationName, IDeepRelationBuilder> converters; 

        public static Tree<Lexem, SurfaceRelationName> NullVerbLexem
        {
            get
            {
                return new Tree<Lexem, SurfaceRelationName>(new Lexem(int.MaxValue,
                    "be", Tag.Infinitive | Tag.Phantom));
            }
        }

        /// <summary>
        /// Инициализирует класс <see cref="DependencyGrammar"/>.
        /// </summary>
        static DependencyGrammar()
        {
            internalStrongRelations = new List<ISurfaceRelation>();
            internalWeakRelations = new List<ISurfaceRelation>();
            externalStrongRelations = new List<ISurfaceRelation>();
            externalWeakRelations = new List<ISurfaceRelation>();
            InitializeSSR();
            //TODO: изменить на инциализацию на базе атрибутов
            converters = new Dictionary<SurfaceRelationName, IDeepRelationBuilder>();
            converters.Add(SurfaceRelationName.Predicative, new PredicativeConverter());
            converters.Add(SurfaceRelationName.Attributive, new AttributiveConverter());
            converters.Add(SurfaceRelationName.Completive1, new Completive1Converter());
            converters.Add(SurfaceRelationName.Completive2, new Completive2Converter());
            converters.Add(SurfaceRelationName.Quantitive, new QuantitiveConverter());
            converters.Add(SurfaceRelationName.Prepositional, new PrepositionalConverter());
            converters.Add(SurfaceRelationName.Attributive1, new Attributive1Converter());
            converters.Add(SurfaceRelationName.Service, new ServiceConverter());
        }

        /// <summary>
        /// Инициализирует списки ПСО.
        /// </summary>
        private static void InitializeSSR()
        {
            var type = typeof (SurfaceRelationName);
            foreach (var name in Enum.GetValues(type))
            {
                var memberInfo = type.GetMember(name.ToString());
                var useCases = memberInfo[0].GetCustomAttributes<SurfaceRelationUseCase>();
                foreach (var useCase in useCases)
                {
                    if (useCase != null)
                    {
                        var pso = useCase.Relation;
                        switch (useCase.Usage)
                        {
                            case (SurfaceRelationUsage.Inner):
                                InsertInternal(pso, useCase.Type);
                                break;
                            case (SurfaceRelationUsage.External):
                                InsertExternal(pso, useCase.Type);
                                break;
                            default:
                                InsertInternal(pso, useCase.Type);
                                InsertExternal(pso, useCase.Type);
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Вставляет заданно ПСО в список внешних ПСО.
        /// </summary>
        /// <param name="rule">ПСО.</param>
        /// <param name="type">Тип ПСО.</param>
        private static void InsertExternal(ISurfaceRelation rule, SurfaceRelationType type)
        {
            if (type == SurfaceRelationType.Strong)
                externalStrongRelations.Add(rule);
            else externalWeakRelations.Add(rule);
        }
        /// <summary>
        /// Вставляет заданно ПСО в список внутренних ПСО.
        /// </summary>
        /// <param name="rule">ПСО.</param>
        /// <param name="type">Тип ПСО.</param>
        private static void InsertInternal(ISurfaceRelation rule, SurfaceRelationType type)
        {
            if (type == SurfaceRelationType.Strong)
                internalStrongRelations.Add(rule);
            else internalWeakRelations.Add(rule);
        }

        /// <summary>
        /// Пытается построить поверхностсон-синтаксическое отношение между двумя синтаксическими 
        /// деревьями.
        /// </summary>
        /// <param name="first">Первое дерево.</param>
        /// <param name="second">Второе дерево.</param>
        /// <param name="head">Главное дерево в отношении.</param>
        /// <param name="usage">Внешнее или внутренне отношение.</param>
        /// <param name="type">Тип отношения.</param>
        /// <returns>Возвращает <c>true</c>, если удалось построить отношение, 
        /// иначе возвращает <c>false</c>.</returns>
        public static bool BuildDependency(Tree<Lexem, SurfaceRelationName> first, 
            Tree<Lexem, SurfaceRelationName> second,
            out Tree<Lexem, SurfaceRelationName> head,
            SurfaceRelationUsage usage = SurfaceRelationUsage.Inner, 
            SurfaceRelationType type = SurfaceRelationType.Weak)
        {
            head = null;
            List<ISurfaceRelation> workingSet = null;
            if (usage == SurfaceRelationUsage.External)
            {
                if (type == SurfaceRelationType.Strong)
                    workingSet = externalStrongRelations;
                else workingSet = externalWeakRelations;
            }
            else
            {
                if (type == SurfaceRelationType.Strong)
                    workingSet = internalStrongRelations;
                else workingSet = internalWeakRelations;
            }
            foreach (var rule in workingSet)
            {
                if (rule.BuildRelation(first, second, out head))
                    return true;
            }
            return false;
        }

        public static Tree<DForm, DeepRelationName> Convert(Tree<Lexem, SurfaceRelationName> node,
            Tree<Lexem, SurfaceRelationName> parent)
        {
            //метод будет вызываться рекурсивно
            //если определён конвертер
            if (converters.ContainsKey(node.DependencyType))
            {
                //транслируем в DD-дерево
                return converters[node.DependencyType].Convert(node, parent);
            }
            //иначе трансляция не требуется
            return null;
        }

        public static Tree<DForm, DeepRelationName> Convert(Tree<Lexem, SurfaceRelationName> node)
        {
            return Convert(node, null);
        }
    }
}
