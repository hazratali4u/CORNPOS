<%@ Page Title="CORN :: Net Cash Sales vs Cash Deposited" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="RptCashRegisterClosing.aspx.cs" Inherits="Forms_RptCashRegisterClosing" %>
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
                                <label><span class="fa fa-caret-right rgt_cart"></span>Cashier</label>
                                <dx:ASPxComboBox ID="drpUser" runat="server" CssClass="form-control">
                                </dx:ASPxComboBox>
                            </div>
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
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" MaxLength="10"
                                    onkeyup="BlockStartDateKeyPress()"></asp:TextBox>
                            </div>
                            <div class="col-md-1" style="margin-top: 27px">
                                <asp:ImageButton ID="ibtnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                    Width="30px" />
                            </div>
                        </div>
                        <cc1:CalendarExtender ID="CESEndMonth" runat="server"  Format="dd-MMM-yyyy"
                            PopupButtonID="ibtnStartDate" TargetControlID="txtStartDate">
                        </cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server"  Format="dd-MMM-yyyy"
                            PopupButtonID="ibtnEndDate" TargetControlID="txtEndDate">
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
