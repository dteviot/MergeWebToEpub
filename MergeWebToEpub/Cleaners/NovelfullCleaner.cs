using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public class NovelfullCleaner : CleanderBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            if (!string.Equals(HostName(item), "novelfull.com"))
            {
                return false;
            }

            var toDelete = doc.GetTextNodes().Where(ShouldRemoveTextNode).ToList();
            foreach(var node in toDelete)
            {
                System.Diagnostics.Trace.WriteLine(node.Value);
                node.Remove();
            }
            return 0 < toDelete.Count;
        }

        private bool ShouldRemoveTextNode(XText node)
        {
            return node.Value.Contains("If you find any errors ( broken links, non-standard content, etc.. ), Please let us know ");
        }
    }
}
