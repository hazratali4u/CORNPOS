<%@ Page Title="CORN :: Time Card Report" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="rptTimeCard.aspx.cs" Inherits="Forms_rptTimeCard" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
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
                            <div class="col-md-8">
                                <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control"
                                    AutoPostBack="true" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Department</label>
                                <br>
                                <asp:CheckBox ID="cbhAllDepartment" runat="server" Text="All Departments" Checked="true" AutoPostBack="true"
                                    OnCheckedChanged="cbhAllDepartment_CheckedChanged"></asp:CheckBox>
                                <br />
                                <asp:Panel ID="pnlDepartment" runat="server" Height="150px" ScrollBars="Vertical" BorderStyle="Groove" BorderWidth="1px">
                                    <asp:CheckBoxList ID="cblDepartment" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="cblDepartment_SelectedIndexChanged"></asp:CheckBoxList>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                &nbsp;
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Employee</label>
                                <br>
                                <asp:CheckBox ID="cbAllEmployee" runat="server" Text="All Employees" Checked="true" AutoPostBack="true"
                                    OnCheckedChanged="cbAllEmployee_CheckedChanged"></asp:CheckBox>
                                <br />
                                <asp:Panel ID="Panel1" runat="server" Height="150px" ScrollBars="Vertical" BorderStyle="Groove" BorderWidth="1px">
                                    <asp:CheckBoxList ID="cbEmployee" runat="server"></asp:CheckBoxList>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                &nbsp;
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Month</label>
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" MaxLength="10"
                                    onkeyup="BlockStartDateKeyPress()"></asp:TextBox>
                            </div>
                            <div class="col-md-1" style="margin-top: 27px">
                                <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                    Width="30px" />
                            </div>
                        </div>
                        <cc1:CalendarExtender ID="CESEndMonth" runat="server" BehaviorID="calendar4"
                            OnClientShown="onCalendarShown4" OnClientHidden="onCalendarHidden4" Format="MMM-yyyy"
                            PopupButtonID="ibtnStartDate" TargetControlID="txtStartDate">
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
