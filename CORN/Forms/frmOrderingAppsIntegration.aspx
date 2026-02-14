<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmOrderingAppsIntegration.aspx.cs" Inherits="Forms_frmOrderingAppsIntegration" Title="CORN :: Food Ordering Apps Integration" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function confirmation() {
            return confirm("Are you sure you want Import Menu to Blink Server?");
        }
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Food Ordering App Name</label>
                            <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                                <Items>
                                    <dx:ListEditItem Value="1" Text="Blink" Selected="true" />
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <asp:Button ID="btnImportMenu" runat="server" OnClick="btnImportMenu_Click" Text="Import Menu" CssClass="btn btn-success"   OnClientClick="return confirmation();"/>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:HiddenField ID="hfNumber" runat="server"></asp:HiddenField>
        </div>
    </div>
</asp:Content>
