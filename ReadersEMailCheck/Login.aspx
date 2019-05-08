<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ReadersEMailCheck.Login" %>

    <div>
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

