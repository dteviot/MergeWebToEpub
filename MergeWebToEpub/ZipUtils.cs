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

        public static MemoryStream ToStream(this XDocument doc)
        {
            var ms = new MemoryStream();
            doc.Save(ms);
            ms.Position = 0;
            return ms;
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
            using (var ms = new MemoryStream(bytes))
            using (var reader = XmlReader.Create(ms, GetXmlReaderSettings()))
            {
                return XDocument.Load(reader);
            }
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

        public static string ToHash(this byte[] rawBytes)
        {
            if (rawBytes.Length == 0)
            {
                return string.Empty;
            }

            var sha = new SHA256Managed();
            byte[] checksum = sha.ComputeHash(rawBytes);
            return BitConverter.ToString(checksum).Replace("-", String.Empty);
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
