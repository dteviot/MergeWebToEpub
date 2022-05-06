using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using static MergeWebToEpub.CleanerUtils;

namespace MergeWebToEpub
{
    public class WuxiapubCleaner : CleanerBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            if (!string.Equals(HostName(item), "www.wuxiapub.com"))
            {
                return false;
            }

            bool fixedTitle = FixTitle(doc, item);

            var toDelete = FindElementsToDelete(doc, item);
            toDelete.RemoveElements();
            return fixedTitle || (0 < toDelete.Count);
        }

        private static bool FixTitle(XDocument doc, EpubItem item)
        {
            var header = doc.Root.Descendants(Epub.xhtmlNs + "h1").FirstOrDefault();
            if (header == null)
            {
                return false;
            }
            var text = header.Value;
            var clean = HttpUtility.HtmlDecode(text);
            bool changed = clean != text;
            if (changed)
            {
                header.Value = clean;
                System.Diagnostics.Trace.WriteLine($"Changing: {item.Source}");
                System.Diagnostics.Trace.WriteLine($" ==>  {text}");
            }
            return changed;
        }

        private static List<XText> FindElementsToDelete(XDocument doc, EpubItem item)
        {
            bool ShouldRemoveTextNode(XText text)
            {
                string rawText = StripWhiteSpace(text.Value);
                return
                    rawText.Equals("Advertisements") ||
                    rawText.Contains(WuxiaString) ||
                    rawText.Contains(WebnovelString);
            }

            var toDelete = doc.GetTextNodes()
                .Where(ShouldRemoveTextNode)
                .ToList();

            if (0 < toDelete.Count)
            {
                System.Diagnostics.Trace.WriteLine($"Changing: {item.Source}");
                foreach (var e in toDelete)
                {
                    System.Diagnostics.Trace.WriteLine($" ==>  {e.Value}");
                }
            }
            return toDelete;
        }

        private static string StripWhiteSpace(string toStrip)
        {
            var sb = new StringBuilder();
            foreach(var c in toStrip)
            {
                if (!char.IsWhiteSpace(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static readonly string WuxiaString = StripWhiteSpace("Read latest Chapters at WuxiaWorld.S");
        private static readonly string WebnovelString = StripWhiteSpace("for faster releases read on webnovel");
    }
}
