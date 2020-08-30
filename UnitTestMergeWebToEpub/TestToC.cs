using System;
using System.Xml.Linq;
using MergeWebToEpub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestToC
    {
        [TestMethod]
        public void TestRoundTripParseToC()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.toc.ncx");
            var mockNcxItem = new EpubItem() { AbsolutePath = "toc.ncx" };

            var toc = new ToC(doc, mockNcxItem);
            var doc2 = toc.ToXDocument();
            var delta = XmlCompare.ElementSame(doc.Root, doc2.Root);
            Assert.IsTrue(delta.AreSame);
        }

        [TestMethod]
        public void TestCalcNavMapDepth_Expect2_ThenExpect3()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.toc.ncx");
            var mockNcxItem = new EpubItem() { AbsolutePath = "toc.ncx" };

            var toc = new ToC(doc, mockNcxItem);
            Assert.AreEqual(2, toc.CalcNavMapDepth());

            toc.Entries[2].Children[0].Children.Add(new TocEntry());
            Assert.AreEqual(3, toc.CalcNavMapDepth());
        }
    }
}
