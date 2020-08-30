using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestMergeWebToEpub
{
    [TestClass]
    public class TestXmlCompare
    {
        [TestMethod]
        public void CompareNames_Same_ShouldSucceed()
        {
            XElement root1 = XElement.Parse(@"<Root />");
            XElement root2 = XElement.Parse(@"<Root />");
            Assert.IsTrue(XmlCompare.CompareNames(root1, root2).AreSame);
        }

        [TestMethod]
        public void CompareNames_Different_ShouldFail()
        {
            XElement root1 = XElement.Parse(@"<Root />");
            XElement root2 = XElement.Parse(@"<Root2 />");
            Assert.IsFalse(XmlCompare.CompareNames(root1, root2).AreSame);
        }

        [TestMethod]
        public void CompareNames_SameNameSpace_ShouldSucceed()
        {
            XElement root1 = XElement.Parse(@"<Root xmlns='http://www.northwind.com'> <Child>1</Child></Root>");
            XElement root2 = XElement.Parse(@"<n:Root xmlns:n='http://www.northwind.com'><n:Child>1</n:Child></n:Root>");
            Assert.IsTrue(XmlCompare.CompareNames(root1, root2).AreSame);
        }

        [TestMethod]
        public void CompareNames_DifferentNameSpace_ShouldFail()
        {
            XElement root1 = XElement.Parse(@"<Root xmlns='http://www.northwind.com'> <Child>1</Child></Root>");
            XElement root2 = XElement.Parse(@"<n:Root xmlns:n='http://www.northwind2.com'><n:Child>1</n:Child></n:Root>");
            Assert.IsFalse(XmlCompare.CompareNames(root1, root2).AreSame);
        }

        [TestMethod]
        public void CompareAttributes_SameWithPrefix_ShouldSucceed()
        {
            XElement root1 = XElement.Parse(@"<Root a='1' b='2' xmlns='http://www.northwind.com'> <Child>1</Child></Root>");
            XElement root2 = XElement.Parse(@"<n:Root b='2' a='1' xmlns:n='http://www.northwind.com'><n:Child>1</n:Child></n:Root>");
            Assert.IsTrue(XmlCompare.CompareAttributes(root1, root2).AreSame);
        }

        [TestMethod]
        public void CompareAttributes_Same_ShouldSucceed()
        {
            XElement root1 = XElement.Parse(@"<Root a='1' b='2' xmlns='http://www.northwind.com'> <Child>1</Child></Root>");
            XElement root2 = XElement.Parse(@"<n:Root b='2' a='1' xmlns:n='http://www.northwind.com'><n:Child>1</n:Child></n:Root>");
            Assert.IsTrue(XmlCompare.CompareAttributes(root1, root2).AreSame);
        }

        [TestMethod]
        public void CompareAttributes_DifferentValue_ShouldFail()
        {
            XElement root1 = XElement.Parse(@"<Root a='1' b='2' xmlns='http://www.northwind.com'> <Child>1</Child></Root>");
            XElement root2 = XElement.Parse(@"<n:Root b='3' a='1' xmlns:n='http://www.northwind.com'><n:Child>1</n:Child></n:Root>");
            Assert.IsFalse(XmlCompare.CompareAttributes(root1, root2).AreSame);
        }

        [TestMethod]
        public void CompareAttributes_Missing_ShouldFail()
        {
            XElement root1 = XElement.Parse(@"<Root a='1' b='2' xmlns='http://www.northwind.com'> <Child>1</Child></Root>");
            XElement root2 = XElement.Parse(@"<n:Root xmlns:n='http://www.northwind.com'><n:Child>1</n:Child></n:Root>");
            Assert.IsFalse(XmlCompare.CompareAttributes(root1, root2).AreSame);
        }

        [TestMethod]
        public void ElementSameIgnoringChildren_Same_ShouldSucceed()
        {
            XElement root1 = XElement.Parse(@"<xmlns:Root a='1' b='2' xmlns='http://www.northwind.com'> <Child>1</Child></xmlns:Root>");
            XElement root2 = XElement.Parse(@"<xmlns:Root b='2' a='1' xmlns='http://www.northwind.com'><xmlns:Child>1</xmlns:Child></xmlns:Root>");
            Assert.IsTrue(XmlCompare.ElementSameIgnoringChildren(root1, root2).AreSame);
        }

        [TestMethod]
        public void ElementSameIgnoringChildren_MissingChildren_ShouldFail()
        {
            XElement root1 = XElement.Parse(@"<xmlns:Root a='1' b='2' xmlns='http://www.northwind.com' />");
            XElement root2 = XElement.Parse(@"<xmlns:Root b='2' a='1' xmlns='http://www.northwind.com'><xmlns:Child>1</xmlns:Child></xmlns:Root>");
            Assert.IsFalse(XmlCompare.ElementSameIgnoringChildren(root1, root2).AreSame);
        }

        [TestMethod]
        public void ElementSameIgnoringChildren_ValueNoMatch_ShouldFail()
        {
            XElement root1 = XElement.Parse(@"<xmlns:Root a='1' b='2' xmlns='http://www.northwind.com'>1</xmlns:Root>");
            XElement root2 = XElement.Parse(@"<xmlns:Root b='2' a='1' xmlns='http://www.northwind.com' />");
            Assert.IsFalse(XmlCompare.ElementSameIgnoringChildren(root1, root2).AreSame);
        }

        [TestMethod]
        public void ElementSameIgnoringChildren_ValuesMatch_ShouldSucceed()
        {
            XElement root1 = XElement.Parse(@"<Root a='1' b='2' xmlns='http://www.northwind.com'>2</Root>");
            XElement root2 = XElement.Parse(@"<n:Root b='2' a='1' xmlns:n='http://www.northwind.com'>2</n:Root>");
            Assert.IsTrue(XmlCompare.ElementSameIgnoringChildren(root1, root2).AreSame);
        }

        [TestMethod]
        public void ElementSame_Same_ShouldSucceed()
        {
            XElement root1 = XElement.Parse(@"<Root a='1' b='2' xmlns='http://www.northwind.com'> <Child>1</Child></Root>");
            XElement root2 = XElement.Parse(@"<n:Root b='2' a='1' xmlns:n='http://www.northwind.com'><n:Child>1</n:Child></n:Root>");
            Assert.IsTrue(XmlCompare.ElementSame(root1, root2).AreSame);
        }

        [TestMethod]
        public void ElementSame_DifferentChildren_ShouldFail()
        {
            XElement root1 = XElement.Parse(@"<Root a='1' b='2' xmlns='http://www.northwind.com'> <Child>1</Child></Root>");
            XElement root2 = XElement.Parse(@"<nw:Root b='2' a='1' xmlns:nw='http://www.northwind.com'><nw:Child>2</nw:Child></nw:Root>");
            Assert.IsFalse(XmlCompare.ElementSame(root1, root2).AreSame);
        }

        [TestMethod]
        public void ElementSame_DifferentCountChildren_ShouldFail()
        {
            XElement root1 = XElement.Parse(@"<Root a='1' b='2' xmlns='http://www.northwind.com'> <Child>1</Child></Root>");
            XElement root2 = XElement.Parse(@"<n:Root b='2' a='1' xmlns:n='http://www.northwind.com'><n:Child>1</n:Child><n:Child /></n:Root>");
            Assert.IsFalse(XmlCompare.ElementSame(root1, root2).AreSame);
        }
    }
}
