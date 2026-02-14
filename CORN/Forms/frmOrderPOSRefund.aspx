<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmOrderPOSRefund.aspx.cs" Inherits="Forms_frmOrderPOSRefund" Title="CORN :: Sales Refund" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
    <script type="text/javascript" language="javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        function validateSingleRow(txtBox) {
            var row = txtBox.closest("tr");
            var qtyText = row.cells[2].innerText.trim();
            var qty = parseFloat(qtyText) || 0;
            var returnQty = parseFloat(txtBox.value) || 0;
            if (returnQty > qty) {
                alert("Return quantity cannot be greater than sold quantity (" + qty + ").");
                txtBox.value = 0;
                txtBox.focus();
                return false;
            }
            return true;
        }

        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Service Type</label>
                            <dx:ASPxComboBox ID="ddlServiceType" runat="server" CssClass="form-control" SelectedIndex="0"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlServiceType_SelectedIndexChanged">
                                <Items>
                                    <dx:ListEditItem Value="-2147483648" Text="All" />
                                    <dx:ListEditItem Value="1" Text="Dine In" />
                                    <dx:ListEditItem Value="2" Text="Delivery" />
                                    <dx:ListEditItem Value="3" Text="Takeaway" />
                                </Items>
                            </dx:ASPxComboBox>                            
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Date</label>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" MaxLength="10" onkeyup="BlockEndDateKeyPress()"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="margin-top: 27px">
                            <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                TargetControlID="txtDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                        </div>
                        <div class="col-md-3">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" Text="Load Invoices"
                            UseSubmitBehavior="False" OnClick="btnLoad_Click" CssClass="btn btn-success"></asp:Button>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Invoice No</label>
                            <dx:ASPxComboBox ID="ddlInvoiceNo" runat="server" CssClass="form-control"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceNo_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-1">
                            <label runat="server" id="lblServiceType"></label>
                        </div>
                        <div class="col-md-2">
                            <label runat="server" id="lblPaymentMode"></label>
                        </div>
                        <div class="col-md-3">
                            <label runat="server" id="lblCustomer"></label>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="row center">
                <div class="col-md-8">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" Height="200px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                    <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False"
                                        CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                        Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="SKU_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_NAME" HeaderText="SKU Name" ReadOnly="true"></asp:BoundField>
                                            <asp:BoundField DataField="QTY" HeaderText="Quantity" ReadOnly="true"></asp:BoundField>
                                            <asp:BoundField DataField="PRICE" HeaderText="Price" ReadOnly="true"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Return Qty" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center"
                                                    Width="15%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtReturnQty" runat="server" Width="100%" Text='0' onkeypress="return onlyDotsAndNumbers(this,event);" onchange="validateSingleRow(this);">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SALE_INVOICE_DETAIL_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DISCOUNT" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Item Type" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center"
                                                    Width="15%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlType" runat="server">
                                                        <asp:ListItem Value="1" Text="Not Cooked" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Cooked"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BRAND_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ISEXEMPTED" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IS_Recipe" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IS_VOID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="IS_FREE" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ITEM_DEAL_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DealQTY" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                        </Columns>
                                        <HeaderStyle CssClass="cf head" />
                                    </asp:GridView>
                                </asp:Panel>
                                <div class="row">
                                    <div class="col-md-2">
                                        <label runat="server" id="lblGrossLabel"></label>
                                    </div>
                                    <div class="col-md-2">
                                        <label runat="server" id="lblDiscountLable"></label>
                                    </div>
                                    <div class="col-md-2">
                                        <label runat="server" id="lblGSTLabel"></label>
                                    </div>
                                    <div class="col-md-2">
                                        <label runat="server" id="lblSCLabel"></label>
                                    </div>
                                    <div class="col-md-2">
                                        <label runat="server" id="lblNetAmountLable"></label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2">
                                        <label runat="server" id="lblGrossAmount"></label>
                                    </div>
                                    <div class="col-md-2">
                                        <label runat="server" id="lblDiscount"></label>
                                    </div>
                                    <div class="col-md-2">
                                        <label runat="server" id="lblGST"></label>
                                    </div>
                                    <div class="col-md-2">
                                        <label runat="server" id="lblSC"></label>
                                    </div>
                                    <div class="col-md-2">
                                        <label runat="server" id="lblNetAmount"></label>
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 15px">
                                    <div class="col-md-12 ">
                                        <div class="btnlist pull-right">
                                            <asp:Button ID="btnDone" runat="server" Text="Done"
                                                UseSubmitBehavior="False" OnClick="btnDone_Click" CssClass="btn btn-success"></asp:Button>
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel"
                                                UseSubmitBehavior="False" CssClass="btn btn-danger" OnClick="btnCancel_Click"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div id="dvSummary" style="width: 50%; height: 530px; padding: 10px; display: none; background-color: #f9f9f9; overflow: auto;">
                <div class="row">
                    <div class="col-md-6">
                        <asp:Panel ID="Panel1" runat="server" Height="200px" ScrollBars="Vertical"
                            BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                            <asp:GridView ID="gvSummary" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="SKU_NAME" HeaderText="SKU Name" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField DataField="Quantity" HeaderText="Returend Quantity" ReadOnly="true"></asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="cf head" />
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-6">
                                <strong>
                                    <asp:Label ID="lblRefundGross" runat="server"></asp:Label>
                                </strong>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <strong>
                                    <asp:Label ID="lblRefundDiscount" runat="server"></asp:Label>
                                </strong>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <strong>
                                    <asp:Label ID="lblRefundGST" runat="server"></asp:Label>
                                </strong>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <strong>
                                    <asp:Label ID="lblRefundTotal" runat="server"></asp:Label>
                                </strong>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <br />
                        <asp:Button ID="btnReturn" runat="server" Text="Return"
                            UseSubmitBehavior="False" OnClick="btnRefund_Click" CssClass="btn btn-success"></asp:Button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
