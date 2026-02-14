<%@ Page Title="CORN :: Prmotion Wizard Step 4" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmPromotionWizardSetp4.aspx.cs" Inherits="Forms_frmPromotionWizardSetp2" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">   
        function ConfirmHome() {
            if (confirm("Are you sure? You want to Return Home?") == true)
                return true;

            else
            { return false; }
        }
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <h1 class="modal-title" id="myModalLabel">
                <span></span>Promotion Wizard Step 4
            </h1>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-6">
                            <dx:ASPxMemo ID="txtSummary"  CssClass="form-control" runat="server"  Height="150">
                            </dx:ASPxMemo>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Button ID="btnHome" runat="server" Text="Home" CssClass=" btn btn-success" OnClick="btnHome_Click" />
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass=" btn btn-success" OnClick="btnBack_Click" />
                            <asp:Button ID="btnFinish" runat="server" Text="Finish" CssClass=" btn btn-success" OnClick="btnFinish_Click" />
                        </div>
                    </div>   
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Panel ID="PanRPSDetail" runat="server" Height="266px" Style="overflow-y: scroll" Width="94%">
                                <br />
                            <asp:GridView ID="grdPromotion" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" Width="100%"
                                CssClass="table table-striped table-bordered table-hover table-condensed cf">
                                <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                    PreviousPageText="Previous" />
                                <RowStyle ForeColor="Black" />
                                <Columns>
                                    <asp:BoundField DataField="BASKET NO" HeaderText="Basket No">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Is_Multiple" HeaderText="Is Mutiple">
                                        <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Multiple_of" HeaderText="Mutiple Of">
                                        <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU" HeaderText="Item">
                                        <HeaderStyle HorizontalAlign="Center" Width="200px" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UOM" HeaderText="UOM">
                                        <HeaderStyle CssClass="hidden" />
                                        <ItemStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Basket On" HeaderText="Basket On">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="From" HeaderText="From">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="To" HeaderText="To">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Discount" HeaderText="Discount">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU Offer" HeaderText="Item Offer">
                                        <HeaderStyle CssClass="hidden" />
                                        <ItemStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU Quantity" HeaderText="Item Quantity">
                                        <HeaderStyle CssClass="hidden" />
                                        <ItemStyle CssClass="hidden" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                    </asp:Panel>
                        </div>
                    </div>                 
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>