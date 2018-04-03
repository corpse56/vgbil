<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogIn.aspx.cs" Inherits="LogIn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="jquery.js"></script>
    <script type="text/javascript" src="jquery.keyboardLayout.js"></script>
    <link rel="stylesheet" type="text/css" href="jquery.keyboardLayout.css" />

</head>
<body>
    <form id="form1" runat="server">
    <div><center>
     <asp:Login ID="Login1" runat="server" DestinationPageUrl="~/statistic.aspx" 
        DisplayRememberMe="False" Font-Size="X-Large" ForeColor="Black" Height="110px" 
        LoginButtonText="Войти" OnAuthenticate="Login1_Authenticate" 
        PasswordLabelText="Пароль:" RememberMeText="" TitleText="Авторизация" 
        UserNameLabelText="Логин :" 
        UserNameRequiredErrorMessage="Поле &quot;Логин&quot; является обязательным." 
        Width="500px">
    </asp:Login>
    </center>
    </div>
   
    </form>
    <script type="text/javascript">
        $(function() {
            $(':password').keyboardLayout();
        });
    </script>
</body>
</html>
