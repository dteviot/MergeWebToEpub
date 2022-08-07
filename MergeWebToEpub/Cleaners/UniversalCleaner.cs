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
                | StripWebnovelChaff(doc, item)
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

        public bool StripWebnovelChaff(XDocument doc, EpubItem item)
        {
            return CleanerUtils.RemoveElementsMatchingFilter(doc, item, IsWebnovelParaCommentNum)
                | StripParagraphDivsFromWebNovel(doc, item);
        }

        public bool StripParagraphDivsFromWebNovel(XDocument doc, EpubItem item)
        {
            var toDelete = new List<XElement>();
            var words = doc.Root.DescendantsAndSelf().Where(IsWebnovelWordsElement).FirstOrDefault();
            if (words != null)
            {
                foreach (var paragraph in words.Elements().Where(IsWebnovelParagraphElement))
                {
                    var dib = paragraph.Elements().Where(IsWebnovelDibElement).FirstOrDefault();
                    if (dib != null)
                    {
                        var children = dib.Elements().ToList();
                        foreach (var c in children)
                        {
                            words.Add(c);
                        }
                        toDelete.Add(paragraph);
                    }
                }
            }
            CleanerUtils.DumpElements(item, toDelete);
            toDelete.RemoveElements();
            return 0 < toDelete.Count;
        }

        public bool IsWebnovelParaCommentNum(XElement element)
        {
            return (element.Name.LocalName == "i")
                && (element.ClassNames().Contains("para-comment-num"));
        }

        public bool IsWebnovelWordsElement(XElement element)
        {
            return (element.Name.LocalName == "div")
                && (element.ClassNames().Contains("cha-words"));
        }

        public bool IsWebnovelParagraphElement(XElement element)
        {
            return (element.Name.LocalName == "div")
                && (element.ClassNames().Contains("cha-paragraph"));
        }
        public bool IsWebnovelDibElement(XElement element)
        {
            return (element.Name.LocalName == "div")
                && (element.Attribute("class").Value == "dib pr");
        }
    }
}
