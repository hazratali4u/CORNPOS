<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmUserLogin.aspx.cs" Inherits="Forms_frmUserLogin" Title="CORN :: User Login" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
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

        function ShowPassword(value) {
            if ($(value).hasClass('fa fa-eye-slash')) {
                $(value).prev('input').attr('type', 'text');
                $(value).attr('class', 'fa fa-eye');
            }
            else {
                $(value).prev('input').attr('type', 'password');
                $(value).attr('class', 'fa fa-eye-slash');
            }
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
                    <div class="col-md-1" style="margin-left: -60px;">
                        <asp:LinkButton ID="btnsearch" OnClick="btnFilter_Click" runat="server" Text="Search"
                            CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                    </div>
                    <asp:LinkButton ID="btndummy" runat="server" UseSubmitBehavior="false" />
                </asp:Panel>
                <div class="col-md-offset-4 col-md-3" style="float: right;">
                    <div class="btnlist pull-right">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd" Text="Add"
                                    OnClick="btnAdd_Click">
                               <span class="fa fa-plus-circle"></span>Add
                                </asp:LinkButton>
                                <asp:LinkButton class="btn btn-success" OnClick="btnActive_Click" ID="btnActive" runat="server"
                                    OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                                <asp:LinkButton runat="server" ID="hbtn"></asp:LinkButton>
                                <!-- POP UP MODEL-->
                                <cc1:ModalPopupExtender ID="mPopUpLocation" runat="server" PopupControlID="pnlParameters"
                                    TargetControlID="hbtn" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                                    CancelControlID="btnClose">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="pnlParameters" runat="server" Style="display: none;" ScrollBars="Auto" DefaultButton="btnSave">
                                    <div class="modal-dialog" style="width: 630px; margin-left: 0px">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_Click">
                                                    <span>&times;</span><span class="sr-only">Close</span></button>
                                                <h1 class="modal-title" id="myModalLabel">
                                                    <span></span>Add User Information</h1>
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
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Base Location</label>
                                                                <dx:ASPxComboBox ID="ddDistributorId" runat="server" CssClass="form-control"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddDistributorId_SelectedIndexChanged">
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Name</label>


                                                                <dx:ASPxComboBox ID="DrpUser" runat="server" CssClass="form-control">
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Roles</label>
                                                                <dx:ASPxComboBox ID="ddRole" runat="server" CssClass="form-control">
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div style="display: block; overflow: hidden; height: 0px">
                                                                <asp:TextBox ID="txtFakeUserID" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <asp:TextBox ID="txtFakePasssword" TextMode="Password" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Login ID</label>
                                                                <asp:RequiredFieldValidator ID="rfvtxtLoginId" runat="server" ErrorMessage="Login ID is required"
                                                                    ControlToValidate="txtLogId" ValidationGroup="vg">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:TextBox ID="txtLogId" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Password</label>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Password is required"
                                                                    ValidationGroup="vg" ControlToValidate="txtPswd">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:TextBox ID="txtPswd" TextMode="Password" runat="server" MaxLength="40" CssClass="form-control"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="row">

                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkCan_DineIn" runat="server" Font-Bold="true"></asp:CheckBox>
                                                                <label>Can DineIn</label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkCan_Delivery" runat="server" Font-Bold="true"></asp:CheckBox>
                                                                <label>Can Delivery</label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkCan_TakeAway" runat="server" Font-Bold="true"></asp:CheckBox>
                                                                <label>Can TakeAway</label>
                                                            </div>
                                                        </div>
                                                        <div class="row">

                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkright" runat="server" Font-Bold="true"></asp:CheckBox>
                                                                <label>Item Delete</label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkright2" runat="server" Font-Bold="true"></asp:CheckBox>
                                                                <label>Item Less</label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkCashier" runat="server" Font-Bold="true"></asp:CheckBox>
                                                                <label>IS Cashier</label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkIS_CanGiveDiscount" runat="server" Font-Bold="true"></asp:CheckBox>
                                                                <label>Can give Discount</label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkIS_CanReverseDayClose" runat="server" Font-Bold="true"></asp:CheckBox>
                                                                <label>Can Reverse DayClose</label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="cbCanComplimentaryItem" runat="server" Font-Bold="true"></asp:CheckBox>
                                                                <label>Complimentary Item</label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="cbOrderPrint" runat="server" Font-Bold="true" Checked="true"></asp:CheckBox>
                                                                <label>Duplcate Order Print</label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="cbServiceCharges" runat="server" Font-Bold="true" Checked="true"></asp:CheckBox>
                                                                <label>Can Alter Ser. Charges</label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="cbDeliveryCharges" runat="server" Font-Bold="true" Checked="true"></asp:CheckBox>
                                                                <label>Can Alter Del. Charges</label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="chkAutoStockAdjustment" runat="server" Font-Bold="true" Checked="false"></asp:CheckBox>
                                                                <label>Can Adjust Stock</label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="cbCanCancelTableReservation" runat="server" Font-Bold="true" Checked="false"></asp:CheckBox>
                                                                <label>Can Cancel Reservation</label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="cbSplitBill" runat="server" Font-Bold="true" Checked="false"></asp:CheckBox>
                                                                <label>Split Bill</label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Default Service Type</label>
                                                                <dx:ASPxComboBox ID="ddlServiceType" runat="server" CssClass="form-control">
                                                                    <Items>
                                                                        <dx:ListEditItem Value="0" Text="Select" Selected="true" />
                                                                        <dx:ListEditItem Value="1" Text="Dine In" />
                                                                        <dx:ListEditItem Value="2" Text="Delivery" />
                                                                        <dx:ListEditItem Value="3" Text="Takeaway" />
                                                                    </Items>
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:CheckBox ID="cbPrintInvoice" runat="server" Font-Bold="true"
                                                                    AutoPostBack="true" OnCheckedChanged="cbPrintInvoice_CheckedChanged"></asp:CheckBox>
                                                                <label>Print Invocie from Service</label>
                                                                <asp:TextBox ID="txtPrinterName" runat="server" CssClass="form-control" placeholder="Printer Name" Enabled="false"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="btnlist pull-right">
                                                                    <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" ValidationGroup="vg" CssClass="btn btn-success" />
                                                                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel" CssClass="btn btn-danger" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="row center">
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanelDetail" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="Grid_users" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                    AutoGenerateColumns="False" OnPageIndexChanging="Grid_users_PageIndexChanging" PageSize="20" AllowPaging="true"
                                    OnRowEditing="Grid_users_RowEditing" EmptyDataText="No Record exist"
                                    OnRowDataBound="Grid_users_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="USER_ID" HeaderText="User Id" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Distributor_ID" HeaderText="Distributor" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="USER_NAME" HeaderText="Name" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="20%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LOGIN_ID" HeaderText="Login" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="PASSWORD" HeaderText="Password" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="HidePanel" />
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderText="Password">
                                            <ItemTemplate>
                                                <input type="password" readonly="readonly" style="border: none; background-color: #f9f9f9" value="<%#Eval("PASSWORD") %>" />
                                                <span class="fa fa-eye-slash" onclick="ShowPassword(this)" style="cursor: pointer;"></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="role_name" HeaderText="Role" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="18%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ROLE_ID" HeaderText="ROLE_ID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="isDelRight" HeaderText="isDelRight" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="isLessRight" HeaderText="isLessRight" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status" ReadOnly="true">
                                            <ItemStyle Width="8%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IS_CASHIER" HeaderText="IS_CASHIER" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IS_CanGiveDiscount" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IS_CanReverseDayClose" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Can_DineIn" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Can_Delivery" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Can_TakeAway" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Can_ComplimentaryItem" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Can_PrintOrder" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CanAlterServiceCharges" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DefaultServiceType" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PRINTER_NAME" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PrintInvoiceFromWS" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="AutoStockAdjustment" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CanAlterDeliveryCharges" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CanCancelTableReservation" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsSplitBill" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
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
</asp:Content>