using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public class RoyalRoadCleaner : CleanerBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            if (!string.Equals(HostName(item), "www.royalroad.com", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var targets = doc.FindElementsMatching("table", e => GetStyle(e) != null);
            foreach (var table in targets)
            {
                var style = GetStyle(table);
                System.Diagnostics.Trace.WriteLine(style.ToString());
                style.Remove();
            }
            return 0 < targets.Count;
        }

        private XAttribute GetStyle(XElement element)
        {
            return element.Attribute("style");
        }
    }
}
