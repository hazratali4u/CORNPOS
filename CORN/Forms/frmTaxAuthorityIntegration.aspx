<%@ Page Language="C#" Title="CORN :: Tax Authority Integration" AutoEventWireup="true" CodeFile="frmTaxAuthorityIntegration.aspx.cs"
    Inherits="Forms_frmTaxAuthorityIntegration" MasterPageFile="~/Forms/PageMaster.master" %>

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
        <div class="container">
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

            <div class="col-md-offset-4 col-md-3" style="float:right">
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
                            <asp:Panel ID="pnlParameters" runat="server" Style="display: none;">
                                        <asp:UpdatePanel ID="pnlBillOfMaterial" runat="server">
                                            <ContentTemplate>
                                                <div class="modal-dialog">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_ServerClick">
                                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                                            <h1 class="modal-title" id="myModalLabel">
                                                                <span></span>Add Tax Authority</h1>
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
                                                                <div class="col-md-8">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Tax Authority</label>
                                                                    <dx:ASPxComboBox ID="ddlTaxAuthority" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="ddlTaxAuthority_SelectedIndexChanged">
                                                                        <Items>
                                                                            <dx:ListEditItem Value="1" Text="FBR - Federal Board of Revenue" Selected="true" />
                                                                            <dx:ListEditItem Value="2" Text="PRA - Punjab Revenue Authority" />
                                                                            <%--<dx:ListEditItem Value="3" Text="KPRA - Khyber Pakhtunkhwa Revenue Authority" />--%>
                                                                            <dx:ListEditItem Value="4" Text="SRB - Sindh Revenue Board" />
                                                                        </Items>
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-8">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                                                    <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-8">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>POS ID</label>
                                                                    <dx:ASPxTextBox ID="txtPOSID" runat="server" CssClass="form-control"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-8">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Token</label>
                                                                    <dx:ASPxTextBox ID="txtToken" runat="server" CssClass="form-control"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-8">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>URL</label>
                                                                    <dx:ASPxTextBox ID="txtURL" runat="server" CssClass="form-control" Enabled="false"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-8">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Invoice Label</label>
                                                                    <dx:ASPxTextBox ID="txtInvocieLabel" runat="server" CssClass="form-control" Text="FBR Invoice No"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <asp:HiddenField Value="0" runat="server" ID="hfMasterId" />
                                                            </div>
                                                            <asp:HiddenField ID="RowId" Value="0" runat="server" />
                                                        </div>
                                                        <div class="modal-footer">
                                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-success"
                                                                OnClick="btnSave_Click" />
                                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                                                OnClick="btnCancel_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
                                    <asp:BoundField DataField="FBRIntegrationID" HeaderText="ID" ReadOnly="true">
                                        <ItemStyle CssClass="HidePanel" />
                                        <HeaderStyle CssClass="HidePanel" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location" ReadOnly="true">
                                        <ItemStyle Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="POSID" HeaderText="POS ID" ReadOnly="true">
                                        <ItemStyle Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Token" HeaderText="Token" ReadOnly="true">
                                        <ItemStyle Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FBRURL" HeaderText="URL" ReadOnly="true">
                                        <ItemStyle Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DistributorID" ReadOnly="true">
                                        <ItemStyle CssClass="HidePanel" />
                                        <HeaderStyle CssClass="HidePanel" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TaxAuthorityLabel" ReadOnly="true">
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
        </div>
</asp:Content>
