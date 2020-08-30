using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace UnitTestMergeWebToEpub
{
    public class XmlCompare
    {
        // records difference in XML
        public class Delta
        {
            public Delta(string s = null)
            {
                Description = s;
            }

            public string Description { get; set; }
            public bool AreSame { get { return string.IsNullOrEmpty(Description); } }
        }

        public static Delta ElementSame(XElement lhs, XElement rhs)
        {
            var delta = ElementSameIgnoringChildren(lhs, rhs);
            if (!delta.AreSame)
            {
                return delta;
            }

            var elements1 = lhs.Descendants().ToList();
            var elements2 = rhs.Descendants().ToList();
            var limit = Math.Min(elements1.Count, elements2.Count);
            for (int i = 0; i < limit; ++i)
            {
                delta = ElementSameIgnoringChildren(elements1[i], elements2[i]);
                if (!delta.AreSame)
                {
                    return delta;
                }
            }

            if (elements1.Count != elements2.Count)
            {
                return new Delta("Different number of elements");
            }

            return new Delta();
        }

        public static Delta ElementSameIgnoringChildren(XElement lhs, XElement rhs)
        {
            var delta = CompareNames(lhs, rhs);
            if (!delta.AreSame)
            {
                return delta;
            }

            delta = CompareAttributes(lhs, rhs);
            if (!delta.AreSame)
            {
                delta.Description = $"Element '{lhs.Name}' : {delta.Description}";
                return delta;
            }

            if (lhs.HasElements != rhs.HasElements)
            {
                return new Delta($"Element '{lhs.Name}' has no children");
            }

            if (!lhs.HasElements && (lhs.Value != rhs.Value))
            {
                return new Delta($"Element '{lhs.Name}' does not match on value '{lhs.Value}' vs '{rhs.Value}'");
            }
            return new Delta();
        }

        public static Delta CompareAttributes(XElement lhs, XElement rhs)
        {
            var delta = CompareNamespaceAttributes(lhs, rhs);
            if (!delta.AreSame)
            {
                return delta;
            }

            var attrs1 = OrderedAttributes(lhs);
            var attrs2 = OrderedAttributes(rhs);
            if (attrs1.Count != attrs2.Count)
            {
                return new Delta($"Different number of attributes");
            }
            for(int i = 0; i < attrs1.Count; ++i)
            {
                var a1 = attrs1[i];
                var a2 = attrs2[i];

                delta = CompareNames(a1.Name, a2.Name, "Attribute");
                if (!delta.AreSame)
                {
                    return delta;
                }
                if (a1.Value != a2.Value)
                {
                    return new Delta($"Attribute {a1.Name} value differs. '{a1.Value ?? "<null>"}' vs '{a2.Value ?? "<null>"}'");
                }
            }
            return new Delta();
        }

        public static Delta CompareNamespaceAttributes(XElement lhs, XElement rhs)
        {
            var attrs1 = OrderedNameSpaceAttributes(lhs);
            var attrs2 = OrderedNameSpaceAttributes(rhs);
            if (attrs1.Count != attrs2.Count)
            {
                return new Delta($"Different number of namespaces");
            }
            for (int i = 0; i < attrs1.Count; ++i)
            {
                var a1 = attrs1[i];
                var a2 = attrs2[i];

                if (a1.Value != a2.Value)
                {
                    return new Delta("Namespaces don't match");
                }

                // namespace attribute looks like
                // xmlns = 'blah'
                // or
                // xmlns:prefix = 'blah'
                // Localname will be either "xmlns" or "prefix"
                // a xmlns will match any xmlns:prefix
                if (a1.Name.LocalName != a2.Name.LocalName)
                {
                    if ((a1.Name.LocalName != "xmlns") && (a2.Name.LocalName != "xmlns"))
                    {
                        return new Delta("Namespaces don't match");
                    }
                }
            }
            return new Delta();
        }

        public static List<XAttribute> OrderedAttributes(XElement element)
        {
            return element.Attributes()
                .Where(a => !a.IsNamespaceDeclaration)
                .OrderBy(a => a.Name.NamespaceName + ">" + a.Name.LocalName)
                .ToList();
        }

        public static List<XAttribute> OrderedNameSpaceAttributes(XElement element)
        {
            return element.Attributes()
                .Where(a => a.IsNamespaceDeclaration)
                .OrderBy(a => a.Value)
                .ToList();
        }

        public static Delta CompareNames(XElement lhs, XElement rhs)
        {
            return CompareNames(lhs.Name, rhs.Name, "Element");
        }

        public static Delta CompareNames(XName lhs, XName rhs, string objectType)
        {
            return (lhs == rhs)
                ? new Delta()
                : new Delta($"{objectType} name '{lhs}' does not match '{rhs}'");
        }
    }
}
