<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="rptPriceList.aspx.cs" Inherits="Forms_rptPriceList" Title="CORN :: Item Price List" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript">
        
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
                    <asp:Panel ID="pnl_rpt" runat="server">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:RadioButtonList ID="rblRate" runat="server" RepeatDirection="Horizontal" Width="300px">
                                <asp:ListItem Selected="True" Text="Avg. Pur. Price" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Last Price" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                <asp:CheckBox ID="ChbSelectAll" onclick="CheckBoxListSelect()" runat="server" Width="100%"
                                Font-Size="11pt" Text="Select All"></asp:CheckBox>
                                <br />
                            <asp:Panel ID="Panel1" runat="server" Width="100%" Height="200px" ScrollBars="Vertical"
                                BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">

                                <asp:CheckBoxList ID="ChbDistributorList" onclick="UnCheckSelectAll()" runat="server" Width="300px" Font-Size="11pt">
                                </asp:CheckBoxList>
                            </asp:Panel>                              
                            </div>
                        </div>                        
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Catagory</label>
                                <dx:ASPxComboBox ID="DrpCatagory" runat="server" CssClass="form-control" AutoPostBack="True">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Status</label>
                                <dx:ASPxComboBox ID="drpStatus" runat="server" CssClass="form-control">
                                    <Items>
                                        <dx:ListEditItem Text="All" Value="0" Selected="true" />
                                        <dx:ListEditItem Text="Active" Value="1" />
                                        <dx:ListEditItem Text="In-Active" Value="2" />
                                    </Items>
                                </dx:ASPxComboBox>
                            </div>
                        </div>

                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-md-offset-1 col-md-3 ">
                <div class="btnlist pull-right">
                <asp:Button ID="btnViewPDF" runat="server" OnClick="btnViewPDF_Click" Text="View PDF" CssClass=" btn btn-success" />
                <asp:Button ID="btnViewExcel" runat="server" OnClick="btnViewExcel_Click" Text="View Excel" CssClass=" btn btn-success" />
            </div>
                </div>
        </div>

    </div>
</asp:Content>
