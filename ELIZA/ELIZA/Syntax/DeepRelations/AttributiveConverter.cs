using System.Linq;
using ELIZA.Morphology;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax.DeepRelations
{
    public class AttributiveConverter: AbstractDeepRelationConverter
    {
        protected override Tree<DForm, DeepRelationName> ConvertInternal(Tree<Lexem, SurfaceRelationName> node,
            Tree<Lexem, SurfaceRelationName> parent)
        {
            //конвертер для согласованных определений
            var result = new Tree<DForm, DeepRelationName>();
            result.Key = new DForm();
            result.Key.Lexem = node.Key;
            //качественное прилагательное / наречие
            if ((node.Key.Tag & Tag.Qualitative) != 0)
            {
                //у качественного определения нет аттрибутов
                result.DependencyType = DeepRelationName.Attribute;
            }
            else
            {
                result.DependencyType = DeepRelationName.Property;
                if (node.Children.Any()) //есть зависимые элементы
                {
                    //он должен быть один
                    //TODO: предусмотреть вариант с союзами
                    result.AddChild(DependencyGrammar.Convert(node.Children.First(), node));
                    result.Children.First().DependencyType = DeepRelationName.Magnitude;
                }
                else
                {
                    //TODO: добавть константу в грамматику DefaultMagnitude
                }
            }
            return result;
        }
    }
}
