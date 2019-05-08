<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ReadersEMailCheck.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>WebForm1 - RD Test</title>
    <style type="text/css">
        #Text1 {
            width: 488px;
        }
    </style>
</head>
<body>
    
        <div style="height: 388px">
            <br /><asp:Label ID="Label1" runat="server" BorderStyle="None" Font-Bold="True" Font-Size="Larger" Font-Underline="True" Text="Справка по читателю с указанным адресом электронной почты" style="text-align:center;" Width="100%"></asp:Label>
            <br /><br />
            <asp:Table ID="Table1" runat="server" BorderStyle="Solid" GridLines="Both" Height="117px" HorizontalAlign="Center" Width="692px">
                <asp:TableRow runat="server" Font-Bold="True">
                    <asp:TableCell runat="server">Параметр</asp:TableCell>
                    <asp:TableCell runat="server">Значение</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">Читатель с таким E-Mail существует?</asp:TableCell>
                    <asp:TableCell runat="server" ID="answer01">-</asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Введите адрес e-mail для проверки" style="text-align:center;" Width="100%" Font-Bold="False" Font-Underline="True"></asp:Label>
            <br />
            <br />
            <form id="form1" runat="server" onserversubmit="Page_Load" method="post">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <input id="Text1" type="email"/>
                </div>
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <button type="submit">Проверить</button>
                </div>
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <input id="Signout1" type="button" value="Выход" runat="server" onserverclick="SignOut_Click" />
                </div>
            </form>
        </div>
</body>
</html>
