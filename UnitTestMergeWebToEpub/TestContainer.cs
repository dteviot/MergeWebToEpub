using System;
using System.Xml.Linq;
using MergeWebToEpub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestContainer
    {
        [TestMethod]
        public void RoundTripContainer()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.container.xml");
            var container = new Container(doc);
            Assert.AreEqual("OEBPS/content.opf", container.FullPath);
            var doc2 = container.ToXDocument();
            var delta = XmlCompare.ElementSame(doc.Root, doc2.Root);
            Assert.IsTrue(delta.AreSame);
        }
    }
}
