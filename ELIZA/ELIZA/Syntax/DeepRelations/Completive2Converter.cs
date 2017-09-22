using System.Linq;
using ELIZA.Morphology;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax.DeepRelations
{
    public class Completive2Converter: AbstractDeepRelationConverter
    {

        protected override Tree<DForm, DeepRelationName> ConvertInternal(Tree<Lexem, SurfaceRelationName> node,
            Tree<Lexem, SurfaceRelationName> parent)
        {
            var result = new Tree<DForm, DeepRelationName>();
            result.Key = new DForm();            
            //непрямое дополнение начинается с предлога
            if ((node.Key.Tag & Tag.Preposition) != 0)
                result = DependencyGrammar.Convert(node.Children.First(), parent);
            else
            {
                result.Key.Lexem = node.Key;
                ConvertChildrenAndApplySign(node, result);
            }
            result.DependencyType = DeepRelationName.Actant3;
            return result;
        }
    }
}
