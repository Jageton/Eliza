using ELIZA.Morphology;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax.DeepRelations
{
    public class Completive1Converter: AbstractDeepRelationConverter
    {

        protected override Tree<DForm, DeepRelationName> ConvertInternal(Tree<Lexem, SurfaceRelationName> node,
            Tree<Lexem, SurfaceRelationName> parent)
        {
            var result = new Tree<DForm, DeepRelationName>();
            result.Key = new DForm();
            result.Key.Lexem = node.Key;
            if ((parent.Key.Tag & (Tag.Verb | Tag.Infinitive | Tag.Gerund | Tag.Participle)) != 0)
            {
                result.DependencyType = DeepRelationName.Actant2;
            }
            else
            {
                result.DependencyType = DeepRelationName.Property;
            }
            ConvertChildrenAndApplySign(node, result);
            return result;
        }
    }
}
