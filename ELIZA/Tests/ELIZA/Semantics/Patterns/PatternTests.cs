using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ELIZA.Morphology;
using ELIZA.Semantics.Patterns;
using ELIZA.Syntax;
using ELIZA.Syntax.DeepRelations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.ELIZA.Semantics.Patterns
{
    [TestClass]
    public class PatternTests
    {
        static Tree<DForm, DeepRelationName> tree;

        [TestMethod]
        public void TestSingleRelationPattern()
        {
            var pattern = Pattern.Create("(Имя)SENT");
            var savedValue = string.Empty;
            Assert.IsTrue(pattern.Match(tree));
            Assert.IsTrue(pattern.TryGetSavedValue("Имя", out savedValue));
            Assert.IsTrue(savedValue == "переводить");
            Assert.IsFalse(pattern.Match(tree.Children.First()));
        }

        [TestMethod]
        public void TestSingleLemmaPattern()
        {
            var pattern = Pattern.Create("(Имя)переводить");
            var savedValue = string.Empty;
            Assert.IsTrue(pattern.Match(tree));
            Assert.IsTrue(pattern.TryGetSavedValue("Имя", out savedValue));
            Assert.IsTrue(savedValue == "переводить");
            Assert.IsFalse(pattern.Match(tree.Children.First()));
        }

        [TestMethod]
        public void TestRelationAndLemmaPattern()
        {
            var pattern = Pattern.Create("(Имя)SENT^переводить");
            var savedValue = string.Empty;
            Assert.IsTrue(pattern is RelationAndLemmaPattern);
            Assert.IsTrue(pattern.Match(tree));
            Assert.IsTrue(pattern.TryGetSavedValue("Имя", out savedValue));
            Assert.IsTrue(savedValue == "переводить");
            Assert.IsFalse(pattern.Match(tree.Children.First()));
        }

        [TestMethod]
        public void TestBranchedPattern()
        {
            var pattern = Pattern.Create("((а1)A2, (а2)A3)");
            var savedValue = string.Empty;
            Assert.IsTrue(pattern is BranchedPattern);
            Assert.IsTrue(pattern.Match(tree));
            Assert.IsTrue(pattern.TryGetSavedValue("а1", out savedValue));
            Assert.IsTrue(savedValue == "число");
            Assert.IsTrue(pattern.TryGetSavedValue("а2", out savedValue));
            Assert.IsTrue(savedValue == "система");
            Assert.IsFalse(pattern.Match(tree.Children.Last()));
        }

        [TestMethod]
        public void TestChainedPattern()
        {
            /*этот паттерн совпадает с деревом вида
             * переводить 
             *|- (A2) *
             *\- (A3) *
             */
            var pattern = Pattern.Create("переводить - ((а2)A2, (а3)A3)");
            var savedValue = string.Empty;
            Assert.IsTrue(pattern is ChainedPattern);
            Assert.IsTrue(pattern.Match(tree));
            Assert.IsTrue(pattern.TryGetSavedValue("а2", out savedValue));
            Assert.IsTrue(savedValue == "число");
            Assert.IsTrue(pattern.TryGetSavedValue("а3", out savedValue));
            Assert.IsTrue(savedValue == "система");
            Assert.IsFalse(pattern.Match(tree.Children.Last()));
        }
        [TestMethod]
        public void TestSkipablePattern()
        {
            /*этот паттерн совпадает с деревом вида
             * (A2) *
             * \- (PROP) *
             */
            var pattern = Pattern.Create("...(а2)A2 - (в)PROP");
            var savedValue = string.Empty;
            Assert.IsTrue(pattern is SkipablePattern);
            Assert.IsTrue(pattern.Match(tree));
            Assert.IsTrue(pattern.TryGetSavedValue("а2", out savedValue));
            Assert.IsTrue(savedValue == "число");
            Assert.IsTrue(pattern.TryGetSavedValue("в", out savedValue));
            Assert.IsTrue(savedValue == "10");
            Assert.IsFalse(pattern.Match(tree.Children.Last()));
        }
        [TestMethod]
        public void TestOrPattern()
        {
            var pattern = Pattern.Create("((в)PROP |...(а2)A2)");
            Assert.IsTrue(pattern is OrPattern);
            Assert.IsTrue(pattern.Match(tree));
            Assert.IsFalse(pattern.Match(tree.Children.Last()));
        }

        [ClassInitialize]
        public static void Initialize1(TestContext context)
        {
            string sent1 = "Как перевести число 10 в двоичную систему счисления?";
            MorphologyModel morphologyModel;
            using (FileStream fs = File.Open("morphology/morhp.mdl", FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                morphologyModel = (MorphologyModel)bf.Deserialize(fs);
            }
            var syntaxModel = new SyntaxModel();
            var morpAnalyzed = morphologyModel.Predict(sent1);
            var syntaxAnalyzed = syntaxModel.SurfaceAnalysis(morpAnalyzed[0]);
            tree = syntaxModel.DeepAnalysis(syntaxAnalyzed);
        }


    }
}
