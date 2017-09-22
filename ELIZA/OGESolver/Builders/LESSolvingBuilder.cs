using System;
using System.Collections.Generic;
using PrLanguages.Expressions;

namespace OGESolver.Builders
{
    public class LESSolvingBuilder : ICommandBuilder<List<Dictionary<string, bool>>>
    {
        public IAlgorithm<List<Dictionary<string, bool>>> Build(object[] args)
        {
            var epxression = (string)args[0];
            var splitted = epxression.Split(new char[] {'='});
            var eh = new ExpressionHelper();
            var left = eh.CreateExpression(splitted[0]);
            var right = (splitted.Length > 1 && (splitted[1] == "1"));
            return new SmartLESSolver(left, right);
        }

        public IAlgorithm<List<Dictionary<string, bool>>> Build(string formattedString)
        {
            throw new NotImplementedException();
        }
    }
}
