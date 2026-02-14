<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmEcommExportJson.aspx.cs" Inherits="frmEcommExportJson" Title="CORN :: Export Json" %>

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
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label><span class="fa fa-caret-right rgt_cart"></span>Access Key ID </label>

                                            <asp:TextBox ID="txtAccessKeyID" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <div class="col-md-4">
                                            <label><span class="fa fa-caret-right rgt_cart"></span>Secret Access Key </label>

                                            <asp:TextBox ID="txtSecretAccessKey" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>

                                        <div class="col-md-4">
                                            <label><span class="fa fa-caret-right rgt_cart"></span>Bucket Name </label>

                                            <asp:TextBox ID="txtBucketName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row center">
                                        <div class="col-md-12" align="center">
                                            <asp:Button ID="btnSaveCategory" OnClick="btnSaveCategory_Click" runat="server" Text="Export JSON" CssClass="btn btn-success" />
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
            </div>

        </div>
    </div>
</asp:Content>