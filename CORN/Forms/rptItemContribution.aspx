<%@ Page Title="CORN :: Item Sales Contribution" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="rptItemContribution.aspx.cs" Inherits="Forms_rptItemContribution" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        } function ValidateForm() {

            return true;
        }

        function onCalendarShown3() {
            var cal = $find("calendar3");
            cal._switchMode("months", true);
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call3);
                    }
                }
            }
            sender._popupBehavior._element.style.zIndex = 10005;
        }

        function onCalendarHidden3() {
            var cal = $find("calendar3");

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call3);
                    }
                }
            }
        }
        function call3(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar3");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
        function onCalendarShown4() {
            var cal = $find("calendar4");
            cal._switchMode("months", true);
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call4);
                    }
                }
            }
            sender._popupBehavior._element.style.zIndex = 10005;
        }

        function onCalendarHidden4() {
            var cal = $find("calendar4");

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call4);
                    }
                }
            }
        }

        function call4(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar4");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    cal._switchMonth(target.date);
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
                    <asp:Panel ID="pnl_rpt" runat="server">
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Report Type</label>
                                <dx:ASPxComboBox ID="DrpReportType" runat="server" CssClass="form-control"
                                    AutoPostBack="true" OnSelectedIndexChanged="DrpReportType_SelectedIndexChanged">
                                    <Items>
                                        <dx:ListEditItem Value="1" Text="Date Wise" Selected="true" />
                                        <dx:ListEditItem Value="2" Text="Month Wise" />
                                    </Items>
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="Label5" runat="server" CssClass="lblbox" Text="Value Type" Width="78px"
                                    Visible="False"></asp:Label>
                                <asp:DropDownList ID="DrpUnitType" runat="server" Width="200px" CssClass="DropList"
                                    Visible="False">
                                    <asp:ListItem>Pieces</asp:ListItem>
                                    <asp:ListItem>Value</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                        <div id="divDate" runat="server">
                            <div class="row">
                                <div class="col-md-4">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>From Date</label>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" MaxLength="10"
                                        onkeyup="BlockStartDateKeyPress()"></asp:TextBox>
                                </div>
                                <div class="col-md-1" style="margin-top: 27px">
                                    <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                        Width="30px" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>To Date</label>
                                    <asp:TextBox ID="txtEndDate" runat="server" onkeyup="BlockEndDateKeyPress()"
                                        CssClass="form-control" MaxLength="10"></asp:TextBox>
                                </div>
                                <div class="col-md-1" style="margin-top: 27px">
                                    <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                        Width="30px" />
                                </div>
                            </div>
                        </div>
                        <div id="divMonth" runat="server" visible="false">
                            <div class="row">
                                <div class="col-md-4">
                                    <sapn class="fa fa-caret-right rgt_cart"></sapn>
                                    <asp:Label ID="lblFromMonth" runat="server" Text="From Month" Font-Bold="true"></asp:Label>
                                    <asp:TextBox ID="txtFromMonth" onkeyup="BlockStartDateKeyPress()" runat="server"
                                        Enabled="true" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                </div>
                                <div class="col-md-1" style="margin-top: 27px">
                                    <asp:ImageButton ID="ibtnStartMonth" runat="server" Width="30px" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <sapn class="fa fa-caret-right rgt_cart"></sapn>
                                    <asp:Label ID="lblToMonth" runat="server" Text="To Month" Font-Bold="true"></asp:Label>
                                    <asp:TextBox ID="txtToMonth" onkeyup="BlockStartDateKeyPress()" runat="server"
                                        Enabled="true" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                </div>
                                <div class="col-md-1" style="margin-top: 27px">
                                    <asp:ImageButton ID="ibtnEndMonth" runat="server" Width="30px" ImageUrl="~/App_Themes/Granite/Images/date.gif"></asp:ImageButton>
                                </div>
                            </div>
                        </div>
                        <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                            TargetControlID="txtStartDate" OnClientShown="calendarShown" PopupPosition="TopLeft"></cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                            TargetControlID="txtEndDate" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="CEStartMonth" runat="server" BehaviorID="calendar3" OnClientShown="onCalendarShown3"
                            OnClientHidden="onCalendarHidden3" Format="MMM-yyyy" PopupButtonID="ibtnStartMonth" PopupPosition="TopLeft"
                            TargetControlID="txtFromMonth">
                        </cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="CESEndMonth" runat="server" BehaviorID="calendar4" OnClientShown="onCalendarShown4"
                            OnClientHidden="onCalendarHidden4" Format="MMM-yyyy" PopupButtonID="ibtnEndMonth" PopupPosition="TopLeft"
                            TargetControlID="txtToMonth">
                        </cc1:CalendarExtender>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-md-offset-1 col-md-3 ">
                <div class="btnlist pull-right">
                    <asp:Button ID="btnViewPDF" runat="server" CssClass=" btn btn-success" Text="View PDF"
                        OnClick="btnViewPDF_Click" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="btn btn-success" Text="View Excel"
                        OnClick="btnViewExcel_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
