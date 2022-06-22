using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MergeWebToEpub.CleanerUtils;

namespace MergeWebToEpub
{
    class SecondLifeTranslationsCleaner : CleanerBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            if (!string.Equals(HostName(item), "secondlifetranslations.com"))
            {
                return false;
            }
            var toDelete = doc.FindElementsWithClassName("h2", "jmbl-disclaimer");
            toDelete.AddRange(doc.FindElementsWithClassName("div", "code-block"));
            DumpElements(item, toDelete);
            toDelete.RemoveElements();

            return DecryptDocument(doc) | (0 < toDelete.Count);
        }

        private static bool DecryptDocument(XDocument doc)
        {
            return decrypter.DecryptElements(doc.FindElementsWithClassName("span", "jmbl"));
        }

        public static string ClearText = "rhbndjzvqkiexcwsfpogytumalVUQXWSAZKBJNTLEDGIRHCPFOMY";

        private static Decrypter decrypter = new Decrypter(ClearText);
    }
}
