using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Resolvers;
using System.Xml.Schema;

namespace MergeWebToEpub
{
    public static class ZipUtils
    {
        public static XDocument ExtractXml(this ZipFile zip, string entryName)
        {
            ZipEntry e = GetEntry(zip, entryName);
            using (var ms = new MemoryStream())
            {
                e.Extract(ms);
                ms.Flush();
                ms.Position = 0;
                return XDocument.Load(ms);
            }
        }

        public static byte[] ToSBytes(this XDocument doc)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Encoding = new UTF8Encoding(false)
            };
            using (var ms = new MemoryStream())
            using (XmlWriter writer = XmlWriter.Create(ms, settings))
            {
                doc.Save(writer);
                writer.Flush();
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        public static byte[] ExtractBytes(this ZipFile zip, string entryName)
        {
            ZipEntry e = GetEntry(zip, entryName);
            using (var ms = new MemoryStream())
            {
                e.Extract(ms);
                ms.Flush();
                return ms.ToArray();
            }
        }

        public static ZipEntry GetEntry(this ZipFile zip, string entryName)
        {
            // Note that hrefs have the name UrlEncoded
            // The filename in the ZIP is NOT UrlEncoded.
            ZipEntry e = zip[entryName];
            if (e == null)
            {
                e = zip[WebUtility.UrlDecode(entryName)];
            }
            return e;
        }

        public static XDocument ToXhtml(this byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes.stripNbsp()))
            using (var reader = XmlReader.Create(ms, GetXmlReaderSettings()))
            {
                return XDocument.Load(reader);
            }
        }

        public static byte[] stripNbsp(this byte[] buffer)
        {
            var text = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetBytes(text.stripNbsp());
        }

        public static string stripNbsp(this string text)
        {
            return text.Replace("&nbsp;", "&#160;")
                .Replace("&copy;", "&#169;")
                .Replace("&igrave;", "&#xcc;")
                .Replace("&egrave;", "&#xc8;")
                .Replace("&agrave;", "&#xc0;")
                .Replace("&uacute;", "&#xda;");
        }

        static XmlReaderSettings xmlReaderSettings = null;

        private static XmlReaderSettings GetXmlReaderSettings()
        {
            if (xmlReaderSettings == null)
            {
                var resolver = new XmlPreloadedResolver();
                resolver.AddDTD("http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", "MergeWebToEpub.Resources.xhtml11-flat.dtd");

                xmlReaderSettings = new XmlReaderSettings
                {
                    DtdProcessing = DtdProcessing.Parse,
                    ValidationType = ValidationType.DTD,
                    XmlResolver = resolver,
                };
                xmlReaderSettings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            }
            return xmlReaderSettings;
        }

        private static void AddDTD(this XmlPreloadedResolver resolver, string url, string resourceName)
        {
            using (var stream = ReadResource(resourceName))
            {
                resolver.Add(new Uri(url), stream);
            }
        }

        public static Stream ReadResource(string resourceName)
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        }

        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            // ignore all
        }

        public static Int64 ToHash(this byte[] rawBytes)
        {
            if (rawBytes.Length == 0)
            {
                return 0;
            }

            var sha = new SHA256Managed();
            byte[] checksum = sha.ComputeHash(rawBytes);
            return BitConverter.ToInt64(checksum, 0);
        }

        public static string RelativePathToAbsolute(string originFolder, string relative)
        {
            if (string.IsNullOrEmpty(originFolder))
            {
                return relative;
            }
            var start = originFolder.Split(pathSeperator).ToList();
            var end = relative.Split(pathSeperator).ToList();
            if ((0 < start.Count) && (0 < end.Count) && (end[0] == ".."))
            {
                start.RemoveAt(start.Count - 1);
                end.RemoveAt(0);
            }
            return string.Join("/", start.Concat(end));
        }

        public static string AbsolutePathToRelative(string startFolder, string endFileAndPath)
        {
            if (string.IsNullOrEmpty(startFolder))
            {
                return endFileAndPath;
            }
            var start = startFolder.Split(pathSeperator).ToList();
            var end = endFileAndPath.Split(pathSeperator).ToList();
            while ((0 < start.Count) && (0 < end.Count) && (start[0] == end[0]))
            {
                start.RemoveAt(0);
                end.RemoveAt(0);
            }
            var sb = new StringBuilder();
            for(int i = 0; i < start.Count; ++i)
            {
                sb.Append("../");
            }
            return sb.ToString() + String.Join("/", end);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipEntryFileName"></param>
        /// <returns>Path portion of a ZipEntry.FileName</returns>
        public static string GetZipPath(this string zipEntryFileName)
        {
            return Path.GetDirectoryName(zipEntryFileName).Replace("\\", "/");
        }

        public static string getZipFileName(this string zipEntryFileName)
        {
            return Path.GetFileName(zipEntryFileName);
        }

        private static readonly char[] pathSeperator = new char[] { '/' };
    }
}
