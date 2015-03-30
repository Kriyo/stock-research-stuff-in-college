using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Net;
using System.Text;

namespace sample
{
    public partial class _Default : System.Web.UI.Page
    {
        // Stock symbols seperated by space or comma.
        protected string m_symbol = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // The page is being loaded and accessed for the first time.
                // Retrieve user input from the form.
                if (Request.QueryString["s"] == null)
                    // Set the default stock symbol to YHOO.
                    m_symbol = "YHOO";
                else
                    // Get the user's input.
                    m_symbol = Request.QueryString["s"].ToString().ToUpper();
                // Update the textbox value.
                txtSymbol.Value = m_symbol;
                // This DIV that contains text and DIVs
                // that displays stock quotes and chart from Yahoo.
                // Set the innerHTML property to replaces the existing content of the DIV.
                divService.InnerHtml = "<br />";
                if (m_symbol.Trim() != "")
                {
                    try
                    {
                        // Return the stock quote data in XML format.
                        String arg = GetQuote(m_symbol.Trim());
                        if (arg == null)
                            return;

                        // Read XML.
                        // Declare an XmlDocument object to represents an XML document.
                        XmlDocument xd = new XmlDocument();
                        // Loads the XML data from a stream.
                        xd.LoadXml(arg);

                        // Read XSLT
                        // Declare an XslCompiledTransform object
                        // to transform XML data using an XSLT style sheet.
                        XslCompiledTransform xslt = new XslCompiledTransform();
                        // Use the Load method to load the Xsl transform object.
                        xslt.Load(Server.MapPath("stock.xsl"));

                        // Transform the XML document into HTML.
                        StringWriter fs = new StringWriter();
                        xslt.Transform(xd.CreateNavigator(), null, fs);
                        string result = fs.ToString();

                        // Replace the characters "&lt;" and "&gt;"
                        // back to "<" and ">".
                        divService.InnerHtml = "<br />" +
                          result.Replace("&lt;", "<").Replace(
                          "&gt;", "<") + "<br />";

                        // Display stock charts.
                        String[] symbols = m_symbol.Replace(",", " ").Split(' ');
                        // Loop through each stock
                        for (int i = 0; i < symbols.Length; ++i)
                        {
                            if (symbols[i].Trim() == "")
                                continue;
                            int index =
                                divService.InnerHtml.ToLower().IndexOf(
                                symbols[i].Trim().ToLower() +
                                " is invalid.");
                            // If index = -1, the stock symbol is valid.
                            if (index == -1)
                            {
                                // Use a random number to defeat cache.
                                Random random = new Random();
                                divService.InnerHtml += "<img id='imgChart_" +
                                    i.ToString() +
                                    "' src='http://ichart.finance.yahoo.com/b?s=" +
                                    symbols[i].Trim().ToUpper() + "& " +
                                    random.Next() + "' border=0><br />";
                                // 1 days
                                divService.InnerHtml +=
                                  "<a style='font-family: Arial, Helvetica, sans-serif; " +
                                  "font-size: 14px; color: Blue;' " +
                                  "href='javascript:changeChart(0," +
                                  i.ToString() + ", \"" + symbols[i].ToLower() +
                                  "\");'><span id='div1d_" + i.ToString() +
                                  "'><b>1d</b></span></a>  ";
                                // 5 days
                                divService.InnerHtml +=
                                  "<a style='font-family: Arial, Helvetica, sans-serif; " +
                                  "font-size: 14px; color: Blue;' " +
                                  "href='javascript:changeChart(1," +
                                  i.ToString() + ", \"" + symbols[i].ToLower() +
                                  "\");'><span id='div5d_" + i.ToString() +
                                  "'>5d</span></a>  ";
                                // 3 months
                                divService.InnerHtml +=
                                  "<a style='font-family: Arial, Helvetica, sans-serif; " +
                                  "font-size: 14px; color: Blue;' " +
                                  "href='javascript:changeChart(2," +
                                  i.ToString() + ", \"" + symbols[i].ToLower() +
                                  "\");'><span id='div3m_" + i.ToString() +
                                  "'>3m</span></a>&  ";
                                // 6 months
                                divService.InnerHtml +=
                                  "<a style='font-family: Arial, Helvetica, sans-serif; " +
                                  "font-size: 14px; color: Blue;' " +
                                  "href='javascript:changeChart(3," +
                                  i.ToString() + ", \"" + symbols[i].ToLower() +
                                  "\");'><span id='div6m_" + i.ToString() +
                                  "'>6m</span></a>  ";
                                // 1 yeas
                                divService.InnerHtml +=
                                  "<a style='font-family: Arial, Helvetica, sans-serif; " +
                                  "font-size: 14px; color: Blue;' " +
                                  "href='javascript:changeChart(4," +
                                  i.ToString() + ", \"" + symbols[i].ToLower() +
                                  "\");'><span id='div1y_" + i.ToString() +
                                  "'>1y</span></a>  ";
                                // 2 years
                                divService.InnerHtml +=
                                  "<a style='font-family: Arial, Helvetica, sans-serif; " +
                                  "font-size: 14px; color: Blue;' " +
                                  "href='javascript:changeChart(5," +
                                  i.ToString() + ", \"" + symbols[i].ToLower() +
                                  "\");'><span id='div2y_" + i.ToString() +
                                  "'>2y</span></a>  ";
                                // 5 years
                                divService.InnerHtml +=
                                  "<a style='font-family: Arial, Helvetica, sans-serif; " +
                                  "font-size: 14px; color: Blue;' " +
                                  "href='javascript:changeChart(6," +
                                  i.ToString() + ", \"" + symbols[i].ToLower() +
                                  "\");'><span id='div5y_" + i.ToString() +
                                  "'>5y</span></a>  ";
                                // Max
                                divService.InnerHtml +=
                                  "<a style='font-family: Arial, Helvetica, sans-serif; " +
                                  "font-size: 14px; color: Blue;' " +
                                  "href='javascript:changeChart(7," +
                                  i.ToString() + ", \"" + symbols[i].ToLower() +
                                  "\");'><span id='divMax_" + i.ToString() +
                                  "'>Max</span></a>" +
                                  "<br><br /><br />  ";
                            }
                        }
                    }
                    catch
                    {
                        // Handle exceptions
                    }
                }
            }
        }

        /// <summary>
        /// This function handles and parses multiple stock symbols as input parameters
        /// and builds a valid XML return document.
        /// </summary>
        /// <param name="symbol">A bunch of stock symbols
        ///    seperated by space or comma</param>
        /// <returns>Return stock quote data in XML format</returns>
        public string GetQuote(string symbol)
        {
            // Set the return string to null.
            string result = null;
            try
            {
                // Use Yahoo finance service to download stock data from Yahoo
                string yahooURL = @"http://download.finance.yahoo.com/d/quotes.csv?s=" +
                                  symbol + "&f=sl1d1t1c1hgvbap2";
                string[] symbols = symbol.Replace(",", " ").Split(' ');

                // Initialize a new WebRequest.
                HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create(yahooURL);
                // Get the response from the Internet resource.
                HttpWebResponse webresp = (HttpWebResponse)webreq.GetResponse();
                // Read the body of the response from the server.
                StreamReader strm =
                  new StreamReader(webresp.GetResponseStream(), Encoding.ASCII);

                // Construct a XML in string format.
                string tmp = "<stockquotes />";
                string content = "";
                for (int i = 0; i < symbols.Length; i++)
                {
                    // Loop through each line from the stream,
                    // building the return XML Document string
                    if (symbols[i].Trim() == "")
                        continue;

                    content = strm.ReadLine().Replace("\"", "");
                    string[] contents = content.ToString().Split(',');
                    // If contents[2] = "N/A". the stock symbol is invalid.
                    if (contents[2] == "N/A")
                    {
                        // Construct XML via strings.
                        tmp += "<Stock>";
                        // "<" and ">" are illegal
                        // in XML elements. Replace the characters "<"
                        // and ">" to "&gt;" and "&lt;".
                        tmp += "<Symbol>&lt;span style='color:red'&gt;" +
                               symbols[i].ToUpper() +
                               " is invalid.&lt;/span&gt;</Symbol>";
                        tmp += "<Last></Last>";
                        tmp += "<Date></Date>";
                        tmp += "<Time></Time>";
                        tmp += "<Change></Change>";
                        tmp += "<High></High>";
                        tmp += "<Low></Low>";
                        tmp += "<Volume></Volume>";
                        tmp += "<Bid></Bid>";
                        tmp += "<Ask></Ask>";
                        tmp += "<Ask></Ask>";
                        tmp += "</Stock>";
                    }
                    else
                    {
                        //construct XML via strings.
                        tmp += "<Stock>";
                        tmp += "<Symbol>" + contents[0] + "</Symbol>";
                        try
                        {
                            tmp += "<Last>" +
                              String.Format("{0:c}", Convert.ToDouble(contents[1])) +
                                            "</Last>";
                        }
                        catch
                        {
                            tmp += "<Last>" + contents[1] + "</Last>";
                        }
                        tmp += "<Date>" + contents[2] + "</Date>";
                        tmp += "<Time>" + contents[3] + "</Time>";
                        // "<" and ">" are illegal in XML elements.
                        // Replace the characters "<" and ">"
                        // to "&gt;" and "&lt;".
                        if (contents[4].Trim().Substring(0, 1) == "-")
                            tmp += "<Change>&lt;span style='color:red'&gt;" +
                                   contents[4] + "(" + contents[10] + ")" +
                                   "&lt;span&gt;</Change>";
                        else if (contents[4].Trim().Substring(0, 1) == "+")
                            tmp += "<Change>&lt;span style='color:green'&gt;" +
                                   contents[4] + "(" + contents[10] + ")" +
                                   "&lt;span&gt;</Change>";
                        else
                            tmp += "<Change>" + contents[4] + "(" +
                                   contents[10] + ")" + "</Change>";
                        tmp += "<High>" + contents[5] + "</High>";
                        tmp += "<Low>" + contents[6] + "</Low>";
                        try
                        {
                            tmp += "<Volume>" + String.Format("{0:0,0}",
                                   Convert.ToInt64(contents[7])) + "</Volume>";
                        }
                        catch
                        {
                            tmp += "<Volume>" + contents[7] + "</Volume>";
                        }
                        tmp += "<Bid>" + contents[8] + "</Bid>";
                        tmp += "<Ask>" + contents[9] + "</Ask>";
                        tmp += "</Stock>";
                    }
                    // Set the return string
                    result += tmp;
                    tmp = "";
                }
                // Set the return string
                result += "</StockQuotes>";
                // Close the StreamReader object.
                strm.Close();
            }
            catch
            {
                // Handle exceptions.
            }
            // Return the stock quote data in XML format.
            return result;
        }
    }
}