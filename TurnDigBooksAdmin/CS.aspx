<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CS.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=40);
            opacity: 0.4;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 300px;
            border: 3px solid #0DA9D0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            line-height: 30px;
            text-align: center;
            padding:5px
        }
        .modalPopup .footer
        {
            padding: 3px;
        }
        .modalPopup .button
        {
            height: 23px;
            color: White;
            line-height: 23px;
            text-align: center;
            font-weight: bold;
            cursor: pointer;
            background-color: #9F9F9F;
            border: 1px solid #5C5C5C;
        }
        .modalPopup td
        {
            text-align:left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID = "ToolkitScriptManager1" runat = "server"></cc1:ToolkitScriptManager>
<asp:GridView ID="GridView1" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"
    runat="server" AutoGenerateColumns="false" OnSelectedIndexChanged="OnSelectedIndexChanged">
    <Columns>
        <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-Width="30" />
        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="150" />
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Label ID="lblCountry" Text='<%# Eval("Country") %>' runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:ButtonField Text="Select" CommandName="Select" />
    </Columns>
</asp:GridView>
<asp:LinkButton Text="" ID = "lnkFake" runat="server" />
<cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkFake"
CancelControlID="btnClose" BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
    <div class="header">
        Details
    </div>
    <div class="body">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style = "width:60px">
                    <b>Id: </b>
                </td>
                <td>
                    <asp:Label ID="lblId" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <b>Name: </b>
                </td>
                <td>
                    <asp:Label ID="lblName" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <b>Country: </b>
                </td>
                <td>
                    <asp:Label ID="lblCountry" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div class="footer" align="right">
        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" />
    </div>
</asp:Panel>
    </form>
</body>
</html>
