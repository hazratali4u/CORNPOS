<%@ Page Title="CORN :: Customer Receipt" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmCustomerReceipt.aspx.cs" Inherits="Forms_frmCustomerReceipt" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 100005;
        }

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        function ValidateForm() {
            var str;
            str = document.getElementById("<%= txtAmount.ClientID %>").value;
            if (str == null || str.length == 0) {
                alert('Amount is required');
                return false;
            }

        }
        function onlyDotsAndNumbers(txt, event) {
            var charCode = (event.which) ? event.which : event.keyCode;

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
            return true;
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
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <div style="z-index: 101; left: 50%; width: 100px; position: absolute; top: 150px; height: 100px">
                <asp:Panel ID="Panel21" runat="server">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                        <ProgressTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </asp:Panel>
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-8">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Receipt Type</label>
                                    <dx:ASPxComboBox ID="ddlRecieptType" runat="server" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlRecieptType_SelectedIndexChanged" CssClass="form-control">
                                        <Items>
                                            <dx:ListEditItem Value="1" Text="Customer Receipt" Selected="true"></dx:ListEditItem>
                                            <dx:ListEditItem Value="2" Text="Franchise Receipt"></dx:ListEditItem>
                                            <%--<dx:ListEditItem Value="3" Text="Royalty Receipt"></dx:ListEditItem>--%>
                                            <dx:ListEditItem Value="4" Text="Customer Advance"></dx:ListEditItem>
                                            <dx:ListEditItem Value="5" Text="Customer Advance Return"></dx:ListEditItem>
                                        </Items>
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Transaction Date</label>
                                    <asp:TextBox ID="txtTransactionDate" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:ImageButton ID="ibTransaction" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="30px" Style="padding-top: 21px" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Transaction Type</label>
                                    <dx:ASPxComboBox ID="DrpAccountType" runat="server" AutoPostBack="True" SelectedIndex="0"
                                        OnSelectedIndexChanged="DrpAccountType_SelectedIndexChanged" CssClass="form-control">
                                        <Items>
                                            <dx:ListEditItem Value="18" Text="Cheque Receipt"></dx:ListEditItem>
                                            <dx:ListEditItem Value="21" Text="Cash Receipt"></dx:ListEditItem>
                                            <dx:ListEditItem Value="33" Text="Online Receipt"></dx:ListEditItem>
                                            <dx:ListEditItem Value="34" Text="Credit Card"></dx:ListEditItem>
                                        </Items>
                                    </dx:ASPxComboBox>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                    <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control"
                                        OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged" AutoPostBack="True">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Account Head</label>
                                    <dx:ASPxComboBox ID="DrpBankAccount" runat="server" CssClass="form-control">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <label runat="server" id="lblChequeNo"><span class="fa fa-caret-right rgt_cart"></span>Cheque No</label>
                                    <asp:TextBox ID="txtChequeNo" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <strong>
                                        <asp:Literal runat="server" ID="lblChequeDate"><span class="fa fa-caret-right rgt_cart"></span>Cheque Date</asp:Literal></strong>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="30px" Style="padding-top: 21px" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Amount</label>
                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"
                                        onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Remarks</label>

                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <asp:Button AccessKey="S" ID="btnSave" OnClick="btnSave_Click" runat="server"
                                Text="Save" CssClass="btn btn-success" />

                            <asp:Button AccessKey="C" ID="btnCancel" runat="server"
                                Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-danger" />
                        </div>
                        <div class="col-md-1">
                        </div>
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Customer</label>
                            <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAddCustomer" Text="Add Customer" Visible="false">
                               <span class="fa fa-plus-circle"></span>Add Customer
                            </asp:LinkButton>
                            <dx:ASPxComboBox ID="ddlCustomer" runat="server" CssClass="form-control"
                                OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" AutoPostBack="True">
                            </dx:ASPxComboBox>
                            <asp:Panel ID="Panel1" runat="server" Height="440px" ScrollBars="Vertical" BorderColor="Silver"
                                BorderStyle="Groove" BorderWidth="1px" Width="100%">
                                <asp:GridView ID="GrdCredit" runat="server" AutoGenerateColumns="False"
                                    Width="100%" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                    DataKeyNames="SALE_INVOICE_ID">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="checkAll" Checked="true" runat="server" onclick="checkAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SALE_INVOICE_ID">
                                            <HeaderStyle CssClass="HidePanel" />
                                            <ItemStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="InvoiceNo" HeaderText="INV No">
                                            <ItemStyle />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="INV Date">
                                            <ItemStyle />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CURRENT_CREDIT_AMOUNT" HeaderText="Credit Amount" DataFormatString="{0:n}">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DOCUMENT_TYPE_ID">
                                            <HeaderStyle CssClass="HidePanel" />
                                            <ItemStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle CssClass="cf head" />
                                </asp:GridView>
                            </asp:Panel>
                            <table width="100%" style="margin-top: 10px;">
                                <tr>
                                    <td style="width: 50%;" align="right">
                                        <strong>Total Credit Amount:
                                        </strong>
                                    </td>
                                    <td style="width: 50%;" align="right">
                                        <asp:TextBox Width="82%" ID="txtTotalCreditAmount" ReadOnly="true" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-1">
                        </div>
                    </div>
                    <cc1:ModalPopupExtender ID="mPopUpCustomer" runat="server" PopupControlID="pnlParameters"
                        TargetControlID="btnAddCustomer" BehaviorID="ModelPopup2" BackgroundCssClass="modal-background"
                        CancelControlID="btnClose">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="pnlParameters" runat="server" Style="display: none; width: 60%" ScrollBars="Auto">
                        <div class="modal-dialog" style="width: 100%">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" id="btnClose" class="close" runat="server">
                                        <span>&times;</span><span class="sr-only">Close</span></button>
                                    <h1 class="modal-title" id="myModalLabel">
                                        <span></span>Add Customer</h1>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <div class="modal-body">
                                            <div class="row" style="margin-top: 10px">
                                                <div class="col-md-6">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Customer Name</label>
                                                    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Primary Contact</label>
                                                    <asp:TextBox ID="txtPrimaryContact" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Address</label>
                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Other Contact</label>
                                                    <asp:TextBox ID="txtOtherContact" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Email</label>
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>CNIC</label>
                                                    <asp:TextBox ID="txtCNIC" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row" style="margin-top: 20px; margin-bottom: 20px;">
                                                <div class="col-md-7">
                                                </div>
                                                <div class="col-md-12" align="right">
                                                    <asp:Button ID="btnSaveCustomer" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnSaveCustomer_Click" />
                                                    <asp:Button ID="btnCancelCustomer" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" OnClick="btnCancelCustomer_Click"/>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:CheckBox ID="ChkIsDiscount" Text="%" runat="server" Visible="false" />
                    <asp:TextBox ID="txtDiscount" runat="server"
                        onkeypress="return onlyDotsAndNumbers(this,event);" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtTax" runat="server"
                        onkeypress="return onlyDotsAndNumbers(this,event);" Visible="false"></asp:TextBox>
                    <dx:ASPxComboBox ID="ddlAccountHead" runat="server" Visible="false">
                    </dx:ASPxComboBox>
                    <asp:Label ID="Label1" Visible="false" runat="server" Text="Received Date" CssClass="lblbox"></asp:Label>
                    <asp:TextBox ID="txtReceivedDate" runat="server" ReadOnly="True" Visible="false"></asp:TextBox>
                    <asp:TextBox ID="txtBankName" runat="server" Width="192px" CssClass="txtBox" Visible="false"></asp:TextBox>
                    <asp:Label runat="server" ID="lblAmount" Visible="false"></asp:Label>
                    <cc1:CalendarExtender ID="CEStartDate" OnClientShown="calendarShown" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                        TargetControlID="txtStartDate"></cc1:CalendarExtender>
                    <cc1:CalendarExtender ID="CalendarExtender1" OnClientShown="calendarShown" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibTransaction"
                        TargetControlID="txtTransactionDate"></cc1:CalendarExtender>
                    <asp:HiddenField ID="HFChqueProcessId" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hfChequeNo" Value="" runat="server"></asp:HiddenField>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
            <hr />
            <div class="row center">
                <div class="col-md-9">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-2">
                                            <strong>
                                                <label id="Label110"><span class="fa fa-caret-right rgt_cart"></span>Search Type</label></strong>
                                        </div>
                                        <div class="col-md-4">
                                            <dx:ASPxComboBox ID="ddSearchType" runat="server" CssClass="form-control" SelectedIndex="0">
                                                <Items>
                                                    <dx:ListEditItem Value="CHEQUE_NO" Text="All Records"></dx:ListEditItem>
                                                    <dx:ListEditItem Value="VENDOR_NAME" Text="Supplier"></dx:ListEditItem>
                                                    <dx:ListEditItem Value="account_name" Text="Bank Account"></dx:ListEditItem>
                                                </Items>
                                            </dx:ASPxComboBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtSeach" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Button ID="btnFilter" runat="server" CssClass="btn box-primary" Text="Filter"
                                                OnClick="btnFilter_Click"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="Panel12" runat="server" Width="100%" Height="200px" ScrollBars="Vertical"
                                            BorderWidth="1px" BorderStyle="Solid" BorderColor="Silver">

                                            <asp:GridView ID="GrdCO" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                AutoGenerateColumns="False" OnRowDeleting="GrdCO_RowDeleting">
                                                <Columns>
                                                    <asp:BoundField DataField="CUSTOMER_ID" HeaderText="VENDOR_ID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Document_no" HeaderText="Inv.No" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Balance" DataFormatString="{0:N}" HeaderText="Amount" ReadOnly="true">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ACCOUNT_NAME" HeaderText="Bank Account" ReadOnly="true">
                                                        <ControlStyle />
                                                        <ItemStyle />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ACCOUNT_HEAD_ID" HeaderText="ACCOUNT_HEAD_ID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DOCUMENT_TYPE_ID" HeaderText="ACCOUNT_HEAD_ID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel" />
                                                        <ItemStyle CssClass="HidePanel" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemStyle CssClass="grdDetail" Width="10%" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnDelete" runat="server" class="fa fa-trash-o" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this?');return false;"
                                                                ToolTip="Delete">            
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="cf head" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <table width="90%">
                                    <tr>
                                        <td style="text-align: center;">
                                            <strong>Total Amount: </strong>
                                            <asp:Label ID="lblTotalAmount" runat="server" Text="0" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>