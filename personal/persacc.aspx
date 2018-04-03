<%@ Page Language="C#" AutoEventWireup="true" CodeFile="persacc.aspx.cs" Inherits="persacc"   %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Личный кабинет читателя</title>
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
          #form1
          {
              /*font-weight: 700;*/
          }
        </style>
</head>
<body>
    <form id="form1" runat="server" >
             <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl ="~/help.aspx" >Помощь</asp:LinkButton> 
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode = "Always">
    <ContentTemplate>
    <asp:Button ID="load" runat="server" OnClick="load_Click" 
    style="z-index:-90; display:none" Text="Button" Visible="true" />

    <center>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></center>
    <br />
        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="1" 
            Height="530px" Width="100%" 
            onactivetabchanged="TabContainer1_ActiveTabChanged"  
            OnClientActiveTabChanged="actTabChng"
            AutoPostBack = "true" >
            
            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Заказ книг из основного фонда" Enabled = "true">
                <HeaderTemplate>
                    Заказ книг из основного фонда
                </HeaderTemplate>
            <ContentTemplate>
                         <asp:Panel ID="Panel1" runat="server" Height="500px" ScrollBars="Auto" BorderWidth = "1px">
                                     <table id="table3" style="z-index: 107; left: 9px; top: 66px;" width="99%">
                                      
                                      <tr>
                                          <td valign="top" style="height: 26px; width: 43%;" >
                                              <asp:Table ID="BasketTable" runat="server" BorderStyle="Solid" GridLines="Both" 
                                                  style="color:Black;table-layout:inherit;border-color:Black;border-width:3px;border-style:Solid;z-index: 100; width: 100%;">
                                                  <asp:TableRow ID="TableRow1" runat="server">
                                                      <asp:TableCell ID="TableCell1" runat="server" RowSpan="3"></asp:TableCell>
                                                      <asp:TableCell ID="TableCell2" runat="server" ColumnSpan="5"></asp:TableCell>
                                                      <asp:TableCell ID="TableCell3" runat="server" RowSpan="3"></asp:TableCell>
                                                  </asp:TableRow>
                                                  <asp:TableRow ID="TableRow2" runat="server">
                                                      <asp:TableCell ID="TableCell4" runat="server"></asp:TableCell>
                                                      <asp:TableCell ID="TableCell5" runat="server"></asp:TableCell>
                                                      <asp:TableCell ID="TableCell6" runat="server"></asp:TableCell>
                                                      <asp:TableCell ID="TableCell7" runat="server"></asp:TableCell>
                                                  </asp:TableRow>
                                                  <asp:TableRow ID="TableRow3" runat="server">
                                                      <asp:TableCell ID="TableCell8" runat="server"></asp:TableCell>
                                                      <asp:TableCell ID="TableCell9" runat="server"></asp:TableCell>
                                                      <asp:TableCell ID="TableCell10" runat="server"></asp:TableCell>
                                                      <asp:TableCell ID="TableCell11" runat="server"></asp:TableCell>
                                                  </asp:TableRow>
                                                  <asp:TableRow ID="TableRow4" runat="server">
                                                      <asp:TableCell ID="TableCell12" runat="server" ColumnSpan="7"></asp:TableCell>
                                                  </asp:TableRow>
                                              </asp:Table>
                                              
                                          </td>
                                          <td valign="top" class="style1">
                                          </td>
                                          <td valign="top" style="height: 26px; width: 43%;">
                                              
                                              <asp:Table ID="OrdersTable" runat="server" BorderStyle="Solid" GridLines="Both" 
                                                  style=" table-layout:inherit; border-color:Black;border-width:3px;border-style:Solid;z-index:100;width:100%; color: Black;">
                                              </asp:Table>
                                              
                                          </td>
                                      </tr>
                                      

                                      <tr>
                                          <td valign="top">

                                              <asp:Button ID="Button3" runat="server" style="display:none;" Text="Button" />
                                                        <asp:Label ID="ErrorLabelOF" runat="server"></asp:Label>

                                              </td>
                                          <td valign="top" class="style1">
                                              
                                          </td>
                                          <td valign="top" style="height: 26px; width: 43%;">
                                              <asp:Table ID="Table6" runat="server" BorderStyle="Solid" GridLines="Both" 
                                                  style="table-layout:inherit;border-color:Black;border-width:3px;border-style:Solid;z-index: 100; width: 100%;">
                                              </asp:Table>
                                          </td>
                                      </tr>
                                      

                                  </table>
                                  </asp:Panel>
                                          <table>
                                  
                                  <tr style="margin:7px; height:40px; padding:7px">
                                  <div style="float: right; width: 160px; margin-top:7px"><asp:Button ID="Button2" runat="server" Text="Обновить статусы" style="height: 25px;"  /></div>
                                             
                                  <div style="float: left; margin-right: 160px; margin-top:7px"><asp:Button ID="Button1" onclick="Button1_Click1" runat="server" Text="Заказать выделенный" style="height: 25px"  /></div>

                                      <!--<td style="height: 26px; width: 3%;" valign="top">
                                      </td>
                                      
                                      <td style="height: 26px; width: 100%; margin: 1px; border-width:medium; border-color:Black" valign="top" align ="right" >
                                      </td>-->
                                      
                                  </tr>
                              </table>
                              

                              
                        </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Электронные книги">
                <ContentTemplate>
                   <asp:Label ID="Label2" runat="server" Text="Нет заказанных электронных книг"></asp:Label>
                    <asp:Table ID="tEBook" runat="server">
                    </asp:Table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel7" runat="server" HeaderText="История выдачи электронные книги">
                <ContentTemplate>
                    <asp:Table ID="tEBook_HST" runat="server">
                    </asp:Table>
                </ContentTemplate>
            </asp:TabPanel>
            
            <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Выданные книги">
            <ContentTemplate>
                                 <asp:Panel ID="Panel2" runat="server" Height="500px" ScrollBars="Auto" BorderWidth = "1px">

                <asp:Label ID="Label4" runat="server" Text="Книги выданные из основного фонда"></asp:Label>
                <br /><br />
                    <asp:Table ID="tISSUED" runat="server" Font-Size = "20px">
                    </asp:Table>
                    
                <br /><br />
                <asp:Label ID="Label5" runat="server" Text="Книги выданные из фонда Британского совета"></asp:Label>
                <br /><br />
                    <asp:GridView   ID="gv_BS" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
                                    BorderStyle="Solid" BorderColor = "Black"  Font-Size = "20px" CellPadding="5" >
                        <RowStyle Wrap="True"></RowStyle>
                        <Columns>
                            <asp:BoundField HeaderText="Заглавие">
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Автор" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата выдачи" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Срок возврата" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>  
        
                <br /><br />
                <asp:Label ID="Label6" runat="server" Text="Книги выданные из фонда Центра Американской Культуры"></asp:Label>
                <br /><br />
                                        <asp:GridView   ID="gv_ACC" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
                                    BorderStyle="Solid" BorderColor = "Black"  Font-Size = "20px" CellPadding="5" >
                        <RowStyle Wrap="True"></RowStyle>
                        <Columns>
                            <asp:BoundField HeaderText="Заглавие">
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Автор" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата выдачи" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Срок возврата" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>  
                <br /><br />
                <asp:Label ID="Label7" runat="server" Text="Книги выданные из фонда Французского культурного центра"></asp:Label>
                <br /><br />
                                        <asp:GridView   ID="gv_FCC" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
                                    BorderStyle="Solid" BorderColor = "Black"  Font-Size = "20px" CellPadding="5" >
                        <RowStyle Wrap="True"></RowStyle>
                        <Columns>
                            <asp:BoundField HeaderText="Заглавие">
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Автор" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата выдачи" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Срок возврата" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>  
                <br /><br />
                <asp:Label ID="Label8" runat="server" Text="Книги выданные из фонда Славянского Культурного Центра"></asp:Label>
                <br /><br />
                                        <asp:GridView   ID="gv_SCC" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
                                    BorderStyle="Solid" BorderColor = "Black"  Font-Size = "20px" CellPadding="5" >
                        <RowStyle Wrap="True"></RowStyle>
                        <Columns>
                            <asp:BoundField HeaderText="Заглавие">
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Автор" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата выдачи" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Срок возврата" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>  
                <br /><br />
                
