<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
     CodeFile="RptCustomerClosing.aspx.cs" Inherits="Forms_RptCustomerClosing"
     Title="CORN :: Customer Closing Balance" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }

        function CheckBoxListSelectSection() {
            var chkBoxList = document.getElementById('<%= drpDistributor.ClientID %>');
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
      
        function UnCheckSelectAllCustomer() {
            var chkBox = document.getElementById('<%= chbAllSections.ClientID %>');
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
        

        function CheckBoxListSelectCustomer() {
            var chkBoxList = document.getElementById('<%= DrpCustomer.ClientID %>');
            var chkBox = document.getElementById('<%= chbAllCustomer.ClientID %>');
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
      
        function UnCheckSelectAllCustomer() {
            var chkBox = document.getElementById('<%= chbAllCustomer.ClientID %>');
            var chkBoxList = document.getElementById('<%= DrpCustomer.ClientID %>');
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
                        <div class="col-md-8">
                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:RadioButtonList ID="rbCustomerType" Width="90%" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rbCustomerType_SelectedIndexChanged">
                                <asp:ListItem Value="1" Selected="True" Text="Customer" />
                                <asp:ListItem Value="2" Text="Franchise" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:RadioButtonList ID="RbReportType" Width="90%" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Selected="True" Text="Summary" />
                                <asp:ListItem Value="0" Text="Detail" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <sapn class="fa fa-caret-right rgt_cart"></sapn>

                            <asp:CheckBox ID="chbAllSections" OnCheckedChanged="chbAllLocation_CheckedChanged" AutoPostBack="true"
                                onclick="CheckBoxListSelectSection()" CssClass="SingleCheckbox1" runat="server" Width="100%"
                                Font-Size="10pt" Text="Location"></asp:CheckBox>
                            <asp:Panel ID="Panel3" runat="server" Width="300px" Height="200px" ScrollBars="Vertical"
                                BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                <asp:CheckBoxList ID="drpDistributor" CssClass="SingleCheckbox1" AutoPostBack="true"
                                     OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" onclick="UnCheckSelectAllSection()" margin-left="15px" runat="server" Width="236px" Font-Size="14px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </div>

                        <div class="col-md-4">
                            <sapn class="fa fa-caret-right rgt_cart"></sapn>

                            <asp:CheckBox ID="chbAllCustomer" onclick="CheckBoxListSelectCustomer()" CssClass="SingleCheckbox1" runat="server" Width="100%"
                                Font-Size="10pt" Text="Customer"></asp:CheckBox>
                            <asp:Panel ID="Panel1" runat="server" Width="300px" Height="200px" ScrollBars="Vertical"
                                BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                <asp:CheckBoxList ID="DrpCustomer" CssClass="SingleCheckbox1" onclick="UnCheckSelectAllCustomer()" margin-left="15px" runat="server" Width="236px" Font-Size="14px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">
                            <label><span class="fa fa-caret-right rgt_cart"></span>As on Date</label>
                            <asp:TextBox ID="txtStartDate" runat="server"
                                CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 27px">
                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                    </div>

                    <cc1:CalendarExtender ID="calendar1" ClientIDMode="Static" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                        PopupPosition="TopLeft" TargetControlID="txtStartDate"></cc1:CalendarExtender>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-md-3">
                <div class="btnlist pull-right">
                    <asp:Button ID="btnViewPDF" runat="server" OnClick="btnViewPDF_Click" Text="View PDF" CssClass=" btn btn-success" />
                    <asp:Button ID="btnViewExcel" runat="server" OnClick="btnViewExcel_Click" Text="View Excel" CssClass=" btn btn-success" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
