using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public class NovelbinCleaner : CleanerBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            var toModify = doc.GetTextNodes().Where(HasWatermark).ToList();
            foreach (var node in toModify)
            {
                RemoveWatermark(node);
            }
            return false;
            // return 0 < toDelete.Count;
        }

        private bool HasWatermark(XText node)
        {
            string raw = node.Value.Replace((char)8230, (char)46);   // elipsis operator generates noise
            string normalized = raw.Normalize(NormalizationForm.FormKD);
            return !raw.Equals(normalized, StringComparison.Ordinal) && raw.Contains("(");
        }

        private void RemoveWatermark(XText node)
        {
            int startAt = FindStartOfWatermark(node.Value);
            if (0 < startAt)
            {
                System.Diagnostics.Trace.WriteLine(node.Value.Substring(startAt + 1));
            }
        }

        public static int FindStartOfWatermark(string raw)
        {
            int index = raw.LastIndexOf(".");
            --index;
            while ((0 < index) && !TerminalChar(raw[index]))
            {
                --index;
            }
            return index;
        }

        public static bool TerminalChar(char ch)
        {
            return (ch != '(')
                && (ch != ')')
                && (ch != '/')
                && Char.IsPunctuation(ch);
        }
    }
}
