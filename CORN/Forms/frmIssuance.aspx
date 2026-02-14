<%@ Page Title="CORN :: Stock Issuance" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmIssuance.aspx.cs" Inherits="Forms_frmIssuance" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript">
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
            }
        }
    </script>
    <script language="JavaScript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }

        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txtQuantity.ClientID%>').value;
            if ((str == null || str.length == 0) && DocNo.GetText() == "New") {
                alert('Must Enter Quantity');
                return false;
            }
            return true;
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

    </script>
    <div class="main-contents">
        <div class="container employee-infomation">

            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:HiddenField runat="server" ID="hfId" />
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Transaction Type</label>

                            <dx:ASPxComboBox ID="DrpDocumentType" runat="server" CssClass="form-control"
                                AutoPostBack="true" SelectedIndex="0"
                                OnSelectedIndexChanged="DrpDocumentType_SelectedIndexChanged">
                                <Items>
                                    <dx:ListEditItem Value="19" Text="Issuance" />
                                    <dx:ListEditItem Value="14" Text="Return" />
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-4">

                            <label><span class="fa fa-caret-right rgt_cart"></span>Document No</label>


                            <dx:ASPxComboBox ID="drpDocumentNo" runat="server" CssClass="form-control"
                                AutoPostBack="true" ClientInstanceName="DocNo"
                                OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">

                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>

                            <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                            </dx:ASPxComboBox>

                        </div>
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Section</label>

                            <dx:ASPxComboBox ID="drpSection" runat="server" CssClass="form-control">
                            </dx:ASPxComboBox>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-md-8">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Remarks</label>
                            <asp:TextBox ID="txtBuiltyNo" runat="server" CssClass="form-control"
                                MaxLength="250"></asp:TextBox>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="row center">
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-2">
                                        <span class="fa fa-caret-right rgt_cart"></span>
                                        <asp:Label ID="Label12" runat="server" Font-Bold="True" Text="Item Description"></asp:Label>
                                    </div>
                                    <div class="col-md-2 reduceSpace">
                                        <asp:CheckBox ID="cbScan" runat="server" Text="By Scan" OnCheckedChanged="cbScan_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                    <div class="col-md-1">
                                        <label><span class="fa fa-caret-right rgt_cart"></span>UOM</label>
                                    </div>
                                    <div class="col-md-2">
                                        <span class="fa fa-caret-right rgt_cart"></span>

                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Quantity"></asp:Label>
                                    </div>
                                </div>
                                <asp:Panel ID="ItemPnl" runat="server" DefaultButton="btnSave">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <dx:ASPxComboBox ID="ddlSkus" runat="server" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSkus_SelectedIndexChanged"
                                                ClientInstanceName="ddlItem" ClientSideEvents-SelectedIndexChanged="function(s,e){ddlItemIndexChanged();}">
                                            </dx:ASPxComboBox>
                                            <asp:TextBox ID="txtSKUCode" runat="server" Visible="false" CssClass="form-control" onkeypress="return SKUCodeKeyPress(event);"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:TextBox ID="txtUOM" runat="server" CssClass="form-control"
                                                Enabled="false"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"
                                                onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:HiddenField ID="hfInventoryType" runat="server" Value="0" />
                                            <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server"
                                                Text="Add" CssClass="btn btn-success" />
                                        </div>
                                        <div class="col-md-2">
                                            <label runat="server" id="lblStock"><span class="fa fa-caret-right rgt_cart"></span>Stock</label>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:HiddenField ID="_rowNo" runat="server" Value="0" />
                                <asp:HiddenField ID="_privouseQty" runat="server" Value="0" />
                                <asp:Panel ID="Panel2" runat="server" Width="715px" Height="160px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                    <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False"
                                        CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                        OnRowDeleting="GrdPurchase_RowDeleting" OnRowEditing="GrdPurchase_RowEditing"
                                        Width="100%" OnRowDataBound="GrdPurchase_RowDataBound" ShowFooter="true">

                                        <Columns>
                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_CODE" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_NAME" HeaderText="Item" ReadOnly="true">
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="UOM" DataField="UOM_DESC" ReadOnly="true">
                                                <ItemStyle Width="80px" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Qty" DataField="FREE_SKU" ReadOnly="true">
                                                <ItemStyle HorizontalAlign="Right" Width="75px" />
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
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                        CssClass=" fa fa-trash-o"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                            </asp:TemplateField>
                                        </Columns>


                                    </asp:GridView>
                                </asp:Panel>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-7" style="margin-top: 15px">
                    <div class="btnlist pull-right">
                        <asp:Button AccessKey="S" ID="btnSaveDocument" runat="server"
                            Text="Save" UseSubmitBehavior="False" OnClick="btnSaveDocument_Click"
                            CssClass="btn btn-success" />
                        <asp:Button AccessKey="C" ID="btnCancel" runat="server"
                            Text="Cancel" UseSubmitBehavior="False" OnClick="btnCancel_Click" CssClass="btn btn-danger" />
                    </div>
                </div>
                <div class="col-md-2" style="display: none">
                    <strong>Total Quantity</strong>
                    <asp:TextBox ID="txtTotalQuantity" runat="server" CssClass="form-control"
                        Enabled="true"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
</asp:Content>