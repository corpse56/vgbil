<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Авторизация</title>
    <script type="text/javascript" src="jquery.js"></script>
    <script type="text/javascript" src="jquery.keyboardLayout.js"></script>
    <link rel="stylesheet" type="text/css" href="jquery.keyboardLayout.css" />
</head>
<body>
    <form id="form1" runat="server">
        <center>
        <!--<asp:Label ID="Label8" runat="server" Text="Label" Font-Size="X-Large">По техническим причинам очередь временно закрыта</asp:Label></center>-->
     <center>

    <div>
    <center>
        <asp:Label ID="Label1" runat="server" Text="Label" Font-Size="X-Large"></asp:Label></center>
     <center>
            <br />
            <table>
            <tr>
            <td>
            <asp:Login ID="Login1" runat="server" ForeColor="Black" LoginButtonText="Войти" 
                PasswordLabelText="Пароль:" TitleText="Авторизация" UserNameLabelText="Номер читательского билета, Email или номер социальной карты:" 
                DestinationPageUrl="~/Default.aspx" DisplayRememberMe="False" 
                OnAuthenticate="Login1_Authenticate" RememberMeText="" Height="110px"  
                    Font-Size="Larger" Width="896px" >
            </asp:Login>     
            </td>
            <td> &nbsp

            </td>
            </tr>
        </table>
            <br  />
            <br  />
           <!-- <asp:RadioButton ID="RadioButton1" runat="server"  AutoPostBack ="true" Font-Size="Larger"
                Text="Читатель"  
                Checked ="true" GroupName = "112" 
                oncheckedchanged="RadioButton1_CheckedChanged" /> <br />
            <asp:RadioButton ID="RadioButton3" runat="server" text="Удалённый читатель"  AutoPostBack ="true" Font-Size="Larger"
                GroupName = "112" oncheckedchanged="RadioButton3_CheckedChanged"/>    <br />
-->
        </center>
        <br />
        <br />
        <br />
                <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl ="~/ViewTurn.aspx">Очередь изданий на оцифровку</asp:LinkButton>

        <asp:HiddenField ID="hfPIN" runat="server" />
        <asp:HiddenField ID="hfIDR" runat="server" />
        <asp:HiddenField ID="hfBAZA" runat="server" />
        <asp:HiddenField ID="hfIsRemote" runat="server" />
    </div>
    </form>
     <script type="text/javascript">
            $(function() {
                $(':password').keyboardLayout();
            });
    </script>
</body>
</html>
