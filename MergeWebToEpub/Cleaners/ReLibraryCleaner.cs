using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MergeWebToEpub.CleanerUtils;

namespace MergeWebToEpub
{
    public class ReLibraryCleaner : CleanerBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            if (!string.Equals(HostName(item), "re-library.com"))
            {
                return false;
            }

            var targets = doc.FindElementsMatching("a", e => e.Value == "⌈ Index ⌋");
            targets.AddRange(doc.FindElementsWithClassName("div", "code-block"));
            targets.AddRange(FindLeaveCommentElements(doc));
            targets.RemoveElements();
            return 0 < targets.Count;
        }

        public IEnumerable<XElement> FindLeaveCommentElements(XDocument doc)
        {
            return doc.FindElementsMatching("a", e => e.Attribute("href")?.Value == "#respond")
                .Select(e => e.Parent);
        }
    }
}
