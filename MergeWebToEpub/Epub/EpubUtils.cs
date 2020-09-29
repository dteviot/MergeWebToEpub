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
                : -1;
        }
    }
}