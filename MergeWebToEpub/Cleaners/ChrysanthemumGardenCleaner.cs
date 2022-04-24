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
            bool modified = false;
            foreach (var e in doc.FindElementsWithClassName("span", "jum"))
            {
                var clearText = DecryptText(e.Value);
                if (!clearText.Equals(e.Value))
                {
                    modified = true;
                    e.Value = clearText;
                }
            }
            return modified;
        }

        public static string DecryptText(string cypherText)
        {
            var sb = new StringBuilder();
            foreach (var c in cypherText)
            {
                char p;
                sb.Append(decryptTable.TryGetValue(c, out p) ? p : c);
            }
            return sb.ToString();
        }

        private static Dictionary<char, char> MakeDecryptTable()
        {
            const string crypt = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string clear = "tonquerzlawicvfjpsyhgdmkbxJKABRUDQZCTHFVLIWNEYPSXGOM";

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

        private static Dictionary<char, char> decryptTable = MakeDecryptTable();
    }
}
