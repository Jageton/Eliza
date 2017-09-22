using System;
using ELIZA.Morphology.Dawg.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.ELIZA.Morphology.Dawg.Utils
{
    [TestClass]
    public class MapperTests
    {
        [TestMethod]
        public void TestByteMapping()
        {
            var mapper = new IndexMapper<int, byte>();
            for (byte i = 0; i < byte.MaxValue; i++)
            {
                Assert.IsTrue(mapper.Map((int)i) == i);
            }
            Assert.IsTrue(mapper.Map(10) == 10);
            try
            {
                mapper.Map(256);
                Assert.Fail();
            }
            catch (IndexOutOfRangeException)
            {
                
            }
        }
    }
}
