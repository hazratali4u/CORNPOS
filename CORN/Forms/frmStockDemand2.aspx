<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmStockDemand2.aspx.cs" Inherits="Forms_frmStockDemand2" Title="CORN :: Stock Demand" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
    <script language="JavaScript" type="text/javascript">
        function ConfirmDelete() {
            if (confirm("Do you want to Cancel this record?") == true)
                return true;
            else {
                return false;
            }
        }
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
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
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Document No</label>
                            <dx:ASPxComboBox ID="drpDocumentNo" runat="server" CssClass="form-control"
                                AutoPostBack="true" ClientInstanceName="DocNo"
                                OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                            <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-3">
                            <asp:CheckBox ID="cbConsumption" runat="server" Text="Show Consumption" AutoPostBack="true" OnCheckedChanged="cbConsumption_CheckedChanged" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Remarks</label>
                            <asp:TextBox ID="txtDocumentNo" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-2" runat="server" id="dvFromDate" visible="false">
                            <label><span class="fa fa-caret-right rgt_cart"></span>From Date</label>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>                            
                        </div>
                        <div class="col-md-1" style="margin-top: 27px" runat="server" id="dvFromImage" visible="false">
                            <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                        <div class="col-md-2" runat="server" id="dvToDate" visible="false">
                            <label><span class="fa fa-caret-right rgt_cart"></span>To Date</label>
                            <asp:TextBox ID="txtEndDate" runat="server" onkeyup="BlockEndDateKeyPress()" CssClass="form-control" MaxLength="10"></asp:TextBox>                            
                        </div>
                        <div class="col-md-1" style="margin-top: 27px" runat="server" id="dvToImage" visible="false">
                            <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                Width="30px" />
                        </div>
                        <ajaxToolkit:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                        PopupPosition="TopLeft" TargetControlID="txtStartDate" OnClientShown="calendarShown"></ajaxToolkit:CalendarExtender>
                    <ajaxToolkit:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                        TargetControlID="txtEndDate" PopupPosition="TopLeft" OnClientShown="calendarShown"></ajaxToolkit:CalendarExtender>                        
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Section</label>
                        </div>
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Category</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <dx:ASPxComboBox ID="ddlSection" runat="server" CssClass="form-control"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlSection_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-3">
                            <dx:ASPxComboBox ID="ddlCategory" runat="server" CssClass="form-control"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-1">
                            <asp:LinkButton CssClass="btn btn-primary" runat="server" ID="btnImport" Text="Import"
                                OnClick="btnImport_Click">
                                <span class="fa fa-plus-circle"></span>Import
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnLoadConsumption" AccessKey="S" OnClick="btnLoadConsumption_Click" runat="server" Text="Load Consumption" UseSubmitBehavior="False" CssClass="btn btn-success" Visible="false" />                            
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
                                        <span></span>Import Stock Demand</h1>
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
                    <div class="row center">
                        <div class="col-md-12">
                            <div class="emp-table">
                                <asp:HiddenField ID="_rowNo" runat="server" Value="0" />
                                <asp:Panel ID="Panel2" runat="server" Width="90%" Height="800px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                    <asp:GridView ID="GrdPurchase" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                        AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel" />
                                                <ItemStyle CssClass="HidePanel" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_HIE_NAME" ReadOnly="true" HeaderText="Category" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SKU_NAME" ReadOnly="true" HeaderText="Item Description" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left"
                                                    Width="40%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UOM_DESC" ReadOnly="true" HeaderText="UOM" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left"
                                                    Width="15%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CLOSING_STOCK" ReadOnly="true" HeaderText="Closing Stock" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:F2}">
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Consumption" ReadOnly="true" HeaderText="Consumption" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:F2}">
                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Center"
                                                    Width="15%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtQuantity" runat="server" Width="100%" Text='<%# Eval("Quantity")%>' onkeypress="return onlyDotsAndNumbers(this,event);">
                                                    </asp:TextBox>
                                                </ItemTemplate>
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