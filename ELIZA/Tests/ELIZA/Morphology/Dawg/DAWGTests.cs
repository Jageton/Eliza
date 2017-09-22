using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ELIZA.Morphology.Dawg;
using ELIZA.Morphology.Dawg.Builders;
using ELIZA.Morphology.Dawg.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BernulliTests
{
    [TestClass]
    public class DawgTests
    {
        private static IDawg<char, bool> instance;

        /*TODO:
         * 1) Полноценные тесты функциональности на случайных данных либо данных из корпуса
         * 2) Соответствующая инициализация
         */

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
