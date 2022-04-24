using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var toDelete = FindElementsToDelete(doc);
            foreach (var e in toDelete)
            {
                System.Diagnostics.Trace.WriteLine(e.Value);
            }
            toDelete.RemoveElements();
            return 0 < toDelete.Count;
        }

        private static List<XText> FindElementsToDelete(XDocument doc)
        {
            bool ShouldRemoveTextNode(XText text)
            {
                string rawText = text.Value.Trim();
                return
                    rawText.Equals("Advertisements") ||
                    rawText.Contains("Read latest Chapters at WuxiaWorld.Site");
            }

            return  doc.GetTextNodes()
                .Where(ShouldRemoveTextNode)
                .ToList();
        }
    }
}
