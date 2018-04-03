<%@ Page Language="C#" AutoEventWireup="true" CodeFile="viewer.aspx.cs" Inherits="viewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" tagPrefix="ajax"  %>
<html xmlns="http://www.w3.org/1999/xhtml">


<head>
<script type="text/javascript" src =  "js/jquery-1.10.2.js" charset = "UTF-8"> </script>
<script type="text/javascript" src = "js/SSEscript.js"  charset = "windows-1251"> </script>
<script type="text/javascript" src = "js/jQueryRotate.js"  charset = "windows-1251"> </script>
<style type="text/css">
    .mybutton
    {
    border:solid 1px #c0c0c0;
    background-color:#0000AA;
    color:#ffffff;
    cursor:pointer; 
    font-weight:bold; 
    }
    .myctr
    {
	    text-align:left;
    }
    .myrightintd
    {
        text-align:right;
    }
    .mymodalBackground
    {
        background-color: Black;
    }
    .mymodalPopup
    {
        background-color: #FFFFFF;
        border-width: 3px;
        border-style: solid;
        border-color: black;
        padding-top: 10px;
        padding-left: 10px;
        width: 500px;
        height: 400px;
    }
    #imgslides {
    transform-origin: top left; /* IE 10+, Firefox, etc. */
    -webkit-transform-origin: top left; /* Chrome */
    -ms-transform-origin: top left; /* IE 9 */
}
#imgslides.rotate90 {
    transform: rotate(90deg) translateY(-100%);
    -webkit-transform: rotate(90deg) translateY(-100%);
    -ms-transform: rotate(90deg) translateY(-100%);
}
#imgslides.rotate180 {
    transform: rotate(180deg) translate(-100%,-100%);
    -webkit-transform: rotate(180deg) translate(-100%,-100%);
    -ms-transform: rotate(180deg) translateX(-100%,-100%);
}
#imgslides.rotate270 {
    transform: rotate(270deg) translateX(-100%);
    -webkit-transform: rotate(270deg) translateX(-100%);
    -ms-transform: rotate(270deg) translateX(-100%);
}
</style>

<meta charset="UTF-8"/>
<title>Просмотр электронного документа</title>


</head>


<!--<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js" charset = "UTF-8"></script>-->

<body>
<div id = "my_main_page" data-role="page" ></div>
<form id="form1" runat="server" >

<ajax:ToolkitScriptManager ID="scriptmanager1" runat="server" >
</ajax:ToolkitScriptManager>
<input type="hidden" value="" id="SendA" name="SendA" />


<asp:Panel ID="Panel1" runat="server">
<div>
<!--<asp:HyperLink ID="hlEBook" runat="server" NavigateUrl ="~/Default.aspx" Target="_blank" Visible = "true" >Описание документа</asp:HyperLink>-->
  <!--  <asp:Label ID="Label3" runat="server" Text="Заглавие: "></asp:Label>-->

<table id = "Ctrl" style="border:Solid 3px #D55500; width:100%; height:100%" cellpadding="0" cellspacing="0">
<tr style="border:Solid 3px #D55500;">
<td class ="myctr">
    <asp:Button ID="btnPrevious" runat="server" Text="<<<" CssClass="mybutton" />
    <asp:Button ID="btnNext" runat="server" Text=">>>" CssClass="mybutton" />
    <asp:Label ID="lblPage" runat="server" Text="Страница 1 из 1" CssClass ="myrightintd"></asp:Label>
</td>
<td>
<asp:Label ID="Label4" runat="server" ></asp:Label>

</td>
<td class ="myrightintd">
    <input id="btnRot" type= "button" value="Повернуть"  onclick = "Rotat()" class="mybutton" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
    
        
    <input id="btnScalePlus" type="button" value="Увеличить"    onclick = "IncSize()" class="mybutton"/>
    <input id="btnScaleMinus" type="button" value="Уменьшить"    onclick = "DecSize()" class="mybutton"/>
    <input id="btn100" type="button" value="100%"    onclick = "Size100()" class="mybutton"/>
</td>
</tr>
<tr>
<td class ="myctr">
    <asp:Label ID="Label1" runat="server" Text="На страницу : "></asp:Label>
    <asp:TextBox  ID="txtPage" runat="server" Text = "1" Width = "40px">   </asp:TextBox>
    <input id="btnGoto" type= "button"  value="ОК"  onclick = "gotoPage()" class="mybutton"/>
</td>
<td class ="myctr">
    <asp:Label ID="Label2" runat="server" Text="Выберите номер года : "></asp:Label>
    <asp:DropDownList ID="DropDownList1"  runat="server" AutoPostBack="True">
    </asp:DropDownList>
</td>
<td>
    
</td>
</tr>
</table>


<table id = "targetTable" style="border:Solid 3px #000000; width:100%; height:100%" cellpadding="0" cellspacing="0">
<tr style="background-color:#D55500">
<td style=" height:10%; color:White; font-weight:bold; font-size:larger" align="center">
<asp:Label ID="lblTitle" runat="server" ></asp:Label>

</td>
</tr>
<tr>
<td id = "ims">
    <asp:Image ID="imgslides" runat="server" Width="100%" Height="100%" />
</td>
</tr>
<tr>
<td align="center">
<asp:Label ID="lblimgdesc" runat="server"></asp:Label>
</td>
</tr>

</table>

<ajax:SlideShowExtender ID="SlideShowExtender1" runat="server" AutoPlay="false" ImageTitleLabelID="lblTitle" 
ImageDescriptionLabelID="lblimgdesc" Loop="true"
NextButtonID="btnNext" PreviousButtonID="btnPrevious"  EnableViewState = "true"  
PlayButtonText="Play" StopButtonText="Stop" UseContextKey ="true"
TargetControlID="imgslides"  SlideShowServicePath="Slideshow.asmx" SlideShowServiceMethod="GetSlides" 
ContextKey = "2" >
</ajax:SlideShowExtender>
</div>

</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Visible = "false">
        <asp:Label ID="lError" runat="server" Text="Неверные входные данные!"></asp:Label>
</asp:Panel>

</form>

</body>

</html>