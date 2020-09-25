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
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.toc.ncx");
            var e1 = doc.Root.Element(Epub.ncxNs + "navMap")
                .Element(Epub.ncxNs + "navPoint");

            string ncxPath = "OEPBS/content.opf";
            string ncxFolder = ncxPath.GetZipPath();
            var api = Utils.FakeAbsolutePathIndex(doc, ncxPath);
            var tocEntry = new TocEntry(e1, ncxFolder, api);
            int playOrder = 0;
            var e2 = tocEntry.ToNavPoint(ref playOrder, ncxFolder);
            Assert.IsTrue(XNode.DeepEquals(e1, e2));
        }
    }
}
