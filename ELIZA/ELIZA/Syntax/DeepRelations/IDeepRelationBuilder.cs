using ELIZA.Morphology;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax.DeepRelations
{
    public interface IDeepRelationBuilder
    {
        Tree<DForm, DeepRelationName> Convert(Tree<Lexem, SurfaceRelationName> node,
            Tree<Lexem, SurfaceRelationName> parent);

        Tree<DForm, DeepRelationName> Convert(Tree<Lexem, SurfaceRelationName> node);
    }
}
