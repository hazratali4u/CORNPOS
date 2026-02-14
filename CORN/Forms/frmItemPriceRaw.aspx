<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmItemPriceRaw.aspx.cs" Inherits="Forms_frmItemPriceRaw" Title="CORN :: Raw Material Item Price" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
    <script language="JavaScript" type="text/javascript">
       
        function ConfirmDelete() {
            if (confirm("Do you want to Cancel this record?") == true)
                return true;

            else {
                return false;
            }
        }

        function ValidateForm() {
            var str;

            return true;
        }

        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
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
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-8">
                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                    <div class="row" style="display:none;">                        

                        <div class="col-md-6">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                            <br>
                            <asp:CheckBox ID="ChbSelectAll" onclick="CheckBoxListSelect()" runat="server" Width="100%"
                                Font-Size="11pt" Text="SelectAll" Checked="true"></asp:CheckBox>
                            <asp:Panel ID="pnlLocation" runat="server" Width="100%" Height="70px" ScrollBars="Horizontal"
                                BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" BackColor="White">
                                <asp:CheckBoxList ID="ChbDistributorList" onclick="UnCheckSelectAll()" runat="server" Width="300px" Font-Size="11pt">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Category</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            
                           <dx:ASPxComboBox ID="ddlCategory" runat="server"  CssClass="form-control" 
                               AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row center">
                        <div class="col-md-12">
                            <div class="emp-table">
                                <asp:HiddenField ID="_rowNo" runat="server" Value="0" />
                                <asp:Panel ID="Panel2" runat="server" Width="90%" Height="800px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                    <asp:GridView ID="GrdPurchase" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                        AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_HIE_NAME"  ReadOnly="true" HeaderText="Category" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_NAME"  ReadOnly="true"  HeaderText="Item Description" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left"
                                                    Width="25%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UOM_DESC"  ReadOnly="true" HeaderText="UOM" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left"
                                                    Width="10%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Price" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center"
                                                    Width="15%"  />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPrice" runat="server" Width="100%" Text='<%# Eval("Price")%>' onkeypress="return onlyDotsAndNumbers(this,event);">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date Effected" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" Width="15%"  />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDateEffected" runat="server" Width="70%" Text='<%# Eval("DATE_EFFECTED")%>'>
                                                    </asp:TextBox>
                                                    <asp:ImageButton ID="ibDateEffected" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                    Width="22px" Height="18px"/>
                                                    <cc1:CalendarExtender ID="ceDateEffected" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibDateEffected"
                                                        TargetControlID="txtDateEffected" OnClientShown="calendarShown">
                                                    </cc1:CalendarExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <div class="row">
                <div class="col-md-offset-3 col-md-2">
                    <div class="btnlist pull-right">
                        <asp:Button ID="btnSaveDocument" AccessKey="S" OnClick="btnSaveDocument_Click" runat="server" Text="Save" UseSubmitBehavior="False" CssClass="btn btn-success" />
                        <asp:Button ID="btnCancel" AccessKey="C" OnClick="btnCancel_Click" runat="server" Text="Cancel" UseSubmitBehavior="False" CssClass="btn btn-danger" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
