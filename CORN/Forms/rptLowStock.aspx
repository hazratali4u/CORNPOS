<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="rptLowStock.aspx.cs" Inherits="Forms_rptLowStock"
    Title="CORN :: Low Stock Alert" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }

        function CheckBoxListSelect() {
            var chkBoxList = document.getElementById('<%= cblCategory.ClientID %>');
            var chkBox = document.getElementById('<%= cbCategory.ClientID %>');
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
            var chkBox = document.getElementById('<%= cbCategory.ClientID %>');
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
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                            <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control"></dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:CheckBox ID="cbCategory" runat="server" Text="All Categories" onclick="CheckBoxListSelect()"  Checked="true" ></asp:CheckBox>
                                <br />
                                <asp:Panel ID="pnlCategory" runat="server" Height="170px" ScrollBars="Vertical" BorderStyle="Groove" BorderWidth="1px">
                                    <asp:CheckBoxList ID="cblCategory" runat="server" onclick="UnCheckSelectAll()"></asp:CheckBoxList>
                                </asp:Panel>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:CheckBox ID="cbLowStock" runat="server" Checked="true" Text="Only Show Low Stock"/>
                        </div>
                    </div>                                        
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-md-offset-1 col-md-3 ">
                <div class="btnlist pull-right">                    
                <asp:Button ID="btnViewPDF" runat="server" OnClick="btnViewPDF_Click" Text="View PDF"  CssClass=" btn btn-success" />
                <asp:Button ID="btnViewExcel" runat="server" OnClick="btnViewExcel_Click" Text="View Excel" CssClass=" btn btn-success" />
            </div>
                </div>
        </div>
       
    </div>
</asp:Content>
