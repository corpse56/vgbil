<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src =  "js/jquery-1.11.1.min.js" charset = "UTF-8"> </script>
    <script type="text/javascript" src =  "js/jquery-1.11.1.min.js" charset = "UTF-8"> </script>
    <script type="text/javascript" src = "js/jQueryRotate.js"  charset = "windows-1251"> </script>

<script type="text/javascript">
    //$(document).ready(function() {
        //$.ajax({});
    //});
    function rot() {
        var angle = 0;
        $('#btnRot').on('click', function() {
            angle += 90;
            $("#Image1").rotate(angle);
        });
    }
</script>
</head>
<body>
 <form id="form1" runat="server">
 <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
 <input id="btnRot" type="button" value="javasc"    onclick = "Rotat()" class="mybutton"/>
<asp:image ID="Image1" runat="server" ImageUrl="~/pts1.JPG" />
 
 </form>
</body>

</html>
