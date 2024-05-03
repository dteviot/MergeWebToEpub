using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeWebToEpub
{
    class CleanerEngine
    {
        public CleanerEngine()
        {
            Cleaners = new List<CleanerBase>()
            {
                new NovelfullCleaner(),
                new ReLibraryCleaner(),
                new VipnovelCleaner(),
                new ChrysanthemumGardenCleaner(),
                new SecondLifeTranslationsCleaner(),
                new WuxiapubCleaner(),
                new RoyalRoadCleaner(),
                new UniversalCleaner(),
            };
        }

        public bool Clean(Epub epub)
        {
            var changes = false;
            foreach (var item in epub.Opf.GetPageItems())
            {
                changes |= Clean(item);
            }
            return changes;
        }

        public bool Clean(EpubItem item)
        {
            if (!string.IsNullOrEmpty(item.Source))
            {
                System.Diagnostics.Trace.WriteLine($"Cleaning: {item.Source}");
            }

            var text = Encoding.UTF8.GetString(item.RawBytes, 0, item.RawBytes.Length);
            var cleanText = text.stripNbsp();
            bool changed = string.Equals(cleanText, text);
            var doc = item.RawBytes.ToXhtml();
            foreach (var cleaner in Cleaners)
            {
                changed |= cleaner.Clean(doc, item);
            }
            if (changed)
            {
                item.RawBytes = doc.ToSBytes();
            }
            return changed;
        }

        private List<CleanerBase> Cleaners { get; set; }
    }
}
