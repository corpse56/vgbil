<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Очередь изданий на оцифровку для адмнистратора</title>
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
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
    <ContentTemplate>
    <div><center>
    <center><h1>Очередь изданий на оцифровку для адмнистратора</h1></center>
    <center><h3>Отображается полная очередь</h3></center><br />
    <div style="text-align:right">
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl = "~/Deleted.aspx">Удалённые из очереди издания</asp:HyperLink>
        <br />
    </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Reservation_RConnectionString %>" 
                    SelectCommand="with main as ( select row_number() over (order by (A.CREATED )) num,A.IDMAIN pin,case when A.BAZA = 1 then avtp.PLAIN else ravtp.PLAIN end avt, DELCAUSE delcause, DELDATE deldate,
                                        case when A.BAZA = 1 then zagp.PLAIN else rzagp.PLAIN end zag,A.CREATED cre,case when A.BAZA = 1 then 'Основной фонд' else 'Фонд редкой книги' end baza, A.ID id 
                                            from Reservation_R..TURNTODIGITIZE A 
                                            left join BJVVV..DATAEXT zag on A.IDMAIN = zag.IDMAIN and zag.MNFIELD = 200 and zag.MSFIELD = '$a' 
                                           left join BJVVV..DATAEXTPLAIN zagp on zag.ID = zagp.IDDATAEXT 
                                            left join BJVVV..DATAEXT avt on A.IDMAIN = avt.IDMAIN and avt.MNFIELD = 700 and avt.MSFIELD = '$a' 
                                            left join BJVVV..DATAEXTPLAIN avtp on avt.ID = avtp.IDDATAEXT 
                                            left join REDKOSTJ..DATAEXT rzag on A.IDMAIN = rzag.IDMAIN and rzag.MNFIELD = 200 and rzag.MSFIELD = '$a'
                                           left join REDKOSTJ..DATAEXTPLAIN rzagp on zag.ID = rzagp.IDDATAEXT 
                                           left join REDKOSTJ..DATAEXT ravt on A.IDMAIN = ravt.IDMAIN and ravt.MNFIELD = 700 and ravt.MSFIELD = '$a' 
                                           left join REDKOSTJ..DATAEXTPLAIN ravtp on ravt.ID = ravtp.IDDATAEXT 
                                           where (A.DELETED is null or DELETED = 0) and not exists (select IDBook,IDBase from BookAddInf..ScanInfo CC where A.IDMAIN = CC.IDBook and A.BAZA = CC.IDBase)
                                           ) select * from main  order by num desc" >
                                           
           
        </asp:SqlDataSource><!-- ) select top 100 * from main   where num<=100 order by num desc" >-->
      <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
         BorderStyle="Solid" BorderColor = "Black"
            RowStyle-Wrap ="true"  Font-Size = "20px" CellPadding="5" 
            onrowdatabound="GridView1_RowDataBound" >
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
                
                <asp:TemplateField HeaderText="Отметка 'в работе'" >
                <HeaderStyle BackColor="Silver" />
                    <ItemTemplate>
                        
                        <asp:LinkButton ID="lbSetMark" runat="server" CommandName="mark" 
                            Text="Отметить" OnCommand="mark_click" CommandArgument='<%#Eval("ID")%>' >
                        </asp:LinkButton>
                        
                    </ItemTemplate>
                
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Удалить отметку 'в работе'" >
                <HeaderStyle BackColor="Silver" />
                    <ItemTemplate>
                        
                        <asp:LinkButton ID="lbDelMark" runat="server" CommandName="delmark" 
                            Text="Удалить отметку" OnCommand="delmark_click" CommandArgument='<%#Eval("ID")%>' >
                        </asp:LinkButton>
                        
                    </ItemTemplate>
                
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Удалить издание из очереди" >
                <HeaderStyle BackColor="Silver" />
                    <ItemTemplate>
                        
                        <asp:LinkButton ID="lbDelFromTurn" runat="server" CommandName="delfromturn" 
                            Text="Удалить из очереди" OnCommand="delfromturn_click" CommandArgument='<%# Eval("ID") %>' >
                        </asp:LinkButton>
      
                    </ItemTemplate>
                
                </asp:TemplateField>
                
            </Columns>
            
        </asp:GridView>  
        
        </center>    
        
        <asp:Button ID="Button3" runat="server" Text="Button3" style="display:none"   />

                <asp:ModalPopupExtender ID="mpe" runat="server" 
                BackgroundCssClass="modalBG" PopupControlID="Panel1"  TargetControlID="Button3" CancelControlID = "Button2" >
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup"   align="center" >
            <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Введите причину удаления из очереди!"></asp:Label>
                </td>
            </tr>
            <tr>
            <td>
                <asp:TextBox ID="TextBox1" runat="server" Height="70px" Width="270px" 
                    TextMode="MultiLine" MaxLength="300" style = "resize:none" ></asp:TextBox>
            </td>
            </tr>
            <tr>
            <td><asp:Button ID="Button1" runat="server" Text="Подтвердить" 
                    onclick="Button1_Click" />&nbsp&nbsp
            <asp:Button ID="Button2" runat="server" Text="Отмена" onclick="Button2_Click" /></td>
            
          </tr>
          </table>

        </asp:Panel>
        
                    <asp:HiddenField ID="hfIDOFSELECTEDROW" runat="server" />    
        
            
                     

        </ContentTemplate>
       
        
        </asp:UpdatePanel>   
        
        
        
        
    </form>
</body>
</html>
