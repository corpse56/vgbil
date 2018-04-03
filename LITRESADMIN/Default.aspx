<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" EnableEventValidation="false" %>

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
    <center><h1>Управление логинами и паролями Литрес</h1></center>
    
    <div style="text-align:left">
        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Добавить аккаунт</asp:LinkButton>
        &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
        <asp:LinkButton ID="LinkButton2" runat="server" onclick="LinkButton2_Click" >Статистика</asp:LinkButton>
        <br />
    </div>
        <asp:Label ID="Label8" runat="server" Text="Label" Visible = "false"></asp:Label>
      <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
         BorderStyle="Solid" BorderColor = "Black"
            RowStyle-Wrap ="true"  Font-Size = "20px" CellPadding="5" 
            onrowcommand="GridView1_RowCommand" AllowPaging = "true"  PageSize = "20" 
            
            onpageindexchanging="GridView1_PageIndexChanging"  >
<RowStyle Wrap="True"></RowStyle>
            <Columns>
                
                <asp:BoundField HeaderText="№№">
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Логин" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Пароль" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                
                <asp:BoundField HeaderText="Аккаунт создан" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                
                <asp:BoundField HeaderText="Аккаунт присвоен" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Аккаунт изменён" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Номер читателя" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Фамилия" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Имя" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Отчество" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                <asp:BoundField HeaderText="Дата рождения" >
                <HeaderStyle BackColor="Silver" />
                </asp:BoundField>
                
                <asp:TemplateField HeaderText="Изменение" >
                <HeaderStyle BackColor="Silver" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lbupdate" runat="server" CommandName="upd" 
                            Text="Изменить"  CommandArgument='<%#Eval("ID")%>' >
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Удаление" >
                <HeaderStyle BackColor="Silver" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lbdelete" runat="server" CommandName="del" 
                            Text="Удалить"  CommandArgument='<%#Eval("ID")%>' >
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                
                
            </Columns>
            
        </asp:GridView>  
        <br />
        <br />
            <div style="text-align:left">
        <asp:Button ID="Button10" runat="server" Text="Выгрузить в *.xls формате" onclick="Button10_Click" />
        <br />
            </div>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:LITRESConnectionString %>" 
                    
            SelectCommand="
with main as(  
SELECT 
 A.LRLOGIN, A.LRPWD, A.CREATED, 
CASE WHEN A.ASSIGNED IS NULL THEN 'Никогда' ELSE CONVERT(varchar(50), A.ASSIGNED, 104)+' '+CONVERT(varchar(50), A.ASSIGNED, 108) END ASSIGNED,
CASE WHEN A.CHANGED IS NULL THEN 'Никогда' ELSE CONVERT(varchar(50), A.CHANGED, 104)+' '+CONVERT(varchar(50), A.CHANGED, 108) END CHANGED,
CASE WHEN A.IDREADER IS NULL THEN 'Не назначен' else convert(varchar(50),A.IDREADER) end IDREADER,
isnull(B.FamilyName,'Не назначен') FamilyName,
isnull(B.Name,'Не назначен') Name,
isnull(B.FatherName,'Не назначен') FatherName,
CASE WHEN B.DateBirth IS NULL THEN 'Никогда' ELSE CONVERT(varchar(50), B.DateBirth, 104) END DateBirth
,A.ID 
FROM [LITRES].[dbo].ACCOUNTS AS A 
LEFT OUTER JOIN Readers.dbo.Main AS B ON A.IDREADER = B.NumberReader 

)
select row_number() over (order by CREATED) num,* from main order by CREATED
" >
           
        </asp:SqlDataSource>
        
        </center>    
<!--
union all
SELECT 
 A.LRLOGIN, A.LRPWD, A.CREATED, 
