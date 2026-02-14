<%@ Page Title="CORN :: Discounted Sale Invoices" Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="RptDiscountedSaleInvoices.aspx.cs" Inherits="Forms_RptDiscountedSaleInvoices" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }

        function CheckBoxListSelectSection() {
            var chkBoxList = document.getElementById('<%= ddlBankDiscount.ClientID %>');
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
            var chkBoxList = document.getElementById('<%= ddlBankDiscount.ClientID %>');
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
                            <div class="row">
                                <div class="col-md-12">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Report Type</label>
                                    <dx:ASPxComboBox ID="ddlType" runat="server" CssClass="form-control"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                        <Items>
                                            <dx:ListEditItem Value="1" Text="Discounted Invoices Sales" Selected="true" />
                                            <dx:ListEditItem Value="2" Text="Bank Discount" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Base Location</label>
                                    <asp:CheckBox ID="ChbSelectAll" AutoPostBack="true" OnCheckedChanged="ChbSelectAll_CheckedChanged" runat="server" Width="100%" Font-Size="11pt" Text="SelectAll" Checked="true"></asp:CheckBox>
                                    <asp:Panel ID="Panel1" runat="server" Width="320px" Height="170px" ScrollBars="Vertical"
                                        BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                        <asp:CheckBoxList ID="ChbDistributorList" AutoPostBack="true" OnSelectedIndexChanged="ChbDistributorList_SelectedIndexChanged" runat="server" Width="300px" Font-Size="11pt">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="row" runat="server" id="rowDiscountType">
                                <div class="col-md-12">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Discount Type</label>
                                    <dx:ASPxComboBox ID="ddlDiscountType" runat="server" CssClass="form-control">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row" runat="server" id="lblCashier">
                                <div class="col-md-12">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Cashier</label>
                                    <dx:ASPxComboBox ID="ddUser" runat="server" CssClass="form-control">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-10">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>From Date</label>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                </div>
                                <div class="col-md-2" style="margin-top: 27px">
                                    <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                        Width="30px" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-10">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>To Date</label>
                                    <asp:TextBox ID="txtEndDate" runat="server"
                                        CssClass="form-control" MaxLength="10"></asp:TextBox>
                                </div>
                                <div class="col-md-2" style="margin-top: 27px">
                                    <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                        Width="30px" />
                                </div>
                            </div>
                            <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                            <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                TargetControlID="txtStartDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                TargetControlID="txtEndDate" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>
                        </div>
                        <div class="col-md-4" runat="server" id="lblBankDiscount" visible="false">
                            <div class="row">
                                <div class="col-md-12">
                                    <sapn class="fa fa-caret-right rgt_cart"></sapn>
                                    <asp:CheckBox ID="chbAllSections" onclick="CheckBoxListSelectSection()" CssClass="SingleCheckbox1" runat="server" Width="100%"
                                        Font-Size="10pt" Text="Bank Discount"></asp:CheckBox>
                                    <asp:Panel ID="Panel3" runat="server" Width="255px" Height="255px" ScrollBars="Vertical"
                                        BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                        <asp:CheckBoxList ID="ddlBankDiscount" CssClass="SingleCheckbox1" onclick="UnCheckSelectAllSection()" margin-left="15px" runat="server" Width="236px" Font-Size="14px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-md-offset-1 col-md-3 ">
                <div class="btnlist pull-right">
                    <asp:Button ID="btnViewPDF" runat="server" OnClick="btnViewPDF_Click" CssClass=" btn btn-success" Text="View PDF" />
                    <asp:Button ID="btnViewExcel" runat="server" OnClick="btnViewExcel_Click" CssClass="btn btn-success" Text="View Excel" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>