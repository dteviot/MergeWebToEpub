using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    [DebuggerDisplay("{Title}: {ContentSrc}")]
    public class TocEntry
    {
        public TocEntry()
        {

        }

        public TocEntry(XElement element, string ncxFolder, Dictionary<string, EpubItem> absolutePathIndex)
        {
            System.Diagnostics.Trace.WriteLine(element.ToString());
            Title = element.Element(Epub.ncxNs + "navLabel").Element(Epub.ncxNs + "text").Value;
            string src = element.Element(Epub.ncxNs + "content").Attribute("src").Value;

            int index = src.IndexOf("#");
            if (0 < index)
            {
                Anchor = src.Substring(index);
                src = src.Substring(0, index);
            }

            Item = absolutePathIndex[ZipUtils.RelativePathToAbsolute(ncxFolder, src)];
            Children = element.Elements(Epub.ncxNs + "navPoint")
                .Select(e => new TocEntry(e, ncxFolder, absolutePathIndex))
                .ToList();
        }

        public string Title { get; set; }

        public string ContentSrc { get { return Item.AbsolutePath; } }

        public EpubItem Item { get; set; }

        public string Anchor { get; set; } = string.Empty;

        public List<TocEntry> Children { get; set; } = new List<TocEntry>();

        public XElement ToNavPoint(ref int playOrder, string ncxFolder)
        {
            ++playOrder;
            var navPoint = new XElement(Epub.ncxNs + "navPoint",
                new XAttribute("id", "body" + playOrder.ToString("D4")),
                new XAttribute("playOrder", playOrder),
                new XElement(Epub.ncxNs + "navLabel",
                    new XElement(Epub.ncxNs + "text", Title)
                ),
                new XElement(Epub.ncxNs + "content",
                    new XAttribute("src", ZipUtils.AbsolutePathToRelative(ncxFolder, ContentSrc) + Anchor)
                )
            );
            foreach(var e in Children)
            {
                navPoint.Add(e.ToNavPoint(ref playOrder, ncxFolder));
            }
            return navPoint;
        }

        public int CalcNavMapDepth()
        {
            return 1 + CalcNavMapDepth(Children);
        }

        public static int CalcNavMapDepth(List<TocEntry> entries)
        {
            int depth = 0;
            foreach(var child in entries)
            {
                depth = Math.Max(depth, child.CalcNavMapDepth());
            }
            return depth;
        }

        public static void AddToScrToTitleMap(Dictionary<string, string> map, List<TocEntry> entries)
        {
            foreach(var entry in entries)
            {
                entry.AddToScrToTitleMap(map);
            }
        }

        public void AddToScrToTitleMap(Dictionary<string, string> map)
        {
            string title = null;
            if (map.TryGetValue(ContentSrc, out title))
            {
                map[ContentSrc] = title + ": " + Title;
            }
            else
            {
                map.Add(ContentSrc, Title);
            }
            AddToScrToTitleMap(map, Children);
        }

        public static (List<TocEntry> entries, int index) FindTocEntry(List<TocEntry> entries, string src)
        {
            for(int i = 0; i < entries.Count; ++i)
            {
                var entry = entries[i];
                if (entry.ContentSrc == src)
                {
                    return (entries, i);
                }
                var possible = FindTocEntry(entry.Children, src);
                if (possible.entries != null)
                {
                    return possible;
                }
            }
            return (null, 0);
        }
    }
}
