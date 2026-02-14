<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMain.master" AutoEventWireup="true" CodeFile="Truncate.aspx.cs" Inherits="Truncate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cDefault" ContentPlaceHolderID="cphChild" runat="Server">
     <script type="text/javascript">
        function confirmation() {
            return confirm("Have you taken Database Backup?");
        }
    </script>
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
                    <div class="row" style="margin-bottom: 0px;">
                        <div class="span6" runat="server">
                            <asp:RadioButtonList runat="server" ID="rblTruncateType" RepeatDirection="Horizontal" Width="400"
                                AutoPostBack="true" OnSelectedIndexChanged="rblTruncateType_SelectedIndexChanged">
                                <asp:ListItem Text="Single Location" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Whole Database" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 0px;">
                        <div class="span6" runat="server" id="rowSingleLocation">
                            <strong>Select Database</strong>
                            <br />
                            <asp:DropDownList ID="ddlDB" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlDB_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />
                            <strong>Select Location</strong>
                            <br />
                            <asp:DropDownList ID="ddlLocation" runat="server"></asp:DropDownList>
                        </div>
                        <div class="span6" runat="server" id="rowWholeDB" visible="false">
                            <strong>Enter Database Name</strong>
                            <br />
                            <asp:TextBox runat="server" ID="txtDBName"></asp:TextBox>
                        </div>
                        <div class="span3">
                            <strong>Select Day Close</strong>
                            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="txtDate"
                                TargetControlID="txtDate"></cc1:CalendarExtender>
                        </div>
                        <div class="span3">
                            <br />
                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" OnClientClick="return confirmation();" Text="Truncate Single Location" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
                <!-- end span12 -->
            </div>
        </div>
    </section>
</asp:Content>