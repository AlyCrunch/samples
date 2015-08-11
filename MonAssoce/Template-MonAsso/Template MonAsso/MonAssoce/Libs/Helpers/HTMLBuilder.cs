using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace MonAssoce.Libs.Helpers
{
    class HTMLBuilder
    {
        public HTMLBuilder() { }

        public string ContentToHTML(List<string> content)
        {
            ResourceLoader resources = new ResourceLoader();
            StringBuilder builder = new StringBuilder();
            builder.Append("<HTML><BODY>");
            builder.Append("<H2>" + content[0] + "</H2>");
            builder.Append("<H3>" + content[1] + "</H3>");
            if (content[2] != "")
            {
                if (content[2][0] == 'h')
                {
                    builder.Append("<img src=\"" + content[2] + "\"/>");
                }
            }
            builder.Append("<P>" + content[3] + "</P>");
            if (content.Count() > 4)
            {
                builder.Append("<H3>Contact</H3>");
                builder.Append(resources.GetString("Name") + " " + content[4] + "<br/>");
                builder.Append(resources.GetString("Email") + " " + content[5] + "<br/>");
                builder.Append(resources.GetString("Phone") + " " + content[6] + "<br/>");
            }
            if (content.Count() > 7)
            {
                builder.Append(resources.GetString("Add") + " " + content[7] + "<br/>");
                builder.Append(resources.GetString("Website") + " " + content[8]);
            }
            builder.Append("</HTML></BODY>");

            return builder.ToString();
        }
    }
}
