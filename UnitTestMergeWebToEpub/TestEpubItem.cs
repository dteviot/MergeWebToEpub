using System;
using System.Xml.Linq;
using MergeWebToEpub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestEpuItem
    {
        [TestMethod]
        public void Test_PrefixAsInt()
        {
            int actual = EpubItem.PrefixAsInt("OEBPS/Images/cover.html");
            Assert.AreEqual(0, actual);
            actual = EpubItem.PrefixAsInt("OEBPS/Images/0006_010507301155442407.jpg");
            Assert.AreEqual(6, actual);
            actual = EpubItem.PrefixAsInt("OEBPS/Images/0015.jpeg");
            Assert.AreEqual(15, actual);
        }
    }
}
