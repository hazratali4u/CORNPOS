<%@ Page Title="CORN :: Item Recipe" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="rptItemRecipe.aspx.cs" Inherits="Forms_rptItemRecipe" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        } function ValidateForm() {

            return true;
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

    </script>
    <div class="main-contents"  style="height:200px;">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnl_rpt" runat="server">
                        <div class="row">
                            <div class="col-md-4">
                                <sapn class="fa fa-caret-right rgt_cart"></sapn>
                                <asp:Label ID="lbltoLocation" runat="server" Text="Location" Font-Bold="true"></asp:Label>
                                <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                         <div class="row">
                        <div class="col-md-8">
                            <asp:RadioButtonList ID="rblRate" runat="server" RepeatDirection="Horizontal" Width="300px">
                                <asp:ListItem Selected="True" Text="Avg. Pur. Price" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Last Price" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
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
                                <asp:CheckBox ID="cbItem" onclick="CheckBoxListSelectItem()" runat="server" Width="100%" Font-Size="11pt" Text="SelectAll" Checked="true"                                    ></asp:CheckBox>
                            <asp:Panel ID="Panel2" runat="server" Width="320px" Height="170px" ScrollBars="Vertical"
                                BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                <asp:CheckBoxList ID="cblItem" onclick="UnCheckSelectAllItem()" runat="server" Width="300px" Font-Size="11pt">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                        <div class="col-md-2">
                            <asp:CheckBox id="cbInActive" runat="server" Text="Include InActive Items" />
                        </div>
                    </div>
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
