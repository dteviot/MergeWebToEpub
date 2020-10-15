using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MergeWebToEpub;
using System.Text;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestZipUtils
    {
        [TestMethod]
        public void TestAbsoluteToRelative_SameRootFolder()
        {
            var actual = ZipUtils.AbsolutePathToRelative(string.Empty, "0013_Chapter_1325_-_Song_Stupid.xhtml");
            Assert.AreEqual("0013_Chapter_1325_-_Song_Stupid.xhtml", actual);
        }

        [TestMethod]
        public void TestAbsoluteToRelative_SameRootFolder2()
        {
            var actual = ZipUtils.AbsolutePathToRelative(null, "0013_Chapter_1325_-_Song_Stupid.xhtml");
            Assert.AreEqual("0013_Chapter_1325_-_Song_Stupid.xhtml", actual);
        }

        [TestMethod]
        public void TestAbsoluteToRelative_SameLeafFolder()
        {
            var actual = ZipUtils.AbsolutePathToRelative("OEBPS/Text", "OEBPS/Text/0013_Chapter_1325_-_Song_Stupid.xhtml");
            Assert.AreEqual("0013_Chapter_1325_-_Song_Stupid.xhtml", actual);
        }

        [TestMethod]
        public void TestAbsoluteToRelative_SiblingFolder()
        {
            var actual = ZipUtils.AbsolutePathToRelative("OEBPS/Text", "OEBPS/Images/0000_Cover.jpeg");
            Assert.AreEqual("../Images/0000_Cover.jpeg", actual);
        }

        [TestMethod]
        public void TestAbsoluteToRelative_ChildFolder()
        {
            var actual = ZipUtils.AbsolutePathToRelative("OEBPS", "OEBPS/Text/0000_Information.xhtml");
            Assert.AreEqual("Text/0000_Information.xhtml", actual);
        }

        [TestMethod]
        public void TestAbsoluteToRelative_ParentFolder()
        {
            var actual = ZipUtils.AbsolutePathToRelative("OEBPS/Text", "OEBPS/0000_Information.xhtml");
            Assert.AreEqual("../0000_Information.xhtml", actual);
        }

        [TestMethod]
        public void TestRelativePathToAbsolute_SameRootFolder()
        {
            var actual = ZipUtils.RelativePathToAbsolute(string.Empty, "0013_Chapter_1325_-_Song_Stupid.xhtml");
            Assert.AreEqual("0013_Chapter_1325_-_Song_Stupid.xhtml", actual);
        }

        [TestMethod]
        public void TestRelativePathToAbsolute_SameRootFolder2()
        {
            var actual = ZipUtils.RelativePathToAbsolute(null, "0013_Chapter_1325_-_Song_Stupid.xhtml");
            Assert.AreEqual("0013_Chapter_1325_-_Song_Stupid.xhtml", actual);
        }

        [TestMethod]
        public void TestRelativePathToAbsolute_SameLeafFolder()
        {
            var actual = ZipUtils.RelativePathToAbsolute("OEBPS/Text", "0013_Chapter_1325_-_Song_Stupid.xhtml");
            Assert.AreEqual("OEBPS/Text/0013_Chapter_1325_-_Song_Stupid.xhtml", actual);
        }

        [TestMethod]
        public void TestRelativePathToAbsolute_SiblingFolder()
        {
            var actual = ZipUtils.RelativePathToAbsolute("OEBPS/Text", "../Images/0000_Cover.jpeg");
            Assert.AreEqual("OEBPS/Images/0000_Cover.jpeg", actual);
        }

        [TestMethod]
        public void TestRelativePathToAbsolute_ChildFolder()
        {
            var actual = ZipUtils.RelativePathToAbsolute("OEBPS", "Text/0000_Information.xhtml");
            Assert.AreEqual("OEBPS/Text/0000_Information.xhtml", actual);
        }

        [TestMethod]
        public void TestRelativePathToAbsolute_ParentFolder()
        {
            var actual = ZipUtils.RelativePathToAbsolute("OEBPS/Text", "../0000_Information.xhtml");
            Assert.AreEqual("OEBPS/0000_Information.xhtml", actual);
        }

        [TestMethod]
        public void TestToXhtmlWithNbsp()
        {
            var xml = Utils.ReadStringResource("UnitTestMergeWebToEpub.TestData.nbsp.xhtml");
            var bytes = Encoding.UTF8.GetBytes(xml);

            // should not throw
            bytes.ToXhtml();
        }

    }
}
