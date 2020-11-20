using System;
using System.Xml.Linq;
using MergeWebToEpub;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestEpubUtils
    {
        [TestMethod]
        public void ExtractProbableChapterNumber_HasNumber()
        {
            int actual = EpubUtils.ExtractProbableChapterNumber("Chapter 1: Beta Testers, Eternal Kingdom, and Bru");
            Assert.AreEqual(1, actual);
        }
        [TestMethod]
        public void ExtractProbableChapterNumber_NoNumber()
        {
            int actual = "Information".ExtractProbableChapterNumber();
            Assert.AreEqual(-1, actual);
        }

        [TestMethod]
        public void ExtractProbableChapterNumber_ForeignChars()
        {
            int actual = "１２年目の春と子猫".ExtractProbableChapterNumber();
            Assert.AreEqual(12, actual);
        }

    }
}
