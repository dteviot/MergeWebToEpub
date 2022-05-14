using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    // operations for every XHTML file
    public class UniversalCleaner : CleanerBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            return RemoveScripts(doc)
                | RemoveEzoic(doc, item)
                | doc.RemoveEmptyDivElements()
                | RemoveEmptyItalic(doc, item)
                | RemoveEmptySpan(doc, item);
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

        public bool RemoveEzoic(XDocument doc, EpubItem item)
        {
            return CleanerUtils.RemoveElementsMatchingFilter(doc, item, IsEzoicElement);
        }

        public bool RemoveEmptyItalic(XDocument doc, EpubItem item)
        {
            return CleanerUtils.RemoveElementsMatchingFilter(doc, item, IsEmptyItalic);
        }

        public bool RemoveEmptySpan(XDocument doc, EpubItem item)
        {
            return CleanerUtils.RemoveElementsMatchingFilter(doc, item, IsEmptySpan);
        }

        public bool IsEzoicElement(XElement element)
        {
            var classNames = element.ClassNames();
            return classNames.Contains("ezoic-adpicker-ad") ||
                classNames.Contains("ezoic-ad");
        }

        public bool IsEmptyItalic(XElement element)
        {
            return (element.Name.LocalName == "i")
                && (!element.HasElements)
                && (element.Value.Trim().Length == 0);
        }

        public bool IsEmptySpan(XElement element)
        {
            return (element.Name.LocalName == "span")
                && (!element.HasElements)
                && (element.Value.Trim().Length == 0);
        }
    }
}
