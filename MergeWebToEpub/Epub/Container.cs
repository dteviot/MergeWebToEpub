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
            this.FullPath = doc.Root.Descendants(Epub.containerNs + "rootfile").First().Attribute("full-path")?.Value;
        }

        public void WriteTo(ZipFile zipFile)
        {
            
            zipFile.AddEntry(Epub.ContainerPath, ToXDocument().ToSBytes());
        }

        public XDocument ToXDocument()
        {
            return new XDocument(
                new XElement(Epub.containerNs + "container",
                    new XAttribute("version", "1.0"),
                    new XAttribute("xmlns", "urn:oasis:names:tc:opendocument:xmlns:container"),
                    new XElement(Epub.containerNs + "rootfiles",
                        new XElement(Epub.containerNs + "rootfile",
                            new XAttribute("full-path", FullPath),
                            new XAttribute("media-type", "application/oebps-package+xml")
                        )
                    )
                )
            );
        }

        public string FullPath { get; set; }
    }
}
