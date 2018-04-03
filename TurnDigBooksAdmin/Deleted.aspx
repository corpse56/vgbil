<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Deleted.aspx.cs" Inherits="_Deleted" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Очередь изданий на оцифровку для адмнистратора</title>
    
</head>

<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div><center>
    <center><h1>Очередь изданий на оцифровку для адмнистратора</h1></center>
    <center><h3>Отображается последние 100 удалённых изданий</h3></center><br />
        <div style="text-align:right">
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl =  "~/Default.aspx">Текущая очередь</asp:HyperLink>
        <br />
    </div>
      <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
         BorderStyle="Solid" BorderColor = "Black"   GridLines = "Both"
            RowStyle-Wrap ="true"  Font-Size = "20px" CellPadding="5" onrowdatabound="GridView1_RowDataBound" 
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

                <asp:BoundField HeaderText="Дата удаления" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Причина удаления" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                
            </Columns>
            
        </asp:GridView>  
        
        </center>         

        </ContentTemplate>
        </asp:UpdatePanel>      
    </form>
</body>
</html>
