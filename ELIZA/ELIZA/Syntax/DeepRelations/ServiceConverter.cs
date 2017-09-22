using ELIZA.Morphology;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax.DeepRelations
{
    public class ServiceConverter: AbstractDeepRelationConverter
    {

        protected override Tree<DForm, DeepRelationName> ConvertInternal(Tree<Lexem,
            SurfaceRelationName> node, Tree<Lexem, SurfaceRelationName> parent)
        {
            var result = new Tree<DForm, DeepRelationName>();
            result.Key = new DForm();
            if ((node.Key.Tag & Tag.Infinitive) != 0)
            {
                result.Key.Lexem = node.Key;
                result.DependencyType = DeepRelationName.Actant1;
                ConvertChildrenAndApplySign(node, result);
                return result;
            }
            return null;
        }
    }
}