</asp:Panel>
            </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="История выданных книг" Enabled = "true">
            <ContentTemplate>
                <asp:Panel ID="Panel3" runat="server" Height="500px" ScrollBars="Auto" BorderWidth = "1px">
                                <asp:Label ID="Label9" runat="server" Text="Книги выданные из основного фонда"></asp:Label>
                <br /><br />

                    <asp:Table ID="tISSUED_HST" runat="server" Font-Size = "20px">
                    </asp:Table>
                     <br /><br />
                <asp:Label ID="Label10" runat="server" Text="История книг выданных из фонда Британского совета"></asp:Label>
                <br /><br />
                    <asp:GridView   ID="gv_BS_HST" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
                                    BorderStyle="Solid" BorderColor = "Black"  Font-Size = "20px" CellPadding="5" >
                        <RowStyle Wrap="True"></RowStyle>
                        <Columns>
                            <asp:BoundField HeaderText="Заглавие">
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Автор" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата выдачи" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата возврата" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>  
        
                <br /><br />
                <asp:Label ID="Label11" runat="server" Text="История книги выданных из фонда Центра Американской Культуры"></asp:Label>
                <br /><br />
                                        <asp:GridView   ID="gv_ACC_HST" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
                                    BorderStyle="Solid" BorderColor = "Black"  Font-Size = "20px" CellPadding="5" >
                        <RowStyle Wrap="True"></RowStyle>
                        <Columns>
                            <asp:BoundField HeaderText="Заглавие">
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Автор" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата выдачи" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата возврата" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>  
                <br /><br />
                <asp:Label ID="Label12" runat="server" Text="История книги выданных из фонда Французского культурного центра"></asp:Label>
                <br /><br />
                                        <asp:GridView   ID="gv_FCC_HST" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
                                    BorderStyle="Solid" BorderColor = "Black"  Font-Size = "20px" CellPadding="5" >
                        <RowStyle Wrap="True"></RowStyle>
                        <Columns>
                            <asp:BoundField HeaderText="Заглавие">
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Автор" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата выдачи" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата возврата" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>  
                <br /><br />
                <asp:Label ID="Label13" runat="server" Text="История книг выданных из фонда Славянского Культурного Центра"></asp:Label>
                <br /><br />
                                        <asp:GridView   ID="gv_SCC_HST" runat="server" AutoGenerateColumns = "False"  BorderWidth="3px"
                                    BorderStyle="Solid" BorderColor = "Black"  Font-Size = "20px" CellPadding="5" >
                        <RowStyle Wrap="True"></RowStyle>
                        <Columns>
                            <asp:BoundField HeaderText="Заглавие">
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Автор" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата выдачи" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="Дата возврата" >
                            <HeaderStyle BackColor="Silver" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>  
                <br /><br />
                    
                    
                </asp:Panel>
            </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="История заказов" Enabled = "true">
            <ContentTemplate>
                <asp:Panel ID="Panel4" runat="server" Height="500px" ScrollBars="Auto" BorderWidth = "1px">
                    <asp:Table ID="tORDHIS" runat="server">
                    </asp:Table>
                </asp:Panel>
            </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel8" runat="server" HeaderText="Литрес" Enabled = "true">
            <ContentTemplate>
                <asp:Panel ID="Panel5" runat="server" Height="500px" ScrollBars="Auto" BorderWidth = "1px" >
                <table><tr><td style="padding:20px 20px 20px 20px; width:400px">
                    <asp:Button ID="bLitres" runat="server" Text="Получить пароль Литрес" 
                        onclick="bLitres_Click" /><br /><br />
                    <asp:Label ID="lblLitresLogin" runat="server" Text="номер читательского билета Литрес:"></asp:Label><br />
                    <asp:Label ID="lblLitresPwd" runat="server" Text="Пароль для Литрес:"></asp:Label><br /><br /><br /><br />
                    <asp:Label ID="Label3" runat="server" Text="В настоящее время закончились номера читательских билетов Литрес. Попробуйте позже." Visible = "false"></asp:Label><br /><br /><br />
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl = "http://al.litres.ru/" Target="_blank">Литрес</asp:HyperLink><br /><br />
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl = "http://al.litres.ru/biblioapps/" Target="_blank" >Мобильное приложение Литрес</asp:HyperLink><br /><br />
                </td></tr></table>
                </asp:Panel>
            </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="Выход">
            </asp:TabPanel>
        </asp:TabContainer>
           <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
            <ProgressTemplate>
                <asp:Label ID="lblWait" runat="server" BackColor="#507CD1" Font-Bold="True" ForeColor="White" Text="Идет загрузка ..."></asp:Label>
                <asp:Image ID="Image3" runat="server" ImageUrl="~/images/ajax-loader.gif" Width = "50px" Height="50px" BorderWidth = "0px" BorderStyle="Solid" BorderColor ="Black" />

            </ProgressTemplate>
           </asp:UpdateProgress>   
    </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <p style = "font-size:larger">Внимание! Не забывайте выходить из личного кабинета во избежание несанкционированного заказа литературы с вашего аккаунта!</p>
    </form>
    
         <script language ="javascript" type="text/javascript">
             /*function doPostBack(eventTarget, eventArgument) {
             if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
             theForm.__EVENTTARGET.value = eventTarget;
             theForm.__EVENTARGUMENT.value = eventArgument;
             alert('fghfghfgh');
             form1.submit();
        
        }
             }*/
             function ch() {
                 var prov = document.getElementById('TabContainer1_TabPanel1_сtb0');
                 //alert(prov.value);
             }
             function isDateSelected() {
                 //alert("ALARM!!!");
                 var chs;
                 var ctbs
                 for (i = 0; i < id3.length; i++) {
                     chs = document.getElementById(id3[i]);
                     //alert(chs.checked);
                     ctbs = document.getElementById(id2[i]);
                     //alert(ctbs.value);
                     if ((chs.checked) && (ctbs.value == '')) {
                         alert('Введите дату!');

                         break;
                     }
                     else {
                         //alert(ctbs.value);
                         continue;
                     }
                 }
             }
             function actTabChng() {
                 //var load = document.getElementById("TabContainer1_TabPanel1_load");
                 var load = document.getElementById("load");
                 //load.click();

                 //alert(load.nodeName);

                 __doPostBack('TabContainer1$TabPanel1$Button3', '');
                 __doPostBack('TabContainer1$TabPanel2$Button4', '');
             }
             // Sys.Application.notifyScriptLoaded();
             Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
             Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
             var y;
             function BeginRequestHandler(sender, args) {
                 var scroll__ = document.getElementById('TabContainer1_TabPanel1_Panel1');
                 y = scroll__.scrollTop;
                 //alert(y);
             }
             function EndRequestHandler(sender, args) {
                 var scroll__ = document.getElementById('TabContainer1_TabPanel1_Panel1');
                 scroll__.scrollTop = y;
                 // alert(y);
             }
             function updatePanel() {
                 var scroll_ = document.getElementById('TabContainer1_TabPanel1_Panel1');
                 y = scroll_.scrollTop;
                 //alert(y);
                 var load = document.getElementById("load");
                 load.click();

                 // это капец!  :( 
             }


             
             /*var sDate = new Date();
             var date = sDate.getDate();
             date = date + 3;
             sDate.setDate(date);*/

             //var cl = new Calendar(1, sDate, onSelect, onClose);
             var cl;
             var but = '';
             var txt = '';
             var dat = '';
             var SPECIAL_DAYS;

             function apiCal(butt, txtt, datt) {
             
                 var dateParts = datt.split(".");

                 var sDate = new Date(dateParts[2], (dateParts[1] - 1), dateParts[0]);

                 cl = new Calendar(1, sDate, onSelect, onClose);

                 cl.setDateStatusHandler(DateStatusFunc);

                 var startDate = new Date().setDate(28);
                
                 
                 
                 cl.create();
                 cl.setDateFormat("%d.%m.%Y");
                 
                 var XY = document.getElementById(butt);
                 var XX = XY.offsetLeft;
                 var YY = XY.offsetTop;
                 var par = XY.offsetParent;
                 while (par !== document.body) {
                     XX += par.offsetLeft
                     YY += par.offsetTop
                     par = par.offsetParent
                 }


                 var scroll_ = document.getElementById("TabContainer1_TabPanel1_Panel1");
                 cl.showAt(XX, YY - scroll_.scrollTop);
                 but = butt;
                 txt = txtt;
                 dat = datt;
                 

             };
             
             var dateInField;

             function onSelect(cl, date) {
                 if (!cl.dateClicked) {
                 }
                 else {
                     var input_field = document.getElementById(txt);
                     input_field.value = date;
                 }
                 if (cl.dateClicked) {
                     cl.callCloseHandler();
                 }
             };
             function onClose(cl) {
                 cl.destroy();
                 var load = document.getElementById("load");
                 load.click();

                 //var load = document.getElementById("load");
                 //load.click();

                 //__doPostBack('', '');
             };
             function dateIsSp(year, month, day) {

                 var m = SPECIAL_DAYS[month];

                 if (!m) return false;

                 for (var i in m) if (m[i] == day) {

                     return true;
                 }
                 return false;
             };
             function DateStatusFunc(date, y, m, d) {
                 var today = new Date();
                 var test = new Date();
                 today.setHours(0, 0, 0, 0);
                 //test.setFullYear(test.getFullYear() + 1);
                 test.setDate(test.getDate() + 7);
                 //alert(test);
                 if (dateIsSp(y, m, d))
                     return true
                 else
                     if ((date < today) || (date >= test)) return true;
                 return false; // other dates are enabled
                 // return true if you want to disable other dates
                 //cl.clicked = true;
             };
             
             
             
             function GetAbsTop(_obj) {
                 var _top = 0;
                 var _parent = _obj;
                 _top += _parent.offsetTop;
                 _top += _parent.clientTop;
                 do {
                     _parent = _parent.offsetParent;
                     _top += _parent.offsetTop;
                     _top += _parent.clientTop;
                 } while (_parent !== document.body);
                 return _top - document.body.scrollTop;
             };
             function GetAbsLeft(_obj) {
                 var _left = 0;
                 var _parent = _obj;
                 _left += _parent.offsetLeft;
                 _left += _parent.clientLeft;
                 do {
                     _parent = _parent.offsetParent;
                     _left += _parent.offsetLeft;
                     _left += _parent.clientLeft;
                 } while (_parent !== document.body);
                 return _left - document.body.scrollLeft;
             };
             function GetAbsCoords(_obj) {
                 var _top = 0;
                 var _left = 0;
                 var _parent = _obj;
                 _top += _parent.offsetTop;
                 _top += _parent.clientTop;
                 _left += _parent.offsetLeft;
                 _left += _parent.clientLeft;
                 do {
                     _parent = _parent.offsetParent;
                     _top += _parent.offsetTop;
                     _top += _parent.clientTop;
                     _left += _parent.offsetLeft;
                     _left += _parent.clientLeft;
                 } while (_parent !== document.body);
                 return [_top - document.body.scrollTop, _left - document.body.scrollLeft];
             }


             //apiCal("cc");
             /*function newCal(btn,txt,dates){

           
            
             function dateIsSpecial(year, month, day) {
             //SPECIAL_DAYS = sp[i];
             var m = SPECIAL_DAYS[month];
                
             if (!m) return false;
                
             for (var i in m) if (m[i] == day)
             {
                 
             return true;
             }
             return false;
             };
             function ourDateStatusFunc(date, y, m, d) {
             var today = new Date();
             today.setHours(0,0,0,0);
             if (dateIsSpecial(y, m, d))
             return true
             else
             if (date < today) return true;
             return false; // other dates are enabled
             // return true if you want to disable other dates
             };
             /*function onSelect(Calendar){
             Calendar.hide();
             updatePanel();
             };

   Calendar.setup(
             {
             inputField    : txt,         // ID of the input field
             ifFormat      : "%d.%m.%Y",    // the date format
             button        : btn,       // ID of the button
             firstDay      : 1,
             weekNumbers   : false,
             electric      : true,
             dateStatusFunc: ourDateStatusFunc,
             align         : 'TL',
             onSelect    :  onSelect
             });
             };*/
                
     </script>
    
</body>
</html>
