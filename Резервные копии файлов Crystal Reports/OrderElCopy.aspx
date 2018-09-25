<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderElCopy.aspx.cs" Inherits="OrderElCopy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="Panel1" runat="server" Visible = "false">
            <asp:Label ID="Label1" runat="server" Text="Label" Font-Size="Large" ForeColor="Red">
            </asp:Label>
        </asp:Panel>
        <asp:HyperLink ID="HyperLink1" runat="server" Font-Size="Large" NavigateUrl = "https://catalog.libfl.ru" Target = "_self">Вернуться в каталог</asp:HyperLink>
    </div>
    </form>
</body>
</html>
