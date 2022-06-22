using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MergeWebToEpub.CleanerUtils;

namespace MergeWebToEpub
{
    public class ChrysanthemumGardenCleaner : CleanerBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            if (!string.Equals(HostName(item), "chrysanthemumgarden.com"))
            {
                return false;
            }
            return DecryptDocument(doc);
        }

        private static bool DecryptDocument(XDocument doc)
        {
            return decrypter.DecryptElements(doc.FindElementsWithClassName("span", "jum"));
        }

        public static string ClearText = "tonquerzlawicvfjpsyhgdmkbxJKABRUDQZCTHFVLIWNEYPSXGOM";

        private static Decrypter decrypter = new Decrypter(ClearText);
    }
}
