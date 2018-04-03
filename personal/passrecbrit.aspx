<%@ Page Language="C#" AutoEventWireup="true" CodeFile="passrecbrit.aspx.cs" Inherits="passrecbrit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Восстановление пароля</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
          <center>
              <br />
              <br />
              <br />
              <br />
              <asp:Label ID="Label1" runat="server" Text="Для восстановления пароля заполните все поля, расположенные ниже: *"></asp:Label>
              <br />
              <br />
              <br />
              <table>
              <tr>
              <td style=" text-align:right">
              <asp:Label ID="Label2" runat="server" Text="Номер читательского билета или номер социальной карты: "></asp:Label>
              </td>
              <td>
              <asp:TextBox ID="TextBox1" runat="server" Width = "250"></asp:TextBox>             
              </td>
              </tr>
              <tr>
              <td style=" text-align:right">
              <asp:Label ID="Label3" runat="server" Text="Фамилия: " ></asp:Label>
              </td>
              <td>
              <asp:TextBox ID="TextBox2" runat="server" Width = "250"></asp:TextBox>
              </td>
              </tr>
              <tr>
              <td style=" text-align:right">
              <asp:Label ID="Label4" runat="server" Text="Имя: "></asp:Label>
              </td>
              <td>
              <asp:TextBox ID="TextBox3" runat="server" Width = "250"></asp:TextBox>
              </td>
              </tr>
              <tr>
              <td style=" text-align:right">
              <asp:Label ID="Label5" runat="server" Text="Отчество: "></asp:Label>
              </td>
              <td>
              <asp:TextBox ID="TextBox4" runat="server" Width = "250"></asp:TextBox>
              </td>
              </tr>
              <tr>
              <td style=" text-align:right">
              <asp:Label ID="Label6" runat="server" Text="Дата рождения: ** "></asp:Label>
              </td>
              <td>
                <asp:TextBox ID="Indate" runat="server"  Width = "250"> </asp:TextBox> 
                <asp:CalendarExtender ID="Indate_CalendarExtender" runat="server"
                    TargetControlID="Indate" Format="dd.MM.yyyy"> </asp:CalendarExtender>
                    <asp:TextBoxWatermarkExtender ID = "wtmrk" runat="server" WatermarkText = "ДД.ММ.ГГГГ" TargetControlID = "Indate" WatermarkCssClass = "watermarked" />
              </td>
              </tr>
              <tr>
              <td style=" text-align:right">
              <asp:Label ID="Label7" runat="server" Text="Желаемый пароль: "></asp:Label>
              </td>
              <td>
              <asp:TextBox ID="TextBox5" runat="server" Width = "250" TextMode ="Password"></asp:TextBox>
              </td>
              </tr>
                  
              <tr>
              <td style=" text-align:right">
              <asp:Label ID="Label8" runat="server" Text="Подтверждение: "></asp:Label>
              </td>
              <td>
              <asp:TextBox ID="TextBox6" runat="server" Width = "250" TextMode ="Password" ></asp:TextBox>
              </td>
              </tr>
              <tr>
              <td id = "tdcol" colspan = 2 align = "center">
                  <asp:Label ID="Label9" runat="server" Text="" ForeColor = "Red"></asp:Label><br />
                  <asp:Button ID="Button1" runat="server" Text="Сменить пароль" 
                      onclick="Button1_Click" />
                  <asp:Button ID="Button2" runat="server" 
                      Text="Вернуться на страницу авторизации" onclick="Button2_Click" />
              </td>
              </tr>
              </table>
              
          </center>
          <br />
          <br />
* - Все поля обязательны                    <br />

** - Чтобы быстрее пролистать нужный год, нажмите два раза на год вверху календаря и затем стрелочками промотайте нужный диапозон. Либо введите дату вручну.         
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    

    </form>
    
<style type="text/css" >
    .watermarked {
    color:gray;
    font-style: italic;
    }
</style>
</body>
</html>
