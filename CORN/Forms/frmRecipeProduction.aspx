<%@ Page Language="C#" Title="CORN :: Recipe Production" AutoEventWireup="true" CodeFile="frmRecipeProduction.aspx.cs"
    Inherits="Forms_frmRecipeProduction" MasterPageFile="~/Forms/PageMaster.master" %>

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

            if (parseFloat(document.getElementById('<%=txtQuantity.ClientID%>').value) < 1) {
                alert('Quantity must be greater than 0');
                return false;
            }

            return true;
        }

        function ValidateActualQty() {
            var str;
            str = document.getElementById('<%=txtActualQty.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Actual Quantity is required');
                return false;
            }

            if (parseFloat(document.getElementById('<%=txtActualQty.ClientID%>').value) <= 0) {
                alert('Actual Quantity must be greater than 0');
                return false;
            }
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
                            <asp:Panel ID="pnlParameters" runat="server" Style="display: none; width: 100%" ScrollBars="Auto">
                                <div class="modal-dialog2" style="width: 70%">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_ServerClick">
                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                            <h1 class="modal-title" id="myModalLabel">
                                                <span></span>Add New Recipe Production</h1>
                                        </div>
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
                                                <asp:HiddenField Value="0" runat="server" ID="hfMasterId" />
                                                <asp:HiddenField Value="0" runat="server" ID="hfFinishMasterId" />
                                                <asp:HiddenField Value="0" runat="server" ID="hfFinishDetailId" />
                                                <asp:HiddenField Value="0" runat="server" ID="hfClosing" />
                                                <asp:HiddenField Value="0" runat="server" ID="hfClosingDetail" />
                                                <div class="col-md-4">
                                                    <label id="lblSKUFinished"><span class="fa fa-caret-right rgt_cart"></span>Item Name</label>
                                                    <dx:ASPxComboBox ID="ddlSKUFinished" runat="server" CssClass="form-control" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlSKUFinished_SelectedIndexChanged">
                                                    </dx:ASPxComboBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Closing Stock</label>
                                                    <asp:TextBox runat="server" ID="txtClosing" CssClass="form-control" Enabled="false">
                                                    </asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>UOM</label>
                                                    <asp:DropDownList runat="server" ID="drpSkuUnit" CssClass="form-control" Enabled="false">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-1">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Qty</label>
                                                    <asp:TextBox ID="txtRecipeQty" runat="server" CssClass="form-control"></asp:TextBox>

                                                </div>
                                                <div class="col-md-2">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Production Qty</label>
                                                    <asp:TextBox ID="txtActualQty" runat="server" onkeypress="return onlyDotsAndNumbers(this,event);" CssClass="form-control" onkeyup="txtActualQty_Change();"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1" style="margin-left:-18px;">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>&nbsp;</label>
                                                    <asp:Button ID="btnApplye" runat="server" Text="Apply" CssClass="btn btn-success"
                                                        OnClick="btnApplye_Click" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                                    &nbsp;&nbsp;&nbsp;<span style="color:red" ID="txtDocumentDate" runat="server"></span>
                                                    <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control" AutoPostBack="true"
                                                        OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                                                    </dx:ASPxComboBox>
                                                </div>
                                                <div class="col-md-2" style="display:none">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Prod Date</label>
                                                    <asp:TextBox ID="txtProductionDate" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                </div>
                                                 <div class="col-md-8">
                                                    <label>
                                                        <span class="fa fa-caret-right rgt_cart"></span>Remarks</label>
                                                    <asp:TextBox ID="txtremarks" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                                </div>
                                            </div>
                                            <asp:Panel runat="server" ID="pnlDetail" DefaultButton="btnAdd" Visible="false">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <label><span class="fa fa-caret-right rgt_cart"></span>Raw Material Item</label>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <label><span class="fa fa-caret-right rgt_cart"></span>Qty</label>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:DropDownList runat="server" ID="ddlSKU" CssClass="form-control"
                                                            Enabled="false">
                                                        </asp:DropDownList>

                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="txtQuantity" runat="server"
                                                            onkeypress="return onlyDotsAndNumbers(this,event);" CssClass="form-control"></asp:TextBox>

                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-success"
                                                            OnClick="btnAdd_Click" OnClientClick="return ValidateForm();" />
                                                    </div>
                                                    <div class="col-md-5">
                                                        <asp:Label ID="lblError" ForeColor="Red" Text="" runat="server"></asp:Label>
                                                    </div>

                                                </div>
                                            </asp:Panel>
                                            <asp:HiddenField ID="RowId" Value="0" runat="server" />
                                            <asp:Panel ID="pnlSKU" runat="server" Width="100%" Height="180px"
                                                ScrollBars="Vertical" BorderColor="Silver" BorderStyle="Groove"
                                                BorderWidth="1px">
                                                <asp:GridView Width="100%" ID="gvSKU" runat="server" Class="table table-striped table-bordered table-hover table-condensed cf"
                                                    AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:BoundField DataField="SKU_ID" ReadOnly="true">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Recipe Item" DataField="SKU_NAME" ReadOnly="true">
                                                            <ItemStyle Width="32%"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="UOM" DataField="UOM_DESC" ReadOnly="true">
                                                            <ItemStyle Width="10%"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CLOSING_STOCK" HeaderText="Closing Stock" ReadOnly="true">
                                                            <ItemStyle Width="18%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Recipe Qty" DataField="OrignalQty" ReadOnly="true">
                                                            <ItemStyle Width="18%"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="UOM_ID" ReadOnly="true">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FINISHED_GOOD_DETAIL_ID" ReadOnly="true">
                                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Consumed Qty">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtActualQty" runat="server" Width="90%" Text='<%# Eval("ActulQty")%>' Style="text-align: right"></asp:TextBox>
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                    ValidChars=".1234567890" TargetControlID="txtActualQty"></ajaxToolkit:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="22%" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="OrignalQty" ReadOnly="true">
                                                            <ItemStyle CssClass="hidden"></ItemStyle>
                                                            <HeaderStyle CssClass="hidden" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success"
                                                OnClick="btnSave_Click" OnClientClick="return ValidateActualQty();" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                                OnClick="btnCancel_Click" />
                                        </div>
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
                                    <asp:BoundField DataField="lngRecipeProductionCode" HeaderText="ID" ReadOnly="true">
                                        <ItemStyle Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU_NAME" HeaderText="Item Name" ReadOnly="true">
                                        <ItemStyle Width="35%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Production_DATE" HeaderText="Prod. Date" ReadOnly="true" DataFormatString="{0:dd-MMM-yyyy}">
                                        <ItemStyle Width="35%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FINISHED_SKU_ID" ReadOnly="true">
                                        <ItemStyle CssClass="HidePanel" />
                                        <HeaderStyle CssClass="HidePanel" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DISTRIBUTOR_ID" ReadOnly="true">
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
