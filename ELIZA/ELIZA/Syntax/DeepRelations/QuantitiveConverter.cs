using ELIZA.Morphology;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax.DeepRelations
{
    public class QuantitiveConverter : AbstractDeepRelationConverter
    {
        protected override Tree<DForm, DeepRelationName> ConvertInternal(Tree<Lexem, SurfaceRelationName> node,
            Tree<Lexem, SurfaceRelationName> parent)
        {
            var result = new Tree<DForm, DeepRelationName>();
            result.Key = new DForm();
            result.Key.Lexem = node.Key;
            result.DependencyType = DeepRelationName.Property;
            //потомков нет
            return result;
        }
    }
}
