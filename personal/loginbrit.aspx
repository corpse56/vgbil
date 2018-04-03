<%@ Page Language="C#" AutoEventWireup="true" CodeFile="loginbrit.aspx.cs" Inherits="loginbrit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Вход в личный кабинет</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <center>
            <br />
            <asp:Login ID="Login1" runat="server" ForeColor="Black" LoginButtonText="Войти" 
                PasswordLabelText="Пароль:" TitleText="Авторизация" UserNameLabelText="Имя* :" 
                DestinationPageUrl="~/persaccbrit.aspx" DisplayRememberMe="False" 
                OnAuthenticate="Login1_Authenticate" RememberMeText="" Height="110px" 
                PasswordRecoveryText="Забыли пароль?" PasswordRecoveryUrl="~/PassRecBrit.aspx">
            </asp:Login>
            <br  />
        </center>
    </div> 
    
        </ContentTemplate>
    </asp:UpdatePanel>
        <p>
        * - Читателю в качестве имени необходимо указать либо номер читательского билета, 
        либо номер социалной карты (первые 19 цифр).</p>
    <p >
        ** - Введите пароль, полученный при заполнении электронной регистрационной карты
        &nbsp;</p>

    
        </form>
</body>
</html>
