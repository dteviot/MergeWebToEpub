using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MergeWebToEpub
{
    /// <summary>
    /// Functions for manipulating XHTML
    /// </summary>
    public class Xhtml
    {
        public static XDocument MakeEmptyXhtmlDoc()
        {
            const string EmptyXhtml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n" +
                "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">\r\n" +
                "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n" +
                "<head>\r\n" +
                "    <title></title>\r\n" +
                "</head>\r\n" +
                "<body>\r\n" +
                "</body>\r\n" +
                "</html>\r\n";
            return XDocument.Parse(EmptyXhtml);
        }
    }
}
