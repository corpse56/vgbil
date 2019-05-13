<%@ Page Trace="false" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ReadersEMailCheck_2.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <title>Справка по читателю с указанным адресом электронной почты</title>
        <link href="~/Content/style.css" rel="stylesheet" />
    </head>
    <body>
        <div class="enter__wrapper">
            <h1 class="title">Справка по читателю с указанным адресом электронной почты</h1>
            <table id="Table1" align="Center" rules="all" border="1" class="reader-table border"><tbody>
                <tr>
                    <td class="table-element">
                        <span>e-mail:</span>
                        <br />
                        <span runat="server" id="question01">...</span>
                    </td>
                    <td runat="server" id="answer01" class="table-element">...</td>
                </tr>
            </tbody></table>
            <form runat="server" onserversubmit="Page_Load" method="post" class="info-form">
                <label>Введите адрес e-mail для проверки:
                    <input id="Text1" type="email" runat="server" required="required" class="input border" />
                </label>
                <button type="submit" class="button border">Проверить</button>
                <input id="Signout1" type="button" value="Выход" runat="server" onserverclick="SignOut_Click" class="button border"/>
            </form>
        </div>
    </body>
</html>
