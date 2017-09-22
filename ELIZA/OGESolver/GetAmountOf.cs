using System.Text.RegularExpressions;

namespace OGESolver
{
    public class GetAmountOf : AbstractAlgorithm<ReferenceOf<int>>
    {
        protected string line;
        protected string pattern;
        
        public GetAmountOf(string line, string pattern)
        {
            this.name = "GET_AMOUNT";
            this.line = line;
            this.pattern = pattern;
        }

        public override ReferenceOf<int> Execute()
        {
            Regex r = new Regex(pattern);
            return r.Matches(line).Count;
        }
    }
}
