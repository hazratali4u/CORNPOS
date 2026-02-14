<%@ Page Language="C#" Title="CORN :: Price Level" AutoEventWireup="true" CodeFile="frmPriceLevel.aspx.cs"
    Inherits="Forms_frmPriceLevel" MasterPageFile="~/Forms/PageMaster.master" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
    <link href="../css/Popup.css" rel="stylesheet" />
    <script type="text/javascript" src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
    <script language="JavaScript" type="text/javascript">

        function pageLoad() {
            var popUp = $find('ModelPopup');
            if (popUp) {
                popUp.add_hidden(HidePopupPanel);
            }
        }
        function HidePopupPanel(source, args) {
            objPanel = document.getElementById(source._PopupControlID);
            if (objPanel) {
                objPanel.style.display = 'none';
            }
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();
            }
        }

        function EndRequestHandler(sender, args) {
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
                            <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd" Text="Add"
                                OnClick="btnAdd_Click">
                                           <span class="fa fa-plus-circle"></span>Add
                            </asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="mPopUpSection" runat="server" PopupControlID="pnlParameters"
                                TargetControlID="btnAdd" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                                CancelControlID="btnClose">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="pnlParameters" runat="server" Width="100%" Style="display: none;" ScrollBars="Auto" DefaultButton="btnSave">
                                <div class="modal-dialog" style="width: 70%;">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_ServerClick">
                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                            <h1 class="modal-title" id="myModalLabel">
                                                <span></span>Add Price Level</h1>
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
                                                        <div class="col-md-4">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                                            <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Price Level Name</label>
                                                            <dx:ASPxTextBox ID="txtPriceLevel" runat="server"  CssClass="form-control"></dx:ASPxTextBox>
                                                        </div> 
                                                        <div class="col-md-4">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Price (%) Wise</label>
                                                            <br />
                                                            <asp:CheckBox ID="chkInPercent" Checked="true" runat="server" AutoPostBack="true"
                                                                 OnCheckedChanged="chkInPercent_SelectedIndexChnaged"
                                                                 CssClass="checkbox"></asp:CheckBox>
                                                        </div>                                                                                                               
                                                    </div>
                                                    <div class="row">
                                                        <asp:HiddenField Value="0" runat="server" ID="hfMasterId" />
                                                        <asp:HiddenField Value="0" runat="server" ID="hfFinishMasterId" />
                                                        <asp:HiddenField Value="0" runat="server" ID="hfFinishDetailId" />
                                                        <asp:HiddenField Value="0" runat="server" ID="hfClosing" />
                                                        <asp:HiddenField Value="0" runat="server" ID="hfClosingDetail" />                                                                                                             
                                                    </div>                                                    
                                                    <asp:HiddenField ID="RowId" Value="0" runat="server" />
                                                    <asp:Panel ID="pnlSKU" runat="server" Width="100%" Height="180px"
                                                        ScrollBars="Vertical" BorderColor="Silver" BorderStyle="Groove"
                                                        BorderWidth="1px">
                                                        
                                                        <asp:GridView ID="GrdPurchase" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                            AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID" ReadOnly="true">
                                                                    <HeaderStyle CssClass="HidePanel" />
                                                                    <ItemStyle CssClass="HidePanel" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SKU_NAME"  ReadOnly="true"  HeaderText="Item Description" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemStyle HorizontalAlign="Left"
                                                                        Width="25%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="UOM_DESC"  ReadOnly="true" HeaderText="UOM" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemStyle HorizontalAlign="Left"
                                                                        Width="10%" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Price (%)" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center"
                                                                        Width="15%"  />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPrice" runat="server" Width="100%" Text='<%# Eval("Price")%>' onkeypress="return onlyDotsAndNumbers(this,event);">
                                                                        </asp:TextBox>
                                                                    </ItemTemplate>
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
                                                                    OnClick="btnSave_Click"/>
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
                            <asp:GridView ID="grdPrice" AllowPaging="true" runat="server"
                                CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                OnRowEditing="grdPrice_RowEditing" OnPageIndexChanging="grdPrice_PageIndexChanging" PageSize="10"
                                AutoGenerateColumns="False" EmptyDataText="No Record exist" OnRowDeleting="grdPrice_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="SKU_PRICES_LEVEL_ID" HeaderText="ID" ReadOnly="true">
                                        <ItemStyle CssClass="HidePanel" />
                                        <HeaderStyle CssClass="HidePanel" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PriceLevelName" HeaderText="Price Level Name" ReadOnly="true">
                                        <ItemStyle Width="35%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location" ReadOnly="true">
                                        <ItemStyle Width="35%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DISTRIBUTOR_ID" ReadOnly="true">
                                        <ItemStyle CssClass="HidePanel" />
                                        <HeaderStyle CssClass="HidePanel" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="IsPercentWise" ReadOnly="true">
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