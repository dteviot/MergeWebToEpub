using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeWebToEpub
{
    public partial class Epub
    {
        public Epub()
        {

        }

        public void ReadFile(string fileName)
        {
            using (ZipFile zip = ZipFile.Read(fileName))
            {
                Container = new Container(zip.ExtractXml(Epub.ContainerPath));
                var containerPath = Container.FullPath;
                Opf = new Opf(zip.ExtractXml(containerPath), containerPath);
                foreach (var item in Opf.Manifest)
                {
                    item.AddRawDataToItem(zip);
                }
                var ncxItem = Opf.NcxItem;
                ToC = new ToC(zip.ExtractXml(ncxItem.AbsolutePath), ncxItem, Opf.AbsolutePathIndex);
            }
        }

        public void WriteFile(string fileName)
        {
            using (ZipFile zip = new ZipFile())
            {
                AddMimeType(zip);
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                Container.WriteTo(zip);
                Opf.WriteTo(zip);
                ToC.WriteToNcxEpubItem();
                Opf.WriteManifestTo(zip);

                zip.Save(fileName);
            }
        }

        private static void AddMimeType(ZipFile zip)
        {
            // Setting this makes mimetype correctly format.
            // refer https://stackoverflow.com/questions/33726113/the-mimetype-file-has-an-extra-field-of-length-n-the-use-of-the-extra-field-fea/33726439#33726439
            zip.EmitTimesInWindowsFormatWhenSaving = false;
            zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
            zip.AddEntry(Epub.MimeType, "application/epub+zip");
        }

        public void DeleteItem(EpubItem item)
        {
            Opf.DeleteItem(item);
            ToC.DeleteItem(item);
        }

        public List<string> ValidateXhtml()
        {
            var errors = new List<string>();
            foreach(var item in Opf.Manifest)
            {
                errors.AddRange(item.ValidateXhtml());
            }
            return errors;
        }

        public Container Container { get; set; }
        public Opf Opf { get; set; }
        public ToC ToC { get; set; }
    }
}
