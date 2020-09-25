using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public partial class Opf
    {
        public Opf()
        {
        }

        public Opf(XDocument doc, string OpfFileName)
        {
            this.OpfFileName = OpfFileName;
            this.Metadata = new Metadata(doc.Root.Element(Epub.PackageNs + "metadata"));
            var opfFolder = OpfFileName.GetZipPath();
            ParseManifest(doc.Root.Element(Epub.PackageNs + "manifest"), opfFolder);
            foreach (var item in Manifest)
            {
                AbsolutePathIndex.Add(item.AbsolutePath, item);
                IdIndex.Add(item.Id, item);
            }
            ParseSpine(doc.Root.Element(Epub.PackageNs + "spine"));
            FindSpecialPages();
        }

        public void WriteTo(ZipFile zipFile)
        {
            zipFile.AddEntry(OpfFileName, ToXDocument().ToStream());
        }

        public void WriteManifestTo(ZipFile zipFile)
        {
            foreach (var item in Manifest)
            {
                item.WriteTo(zipFile);
            }
        }

        public XDocument ToXDocument()
        {
            return new XDocument(
                new XElement(Epub.PackageNs + "package",
                    new XAttribute("xmlns", "http://www.idpf.org/2007/opf"),
                    new XAttribute("version", "2.0"),
                    new XAttribute("unique-identifier", "BookId"),
                    Metadata.ToXElement(),
                    ManifestToXElement(),
                    SpineToXElement()
                )
            );
        }

        private void ParseManifest(XElement manifestElement, string opfFolder)
        {
            Manifest = manifestElement.Elements(Epub.PackageNs + "item")
                .Select(e => new EpubItem(e, opfFolder)).ToList();
        }

        private void ParseSpine(XElement spineElement)
        {
            Spine = spineElement.Elements(Epub.PackageNs + "itemref")
                .Select(e => IdIndex[e.Attribute("idref").Value]).ToList();
            TocId = spineElement.Attribute("toc")?.Value;
        }

        private XElement ManifestToXElement()
        {
            var root = new XElement(Epub.PackageNs + "manifest");
            var opfFolder = OpfFileName.GetZipPath();
            foreach (var item in Manifest)
            {
                root.Add(item.ToXElement(opfFolder));
            }
            return root;
        }

        private XElement SpineToXElement()
        {
            var root = new XElement(Epub.PackageNs + "spine",
                new XAttribute("toc", TocId)
            );
            foreach (var item in Spine)
            {
                root.Add(new XElement(Epub.PackageNs + "itemref",
                    new XAttribute("idref", item.Id)
                ));
            }
            return root;
        }

        private void FindSpecialPages()
        {
            var coverImageId = Metadata.CoverImageId;
            if (coverImageId != null)
            {
                CoverImage = IdIndex[coverImageId];
            }
        }

        public List<EpubItem> GetPageItems()
        {
            return GetItems(i => i.IsXhtmlPage);
        }

        public List<EpubItem> GetImageItems()
        {
            return GetItems(i => i.IsImage);
        }

        public List<EpubItem> GetItems(Func<EpubItem, bool> filter)
        {
            return Manifest.Where(filter).ToList();
        }

        public void AppendItem(EpubItem item, string source)
        {
            Metadata.AddSource(item.MetadataId, source);
            Manifest.Add(item);
            AbsolutePathIndex.Add(item.AbsolutePath, item);
            IdIndex.Add(item.Id, item);
        }

        public void DeleteItem(EpubItem item)
        {
            Metadata.RemoveSource(item.MetadataId);
            Manifest.Remove(item);
            AbsolutePathIndex.Remove(item.AbsolutePath);
            IdIndex.Remove(item.Id);
            Spine.Remove(item);
        }

        public Metadata Metadata { get; set; }

        public List<EpubItem> Manifest { get; set; }
        public List<EpubItem> Spine { get; set; }

        public string TocId { get; set; }

        private string OpfFileName { get; set; }
        public Dictionary<string, EpubItem> AbsolutePathIndex { get; set; } = new Dictionary<string, EpubItem>();
        public Dictionary<string, EpubItem> IdIndex { get; set; } = new Dictionary<string, EpubItem>();

        public EpubItem CoverImage { get; set; }

        public EpubItem NcxItem
        {
            get { return IdIndex[TocId];  }
        }

        public EpubItem CoverPage
        {
            get {
                EpubItem page = null;
                IdIndex.TryGetValue(Epub.CoverPageId, out page);
                return page; 
            }
        }
    }
}
