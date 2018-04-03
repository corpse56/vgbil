<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewTurn.aspx.cs" Inherits="ViewTurn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Просмотр очереди изданий на оцифровку</title>
</head>
<body>
    <form id="form1" runat="server">
    <div><center>
    <center><h1>Очередь изданий на оцифровку</h1></center>
    <!--<center><h3>Отображается первые 50 изданий очереди</h3></center><br />-->
    <div style="text-align:right">
    <div style="text-align:right">
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl =  "~/ViewTurn.aspx">Текущая очередь</asp:HyperLink> &nbsp&nbsp&nbsp
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl =  "~/ViewCompleted.aspx">Выполнены</asp:HyperLink> &nbsp&nbsp&nbsp
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl =  "~/ViewDeleted.aspx">Удалены</asp:HyperLink> &nbsp&nbsp&nbsp
        </div>       </div>
        <br />
      <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
         BorderStyle="Solid" BorderColor = "Black"   GridLines = "Both"
            RowStyle-Wrap ="true"  Font-Size = "20px" CellPadding="5" 
              >
<RowStyle Wrap="True"></RowStyle>
            <Columns>
                
                <asp:BoundField HeaderText="№№">
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>

                <asp:BoundField HeaderText="База" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>

                <asp:BoundField HeaderText="ПИН" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                
                <asp:BoundField HeaderText="Автор" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                
                <asp:BoundField HeaderText="Заглавие" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Дата добавления" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
     
            </Columns>
            
        </asp:GridView>  
        </center>        
    </div>
    </form>
</body>
</html>
