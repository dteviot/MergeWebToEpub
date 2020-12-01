using System;
using System.Xml.Linq;
using MergeWebToEpub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestCleaner
    {
        [TestMethod]
        public void TestRemoveScript()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.0000_Chapter_1_I_Became_Like_This_After_Reincarnating.xhtml");
            var cleaner = new UniversalCleaner();
            bool actual = cleaner.Clean(doc, null);
            Assert.IsTrue(actual);
        }
    }
}
