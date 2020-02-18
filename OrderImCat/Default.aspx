<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
            alert(document.body.clientHeight);
        }
        else
        {
            bcgDiv.style.height = document.body.scrollHeight + "px";
            
        }
        

        imgDiv.style.left = "100px";//(width - 650) / 2 + "px";
        imgDiv.style.top = "20px";
        bcgDiv.style.width = "100%";
        imgDiv.style.width = "650px";

        bcgDiv.style.display = "block";
        imgDiv.style.display = "block";
        return false;
    }

    function HideDiv()
    {
        var bcgDiv = document.getElementById("divBackground");
        var imgDiv = document.getElementById("divImage");
        var imgFull = document.getElementById("imgFull");
        if (bcgDiv != null) {
        bcgDiv.style.display = "none";
        imgDiv.style.display = "none";
        imgFull.style.display = "none";
    }
}
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
            <center>
                <asp:Label ID="Label1" runat="server" Text="Личный кабинет для заказа литературы из имидж-каталога"></asp:Label>
                <asp:Label ID="Label2" runat="server" Text="ФИО"></asp:Label>
            </center>
            <br />
            <br />
            <br />
             
             <asp:GridView ID="gwBasket" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
                  BorderStyle="Solid" BorderColor = "Black"  
                  Font-Size = "15px" CellPadding="5" 
                  onrowdatabound="gwBasket_RowDataBound">                                  
                <Columns>
                    <asp:BoundField
                        DataField="OrderId" >
                        <HeaderStyle CssClass="hiddencol" />
                        <ItemStyle CssClass="hiddencol" />
                    </asp:BoundField>
                                        
                    <asp:BoundField HeaderText="№№" >
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

                                        
                   
                                                                             
                </Columns>
            </asp:GridView>
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
