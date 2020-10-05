using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public partial class Epub
    {
        public const string ContainerPath = "META-INF/container.xml";
        public const string MimeType = "mimetype";
        public const string XhtmlMedia = "application/xhtml+xml";
        public const string webpMedia = "image/webp";
        public const string jpegMedia = "image/jpeg";
        public const string CoverPageId = "cover";                 // Magic "Number".

        public static readonly XNamespace containerNs = "urn:oasis:names:tc:opendocument:xmlns:container";
        public static readonly XNamespace PackageNs = "http://www.idpf.org/2007/opf";
        public static readonly XNamespace DaisyNs = "http://purl.org/dc/elements/1.1/";
        public static readonly XNamespace ncxNs = "http://www.daisy.org/z3986/2005/ncx/";

        public static readonly XNamespace xmlNs = "http://www.w3.org/XML/1998/namespace";
        public static readonly XNamespace xhtmlNs = "http://www.w3.org/1999/xhtml";
        public static readonly XNamespace xlinkNs = "http://www.w3.org/1999/xlink";
        public static readonly XNamespace svgNs = "http://www.w3.org/2000/svg";

        public const string EmptyXhtmlDoc = "<?xml version=\"1.0\" encoding=\"utf-8\"?><!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title></title><link href=\"../Styles/stylesheet.css\" type=\"text/css\" rel=\"stylesheet\" /></head><body></body></html>";

        public const int NoChapterNum = -1;
    }
}
