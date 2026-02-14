<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmSupplierAdvanceKnockOff.aspx.cs" Inherits="Forms_frmSupplierAdvanceKnockOff" Title="CORN :: Supplier Advance knock Off" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">

    <script language="JavaScript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <div class="main-contents">
                    <div class="container role_management">
                        <div class="row">
                            <div class="col-md-4">
                                <label>
                                    <span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                <dx:ASPxComboBox ID="ddlLocation" runat="server" CssClass="form-control"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                </dx:ASPxComboBox>
                            </div>
                            <div class="col-md-4">
                                <label>
                                    <span class="fa fa-caret-right rgt_cart"></span>Supplier</label>
                                <dx:ASPxComboBox ID="ddlSupplier" runat="server" CssClass="form-control"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-8">
                                <label>
                                    <span class="fa fa-caret-right rgt_cart"></span>Remarks</label>
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="module_contents" style="padding: 0; border: none;">
                            <div class="col-md-6">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Pending Advances</label>

                            </div>
                            <div class="col-md-6">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Pending Invoices</label>
                            </div>
                        </div>
                        <div class="module_contents" style="margin-bottom:10px;padding: 20px 0%;">
                            <div class="col-md-6">
                                <asp:Panel ID="Panel2" runat="server" Height="300" ScrollBars="Vertical">
                                <asp:GridView ID="grdAdvance" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                    DataKeyNames="PURCHASE_MASTER_ID" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChbIsAssigned" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="INVOICE_NO" HeaderText="INV No">
                                            <ItemStyle CssClass="grdDetail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="  INV Date  " DataFormatString="{0:dd/MMM/yyyy}">
                                            <ItemStyle CssClass="grdDetail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CURRENT_CREDIT_AMOUNT" HeaderText="Balance" DataFormatString="{0:N2}">
                                            <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" />
                                        </asp:BoundField>         
                                        <asp:BoundField DataField="PAYMENT_MODE">
                                            <ItemStyle CssClass="hidden"/>
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ACCOUNT_HEAD_ID" >
                                            <ItemStyle CssClass="hidden"/>
                                            <HeaderStyle CssClass="hidden" />
                                        </asp:BoundField>
                                    </Columns>
                                    <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                </asp:GridView>
                                    </asp:Panel>
                            </div>
                            <div class="col-md-6">
                                <asp:Panel ID="pnlInvocie" runat="server" Height="300" ScrollBars="Vertical">
                                <asp:GridView ID="grdInvoice" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                    DataKeyNames="PURCHASE_MASTER_ID" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChbIsAssigned" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="INVOICE_NO" HeaderText="INV No">
                                            <ItemStyle CssClass="grdDetail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="  INV Date  " DataFormatString="{0:dd/MMM/yyyy}">
                                            <ItemStyle CssClass="grdDetail" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CURRENT_CREDIT_AMOUNT" HeaderText="Invoice Amnt." DataFormatString="{0:N2}">
                                            <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BALANCE" HeaderText="Balance" DataFormatString="{0:N2}">
                                            <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" />
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="KNOCK_OFF" HeaderText="Knock off Amnt" DataFormatString="{0:N2}">
                                            <ItemStyle CssClass="grdDetail" HorizontalAlign="Right" />
                                        </asp:BoundField>                                        
                                    </Columns>
                                    <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                </asp:GridView>
                                    </asp:Panel>
                            </div>

                        </div>
                        <div class="module_contents" style="padding: 0; border: none;">
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-1">
                                        <strong><label>Total: </label></strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtAdvanceTotal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-1">
                                        <strong><label>Total: </label></strong>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtInvoicesTotal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="row">
                <div class="col-md-5">
                </div>
                <div class="col-md-2">
                    <asp:Button class="btn btn-success" Text="Save" runat="server" ID="btnSave" TabIndex="9" OnClick="btnSave_Click"></asp:Button>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>