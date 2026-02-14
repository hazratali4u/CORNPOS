<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMain.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Default" %>

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
                        <strong>Select Database</strong>
                        <br />
                        <asp:DropDownList ID="ddlDB" runat="server"></asp:DropDownList>
                        <br />
                        <br />
                        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Create/Update Indexes" class="btn btn-primary" />
                    </div>
                </div>
                <!-- end span12 -->
            </div>
        </div>
    </section>
</asp:Content>
