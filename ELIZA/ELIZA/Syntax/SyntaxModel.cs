using System;
using System.Collections.Generic;
using System.Linq;
using ELIZA.Morphology;
using ELIZA.Syntax.DeepRelations;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax
{
    public class SyntaxModel: ISyntaxModel
    {
        public SyntaxModel()
        {

        }
     
        #region ISyntaxModel Members

        /// <summary>
        /// Выполяет поверхностный синтаксический анализ заданного предложения.
        /// </summary>
        /// <param name="sentence">Предложение.</param>
        /// <returns>
        /// Возвращает дерево поверхностного разбора.
        /// </returns>
        public Tree<Lexem, SurfaceRelationName> SurfaceAnalysis(List<Lexem> sentence)
        {
            var trees = sentence.Select(lexem =>
                new Tree<Lexem, SurfaceRelationName>(lexem)).ToList();
            trees = AnalyzeSegment(trees);
            //на данный момент нет реализации разрешения эллиптических конструкций
            if(trees.Count > 1)
                throw new ArgumentException("Предложение содержит эллиптические конструкции.",
                    "sentence");
            return trees[0];
        }
        /// <summary>
        /// Строит связи в заданном сегменте. 
        /// </summary>
        /// <param name="segment">Сегмент. Может быть предложением или частью предложения.</param>
        /// <returns>Возвращает список независимых вершин.</returns>
        private List<Tree<Lexem, SurfaceRelationName>> AnalyzeSegment(
            List<Tree<Lexem, SurfaceRelationName>> segment)
        {
            var result = new List<Tree<Lexem, SurfaceRelationName>>();
            //разбиваем на сегменты и строми межсегментные связи
            var segments = Segmentize(segment); 
            //попытаемся построить как можно больше межсегментных связей
            segments = MergeSegments(segments);

            //делаем вывод, что всё остальные сегменты, на самом деле, - один сегмент
            //такая ситуация возможна, если в предложении были однородные члены
            //или если знаки препинания были расставлены неправильно

            foreach(var seg in segments)
                result.AddRange(seg);

            //построим внутрисегментные связи
            //такая ситуация возможна, если один сегмент был разделён на две части другим сегментом
            //например в таком случае: 
            //<начало предложения>, <причастный оборот>, <конец предложения>
            result = BuildDependencies(result, SurfaceRelationType.Strong,
                            SurfaceRelationUsage.Inner);
            result = BuildDependencies(result, SurfaceRelationType.Weak,
                            SurfaceRelationUsage.Inner);
            result = BuildDependencies(result, SurfaceRelationType.Strong,
                            SurfaceRelationUsage.External);
            result = BuildDependencies(result, SurfaceRelationType.Weak,
                            SurfaceRelationUsage.External);
            return result;
        }
        /// <summary>
        /// Разбивает исходный сегмент на другие сегменты и строит внутрисегментные связи.
        /// </summary>
        /// <param name="segment">Исходный сегмент.</param>
        /// <returns>Возвращает спсиок сегментов, в которых построены внутрисегментные связи.</returns>
        private List<List<Tree<Lexem, SurfaceRelationName>>> Segmentize(
            List<Tree<Lexem, SurfaceRelationName>> segment)
        {
            var segments = new List<List<Tree<Lexem, SurfaceRelationName>>>();
            var i = 0;
            //разобъём на сегменты и построим внутрисегментные связи
            while (i < segment.Count)
            {
                var currentSegment = new List<Tree<Lexem, SurfaceRelationName>>();
                while (i <= segment.Count && (segment[i].Key.Tag & Tag.Punctuation) == 0)
                {
                    currentSegment.Add(segment[i]);
                    i++;
                }
                var parts = BuildDependencies(currentSegment, SurfaceRelationType.Strong,
                    SurfaceRelationUsage.Inner).OrderByDescending((a) => a.Key.LexemPosition).ToList();
                bool changed = true;
                while (changed)
                {
                    changed = false;
                    for (int first = 0; first < parts.Count; first++)
                    {
                        for (int second = 0; second < parts.Count; second++)
                        {
                            if(second == first)
                                continue;
                            var h = parts[first];
                            var a = new List<Tree<Lexem, SurfaceRelationName>>(parts[second].OrderBy((c) => c.Key.LexemPosition));
                            var t = BuildDependencies(ref h, a, SurfaceRelationUsage.Inner);
                            if (t) 
                            {
                                parts.RemoveAt(first);
                                changed = true;
                                break;
                            }
                        }
                        if (changed) break;
                    }                                        
                }
                segments.Add(parts);
                i++;
            }
            return segments;
        }
        /// <summary>
        /// Объединяет сегменты межсегментными связями.
        /// </summary>
        /// <param name="segments">Исходный список сегментов.</param>
        /// <returns>Возвращает список сегментов, с построенными межсегментными связями.</returns>
        private List<List<Tree<Lexem, SurfaceRelationName>>> MergeSegments(
            List<List<Tree<Lexem, SurfaceRelationName>>> segments)
        {
            bool united = true; //объединены ли на шаге хотя бы два сегмента
            while (united) //пока объединены не все сегменты, которые можно объединить
            {
                united = false;
                for (var first = 0; first < segments.Count; first++)
                {
                    for (var second = first + 1; second < segments.Count; second++)
                    {
                        var temp = new List<Tree<Lexem, SurfaceRelationName>>(segments[first]);
                        //сливаем два сегмента
                        temp.AddRange(segments[second]);
                        //пытаемся построить какие-нибудь межсегментные связи
                        temp = BuildDependencies(temp, SurfaceRelationType.Strong,
                            SurfaceRelationUsage.External);
                        temp = BuildDependencies(temp, SurfaceRelationType.Weak,
                            SurfaceRelationUsage.External);
                        if (temp.Count < segments[first].Count + segments[second].Count)
                        {
                            segments.RemoveAt(second);
                            segments[first] = temp;
                            united = true;
                            break;
                        }
                    }
                    if (united) break;
                }
            }
            return segments;
        }
        /// <summary>
        /// Алгоритм построения связей грамматики зависимостей.
        /// </summary>
        /// <param name="trees">Исходные поддеревья грамматики.</param>
        /// <param name="type">Тип строящихся связей.</param>
        /// <param name="usage">Внтрисегментные или межсегментные связи.</param>
        /// <returns>Возвращает список главных (независимых вершин).</returns>
        private List<Tree<Lexem, SurfaceRelationName>> BuildDependencies(
            List<Tree<Lexem, SurfaceRelationName>> trees,
            SurfaceRelationType type, SurfaceRelationUsage usage)
        {
            var headless = new List<Tree<Lexem, SurfaceRelationName>>();
            var all = new List<Tree<Lexem, SurfaceRelationName>>(trees);
            Tree<Lexem, SurfaceRelationName> head = null;            
            for (var i = 0; i < all.Count; i++) //для каждого слова в сегменте
            {
                var headOf = new List<int>();
                for (var j = 0; j < headless.Count; j++)
                {
                    //можно соединить, и главным словом является рассматриваемое слово
                    if (DependencyGrammar.BuildDependency(all[i], headless[j], out head, usage, type))
                    {
                        headOf.Add(j);
                        all[i] = head;
                        headless.RemoveAt(j);
                    }
                }
                var found = false; //не связан ни с чем
                var predecessor = i - 1;
                while (predecessor >= 0)
                {
                    if (!headOf.Contains(predecessor))
                    {
                        //можно связать с одним из ранее появившихся слов
                        if (DependencyGrammar.BuildDependency(all[predecessor], all[i], out head, usage, type))
                        {                            
                            if (!head.Key.Equals(all[predecessor]))
                            {
                                var index = headless.IndexOf(all[predecessor]);
                                if (index > -1)
                                    headless[index] = head;
                            }
                            all[predecessor] = head; //заменяем на построенное дерево
                            found = true; //связан с одним из предыдущих слов
                            break;
                        }
                    }
                    predecessor--;
                }
                if (!found) headless.Insert(0, all[i]); //не нашли родителя
            }
            return headless;
        }

        private bool BuildDependencies(
            ref Tree<Lexem, SurfaceRelationName> h, List<Tree<Lexem, SurfaceRelationName>> a,
            SurfaceRelationUsage usage)
        {
            var headless = new List<Tree<Lexem, SurfaceRelationName>>();
            headless.Add(h);
            var all = new List<Tree<Lexem, SurfaceRelationName>>(a);
            Tree<Lexem, SurfaceRelationName> head = null;
            for (var i = 0; i < all.Count; i++) //для каждого слова в сегменте
            {
                //можно соединить, и главным словом является рассматриваемое слово
                if (DependencyGrammar.BuildDependency(all[i], headless[0], out head, usage,
                    SurfaceRelationType.Weak))
                {
                    all[i] = head;
                    headless.RemoveAt(0);
                    return true;
                }
            }
            return false;
        }
        public Tree<DForm, DeepRelationName> DeepAnalysis(Tree<Lexem, SurfaceRelationName> tree)
        {
            return DependencyGrammar.Convert(tree, null);
        }
        #endregion
    }
}
