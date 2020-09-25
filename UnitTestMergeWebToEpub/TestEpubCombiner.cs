using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MergeWebToEpub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestEpubCombiner
    {
        [TestMethod]
        public void TestGetMaxPrefix()
        {
            var combiner = new EpubCombiner(MockEpub1());
            combiner.ToAppend = MockEpub2();

            int actual = combiner.GetMaxPrefix(combiner.InitialEpub.Opf.GetPageItems());
            Assert.AreEqual(13, actual);
        }

        [TestMethod]
        public void TestCalculateNewPathsAndIds()
        {
            var combiner = MakeCombiner();
            Assert.AreEqual("cover0014", combiner.NewItemIds["cover"]);
            Assert.AreEqual("xhtml0015", combiner.NewItemIds["xhtml0000"]);
            Assert.AreEqual("OEPBS/Text/0014_Cover.xhtml", combiner.NewAbsolutePaths["OEPBS/Text/Cover.xhtml"]);
            Assert.AreEqual("OEPBS/Text/0015_Splash_pages.xhtml", combiner.NewAbsolutePaths["OEPBS/Text/0000_Splash_pages.xhtml"]);

            // ToDo: Test images updated correctly
            Assert.AreEqual("cover-image0002", combiner.NewItemIds["cover-image"]);
            Assert.AreEqual("image0004", combiner.NewItemIds["image0002"]);
            Assert.AreEqual("OEPBS/Images/0002_p1alt2en.png", combiner.NewAbsolutePaths["OEPBS/Images/0000_p1alt2en.png"]);
            Assert.AreEqual("OEPBS/Images/0022_p222-p223.png", combiner.NewAbsolutePaths["OEPBS/Images/0020_p222-p223.png"]);
        }

        [TestMethod]
        public void TestMetadata()
        {
            var combiner = MakeCombiner();
            var sources = combiner.InitialEpub.Opf.Metadata.ToXElement()
                .Elements(Epub.DaisyNs + "source")
                .Select(e => new Tuple<string, string>(e.Attribute("id").Value, e.Value))
                .ToList();
            Assert.AreEqual(40, sources.Count);
            Assert.AreEqual(new Tuple<string, string>("id.cover-image0002", "https://cgtranslations321782266.files.wordpress.com/2020/07/p1alt2en.png"), sources[14]);
            Assert.AreEqual(new Tuple<string, string>("id.image0010", "https://cgtranslations321782266.files.wordpress.com/2020/07/ch2.png"), sources[15]);
        }

        [TestMethod]
        public void TestSpine()
        {
            var combiner = MakeCombiner();
            var spine = combiner.InitialEpub.Opf.Spine;
            Assert.AreEqual(21, spine.Count);
            Assert.AreEqual("cover0014", spine[14].Id);
            Assert.AreEqual("xhtml0015", spine[15].Id);
        }

        [TestMethod]
        public void TestFixupUrl_HasFragment()
        {
            var combiner = MakeCombiner();
            var item = combiner.ToAppend.Opf.Manifest.Where(i => i.Id == "cover").First();
            var itemPath = item.AbsolutePath.GetZipPath();
            var actual = combiner.FixupUrl("../Images/0000_p1alt2en.png#start", itemPath);
            Assert.AreEqual("../Images/0002_p1alt2en.png#start", actual);
        }

        [TestMethod]
        public void TestFixupUrl_NoFragment()
        {
            var combiner = MakeCombiner();
            var item = combiner.ToAppend.Opf.Manifest.Where(i => i.Id == "cover").First();
            var itemPath = item.AbsolutePath.GetZipPath();
            var actual = combiner.FixupUrl("../Images/0000_p1alt2en.png", itemPath);
            Assert.AreEqual("../Images/0002_p1alt2en.png", actual);
        }

        [TestMethod]
        public void TestFixupUrl_OnlyFragment()
        {
            var combiner = MakeCombiner();
            var item = combiner.ToAppend.Opf.Manifest.Where(i => i.Id == "cover").First();
            var itemPath = item.AbsolutePath.GetZipPath();
            var actual = combiner.FixupUrl("#start", itemPath);
            Assert.AreEqual("#start", actual);
        }

        [TestMethod]
        public void TestFixupUrl_UrlExternalToDoc()
        {
            var combiner = MakeCombiner();
            var item = combiner.ToAppend.Opf.Manifest.Where(i => i.Id == "cover").First();
            var itemPath = item.AbsolutePath.GetZipPath();
            var actual = combiner.FixupUrl("https://cgtranslations.me/konosuba", itemPath);
            Assert.AreEqual("https://cgtranslations.me/konosuba", actual);
        }

        [TestMethod]
        public void TestUpdateXhtmlPage()
        {
            var combiner = MakeCombiner();
            var item = combiner.ToAppend.Opf.Manifest.Where(i => i.Id == "cover").First();
            var actual = combiner.UpdateXhtmlPage(item).ToXhtml();
            var element = actual.Root.Descendants(Epub.svgNs + "image").First();
            Assert.AreEqual("../Images/0002_p1alt2en.png", element.Attribute(Epub.xlinkNs + "href").Value);

            element = actual.Root.Descendants(Epub.xhtmlNs + "img").First();
            Assert.AreEqual("../Images/0005_typeset.jpg", element.Attribute("src").Value);

            var attribs = actual.Root.Descendants(Epub.xhtmlNs + "a")
                .Select(e => e.Attribute("href").Value)
                .ToList();
            Assert.AreEqual("0019_Chapter_4.xhtml", attribs[0]);
            Assert.AreEqual("https://cgtranslations.me/konosuba", attribs[1]);
        }

        [TestMethod]
        public void TestCopyTableOfContents()
        {
            var combiner = new EpubCombiner(MockEpub1());
            combiner.ToAppend = MockEpub2();

            var initialTocLength = combiner.InitialEpub.ToC.Entries.Count;
            combiner.Combine();
            var newToc = combiner.InitialEpub.ToC.Entries.Skip(initialTocLength).ToList();

            Assert.AreEqual(3, newToc.Count);
            Assert.AreEqual(0, newToc[0].Children.Count);
            Assert.AreEqual("Splash pages", newToc[0].Title);
            Assert.AreEqual("OEPBS/Text/0015_Splash_pages.xhtml", newToc[0].ContentSrc);
            Assert.AreEqual(4, newToc[1].Children.Count);
            Assert.AreEqual("Chapter 2", newToc[1].Children[1].Title);
            Assert.AreEqual("OEPBS/Text/0017_Chapter_2.xhtml", newToc[1].Children[1].ContentSrc);
        }

        [TestMethod]
        public void TestAdd_CallMultipleTimes()
        {
            var combiner = new EpubCombiner(MockEpub1());
            var manifest = combiner.InitialEpub.Opf.Manifest;
            Assert.AreEqual(16, manifest.Count);
            Assert.AreEqual(0, combiner.InitialEpub.Opf.GetImageItems().Count());

            var epub2 = MockEpub2();
            combiner.Add(epub2);
            Assert.AreEqual(43, manifest.Count);
            Assert.AreEqual(20, combiner.InitialEpub.Opf.GetImageItems().Count());

            combiner.Add(epub2);
            Assert.AreEqual(50, manifest.Count);
            Assert.AreEqual(20, combiner.InitialEpub.Opf.GetImageItems().Count());
            Assert.AreEqual(20, combiner.ImageHashes.Count);
            Assert.AreEqual("OEPBS/Images/0002_p1alt2en.png", combiner.NewAbsolutePaths["OEPBS/Images/0000_p1alt2en.png"]);
        }

        public EpubCombiner MakeCombiner()
        {
            var combiner = new EpubCombiner(MockEpub1());
            combiner.Add(MockEpub2());
            return combiner;
        }

        public Epub MockEpub1()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.content.opf");
            XDocument toc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.tocCultivation.ncx");

            var opf = new Opf(doc, "OEPBS/content.opf");
            MockRawData(opf);
            return new Epub()
            {
                Opf = opf,
                ToC = new ToC(toc, MockTocEpubItem(), opf.AbsolutePathIndex)
            };
        }

        public Epub MockEpub2()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.contentWithImages.opf");
            XDocument toc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.tocGifting.ncx");

            var opf = new Opf(doc, "OEPBS/content.opf");
            MockRawData(opf);
            return new Epub()
            {
                Opf = opf,
                ToC = new ToC(toc, MockTocEpubItem(), opf.AbsolutePathIndex)
            };
        }

        public void MockRawData(Opf opf)
        {
            foreach (var item in opf.Manifest)
            {
                if (item.IsXhtmlPage)
                {
                    item.RawBytes = MakeDummyDoc();
                }
                else if (item.IsImage)
                {
                    item.RawBytes = MakeDummyImage(item);
                }
            };
        }

        public EpubItem MockTocEpubItem()
        {
            var ncxItem = new EpubItem() { AbsolutePath = "OEPBS/toc.ncx" };
            return ncxItem;
        }

        public byte[] MakeDummyDoc()
        {
            var page = Xhtml.MakeEmptyXhtmlDoc();
            var bodyElement = XElement.Parse(
                "<div xmlns=\"http://www.w3.org/1999/xhtml\" class=\"svg_outer svg_inner\">" +
                    "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" height=\"99%\" width=\"100%\" version=\"1.1\" preserveAspectRatio=\"xMidYMid meet\" viewBox=\"0 0 1447 2092\">" +
                        "<image xlink:href=\"../Images/0000_p1alt2en.png\" width=\"1447\" height=\"2092\" />" +
                        "<desc>https://cgtranslations321782266.files.wordpress.com/2020/07/p1alt2en.png</desc>" +
                    "</svg>" +
                    "<img src=\"../Images/0003_typeset.jpg\" />" +
                    "<a href=\"0004_Chapter_4.xhtml\" />" +
                    "<a href=\"https://cgtranslations.me/konosuba\" />" +
                "</div>");
            page.Root.Element(Epub.xhtmlNs + "body").Add(bodyElement);
            return page.ToStream().ToArray();
        }

        public byte[] MakeDummyImage(EpubItem item)
        {
            return Encoding.UTF8.GetBytes(item.AbsolutePath);
        }
    }
}
