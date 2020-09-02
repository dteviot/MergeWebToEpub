using System;
using System.Xml.Linq;
using MergeWebToEpub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestOpfMetadata
    {
        [TestMethod]
        public void RoundTripMetadata_WithCover()
        {
            RoundTripMetadata("UnitTestMergeWebToEpub.TestData.contentWithImages.opf");
        }

        [TestMethod]
        public void RoundTripMetadata_WithoutCover()
        {
            RoundTripMetadata("UnitTestMergeWebToEpub.TestData.content.opf");
        }

        public void RoundTripMetadata(string resourceName)
        {
            XDocument doc = Utils.ReadXmlResource(resourceName);
            var element = doc.Root.Element(Epub.PackageNs + "metadata");
            var metadata = new Metadata(element);

            var element2 = metadata.ToXElement();
            var delta = XmlCompare.ElementSame(element, element2);
            Assert.IsTrue(delta.AreSame);
        }
    }
}
