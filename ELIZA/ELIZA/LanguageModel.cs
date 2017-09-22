using System;
using ELIZA.Morphology;
using ELIZA.Syntax;
using ELIZA.Semantics;

namespace ELIZA
{
    [Serializable]
    public class LanguageModel
    {
        protected MorphologyModel morphologyModel;
        protected ISyntaxModel syntaxModel;
        protected ISemanticsModel semanticsModel;

        public MorphologyModel MorphologyModel
        {
            get { return morphologyModel; }
            set { morphologyModel = value; }
        }
        public ISyntaxModel SyntaxModel
        {
            get { return syntaxModel; }
            set { syntaxModel = value; }
        }
        public ISemanticsModel SemanticsModel
        {
            get { return semanticsModel; }
        }

        public LanguageModel(MorphologyModel morphModel)
        {
            this.morphologyModel = morphModel;
        }

        public LanguageModel(MorphologyModel morphModel, ISyntaxModel syntModel,
            ISemanticsModel semanticsModel)
        {
            morphologyModel = morphModel;
            syntaxModel = syntModel;
            this.semanticsModel = semanticsModel;
        }
    }
}
