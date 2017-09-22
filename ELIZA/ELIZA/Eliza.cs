using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ELIZA.Abstract;
using ELIZA.Morphology;
using ELIZA.Semantics;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA
{
    public class Eliza: AbstractEliza
    {
        public bool AppendMorphologicalResults { get; set; }
        public bool AppendSyntaxResults { get; set; }
        public bool AppendSemanticsResults { get; set; }

        public bool AppendInternalAnalysisResults
        {
            get { return AppendSyntaxResults | AppendSyntaxResults | AppendSemanticsResults; }
            set
            {
                AppendInternalAnalysisResults = value;
                AppendSemanticsResults = value;
                AppendSyntaxResults = value;
            }
        }

        public Eliza(MorphologyModel model): base(new LanguageModel(model))
        {

        }
        public Eliza(MorphologyModel morphModel, ISyntaxModel syntModel,
            ISemanticsModel semModel):
            base(new LanguageModel(morphModel, syntModel, semModel))
        {
            
        }

        public override string GetResponse(string input)
        {
            var sb = new StringBuilder();
            var sentences = langModel.MorphologyModel.SplitIntoSentences(input).ToArray();
            var parsedSentences = langModel.MorphologyModel.Predict(input);
            if (AppendMorphologicalResults)
                sb.AppendLine("Найдены следующие предложения:");
            var trees = new List<Tree<DForm, DeepRelationName>>();
            for (var i = 0; i < sentences.Length; i++)
            {
                if (AppendMorphologicalResults)
                {
                    sb.AppendLine(sentences[i]);
                    sb.AppendLine("Найдены следующие лексемы:");
                    foreach (var lexem in parsedSentences[i])
                        sb.AppendLine(lexem.ToString());
                }
                try
                {
                    var surfaceTree = langModel.SyntaxModel.SurfaceAnalysis(parsedSentences[i]);
                    var deepTree = langModel.SyntaxModel.DeepAnalysis(surfaceTree);
                    if (AppendSyntaxResults)
                    {
                        sb.AppendLine("Дерево поверхностного разбора: ");
                        sb.AppendLine(surfaceTree.ToString());
                        sb.AppendLine("Дерево глубинного разбора: ");
                        sb.AppendLine(deepTree.ToString());                        
                    }
                    trees.Add(deepTree);
                }
                catch (Exception)
                {
                    sb.AppendLine("Не удалось построить синтаксическое дерево.");
                }
            }
            sb.AppendLine(langModel.SemanticsModel.GetAnswer(trees));
            return sb.ToString();
        }
    }
}
