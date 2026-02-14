<%@ Page Title="CORN :: Prmotion Wizard Step 3" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmPromotionWizardSetp3.aspx.cs" Inherits="Forms_frmPromotionWizardSetp3" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        
        function ValidateForm() {
            var str;
            var rblSelectedValue = $('#<%=rblSKUType.ClientID %> input:checked').val();
            if (rblSelectedValue === "2")
            {
                str = SKUSelectedGroupdd.GetValue();
			    if (str == null || str.length == 0) {
			        alert('Must select Item Group');
			        return false;
			    }
            }

            if (document.getElementById('<%=chIsMultiple.ClientID%>').checked == true) {
                str = MultipleOftxt.GetValue();
			    if (str == null || str.length == 0) {
			        alert('Must enter Multiple of');
			        return false;
			    }
			}
            str = Fromtxt.GetValue();
            if (str == null || str.length == 0) {
                alert('Must enter From Range');
                return false;
            }
            str = Totxt.GetValue();
			if (str == null || str.length == 0) {
			    alert('Must enter To Range');
			    return false;
			}
			if (!(document.getElementById('<%=chDiscount.ClientID%>').checked || document.getElementById('<%=chSKU.ClientID%>').checked)) {
			    alert('Must Supply values either of Discount or Item or both');
			    return false;
			}
            if (document.getElementById('<%=chDiscount.ClientID%>').checked) {
                debugger;
                var rblSelectedValue2 = $('#<%=rblDiscountType.ClientID %> input:checked').val();
                if (rblSelectedValue2 == 1) {
			        str = Discounttxt.GetValue();
			        if (str == null || str.length == 0) {
			            alert('Must enter Discount Rate');
			            return false;
			        }
			        var num = parseFloat(str);
			        if (num >= 100.0) {
			            alert('Discount Rate must not be greater than 100');
			            return false;
			        }
			    }
                else {
			        str = Discounttxt.GetValue();
				    if (str == null || str.length == 0) {
				        alert('Must enter Discount Amount');
				        return false;
				    }
				}
            }
            return true;
        }
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <h1 class="modal-title" id="myModalLabel">
                <span></span>Promotion Wizard Step 3
            </h1>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:RadioButtonList ID="rblSKUType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblSKUType_SelectedIndexChanged"
                                        RepeatDirection="Horizontal" Width="250px">
                                        <asp:ListItem Value="1" Text="By Item Hierarchy" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="By Item Group"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <br />
                                    <asp:Panel ID="pnlHierarchy" runat="server">
                                        <label><span class="fa fa-caret-right rgt_cart"></span>Item Category</label>
                                        <dx:ASPxComboBox ID="ddSKUCatagory" runat="server" CssClass="form-control" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddSKUCatagory_SelectedIndexChanged">
                                        </dx:ASPxComboBox>
                                        <label><span class="fa fa-caret-right rgt_cart"></span>Item</label>
                                        <dx:ASPxComboBox ID="ddSKU" runat="server" CssClass="form-control">
                                        </dx:ASPxComboBox>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlGroup" runat="server" Visible="false">
                                        <label><span class="fa fa-caret-right rgt_cart"></span>Item Group</label>
                                        <dx:ASPxComboBox ID="ddSKUSelectedGroup" runat="server" CssClass="form-control"
                                            ClientInstanceName="SKUSelectedGroupdd">
                                        </dx:ASPxComboBox>
                                    </asp:Panel>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Slab On</label>
                                    <dx:ASPxComboBox ID="ddSlabOn" runat="server" CssClass="form-control"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddSlabOn_SelectedIndexChanged">
                                        <Items>
                                            <dx:ListEditItem Value="82" Text="Quantity" Selected="true" />
                                            <dx:ListEditItem Value="83" Text="Amount" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row" style="display:none;">
                                <div class="col-md-12">
                                    <asp:CheckBox ID="chIsMultiple" runat="server" Text="Is Multiple" AutoPostBack="True" OnCheckedChanged="chIsMultiple_CheckedChanged"></asp:CheckBox>
                                    <dx:ASPxSpinEdit ID="txtMultipleOf" runat="server" CssClass="form-control" Enabled="false" SpinButtons-ShowIncrementButtons="false" MinValue="0"
                                        ClientInstanceName="MultipleOftxt">
                                    </dx:ASPxSpinEdit>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label>
                                        <span class="fa fa-caret-right rgt_cart"></span>
                                        <asp:Label ID="lblfromquantity" runat="server" Text="From Quantity"></asp:Label>
                                    </label>
                                    <dx:ASPxSpinEdit ID="txtFrom" runat="server" CssClass="form-control" SpinButtons-ShowIncrementButtons="false" MinValue="0"
                                        ClientInstanceName="Fromtxt">
                                    </dx:ASPxSpinEdit>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label>
                                        <span class="fa fa-caret-right rgt_cart"></span>
                                        <asp:Label ID="lblToQuantity" runat="server" Text="To Quantity"></asp:Label></label>
                                    <dx:ASPxSpinEdit ID="txtTo" runat="server" CssClass="form-control" SpinButtons-ShowIncrementButtons="false" MinValue="0"
                                        ClientInstanceName="Totxt">
                                    </dx:ASPxSpinEdit>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-1">
                        </div>
                        <div class="col-md-5">
                            <h3>Promotion Offer</h3>
                            <fieldset>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:CheckBox ID="chDiscount" runat="server" Text="Discount"></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-10">
                                        <asp:RadioButtonList ID="rblDiscountType" runat="server" RepeatDirection="Horizontal" Width="250px">
                                            <asp:ListItem Value="1" Text="Discount Rate" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Discount Amount"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-10">
                                        <dx:ASPxSpinEdit ID="txtDiscount" runat="server" CssClass="form-control" SpinButtons-ShowIncrementButtons="false" MinValue="0"
                                            ClientInstanceName="Discounttxt">
                                        </dx:ASPxSpinEdit>
                                    </div>
                                </div>
                                <%--Free Item Hidden Started--%>
                                <div class="row" style="display:none;">
                                    <div class="col-md-12">
                                        <asp:CheckBox ID="chSKU" runat="server" Text="Item"></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="row" style="display:none;">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-10">
                                        <label>
                                            <span class="fa fa-caret-right rgt_cart"></span>
                                            <asp:Label ID="lblSKUCategory2" runat="server" Text="Item Category"></asp:Label></label>
                                        <dx:ASPxComboBox ID="ddPromotionCatagory" runat="server" CssClass="form-control"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddPromotionCatagory_SelectedIndexChanged">
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="row" style="display:none;">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-10">
                                        <label>
                                            <span class="fa fa-caret-right rgt_cart"></span>
                                            <asp:Label ID="lblSKU2" runat="server" Text="Item"></asp:Label></label>
                                        <dx:ASPxComboBox ID="ddPromotionSKU" runat="server" CssClass="form-control">
                                        </dx:ASPxComboBox>
                                    </div>
                                </div>
                                <div class="row" style="display:none;">
                                    <div class="col-md-2"></div>
                                    <div class="col-md-10">
                                        <label>
                                            <span class="fa fa-caret-right rgt_cart"></span>
                                            <asp:Label ID="lblQuantity2" runat="server" Text="Quantity"></asp:Label></label>
                                        <dx:ASPxSpinEdit ID="txtPromotionQuantity" runat="server" CssClass="form-control" SpinButtons-ShowIncrementButtons="false" MinValue="0">
                                        </dx:ASPxSpinEdit>
                                    </div>
                                </div>
                                <%--Free Item Hidden Ended--%>
                            </fieldset>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <asp:Button ID="btnAddtoSlab" runat="server" Text="Add To Slab" CssClass=" btn btn-success" OnClick="btnAddtoSlab_Click" />
                            <asp:Button ID="btnCreateNewSlab" runat="server" Text="Create New Slab" CssClass=" btn btn-success" OnClick="btnCreateNewSlab_Click" />
                        </div>
                        <div class="col-md-1"></div>
                        <div class="col-md-4">
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass=" btn btn-success" OnClick="btnBack_Click" />
                            <asp:Button ID="btnNext" runat="server" Text="Next" CssClass=" btn btn-success" OnClick="btnNext_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-11">
                            <br />
                            <asp:GridView ID="grdSlab" runat="server" Width="94%" OnRowDeleting="grdSlab_RowDeleting" AutoGenerateColumns="False" HorizontalAlign="Center" OnRowEditing="grdSlab_RowEditing"
                                CssClass="table table-striped table-bordered table-hover table-condensed cf">
                                <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next" PreviousPageText="Previous"></PagerSettings>
                                <Columns>
                                    <asp:BoundField DataField="SLAB_NO" HeaderText="SLAB_NO">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SLAB NO" HeaderText="Slab No">
                                        <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Is_Multiple" HeaderText="Is Multiple"></asp:BoundField>
                                    <asp:BoundField DataField="Multiple_of" HeaderText="Multiple Of">
                                        <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU" HeaderText="Item">
                                        <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UOM" HeaderText="UOM" Visible="False">
                                        <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Slab On" HeaderText="Slab On">
                                        <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="From" HeaderText="From">
                                        <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="To" HeaderText="To">
                                        <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Discount" HeaderText="Discount">
                                        <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU Offer" HeaderText="Item Offer">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SKU Quantity" HeaderText="Item Quantity">
                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                        <ItemStyle CssClass="HidePanel"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" ToolTip="Edit">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                class="fa fa-trash-o"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>