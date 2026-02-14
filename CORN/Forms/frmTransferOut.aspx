<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmTransferOut.aspx.cs" Inherits="Forms_frmTransferOut" Title="CORN :: Transfer Out against Demand" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" language="javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }

        function Calculations(txtcntrlname) {
            var qty = $(txtcntrlname).val();
            var actualQty = $($(txtcntrlname).closest('tr').find('td:eq(5)')).text();
            var closingQty = $($(txtcntrlname).closest('tr').find('td:eq(4)')).text();

            if (qty == undefined || qty == "" || isNaN(qty) || qty == null) {
                qty = 0;
            }

            if (actualQty == undefined || actualQty == "" || isNaN(actualQty) || actualQty == null) {
                actualQty = 0;
            }

            if (closingQty == undefined || closingQty == "" || isNaN(actualQty) || closingQty == null) {
                closingQty = 0;
            }

            if (parseFloat(qty) > parseFloat(actualQty)) {
                $(txtcntrlname).val(actualQty);
                $(txtcntrlname).focus();
                alert('Quantity can not be greater than Demand Qty');
            }

            if (parseFloat(qty) > parseFloat(closingQty)) {
                $(txtcntrlname).val(closingQty);
                $(txtcntrlname).focus();
                alert('Quantity can not be greater than available Stock');
            }
        }
    </script>
    <div class="main-contents">
        <div class="container">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Demand No</label>
                            <dx:ASPxComboBox ID="drpDocumentNo" runat="server" CssClass="form-control"
                                AutoPostBack="true" SelectedIndex="0"
                                OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Transfer From</label>
                            <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control"
                                Enabled="false">
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Transfer To</label>
                            <dx:ASPxComboBox ID="DrpTransferTo" runat="server" CssClass="form-control"
                                Enabled="false">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <span class="fa fa-caret-right rgt_cart"></span>
                            <asp:Label ID="lblInvoice" runat="server" Text="Driver Name" MaxLength="100" />
                            <asp:TextBox ID="txtDocumentNo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <span class="fa fa-caret-right rgt_cart"></span>
                            <asp:Label ID="Label1" runat="server" Text="Remarks" />
                            <asp:TextBox ID="txtBuiltyNo" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="row center">
                <div class="col-md-9">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" Height="200px" ScrollBars="Vertical" BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                    <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                        Width="100%" OnRowDataBound="GrdPurchase_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="SKU_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_CODE" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_NAME" HeaderText="Item Name" ReadOnly="true">
                                                <ItemStyle Width="35%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UOM_DESC" HeaderText="UOM" ReadOnly="true">
                                                <ItemStyle Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CLOSING_STOCK" HeaderText="Closing Stock" ReadOnly="true">
                                                <ItemStyle Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Quantity" HeaderText="Demand Qty" ReadOnly="true">
                                                <HeaderStyle Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UOM_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Dispatch Qty" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center" Width="25%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQuantity" runat="server" Width="100%" Text='<%# Eval("Quantity")%>' onkeypress="return onlyDotsAndNumbers(this,event);">
                                                    </asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="cf head" />
                                    </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="row" style="margin-top: 15px">
                            <div class="col-md-12 ">
                                <div class="btnlist pull-right">
                                    <asp:Button AccessKey="S" ID="btnSave" runat="server" Text="Save"
                                        UseSubmitBehavior="False" OnClick="btnSave_Click" CssClass="btn btn-success"></asp:Button>
                                    <asp:Button AccessKey="C" ID="btnCancel" runat="server" Text="Cancel"
                                        UseSubmitBehavior="False" CssClass="btn btn-danger" OnClick="btnCancel_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>