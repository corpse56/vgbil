<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PassRec_ChooseT.aspx.cs" Inherits="PassRec_ChooseT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:500px; font-size:24pt; text-align:center">
    <div style = "vertical-align:middle;display:inline-block">
        <asp:RadioButton ID="RadioButton1" runat="server" GroupName = "112" Text = "Я являюсь читателем библиотеки"/>
        <br />
        <br />
        <asp:RadioButton ID="RadioButton2" runat="server" GroupName = "112" Text = "Я являюсь удалённым читателем библиотеки"/>
        <br />
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="Далее" onclick="Button1_Click" Font-Size = "24pt" />
    </div>
    <div style ="display:inline-block;    vertical-align:middle;    height:100%;    width:0px;">
    </div>
    </div>
    </form>
</body>
</html>
