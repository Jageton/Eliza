using System.Collections.Generic;
using System.Linq;
using Diggins.Jigsaw;
using ELIZA.Syntax.DeepRelations;

namespace ELIZA.Semantics.Patterns.Builders
{
    public class RelationAndLemmaPatternBuilder: IPatternBuilder
    {
        private static Dictionary<string, DeepRelationName> relations;

        public static Dictionary<string, DeepRelationName> Relations
        {
            get { return relations; }
        }

        static RelationAndLemmaPatternBuilder()
        {
            relations = new Dictionary<string, DeepRelationName>();
            relations.Add("SENT", DeepRelationName.Sentence);
            relations.Add("COOR", DeepRelationName.Coordination);
            relations.Add("REL", DeepRelationName.Relative);
            relations.Add("A1", DeepRelationName.Actant1);
            relations.Add("A2", DeepRelationName.Actant2);
            relations.Add("A3", DeepRelationName.Actant3);
            relations.Add("AGGR", DeepRelationName.Arrgregation);
            relations.Add("PROP", DeepRelationName.Property);
            relations.Add("ATTR", DeepRelationName.Attribute);
            relations.Add("MAGN", DeepRelationName.Magnitude);
        }

        

        public Pattern Build(Node node)
        {
            var nameNode = node.Nodes.FirstOrDefault((n) => n.Label == PatternGrammar.Name.Name);
            var name = string.Empty;
            var save = false;
            if (nameNode != null)
            {
                save = true;
                name = nameNode.Text;
            }
            var lemma = node.Descendants.First((n) =>
                n.Label == PatternGrammar.MatchLemma.Name).Descendants.First().Text;
            var relation = relations[node.Descendants.First((n) =>
                n.Label == PatternGrammar.MatchDep.Name).Text];
            return new RelationAndLemmaPattern(name, lemma, relation, save);
        }
    }
}
