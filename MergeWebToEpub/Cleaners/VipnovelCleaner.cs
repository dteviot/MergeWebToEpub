using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MergeWebToEpub.CleanerUtils;

namespace MergeWebToEpub
{
    public class VipnovelCleaner : CleanerBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            if (!string.Equals(HostName(item), "vipnovel.com"))
            {
                return false;
            }

            bool modified = false;
            var toDelete = doc.FindElementsMatching("div", IsZipNovelPromo);
            toDelete.RemoveElements();
            return modified;
        }

        private static bool IsZipNovelPromo(XElement element)
        {
            return element.Value?.StartsWith("Read more chapter on") ?? false;
        }

        private Signature previousSignature = new Signature();

    }
}
