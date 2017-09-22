using ELIZA.Morphology;

namespace ELIZA.Syntax.SurfaceRelations
{
    public class FormulaSSR: AbstractSSR
    {
        protected override bool TryBuildRelation(Tree<Lexem, SurfaceRelationName> first,
            Tree<Lexem, SurfaceRelationName> second, out Tree<Lexem, SurfaceRelationName> head)
        {
            Lexem f = first.Key;
            Lexem s = second.Key;
            head = first;
            if ((s.Tag & Tag.Phantom) != 0)
            {
                if (f.Lemma == "уравнение")
                {
                    first.AddChild(second, SurfaceRelationName.Completive1);
                    return true;
                }
            }
            return false;
        }
    }
}
