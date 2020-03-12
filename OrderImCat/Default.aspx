<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="scripts/jquery-3.4.1.js"></script>
    <script type="text/javascript" src="scripts/jquery.cookie.js"></script>
    
    <script type="text/javascript">

    function LoadDiv(url)
    {
        var img = new Image();
        var bcgDiv = document.getElementById("divBackground");
        var imgDiv = document.getElementById("divImage");
        var imgFull = document.getElementById("imgFull");
        var imgLoader = document.getElementById("imgLoader");
        imgLoader.style.display = "block";
        img.onload = function ()
        {
            imgFull.src = img.src;
            imgFull.style.display = "block";
            imgLoader.style.display = "none";
            imgFull.style.width = "600px";
        };
        img.src = url;
        var width = document.body.clientWidth;
        if (document.body.clientHeight > document.body.scrollHeight)
        {
            bcgDiv.style.height = document.body.clientHeight + "px";
            //alert(document.body.clientHeight);
        }
        else
        {
            bcgDiv.style.height = document.body.scrollHeight + "px";
            
        }
        

        imgDiv.style.left = "100px";//(width - 650) / 2 + "px";
        imgDiv.style.top = "20px";
        bcgDiv.style.width = "100%";
        imgDiv.style.width = "650px";

        bcgDiv.style.display = "none";
        imgDiv.style.display = "block";
        //disableScroll();
        return false;
    }

        function HideDiv() {
            var bcgDiv = document.getElementById("divBackground");
            var imgDiv = document.getElementById("divImage");
            var imgFull = document.getElementById("imgFull");
            if (bcgDiv != null) {
                bcgDiv.style.display = "none";
                imgDiv.style.display = "none";
                imgFull.style.display = "none";
                //enableScroll();
            }
        }
        function setCookie(cname, cvalue, exdays)
        {
          var d = new Date();
          d.setTime(d.getTime() + (exdays*24*60*60*1000));
          var expires = "expires="+ d.toUTCString();
          document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
        }
    // left: 37, up: 38, right: 39, down: 40,
// spacebar: 32, pageup: 33, pagedown: 34, end: 35, home: 36
//var keys = {37: 1, 38: 1, 39: 1, 40: 1};

//function preventDefault(e) {
//  e = e || window.event;
//  if (e.preventDefault)
//      e.preventDefault();
//  e.returnValue = false;  
//}

//function preventDefaultForScrollKeys(e) {
//    if (keys[e.keyCode]) {
//        preventDefault(e);
//        return false;
//    }
//}

//function disableScroll() {
//  if (window.addEventListener) // older FF
//      window.addEventListener('DOMMouseScroll', preventDefault, false);
//  document.addEventListener('wheel', preventDefault, {passive: false}); // Disable scrolling in Chrome
//  window.onwheel = preventDefault; // modern standard
//  window.onmousewheel = document.onmousewheel = preventDefault; // older browsers, IE
//  window.ontouchmove  = preventDefault; // mobile
//  document.onkeydown  = preventDefaultForScrollKeys;
//}

//function enableScroll() {
//    if (window.removeEventListener)
//        window.removeEventListener('DOMMouseScroll', preventDefault, false);
//    document.removeEventListener('wheel', preventDefault, {passive: false}); // Enable scrolling in Chrome
//    window.onmousewheel = document.onmousewheel = null; 
//    window.onwheel = null; 
//    window.ontouchmove = null;  
//    document.onkeydown = null;  
//}

