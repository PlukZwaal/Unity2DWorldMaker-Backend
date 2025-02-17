using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void Student_WrittenFirstUnitTest_Succesfully()
        {
            Assert.AreEqual(5, 2 + 3);
        }
    }
}