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
            zipFile.CompressionLevel = this.IsXhtmlPage
                ? Ionic.Zlib.CompressionLevel.BestCompression
                : Ionic.Zlib.CompressionLevel.None;
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

        public List<string> ValidateXhtml()
        {
            var errors = new List<string>();
            if (this.IsXhtmlPage)
            {
                try
                {
                    RawBytes.ToXhtml();
                }
                catch(Exception e)
                {
                    errors.Add($"Chapter '{AbsolutePath}' has error:  {e.Message}");
                }
            }
            return errors;
        }

        public List<string> ValidateXhtmlWitAgilityPack()
        {
            var errors = new List<string>();
            if (this.IsXhtmlPage)
            {
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.LoadHtml(Encoding.UTF8.GetString(RawBytes));
                if (htmlDoc.ParseErrors != null)
                {
                    foreach (var err in htmlDoc.ParseErrors)
                    {
                        errors.Add($"Chapter '{AbsolutePath}' has error: {err.Reason}");
                    }
                }
            }
            return errors;
        }

        public void RenumberItemId(int newIdNum)
        {
            var fileName = AbsolutePath.getZipFileName().StripPrefixFromFileName();
            var path = AbsolutePath.GetZipPath();
            if (!string.IsNullOrEmpty(path))
            {
                path += '/';
            }
            var newPrefix = newIdNum.ToString("D4");
            AbsolutePath = $"{path}{newPrefix}_{fileName}";
            Id = Id.StripDigits() + newPrefix;
        }

        public int PrefixAsInt()
        {
            return PrefixAsInt(AbsolutePath);
        }

        public static int PrefixAsInt(string absolutePath)
        {
            string prefixString = ExtractPrefixFromFileName(absolutePath);
            return string.IsNullOrEmpty(prefixString) ? 0 : Convert.ToInt32(prefixString);
        }

        /// <summary>
        /// Assumes file has a four digit numeric prefix, followed by an underscore
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public static string ExtractPrefixFromFileName(string absolutePath)
        {
            var fileName = absolutePath.getZipFileName();
            return ((5 < fileName.Length) && ((fileName[4] == '_') || (fileName[4] == '.')))
                ? fileName.Substring(0, 4)
                : null;
        }

        public void RemoveImageLink(string absolutePath)
        {
            string relativePath = ZipUtils.AbsolutePathToRelative(AbsolutePath.GetZipPath(), absolutePath);
            var doc = RawBytes.ToXhtml();
            bool changed = false;
            foreach (var element in FindImageElements(doc))
            {
                var href = element.GetImageHref();
                if (relativePath == href)
                {
                    doc.RemoveImage(element);
                    changed = true;
                }
            }
            if (changed)
            {
                RawBytes = doc.ToSBytes();
            }
        }

        public void AddImagesTo(Epub.ImageUseIndex index)
        {
            string itemPath = AbsolutePath.GetZipPath();
            XDocument doc = null;
            try
            {
                doc = RawBytes.ToXhtml();
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine($"XML Error with file {AbsolutePath}");
                return;
            }

            foreach (var element in FindImageElements(doc))
            {
                HashSet<EpubItem> set = null;
                var href = element.GetImageHref();
                var urlAbsolutePath = ZipUtils.RelativePathToAbsolute(itemPath, href);
                if (!index.TryGetValue(urlAbsolutePath, out set))
                {
                    set = new HashSet<EpubItem>();
                    index.Add(urlAbsolutePath, set);
                }
                set.Add(this);
            }
        }

        public List<XElement> FindImageElements(XDocument doc)
        {
            var elements = doc.Root.Descendants(Epub.svgNs + "image").ToList();
            elements.AddRange(doc.Root.Descendants(Epub.xhtmlNs + "img"));
            return elements;
        }

        public string Id { get; set; }

        public string MetadataId { get { return "id." + Id; } }
        public string AbsolutePath { get; set; }

        public string MediaType { get; set; }

        public byte[] RawBytes { get; set; }

        public bool IsXhtmlPage { get { return MediaType == Epub.XhtmlMedia; } }

        public bool IsImage { get { return MediaType.IndexOf("image/") == 0; } }

        /// <summary>
        /// URI for where item was originally obtained from
        /// </summary>
        public string Source { get; set; }

        public XElement SourceAsXml()
        {
            return string.IsNullOrEmpty(Source)
                ? null
                : new XElement(Epub.DaisyNs + "source",
                    new XAttribute("id", MetadataId),
                    Source
                );
        }
    }
}
