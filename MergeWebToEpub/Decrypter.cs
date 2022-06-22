using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace MergeWebToEpub
{
    public class Decrypter
    {
        public Decrypter(string cleartext)
        {
            MakeDecryptTable(DefaultCypherText, cleartext);
        }

        public bool DecryptElements(IEnumerable<XElement> encryptedElements)
        {
            bool modified = false;
            foreach (var e in encryptedElements)
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

        public string DecryptText(string cypherText)
        {
            var sb = new StringBuilder();
            foreach (var c in cypherText)
            {
                char p;
                sb.Append(decryptTable.TryGetValue(c, out p) ? p : c);
            }
            return sb.ToString();
        }

        private Dictionary<char, char> MakeDecryptTable(string cyphertext, string cleartext)
        {
            if (decryptTable == null)
            {
                decryptTable = new Dictionary<char, char>();
                for (int i = 0; i < cyphertext.Length; ++i)
                {
                    decryptTable.Add(cyphertext[i], cleartext[i]);
                }
            }
            return decryptTable;
        }

        private Dictionary<char, char> decryptTable;

        public static string DefaultCypherText = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
}
