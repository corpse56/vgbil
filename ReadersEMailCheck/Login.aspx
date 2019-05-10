<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ReadersEMailCheck_2.Login" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Справка по читателю с указанным адресом электронной почты - вход</title>
    </head>
    <body>
        <div style="width:100%;">
            <br /><asp:Label ID="Label1" runat="server" BorderStyle="None" Font-Bold="True" Font-Size="Larger" Font-Underline="True" Text="Справка по читателю с указанным адресом электронной почты - вход" style="text-align:center;" Width="100%"></asp:Label>
            <br />
            <br />
            <form runat="server" onserversubmit="Page_Load" method="post" align="middle">
                <div style="width:220px; margin: 0 auto;">
                    <label for="name">Имя:</label>
                    <input style="width:220px;" name="name" />
                </div>
                <div style="width:220px; margin: 0 auto;">
                    <label for="password">Пароль:</label>
                    <input style="width:220px;" type="password" name="password" />
                </div>
                <br />
                <button style="width:220px;" type="submit">Войти</button>
            </form>
        </div>
    </body>
</html>
