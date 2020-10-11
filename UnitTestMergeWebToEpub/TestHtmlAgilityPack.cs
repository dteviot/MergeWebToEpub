using System;
using System.Xml.Linq;
using MergeWebToEpub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestHtmlAgilityPack
    {
        [TestMethod]
        public void TestHtmlTextToXhtml()
        {
            var actual = HtmlAgilityPackUtils.HtmlTextToXhtml("Weekend at Uncle Discord&#039;s: Day One &amp; Two");
            Assert.AreEqual("Weekend at Uncle Discord's: Day One &amp; Two", actual);
        }

        [TestMethod]
        public void TestValidateXhtmlWitAgilityPack()
        {
            string xml = Utils.ReadStringResource("UnitTestMergeWebToEpub.TestData.PrettyPrint.xhtml");
            var actual = HtmlAgilityPackUtils.ValidateXhtmlWitAgilityPack(xml);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void TestPrettyPrint()
        {
            string xml = Utils.ReadStringResource("UnitTestMergeWebToEpub.TestData.PrettyPrint.xhtml");
            var fromAgility = HtmlAgilityPackUtils.PrettyPrintXhtml(xml);
            XDocument agilityDoc = XDocument.Parse(fromAgility);
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.PrettyPrint.xhtml");
            var delta = XmlCompare.ElementSame(agilityDoc.Root, doc.Root);
            Assert.IsTrue(delta.AreSame);
        }
    }
}
