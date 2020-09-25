using System;
using System.Collections.Generic;
using System.Net.Mime;
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
            var api = Utils.FakeAbsolutePathIndex(doc, "toc.ncx");

            var toc = new ToC(doc, mockNcxItem, api);
            var doc2 = toc.ToXDocument();
            var delta = XmlCompare.ElementSame(doc.Root, doc2.Root);
            Assert.IsTrue(delta.AreSame);
        }

        [TestMethod]
        public void TestCalcNavMapDepth_Expect2_ThenExpect3()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.toc.ncx");
            var mockNcxItem = new EpubItem() { AbsolutePath = "toc.ncx" };
            var api = Utils.FakeAbsolutePathIndex(doc, "toc.ncx");

            var toc = new ToC(doc, mockNcxItem, api);
            Assert.AreEqual(2, toc.CalcNavMapDepth());

            toc.Entries[2].Children[0].Children.Add(new TocEntry());
            Assert.AreEqual(3, toc.CalcNavMapDepth());
        }

        [TestMethod]
        public void TestBuildScrToTitleMap()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.tocGifting.ncx");
            var mockNcxItem = new EpubItem() { AbsolutePath = "OEPBS/toc.ncx" };
            var api = Utils.FakeAbsolutePathIndex(doc, "OEPBS/toc.ncx");
            var toc = new ToC(doc, mockNcxItem, api);
            var actual = toc.BuildScrToTitleMap();
            Assert.AreEqual(6, actual.Count);
            Assert.AreEqual("Splash pages", actual["OEPBS/Text/0000_Splash_pages.xhtml"]);
            Assert.AreEqual("Main Story: Chapter 1", actual["OEPBS/Text/0001_Chapter_1.xhtml"]);
            Assert.AreEqual("Chapter 3", actual["OEPBS/Text/0003_Chapter_3.xhtml"]);
        }

        [TestMethod]
        public void TestFindTocEntry_Exists()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.tocGifting.ncx");
            var mockNcxItem = new EpubItem() { AbsolutePath = "OEPBS/toc.ncx" };
            var api = Utils.FakeAbsolutePathIndex(doc, "OEPBS/toc.ncx");
            var toc = new ToC(doc, mockNcxItem, api);
            var actual = toc.FindTocEntry("OEPBS/Text/0002_Chapter_2.xhtml");
            Assert.IsNotNull(actual.entries);
        }

        [TestMethod]
        public void TestFindTocEntry_DoesNotExist()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.tocGifting.ncx");
            var mockNcxItem = new EpubItem() { AbsolutePath = "OEPBS/toc.ncx" };
            var api = Utils.FakeAbsolutePathIndex(doc, "OEPBS/toc.ncx");
            var toc = new ToC(doc, mockNcxItem, api);
            var actual = toc.FindTocEntry("OEPBS/Text/0002_Chapter_A.xhtml");
            Assert.IsNull(actual.entries);
        }

        [TestMethod]
        public void TestDeleteItem_SimpleCase()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.tocGifting.ncx");
            var mockNcxItem = new EpubItem() { AbsolutePath = "OEPBS/toc.ncx" };
            var api = Utils.FakeAbsolutePathIndex(doc, "OEPBS/toc.ncx");
            var toc = new ToC(doc, mockNcxItem, api);
            var actual = toc.FindTocEntry("OEPBS/Text/0002_Chapter_2.xhtml");
            Assert.IsNotNull(actual.entries);

            toc.DeleteItem(new EpubItem() { AbsolutePath = "OEPBS/Text/0002_Chapter_2.xhtml" });
            actual = toc.FindTocEntry("OEPBS/Text/0002_Chapter_2.xhtml");
            Assert.IsNull(actual.entries);
        }
    }
}
