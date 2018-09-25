<%@ Page Language="C#" AutoEventWireup="true" CodeFile="viewer.aspx.cs" Inherits="viewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" tagPrefix="ajax"  %>
<html xmlns="http://www.w3.org/1999/xhtml">


<head runat="server">
<meta charset="UTF-8"/>

  <!--<script type="text/javascript" src="js/pace.min.js"></script>
  <link href="css/center-radar.css" rel="stylesheet" />-->
<script type="text/javascript" src =  "jquery-1.10.2.js" charset = "UTF-8"> </script>
<script type="text/javascript" src = "SSEscript.js"  charset = "windows-1251"> </script>
<script type="text/javascript" src = "jQueryRotate.js"  charset = "windows-1251"> </script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>

    
<script type="text/javascript">
    /*paceOptions = {
        // Configuration goes here. Example:
        elements: false,
        restartOnPushState: false,
        restartOnRequestAfter: false
    }*/
    
    
    //$('form').live("onload", function() {
    //    ShowProgress();
    //});
</script>
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
    .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        /*display: none;*/
        position: fixed;
        background-color: White;
        z-index: 999;
    }
</style>

<title>Просмотр электронного документа</title>


</head>


<!--<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js" charset = "UTF-8"></script>-->

<body>
<div id = "my_main_page" data-role="page" ></div>
<form id="form1" runat="server" >

<ajax:ToolkitScriptManager ID="scriptmanager1" runat="server" >
</ajax:ToolkitScriptManager>


<input type="hidden" value="" id="SendA" name="SendA" />
<asp:Button ID="bShowMP" runat="server" Text="Show" Visible = "false"  />
<asp:Panel ID="Panel3" runat="server" CssClass="mymodalPopup" align = "left" style = "display:compact" Visible = "false">
    <asp:CheckBox ID="CheckBox1" runat="server" Text="С соглашениями согласен и обязуюсь выполнять" 
        oncheckedchanged="CheckBox1_CheckedChanged" AutoPostBack ="true" />
    <asp:Button ID="bAgree" runat="server" Text="Продолжить" 
        onclick="bAgree_Click" Enabled= "false"  />

</asp:Panel>


<asp:Panel ID="Panel1" runat="server">
<div>
<asp:HyperLink ID="hlEBook" runat="server" NavigateUrl ="~/Default.aspx" Target="_blank" Visible = "true" >Описание документа</asp:HyperLink>

<table id = "Ctrl" style="border:Solid 3px #D55500; width:100%; height:100%" cellpadding="0" cellspacing="0">
<tr style="border:Solid 3px #D55500;">
<td class ="myctr">
    <asp:Button ID="btnPrevious" runat="server" Text="<<<" CssClass="mybutton" />
    <asp:Button ID="btnNext" runat="server" Text=">>>" CssClass="mybutton" />
    <asp:Label ID="lblPage" runat="server" Text="Страница 1 из 1" CssClass ="myrightintd"></asp:Label>
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
ContextKey = "2"   >
</ajax:SlideShowExtender>

</div>

</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Visible = "false">
        <asp:Label ID="lError" runat="server" Text=""></asp:Label>
</asp:Panel>
    <div class="loading" align="center" runat="server" id="progress">
    Загрузка изображений<br />
    <br />
    <img src="img/ajax-loader.gif" alt="Загрузка" />
</div>
</form>

</body>

</html>