using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public class VipnovelCleaner : CleanderBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            if (!string.Equals(HostName(item), "vipnovel.com"))
            {
                return false;
            }

            bool error = false;
            var paragraphs = doc.Root.Descendants(Epub.xhtmlNs + "p").ToList();
            var max = Math.Min(paragraphs.Count, 3);
            if (2 <= max)
            {
                string text = paragraphs[max - 1].Value.Trim();
                error = text.Equals(PreviousParagraph);
                if (error)
                {
                    System.Diagnostics.Trace.WriteLine($"Possible Duplicate chaptesr {item.AbsolutePath}");
                    System.Diagnostics.Trace.WriteLine(text);
                }
                PreviousParagraph = text;
            }
            return error;
        }

        private string PreviousParagraph = string.Empty;

    }
}
