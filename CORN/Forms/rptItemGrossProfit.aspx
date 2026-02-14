<%@ Page Title="CORN :: Item Wise Gross Profit Report" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="rptItemGrossProfit.aspx.cs" Inherits="Forms_rptItemGrossProfit" %>

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
            var chkBoxList = document.getElementById('<%= cblCategory.ClientID %>');
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
            var chkBoxList = document.getElementById('<%= cblCategory.ClientID %>');
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
                            <div >
                                <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-10">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-10">
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
                            <div class="col-md-10">
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
                                <div class="col-md-6">
                                    <strong>
                                <asp:Label ID="Label11" runat="server" Width="100%" Font-Bold="True"
                                    Text="Category"></asp:Label></strong>
                            <br>
                            <asp:CheckBox ID="ChbSelectAll" onclick="CheckBoxListSelect()" runat="server" Width="100%"
                                Font-Size="11pt" Text="Select All" Checked="true"></asp:CheckBox>
                            <asp:Panel ID="Panel1" runat="server" Width="320px" Height="250px" ScrollBars="Vertical"
                                BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                <asp:CheckBoxList ID="cblCategory" onclick="UnCheckSelectAll()" runat="server" Width="300px" Font-Size="11pt">
                                </asp:CheckBoxList>
                            </asp:Panel>
                                </div>
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
