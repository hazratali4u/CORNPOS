<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmInvoiceBooking.aspx.cs" Inherits="Forms_frmInvoiceBooking" Title="CORN :: Invoice Booking" %>

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

        function GetNetAmount() {

            var gross = document.getElementById('<%=txtGross.ClientID%>');
            var discount = document.getElementById('<%=txtDiscount.ClientID%>');
            var gst = document.getElementById('<%=txtGst.ClientID%>');
            var gstadvance = document.getElementById('<%=txtAdvanceTax.ClientID%>');
            var netAmount = document.getElementById('<%=txtNetAmount.ClientID%>');
            var gross1 = gross.value;
            var discount1 = discount.value;
            var gst1 = gst.value;
            var gstadvance1 = gstadvance.value;
            if (gross.value == "") {
                gross1 = 0;
            }
            if (discount.value == "") {
                discount1 = 0;
            }
            if (gst.value == "") {
                gst1 = 0;
            }
            if (gstadvance.value == "")
            {
                gstadvance1 = 0;
            }
            var netValue = parseInt(gross1) - parseInt(discount1) + parseInt(gst1) + parseFloat(gstadvance1);
            netAmount.value = netValue;
        }
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
        function ValidateForm() {
            var str;
            str = document.getElementById('<%=txtInvNo.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Invoice no. is required');
                return false;
            }
            str = document.getElementById('<%=txtGross.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Gross amount required');
                return false;
            }
            str = document.getElementById('<%=txtInvDate.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Invoice date is required');
                return false;
            }
            return true;
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
                                <asp:LinkButton class="btn btn-success" OnClick="btnActive_Click" ID="btnActive" runat="server"
                                    OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                         </div>
                         </div>
                </asp:Panel>
            </div>

            <div class="row top" runat="server" id="contentBox" style="margin: 0 0 0 0;">

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>

                        <div style="z-index: 101; left: 40px; width: 100px; position: absolute; top: 5px; height: 100px"
                            id="DIV1">
                            <asp:UpdateProgress ID="UpdateProgress5" runat="server">
                                <ProgressTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" Width="26px" Height="23px" ImageUrl="~/OrderPOS/images/ajax-loader1.gif"></asp:ImageButton>&nbsp; Loading....
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                        <div class="row">
                            <div class="col-md-10">
                                <asp:Literal ID="lblErrorMsg" runat="server" Visible="false"></asp:Literal>
                                <asp:HiddenField ID="hfDocumentID" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                <dx:ASPxComboBox ID="ddDistributorId" runat="server" CssClass="form-control" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddDistributorId_SelectedIndexChanged">
                                </dx:ASPxComboBox>
                            </div>
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Contract Type</label>
                                <dx:ASPxComboBox ID="drpContractType" runat="server" CssClass="form-control" AutoPostBack="True"
                                    OnSelectedIndexChanged="drpContractType_SelectedIndexChanged">
                                    <Items>
                                        <dx:ListEditItem Text="Supplier" Value="1" Selected="true" />
                                        <dx:ListEditItem Text="Franchise" Value="2" />
                                    </Items>
                                </dx:ASPxComboBox>
                            </div>
                            <div class="col-md-4">
                                <label runat="server" id="loadType"><span class="fa fa-caret-right rgt_cart"></span>Supplier</label>
                                <dx:ASPxComboBox ID="DrpSupplier" runat="server" CssClass="form-control">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                        <div class="row">
                             <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Invoice No.</label>
                                <asp:TextBox ID="txtInvNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                             <div class="col-md-3">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Invoice Date</label>
                                <asp:TextBox ID="txtInvDate" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                    Width="30px" Style="margin-top: 27px" />
                                <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                    TargetControlID="txtInvDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                            </div>
                             <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Gross Amount</label>
                                <asp:TextBox ID="txtGross" runat="server" onkeyup="GetNetAmount();" CssClass="form-control"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom"
                                    ValidChars="0123456789." TargetControlID="txtGross"></cc1:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Discount</label>
                                <asp:TextBox ID="txtDiscount" runat="server" onkeyup="GetNetAmount();" MaxLength="40" CssClass="form-control"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom"
                                    ValidChars="0123456789." TargetControlID="txtDiscount"></cc1:FilteredTextBoxExtender>
                            </div>
                            <div class="col-md-3">
                                <label><span class="fa fa-caret-right rgt_cart"></span>G.S.T</label>
                                <asp:TextBox ID="txtGst" runat="server" onkeyup="GetNetAmount();" CssClass="form-control"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom"
                                    ValidChars="0123456789." TargetControlID="txtGst"></cc1:FilteredTextBoxExtender>
                            </div>
                            <div class="col-md-3">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Advance Tax</label>
                                <asp:TextBox ID="txtAdvanceTax" runat="server" onkeyup="GetNetAmount();" CssClass="form-control"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Custom"
                                    ValidChars="0123456789." TargetControlID="txtAdvanceTax"></cc1:FilteredTextBoxExtender>
                            </div>
                             <div class="col-md-3">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Net Amount</label>
                                <asp:TextBox ID="txtNetAmount" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-10">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Remarks</label>
                                <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        </ContentTemplate>
                </asp:UpdatePanel>
                <div class="container role_management">
                        <div class="module_contents" style="padding: 0; border: none;">
                            <div class="col-md-6">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Detail</label>

                            </div>
                        </div>
                        <div class="module_contents" style="width:84%;margin-bottom:10px;padding: 20px 0%;border:solid 1px">
                            <div class="row">
                                <div class="col-md-5">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Debit Account Head</label>
                                    <dx:ASPxComboBox ID="ddlAccountHead" runat="server" CssClass="form-control">
                                    </dx:ASPxComboBox>
                                </div>
                                <div class="col-md-3">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Net Amount</label>
                                <asp:TextBox ID="txtDetailNetAmount" runat="server" onkeyup="GetNetAmount();" CssClass="form-control"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Custom"
                                    ValidChars="0123456789." TargetControlID="txtDetailNetAmount"></cc1:FilteredTextBoxExtender>
                            </div>
                                <div class="col-md-1">
                                <asp:Button AccessKey="A" Style="margin-top:23px;"
                                    ID="btnAdd_Item" OnClick="btnAddItem_Click" runat="server" Text="Add" CssClass="btn btn-success" />
                            </div>
                            </div>

                            <div class="row">
                                <div class="col-md-9">
                                    <asp:HiddenField ID="_rowNo2" runat="server" Value="0" />
                                    <asp:Panel ID="Panel2" runat="server" Height="300" BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver" ScrollBars="Vertical">
                                        <asp:GridView ID="grdCOA" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                            OnRowDeleting="grdCOA_RowDeleting" OnRowEditing="grdCOA_RowEditing"
                                            AutoGenerateColumns="false" ShowHeader="true">
                                            <Columns>
                                                <asp:BoundField DataField="ACCOUNT_HEAD" HeaderText=" Debit Account Head " ReadOnly="true">
                                                    <ItemStyle CssClass="grdDetail" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AMOUNT" ReadOnly="true" HeaderText="Net Amount" DataFormatString="{0:N2}">
                                                    <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ACCOUNT_HEAD_ID" ReadOnly="true">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
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
                                            <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>
                            </div>
                            </div>
                    
                <div class="row">
                    <div class="col-md-offset-5 col-md-3 ">
                        <div class="btnlist pull-right">
                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn btn-success" />
                            <asp:LinkButton CssClass="btn btn-danger" runat="server" ID="btnCancel" Text="Cancel"
                                OnClick="btnClose_Click">
                               <span class="fa fa-close"></span>Cancel
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row center" runat="server" id="lookupBox">
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:GridView ID="Grid_users" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                            AutoGenerateColumns="False" OnPageIndexChanging="Grid_users_PageIndexChanging" PageSize="8" AllowPaging="true"
                            OnRowEditing="Grid_users_RowEditing" EmptyDataText="No Record exist">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="PURCHASE_MASTER_ID" HeaderText="PURCHASE_MASTER_ID" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DISTRIBUTOR_ID" HeaderText="DISTRIBUTOR_ID" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel "></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SUPPLIER" HeaderText="Supplier" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="20%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ORDER_NUMBER" HeaderText="Invoice Number" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>

                                <asp:BoundField DataField="DOCUMENT_DATE2" HeaderText="Document Date" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SOLD_TO" HeaderText="SOLD_TO" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SOLD_FROM" HeaderText="SOLD_FROM" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TOTAL_AMOUNT" HeaderText="TOTAL_AMOUNT" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DEBIT_AMOUNT" HeaderText="DEBIT_AMOUNT" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="GST_AMOUNT" HeaderText="GST_AMOUNT" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DISCOUNT" HeaderText="GST_AMOUNT" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NET_AMOUNT" HeaderText="GST_AMOUNT" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="REMARKS" HeaderText="Remarks" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="25%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="STATUS" HeaderText="Status" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ACCOUNT_HEAD_ID" HeaderText="ACCOUNT_HEAD_ID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" Width="10%"></ItemStyle>
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CONTRACT_TYPE" HeaderText="CONTRACT_TYPE" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" Width="10%"></ItemStyle>
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GST_ADVANCE" HeaderText="GST_ADVANCE" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                    <HeaderStyle CssClass="HidePanel" />
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
