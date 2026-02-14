<%@ Page Title="CORN :: Items Sale Report" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="RptItemWiseSales.aspx.cs" Inherits="Forms_RptItemWiseSales" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        } function ValidateForm() {

            return true;
        }

        function CheckBoxListSelect() {
            var chkBoxList = document.getElementById('<%= ChbDistributorList.ClientID %>');
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
            var chkBoxList = document.getElementById('<%= ChbDistributorList.ClientID %>');
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
            var chkBox = document.getElementById('<%= cbItem.ClientID %>');
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
            var chkBox = document.getElementById('<%= cbItem.ClientID %>');
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

        function CheckBoxListSelectService() {
            var chkBoxList = document.getElementById('<%= cblService.ClientID %>');
            var chkBox = document.getElementById('<%= cbServiceAll.ClientID %>');
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

        function UnCheckSelectAllService() {
            var chkBox = document.getElementById('<%= cbServiceAll.ClientID %>');
            var chkBoxList = document.getElementById('<%= cblService.ClientID %>');
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
                    <asp:Panel ID="pnl_rpt" runat="server">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:RadioButtonList ID="rbReportType" OnSelectedIndexChanged="rbReportType_SelectedIndexChanged"
                                    runat="server" Width="300px" RepeatDirection="Horizontal" AutoPostBack="true">
                                    <asp:ListItem Text="Item Wise" Value="0" Selected="True" />
                                    <asp:ListItem Text="Deal Wise" Value="1" />
                                    <asp:ListItem Text="Category Wise" Value="2" />
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                <br>
                                <asp:CheckBox ID="ChbSelectAll" onclick="CheckBoxListSelect()" runat="server" Width="100%"
                                    Font-Size="11pt" Text="SelectAll" Checked="true"></asp:CheckBox>
                                <asp:Panel ID="Panel1" runat="server" Width="320px" Height="170px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                    <asp:CheckBoxList ID="ChbDistributorList" onclick="UnCheckSelectAll()" runat="server" Width="300px" Font-Size="11pt"
                                        AutoPostBack="true" OnSelectedIndexChanged="ChbDistributorList_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </div>
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Service Type</label>
                                <br>
                                <asp:CheckBox ID="cbServiceAll" onclick="CheckBoxListSelectService()" runat="server" Width="100%"
                                    Font-Size="11pt" Text="SelectAll" Checked="true"></asp:CheckBox>
                                <asp:Panel ID="Panel4" runat="server" Width="320px" Height="170px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                    <asp:CheckBoxList ID="cblService" onclick="UnCheckSelectAllService()" runat="server" Width="300px" Font-Size="11pt">
                                        <asp:ListItem Value="1" Text="Dine In" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Delivery" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Takeaway" Selected="True"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row" id="rowCashier" runat="server">
                            <div class="col-md-4">
                                <sapn class="fa fa-caret-right rgt_cart"></sapn>
                                <asp:Label ID="lblCashier" runat="server" Text="Cashier" Font-Bold="true"></asp:Label>
                                <dx:ASPxComboBox ID="ddlCashier" runat="server" CssClass="form-control" Width="320px">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Category</label>
                                <br>
                                <asp:CheckBox ID="cbCategory" runat="server" Width="100%" Font-Size="11pt" Text="SelectAll" Checked="true"
                                    AutoPostBack="true" OnCheckedChanged="cbCategory_CheckedChanged"></asp:CheckBox>
                                <asp:Panel ID="Panel3" runat="server" Width="320px" Height="170px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                    <asp:CheckBoxList ID="cblCategory" runat="server" Width="300px" Font-Size="11pt"
                                        AutoPostBack="true" OnSelectedIndexChanged="cblCategory_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </div>
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Item</label>
                                <br>
                                <asp:CheckBox ID="cbItem" onclick="CheckBoxListSelectItem()" runat="server" Width="100%" Font-Size="11pt" Text="SelectAll" Checked="true"></asp:CheckBox>
                                <asp:Panel ID="Panel2" runat="server" Width="320px" Height="170px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                    <asp:CheckBoxList ID="cblItem" onclick="UnCheckSelectAllItem()" runat="server" Width="300px" Font-Size="11pt">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </div>

                        </div>
                        <div class="row" runat="server" id="itemdealRow">
                            <div class="col-md-4">
                                <asp:CheckBox runat="server" Checked="false" Text=" Including Deal" ID="chkIncludeDeal" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <label><span class="fa fa-caret-right rgt_cart"></span>From Date</label>
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" MaxLength="10"
                                    onkeyup="BlockStartDateKeyPress()"></asp:TextBox>
                            </div>
                            <div class="col-md-1" style="margin-top: 27px">
                                <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                    Width="30px" />
                            </div>
                            <div class="col-md-3">
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
                            TargetControlID="txtStartDate" OnClientShown="calendarShown" PopupPosition="TopLeft"></cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                            TargetControlID="txtEndDate" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-md-3">
                <asp:Button ID="btnViewPDF" runat="server" CssClass=" btn btn-success" Text="View PDF"
                    OnClick="btnViewPDF_Click" />
                <asp:Button ID="btnViewExcel" runat="server" CssClass="btn btn-success" Text="View Excel"
                    OnClick="btnViewExcel_Click" />
            </div>
        </div>
    </div>
</asp:Content>
