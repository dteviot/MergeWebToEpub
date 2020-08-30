using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UnitTestMergeWebToEpub
{
    class Utils
    {
        public static Stream ReadResource(string resourceName)
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        }

        public static XDocument ReadXmlResource(string resourceName)
        {
            using (var stream = Utils.ReadResource(resourceName))
            {
                return XDocument.Load(stream);
            }
        }

        public static void CompareDocPart(XDocument doc1, XDocument doc2, XNamespace ns, string elementName)
        {
            var e1 = doc1.Root.Element(ns + elementName);
            var e2 = doc2.Root.Element(ns + elementName);
            Assert.IsTrue(XNode.DeepEquals(e1, e2));
        }

    }
}
