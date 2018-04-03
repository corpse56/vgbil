<%@ Page Language="C#" AutoEventWireup="true" CodeFile="loginemployee.aspx.cs" Inherits="loginemployee" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Вход в личный кабинет</title>
    <script type="text/javascript" src="jquery.js"></script>
    <script type="text/javascript" src="jquery.keyboardLayout.js"></script>
    <link rel="stylesheet" type="text/css" href="jquery.keyboardLayout.css" />
</head>
<body>

    <form id="form1" runat="server">
            <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl ="~/help.aspx" >Помощь</asp:LinkButton>

        <center>
            <br />
            <table>
            <tr>
            <td>
            <asp:Login ID="Login1" runat="server" ForeColor="Black" LoginButtonText="Войти" 
                PasswordLabelText="Пароль:" TitleText="Авторизация" UserNameLabelText="Номер читательского билета, email или номер социальной карты*:" 
                DestinationPageUrl="~/Default.aspx" DisplayRememberMe="False" 
                OnAuthenticate="Login1_Authenticate" RememberMeText="" Height="110px" 
                PasswordRecoveryText="Забыли пароль?" PasswordRecoveryUrl="~/PassRec.aspx" >
            </asp:Login>     
            </td>
            <td> &nbsp

            </td>
            </tr>
        </table>
            <br  />
            <br  />
            <asp:RadioButton ID="RadioButton1" runat="server"  AutoPostBack ="true"
                Text="Читатель или сотрудник-читатель *"  
                Checked ="true" GroupName = "112" 
                oncheckedchanged="RadioButton1_CheckedChanged" /> <br />
            <!--<asp:RadioButton ID="RadioButton3" runat="server" text="Удалённый читатель **"  AutoPostBack ="true"
                GroupName = "112" oncheckedchanged="RadioButton3_CheckedChanged"/>    <br />-->

            <asp:RadioButton ID="RadioButton2" runat="server" text="Сотрудник **"  AutoPostBack ="true"
                GroupName = "112" oncheckedchanged="RadioButton2_CheckedChanged"/>
        </center>
          
    <!--<div style="vertical-align:middle; margin-top:15%">-->
    <p>
        * - Необходимо указать либо номер читательского билета, 
        либо номер социалной карты (первые 19 цифр), либо email.</p>
    <asp:Panel ID="Panel1" runat="server" Visible ="false" >
    Если Вы не зарегистрированы в библиотеке, то Вам необходимо:
    <br />
    1. <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl ="http://80.250.173.145/WebReaderT/Default.aspx" >Заполнить регистрационную карточку читателя</asp:HyperLink>
    <br />
    2. Приехать в библиотеку имея при себе паспорт и одну фотографию либо социальную карту
    <br />
    3. Сотрудник отдела регистрации зарегистрирует Вас.
    </asp:Panel>
    <br />
    <br />
    <p >
        
        Если Вы хотите стать удалённым читателем, то Вам необходимо пройти процедуру
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl ="http://80.250.173.145/WebRemoteReg/Default.aspx">регистрации</asp:HyperLink>.<br />
        Удалённый читатель - это читатель, который имеет право пользоваться только электронными копиями документов и которому не требуется физическое присутствие для регистрации.
    </p>
    <p >
        ** - Выдача литературы на длительное пользование в отделы
    </p>
    <br />
    <br />
    <br />
    </form>
        <script type="text/javascript">
            $(function() {
                $(':password').keyboardLayout();
            });
    </script>
</body>
</html>
