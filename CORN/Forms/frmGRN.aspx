<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmGRN.aspx.cs"
    Inherits="Forms_frmGRN" Title="CORN :: GRN" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <style type="text/css">
        .reduceSpace {
            padding: 0 2px;
        }
    </style>
    <script type="text/javascript" src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
    <script type="text/javascript" src="../AjaxLibrary/jquery-1.6.1.min.js"></script>

    <script language="JavaScript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }

        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }

        function Check_Click(objRef) {
            var row = objRef.parentNode.parentNode;
            var GridView = row.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var headerCheckBox = inputList[0];
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }

            headerCheckBox.checked = checked;
        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }

        function ValidateForm() {
            $("#<%=GridViewPurchase.ClientID%> tr").each(function (index) {
                if (index > 0) {
                    var enteredQty = $($(this).closest('tr').find("td:eq(7)").find('input')).val();
                    var qty = $($(this).closest('tr').find('td:eq(6)')).text();

                    if (qty == undefined || qty == "" || isNaN(qty) || qty == null) {
                        qty = 0;
                    }
                    if (enteredQty == undefined || enteredQty == "" || isNaN(enteredQty) || enteredQty == null) {
                        enteredQty = 0;
                    }

                    if (parseFloat(enteredQty) > parseFloat(qty)) {
                        alert('Entered Qty cannot be greater than Pending Qty');
                        return false;
                    }
                }
            });

            return true;
        }


        function QtyKeyPress(txt, event) {

            var charCode = (event.which) ? event.which : event.keyCode;

            if (hfInventoryType == '0') {
                if (charCode == 9 || charCode == 8) {
                    return true;
                }
                if (charCode == 46) {
                    if (txt.value.indexOf(".") < 0)
                        return true;
                    return false;
                }
                if (charCode == 31 || charCode < 48 || charCode > 57)
                    return false;
            }
            else {
                return false
            }

            return true;
        }
        function Calculations(txtcntrlname,txtprice) {
            var qty = $(txtcntrlname).val();
            var price = $(txtprice).val();
            var discountPercent = $($(txtcntrlname).closest('tr').find('td:eq(9)').find('input')).val();
            var taxPercent = $($(txtcntrlname).closest('tr').find('td:eq(10)').find('input')).val();

            if (qty == undefined || qty == "" || isNaN(qty) || qty == null) {
                qty = 0;
            }
            if (price == undefined || price == "" || isNaN(price) || price == null) {
                price = 0;
            }
            if (discountPercent == undefined || discountPercent == "" || isNaN(discountPercent) || discountPercent == null) {
                discountPercent = 0;
            }
            if (taxPercent == undefined || taxPercent == "" || isNaN(taxPercent) || taxPercent == null) {
                taxPercent = 0;
            }

            var grossAmount = parseFloat(qty * price);
            var amuntWithDiscount = grossAmount - grossAmount * (discountPercent / 100);
            var taxAmount = amuntWithDiscount * (taxPercent / 100);
            var netAmount = taxAmount + amuntWithDiscount;
            $($(txtcntrlname).closest('tr').find('td:eq(11)')).text(parseFloat(netAmount).toFixed(2));
            $($(txtcntrlname).closest('tr').find('td:eq(13)').find('input')).val(parseFloat(netAmount).toFixed(2));
            $($(txtcntrlname).closest('tr').find('td:eq(13)').find('input')).text(parseFloat(netAmount).toFixed(2));
            calculateGrandTotal();
        }

        function calculateGrandTotal() {
            var totalQty = 0;
            var totalGrossAmount = 0;
            var totalDiscountAmount = 0;
            var totalTaxAmount = 0;
            var grandTotal = 0;
            var freightAmount = document.getElementById('<%=txtFreight.ClientID%>').value;
            if (freightAmount == '')
            {
                freightAmount = 0;
            }
            $("#<%=GridViewPurchase.ClientID%> tr").each(function () {
                var qty = $($(this).closest('tr').find("td:eq(7)").find('input')).val();
                var price = $($(this).closest('tr').find("td:eq(8)").find('input')).val();
                var discountPercent = $($(this).closest('tr').find("td:eq(9)").find('input')).val();
                var taxPercent = $($(this).closest('tr').find("td:eq(10)").find('input')).val();
                if (qty == undefined || qty == "" || isNaN(qty) || qty == null) {
                    qty = 0;
                }
                if (price == undefined || price == "" || isNaN(price) || price == null) {
                    price = 0;
                }
                if (discountPercent == undefined || discountPercent == "" || isNaN(discountPercent) || discountPercent == null) {
                    discountPercent = 0;
                }
                if (taxPercent == undefined || taxPercent == "" || isNaN(taxPercent) || taxPercent == null) {
                    taxPercent = 0;
                }

                var grossAmount = parseFloat(qty * price);
                totalGrossAmount = totalGrossAmount + grossAmount;

                var discountAmount = grossAmount * (discountPercent / 100);
                totalDiscountAmount = totalDiscountAmount + discountAmount;

                var amuntWithDiscount = grossAmount - discountAmount;
                var taxAmount = amuntWithDiscount * (taxPercent / 100);
                totalTaxAmount = totalTaxAmount + taxAmount;

                var netAmount = taxAmount + amuntWithDiscount;
                grandTotal = grandTotal + netAmount;
            });

            document.getElementById('<%=txtTotalAmount.ClientID%>').value = parseFloat(totalGrossAmount).toFixed(2);
            document.getElementById('<%=txtDiscount.ClientID%>').value = parseFloat(totalDiscountAmount).toFixed(2);
            document.getElementById('<%=txtGstAmount.ClientID%>').value = parseFloat(totalTaxAmount).toFixed(2);
            document.getElementById('<%=txtNetAmount.ClientID%>').value = parseFloat(grandTotal + parseFloat(freightAmount)).toFixed(2);
        }

    </script>
    <div class="main-contents">
        <div class="container">
            <div class="row top">
                <asp:Panel ID="Panel3" runat="server" DefaultButton="btnsearch">
                    <div class="col-md-4">
                        <div class="search">
                            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" CssClass="form-control"
                                TabIndex="0"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-2" style="margin-left: -60px;">
                        <asp:LinkButton ID="btnsearch" OnClick="btnFilter_Click" runat="server" Text="Search"
                            CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                    </div>
                </asp:Panel>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="col-md-offset-3 col-md-3" style="float:right;">
                            <div class="btnlist pull-right">
                                <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd" Text="Add"
                                    OnClick="btnAdd_Click">
                               <span class="fa fa-plus-circle"></span>Add
                                </asp:LinkButton>
                                <asp:LinkButton class="btn btn-success" ID="btnActive" runat="server"
                                    OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                            </div>
                        </div>
                        <asp:LinkButton runat="server" ID="hbtn"></asp:LinkButton>
                        <cc1:ModalPopupExtender ID="mPopUpLocation" runat="server" PopupControlID="pnlParameters"
                            TargetControlID="hbtn" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                            CancelControlID="btnClose">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlParameters" runat="server" Style="display: none; height: 700px" ScrollBars="Auto" DefaultButton="btnSave_Document">
                            <div class="modal-dialog2" style="width: 100%;">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_Click">
                                            <span>&times;</span><span class="sr-only">Close</span></button>
                                        <h1 class="modal-title" id="myModalLabel">
                                            <span></span>Goods Receiving Note</h1>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                        <ContentTemplate>
                                            <div class="modal-body">
                                                <div style="z-index: 101; left: 40px; width: 100px; position: absolute; top: 10px; height: 100px"
                                                    id="DIV1">
                                                    <asp:UpdateProgress ID="UpdateProgress5" runat="server">
                                                        <ProgressTemplate>
                                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif"></asp:ImageButton>&nbsp; Loading....
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-10">
                                                        <asp:Literal ID="lblErrorMsg" runat="server" Visible="false"></asp:Literal>
                                                    </div>
                                                </div>
                                                <!--- FORM STARTS HERE ---->
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <div class="modal-body">
                                                                    <div class="row" id="PurchaseOrderInfo" runat="server" visible="false">
                                                                        <div class="col-md-6"></div>
                                                                        <div class="col-md-6">
                                                                            <div class="col-md-6" style="padding-bottom: 15px;">
                                                                                <b>Delivery Date:</b> <u><span runat="server" id="deliveryDate"></span></u>
                                                                            </div>
                                                                            <div class="col-md-6" style="padding-bottom: 15px;">
                                                                                <b>Expiry Date:</b> <u><span runat="server" id="expiryDate"></span></u>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-4">
                                                                            <asp:HiddenField ID="PurchaseMasterID" runat="server" Value="0" />
                                                                            <asp:HiddenField ID="Document_Date" runat="server" Value="" />
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>

                                                                            <dx:ASPxComboBox ID="ddlLocation" runat="server" CssClass="form-control"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                                                            </dx:ASPxComboBox>
                                                                        </div>

                                                                        <div class="col-md-4">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Supplier </label>

                                                                            <dx:ASPxComboBox ID="ddlSupplier" runat="server" CssClass="form-control"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged">
                                                                            </dx:ASPxComboBox>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Purchase Order</label>
                                                                            <asp:HiddenField ID="hfInventoryType" runat="server" Value="0" />
                                                                            <dx:ASPxComboBox ID="ddlPurchase" runat="server" CssClass="form-control"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlPurchase_SelectedIndexChanged">
                                                                            </dx:ASPxComboBox>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row">
                                                                        <div class="col-md-3">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Payment Mode</label>
                                                                            <dx:ASPxComboBox ID="ddlPaymentMode" runat="server" CssClass="form-control" SelectedIndex="0">
                                                                                <Items>
                                                                                    <dx:ListEditItem Value="1" Text="Credit" />
                                                                                    <dx:ListEditItem Value="2" Text="Cash"></dx:ListEditItem>
                                                                                    <dx:ListEditItem Value="3" Text="Advance"></dx:ListEditItem>
                                                                                </Items>
                                                                            </dx:ASPxComboBox>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Freight Charges</label>
                                                                            <asp:TextBox ID="txtFreight" runat="server" CssClass="form-control" onkeypress="return onlyDotsAndNumbers(this,event);" onkeyup="CalculateNetAmount();"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <span class="fa fa-caret-right rgt_cart"></span>
                                                                            <asp:Label ID="lblInvoice" runat="server" Text="INV/DC No" MaxLength="100" />
                                                                            <asp:TextBox ID="txtDocumentNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Remarks</label>
                                                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <div class="row center">
                                                                    <div class="col-md-10">
                                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                                                        <asp:HiddenField ID="_privouseQty" runat="server" Value="0" />
                                                                        <div class="emp-table">
                                                                            <asp:Panel ID="Panel2" runat="server" Height="215px" Width="100%" ScrollBars="Vertical"
                                                                                BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                                                                <asp:GridView ID="GridViewPurchase" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center"
                                                                                    CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                                                    ShowHeader="true" OnRowDataBound="GridViewPurchase_RowDataBound">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID" ReadOnly="true">
                                                                                            <HeaderStyle CssClass="HidePanel" />
                                                                                            <ItemStyle CssClass="HidePanel" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="SKU_CODE" ReadOnly="true">
                                                                                            <HeaderStyle CssClass="HidePanel" />
                                                                                            <ItemStyle CssClass="HidePanel" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField HeaderText="Item" DataField="SKU_NAME" ReadOnly="true">
                                                                                            <ItemStyle Width="15%" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField HeaderText="UOM" DataField="UOM_DESC" ReadOnly="true">
                                                                                            <ItemStyle Width="5%" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField HeaderText="Ordered Qty" DataField="OrderedQty" DataFormatString="{0:f5}" ReadOnly="true">
                                                                                            <ItemStyle Width="9%" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField HeaderText="Received Qty" DataField="ReceivedQty" DataFormatString="{0:f5}" ReadOnly="true">
                                                                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField HeaderText="Pending Qty" DataField="Quantity" DataFormatString="{0:f5}" ReadOnly="true">
                                                                                            <ItemStyle CssClass="HidePanel" />
                                                                                            <HeaderStyle CssClass="HidePanel" />
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="Cur. Qty">
                                                                                            <ItemStyle Width="10%" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtQty" runat="server" Text='<%# Eval("EnteredQty")%>'  CssClass="form-control" onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Price">
                                                                                            <ItemStyle Width="10%" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" Text='<%# Eval("Price")%>' onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Disc %">
                                                                                            <ItemStyle Width="10%" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtDisc" runat="server" CssClass="form-control" Text='<%# Eval("DiscountPercentage")%>' onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Tax %">
                                                                                            <ItemStyle Width="10%" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtTax" runat="server" CssClass="form-control" Text='<%# Eval("TaxPercentage")%>' onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:BoundField HeaderText="Amount" DataField="AMOUNT" DataFormatString="{0:f2}">
                                                                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="UOM_ID" ReadOnly="true">
                                                                                            <HeaderStyle CssClass="HidePanel" />
                                                                                            <ItemStyle CssClass="HidePanel" />
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="Amount Changed">
                                                                                            <HeaderStyle CssClass="HidePanel" />
                                                                                            <ItemStyle CssClass="HidePanel" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="rownetAmount" runat="server" />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Expiry Date">
                                                                                            <ItemStyle Width="10%" />
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtExpiryDate" Font-Size="13px" runat="server" CssClass="form-control" AutoComplete="off"></asp:TextBox>
                                                                                                <cc1:CalendarExtender ID="CalendarExtender2" OnClientShown="calendarShown" runat="server"
                                                                                                    Format="dd-MMM-yyyy" TargetControlID="txtExpiryDate"></cc1:CalendarExtender>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <div class="row">
                                                                            <div class="col-md-6 reduceSpace">
                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Gross Amount</label>
                                                                                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                            </div>
                                                                            <div class="col-md-6 reduceSpace">
                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Discount</label>
                                                                                <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control"
                                                                                    onkeypress="return onlyDotsAndNumbers(this,event);" onkeyup="CalculateNetAmount();"></asp:TextBox>
                                                                            </div>

                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-md-6 reduceSpace">
                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Gst Amount</label>
                                                                                <asp:TextBox ID="txtGstAmount" runat="server" CssClass="form-control"
                                                                                    onkeypress="return onlyDotsAndNumbers(this,event);" onkeyup="CalculateNetAmount();"></asp:TextBox>
                                                                            </div>
                                                                            <div class="col-md-6 reduceSpace">
                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Advance Tax</label>
                                                                                <asp:TextBox ID="txtAdvanceTax" runat="server" CssClass="form-control"
                                                                                    onkeypress="return onlyDotsAndNumbers(this,event);" onkeyup="CalculateNetAmount();"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                            <div class="col-md-6 reduceSpace">
                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Net Amount</label>
                                                                                <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                            </div>
                                                                            <div class="col-md-6 reduceSpace">
                                                                                
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-3">
                                                                    </div>
                                                                    <div class="col-md-2">
                                                                        <label><span class="fa fa-caret-right rgt_cart"></span>Total Quantity</label>
                                                                        <asp:TextBox ID="txtTotalQuantity" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-7">
                                                                    </div>

                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                        <div class="row">
                                                            <div class="col-md-offset-5 col-md-3 ">
                                                                <div class="btnlist pull-right">
                                                                    <asp:Button ID="btnSave_Document" AccessKey="S" OnClick="btnSaveDocument_Click" runat="server" Text="Save" UseSubmitBehavior="False" CssClass="btn btn-success" />
                                                                    <asp:Button ID="btnCancel" AccessKey="C" OnClick="btnCancel_Click" runat="server" Text="Cancel" UseSubmitBehavior="False" CssClass="btn btn-danger" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        </div>
                                                                </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="row center" style="margin-left:1px; margin-right:1px;">
                <asp:HiddenField ID="_rowNo" runat="server" Value="0" />
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanelDetail" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="Grid_users" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                    AutoGenerateColumns="False" OnPageIndexChanging="Grid_users_PageIndexChanging" PageSize="20" AllowPaging="true"
                                    OnRowEditing="GrdPurchase_RowEditing" EmptyDataText="No Record exist">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PURCHASE_MASTER_ID" HeaderText="GRN No" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="5%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DISTRIBUTOR_ID" HeaderText="Distributor" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Distributor" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PRINCIPAL_ID" HeaderText="PRINCIPAL_ID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SKU_HIE_NAME" HeaderText="Supplier" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Purchase_Order_Master_ID" HeaderText="Purchase Order #" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Payment_Mode" HeaderText="Payment_Mode" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Ordered_Qty" HeaderText="Ordered Qty" DataFormatString="{0:f2}" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Received_Qty" HeaderText="Received Qty" DataFormatString="{0:f2}" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REMARKS" HeaderText="REMARKS" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FREIGHT_AMOUNT" HeaderText="FREIGHT_AMOUNT" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Discount_Amount" HeaderText="Discount_Amount" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Net_Amount" HeaderText="Amount" DataFormatString="{0:f2}" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Gross_Amount" HeaderText="Gross_Amount" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="DOCUMENT_DATE" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ORDER_NUMBER" HeaderText="ORDER_NUMBER" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GST_ADVANCE" HeaderText="GST_ADVANCE" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" class="fa fa-pencil" CommandName="Edit" ToolTip="Edit">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                                    <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function CalculateNetAmount() {
            var GrossAmount = $("[id$='txtTotalAmount']").val();
            var Discount = $("[id$='txtDiscount']").val();
            var Gst = $("[id$='txtGstAmount']").val();
            var GstAdvance = $("[id$='txtAdvanceTax']").val();
            var freight = $("[id$='txtFreight']").val();
            if (GrossAmount == "") {
                GrossAmount = 0;
            }
            if (Discount == "") {
                Discount = 0;
            }
            if (Gst == "") {
                Gst = 0;
            }
            if (freight == "") {
                freight = 0;
            }
            if (GstAdvance == "")
            {
                GstAdvance = 0;
            }
            $("[id$='txtNetAmount']").val((parseFloat(GrossAmount) + parseFloat(Gst) + parseFloat(GstAdvance) - parseFloat(Discount) + parseFloat(freight)).toFixed(2));
        }
    </script>
</asp:Content>