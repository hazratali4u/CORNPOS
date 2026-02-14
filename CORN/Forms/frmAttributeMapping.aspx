<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmAttributeMapping.aspx.cs" Inherits="frmAttributeMapping" Title="CORN :: Attribute Mapping" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">   
    
    <script language="JavaScript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }

        function ValidateForm() {
            debugger;
            return true;
        }
       </script>

    <div class="main-contents">
        <div class="container employee-infomation">

            <div class="row top">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">
                                    <div class="row center">
                                        <div class="col-md-3">
                                            <label>
                                                <span class="fa fa-caret-right rgt_cart"></span>Category</label>

                                            <dx:ASPxComboBox ID="ddlCategory" runat="server" CssClass="form-control"
                                                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                                AutoPostBack="True" TabIndex="2">
                                            </dx:ASPxComboBox>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-8">
                                            <asp:GridView ID="Gridview1" runat="server" ShowFooter="false" AutoGenerateColumns="false">

                                                <Columns>

                                                    <asp:BoundField DataField="SKU_ID" ReadOnly="true">
                                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                        <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="SKU_Name" HeaderText="Item" ReadOnly="true">
                                                        <ItemStyle Width="400px"></ItemStyle>
                                                        <HeaderStyle Height="35px"></HeaderStyle>
                                                    </asp:BoundField>

                                                    <asp:TemplateField HeaderText="Size">
                                                        <ItemStyle Width="350px" />
                                                        <HeaderStyle Height="35px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSize" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Crust">
                                                        <ItemStyle Width="350px" />
                                                        <HeaderStyle Height="35px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtCrust" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>

                                            </asp:GridView>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px;">
                                        <div class="col-md-7">
                                        </div>
                                        <div class="col-md-12" align="center">
                                            <asp:Button ID="btnSaveCategory" OnClick="btnSaveCategory_Click" runat="server" Text="Save" CssClass="btn btn-success" />
                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" />
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
            </div>

        </div>
    </div>
</asp:Content>