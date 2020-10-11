using MergeWebToEpub;
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
    static class Utils
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

        public static string ReadStringResource(string resourceName)
        {
            using (var stream = Utils.ReadResource(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static void CompareDocPart(XDocument doc1, XDocument doc2, XNamespace ns, string elementName)
        {
            var e1 = doc1.Root.Element(ns + elementName);
            var e2 = doc2.Root.Element(ns + elementName);
            Assert.IsTrue(XNode.DeepEquals(e1, e2));
        }

        public static Dictionary<string, EpubItem> FakeAbsolutePathIndex(XDocument doc, string ncxFileName)
        {
            var index = new Dictionary<string, EpubItem>();
            string ncxFolder = ncxFileName.GetZipPath();
            foreach (var e in doc.Root.Descendants(Epub.ncxNs + "content"))
            {
                string relativePath = e.Attribute("src").Value;
                string absolute = ZipUtils.RelativePathToAbsolute(ncxFolder, relativePath);
                index[absolute] = new EpubItem() { AbsolutePath = absolute };
            }
            return index;
        }

        public static Dictionary<string, EpubItem> FakeItems(XDocument doc)
        {
            var dic = new Dictionary<string, EpubItem>();
            foreach (var e in doc.Root.Descendants(Epub.DaisyNs + "source"))
            {
                var id = e.Attribute("id").Value.Substring(3);
                dic.Add(id, new EpubItem() { Id = id });
            }
            return dic;
        }

        public static Dictionary<string, string> ExtractSources(this List<EpubItem> items)
        {
            var dic = new Dictionary<string, string>();
            foreach (var item in items.Where(i => !string.IsNullOrEmpty(i.Source)))
            {
                dic.Add(item.MetadataId, item.Source);
            }
            return dic;
        }
    }
}
