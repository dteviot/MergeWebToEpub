using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    // operations for every XHTML file
    public class UniversalCleaner : CleanderBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            return RemoveScripts(doc, item)
                | doc.RemoveEmptyDivElements();
        }

        public bool RemoveScripts(XDocument doc, EpubItem item)
        {
            var scripts = doc.Root.Descendants(Epub.xhtmlNs + "script").ToList();
            foreach (var script in scripts)
            {
                System.Diagnostics.Trace.WriteLine(script.ToString());
                script.Remove();
            }
            return 0 < scripts.Count;
        }
    }
}
