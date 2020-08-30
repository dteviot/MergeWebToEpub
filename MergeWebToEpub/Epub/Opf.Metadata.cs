using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public partial class Metadata
    {
        public Metadata()
        {
        }

        public Metadata(XElement element)
        {
            MetadataElement = element;
            foreach(var e in element.Elements(Epub.DaisyNs + "source"))
            {
                Sources.Add(e.Attribute("id").Value, e.Value);
            }
        }

        public XElement ToXElement()
        {
            // ToDo: should generate this from Metadata info
            return MetadataElement;
        }

        public void AddSource(string id, string source)
        {
            Sources.Add(id, source);
            MetadataElement.Add(
                new XElement(Epub.DaisyNs + "source",
                    new XAttribute("id", id),
                    source
                )
            );
        }

        public string CoverImageId()
        {
            return MetadataElement
                .Elements(Epub.PackageNs + "meta")
                .Where(e => e.Attribute("name")?.Value == "cover")
                .Select(e => e.Attribute("content")?.Value)
                .FirstOrDefault();
        }

        public Dictionary<string, string> Sources { get; set; } = new Dictionary<string, string>();

        private XElement MetadataElement { get; set; }
    }
}
