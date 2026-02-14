<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmPhysicalStockTaking.aspx.cs" Inherits="Forms_frmPhysicalStockTaking" Title="CORN :: Physical Stock Taking" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <style>
    /*.modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=40);
        opacity: 0.4;
    }*/
    .modalPopup
    {
        background-color: ghostwhite;
        width: 360px;
        border: 3px solid #e2c200;
    }
    .modalPopup .header
    {
        background-color: #e2c200;
        height: 30px;
        color: White;
        line-height: 30px;
        text-align: center;
        font-weight: bold;
    }
    .modalPopup .body
    {
        min-height: 50px;
        line-height: 30px;
        text-align: center;
        font-weight: bold;
    }
    .modalPopup .footer
    {
        padding: 3px;
    }
</style>
    <script type="text/javascript">
        function ddlItemIndexChanged(s, e) {
            hfInventoryType = document.getElementById('<%=hfInventoryType.ClientID%>').value;
            document.getElementById('<%=txtQuantity.ClientID%>').focus();
            <%--if (hfInventoryType == '0') {
                document.getElementById('<%=txtQuantity.ClientID%>').focus();
            }
            {
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
                        data = data.replace(/[^\d.,]+/, '');
                        document.getElementById('<%=txtQuantity.ClientID%>').value = data.replace(/[^\d.,]+/, '');
                    }
                });
            }
        }
    </script>
    <script language="JavaScript" type="text/javascript">
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
                        </div>
                     </div>
                </asp:Panel>
                </div>
            <div class="row top" runat="server" id="contentBox" style="margin: 0 0 0 0;">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-8">
                            <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
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
                    </div>
                    <div class="row">
                          <div class="col-md-2" style="margin:5px;">
                            <asp:LinkButton CssClass="btn btn-primary" runat="server" ID="btnImport" Text="Import"
                                OnClick="btnImport_Click">
                                <span class="fa fa-plus-circle"></span>Import
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-3" style="margin-top:10px;" runat="server" id="secAuto">
                           <asp:CheckBox runat="server" ID="chkAutoAdjust" Checked="false" /> Auto Stock Adjustment
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
                                    <button type="button" id="btnClose_Import" class="close" runat="server" onserverclick="btnClose_Import_ServerClick">
                                        <span>&times;</span><span class="sr-only">Close</span></button>
                                    <h1 class="modal-title" id="myModalLabel1">
                                        <span></span>Import Physical Stock Taking</h1>
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
                                                        <asp:FileUpload ID="txtFile" runat="server" />
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
                                            <asp:PostBackTrigger ControlID="btnImportOpening" />
                                            <asp:PostBackTrigger ControlID="btnExportOpeningTemplate" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-md-6">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Remarks</label>
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Item Description</label> &nbsp;&nbsp;&nbsp; (<asp:CheckBox id="cbScan" runat="server" Text="By Scan"
                                OnCheckedChanged="cbScan_CheckedChanged" AutoPostBack="true" />)
                        </div>
                        <div class="col-md-1">
                            <label><span class="fa fa-caret-right rgt_cart"></span>UOM</label>
                        </div>
                        <div class="col-md-2">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Quantity</label>
                        </div>
                    </div>
                    <asp:Panel ID="pnlItemDetail" runat="server" DefaultButton="btnSave">
                        <div class="row">
                        <div class="col-md-4">
                            <dx:ASPxComboBox ID="ddlSkus" runat="server" CssClass="form-control"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlSkus_SelectedIndexChanged"
                                ClientInstanceName="ddlItem" ClientSideEvents-SelectedIndexChanged="function(s,e){ddlItemIndexChanged();}">
                            </dx:ASPxComboBox>
                            <asp:TextBox ID="txtItemCode" runat="server" CssClass="form-control"
                                Visible="false"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:TextBox ID="txtUOM" runat="server" CssClass="form-control"
                                Enabled="false"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Text="1"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:HiddenField ID="hfInventoryType" runat="server" Value="0" />
                            <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Add"
                                ValidationGroup="vg" CssClass="btn btn-success" />
                        </div>
                    </div>
                    </asp:Panel>

                    <div class="row center">
                        <div class="col-md-12">
                            <div class="emp-table">
                                <asp:HiddenField ID="_rowNo" runat="server" Value="0" />
                                <asp:Panel ID="Panel2" runat="server" Width="715px" Height="260px" ScrollBars="Vertical"
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
                                            <asp:BoundField HeaderText="Item" DataField="SKU_NAME" ReadOnly="true">
                                                <ItemStyle HorizontalAlign="Left"
                                                    Width="50%" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="UOM" DataField="UOM_DESC" ReadOnly="true">
                                                <ItemStyle Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Qty" DataField="Quantity" ReadOnly="true">
                                                <ItemStyle HorizontalAlign="Right"
                                                    Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="ERPCode" DataField="strERPCode" ReadOnly="true">
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
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
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
                <div class="col-md-offset-3 col-md-4">
                    <div class="btnlist pull-right">
                        <asp:Button ID="btnSaveDocument" AccessKey="S" OnClick="btnSaveDocument_Click" runat="server" Text="Save" UseSubmitBehavior="False" CssClass="btn btn-success" />
                        
                        <asp:LinkButton CssClass="btn btn-danger" runat="server" ID="btnCancel" Text="Cancel"
                                OnClick="btnClose_Click">
                               <span class="fa fa-close"></span>Cancel
                            </asp:LinkButton>
                    </div>
                </div>
            </div>
             <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" DisplayModalPopupID="mpe" TargetControlID="btnSaveDocument">
                </ajaxToolkit:ConfirmButtonExtender>
                <ajaxToolkit:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="btnSaveDocument" OkControlID = "btnYes"
                    CancelControlID="btnNo" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
                    <div class="header">
                        Confirmation
                    </div>
                    <div class="body">
                        Once record saved, cannot be modified again... <br />
                         Do you still want to save?
                    </div>
                    <div class="footer" align="right">
                        <asp:Button ID="btnYes" CssClass="btn btn-danger" runat="server" Text="Yes" />
                        <asp:Button ID="btnNo" CssClass="btn btn-success" runat="server" Text="No" />
                    </div>
                </asp:Panel>
        </div>
            <div class="row center" runat="server" id="lookupBox">
                <asp:HiddenField ID="hfPhysical_Stock_ID" runat="server" Value="0" />
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:GridView ID="Grid_users" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                            AutoGenerateColumns="False" OnPageIndexChanging="Grid_users_PageIndexChanging" PageSize="20" AllowPaging="true"
                            OnRowEditing="Grid_users_RowEditing" EmptyDataText="No Record exist"
                            OnRowDataBound="Grid_users_RowDataBound" OnRowDeleting="Grid_users_RowDeleting">
                            <Columns>
                                <%--<asp:TemplateField>
                                    <HeaderTemplate>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                                <asp:BoundField DataField="Physical_Stock_Master_ID" HeaderText="Doc No" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DISTRIBUTOR_ID" HeaderText="Distributor" ReadOnly="true">
                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                    <ItemStyle CssClass="HidePanel "></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="Date" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>
                                <asp:BoundField DataField="REMARKS" HeaderText="Remarks" ReadOnly="true">
                                    <ItemStyle CssClass="grdDetail" Width="46%"></ItemStyle>
                                    <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                </asp:BoundField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" class="fa fa-pencil" CommandName="Edit" ToolTip="Edit">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="7%" HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete?');return false;"
                                            class="fa fa-trash-o"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="7%" />
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