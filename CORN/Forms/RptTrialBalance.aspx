<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="RptTrialBalance.aspx.cs" Inherits="Forms_RptTrialBalance" Title="CORN :: Trial Balance" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
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
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Report Type</label>
                            <dx:ASPxComboBox ID="ddlReportType" runat="server" CssClass="form-control">
                                <Items>
                                    <dx:ListEditItem Value="1" Text="Detail" Selected="true" />
                                    <dx:ListEditItem Value="2" Text="Summary" />
                                </Items>
                            </dx:ASPxComboBox>
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
                                    <asp:CheckBoxList ID="ChbDistributorList" onclick="UnCheckSelectAll()" runat="server" Width="300px" Font-Size="11pt">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </div>
                        </div>
                    <div class="row">
                        <div class="col-md-1">

                            <label><span class="fa fa-caret-right rgt_cart"></span>Level</label>
                            <dx:ASPxComboBox ID="DrpLevel" runat="server" CssClass="form-control">
                                <Items>
                                    <dx:ListEditItem Value="4" Text="4" Selected="true"></dx:ListEditItem>
                                    <dx:ListEditItem Value="3" Text="3"></dx:ListEditItem>
                                    <dx:ListEditItem Value="2" Text="2"></dx:ListEditItem>
                                    <dx:ListEditItem Value="1" Text="1"></dx:ListEditItem>
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Main Account</label>
                            <dx:ASPxComboBox ID="DrpMainAccount" runat="server" CssClass="form-control">
                            </dx:ASPxComboBox>
                        </div>

                    </div>


                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>From Account Head</label>
                            <dx:ASPxComboBox ID="ddlFromAccount" runat="server" CssClass="form-control">
                            </dx:ASPxComboBox>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>To Account Head</label>
                            <dx:ASPxComboBox ID="ddlToAccount" runat="server" CssClass="form-control">
                            </dx:ASPxComboBox>                            
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>From Date</label>
                            &nbsp;<asp:TextBox ID="txtStartDate" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 27px">
                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>To Date</label>
                            &nbsp;<asp:TextBox ID="txtEndDate" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 27px">
                            <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                    </div>

                    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                    <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                        TargetControlID="txtStartDate"></cc1:CalendarExtender>
                    <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                        TargetControlID="txtEndDate"></cc1:CalendarExtender>                    
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
