<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMain.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cDefault" ContentPlaceHolderID="cphChild" runat="Server">
    <style>
        .HidePanel {
            display: none;
        }
    </style>
    <script language="JavaScript" type="text/javascript">
        function CheckBoxListSelect() {
            var chkBoxList = document.getElementById('<%= cblLocation.ClientID %>');
            var chkBox = document.getElementById('<%= cbAll.ClientID %>');
            var chkBoxCount;
            var i;
            if (chkBox.checked == true) {
                chkBoxCount = chkBoxList.getElementsByTagName("input");
                for (i = 0; i < chkBoxCount.length; i++) {
                    chkBoxCount[i].checked = true;
                }
            } else {
                chkBoxCount = chkBoxList.getElementsByTagName("input");
                for (i = 0; i < chkBoxCount.length; i++) {

                    chkBoxCount[i].checked = false;

                }
            }
        }
        function UnCheckSelectAll() {
            var chkBox = document.getElementById('<%= cbAll.ClientID %>');
            var chkBoxList = document.getElementById('<%= cblLocation.ClientID %>');
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            var count = 0;
            for (var i = 0; i < chkBoxCount.length; i++) {
                if (chkBoxCount[i].checked == false) {
                    count += 1;
                }
            }
            if (count > 0) {
                chkBox.checked = false;
            }
            else {
                chkBox.checked = true;
            }
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
                    <div class="row" style="margin-bottom: 0px;" runat="server" id="rowLicense" visible="false">
                        <div class="span8">
                            <strong>Activity Type</strong>
                            <br />
                            <asp:DropDownList ID="rblType" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                                <asp:ListItem Value="1" Text="License" Selected="true"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Activate"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Invoice Footer"></asp:ListItem>
                                <asp:ListItem Value="4" Text="GST-NTN Lable"></asp:ListItem>
                                <asp:ListItem Value="5" Text="Add/Remove Menu"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Delete Pending KOTs"></asp:ListItem>
                                <asp:ListItem Value="7" Text="Insert Prices"></asp:ListItem>
                            </asp:DropDownList>                            
                            <br />
                            <strong>Select Database</strong>
                            <br />
                            <asp:DropDownList ID="ddlDB" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlDB_SelectedIndexChanged">
                            </asp:DropDownList>
                            <br />
                            <asp:CheckBox ID="cbAll" runat="server" Text="Select All" Checked="true" onclick="CheckBoxListSelect()" />
                            <asp:Panel ID="pnlDb" runat="server" Height="250" ScrollBars="Vertical" BorderColor="Black" Width="100%" Style="overflow-y: scroll;">
                                <asp:CheckBoxList ID="cblLocation" runat="server" Width="100%" onclick="UnCheckSelectAll()">
                                </asp:CheckBoxList>
                                <asp:GridView runat="server" ID="gvLocation" AutoGenerateColumns="False" Visible="false"
                                    OnRowDataBound="gvLocation_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="DISTRIBUTOR_ID" HeaderText="DISTRIBUTOR_ID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel" />
                                            <ItemStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Location Name" DataField="DISTRIBUTOR_NAME" ReadOnly="true">
                                            <ItemStyle Width="35%" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Invoice Footer Format">
                                            <ItemTemplate>
                                                <asp:RadioButtonList ID="rbType" runat="server" RepeatDirection="Horizontal" Width="100%">
                                                    <asp:ListItem Value="0" Selected="True" Text="Default"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Both Cash Cr"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="New"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </ItemTemplate>
                                            <ItemStyle Width="65%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="InvoiceGSTFooter" HeaderText="InvoiceGSTFooter" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel" />
                                            <ItemStyle CssClass="HidePanel" />
                                        </asp:BoundField>                                        
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView runat="server" ID="gvLocation2" AutoGenerateColumns="False" Visible="false"
                                    OnRowDataBound="gvLocation2_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="DISTRIBUTOR_ID" HeaderText="DISTRIBUTOR_ID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel" />
                                            <ItemStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Location Name" DataField="DISTRIBUTOR_NAME" ReadOnly="true">
                                            <ItemStyle Width="30%" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="GST Label">
                                            <ItemTemplate>
                                                <asp:TextBox id="txtGST" runat="server" Width="100" ></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NTN Label">
                                            <ItemTemplate>
                                                <asp:TextBox id="txtNTN" runat="server" Width="100"></asp:TextBox>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TAX_AUTHORITY" HeaderText="TAX_AUTHORITY" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel" />
                                            <ItemStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TAX_AUTHORITY2" HeaderText="TAX_AUTHORITY2" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel" />
                                            <ItemStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <asp:CheckBoxList ID="cblMenu" runat="server" Visible="false">

                                </asp:CheckBoxList>

                                <div id="dvPrices" runat="server" visible="false">
                                    <table style="width:100%;">
                                        <tr>
                                            <td style="width:50%;">
                                                <strong>From Location</strong>
                                            </td>
                                            <td style="width:50%;">
                                                <strong>To Location</strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:50%;">
                                                <asp:DropDownList ID="ddlFrom" runat="server"></asp:DropDownList>
                                            </td>
                                            <td style="width:50%;">
                                                <asp:DropDownList ID="ddlTo" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="span3" runat="server" id="divButton">
                            <br />
                            <br />
                            <strong>
                                <asp:Label ID="lblDate" runat="server" Text="Select Date"></asp:Label>
                            </strong>
                            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                            <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="txtDate"
                                TargetControlID="txtDate"></cc1:CalendarExtender>
                        </div>
                        <div class="span1">
                            <br />
                            <br />
                            <br />
                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update License" class="btn btn-primary" />
                        </div>
                    </div>
                    <div class="row" style="margin-bottom: 0px;" runat="server" id="rowLogin">
                        <div class="span6">
                            <strong>Username</strong>
                            <br />
                            <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
                            <br />
                            <strong>Password</strong>
                            <br />
                            <asp:TextBox ID="txtPass" runat="server" TextMode="Password"></asp:TextBox>
                            <br />
                            <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
                <!-- end span12 -->
            </div>
        </div>
    </section>
</asp:Content>