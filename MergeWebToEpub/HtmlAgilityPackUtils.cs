using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MergeWebToEpub
{
    public class HtmlAgilityPackUtils
    {
        public static List<string> ValidateXhtmlWitAgilityPack(string xml)
        {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;
            htmlDoc.LoadHtml(xml);
            return htmlDoc.ParseErrors.Select(e => e.Reason).ToList();
        }

        public static string PrettyPrintXhtml(string xhtml)
        {
            // this really only needs to be done once
            // Fixes CDATA being added to <style> elements
            RemoveSetting("style", HtmlElementFlag.CData);

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionWriteEmptyNodes = true;
            doc.OptionOutputAsXml = true;
            doc.OptionOutputOriginalCase = true;
            doc.LoadHtml(xhtml);
            return PrettyPrint(doc);
        }

        public static void AjustHtmlAgilityPackSettings()
        {
            BuildElementsToClose().ForEach(element => MarkTagAsClosed(element));
            RemoveSetting("form", HtmlElementFlag.Empty);
            RemoveSetting("style", HtmlElementFlag.CData);
        }

        public static string PrettyPrint(HtmlDocument doc)
        {
            var sb = new StringBuilder();
            foreach (var node in doc.DocumentNode.ChildNodes)
            {
                PretyPrintNode(sb, node, "");
            }
            return sb.ToString();
        }

        private static void PretyPrintNode(StringBuilder sb, HtmlNode node, string indent)
        {
            if (node.HasChildNodes == false)
            {
                var text = node.OuterHtml.Trim();
                if (node.NodeType == HtmlNodeType.Text)
                {
                    text = HtmlTextToXhtml(text);
                }
                if (!string.IsNullOrEmpty(text))
                {
                    sb.AppendLine(indent + text);
                }
            }
            else if (OnlyChildIsText(node))
            {
                sb.AppendLine(indent + BuildOpenTag(node) + HtmlTextToXhtml(node.ChildNodes[0].OuterHtml) + BuildCloseTag(node));
            }
            else
            {
                sb.AppendLine(indent + BuildOpenTag(node));
                foreach (var chldNode in node.ChildNodes)
                {
                    PretyPrintNode(sb, chldNode, indent + "    ");
                }
                sb.AppendLine(indent + BuildCloseTag(node));
            }
        }

        private static bool OnlyChildIsText(HtmlNode node)
        {
            return (node.ChildNodes.Count == 1)
                && (node.ChildNodes[0].NodeType == HtmlNodeType.Text);
        }

        private static string BuildOpenTag(HtmlNode node)
        {
            var sb = new StringBuilder("<" + node.Name);
            foreach (var attr in node.Attributes)
            {
                sb.Append(string.Format(" {0}=\"{1}\"", attr.OriginalName, attr.Value));
            }
            sb.Append(">");
            return sb.ToString();
        }

        private static string BuildCloseTag(HtmlNode node)
        {
            return $"</{node.Name}>";
        }

        public static string HtmlTextToXhtml(string html)
        {
            if (html.StartsWith("<?"))
            {
                return html;
            }

            var unescaped = WebUtility.HtmlDecode(html);
            var sb = new StringBuilder();
            foreach(var c in unescaped)
            {
                switch(c)
                {
                    case '&':
                        sb.Append("&amp;");
                        break;
                    // These don't need to be escaped in text 
                    //case '\'':
                    //    sb.Append("&apos;");
                    //    break;
                    //case '"':
                    //    sb.Append("&quot;");
                    //    break;
                    //case '>':
                    //    sb.Append("&gt;");
                    //    break;
                    case '<':
                        sb.Append("&lt;");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// The following tag types are not closed with HTML and are with XHTML
        /// </summary>
        private static List<string> BuildElementsToClose()
        {
            var elements = new List<string>();
            elements.Add("img");
            elements.Add("meta");
            elements.Add("input");
            elements.Add("hr");
            return elements;
        }

        /// <summary>
        ///  Tell HTML agility pack to make tag closed
        /// </summary>
        /// <param name="tag"></param>
        private static void MarkTagAsClosed(string tag)
        {
            if (HtmlNode.ElementsFlags.ContainsKey(tag))
            {
                HtmlNode.ElementsFlags[tag] = HtmlNode.ElementsFlags[tag] | HtmlElementFlag.Closed | HtmlElementFlag.Empty;
            }
            else
            {
                HtmlNode.ElementsFlags.Add(tag, HtmlElementFlag.Closed | HtmlElementFlag.Empty);
            }
        }

        private static void RemoveSetting(string tag, HtmlElementFlag flag)
        {
            if (HtmlNode.ElementsFlags.ContainsKey(tag))
            {
                HtmlNode.ElementsFlags[tag] = HtmlNode.ElementsFlags[tag] & ~flag;
            }
        }
    }
}


