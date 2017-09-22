using System.Collections.Generic;
using System.Linq;
using ELIZA.Morphology;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax.DeepRelations
{
    public abstract class AbstractDeepRelationConverter: IDeepRelationBuilder
    {
        public Tree<DForm, DeepRelationName> Convert(Tree<Morphology.Lexem, SurfaceRelationName> node,
            Tree<Morphology.Lexem, SurfaceRelationName> parent)
        {            
            if (parent == null)
            {
                var result = new Tree<DForm, DeepRelationName>();
                result.Key = new DForm(node.Key);
                result.DependencyType = DeepRelationName.Sentence;
                ConvertChildrenAndApplySign(node, result);
                return result;
            }
            else
                return ConvertInternal(node, parent);
        }

        protected IEnumerable<Tree<DForm, DeepRelationName>> ConvertChildren(Tree<Lexem,
            SurfaceRelationName> node)
        {
            return node.Children.Select(child => DependencyGrammar.Convert(child, node));
        }

        protected void ConvertChildrenAndApplySign(Tree<Lexem,
            SurfaceRelationName> node, Tree<DForm, DeepRelationName> converted)
        {
            foreach (var child in ConvertChildren(node))
            {
                if (child != null)
                {
                    if (child.Key.Lexem.Word == "не")
                    {
                        if(converted.Key.Sign != LexicalSign.Negative)
                            converted.Key.Sign = LexicalSign.Negative;
                        else converted.Key.Sign = LexicalSign.NoSign;
                    }
                    else 
                        converted.AddChild(child);
                }
            }
        }
        public Tree<DForm, DeepRelationName> Convert(Tree<Morphology.Lexem,
            SurfaceRelationName> node)
        {
            return Convert(node, null);
        }

        protected abstract Tree<DForm, DeepRelationName> ConvertInternal(
            Tree<Morphology.Lexem, SurfaceRelationName> node,
            Tree<Morphology.Lexem, SurfaceRelationName> parent);
    }
}