CASE WHEN A.ASSIGNED IS NULL THEN 'Никогда' ELSE CONVERT(varchar(50), A.ASSIGNED, 104)+' '+CONVERT(varchar(50), A.ASSIGNED, 108) END ASSIGNED,
CASE WHEN A.CHANGED IS NULL THEN 'Никогда' ELSE CONVERT(varchar(50), A.CHANGED, 104)+' '+CONVERT(varchar(50), A.CHANGED, 108) END CHANGED,
CASE WHEN A.IDREADER IS NULL THEN 'Не назначен' else convert(varchar(50),A.IDREADER) end IDREADER,
A.ID 
FROM [LITRES].[dbo].ACCOUNTS AS A 
where A.RTYPE = 1LEFT OUTER JOIN Readers.dbo.RemoteMain AS C ON A.IDREADER = C.NumberReader and A.RTYPE = 1
isnull(C.FamilyName,'Не назначен') FamilyName,
isnull(C.Name,'Не назначен') Name,
isnull(C.FatherName,'Не назначен') FatherName,
CASE WHEN C.DateBirth IS NULL THEN 'Никогда' ELSE CONVERT(varchar(50), C.DateBirth, 104) END DateBirth
,
-->
        
        <asp:Button ID="Button3" runat="server" Text="Button3" style="display:none"   />
        <asp:Button ID="Button8" runat="server" Text="Button3" style="display:none"   />
        <asp:Button ID="Button9" runat="server" Text="Button3" style="display:none"   />

                <asp:ModalPopupExtender ID="mpeADD" runat="server" 
                BackgroundCssClass="modalBG" PopupControlID="Panel1"  TargetControlID="Button3" CancelControlID="Button5" >
        </asp:ModalPopupExtender>
        <asp:ModalPopupExtender ID="mpeEDIT" runat="server" 
                BackgroundCssClass="modalBG" PopupControlID="Panel2"  TargetControlID="Button8" CancelControlID="Button4" >
        </asp:ModalPopupExtender>
        <asp:ModalPopupExtender ID="mpeDEL" runat="server" 
                BackgroundCssClass="modalBG" PopupControlID="Panel3"  TargetControlID="Button9" CancelControlID="Button7" >
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup"   align="center" Width = "400px" >
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
            <table>
            <tr>
                <td colspan = "2" align="center">
                    <asp:Label ID="Label1" runat="server" Text="Введите логин и пароль, которые собираетесь добавить!"></asp:Label>
                </td>
            </tr>
            <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Логин"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox2" runat="server"  Width="300px" 
                     MaxLength="300" ></asp:TextBox>
            </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Пароль"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" Width="300px"></asp:TextBox>
                </td>
            <tr>
            <td>
            </td>
            <td>
            <asp:Button ID="Button1" runat="server" Text="Подтвердить" onclick="Button1_Click" 
                     />&nbsp&nbsp
            <asp:Button ID="Button5" runat="server" Text="Отмена" onclick="Button5_Click" />
            </td>
                        <tr>
            <td colspan="2">
                <asp:Label ID="Label10" runat="server" Text="Логин и/или пароль не могут быть пустыми!" ForeColor = "Red" Visible ="false" ></asp:Label>
            </td>
            </tr>
          </tr>
          </table>
</ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup"   align="center" Width = "400px" >
            <table>
            <tr>
                <td colspan = "2" align="center">
                    <asp:Label ID="Label4" runat="server" Text="Измените логин и пароль!"></asp:Label>
                </td>
            </tr>
            <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Логин"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TextBox3" runat="server"  Width="300px" 
                     MaxLength="300" ></asp:TextBox>
            </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Пароль"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TextBox4" runat="server" Width="300px"></asp:TextBox>
                </td>
            </tr>
            
            <tr>
            <td>
            </td>
            <td>
            <asp:Button ID="Button2" runat="server" Text="Подтвердить" onclick="Button2_Click" 
                     />&nbsp&nbsp
            <asp:Button ID="Button4" runat="server" Text="Отмена" onclick="Button4_Click"  />
            </td>
            
          </tr>
            <tr>
            <td colspan="2">
                <asp:Label ID="Label9" runat="server" Text="Логин и/или пароль не могут быть пустыми!" ForeColor = "Red" Visible ="false" ></asp:Label>
            </td>
            </tr>
          </table>

        </asp:Panel>
        <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup"   align="center" Width = "300px" Height = "70" >
            <table>
            <tr>
                <td colspan = "2" align="center">
                    <asp:Label ID="Label7" runat="server" Text="Вы уверены, что хотите удалить запись?"></asp:Label>
                </td>
            </tr>
            <tr>
            <td>
            </td>
            <td>
            <asp:Button ID="Button6" runat="server" Text="Подтвердить" onclick="Button6_Click" 
                     />&nbsp&nbsp
            <asp:Button ID="Button7" runat="server" Text="Отмена" onclick="Button7_Click" />
            </td>
            
          </tr>
          </table>

        </asp:Panel>
        
                    <asp:HiddenField ID="hfIDOFSELECTEDROW" runat="server" />    
        
            
                     

        </ContentTemplate>
       <Triggers>
       <asp:PostBackTrigger  ControlID="Button10" />
       </Triggers>
        
        </asp:UpdatePanel>   
    </form>
</body>
</html>
