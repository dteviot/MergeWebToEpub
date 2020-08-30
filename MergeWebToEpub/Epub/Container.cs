using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    public class Container
    {
        public Container()
        {
        }

        public Container(XDocument doc)
        {
            this.doc = doc;
        }

        public string FullPath()
        {
            return doc.Root.Descendants(Epub.containerNs + "rootfile").First().Attribute("full-path")?.Value;
        }

        public void WriteTo(ZipFile zipFile)
        {
            // ToDo: should generate the XML from the FullPath value
            zipFile.AddEntry(Epub.ContainerPath, doc.ToStream());
        }

        private XDocument doc { get; set; }
    }
}
