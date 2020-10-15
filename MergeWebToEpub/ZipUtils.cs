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
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public static class ZipUtils
    {
        public static XDocument ExtractXml(this ZipFile zip, string entryName)
        {
            ZipEntry e = zip[entryName];
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
            ZipEntry e = zip[entryName];
            if (e == null)
            {
                e = zip[WebUtility.UrlDecode(entryName)];
            }
            using (var ms = new MemoryStream())
            {
                e.Extract(ms);
                ms.Flush();
                return ms.ToArray();
            }
        }

        public static XDocument ToXhtml(this byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes.FixupNbsp()))
            {
                return XDocument.Load(ms);
            }
        }

        /// <summary>
        /// Ugly hack to deal with XmlReader faulting on &nbsp;
        /// Ideally, would use XmlPreloadedResolver against xhtml11-flat.dtd
        /// But that's too strict
        /// </summary>
        /// <param name="byutes"></param>
        /// <returns></returns>
        public static byte[] FixupNbsp(this byte[] bytes)
        {
            var s = Encoding.UTF8.GetString(bytes);
            return Encoding.UTF8.GetBytes(s.Replace("&nbsp;", "&#160;"));
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
