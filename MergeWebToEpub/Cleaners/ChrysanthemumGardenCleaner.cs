using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MergeWebToEpub.CleanerUtils;

namespace MergeWebToEpub
{
    public class ChrysanthemumGardenCleaner : CleanderBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            if (!string.Equals(HostName(item), "chrysanthemumgarden.com"))
            {
                return false;
            }
            MakeDecryptTable();
            return DecryptText(doc);
        }

        private static bool DecryptText(XDocument doc)
        {
            bool modified = false;
            foreach (var e in doc.FindElementsWithClassName("span", "jum"))
            {
                var sb = new StringBuilder();
                foreach(var c in e.Value)
                {
                    char p = c;
                    if (decryptTable.TryGetValue(c, out p))
                    {
                        modified = true;
                        sb.Append(p);
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                if (modified)
                {
                    e.Value = sb.ToString();
                }
            }
            return modified;
        }

        private static Dictionary<char, char> MakeDecryptTable()
        {
            if (decryptTable == null)
            {
                decryptTable = new Dictionary<char, char>();
                for(int i = 0; i < crypt.Length; ++i)
                {
                    decryptTable.Add(crypt[i], clear[i]);
                }
            }
            return decryptTable;
        }

        private static Dictionary<char, char> decryptTable = null;

        private Signature previousSignature = new Signature();

        private static string crypt = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static string clear = "tonquerzlawicvfjpsyhgdmkbxJKABRUDQZCTHFVLIWNEYPSXGOM";
    }
}
