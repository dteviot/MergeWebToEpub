using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

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

        public static bool IsWebp(this EpubItem item)
        {
            return item.MediaType == Epub.webpMedia;
        }

        public static void ConvertToJpeg(this EpubItem item)
        {
            item.MediaType = Epub.jpegMedia;
            int index = item.AbsolutePath.LastIndexOf(".");
            item.AbsolutePath = item.AbsolutePath.Substring(0, index + 1) + "jpeg";
            var image = item.RawBytes.ExtractImage(true);
            item.RawBytes = image.ConvertToJpeg();
        }

        public static byte[] UpdateXhtmlPage(this EpubItem item, Dictionary<string, string> newAbsolutePaths)
        {
            System.Diagnostics.Trace.WriteLine($"Fixing up references on page {item.AbsolutePath}");
            var xhtml = item.RawBytes.ToXhtml();
            var itemPath = item.AbsolutePath.GetZipPath();
            FixupReferences(xhtml, itemPath, newAbsolutePaths);
            return xhtml.ToSBytes();
        }

        public static void FixupReferences(XDocument doc, string itemPath, Dictionary<string, string> newAbsolutePaths)
        {
            FixupReferences(doc, Epub.svgNs + "image", Epub.xlinkNs + "href", itemPath, newAbsolutePaths);
            FixupReferences(doc, Epub.xhtmlNs + "img", "src", itemPath, newAbsolutePaths);
            FixupReferences(doc, Epub.xhtmlNs + "a", "href", itemPath, newAbsolutePaths);
            // ToDo, <link> tags
        }

        public static void FixupReferences(XDocument doc, XName element, XName attributeName, string itemPath, Dictionary<string, string> newAbsolutePaths)
        {
            foreach (var e in doc.Root.Descendants(element))
            {
                var attrib = e.Attribute(attributeName);
                if (attrib != null)
                {
                    attrib.Value = FixupUrl(attrib.Value, itemPath, newAbsolutePaths);
                }
            }
        }

        public static string FixupUrl(string uri, string itemPath, Dictionary<string, string> newAbsolutePaths)
        {
            // special case, it's a link to anchor on same page
            if (uri[0] == '#')
            {
                return uri;
            }

            // internal URLs are relative, so, if not relative
            // leave it alone
            Uri testUrl = null;
            if (!Uri.TryCreate(uri, UriKind.Relative, out testUrl))
            {
                return uri;
            }

            var fragments = uri.Split(new char[] { '#' });
            var path = fragments[0];
            var urlAbsolutePath = ZipUtils.RelativePathToAbsolute(itemPath, path);
            string newAbsolutePath = null;
            if (!newAbsolutePaths.TryGetValue(urlAbsolutePath, out newAbsolutePath))
            {
                // URL isn't in set to update
                return uri;
            }
            var newRelativePath = ZipUtils.AbsolutePathToRelative(itemPath, newAbsolutePath);
            if (2 == fragments.Length)
            {
                newRelativePath += "#" + fragments[1];
            }
            return newRelativePath;
        }

        public static Image ExtractImage(this EpubItem item)
        {
            System.Diagnostics.Trace.Assert(item.IsImage);
            return item.RawBytes.ExtractImage(item.IsWebp());
        }

        public static void ConvertWebpImagesToJpeg(this Epub epub)
        {
            var imageItems = epub.Opf.GetImageItems().Where(EpubUtils.IsWebp);
            var newPaths = new Dictionary<string, string>();
            foreach(var item in imageItems.Where(IsWebp))
            {
                var old = item.AbsolutePath;
                item.ConvertToJpeg();
                newPaths.Add(old, item.AbsolutePath);
            }
            foreach(var item in epub.Opf.GetPageItems())
            {
                item.RawBytes = item.UpdateXhtmlPage(newPaths);
            }
            MessageBox.Show("Done");
        }

        public static string GetImageHref(this XElement element)
        {
            if (element.Name == Epub.svgNs + "image")
            {
                return element.Attribute(Epub.xlinkNs + "href").Value;
            }
            else if (element.Name == Epub.xhtmlNs + "img")
            {
                return element.Attribute("src").Value;
            }
            else
            {
                throw new Exception("Element is not an <image> or <img>: " + element.ToString());
            }
        }

        public static void RemoveImage(this XDocument doc, XElement element)
        {
            if (element.Name == Epub.svgNs + "image")
            {
                element.Parent.Remove();
            }
            else if (element.Name == Epub.xhtmlNs + "img")
            {
                element.Remove();
            }
            else
            {
                throw new Exception("Element is not an <image> or <img>: " + element.ToString());
            }
        }
    }
}