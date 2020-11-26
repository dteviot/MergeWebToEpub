using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    // Base class all cleaners derive from
    public abstract class CleanderBase
    {
        public abstract bool Clean(XDocument doc, EpubItem item);

        public static string HostName(EpubItem item)
        {
            return string.IsNullOrEmpty(item.Source)
                ? null
                : new Uri(item.Source)?.Host;
        }
    }
}
