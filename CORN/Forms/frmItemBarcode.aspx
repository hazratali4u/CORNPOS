<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmItemBarcode.aspx.cs" Inherits="Forms_frmItemBarcode" Title="CORN :: Item Barcode" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" runat="Server">
    <%--<script src="../AjaxLibrary/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>--%>
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            //$("select").searchable();
        }
        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txt_row.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert no of Rows..');
                return false;
            }
            str = document.getElementById('<%=txt_col.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert no of colmns..');
                return false;
            }
            var str;

            str = document.getElementById('<%=txt_productName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Select Item');
                return false;
            }
            str = document.getElementById('<%=txt_code.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert BarCode');
                return false;
            }
            str = document.getElementById('<%=txt_price.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert Code');
                return false;
            }
            return true;
        }

        function ValidateForm2() {
            var str;

            str = document.getElementById('<%=txt_productName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Select Item');
                return false;
            }
            str = document.getElementById('<%=txt_code.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert Code');
                return false;
            }
            str = document.getElementById('<%=txt_price.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert Price');
                return false;
            }
            str = document.getElementById('<%=txt_row.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Plz Insert no of Rows..');
                return false;
            }
            else if (str.length > 10) {
                alert('Range is not valid..');
            }

            return true;
        }
        function ProductSelected(source, eventArgs) {
            var SKUDetail = eventArgs.get_text();
            var num = eventArgs.get_value();
            num = Math.round(num);

            document.getElementById("<%= txt_price.ClientID %>").value = num;

            document.getElementById("<%=txt_code.ClientID %>").value = SKUDetail.substring(0, SKUDetail.indexOf('-'));
            document.getElementById("<%=txt_productName.ClientID %>").value = SKUDetail.substring(SKUDetail.indexOf('-') + 1);
            document.getElementById("<%= btnDummy.ClientID %>").click();
        }
    </script>
    <div id="right_data">
        <asp:UpdatePanel ID="up_pnl1" runat="server">
            <ContentTemplate>
                <div>

                    <div class="row">
                       <div class="col-md-4">
                         <label><span class="fa fa-caret-right rgt_cart"></span>Sheet</label>

                         <dx:ASPxComboBox ID="ddlSheet" runat="server" CssClass="form-control" SelectedIndex="0">
                             <Items>
                                 <dx:ListEditItem Text ="Barcode Printer (4 x 1 inches)" Value="1" />
                                 <dx:ListEditItem Text ="A4 Sticker Sheet" Value="2" />
                                 <dx:ListEditItem Text ="Single Sticker Barcode Printer" Value="3" />
                             </Items>
                         </dx:ASPxComboBox>
                        </div>

                        <div class="col-md-4">
                         <label><span class="fa fa-caret-right rgt_cart"></span>Sticker Size</label>

                         <dx:ASPxComboBox ID="ddlStickerSize" runat="server" CssClass="form-control" SelectedIndex="0">
                             <Items>
                                 <dx:ListEditItem Text ="2 x 1 inches" Value="1" />
                                 <dx:ListEditItem Text ="1.18 x 0.74 inches" Value="2" />
                             </Items>
                         </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>

                         <dx:ASPxComboBox ID="drpDistributor" runat="server" AutoPostBack="true"
                              CssClass="form-control" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                         </dx:ASPxComboBox>
                            <%--<asp:HiddenField runat="server" ID="hfgstPercent" Value="0" />--%>
                        </div>

                         <div class="col-md-4">
                            <label><span class="fa fa-caret-right rgt_cart"></span>Item Name</label>

                         <dx:ASPxComboBox ID="ddlSKU" runat="server" AutoPostBack="true"
                              CssClass="form-control" OnSelectedIndexChanged="ddlSKU_SelectedIndexChanged">
                         </dx:ASPxComboBox>
                        </div>
                    </div>

                    <table cellspacing="10" style="border-collapse: separate;border-spacing: 10px;">
                        <tr>
                            <td>
                                <strong>Item Name</strong>
                                <asp:Button ID="btnDummy" runat="server" CssClass="HidePanel" />
                            </td>
                            <td>
                                <asp:TextBox ID="txt_productName" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                                <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                <cc1:AutoCompleteExtender ID="aceProduct" runat="server" TargetControlID="txt_productName"
                                    ServicePath="wsProductList.asmx" MinimumPrefixLength="1" CompletionInterval="500"
                                    CompletionSetCount="10" CompletionListCssClass="autocomplete_completionListElement"
                                    CompletionListItemCssClass="autocomplete_listItem" EnableCaching="true" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    OnClientItemSelected="ProductSelected" FirstRowSelected="true" ServiceMethod="GetBarcodeProducts">
                                </cc1:AutoCompleteExtender>
                            </td>
                            <td style="width: 100px;"></td>
                        </tr>
                        <tr>
                            <td>
                                <strong>BarCode</strong>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_code" runat="server" Width="200px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Price</strong>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_price" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                          <tr>
                            <td>
                                <strong>Price Inc. GST</strong>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPriceWithGST" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td>
                                <strong>Size</strong>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSize" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td>
                                <strong>Color</strong>
                            </td>
                            <td>
                                <asp:TextBox ID="txtColor" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong>Print</strong>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbCompany" runat="server" Text="Location Name" Checked="true" />
                                &nbsp;
                                <asp:CheckBox ID="cbName" runat="server" Text="Item Name" Checked="true" />
                                &nbsp;
                                <asp:CheckBox ID="cbPrice" runat="server" Text="Item Price" Checked="true" AutoPostBack="true" OnCheckedChanged="cbPrice_CheckedChanged" />
                                &nbsp;
                                <asp:CheckBox Visible="false" ID="cbSize" runat="server" Text="Item Size" Checked="false" />
                                &nbsp;
                                <asp:CheckBox Visible="false" ID="cbColor" runat="server" Text="Item Color" Checked="false" />
                                 &nbsp;
                                <asp:CheckBox ID="chbPriceWithGST" runat="server" Text="Item Price Incl. GST" AutoPostBack="true" Checked="false" OnCheckedChanged="chbPriceWithGST_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label Text="No Of Rows " runat="server" Visible="true" Font-Bold="true"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_row" runat="server" Width="60" MaxLength="2" Text="10" Visible="true"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbe_txtrow" runat="server" ValidChars="0123456789"
                                    TargetControlID="txt_row">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txt_row"
                                    MaximumValue="10" MinimumValue="1" SetFocusOnError="true" ValidationGroup="vg"></asp:RangeValidator>
                                <asp:TextBox ID="txt_col" runat="server" Width="60" MaxLength="1" Text="3" ReadOnly="true" Visible="false"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbe_txtcol" runat="server" ValidChars="012345"
                                    TargetControlID="txt_col">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel1" runat="server" Visible="false">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_companyname" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_color" runat="server"></asp:Label>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_pcode" runat="server"></asp:Label>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_pprice" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Image ID="img_brcode" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btn_print" runat="server" Text="Print" OnClick="btn_print_Click"
                            Width="80" CssClass="Button" Visible="false" />
                        <asp:Button ID="btn_generate" runat="server" Text="Generate" OnClick="btn_generate_Click"
                            Width="80" CssClass="Button" CausesValidation="true" ValidationGroup="vg" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>