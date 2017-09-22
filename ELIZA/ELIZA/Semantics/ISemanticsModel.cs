using System.Collections.Generic;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics
{
    public interface ISemanticsModel
    {
        string GetAnswer(List<Tree<DForm, DeepRelationName>> trees);
    }
}
