using System;
using System.Xml.Linq;
using MergeWebToEpub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestOpf
    {
        [TestMethod]
        public void TestRoundTripParseOpf()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.content.opf");
            var opf = new Opf(doc, "OEPBS/content.opf");
            var doc2 = opf.ToXDocument();

            var delta = XmlCompare.ElementSame(doc.Root, doc2.Root);
            Assert.IsTrue(delta.AreSame);
        }

        [TestMethod]
        public void TestFindSpecialPages_Present()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.contentWithImages.opf");
            var opf = new Opf(doc, "OEPBS/content.opf");
            var doc2 = opf.ToXDocument();

            Assert.AreEqual("OEPBS/Images/0000_p1alt2en.png", opf.CoverImage.AbsolutePath);
            Assert.AreEqual("OEPBS/Text/Cover.xhtml", opf.CoverPage.AbsolutePath);
        }

        [TestMethod]
        public void TestFindSpecialPages_NotPresent()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.content.opf");
            var opf = new Opf(doc, "OEPBS/content.opf");
            var doc2 = opf.ToXDocument();

            Assert.IsNull(opf.CoverImage);
            Assert.IsNull(opf.CoverPage);
        }

        [TestMethod]
        public void TestGetPageItems()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.content.opf");
            var opf = new Opf(doc, "OEPBS/content.opf");
            var doc2 = opf.ToXDocument();

            var items = opf.GetPageItems();
            Assert.AreEqual(14, items.Count);
            Assert.AreEqual("OEPBS/Text/0000_Information.xhtml", items[0].AbsolutePath);
        }

        [TestMethod]
        public void TestGetImageItems_None()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.content.opf");
            var opf = new Opf(doc, "OEPBS/content.opf");
            var doc2 = opf.ToXDocument();

            var items = opf.GetImageItems();
            Assert.AreEqual(0, items.Count);
        }

        [TestMethod]
        public void TestGetImageItems_HasImages()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.contentWithImages.opf");
            var opf = new Opf(doc, "OEPBS/content.opf");
            var doc2 = opf.ToXDocument();

            var items = opf.GetImageItems();
            Assert.AreEqual(20, items.Count);
        }
    }
}
