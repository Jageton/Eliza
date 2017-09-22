using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticeLib.Dawg.Utils;

namespace BernulliTests
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
                Assert.IsTrue(mapper.Map(i) == i);
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
