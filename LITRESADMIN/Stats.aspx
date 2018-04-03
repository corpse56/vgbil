<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Stats.aspx.cs" Inherits="Stats" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Выдача паролей ЛИТРЕС</title>
    <style type ="text/css">
    .modalBG
    {
        background-color:Black;
        filter:alpha(opacity=90);
        opacity:0.8;
    }
    .modalPopup
    {
        background-color:#FFFFFF;
        border-width:3;
        border-style:solid;
        border-color:Black;
        padding-top:10px;
        padding-left:10px;
        width:300px;
        height:150px;
    }
    </style> 
</head>
<body>
    <form id="form1" runat="server">
     
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
    <ContentTemplate>
    <div><center>
    <center><h1>Статистика</h1></center>
    
    <div style="text-align:left">
        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">На главную</asp:LinkButton>
        <br />
    </div>
        <asp:Label ID="Label8" runat="server" Text="Label" Visible = "false"></asp:Label>
      <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
         BorderStyle="Solid" BorderColor = "Black"
            RowStyle-Wrap ="true"  Font-Size = "20px" CellPadding="5" 
             AllowPaging = "true"  PageSize = "20" 
            
            onpageindexchanging="GridView1_PageIndexChanging"  >
<RowStyle Wrap="True"></RowStyle>
            <Columns>
                
                <asp:BoundField HeaderText="Дата">
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Зарегистрировано читателей" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Выдано аккаунтов LITRES читателям" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                
                <asp:BoundField HeaderText="Зарегистрировано удалённых читателей" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                
                <asp:BoundField HeaderText="Выдано аккаунтов LITRES удалённым читателям" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                
                
            </Columns>
            
        </asp:GridView>  
        <br />
        <br />
            <div style="text-align:left">
        <asp:Button ID="Button1" runat="server" Text="Выгрузить в *.xls формате" 
                    onclick="Button1_Click" />
        <br />
            </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:LITRESConnectionString %>"  
SelectCommand="
with byday as
(
select dateadd(d, CLEARNUMBER, '20160228') myd from LITRES..CLEARNUMBERS 
where CLEARNUMBER between 0 and datediff(d, '20160228', GETDATE()) 
),

ASGREG as
(
select A.myd , COUNT(B.ASSIGNED) assigned
    from byday A
left join LITRES..ACCOUNTS B on A.myd = cast(B.ASSIGNED as date) and B.RTYPE = 0
group by A.myd
),
ASGREMREG as
(
select A.myd , COUNT(B.ASSIGNED) assigned
    from byday A
left join LITRES..ACCOUNTS B on A.myd = cast(B.ASSIGNED as date) and B.RTYPE = 1

group by A.myd
),
REG as
(
select A.myd , COUNT(B.DateRegistration) registered
    from byday A
left join Readers..MAIN B on A.myd = cast(B.DateRegistration as date)
where B.TypeReader = 0
group by A.myd
),
REMREG as
(
select A.myd , COUNT(B.DateRegistration) remoteregistered
    from byday A
left join Readers..Main B on A.myd = cast(B.DateRegistration as date)
where B.TypeReader = 1
group by A.myd
)
select cast(A.myd as date) myd,isnull(REG.registered,0) registered, isnull(ASGREG.assigned,0) asg, isnull(REMREG.remoteregistered,0) remoteregistered, isnull(ASGREMREG.assigned,0) asgr
from byday A
left join ASGREG on ASGREG.myd = A.myd
left join ASGREMREG on ASGREMREG.myd = A.myd
left join REG on REG.myd = A.myd
left join REMREG on REMREG.myd = A.myd
order by myd"
onselecting="SqlDataSource1_Selecting" >
           
        </asp:SqlDataSource>
        
        </center>    
        </div>
       
            
                     

        </ContentTemplate>
        <Triggers> 
         <asp:PostBackTrigger ControlID="Button1" />
        </Triggers>
       
        
        </asp:UpdatePanel>   
    </form>
</body>
</html>
