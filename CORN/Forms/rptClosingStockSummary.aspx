<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="rptClosingStockSummary.aspx.cs" Inherits="Forms_rptClosingStockSummary"
    Title="CORN :: Closing Stock Summary" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
        function CheckBoxListSelectSection() {
            var chkBoxList = document.getElementById('<%= chblSection.ClientID %>');
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
            var chkBoxList = document.getElementById('<%= chblSection.ClientID %>');
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

        function CheckBoxListSelectLocation() {
            var chkBoxList = document.getElementById('<%= drpDistributor.ClientID %>');
            var chkBox = document.getElementById('<%= chbAllLocation.ClientID %>');
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
        function UnCheckSelectAllLocation() {
            var chkBox = document.getElementById('<%= chbAllLocation.ClientID %>');
            var chkBoxList = document.getElementById('<%= drpDistributor.ClientID %>');
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
                    <div class="row">
                        <div class="col-md-4">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:CheckBox ID="chbAllLocation" onclick="CheckBoxListSelectLocation()" CssClass="SingleCheckbox1" runat="server" Width="100%"
                                        Font-Size="10pt" Text="Location"></asp:CheckBox>
                                    <asp:Panel ID="Panel1" runat="server" Width="95%" Height="350px" ScrollBars="Vertical"
                                        BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                        <asp:CheckBoxList Style="padding-left:4px" ID="drpDistributor" CssClass="SingleCheckbox1" onclick="UnCheckSelectAllLocation()" margin-left="15px" runat="server" Width="236px" Font-Size="14px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                           
                        </div>

                        <div class="col-md-3" runat="server" id="sectionRow">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:CheckBox ID="chbAllSections" onclick="CheckBoxListSelectSection()" CssClass="SingleCheckbox1" runat="server" Width="100%"
                                        Font-Size="10pt" Text="Section"></asp:CheckBox>
                                    <asp:Panel ID="Panel3" runat="server" Width="265px" Height="350px" ScrollBars="Vertical"
                                        BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                        <asp:CheckBoxList Style="padding-left:4px" ID="chblSection" CssClass="SingleCheckbox1" onclick="UnCheckSelectAllSection()" margin-left="15px" runat="server" Width="236px" Font-Size="14px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="col-md-3">
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

                            <div class="row">
                                <div class="col-md-offset-0 col-md-12 ">
                                    <div class="btnlist pull-center" style="padding:15px;">
                                        <asp:Button ID="btnViewPDF" runat="server" CssClass=" btn btn-success" Text="View PDF"
                                            OnClick="btnViewPDF_Click" /> &nbsp;&nbsp;
                                        <asp:Button ID="btnViewExcel" runat="server" CssClass="btn btn-success" Text="View Excel"
                                            OnClick="btnViewExcel_Click" />
                                    </div>
                                </div>

                            </div>

                            <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                                TargetControlID="txtStartDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                TargetControlID="txtEndDate" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>
                        </div>
                    </div>
        </div>
        
        
    </div>
</asp:Content>
