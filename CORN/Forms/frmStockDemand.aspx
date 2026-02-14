<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmStockDemand.aspx.cs" Inherits="Forms_frmStockDemand" Title="CORN :: Stock Demand" %>

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
        <div class="container employee-infomation">
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
                        <div class="col-md-3" style="margin-top: 25px;">
                            <asp:CheckBox runat="server" ID="chkFranchiseDemand" Checked="false" /> Franchise Demand
                            </div>
                    </div>

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
                       <%-- <div class="col-md-1">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Price</label>
                        </div>--%>
                    </div>
                    <div class="row">
                        <div class="col-md-3">                            
                           <dx:ASPxComboBox ID="ddlSkus" runat="server"  CssClass="form-control" 
                               AutoPostBack="true" OnSelectedIndexChanged="ddlSkus_SelectedIndexChanged">
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
                            <asp:HiddenField ID="hfInventoryType" runat="server" Value="0" />
                            <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Add" 
                                ValidationGroup="vg" CssClass="btn btn-success" />
                        </div>
                        <div class="col-md-6">
                                <label>
                                    <asp:Label ID="lblStock" runat="server" Text="Stock:0"></asp:Label>
                                </label>
                            </div>
                    </div>

                    <div class="row center">
                        <div class="col-md-12">
                            <div class="emp-table">
                                <asp:HiddenField ID="_rowNo" runat="server" Value="0" />
                                <asp:Panel ID="Panel2" runat="server" Width="60%" Height="180px" ScrollBars="Vertical"
                                    BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                    <asp:GridView ID="GrdPurchase" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                        ShowHeader="False" AutoGenerateColumns="False"
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
                                            <asp:BoundField DataField="SKU_NAME"  ReadOnly="true">
                                                <ItemStyle HorizontalAlign="Left"
                                                    Width="50%" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UOM_DESC" ReadOnly="true">
                                                    <ItemStyle Width="25%" />
                                                </asp:BoundField>
                                            <asp:BoundField DataField="QUANTITY"  ReadOnly="true">
                                                <ItemStyle HorizontalAlign="Right"
                                                    Width="25%" />
                                            </asp:BoundField>
                                           <%-- <asp:BoundField DataField="PRICE"  ReadOnly="true">
                                                <ItemStyle HorizontalAlign="Right"
                                                    Width="20%" />
                                            </asp:BoundField>--%>
                                            <asp:BoundField DataField="PRICE"  ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                            <asp:BoundField DataField="UOM_ID"  ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                             <asp:BoundField DataField="STOCK_DEMAND_DETAIL_ID"  ReadOnly="true">
                                                    <HeaderStyle CssClass="HidePanel" />
                                                    <ItemStyle CssClass="HidePanel" />
                                                </asp:BoundField>
                                             <asp:BoundField DataField="DISTRIBUTOR_ID"  ReadOnly="true">
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
