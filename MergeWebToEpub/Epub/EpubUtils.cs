using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeWebToEpub
{
    public static class EpubUtils
    {
        public static int ExtractProbableChapterNumber(this string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return Epub.NoChapterNum;
            }
            var digits = new StringBuilder();
            bool foundDigit = false;
            foreach (var c in title)
            {
                if (Char.IsDigit(c))
                {
                    foundDigit = true;
                    digits.Append(c);
                }
                else if (foundDigit)
                {
                    break;
                }
            }
            return (0 < digits.Length)
                ? Convert.ToInt32(digits.ToString())
                : Epub.NoChapterNum;
        }

        public static int GetMaxPrefix(this List<EpubItem> items)
        {
            int maxPrefix = 0;
            foreach (var item in items)
            {
                var prefix = item.PrefixAsInt();
                maxPrefix = Math.Max(maxPrefix, Convert.ToInt32(prefix));
            }
            return maxPrefix;
        }

        public static string StripDigits(this string oldId)
        {
            var prefix = new StringBuilder();
            foreach (var c in oldId)
            {
                if (!Char.IsDigit(c))
                {
                    prefix.Append(c);
                }
            }
            return prefix.ToString();
        }

        public static string StripPrefixFromFileName(this string fileName)
        {
            return ((5 < fileName.Length) && (fileName[4] == '_'))
                ? fileName.Substring(5, fileName.Length - 5)
                : fileName;
        }
    }
}