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
                string rawText = text.Value.StripWhiteSpace();
                return
                    rawText.Equals("Advertisements") ||
                    rawText.Contains(WuxiaString) ||
                    rawText.Contains(BoxNovelString) ||
                    rawText.Contains(WebnovelString);
            }

            return CleanerUtils.FindTextNodesToDelete(doc, item, ShouldRemoveTextNode);
        }

        private static readonly string WuxiaString = CleanerUtils.StripWhiteSpace("Read latest Chapters at WuxiaWorld.S");
        private static readonly string WebnovelString = CleanerUtils.StripWhiteSpace("for faster releases read on webnovel");
        private static readonly string BoxNovelString = CleanerUtils.StripWhiteSpace("MYBOXNOVEL");
    }
}
