<%@ Page Title="CORN :: Stock Valuation Report" Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="rptStockValuationItemWise.aspx.cs" Inherits="Forms_rptStockValuationItemWise" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadPage" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
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
    </script>
    <script language="JavaScript" type="text/javascript">

        function searchItems() {
            debugger;

            var searchText = document.getElementById('<%= txtSearch.ClientID %>').value.toLowerCase();
            var checkBoxList = document.getElementById('<%= chblSection.ClientID %>');

            var checkBoxItems = checkBoxList.getElementsByTagName('input');

            for (var i = 0; i < checkBoxItems.length; i++) {
                var itemLabel = checkBoxItems[i].parentNode.getElementsByTagName('label')[0];
                var words = itemLabel.textContent.toLowerCase().split(' ');

                var matchFound = words.some(function (word) {
                    return word.includes(searchText);
                });

                checkBoxItems[i].parentNode.style.display = matchFound ? '' : 'none';
            }
        }
</script>

    <div class="main-contents">
        <div class="container" style="min-height:250px;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>                    
                    <div class="row">
                        <div class="col-md-8">
                            <asp:RadioButtonList ID="rblRate" runat="server" RepeatDirection="Horizontal" Width="300px">
                                <asp:ListItem Selected="True" Text="Avg. Pur. Price" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Last Price" Value="4"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                            <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                      <div class="row">
                            <div class="col-md-4">
                            </div>
                        </div>
                     <div class="row">
                     <%--<div class="col-md-4">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:CheckBox ID="chbAllSections" onclick="CheckBoxListSelectSection()" AutoPostBack="true"  Checked="true" CssClass="SingleCheckbox1" runat="server" Width="100%"
                                        Font-Size="10pt" Text="Category"></asp:CheckBox>
                                    <asp:Panel ID="Panel3" runat="server" Width="345px" Height="200px" ScrollBars="Vertical"
                                        BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                        <asp:CheckBoxList ID="chblSection" AutoPostBack="true"   CssClass="SingleCheckbox1" onclick="UnCheckSelectAllSection()" margin-left="15px" runat="server" Width="236px" Font-Size="14px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                         </div>--%>

                         <div class="col-md-4">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:CheckBox ID="chbAllSections" onclick="CheckBoxListSelectSection()" Checked="true" CssClass="SingleCheckbox1" runat="server" Width="100%"
                                        Font-Size="10pt" Text="Items"></asp:CheckBox>

                                    <asp:TextBox ID="txtSearch" onkeyup="searchItems()" placeholder="Search Item name" runat="server" Width="100%"></asp:TextBox>
                                           

                                    <asp:Panel ID="Panel3" runat="server" Width="345px" Height="200px" ScrollBars="Vertical"
                                        BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                        <asp:CheckBoxList ID="chblSection" CssClass="SingleCheckbox1" onclick="UnCheckSelectAllSection()" margin-left="15px" runat="server" Width="236px" Font-Size="14px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                         </div>
                          </div>
                       <div class="row">
                            <div class="col-md-4">
                                &nbsp
                            </div>
                        </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>As On Date</label>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 27px">
                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                    </div>
                    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                    <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                        TargetControlID="txtStartDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
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

