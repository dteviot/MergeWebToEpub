using System;
using System.Linq;
using System.Xml.Linq;
using MergeWebToEpub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestCleaner
    {
        [TestMethod]
        public void TestRemoveScript()
        {
            XDocument doc = Utils.ReadXmlResource("UnitTestMergeWebToEpub.TestData.0000_Chapter_1_I_Became_Like_This_After_Reincarnating.xhtml");
            var cleaner = new UniversalCleaner();
            bool actual = cleaner.Clean(doc, null);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void TestChrysanthemumGardenDecryptText()
        {
            var cypherText = "——Pc gfagbrqfma, Bf Wec kbeiv tjnf qgfofggfv ab tjnf gfwjlcfv lvif. Snfc lo tf vfnfibqfv tfwbggtblvr ogbw rlaalcu rb ibcu, tf ralii kbeivc’a tjnf mtbrfc ab ajxf j rafq lcab atja ujiifgs.";
            var decryptor = new Decrypter(ChrysanthemumGardenCleaner.ClearText);
            var clearText = decryptor.DecryptText(cypherText);
            Assert.AreEqual("——In retrospect, Ke Xun would have preferred to have remained idle. Even if he developed hemorrhoids from sitting so long, he still wouldn’t have chosen to take a step into that gallery.", clearText);
        }

        [TestMethod]
        public void TestItalicWithSpan()
        {
            var doc = XDocument.Parse(
                "<?xml version='1.0' encoding='utf - 8' ?>" +
                "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.1//EN' 'http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd'>"+
                "<html xmlns='http://www.w3.org/1999/xhtml'><head><title /></head>"+
                "<body>" +
                "<div id='1'><i><span></span></i></div>" +  // should remove span but not italic
                "<div id='2'><span><i></i></span></div>" +  // should remove both
                "<div id='3'><span><i>2</i></span></div>" +  // should leave both
                "<div id='4'><span><i></i>3</span></div>" +  // should leave span
                "</body></html>"
                );

            string GetDiv(string id)
            {
                return doc.Descendants(Epub.xhtmlNs + "div")
                    .Where(e => e.Attribute("id").Value == id)
                    .First().ToString()
                    .StripWhiteSpace();
            }

            var cleaner = new UniversalCleaner();
            var item = new EpubItem() { Source = "dummy" } ;
            cleaner.Clean(doc, item);
            Assert.AreEqual("<divid=\"1\"xmlns=\"http://www.w3.org/1999/xhtml\"><i/></div>", GetDiv("1"));
            Assert.AreEqual("<divid=\"2\"xmlns=\"http://www.w3.org/1999/xhtml\"/>", GetDiv("2"));
            Assert.AreEqual("<divid=\"3\"xmlns=\"http://www.w3.org/1999/xhtml\"><span><i>2</i></span></div>", GetDiv("3"));
            Assert.AreEqual("<divid=\"4\"xmlns=\"http://www.w3.org/1999/xhtml\"><span>3</span></div>", GetDiv("4"));
        }

        [TestMethod]
        public void TestFindStartOfWatermark()
        {
            string raw = "“Let’s hope so.”Nêww 𝒄hapters will be fully updated at (n)ov(𝒆)l/bin(.)com";
            var startAt = NovelbinCleaner.FindStartOfWatermark(raw);
            Assert.AreEqual(startAt, 15);

            raw = "Reêad latest 𝒏ov𝒆ls at n𝒐𝒐v/e/l/bi𝒏(.)com";
            startAt = NovelbinCleaner.FindStartOfWatermark(raw);
        }
    }
}
