<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectRole.aspx.cs" Inherits="SelectRole" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Выберите роль"></asp:Label>
            <br />
            <br />
            <asp:DropDownList ID="ddlRoles" runat="server"></asp:DropDownList>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" Text="Продолжить" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>
