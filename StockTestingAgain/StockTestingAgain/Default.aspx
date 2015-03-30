<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="StockTestingAgain._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
      "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title>Stock quote and chart from Yahoo in C#</title>
<script type="text/javascript" language="JavaScript">
    /// <summary>
    /// This function will be called when user clicks the Get Quotes button.
    /// </summary>
    /// <returns>Always return false.</returns>
    function SendRequest() {
        var txtSymbol = document.getElementById("txtSymbol");
        // Refresh the page.
        window.location = "default.aspx?s=" + txtSymbol.value;
        return false;
    }

    /// <summary>
    /// The functyion will be called when a keyboard key is pressed in the textbox.
    /// </summary>
    /// <param name="e">Onkeypress event.</param>
    /// <returns>Return true if user presses Enter key; otherwise false.</returns>
    function CheckEnter(e) {
        if ((e.keyCode && e.keyCode == 13) || (e.which && e.which == 13))
            // Enter is pressed in the textbox.
            return SendRequest();
        return true;
    }

    /// <summary>
    /// The function will be called when user
    // changes the chart type to another type.
    /// </summary>
    /// <param name="type">Chart type.</param>
    /// <param name="num">Stock number.</param>
    /// <param name="symbol">Stock symobl.</param>     
    function changeChart(type, num, symbol) {
        // All the DIVs are inside the main DIV
        // and defined in the code-behind class.
        var div1d = document.getElementById("div1d_" + num);
        var div5d = document.getElementById("div5d_" + num);
        var div3m = document.getElementById("div3m_" + num);
        var div6m = document.getElementById("div6m_" + num);
        var div1y = document.getElementById("div1y_" + num);
        var div2y = document.getElementById("div2y_" + num);
        var div5y = document.getElementById("div5y_" + num);
        var divMax = document.getElementById("divMax_" + num);
        var divChart = document.getElementById("imgChart_" + num);
        // Set innerHTML property.
        div1d.innerHTML = "1d";
        div5d.innerHTML = "5d";
        div3m.innerHTML = "3m";
        div6m.innerHTML = "6m";
        div1y.innerHTML = "1y";
        div2y.innerHTML = "2y";
        div5y.innerHTML = "5y";
        divMax.innerHTML = "Max";
        // Use a random number to defeat cache.
        var rand_no = Math.random();
        rand_no = rand_no * 100000000;
        //  Display the stock chart.
        switch (type) {
            case 1: // 5 days
                div5d.innerHTML = "<b>5d</b>";
                divChart.src = "http://ichart.finance.yahoo.com/w?s=" +
                               symbol + "&" + rand_no;
                break;
            case 2: // 3 months
                div3m.innerHTML = "<b>3m</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/3m/" +
                               symbol + "?" + rand_no;
                break;
            case 3: // 6 months 
                div6m.innerHTML = "<b>6m</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/6m/" +
                               symbol + "?" + rand_no;
                break;
            case 4: // 1 year
                div1y.innerHTML = "<b>1y</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/1y/" +
                               symbol + "?" + rand_no;
                break;
            case 5: // 2 years
                div2y.innerHTML = "<b>2y</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/2y/" +
                               symbol + "?" + rand_no;
                break;
            case 6: // 5 years
                div5y.innerHTML = "<b>5y</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/5y/" +
                               symbol + "?" + rand_no;
                break;
            case 7: // Max
                divMax.innerHTML = "<b>msx</b>";
                divChart.src = "http://chart.finance.yahoo.com/c/my/" +
                               symbol + "?" + rand_no;
                break;
            case 0: // 1 day
            default:
                div1d.innerHTML = "<b>1d</b>";
                divChart.src = "http://ichart.finance.yahoo.com/b?s=" +
                               symbol + "&" + rand_no;
                break;
        }
    }
</script>    

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" border="0" 
                 cellspacing="0" cellpadding="0">
            <tr valign="top">                                            
                <td style="font-family: Arial, Helvetica, sans-serif; 
                           font-size: 14px; color: #000; text-decoration: none;">
                    <input type="text" value="" id="txtSymbol" 
                        runat="server" onkeypress="return CheckEnter(event);" />
                    <input type="button" value="Get Quotes" 
                        onclick="return SendRequest();" />
                    <br />
                    <span style="font-family: Arial, Helvetica, sans-serif; 
                                 font-size: 11px; color: #666;">
                        e.g. "YHOO or YHOO GOOG"
                    </span>
                    <%if (m_symbol != "") {%>                        
                        <div id="divService" runat="server">
                        <!-- Main DIV: this DIV contains text and DIVs 
                                       that displays stock quotes and chart. -->
                        </div>
                    <%}%>
                </td>    
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
