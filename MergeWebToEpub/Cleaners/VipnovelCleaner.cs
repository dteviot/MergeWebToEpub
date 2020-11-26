using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MergeWebToEpub.CleanerUtils;

namespace MergeWebToEpub
{
    public class VipnovelCleaner : CleanderBase
    {
        public override bool Clean(XDocument doc, EpubItem item)
        {
            if (!string.Equals(HostName(item), "vipnovel.com"))
            {
                return false;
            }

            bool error = false;
            var sig = doc.CalcSignature();
            var errorMsg = item.CheckForErrors(sig, previousSignature);
            if (errorMsg != null)
            {
                System.Diagnostics.Trace.WriteLine(errorMsg);
                error = true;
            }
            previousSignature = sig;
            return error;
        }

        private Signature previousSignature = new Signature();

    }
}
