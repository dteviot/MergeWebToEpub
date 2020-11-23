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
            Cleaners = new List<CleanderBase>()
            {
                new NovelfullCleaner(),
                new ScriptCleaner(),
                new VipnovelCleaner()
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
            bool changed = false;
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

        private List<CleanderBase> Cleaners { get; set; }
    }
}
