<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ReadersEMailCheck_2.Login" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Справка по читателю с указанным адресом электронной почты - вход</title>
        <link href="~/Content/style.css" rel="stylesheet" />
    </head>
    <body>
        <div class="enter__wrapper">
            <h1 class="title">Справка по читателю с указанным адресом электронной почты - вход</h1>
            <form runat="server" onserversubmit="Page_Load" method="post" align="middle" class="enter__form">
                <div class="input-wrapper">
                    <label for="name">Имя</label>
                    <input class="input border" name="name" id="name" />
                </div>
                <div class="input-wrapper">
                    <label for="password">Пароль</label>
                    <input class="input border" type="password" name="password" id="password" />
                </div>
                <button class="button border" type="submit">Войти</button>
            </form>
        </div>
    </body>
</html>
