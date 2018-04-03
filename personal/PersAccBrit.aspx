<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersAccBrit.aspx.cs" Inherits="PersAccBrit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Личный кабинет</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <br />
    <br />
    <center>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></center>
    <br />
    <br />
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" 
            Height="500px" Width="100%" 
            onactivetabchanged="TabContainer1_ActiveTabChanged"  
            AutoPostBack = "true" >
            
            <asp:TabPanel ID="TabPanel3" runat="server" 
                HeaderText="Выданные книги фонда британского совета">               
                <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" Height="500px" ScrollBars="Auto">
                    <asp:Table ID="Table1" runat="server">
                    </asp:Table>
                </asp:Panel>
                </ContentTemplate>
            </asp:TabPanel>
            
            <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="История выданных книг">
                <ContentTemplate>
                <asp:Panel ID="Panel2" runat="server" Height="500px" ScrollBars="Auto">

                    <asp:Table ID="Table2" runat="server">
                    </asp:Table>
                </asp:Panel>

                </ContentTemplate>
                
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="Выход">
                
            </asp:TabPanel>
        </asp:TabContainer>
    
    </ContentTemplate>
    </asp:UpdatePanel>    
    </form>
</body>
</html>
