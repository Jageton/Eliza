using System.Linq;
using ELIZA.Morphology;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax.DeepRelations
{
    public class PredicativeConverter: AbstractDeepRelationConverter
    {
        protected override Tree<DForm, DeepRelationName> ConvertInternal(Tree<Lexem,
            SurfaceRelationName> node, Tree<Lexem, SurfaceRelationName> parent)
        {
            var result = new Tree<DForm, DeepRelationName>();
            result.Key = new DForm();
            if ((parent.Key.Tag & Tag.Phantom) == 0)
            {
                if ((node.Key.Tag & Tag.Verb) == 0)
                {
                    result.Key.Lexem = node.Key;
                    //аргумент такой связи - субъект действия
                    result.DependencyType = DeepRelationName.Actant1;
                }
                else
                {
                    //зависимое слово - глагол (курить - здоровью вредить)
                    //что делать в таком случае, непонятно
                }
            }
            else //предикативная конструкция 
            {
                //листом предикативной зависимости будет существительное 
                //или другое слово в роли существительного
                //например в предложении "Полный перебор."
                if (node.DependencyType == SurfaceRelationName.Predicative)
                {
                    result.Key.Lexem = node.Key;
                    result.AddChild(DependencyGrammar.Convert(parent.Children.First((c) =>
                        c.DependencyType == SurfaceRelationName.Completive1), parent),
                        DeepRelationName.Attribute);
                }
                else if (node.DependencyType == SurfaceRelationName.Completive1)
                {
                    result.Key.Lexem = node.Key;
                    //качественное прилагательное
                    if ((node.Key.Tag & Tag.Qualitative) != 0)
                    {
                        result.DependencyType = DeepRelationName.Property;
                    }
                    else 
                        result.DependencyType = DeepRelationName.Attribute;
                    
                }
            }            
            ConvertChildrenAndApplySign(node, result);
            return result;
        }
    }
}
