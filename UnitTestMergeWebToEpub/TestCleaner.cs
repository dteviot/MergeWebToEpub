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

        [TestMethod]
        public void TestChrysanthemumGardenDecryptText()
        {
            var cypherText = "——Pc gfagbrqfma, Bf Wec kbeiv tjnf qgfofggfv ab tjnf gfwjlcfv lvif. Snfc lo tf vfnfibqfv tfwbggtblvr ogbw rlaalcu rb ibcu, tf ralii kbeivc’a tjnf mtbrfc ab ajxf j rafq lcab atja ujiifgs.";
            var clearText = ChrysanthemumGardenCleaner.DecryptText(cypherText);
            Assert.AreEqual("——In retrospect, Ke Xun would have preferred to have remained idle. Even if he developed hemorrhoids from sitting so long, he still wouldn’t have chosen to take a step into that gallery.", clearText);
        }
    }
}
