using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public static class CleanerUtils
    {
        public class Signature : Dictionary<Int64, int> 
        {
            public bool HasImages { get; set; }
        }

        static public Signature CalcSignature(this XDocument doc)
        {
            var sig = new Signature()
            {
                HasImages = 0 < doc.GetImages().Count
            };
            var texts = doc.GetTextNodes()
                .Select(RemoveWhitespace)
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();
            foreach (var s in texts)
            {
                var hash = Encoding.UTF8.GetBytes(s).ToHash();
                int count = 0;
                if (!sig.TryGetValue(hash, out count))
                {
                    sig.Add(hash, 1);
                }
                else
                {
                    sig[hash] = count + 1;
                }
            }
            return sig;
        }

        static public IEnumerable<XText> GetTextNodes(this XDocument doc)
        {
            return doc.DescendantNodes().OfType<XText>();
        }

        static public List<XElement> GetImages(this XDocument doc)
        {
            var images = doc.Descendants(Epub.svgNs + "image").ToList();
            images.AddRange(doc.Descendants(Epub.xhtmlNs + "img"));
            return images;
        }

        static public string RemoveWhitespace(this XText node)
        {
            var sb = new StringBuilder();
            foreach(var c in node.Value)
            {
                if (!char.IsWhiteSpace(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        static public string CheckForErrors(this EpubItem item, Signature sig1, Signature prevSig)
        {
            var baseName = Path.GetFileName(item.AbsolutePath);
            bool littleTextExpected = baseName.Equals("0000_Information.xhtml") || sig1.HasImages;
            if (littleTextExpected)
            {
                return null;
            }
            if (sig1.Count < 3)
            {
                return $"{item.AbsolutePath} might be empty";
            }
            else if (sig1.ProbableDuplicate(prevSig))
            {
                return $"Possible Duplicate chapters {item.AbsolutePath}";
            }
            return null;
        }

        /// <summary>
        /// probably a duplicate if 50% or more of the lines are the same
        /// </summary>
        static public bool ProbableDuplicate(this Signature sig1, Signature sig2)
        {
            int sameLines = 0;
            int totalLines = 0;
            foreach(var pair in sig1)
            {
                totalLines += pair.Value;
                int count = 0;
                if (sig2.TryGetValue(pair.Key, out count))
                {
                    sameLines += Math.Min(pair.Value, count);
                }
            }
            return (totalLines / 2) <= sameLines;
        }

        public static List<XElement> FindElementsMatching(this XDocument doc, string elementName, Func<XElement, bool> testElement)
        {
            return doc.Root.Descendants(Epub.xhtmlNs + elementName)
                .Where(testElement)
                .ToList();
        }

        public static List<XElement> FindElementsWithClassName(this XDocument doc, string elementName, string className)
        {
            bool ClassNameMatches(XElement e)
            {
                return e.ClassNames().Contains(className);
            }
            return doc.FindElementsMatching(elementName, ClassNameMatches);
        }

        public static string[] ClassNames(this XElement element)
        {
            return element.Attribute("class")?.Value?.Split(new char[] { ' ' }) ?? new string[] { };
        }

        public static void RemoveElements(this IEnumerable<XNode> nodes)
        {
            foreach (var node in nodes)
            {
                node.Remove();
            }
        }

        public static bool RemoveEmptyDivElements(this XDocument doc)
        {
            bool modified = false;
            bool doSweep = true;
            while (doSweep)
            {
                var toDelete = doc.Root.Descendants(Epub.xhtmlNs + "div")
                    .Where(HasNoChildNodes)
                    .ToList();
                toDelete.RemoveElements();
                doSweep = 0 < toDelete.Count;
                modified |= doSweep;
            }
            return modified;
        }

        public static bool HasNoChildNodes(this XElement element)
        {
            return element.Nodes().Count() == 0;
        }

        public static List<XText> FindTextNodesToDelete(XDocument doc, EpubItem item, Func<XText, bool>shouldRemoveTextNode)
        {
            var toDelete = doc.GetTextNodes()
                .Where(shouldRemoveTextNode)
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

        public static void DumpElements(EpubItem item, List<XElement> elements)
        {
            if (0 < elements.Count)
            {
                System.Diagnostics.Trace.WriteLine($"Changing: {item.Source}");
                foreach (var e in elements)
                {
                    System.Diagnostics.Trace.WriteLine($" ==>  {e}");
                }
            }
        }

        public static bool RemoveElementsMatchingFilter(XDocument doc, EpubItem item, Func<XElement, bool> filter)
        {
            var toRemove = doc.Root.DescendantsAndSelf()
                .Where(filter)
                .ToList();
            toRemove.RemoveElements();
            DumpElements(item, toRemove);
            return 0 < toRemove.Count;
        }

        public static string StripWhiteSpace(this string toStrip)
        {
            var sb = new StringBuilder();
            foreach (var c in toStrip)
            {
                if (!char.IsWhiteSpace(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
