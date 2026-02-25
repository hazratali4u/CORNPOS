<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMain.master" AutoEventWireup="true" CodeFile="AddDB.aspx.cs" Inherits="AddDB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cDefault" ContentPlaceHolderID="cphChild" runat="Server">
    <style type="text/css">
        label {
            display: inline-block;
            margin-left: 5px;
            padding-left: 5px;
        }
    </style>
    <section id="content">
        <div class="container">
            <div class="row demobtn">
                <div class="span12">
                    <div class="row">
                        <div class="span6">
                            <strong>
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                            </strong>
                        </div>
                    </div>
                    <div class="span6">
                        <strong><asp:Label ID="lblDBName" runat="server" Text="Database Name"></asp:Label> </strong>
                        <br />
                        <asp:TextBox ID="txtDbName" runat="server" CssClass="form-control"></asp:TextBox>
                        <br />
                        <strong><asp:Label ID="lblCompanyName" runat="server" Text="Company Name"></asp:Label> </strong>
                        <br />
                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control"></asp:TextBox>
                        <br />
                        <strong><asp:Label ID="lblPIN" runat="server" Text="PIN"></asp:Label> </strong>
                        <br />
                        <asp:TextBox ID="txtPIN" runat="server" CssClass="form-control"></asp:TextBox>
                        <br />
                        <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" Text="Create Database" class="btn btn-primary" />
                    </div>
                </div>
                <!-- end span12 -->
            </div>
        </div>
    </section>
</asp:Content>