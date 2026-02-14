<%@ Page Title="CORN :: Item Group" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmItemGroup.aspx.cs" Inherits="Forms_frmItemGroup" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <div class="main-contents">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Group Name</label>
                            <dx:ASPxTextBox ID="txtGroupName" CssClass="form-control" runat="server">
                            </dx:ASPxTextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Category</label>
                            <dx:ASPxComboBox ID="ddCatagory" runat="server" CssClass="form-control" AutoPostBack="true"
                                OnSelectedIndexChanged="ddCatagory_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-8">
                            <fieldset>
                                <legend>Item Collection</legend>
                                <div class="row">
                                    <div class="col-md-5">
                                        <asp:ListBox ID="lstUnAssignSKU" runat="server" Width="270px" Height="200px"></asp:ListBox>
                                    </div>
                                    <div class="col-md-2" style="text-align: center;">
                                        <div class="row">
                                            <asp:Button ID="btnAddAll" runat="server" Text=">>" CssClass=" btn btn-success" OnClick="btnAddAll_Click" />
                                        </div>
                                        <br />
                                        <div class="row">
                                            <asp:Button ID="btnAdd" runat="server" Text=">" CssClass=" btn btn-success" OnClick="btnAdd_Click" />
                                        </div>
                                        <br />
                                        <div class="row">
                                            <asp:Button ID="btnRemove" runat="server" Text="<" CssClass=" btn btn-success" OnClick="btnRemove_Click" />
                                        </div>
                                        <br />
                                        <div class="row">
                                            <asp:Button ID="btnRemoveAll" runat="server" Text="<<" CssClass=" btn btn-success" OnClick="btnRemoveAll_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:ListBox ID="lstAssignSKU" runat="server" Width="270px" Height="200px"></asp:ListBox>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-8" style="text-align: center;">
                            <br />
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass=" btn btn-success" OnClick="btnSave_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-8">
                            <br />
                            <div class="emp-table">
                                <asp:UpdatePanel ID="up" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="SKUGroup_Grid" AllowPaging="true" runat="server"
                                            CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                            OnRowEditing="SKUGroup_Grid_RowEditing" PageSize="10" OnPageIndexChanging="SKUGroup_Grid_PageIndexChanging"
                                            AutoGenerateColumns="False" EmptyDataText="No Record exist">
                                            <Columns>
                                                <asp:BoundField DataField="SKU_GROUP_ID" HeaderText="Group ID">
                                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="GROUP_NAME" HeaderText="Group Name">
                                                    <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="SKU Name">
                                                    <ItemTemplate>
                                                        <asp:ListBox ID="listbox1" runat="server" Width="400px" CssClass="DropList" AutoPostBack="True"></asp:ListBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle HorizontalAlign="Center" Width="5%"/>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit"
                                                            class="fa fa-pencil" ToolTip="Edit">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>