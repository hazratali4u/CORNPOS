<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptStockDemandvsTransferOut.aspx.cs" Inherits="Forms_RptStockDemandvsTransferOut" Title="CORN :: Stock Demand vs Transfer Out" %>
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
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-8">
                                    <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Transfer From</label>
                                    <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                                    </dx:ASPxComboBox>
                                </div>
                                </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Transfer To</label>
                                    <dx:ASPxComboBox ID="DrpTransferTo" runat="server" CssClass="form-control">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>From Date</label>
                                    <asp:TextBox ID="txtStartDate" runat="server"
                                        CssClass="form-control" MaxLength="10"></asp:TextBox>
                                </div>
                                <div class="col-md-1" style="margin-top: 27px">
                                    <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                        Width="27px" />
                                </div>
                                </div>
                            <div class="row">
                                 <div class="col-md-3">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>To Date</label>
                                    <asp:TextBox ID="txtEndDate" runat="server"
                                        CssClass="form-control" MaxLength="10"></asp:TextBox>
                                </div>
                                <div class="col-md-1" style="margin-top: 27px">
                                    <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                        Width="27px" />
                                </div>
                            </div>
                            
                            <div class="row">
                                <div class="col-md-8 ">
                                    <div class="btnlist">
                                        <asp:Button ID="btnViewPDF" runat="server" OnClick="btnViewPDF_Click" Text="View PDF" CssClass=" btn btn-success" />
                                        <asp:Button ID="btnViewExcel" Style="margin-right: 25px;" runat="server" OnClick="btnViewExcel_Click" Text="View Excel" CssClass=" btn btn-success" />
                                    </div>
                                </div>
                            </div>
                            <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                PopupPosition="BottomLeft" TargetControlID="txtStartDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                TargetControlID="txtEndDate" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>
                        </div>
                                   
                     
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnViewPDF" />
                    <asp:PostBackTrigger ControlID="btnViewExcel" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="row">
        </div>
    </div>
</asp:Content>
