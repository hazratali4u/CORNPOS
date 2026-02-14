<%@ Page Language="C#" Title="CORN :: Split Item" AutoEventWireup="true" CodeFile="frmSplitItem.aspx.cs"
    Inherits="Forms_frmSplitItem" MasterPageFile="~/Forms/PageMaster.master" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
    <link href="../css/Popup.css" rel="stylesheet" />
    <script type="text/javascript" src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
    <script language="JavaScript" type="text/javascript">

        function ValidateForm() {
            var str;
            str = document.getElementById('<%=txtQuantity.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Quantity is required');
                return false;
            }

            str = document.getElementById('<%=txtPrice.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Price is required');
                return false;
            }

            if (parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value) < 1) {
                alert('Quantity must be greater than 0');
                return false;
            }
            return true;
        }

        function ValidateActualQty() {
            var str;
        }

        function pageLoad() {

            var popUp = $find('ModelPopup');

            //check it exists so the script won't fail
            if (popUp) {
                //Add the function below as the event
                popUp.add_hidden(HidePopupPanel);
            }
        }

        function HidePopupPanel(source, args) {
            //find the panel associated with the extender
            objPanel = document.getElementById(source._PopupControlID);

            //check the panel exists
            if (objPanel) {
                //set the display attribute, so it remains hidden on postback
                objPanel.style.display = 'none';
            }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
        prm.add_beginRequest(BeginRequestHandler);
        // Raised after an asynchronous postback is finished and control has been returned to the browser.
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            //Shows the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();
            }
        }

        function EndRequestHandler(sender, args) {
            //Hide the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
    </script>

    <div class="main-contents">
        <div class="row center">
            <asp:Panel ID="Panel4" runat="server" DefaultButton="btnsearch">
                <div class="col-md-4">
                    <div class="search">
                        <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" CssClass="form-control"
                            TabIndex="0"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-1" style="margin-left: -60px;">
                    <asp:LinkButton ID="btnsearch" OnClick="btnsearch_Click" runat="server" Text="Search"
                        CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                </div>
            </asp:Panel>
            <div class="col-md-offset-4 col-md-3" style="float:right;">
                <div class="btnlist pull-right">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd2" Text="Add"
                                OnClick="btnAdd2_Click">
                                           <span class="fa fa-plus-circle"></span>Add
                            </asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="mPopUpSection" runat="server" PopupControlID="pnlParameters"
                                TargetControlID="btnAdd2" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                                CancelControlID="btnClose">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="pnlParameters" runat="server" Style="display: none;" ScrollBars="Auto" Width="100%" DefaultButton="btnSave">
                                <div class="modal-dialog2" style="width: 70%;">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_ServerClick">
                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                            <h1 class="modal-title" id="myModalLabel">
                                                <span></span>Add Item Split</h1>
                                        </div>
                                        <asp:UpdatePanel ID="pnlBillOfMaterial" runat="server">
                                            <ContentTemplate>
                                                <div class="modal-body">
                                                    <div>
                                                        <asp:UpdateProgress ID="UpdateProgress" runat="server">
                                                            <ProgressTemplate>
                                                                <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif" />
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                        <asp:LinkButton runat="server" ID="hbtn"></asp:LinkButton>
                                                        <ajaxToolkit:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="hbtn" PopupControlID="UpdateProgress" BackgroundCssClass="modalBackground">
                                                        </ajaxToolkit:ModalPopupExtender>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                                            <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <asp:HiddenField Value="0" runat="server" ID="hfMasterId" />
                                                        <asp:HiddenField Value="0" runat="server" ID="hfFinishMasterId" />
                                                        <asp:HiddenField Value="0" runat="server" ID="hfFinishDetailId" />
                                                        <asp:HiddenField Value="0" runat="server" ID="hfClosing" />
                                                        <asp:HiddenField Value="0" runat="server" ID="hfClosingDetail" />
                                                        <asp:HiddenField Value="1" runat="server" ID="hfValidateStock" />
                                                        <div class="col-md-4">
                                                            <label id="lblSKUFinished"><span class="fa fa-caret-right rgt_cart"></span>Split Item Name</label>
                                                            <dx:ASPxComboBox ID="ddlSplitItem" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlSplitItem_SelectedIndexChanged">
                                                            </dx:ASPxComboBox>
                                                        </div>                                                        
                                                        <div class="col-md-2">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Unit</label>
                                                            <asp:DropDownList runat="server" ID="drpSkuUnit" CssClass="form-control" Enabled="false">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Closing Stock</label>
                                                            <asp:TextBox runat="server" ID="txtClosing" CssClass="form-control" Enabled="false">
                                                            </asp:TextBox>
                                                        </div>
                                                        <div class="col-md-2" id="divConsumedQty" runat="server" visible="false">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Consumed Qty</label>
                                                            <asp:TextBox runat="server" ID="txtConsumedQty" onkeypress="return onlyDotsAndNumbers(this,event);" CssClass="form-control">
                                                            </asp:TextBox>
                                                        </div>                                                        
                                                    </div>
                                                    <asp:Panel runat="server" ID="pnlDetail" DefaultButton="btnAdd">
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Item Name</label>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Qty</label>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Price</label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                 <dx:ASPxComboBox ID="ddlSKU" runat="server" CssClass="form-control">
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:TextBox ID="txtQuantity" runat="server"
                                                                    onkeypress="return onlyDotsAndNumbers(this,event);" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:TextBox ID="txtPrice" runat="server"
                                                                    onkeypress="return onlyDotsAndNumbers(this,event);" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-success"
                                                                    OnClick="btnAdd_Click" OnClientClick="return ValidateForm();" />
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:HiddenField ID="RowId" Value="0" runat="server" />
                                                    <asp:Panel ID="pnlSKU" runat="server" Width="95%" Height="180px"
                                                        ScrollBars="Vertical" BorderColor="Silver" BorderStyle="Groove"
                                                        BorderWidth="1px">
                                                        <asp:GridView Width="95%" ID="gvSKU" runat="server" Class="table table-striped table-bordered table-hover table-condensed cf"
                                                            AutoGenerateColumns="False" OnRowEditing="gvSKU_RowEditing">
                                                            <Columns>
                                                                <asp:BoundField DataField="SKU_ID" ReadOnly="true">
                                                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Item Name" DataField="SKU_NAME" ReadOnly="true">
                                                                    <ItemStyle Width="37%"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="UOM" DataField="UOM_DESC" ReadOnly="true">
                                                                    <ItemStyle Width="20%"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Selling" DataField="Selling" ReadOnly="true">
                                                                    <ItemStyle Width="14%"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Price" DataField="Price" ReadOnly="true">
                                                                    <ItemStyle Width="14%"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Qty" DataField="QUANTITY" ReadOnly="true">
                                                                    <ItemStyle Width="8%"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UOM_ID" ReadOnly="true">
                                                                    <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                    <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                                </asp:BoundField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="fa fa-pencil" CommandName="Edit" ToolTip="Edit">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="2%"/>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                    <div>
                                                        <hr />
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-offset-4 col-md-3 ">
                                                            <div class="btnlist pull-right">
                                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success"
                                                                    OnClick="btnSave_Click" OnClientClick="return ValidateActualQty();" />
                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                                                    OnClick="btnCancel_Click" />
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
            <div class="col-md-12">
                <div class="emp-table">
                    <asp:UpdatePanel ID="up" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GrdRecipe" AllowPaging="true" runat="server"
                                CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                OnRowEditing="GrdRecipe_RowEditing" OnPageIndexChanging="GrdRecipe_PageIndexChanging" PageSize="10"
                                AutoGenerateColumns="False" EmptyDataText="No Record exist" OnRowDeleting="GrdRecipe_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="lngSplitItemCode" HeaderText="ID" ReadOnly="true">
                                        <ItemStyle Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU_NAME" HeaderText="Item Name" ReadOnly="true">
                                        <ItemStyle Width="35%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DocumentDate" HeaderText="Split Date" ReadOnly="true" DataFormatString="{0:dd-MMM-yyyy}">
                                        <ItemStyle Width="35%" />
                                    </asp:BoundField>                                    
                                    <asp:BoundField DataField="DISTRIBUTOR_ID" ReadOnly="true">
                                        <ItemStyle CssClass="HidePanel" />
                                        <HeaderStyle CssClass="HidePanel" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FinishedSKUID" ReadOnly="true">
                                        <ItemStyle CssClass="HidePanel" />
                                        <HeaderStyle CssClass="HidePanel" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SplitQty" ReadOnly="true">
                                        <ItemStyle CssClass="HidePanel" />
                                        <HeaderStyle CssClass="HidePanel" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit"
                                                class="fa fa-pencil" ToolTip="Edit">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                class="glyphicon glyphicon-trash" ToolTip="Delete"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="cf head"></HeaderStyle>
                                <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                                <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />

                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>