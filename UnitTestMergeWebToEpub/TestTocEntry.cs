using System;
using System.Xml.Linq;
using MergeWebToEpub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestTocEntry
    {
        [TestMethod]
        public void RoundTripTocEntry()
        {
            XDocument doc = null;
            using (var stream = Utils.ReadResource("UnitTestMergeWebToEpub.TestData.toc.ncx"))
            {
                doc = XDocument.Load(stream);
            }
            var e1 = doc.Root.Element(Epub.ncxNs + "navMap")
                .Element(Epub.ncxNs + "navPoint");

            string ncxPath = "OEPBS/content.opf";
            var tocEntry = new TocEntry(e1, ncxPath);
            int playOrder = 0;
            var e2 = tocEntry.ToNavPoint(ref playOrder, ncxPath);
            Assert.IsTrue(XNode.DeepEquals(e1, e2));
        }
    }
}
