using System;
using System.Collections.Generic;
using ELIZA.Morphology.Dawg.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ELIZA.Morphology.Dawg;
using ELIZA.Morphology.Dawg.Utils;

namespace Tests.ELIZA.Morphology.Dawg.Builders
{
    [TestClass]
    public class OrdreredDacukTests
    {
        private static IDawg<char, bool> instance;

        /// <summary>
        /// Example from Dacuk's article.
        /// This method checks whateaver the resulting dawg really contains 4 values.
        /// </summary>
        [TestMethod]
        public void DacukConsistencyCheck()
        {
            Assert.IsTrue(instance.Count == 4);
        }
        /// <summary>
        /// Example from Dacuk's article. This method checks if there are only 8 states 
        /// comparing them by reference. This means that the resulting dawg is minimized.
        /// </summary>
        [TestMethod]
        public void DacukIntegrityCheck()
        {
            var hashSet = new HashSet<IDawgNode<char, bool>>(new StateReferenceComparer<char, bool>());
            var stack = new Stack<IDawgNode<char, bool>>();
            stack.Push(((Dawg<char, bool>)instance).Root);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (!hashSet.Contains(current))
                    hashSet.Add(current);
                foreach (var child in current.Children)
                    stack.Push(child);
            }
            Assert.IsTrue(hashSet.Count == 8);
        }
        [ClassInitialize]
        public static void GetDacukExampleDawg(TestContext context)
        {
            IIncrementalDawgBuilder<Dawg<char, bool>, char, bool> builder = new OrderedDacukBuilder<char, bool>();
            builder.Append("aient", true);
            builder.Append("ais", false);
            builder.Append("ait", true);
            builder.Append("ant", true);
            instance = builder.Instance;
        }
    }
}
