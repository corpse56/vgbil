<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ReadersEMailCheck.Login" %>

    <div>
        <br /><asp:Label ID="Label1" runat="server" BorderStyle="None" Font-Bold="True" Font-Size="Larger" Font-Underline="True" Text="Справка по читателю с указанным адресом электронной почты - вход" style="text-align:center;" Width="100%"></asp:Label>
        <form runat="server" onserversubmit="Page_Load" method="post">
        <div>
            <label for="name">Имя:</label>
            <input name="name" />
        </div>
        <div>
            <label for="password">Пароль:</label>
            <input type="password" name="password" />
        </div>
        <button type="submit">Войти</button>
        </form>
    </div>

