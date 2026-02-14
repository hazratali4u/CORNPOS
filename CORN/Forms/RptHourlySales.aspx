<%@ Page Title="CORN :: Hourly Sale Report" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="RptHourlySales.aspx.cs" Inherits="Forms_RptHourlySales" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }

        function CheckBoxListSelect() {
            var chkBoxList = document.getElementById('<%= cblHour.ClientID %>');
            var chkBox = document.getElementById('<%= ChbSelectAll.ClientID %>');
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
        function UnCheckSelectAll() {
            var chkBox = document.getElementById('<%= ChbSelectAll.ClientID %>');
            var chkBoxList = document.getElementById('<%= cblHour.ClientID %>');
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

        function CheckBoxListSelectItem() {
            var chkBoxList = document.getElementById('<%= cblItem.ClientID %>');
            var chkBox = document.getElementById('<%= cbItemAll.ClientID %>');
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
        function UnCheckSelectAllItem() {
            var chkBox = document.getElementById('<%= cbItemAll.ClientID %>');
            var chkBoxList = document.getElementById('<%= cblItem.ClientID %>');
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
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:RadioButtonList ID="rbReportType" runat="server" Width="300px" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rbReportType_SelectedIndexChanged">
                                <asp:ListItem Text="Detail" Value="0" Selected="True" />
                                <asp:ListItem Text="Summary" Value="1" />
                                <asp:ListItem Text="Item Wise" Value="2" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                            <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Hour</label>                            
                                <asp:CheckBox ID="ChbSelectAll" runat="server" onclick="CheckBoxListSelect()" Width="100%" Font-Size="11pt" Text="SelectAll" Checked="true"
                                    AutoPostBack="true"></asp:CheckBox>
                                <asp:Panel ID="Panel1" runat="server" Width="100%" Height="170px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                    <asp:CheckBoxList ID="cblHour" onclick="UnCheckSelectAll()"  runat="server" Width="100%" Font-Size="11pt"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                        </div>
                        <div class="col-md-4" runat="server" id="dvItem" visible="false">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Item</label>                            
                                <asp:CheckBox ID="cbItemAll" runat="server" onclick="CheckBoxListSelectItem()" Width="100%" Font-Size="11pt" Text="SelectAll" Checked="true"
                                    AutoPostBack="true"></asp:CheckBox>
                                <asp:Panel ID="Panel2" runat="server" Width="100%" Height="170px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                    <asp:CheckBoxList ID="cblItem" onclick="UnCheckSelectAllItem()"  runat="server" Width="100%" Font-Size="11pt"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                        </div>
                    </div>
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
                    <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                        TargetControlID="txtStartDate" OnClientShown="calendarShown" PopupPosition="TopRight"></cc1:CalendarExtender>
                    <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                        TargetControlID="txtEndDate" PopupPosition="TopRight" OnClientShown="calendarShown"></cc1:CalendarExtender>

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
