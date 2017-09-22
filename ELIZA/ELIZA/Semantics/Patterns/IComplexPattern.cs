using System.Collections.Generic;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns
{
    public interface IComplexPattern
    {
        bool Match(List<Tree<DForm, DeepRelationName>> trees);
    }
}
