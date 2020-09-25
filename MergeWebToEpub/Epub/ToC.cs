using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    // The table of contents
    public class ToC
    {
        public ToC()
        {
        }

        public ToC(XDocument doc, EpubItem ncxItem, Dictionary<string, EpubItem> absolutePathIndex)
        {
            NcxItem = ncxItem;
            Version = doc.Root.Attribute("version").Value;
            Language = doc.Root.Attribute(Epub.xmlNs + "lang").Value;

            Uid = doc.Root.Element(Epub.ncxNs + "head")
                .Elements(Epub.ncxNs + "meta")
                .Where(e => e.Attribute("name").Value == "dtb:uid")
                .First()
                .Attribute("content").Value;

            DocTitle = doc.Root.Element(Epub.ncxNs + "docTitle")
                .Element(Epub.ncxNs + "text")
                .Value;

            var map = doc.Root.Element(Epub.ncxNs + "navMap");
            string ncxPath = NcxFileName.GetZipPath();
            Entries = map.Elements(Epub.ncxNs + "navPoint")
                .Select(e => new TocEntry(e, ncxPath, absolutePathIndex))
                .ToList();
        }

        public void WriteToNcxEpubItem()
        {
            // Pack this into the ToC EpuhItem
            // so will be packed into the zip file
            NcxItem.RawBytes = ToXDocument().ToStream().ToArray();
        }

        public XDocument ToXDocument()
        {
            return new XDocument(
                new XElement(Epub.ncxNs + "ncx",
                    new XAttribute("xmlns", "http://www.daisy.org/z3986/2005/ncx/"),
                    new XAttribute("version", Version),
                    new XAttribute(Epub.xmlNs + "lang", Language),
                    BuildHeadElement(),
                    BuildDocTitleElment(),
                    BuildNavMap()
                )
            );
        }

        public XElement BuildHeadElement()
        {
            return new XElement(Epub.ncxNs + "head",
                BuildContentlement("dtb:uid", Uid),
                BuildContentlement("dtb:depth", CalcNavMapDepth().ToString()),
                BuildContentlement("dtb:totalPageCount", "0"),
                BuildContentlement("dtb:maxPageNumber", "0")
            );
        }

        public XElement BuildContentlement(string name, string content)
        {
            return new XElement(Epub.ncxNs + "meta",
                new XAttribute("name", name),
                new XAttribute("content", content)
            );
        }

        public XElement BuildDocTitleElment()
        {
            return new XElement(Epub.ncxNs + "docTitle",
                new XElement(Epub.ncxNs + "text", DocTitle)
            );
        }

        public int CalcNavMapDepth()
        {
            return TocEntry.CalcNavMapDepth(Entries);
        }

        public XElement BuildNavMap()
        {
            var navMap = new XElement(Epub.ncxNs + "navMap");
            int i = 0;
            string ncxPath = NcxFileName.GetZipPath();
            foreach(var e in Entries)
            {
                navMap.Add(e.ToNavPoint(ref i, ncxPath));
            }
            return navMap;
        }

        public Dictionary<string, string> BuildScrToTitleMap()
        {
            var map = new Dictionary<string, string>();
            TocEntry.AddToScrToTileMap(map, Entries);
            return map;
        }

        public void DeleteItem(EpubItem item)
        {
            // ToDo, better handling of case when item appears in ToC
            // in multple places.  (Due to nested chapters)
            var entryDetails = FindTocEntry(item.AbsolutePath);
            while (entryDetails.entries != null)
            {
                entryDetails.entries.RemoveAt(entryDetails.index);
                entryDetails = FindTocEntry(item.AbsolutePath);
            }
        }

        public (List<TocEntry> entries, int index) FindTocEntry(string src)
        {
            return TocEntry.FindTocEntry(Entries, src);
        }

        public string Uid { get; set; }

        public string Version { get; set; }

        public string Language { get; set; }

        public string DocTitle { get; set; }

        public List<TocEntry> Entries { get; set; } = new List<TocEntry>();
        public string NcxFileName { get { return NcxItem.AbsolutePath; } }

        public EpubItem NcxItem;
    }
}
