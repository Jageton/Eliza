using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticeLib.Dawg;
using PracticeLib.Dawg.Utils;

namespace BernulliTests
{
    [TestClass]
    public class DawgTests
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
                foreach(var child in current.Children)
                    stack.Push(child);
            }
            Assert.IsTrue(hashSet.Count == 8);
        }
        [TestMethod]
        public void DawgFunctionalityCheck()
        {
            #region ContainsKey

            Assert.IsTrue(instance.ContainsKey("aient"));
            Assert.IsTrue(instance.ContainsKey("ais"));
            Assert.IsTrue(instance.ContainsKey("ait"));
            Assert.IsTrue(instance.ContainsKey("ant"));
            Assert.IsFalse(instance.ContainsKey("aint"));

            #endregion

            #region TryGetValue

            var value = false;
            Assert.IsTrue(instance.TryGetValue("aient", out value));
            Assert.IsTrue(instance.TryGetValue("ais", out value));
            Assert.IsTrue(instance.TryGetValue("ait", out value));
            Assert.IsTrue(instance.TryGetValue("ant", out value));
            Assert.IsFalse(instance.TryGetValue("aint", out value));

            #endregion

            #region indexer

            Assert.IsTrue(instance["aient"]);
            Assert.IsTrue(instance["ais"] == false);
            Assert.IsTrue(instance["ait"]);
            Assert.IsTrue(instance["ant"]);
            try
            {
                value = instance["aint"];
                Assert.Fail("Should throw KeyNotFoundException.");
            }
            catch (KeyNotFoundException) { }

            #endregion
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
