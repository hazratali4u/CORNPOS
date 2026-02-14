<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
     CodeFile="RptConsumptionMonthWise.aspx.cs" Inherits="Forms_RptConsumptionMonthWise"
     Title="CORN :: Month Wise Consumption" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }

        function CheckBoxListSelectSection() {
            var chkBoxList = document.getElementById('<%= drpDistributor.ClientID %>');
            var chkBox = document.getElementById('<%= chbAllSections.ClientID %>');
            var chkBoxCount;
            var i;
            if (chkBox.checked == true) {
                chkBoxCount = chkBoxList.getElementsByTagName("input");
                for (i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = true;
                }
            } else {
                chkBoxCount = chkBoxList.getElementsByTagName("input");
                for (i = 0; i < chkBoxCount.length; i++) {

                    chkBoxCount[i].checked = false;
                }
            }
        }
      
        function UnCheckSelectAllSection() {
            var chkBox = document.getElementById('<%= chbAllSections.ClientID %>');
            var chkBoxList = document.getElementById('<%= drpDistributor.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            var count = 0;
            for (var i = 0; i < chkBoxCount.length; i++) {
                if (chkBoxCount[i].checked == false) {
                    count += 1;
                }
            }
            if (count > 0) {
                chkBox.checked = false;
            }
            else {
                chkBox.checked = true;
            }
        }


        function onCalendarHidden() {
            var cal = $find("calendar1");

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function onCalendarShown() {

            var cal = $find("calendar1");

            cal._switchMode("months", true);
            cal._popupBehavior._element.style.zIndex = 10005;

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    //cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }


        function onCalendarHidden1() {
            var cal = $find("calendar2");

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call1);
                    }
                }
            }
        }

        function onCalendarShown1() {

            var cal = $find("calendar2");

            cal._switchMode("months", true);
            cal._popupBehavior._element.style.zIndex = 10005;

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call1);
                    }
                }
            }
        }

        function call1(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar2");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    //cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
        
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-8">
                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:RadioButtonList ID="RbReportType" Width="100%" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="2" Selected="True" Text="Category Wise" />
                                <asp:ListItem Value="1" Text="Location Wise" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <sapn class="fa fa-caret-right rgt_cart"></sapn>

                            <asp:CheckBox ID="chbAllSections" onclick="CheckBoxListSelectSection()" CssClass="SingleCheckbox1" runat="server" Width="100%"
                                Font-Size="10pt" Text="Location"></asp:CheckBox>
                            <asp:Panel ID="Panel3" runat="server" Width="255px" Height="200px" ScrollBars="Vertical"
                                BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                <asp:CheckBoxList ID="drpDistributor" CssClass="SingleCheckbox1" onclick="UnCheckSelectAllSection()" margin-left="15px" runat="server" Width="236px" Font-Size="14px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            <label><span class="fa fa-caret-right rgt_cart"></span>From Month</label>
                            <asp:TextBox ID="txtStartDate" runat="server"
                                CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 27px">
                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2">
                            <label><span class="fa fa-caret-right rgt_cart"></span>To Month</label>
                            <asp:TextBox ID="txtEndDate" runat="server" onkeyup="BlockEndDateKeyPress()"
                                CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 27px">
                            <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-3">
                            <sapn class="fa fa-caret-right rgt_cart"></sapn>
                            <asp:Label ID="Label1" runat="server" Text="Sub Type" Font-Bold="true"></asp:Label>
                            <dx:ASPxComboBox ID="drpSubType" runat="server" CssClass="form-control">
                                <Items>
                                    <dx:ListEditItem Text="By Amount" Value="1" Selected="true" />
                                    <dx:ListEditItem Text="By Qty" Value="2" />
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                    </div>

                    <cc1:CalendarExtender ID="calendar1" ClientIDMode="Static" runat="server" Format="MMM-yyyy" PopupButtonID="ibtnStartDate"
                        PopupPosition="TopLeft" TargetControlID="txtStartDate" OnClientShown="onCalendarShown" OnClientHidden="onCalendarHidden" DefaultView="Months"></cc1:CalendarExtender>
                    <cc1:CalendarExtender ID="calendar2" ClientIDMode="Static" runat="server" Format="MMM-yyyy" PopupButtonID="ibnEndDate"
                        TargetControlID="txtEndDate" PopupPosition="TopLeft" OnClientShown="onCalendarShown1" OnClientHidden="onCalendarHidden1" DefaultView="Months"></cc1:CalendarExtender>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-md-3">
                <div class="btnlist pull-right">
                    <asp:Button ID="btnViewPDF" runat="server" OnClick="btnViewPDF_Click" Text="View PDF" CssClass=" btn btn-success" />
                    <asp:Button ID="btnViewExcel" runat="server" OnClick="btnViewExcel_Click" Text="View Excel" CssClass=" btn btn-success" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
