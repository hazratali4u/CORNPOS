<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmOpeningStock.aspx.cs" Inherits="Forms_frmOpeningStock" Title="CORN :: Opening Stock" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
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
                //Do nothing.
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
       
        function ConfirmDelete() {
            if (confirm("Do you want to Cancel this record?") == true)
                return true;

            else {
                return false;
            }
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


    </script>
    <div class="main-contents">
        <div class="container">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-8">
                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3" style="display:none;">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Transaction Type</label>
                           
                            <dx:ASPxComboBox ID="DrpDocumentType" runat="server"  CssClass="form-control"
                                SelectedIndex="0">
                                <Items>
                                    <dx:ListEditItem Value="7" Text="Opening Stock"/>
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Document No</label>
                             <dx:ASPxComboBox ID="drpDocumentNo" runat="server"  CssClass="form-control"
                                AutoPostBack="true" ClientInstanceName="DocNo"
                                OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged" >
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                             <dx:ASPxComboBox ID="drpDistributor" runat="server"  CssClass="form-control"
                                AutoPostBack="true" 
                                OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-3">
                            <asp:LinkButton CssClass="btn btn-primary" runat="server" ID="btnImport" Text="Add"
                                OnClick="btnImport_Click">
                                <span class="fa fa-plus-circle"></span>Import
                            </asp:LinkButton>
                        </div>
                    </div>
                    <ajaxToolkit:ModalPopupExtender ID="mPOPImport" runat="server" PopupControlID="pnlImportForm"
                                                TargetControlID="btnImport" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                                                CancelControlID="btnClose_Import">
                                            </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="pnlImportForm" runat="server" Style="display: none; width: 30%" ScrollBars="Auto">
                                                <div class="modal-dialog" style="width: 100%">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" id="btnClose_Import" class="close" runat="server" onserverclick="btnImportClose_Click">
                                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                                            <h1 class="modal-title" id="myModalLabel1">
                                                                <span></span>Import Opening</h1>
                                                        </div>
                                                                <div class="modal-body">
                                                                    <asp:UpdatePanel ID="importUpdatePanel" runat="server">
                                                                        <ContentTemplate> 
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <div id="Div1" runat="server" visible="true">
                                                                            <label>
                                                                                <span class="fa fa-caret-right rgt_cart"></span>Upload Excel
                                                                                    &nbsp;(.xlsx | .xls)
                                                                            </label>
                                                                            <asp:FileUpload id="txtFile" runat="server" />
                                                                        </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px;">
                                                                        <div class="col-md-7">
                                                                        </div>
                                                                        <div class="col-md-12" align="right">
                                                                            <asp:Button ID="btnExportOpeningTemplate" OnClick="btnExportOpeningTemplate_Click" runat="server" Text="Download Template" CssClass="btn btn-primary" />
                                                                            <asp:Button ID="btnImportOpening" OnClick="btnImportOpening_Click" runat="server" Style="margin-left: 5px" Text="Import" CssClass="btn btn-success" />
                                                                        </div>
                                                                    </div>
                                                                    </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID = "btnImportOpening" />
                                                                            <asp:PostBackTrigger ControlID = "btnExportOpeningTemplate" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                    <div class="row">
                        <div class="col-md-6">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Remarks</label>
                            <asp:TextBox ID="txtDocumentNo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            
                           <dx:ASPxComboBox ID="ddlSkus" runat="server"  CssClass="form-control" 
                               AutoPostBack="true" OnSelectedIndexChanged="ddlSkus_SelectedIndexChanged"
                               ClientInstanceName="ddlItem" ClientSideEvents-SelectedIndexChanged = "function(s,e){ddlItemIndexChanged();}">
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtUOM" runat="server" CssClass="form-control"
                                Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Text="1"
                                onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" 
                                onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:HiddenField ID="hfInventoryType" runat="server" Value="0" />
                            <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Add" 
                                ValidationGroup="vg" CssClass="btn btn-success" />
                        </div>
                    </div>

                    <div class="row center">
                        <div class="col-md-12">
                            <div class="emp-table">
                                <asp:HiddenField ID="_rowNo" runat="server" Value="0" />
                                <asp:Panel ID="Panel2" runat="server" Width="60%" Height="180px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                    <asp:GridView ID="GrdPurchase" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                        AutoGenerateColumns="False"
                                        OnRowDeleting="GrdPurchase_RowDeleting" OnRowEditing="GrdPurchase_RowEditing">
                                        <Columns>
                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_CODE" HeaderText="SKU Code" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Item" DataField="SKU_NAME"  ReadOnly="true">
                                                <ItemStyle HorizontalAlign="Left"
                                                    Width="40%" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="UOM" DataField="UOM_DESC" ReadOnly="true">
                                                    <ItemStyle Width="20%" />
                                                </asp:BoundField>
                                            <asp:BoundField HeaderText="Qty" DataField="Quantity"  ReadOnly="true">
                                                <ItemStyle HorizontalAlign="Right"
                                                    Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Price" DataField="PRICE"  ReadOnly="true">
                                                <ItemStyle HorizontalAlign="Right"
                                                    Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UOM_ID"  ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" ToolTip="Edit">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete?');return false;"
                                                        class="fa fa-trash-o"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10%"/>
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
