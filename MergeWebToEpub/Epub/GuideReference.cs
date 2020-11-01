using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public class GuideReference
    {
        public GuideReference(XElement element, string opfFolder, Dictionary<string, EpubItem> absolutePathIndex)
        {
            string path =  ZipUtils.RelativePathToAbsolute(opfFolder, element.Attribute("href").Value);
            EpubItem temp = null;
            absolutePathIndex.TryGetValue(path, out temp);
            Item = temp;
            Title = element.Attribute("title")?.Value;
            TypeName = element.Attribute("type").Value;
        }

        public XElement ToXElement(string opfFolder)
        {
            string href = Item == null
                ? string.Empty
                : ZipUtils.AbsolutePathToRelative(opfFolder, Item.AbsolutePath);
            var element = new XElement(Epub.PackageNs + "reference",
                new XAttribute("href", href),
                new XAttribute("type", TypeName)
            );
            if (Title != null)
            {
                element.Add(new XAttribute("title", Title));
            }
            return element;
        }

        public EpubItem Item { get; set; }
        public string Title { get; set; }
        public string TypeName { get; set; }
    }
}
