using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    [DebuggerDisplay("{Id}: {AbsolutePath}")]
    public class EpubItem
    {
        public EpubItem()
        {
        }

        public EpubItem(XElement element, string opfFolder)
        {
            this.Id = element.Attribute("id").Value;
            string relativePath = element.Attribute("href").Value;
            this.AbsolutePath = ZipUtils.RelativePathToAbsolute(opfFolder, relativePath);
            this.MediaType = element.Attribute("media-type").Value;
        }

        public void AddRawDataToItem(ZipFile zip)
        {
            this.RawBytes = zip.ExtractBytes(this.AbsolutePath);
        }

        public void WriteTo(ZipFile zipFile)
        {
            zipFile.AddEntry(AbsolutePath, RawBytes);
        }

        public XElement ToXElement(string opfFolder)
        {
            return new XElement(Epub.PackageNs + "item",
                new XAttribute("href", ZipUtils.AbsolutePathToRelative(opfFolder, this.AbsolutePath)),
                new XAttribute("id", Id),
                new XAttribute("media-type", MediaType)
            );
        }

        public string Id { get; set; }

        public string MetadataId { get { return "id." + Id; } }
        public string AbsolutePath { get; set; }

        public string MediaType { get; set; }

        public byte[] RawBytes { get; set; }

        public bool IsXhtmlPage { get { return MediaType == Epub.XhtmlMedia; } }

        public bool IsImage { get { return MediaType.IndexOf("image/") == 0; } }
        public XElement ToManifestItem()
        {
            throw new NotImplementedException();
        }
    }
}
