<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmPurchaseOrder.aspx.cs" Inherits="Forms_frmPurchaseOrder" Title="CORN :: Purchase Order" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
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
            var str;
            str = document.getElementById('<%=txtQuantity.ClientID%>').value;
            if ((str == null || str.length == 0)) {
                alert('Must Enter Quantity');
                document.getElementById('<%=txtQuantity.ClientID%>').focus();
                return false;
            }

            str = document.getElementById('<%=txtPrice.ClientID%>').value;
            if ((str == null || str.length == 0)) {
                alert('Must Enter Price');
                document.getElementById('<%=txtPrice.ClientID%>').focus();
                return false;
            }

            return true;
        }

        function CalculateAmount() {

            var Qty = document.getElementById('<%=txtQuantity.ClientID%>').value;
                    var Rate = document.getElementById('<%=txtPrice.ClientID%>').value;
                    var discountPercent = document.getElementById('<%=txtDiscountPercent.ClientID%>').value;
                    var taxPercent = document.getElementById('<%=txtTaxPercent.ClientID%>').value;

                    if (Qty == null || isNaN(Qty) || Qty == undefined || Qty == "") {
                        Qty = 0;
                    }
                    if (Rate == null || isNaN(Rate) || Rate == undefined || Rate == "") {
                        Rate = 0;
                    }
                    if (discountPercent == null || isNaN(discountPercent) || discountPercent == undefined || discountPercent == "") {
                        discountPercent = 0;
                    }
                    if (taxPercent == null || isNaN(taxPercent) || taxPercent == undefined || taxPercent == "") {
                        taxPercent = 0;
                    }

                    var grossAmount = (Qty * Rate);
                    var discountAmount = grossAmount * (discountPercent / 100);
                    var amountAfterDiscount = grossAmount - discountAmount;

                    var taxAmount = amountAfterDiscount * (taxPercent / 100);
                    var amountAfterTax = amountAfterDiscount + taxAmount;

                    document.getElementById("<%= txtAmount.ClientID %>").value = parseFloat(amountAfterTax).toFixed(2);
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

        function ddlItemIndexChanged(s, e) {
            hfInventoryType = document.getElementById('<%=hfInventoryType.ClientID%>').value;
            document.getElementById('<%=txtQuantity.ClientID%>').focus();
            <%--if (hfInventoryType == '0') {
                document.getElementById('<%=txtQuantity.ClientID%>').focus();
            }
            else {
                $.ajax({
                    url: "http://localhost/CORNWeighingScale/Home/GetData",
                    data: { id: '1' },
                    type: "GET",
                    dataType: "jsonp",
                    jsonp: "callback",
                    success: function (data) {
                        data = data.replace(/[^\d.,]+/, '');
                        document.getElementById('<%=txtQuantity.ClientID%>').value = data.replace(/[^\d.,]+/, '');
                    }
                });
            }--%>
        }

        function pageLoad() {
            hfInventoryType = document.getElementById('<%=hfInventoryType.ClientID%>').value;
            if (hfInventoryType == '0') {
                //Do nothing
            }
            else {
                $.ajax({
                    url: "http://localhost/CORNWeighingScale/Home/GetData",
                    data: { id: '1' },
                    type: "GET",
                    dataType: "jsonp",
                    jsonp: "callback",
                    success: function (data) {
                        if (data.indexOf("does not exist") == -1) {
                            data = data.replace(/[^\d.,]+/, '');
                            document.getElementById('<%=txtQuantity.ClientID%>').value = data.replace(/[^\d.,]+/, '');
                        }
                    }
                });
            }

            $("#button2").click(function () {
                $.getJSON("http://localhost/CORNWeighingScale/Home/GetData?id=1&callback=?", function (data) {
                    alert(data);
                });
            });
        }

    </script>
    <div class="main-contents">
        <div class="container">
            <div class="row">
            <asp:Panel ID="Panel3" runat="server" DefaultButton="btnsearch">
                    <div class="col-md-4" runat="server" id="searchBox">
                        <div class="search">
                            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" CssClass="form-control"
                                TabIndex="0"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1" runat="server" id="searchBtn" style="margin-left: -60px;">
                        <asp:LinkButton ID="btnsearch" OnClick="btnFilter_Click" runat="server" Text="Search"
                            CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                    </div>
                 <div class="col-md-6" style="right: 5px; float: right;">
                     <div class="btnlist pull-right">
                        <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd" Text="Add"
                            OnClick="btnAdd_Click">
                               <span class="fa fa-plus-circle"></span>Add
                        </asp:LinkButton>
                        <asp:LinkButton class="btn btn-danger" ID="btnForceClose" runat="server" OnClick="btnForceClose_Click"
                            OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-close"></span>Force Close</asp:LinkButton>
                        </div>
                     </div>
                </asp:Panel>
                </div>
           
            <div class="row top" runat="server" id="contentBox" style="margin: 0 0 0 0;">
                <asp:LinkButton runat="server" ID="hbtn"></asp:LinkButton>
                <div class="row">
                    <div class="col-md-10">
                        <asp:Literal ID="lblErrorMsg" runat="server" Visible="false"></asp:Literal>
                    </div>
                </div>
                <!--- FORM STARTS HERE ---->
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:HiddenField ID="PurchaseOrderID" runat="server" Value="0" />
                                <asp:HiddenField ID="Document_Date" runat="server" Value="" />
                                <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>

                                <dx:ASPxComboBox ID="ddlLocation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndex_Changed" CssClass="form-control">
                                </dx:ASPxComboBox>
                            </div>

                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Supplier </label>

                                <dx:ASPxComboBox ID="ddlSupplier" runat="server" CssClass="form-control">
                                </dx:ASPxComboBox>
                            </div>

                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Payment Mode</label>
                                <dx:ASPxComboBox ID="ddlPaymentMode" runat="server" CssClass="form-control" SelectedIndex="0">
                                    <Items>
                                        <dx:ListEditItem Value="1" Text="Credit" />
                                        <dx:ListEditItem Value="2" Text="Cash"></dx:ListEditItem>
                                        <dx:ListEditItem Value="3" Text="Advance"></dx:ListEditItem>
                                    </Items>
                                </dx:ASPxComboBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-3">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Delivery Date</label>
                                <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-1" style="margin-top: 25px;">
                                <asp:ImageButton ID="ibtnFromDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                    Width="30px" />
                                <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                <cc1:CalendarExtender ID="CEDeliveryDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnFromDate"
                                    TargetControlID="txtDeliveryDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                            </div>


                            <div class="col-md-3">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Expiry Date</label>
                                <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-1" style="margin-top: 25px;">
                                <asp:ImageButton ID="ibtnToDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                    Width="30px" />
                                <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                <cc1:CalendarExtender ID="CEExpiryDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnToDate"
                                    TargetControlID="txtExpiryDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Remarks</label>
                                <asp:TextBox ID="txtRemarks" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                            </div>
                            <div class="col-md-3" style="margin-top: 30px;">
                                <label>
                                    <asp:Label ID="lblStock" ForeColor="Red" runat="server" Text="Closing Stock:0"></asp:Label>
                                </label>
                                <br />
                                <label>
                                    <asp:Label ID="lblLastPrice" ForeColor="Red" runat="server" Text="Last Price: 0"></asp:Label>
                                </label>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-3">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Item Description</label>
                            </div>
                            <div class="col-md-1">
                                <label><span class="fa fa-caret-right rgt_cart"></span>UOM</label>
                            </div>
                            <div class="col-md-1">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Qty</label>
                            </div>
                            <div class="col-md-1">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Price</label>
                            </div>
                            <div class="col-md-2">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Discount %</label>
                            </div>
                            <div class="col-md-1">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Tax %</label>
                            </div>
                            <div class="col-md-2">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Amount</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <dx:ASPxComboBox ID="ddlSkus" runat="server" CssClass="form-control"
                                    ClientInstanceName="ddlItem" ClientSideEvents-SelectedIndexChanged="function(s,e){ddlItemIndexChanged();}"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlSkus_SelectedIndexChanged">
                                </dx:ASPxComboBox>
                            </div>
                            <div class="col-md-1">
                                <asp:TextBox ID="txtUOM" runat="server" CssClass="form-control"
                                    Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"
                                    onkeypress="return QtyKeyPress(this,event);" onblur="CalculateAmount();"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:TextBox ID="txtPrice" onblur="CalculateAmount();" runat="server"
                                    onkeypress="return onlyDotsAndNumbers(this,event);" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtDiscountPercent" onblur="CalculateAmount();" runat="server"
                                    onkeypress="return onlyDotsAndNumbers(this,event);" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:TextBox ID="txtTaxPercent" onblur="CalculateAmount();" runat="server"
                                    onkeypress="return onlyDotsAndNumbers(this,event);" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-2" id="divAmount2" runat="server">
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:HiddenField ID="hfInventoryType" runat="server" Value="0" />
                                <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Add" CssClass="btn btn-success" />
                            </div>
                        </div>
                        <div class="row center">
                            <div class="col-md-8">
                                <asp:HiddenField ID="_rowNo2" runat="server" Value="0" />
                                <asp:HiddenField ID="_privouseQty" runat="server" Value="0" />
                                <div class="emp-table">
                                    <asp:Panel ID="Panel2" runat="server" Height="150px" Width="100%" ScrollBars="Vertical"
                                        BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                        <asp:GridView ID="GridViewPurchase" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center"
                                            CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                            OnRowDeleting="GridViewPurchase_RowDeleting" OnRowEditing="GridViewPurchase_RowEditing"
                                            ShowHeader="true">
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
                                                    <ItemStyle Width="30%" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="UOM" DataField="UOM_DESC" ReadOnly="true">
                                                    <ItemStyle Width="15%" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Qty" DataField="Quantity" DataFormatString="{0:f5}" ReadOnly="true">
                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Price" DataField="PRICE" DataFormatString="{0:f2}" ReadOnly="true">
                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Disc %" DataField="DiscountPercentage" DataFormatString="{0:f2}" ReadOnly="true">
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Tax %" DataField="TaxPercentage" DataFormatString="{0:f2}" ReadOnly="true">
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Amount" DataField="AMOUNT" DataFormatString="{0:f2}" ReadOnly="true">
                                                    <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="UOM_ID" ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" ToolTip="Edit">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                            class="fa fa-trash-o"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-5">
                                        <label><span class="fa fa-caret-right rgt_cart"></span>Gross Amount</label>
                                        <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-5">
                                        <label><span class="fa fa-caret-right rgt_cart"></span>Discount</label>
                                        <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control"
                                            onkeypress="return onlyDotsAndNumbers(this,event);" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-5">
                                        <label><span class="fa fa-caret-right rgt_cart"></span>Gst Amount</label>
                                        <asp:TextBox ID="txtGstAmount" runat="server" CssClass="form-control"
                                            onkeypress="return onlyDotsAndNumbers(this,event);" Enabled="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-5">
                                        <label><span class="fa fa-caret-right rgt_cart"></span>Net Amount</label>
                                        <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
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
                            <asp:Button ID="btnSave_Document" AccessKey="S" OnClick="btnSaveDocument_Click" runat="server" Text="Save" CssClass="btn btn-success" />
                            <asp:LinkButton CssClass="btn btn-danger" runat="server" ID="btnCancel" Text="Cancel"
                                OnClick="btnClose_Click">
                               <span class="fa fa-close"></span>Cancel
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row center" runat="server" id="lookupBox">
                <asp:HiddenField ID="_rowNo" runat="server" Value="0" />
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:GridView ID="Grid_users" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                            AutoGenerateColumns="False" OnPageIndexChanging="Grid_users_PageIndexChanging" PageSize="20" AllowPaging="true"
                            OnRowEditing="GrdPurchase_RowEditing" EmptyDataText="No Record exist"
                            OnRowDataBound="Grid_users_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="ID" HeaderText="PO No" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="5%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DISTRIBUTOR_ID" HeaderText="Distributor" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel "></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location" ReadOnly="true">
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
                                <asp:BoundField DataField="Payment_Mode" HeaderText="Payment_Mode" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel "></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Delivery_Date" HeaderText="Delivery Date" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Expiry_Date" HeaderText="Expiry Date" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Freight_Charges" HeaderText="Freight_Charges" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel "></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel "></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="GST_Amount" HeaderText="GST_Amount" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel "></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Discount_Amount" HeaderText="Discount_Amount" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel "></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="Net_Amount" HeaderText="Amount" ReadOnly="true">
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
                                <asp:BoundField DataField="Status" HeaderText="Status" ReadOnly="true">
                                   <ItemStyle CssClass="grdDetail" Width="5%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
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
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
