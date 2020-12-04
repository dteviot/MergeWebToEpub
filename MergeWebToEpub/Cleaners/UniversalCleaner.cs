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
            return RemoveScripts(doc)
                | RemoveEzoic(doc)
                | doc.RemoveEmptyDivElements();
        }

        public bool RemoveScripts(XDocument doc)
        {
            var scripts = doc.Root.Descendants(Epub.xhtmlNs + "script").ToList();
            foreach (var script in scripts)
            {
                System.Diagnostics.Trace.WriteLine(script.ToString());
                script.Remove();
            }
            return 0 < scripts.Count;
        }

        public bool RemoveEzoic(XDocument doc)
        {
            var ezoic = doc.Root.DescendantsAndSelf()
                .Where(IsEzoicElement)
                .ToList();
            foreach (var element in ezoic)
            {
                System.Diagnostics.Trace.WriteLine(element.ToString());
                element.Remove();
            }
            return 0 < ezoic.Count;
        }

        public bool IsEzoicElement(XElement element)
        {
            var classNames = element.ClassNames();
            return classNames.Contains("ezoic-adpicker-ad") ||
                classNames.Contains("ezoic-ad");
        }
    }
}
