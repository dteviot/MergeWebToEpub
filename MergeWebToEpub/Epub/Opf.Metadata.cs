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

        public Metadata(XElement element, Dictionary<string, EpubItem> idIndex)
        {
            Title = element.Element(Epub.DaisyNs + "title").Value;
            Language = element.Element(Epub.DaisyNs + "language").Value;
            Date = element.Element(Epub.DaisyNs + "date")?.Value;

            var creatorElement = element.Element(Epub.DaisyNs + "creator");
            Creator = creatorElement?.Value;
            CreatorFileAs = creatorElement?.Attribute(Epub.PackageNs + "file-as")?.Value ?? Creator;

            var identifierElement = element.Element(Epub.DaisyNs + "identifier");
            IdentifierId = identifierElement?.Attribute("id")?.Value;
            Identifier = identifierElement?.Value;

            var contributorElement = element.Element(Epub.DaisyNs + "contributor");
            ContributorRole = contributorElement?.Attribute(Epub.PackageNs + "role")?.Value ?? "bkp";
            Contributor = contributorElement?.Value;

            CoverImageId = FindCoverImageId(element);

            foreach (var e in element.Elements(Epub.DaisyNs + "source"))
            {
                var id = e.Attribute("id")?.Value;
                if ((id != null) && id.StartsWith("id."))
                {
                    idIndex[id.Substring(3)].Source = e.Value;
                }
            }
        }

        public XElement ToXElement(List<EpubItem> items)
        {
            var element = new XElement(Epub.PackageNs + "metadata",
                new XAttribute(XNamespace.Xmlns + "dc", Epub.DaisyNs.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "opf", Epub.PackageNs.NamespaceName),
                new XElement(Epub.DaisyNs + "title", Title),
                new XElement(Epub.DaisyNs + "language", Language),
                new XElement(Epub.DaisyNs + "date", Date),
                new XElement(Epub.DaisyNs + "creator",
                    new XAttribute(Epub.PackageNs + "file-as", CreatorFileAs),
                    new XAttribute(Epub.PackageNs + "role", "aut"),
                    Creator
                ),
                new XElement(Epub.DaisyNs + "identifier",
                    new XAttribute("id", IdentifierId),
                    new XAttribute(Epub.PackageNs + "scheme", "URI"),
                    Identifier
                ),
                new XElement(Epub.DaisyNs + "contributor",
                    new XAttribute(Epub.PackageNs + "role", ContributorRole),
                    Contributor
                )
            );

            if (CoverImageId != null)
            {
                element.Add(new XElement(Epub.PackageNs + "meta",
                    new XAttribute("content", CoverImageId),
                    new XAttribute("name", "cover")
                ));
            }

            foreach (var xml in items.Select(i => i.SourceAsXml()).Where(i => i != null))
            {
                element.Add(xml);
            }
            return element;
        }

        public string FindCoverImageId(XElement metadata)
        {
            return metadata
                .Elements(Epub.PackageNs + "meta")
                .Where(e => e.Attribute("name")?.Value == "cover")
                .Select(e => e.Attribute("content")?.Value)
                .FirstOrDefault();
        }

        public string Title { get; set; }
        public string Language { get; set; }
        public string Date { get; set; }

        public string CreatorFileAs { get; set; }
        public string Creator { get; set; }
        public string IdentifierId { get; set; }
        public string Identifier { get; set; }
        public string ContributorRole { get; set; }
        public string Contributor { get; set; }

        public string CoverImageId { get; set; }
    }
}
