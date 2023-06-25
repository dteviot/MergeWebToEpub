using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MergeWebToEpub
{
    public class Examples
    {
        public static void CombineTwoEpubs()
        {
            var epub1 = new Epub();
            var epub2 = new Epub();
            try
            {
                epub1.ReadFile(@"D:\temp\work\zip\unpack\ccg\Cultivation Chat Group.c1313-1325.epub");
                epub2.ReadFile(@"D:\temp\work\zip\unpack\ccg\Cultivation Chat Group.c1326-c1331.epub");

                var combiner = new EpubCombiner(epub1);
                combiner.Add(epub2);

                epub1.WriteFile(@"D:\temp\work\zip\unpack\ccg\Cultivation Chat Group.c1313-c1331.epub");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
