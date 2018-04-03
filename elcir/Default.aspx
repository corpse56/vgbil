<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" tagPrefix="ajax"  %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Электронная копия документа</title>
    <style type = "text/css">
        table
        {
        	border-collapse:collapse;
        	width:100%;
        }
        table, td, th
        {
        	border-style:solid;
        	border-width:2px;
        	border-color:Maroon;
        	
        }
        td
        {
        	width:50%;
        	padding:2px;
        }
        th
        {
        	background-color:GrayText;
        	font-size:x-large;
        }
        .tctr
        {

        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="Panel1" runat="server">
    <div>
    <h1 style="text-align:center">Электронная копия документа</h1>
    <h3>
        <asp:Label ID="lTitle" runat="server" Text="Label"></asp:Label>
    </h3>
    <p/>
    <h3>
        <asp:Label ID="lAuthor" runat="server" Text="Label"></asp:Label>
    </h3>
    <table cellpadding="0" cellspacing = "0" >
        <tr>
            <th >
                Сведение
            </th>
            <th>
                Ссылка
            </th>
        </tr>
        <tr>
            <td>
                Титульный лист
            </td>
            <td>
                Изображение отсутствует
            </td>
        </tr>
        <tr>
            <td>
                Аннотация
            </td>
            <td>
                Изображение отсутствует
            </td>
        </tr>
        <tr>
            <td>
                Содержание
            </td>
            <td>
                Изображение отсутствует
            </td>
        </tr>
        <tr>
            <td>
                Информационный лист
            </td>
            <td>
                Информационный лист отсутствует
            </td>
        </tr>
        <tr>
            <td>
                Электронная копия
            </td>
            <td>
                <asp:Label ID="lEBook" runat="server" Text="Электронная копия отсутствует"></asp:Label>
                <asp:HyperLink ID="hlEBook" runat="server" NavigateUrl ="~/viewer.aspx" Visible = "false">Просмотр</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                Доступ
            </td>
            <td>
                <asp:Label ID="lAccess" runat="server" Text="Защищено авторским правом. Для просмотра документа закажите его через личный кабинет."></asp:Label>
                
            </td>
        </tr>
    </table>
    </div>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Visible = "false">
        <asp:Label ID="lError" runat="server" Text="Неверные входные данные!"></asp:Label>
    </asp:Panel>
    </form>
</body>
</html>
