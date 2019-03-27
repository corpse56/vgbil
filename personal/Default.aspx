<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Заказ книг сотрудниками</title>
  <!-- calendar stylesheet -->
<link rel="stylesheet" type="text/css" media="all" href="calendar-win2k-cold-2.css" title="win2k-cold-2" />
  

  <!-- main calendar program -->
  <script type="text/javascript" src="calendar.js"></script>

  <!-- language for the calendar -->
  <script type="text/javascript" src="lang/calendar-ru.js"></script>

  <!-- the following script defines the Calendar.setup helper function, which makes
       adding a calendar a matter of 1 or 2 lines of code. -->
  <script type="text/javascript" src="calendar-setup.js"></script>
    <style type="text/css">
          .hiddencol
          {
            display: none;
          }
        .style1
        {
            width: 2%;
            height: 26px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode ="Always" >
        <ContentTemplate>
          <table style="width:100%">
          <tr>
          <td>
            <div style="width:100%;text-align:center" >
                <asp:Label ID="Label1" runat="server" Text="Личный кабинет сотрудника" Font-Bold="True" Font-Size = "Large" ></asp:Label>
                <br />
                <asp:Label ID="Label2" runat="server" Text="" Font-Bold="True" Font-Size = "Large" ></asp:Label>
            </div>
            <br />
          </td>
          </tr>
          <tr>
              <td style="height: 356px">  
                          <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                       OnActiveTabChanged="TabContainer1_ActiveTabChanged" 
                       ScrollBars="None"  AutoPostBack = "true"
                      Style="left: 0px; top: 0px;visibility:visible; " 
                      Visible="True" Width="100%" Height = "580">
                      
                      <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Заказ книг из основного фонда">
                        <ContentTemplate>
                              <asp:Panel ID="Panel1" runat="server" Height="500px" ScrollBars="Auto" BorderWidth = "1px">
                              
                                <asp:GridView ID="gwBasket" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
                                                    BorderStyle="Solid" BorderColor = "Black"  
                                      Font-Size = "15px" CellPadding="5" 
                                                    onrowcommand="GridView1_RowCommand"   
                                       ondatabound="gwBasket_DataBound" 
                                      onrowdatabound="gwBasket_RowDataBound">
                                                    
                                    <Columns>
                                        <asp:BoundField
                                         DataField="IDMAIN" >
                                            <HeaderStyle CssClass="hiddencol" />
                                            <ItemStyle CssClass="hiddencol" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField HeaderText="№№" >
                                        <HeaderStyle BackColor="Silver" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Биб. описание" >
                                        <HeaderStyle BackColor="Silver" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Заглавие" >
                                        <HeaderStyle BackColor="Silver" />
                                        </asp:BoundField>
                                        
                                        <asp:BoundField HeaderText="Автор" >
                                        <HeaderStyle BackColor="Silver" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Инв. Номер" >
                                        <HeaderStyle BackColor="Silver" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Инв. метка" Visible = "False" >
                                        <HeaderStyle BackColor="Silver" />
                                        </asp:BoundField>

                                        <asp:BoundField HeaderText="Статус" >
                                        <HeaderStyle BackColor="Silver" />
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Заказ" >
                                        <HeaderStyle BackColor="Silver" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbOrd" runat="server" CommandName="ord" 
                                                    Text="Заказать"  CommandArgument='<%#Eval("IDDATA")%>'  >
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Удаление" >
                                        <HeaderStyle BackColor="Silver" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbDelete" runat="server" CommandName="del"  
                                                    Text="Удалить"  CommandArgument='<%#Eval("IDMAIN")%>' >
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:BoundField
                                         DataField="IDDATA" >
                                            <HeaderStyle CssClass="hiddencol" />
                                            <ItemStyle CssClass="hiddencol" />
                                        </asp:BoundField> 
                                          
                                        
                                        <asp:BoundField> 
                                            <HeaderStyle CssClass="hiddencol" />
                                            <ItemStyle CssClass="hiddencol" />
                                        </asp:BoundField>   

                                        
                                        <asp:BoundField HeaderText="Статус">
                                            <HeaderStyle CssClass="hiddencol" />
                                            <ItemStyle CssClass="hiddencol" />
                                        </asp:BoundField>   
                                        
                                        <asp:BoundField HeaderText="Стеллаж" >
                                        <HeaderStyle BackColor="Silver" />
                                            <ItemStyle Width="200px" />
                                        </asp:BoundField>
                                                                             
                                    </Columns>

                                    

                                  </asp:GridView>         

                              </asp:Panel>
                            <br />
                              <table>
                                  <tr>
                                      <td style="height: 26px; width: 43%;" valign="top">
                                        
                                         
                                          <asp:Button ID="Button4" runat="server" Text="Сформировать список БО" 
                                              onclick="Button4_Click"  OnClientClick="aspnetForm.target ='_blank';" />&nbsp;&nbsp;
                                          <asp:Button ID="Button5" runat="server" Text="Очистить корзину" 
                                              onclick="Button5_Click"  />&nbsp;&nbsp;
                                              
                                      </td>
                                      <td style="height: 26px; width: 3%;" valign="top">
                                      </td>
                                      
                                      <td style="height: 26px; width: 43%; margin: 1px;" valign="top">
                                      </td>
                                  </tr>
                              </table>
                              <asp:PlaceHolder ID="holder1" runat="server"></asp:PlaceHolder>
                          </ContentTemplate>
                      </asp:TabPanel>
                      <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="История">

                          <ContentTemplate>
                              <asp:Table ID="Table4" runat="server" GridLines="Both" style="z-index: 107; left: 9px; top: 66px;" width="99%">
                                  
                              </asp:Table>
                          </ContentTemplate>
                      </asp:TabPanel>
                      <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Выход"></asp:TabPanel>
                      
                  </asp:TabContainer>

              </td>
          </tr>
          </table> 
           </ContentTemplate>
            
         </asp:UpdatePanel> 
         <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <asp:Label ID="lblWait" runat="server" BackColor="#507CD1" Font-Bold="True" ForeColor="White" Text="Идет загрузка ..."></asp:Label>
                <asp:Image ID="Image3" runat="server" ImageUrl="~/images/ajax-loader.gif" Width = "50px" Height="50px" BorderWidth = "0px" BorderStyle="Solid" BorderColor ="Black" />
                
            </ProgressTemplate>
           </asp:UpdateProgress>
           
         </form>

    <!-- =================================================  Скрипты  ============================================-->
     <script language ="javascript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler); 
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler); 
        var y;
        function BeginRequestHandler(sender, args)
                {
                            var scroll__ = document.getElementById('TabContainer1_TabPanel1_Panel1');
                            y = scroll__.scrollTop;   
                }
        function EndRequestHandler(sender, args)
                {
                            var scroll__ = document.getElementById('TabContainer1_TabPanel1_Panel1');
                            scroll__.scrollTop = y;   
                }
        function updatePanel()
                {
                            var scroll_ = document.getElementById('TabContainer1_TabPanel1_Panel1');
                            y = scroll_.scrollTop;
                }
                
       
        function GetAbsTop(_obj) {
           var _top=0;
           var _parent=_obj;
              _top+=_parent.offsetTop;
              _top+=_parent.clientTop;
           do {
              _parent=_parent.offsetParent;
              _top+=_parent.offsetTop;
              _top+=_parent.clientTop;
           } while (_parent!==document.body);
          return _top-document.body.scrollTop;
        };
        function GetAbsLeft(_obj) {
             var _left=0;
             var _parent=_obj;
                 _left+=_parent.offsetLeft;
                 _left+=_parent.clientLeft;
             do {
                 _parent=_parent.offsetParent;
                 _left+=_parent.offsetLeft;
                 _left+=_parent.clientLeft;
             } while (_parent!==document.body);
             return _left-document.body.scrollLeft;
        };
        function GetAbsCoords(_obj) {
             var _top=0;
             var _left=0;
             var _parent=_obj;
                 _top+=_parent.offsetTop;
                 _top+=_parent.clientTop;
                 _left+=_parent.offsetLeft;
                 _left+=_parent.clientLeft;
             do {
                 _parent=_parent.offsetParent;
                 _top+=_parent.offsetTop;
                 _top+=_parent.clientTop;
                 _left+=_parent.offsetLeft;
                 _left+=_parent.clientLeft;
             } while (_parent!==document.body);
             return [_top-document.body.scrollTop, _left-document.body.scrollLeft];
          }  
            
            
          
     </script>
                
    </body>
</html>
