<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMain.master" AutoEventWireup="true" CodeFile="Invoice.aspx.cs" Inherits="Invoice" %>
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
                                <asp:ListItem Value="1" Text="Rollback" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Date Change"></asp:ListItem>
                            </asp:RadioButtonList>
                            <br />
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
                            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="txtDate"
                                TargetControlID="txtDate"></cc1:CalendarExtender>
                            <br />
                            <asp:Button ID="btnLoad" runat="server" OnClick="btnLoad_Click" Text="Load Data" class="btn btn-primary" />
                            <br />
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
                            <br />
                            <strong> <asp:Label runat="server" ID="lblNewDate" Text="New Date" Visible="false"></asp:Label> </strong>
                            <br />
                            <asp:TextBox ID="txtNewDate" runat="server" Visible="false"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="txtNewDate"
                                TargetControlID="txtNewDate"></cc1:CalendarExtender>
                            <br />
                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" OnClientClick="return confirmation(this);" Text="Rollback Invocie" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
                <!-- end span12 -->
            </div>
        </div>
    </section>
</asp:Content>