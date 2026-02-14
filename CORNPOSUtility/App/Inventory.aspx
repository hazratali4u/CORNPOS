<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMain.master" AutoEventWireup="true" CodeFile="Inventory.aspx.cs" Inherits="Inventory" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cDefault" ContentPlaceHolderID="cphChild" runat="Server">
    <style>
        .HidePanel {
            display: none;
        }
    </style>
     <script type="text/javascript">
         function confirmation(btn) {
             return confirm("Are you sure to " + btn.value + "?");
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
                            <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" Width="100%"
                                AutoPostBack="true" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                <asp:ListItem Value="1" Text="Purchase" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Transfer Out" Enabled="false"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Purchase Return" Enabled="false"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Damage" Enabled="false"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Production In" Enabled="false"></asp:ListItem>
                            </asp:RadioButtonList>
                            <br />
                            <asp:RadioButtonList ID="rblActionType" runat="server" RepeatDirection="Horizontal" Width="30%"
                                AutoPostBack="true" OnSelectedIndexChanged="rblActionType_SelectedIndexChanged">
                                <asp:ListItem Value="1" Text="Delete" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Edit" Enabled="false"></asp:ListItem>
                            </asp:RadioButtonList>
                            <strong>Select Database</strong>
                            <br />
                            <asp:DropDownList ID="ddlDB" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlDB_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />
                            <strong>Select Location</strong>
                            <br />
                            <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"></asp:DropDownList>
                            <br />
                            <strong>Select Date</strong>
                            <br />
                            <asp:TextBox ID="txtDate" runat="server" EnableViewState="true"></asp:TextBox>
                            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="txtDate"
                                TargetControlID="txtDate"></cc1:CalendarExtender>
                            <br />
                            <asp:Button ID="btnLoad" runat="server" OnClick="btnLoad_Click" Text="Load Data" class="btn btn-primary" />
                            <br />
                            <strong>Document No</strong>
                            <br />
                            <asp:DropDownList ID="ddlRecord" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRecord_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />
                            <asp:GridView runat="server" ID="gvInvoice" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbInvocie" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="SALE_INVOICE_ID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel" />
                                            <ItemStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Invoice #" DataField="InvoiceNo" ReadOnly="true">
                                            <ItemStyle Width="30%" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Gross Amount" DataField="AMOUNTDUE" ReadOnly="true">
                                            <ItemStyle Width="30%" />
                                        </asp:BoundField>                                                                                
                                    </Columns>
                                </asp:GridView>                            
                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" OnClientClick="return confirmation(this);" Text="Delete Purchase" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
                <!-- end span12 -->
            </div>
        </div>
    </section>
</asp:Content>