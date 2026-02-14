<%@ Page Title="CORN :: Customer Ledger" Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="RptCustomerLedger.aspx.cs" Inherits="Forms_RptCustomerLedger" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }


    </script>

    <div class="main-contents">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>                    
                    <div class="row">
                        <div class="col-md-4">
                            <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" Width="100%"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                        <asp:ListItem Value="1" Text="Customer Ledger" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Franchise Ledger"></asp:ListItem>
                                    </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                            <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control"
                                OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" AutoPostBack="true">
                            </dx:ASPxComboBox>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Customer</label>
                            <dx:ASPxComboBox ID="drpCustomer" runat="server" CssClass="form-control">
                            </dx:ASPxComboBox>
                        </div>
                    </div>

                    <div class="row" id="divSortBy" runat="server" visible="false">
                        <div class="col-md-6">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Sort By</label>
                            <asp:RadioButtonList ID="RblSortBy" runat="server" Width="200px" RepeatDirection="Horizontal">
                                <asp:ListItem Value="0" Selected="True" Text="Customer" />
                                <asp:ListItem Value="1" Text="Invoice Date" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>From Date</label>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 27px">
                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>To Date</label>
                            <asp:TextBox ID="txtEndDate" runat="server"
                                CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 27px">
                            <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                    </div>

                    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                    <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                        TargetControlID="txtStartDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                    <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                        TargetControlID="txtEndDate" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-md-offset-1 col-md-3 ">
                <div class="btnlist pull-right">
                    <asp:Button ID="btnViewPDF" runat="server" CssClass=" btn btn-success" OnClick="btnViewPDF_Click" Text="View PDF" />
                    <asp:Button ID="btnViewExcel" runat="server" CssClass="btn btn-success" Text="View Excel" OnClick="btnViewExcel_Click" />
                </div>
            </div>

        </div>
    </div>
</asp:Content>

