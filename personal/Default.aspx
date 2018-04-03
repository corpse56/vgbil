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
        #form1
        {}
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
                      <asp:UpdatePanel runat="server" ID="UpdatePanel1" 
            UpdateMode="Conditional" >
                      <ContentTemplate>
                      
                                              <asp:Button ID="load" runat="server" OnClick="load_Click" 
                                                  style="z-index:-90; display:none" Text="Button" Visible="true" />

          <table style="width:100%">
          <tr>
          <td>
            <div style="width:100%;text-align:center" >
                <asp:Label ID="Label1" runat="server" Text="Личный кабинет сотрудника" Font-Bold="True" Font-Size = "Large" Width="265px"></asp:Label>
            </div>
            <br />
          </td>
          </tr>
          <tr>
              <td style="height: 356px">  
        
                  <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                       OnActiveTabChanged="TabContainer1_ActiveTabChanged" 
                     OnClientActiveTabChanged="actTabChng"
                       ScrollBars="None"  AutoPostBack = "true"
                      Style="left: 0px; top: 0px;visibility:visible; " 
                      Visible="True" Width="95%" Height = "580">
                      <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Заказ книг из основного фонда"><ContentTemplate>
                              <asp:Panel ID="Panel1" runat="server" Height="500px" ScrollBars="Auto" BorderWidth = "1px">
                                  <table id="table3" style="z-index: 107; left: 9px; top: 66px;" width="99%">
                                      <tr>
                                          <td style="height: 26px; width: 43%;" valign="top">
                                              <asp:Table ID="Table5" runat="server" BorderStyle="Solid" GridLines="Both" 
                                                  style="color:Black;table-layout:inherit;border-color:Black;border-width:3px;border-style:Solid;z-index: 100; width: 100%;">
                                                  <asp:TableRow runat="server">
                                                      <asp:TableCell runat="server" RowSpan="3"></asp:TableCell>
                                                      <asp:TableCell runat="server" ColumnSpan="5"></asp:TableCell>
                                                      <asp:TableCell runat="server" RowSpan="3"></asp:TableCell>
                                                  </asp:TableRow>
                                                  <asp:TableRow runat="server">
                                                      <asp:TableCell runat="server"></asp:TableCell>
                                                      <asp:TableCell runat="server"></asp:TableCell>
                                                      <asp:TableCell runat="server"></asp:TableCell>
                                                      <asp:TableCell runat="server"></asp:TableCell>
                                                  </asp:TableRow>
                                                  <asp:TableRow runat="server">
                                                      <asp:TableCell runat="server"></asp:TableCell>
                                                      <asp:TableCell runat="server"></asp:TableCell>
                                                      <asp:TableCell runat="server"></asp:TableCell>
                                                      <asp:TableCell runat="server"></asp:TableCell>
                                                  </asp:TableRow>
                                                  <asp:TableRow runat="server">
                                                      <asp:TableCell runat="server" ColumnSpan="7"></asp:TableCell>
                                                  </asp:TableRow>
                                              </asp:Table>
                                            
                                          </td>
                                          <td valign="top" class="style1">
                                          </td>
                                          <td style="height: 26px; width: 43%;" valign="top">
                                              
                                              <asp:Table ID="Table2" runat="server" BorderStyle="Solid" GridLines="Both" 
                                                  style=" table-layout:inherit; border-color:Black;border-width:3px;border-style:Solid;z-index:100;width:100%; color: Black;">
                                              </asp:Table>
                                            
                                          </td>
                                      </tr>
                                      <tr>
                                          <td valign="top" >
                                              <asp:Button ID="Button3" runat="server" style="display:none;" Text="Button" />
                                              
                                          </td>
                                          <td valign="top" class="style1">
                                              
                                          </td>
                                          <td style="height: 26px; width: 43%;" valign="top">
                                              <asp:Table ID="Table1" runat="server" BorderStyle="Solid" GridLines="Both" 
                                                  style="table-layout:inherit;border-color:Black;border-width:3px;border-style:Solid;z-index: 100; width: 100%;">
                                              </asp:Table>
                                          </td>
                                      </tr>
                                  </table>
                              </asp:Panel>
                              <table>
                                  <tr>
                                      <td style="height: 26px; width: 43%;" valign="top">
                                        
                                         
                                          <asp:Button ID="Button1" runat="server" onclick="Button1_Click1" Text="Заказать выделенные" style="height: 25px"  /> &nbsp;&nbsp;
                                          <asp:Button ID="Button2" runat="server" Text="Отметить все" onclick="Button2_Click2"  />&nbsp;&nbsp;
                                          <asp:Button ID="Button4" runat="server" Text="Сформировать список БО" 
                                              onclick="Button4_Click"  OnClientClick="aspnetForm.target ='_blank';" />
                                          <asp:Button ID="Button5" runat="server" Text="Очистить корзину" 
                                              onclick="Button5_Click"  />&nbsp;&nbsp;
                                          <asp:Button ID="Button6" runat="server" Text="Удалить выбранные" 
                                              onclick="Button6_Click"  />&nbsp;&nbsp;
                                              
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
     /*function doPostBack(eventTarget, eventArgument) {
        if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
        theForm.__EVENTTARGET.value = eventTarget;
        theForm.__EVENTARGUMENT.value = eventArgument;
        alert('fghfghfgh');
        form1.submit();
        
        }
    }*/
        function ch()
        {
            var prov = document.getElementById('TabContainer1_TabPanel1_сtb0');
            alert(prov.value);
        }
        function isDateSelected()
        {
            //alert("ALARM!!!");
            var chs;
            var ctbs
            for (i = 0; i < id3.length; i++)
            {
                chs = document.getElementById(id3[i]);
                //alert(chs.checked);
                ctbs = document.getElementById(id2[i]);
                //alert(ctbs.value);
                if ((chs.checked) && (ctbs.value ==''))
                {
                    alert('Введите дату!');
                    
                    break;
                }
                else
                {
                    //alert(ctbs.value);
                    continue;    
                }
            }
        }
        function actTabChng()
        {
            //var load = document.getElementById("TabContainer1_TabPanel1_load");
            var load = document.getElementById("load");
            //load.click();
            
            //alert(load.nodeName);
            
            __doPostBack('TabContainer1$TabPanel1$Button3','');
        }
        // Sys.Application.notifyScriptLoaded();
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler); 
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler); 
        var y;
        function BeginRequestHandler(sender, args)
                {
                            var scroll__ = document.getElementById('TabContainer1_TabPanel1_Panel1');
                            y = scroll__.scrollTop;   
                            //alert(y);
                }
        function EndRequestHandler(sender, args)
                {
                            var scroll__ = document.getElementById('TabContainer1_TabPanel1_Panel1');
                            scroll__.scrollTop = y;   
                           // alert(y);
                }
        function updatePanel()
                {
                            var scroll_ = document.getElementById('TabContainer1_TabPanel1_Panel1');
                            y = scroll_.scrollTop;
                            //alert(y);
                            var load = document.getElementById("load");
                            load.click();
                            
                             // это капец!  :( 
                }
                
                
                //alert(startDate.getDate());
                //alert(startDate);
        var cl = new Calendar(1,Date(),onSelect,onClose);
        
        var but = '';
        var txt = '';
        var dat = '';
        var SPECIAL_DAYS;
        
        function apiCal(butt,txtt,datt){
        //cl = new Calendar(1,datt,onSelect,onClose);
        cl.setDateStatusHandler(DateStatusFunc);
        cl.create();
        cl.setDateFormat("%d.%m.%Y");
        //alert('cr');
        //cl.callCloseHandler();
        //cl.callCloseHandler();
        
        //var input_field = document.getElementById(txtt);
        //alert(datt);
        //input_field.value = datt;
        
        //if (butt != null)
        //alert(butt);
        var XY = document.getElementById(butt);
        //alert(butt);
        var XX = XY.offsetLeft
        var YY = XY.offsetTop
        //alert(XY.offsetTop);
        var par = XY.offsetParent
        //alert(par);
        //var node = XY.parentNode;
        
        while (par !== document.body)
        {
            //alert(par);
            /*if (par.id == "TabContainer1_TabPanel1_Panel1")
            {
                YY -= par.scrollTop;
                alert(par.scrollTop);
                YY += par.offsetTop;
                XX += par.offsetLeft;
                par = par.offsetParent;
                continue;
            //alert(par.scrollTop);
            };*/
            //alert(par.scrollTop);
            XX += par.offsetLeft
            YY += par.offsetTop
            //alert(par.offsetTop+'  '+par.id);
           // alert(node.scrollTop+'  '+ node.id);
            //node = node.parentNode;
            //if (par.offsetParent == null) break;
            par = par.offsetParent
            //alert(par);
            //alert(YY+' ' +par.id+' '+par.scrollTop);
//            alert(YY +'  '+ par.id);
        }  
        //var bodytop = document.getElementById('fff');
            //alert(bodytop.offsetTop);
        
            
        var scroll_ = document.getElementById("TabContainer1_TabPanel1_Panel1");
        cl.showAt(XX,YY-scroll_.scrollTop);
        cl.show();
        but = butt;
        txt = txtt;
        dat = datt;
       //onSelect(cl,Date());
       var startDate = new Date();
                while (dateIsSp(startDate.getFullYear(),startDate.getMonth(),startDate.getDate()) )
                {
                    startDate.setDate(startDate.getDate()+1);
                }
                //alert(startDate);
                
                cl.setDate(startDate);
        };
          var dateInField;
          
          function onSelect(cl,date) {
                //alert('select');
                if (!cl.dateClicked)
                {
                //alert(date);
                }
                else
                {
                //alert(date);
                var input_field = document.getElementById(txt);
                input_field.value = date;
                var test = new Date();//test.setDate
                test.setFullYear(test.getFullYear()+1);
                if (date>test) 
                {
                    //alert(date);
                    date = new Date();
                    input_field.value = date;
                    dateInField = date;
                    
                }
                }
                //updatePanel();
                //alert(date);
               // alert(y);
                if (cl.dateClicked)
                 //{
                 //alert('close');
                    cl.callCloseHandler();  
                //}
                 //   alert(y);
          };
          function onClose(cl){
                //alert(dateInField);
               /* var input_field = document.getElementById(txt);
                var test = new Date();
                test.setFullYear(test.getFullYear()+1);
                if (dateInField>test) dateInField = new Date();
                input_field.value = dateInField;*/
                
                cl.destroy();
          };
          function dateIsSp(year, month, day) {
                
                var m = SPECIAL_DAYS[month];
                
                if (!m) return false;
                
                for (var i in m) if (m[i] == day)
                {
                 
                 return true;
                }
                return false;
          };
          function DateStatusFunc(date, y, m, d) {
                var today = new Date();
                var test = new Date();
                today.setHours(0,0,0,0);
                test.setFullYear(test.getFullYear()+1);
                //alert(test);
                if (dateIsSp(y, m, d))
                    return true
                else
                    if ( (date < today) || (date>=test) ) return true;
                    return false; // other dates are enabled
              // return true if you want to disable other dates
              //cl.clicked = true;
          };
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
    <p>
                                            <%--  <asp:Button runat="server" Text="Button" 
            ID="Button2" onclick="Button2_Click1"></asp:Button>--%>

                                          </p>
    
    </body>
</html>
