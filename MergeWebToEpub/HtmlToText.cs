// This class is based on https://raw.githubusercontent.com/ceee/ReadSharp/master/ReadSharp/HtmlUtilities.cs
// Which is available under the MIT license

using HtmlAgilityPack;
using System;
using System.IO;

namespace MergeWebToEpub
{
    internal class HtmlToText
    {
        public static Stream ConvertToPlainText(HtmlDocument htmlDoc)
        {
            var stream = new MemoryStream();
            var sw = new StreamWriter(stream);
            ConvertTo(htmlDoc.DocumentNode.SelectSingleNode("//body"), sw);
            sw.Flush();

            stream.Position = 0;
            return stream;
        }

        private static void ConvertTo(HtmlNode node, StreamWriter outText)
        {
            string html;
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // get text
                    html = ((HtmlTextNode)node).Text;

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0)
                    {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    AddLeadingWhiteSpace(node, outText);
                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    AddTrailingWhiteSpace(node, outText);
                    break;
            }
        }

        private static void AddLeadingWhiteSpace(HtmlNode node, StreamWriter outText)
        {
            switch (node.Name)
            {
                case "a":
                    outText.Write(" ");
                    break;

                case "ul":
                case "ol":
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                case "p": // treat paragraphs as crlf
                case "br":
                    outText.Write("\r\n");
                    break;
            }

        }

        private static void AddTrailingWhiteSpace(HtmlNode node, StreamWriter outText)
        {
            switch (node.Name)
            {
                case "a":
                    outText.Write(" ");
                    break;

                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                case "li":
                    outText.Write("\r\n");
                    break;
            }
        }

        private static void ConvertContentTo(HtmlNode node, StreamWriter outText)
        {
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }
    }
}