</script>
    <style type="text/css">
        .hiddencol
          {
            display: none;
          }
        body
        {
            margin: 0;
            padding: 0;
            height: 100%;
        }
        .modal
        {
            display: none;
            position: absolute;
            top: 0px;
            left: 0px;
            background-color: black;
            z-index: 100;
            opacity: 0.8;
            filter: alpha(opacity=60);
            -moz-opacity: 0.8;
            min-height: 100%;
        }
        #divImage
        {
            display: none;
            z-index: 1000;
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            height: 550px;
            width: 600px;
            padding: 3px;
            border: solid 1px black;
        }
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <br />
            <center>
                <asp:Label ID="Label1" runat="server" Font-Size = "20px" Text="Личный кабинет для заказа литературы из имидж-каталога"></asp:Label>
                <br />
                <asp:Label ID="Label2" runat="server" Font-Size = "20px" Text="ФИО"></asp:Label>
            </center>
            <br />
            <br />
            <br />
            <div align="right">
                <asp:LinkButton ID="LinkButton1" runat="server" Font-Size ="20px" PostBackUrl="~/history.aspx">История заказов.</asp:LinkButton>
            </div>
            <br />
            <div align="right">
                <asp:LinkButton ID="LinkButton2" runat="server" Font-Size ="20px" PostBackUrl="http://imcatnew.libfl.ru/ic/book/index.php">Перейти в имидж каталог</asp:LinkButton>
            </div>
            <br />
            <br />
           <div align="center" style="font-size:x-large">Активные заказы из имидж каталога</div>
            <br />

        <div  style="position:absolute;left:50px;">

             <asp:GridView ID="gwBasket" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
                  BorderStyle="Solid" BorderColor = "Black" 
                  Font-Size = "20px" CellPadding="5" 
                  onrowdatabound="gwBasket_RowDataBound" OnRowCommand="gwBasket_RowCommand">                                  
                <Columns>
                    <asp:BoundField
                        DataField="orderId" >
                        <HeaderStyle CssClass="hiddencol" />
                        <ItemStyle CssClass="hiddencol" />
                    </asp:BoundField>
                    <asp:BoundField
                        DataField="mainSideUrl" >
                        <HeaderStyle CssClass="hiddencol" />
                        <ItemStyle CssClass="hiddencol" />
                    </asp:BoundField>
                    <asp:BoundField
                        DataField="selectedSideUrl" >
                        <HeaderStyle CssClass="hiddencol" />
                        <ItemStyle CssClass="hiddencol" />
                    </asp:BoundField>
                                        
                    <asp:BoundField HeaderText="№№" 
                        DataField="num">
                    <HeaderStyle BackColor="Silver" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="Главная карточка" >
                    <HeaderStyle BackColor="Silver" />
                        <ItemTemplate>
                            <asp:ImageButton ID="mainSideImage" runat="server" 
                            Width="300px" Height ="150px" Style="cursor: pointer" OnClientClick="return LoadDiv(this.src);" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Выбранная карточка" >
                    <HeaderStyle BackColor="Silver" />
                        <ItemTemplate>
                            <asp:ImageButton ID="selectedSideImage" runat="server" 
                            Width="300px" Height ="150px" Style="cursor: pointer" OnClientClick="return LoadDiv(this.src);" />
                        </ItemTemplate>
                    </asp:TemplateField>
               
                    <asp:BoundField HeaderText="Комментарий" >
                    <HeaderStyle BackColor="Silver" />
                    </asp:BoundField>

                    <asp:BoundField HeaderText="Статус" >
                    <HeaderStyle BackColor="Silver" />
                        <ItemStyle Width="200px" />
                    </asp:BoundField>
                    
                    <asp:TemplateField HeaderText="Отменить заказ" >
                    <HeaderStyle BackColor="Silver" />
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="delOrder" 
                            CommandArgument='<%#Eval("orderId")%>'>Отменить заказ</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>

 <!--<asp:TemplateField HeaderText="Удаление" >
                    <HeaderStyle BackColor="Silver" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lbDelete" runat="server" CommandName="del"  
                                Text="Удалить"  CommandArgument='<%#Eval("IDMAIN")%>' >
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>-->
            
            
<div id="divBackground" class="modal" style="display: none; ">
</div>

<div id="divImage" >
<table style="height: 100%; width: 100%">
    <tr>
        <td valign="middle" align="center">
            <img id="imgLoader" alt="" src="images/loader.gif" />
            <img id="imgFull" alt="" src="" style="display: none; " />
        </td>
    </tr>
    <tr>
        <td align="center" valign="bottom">
            <input id="btnClose" type="button" value="Закрыть" onclick="HideDiv()" />
        </td>
    </tr>
</table>
</div>
        </div>
    </form>
</body>
</html>
