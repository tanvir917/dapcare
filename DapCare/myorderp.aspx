<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="myorderp.aspx.cs" Inherits="DapCare.pdf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    
<body>
    
    <form id="form1" runat="server">
        <div>
            <h2></h2>
            <h5></h5>
           
            <asp:Panel ID="panelPdf" runat ="server">
                <asp:GridView ID="gridViewData" runat="server">

                </asp:GridView>
            </asp:Panel>
            <asp:Button  runat="server" ID="btnPrint" Text="Print" OnCLick="btnPrint_Click"/>
        </div>
    </form>
</body>
</html>
