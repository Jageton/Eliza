using ELIZA.Morphology;
using ELIZA.Syntax.SurfaceRelations;

namespace ELIZA.Syntax.DeepRelations
{
    public class PrepositionalConverter: AbstractDeepRelationConverter
    {

        protected override Tree<DForm, DeepRelationName> ConvertInternal(Tree<Lexem, SurfaceRelationName> node,
            Tree<Lexem, SurfaceRelationName> parent)
        {
            //после предлога идёт группа существительного
            //предлог опускается другими обработчиками
            //этот обработчик просто возвращает группу существительного
            //тип связи будет установлен вызывающим обработчиком
            var result = new Tree<DForm, DeepRelationName>();
            result.Key = new DForm();
            result.Key.Lexem = node.Key;
            ConvertChildrenAndApplySign(node, result);
            return result;
        }
    }
}
