<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmDownloadKOTService.aspx.cs" Inherits="Forms_frmDownloadKOTService" Title="CORN :: Download KOT Service" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
    <script type="text/javascript" src="../AjaxLibrary/jquery-1.6.1.min.js"></script>
    <script language="JavaScript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
        function Check_Click(objRef) {
            var row = objRef.parentNode.parentNode;
            var GridView = row.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var headerCheckBox = inputList[0];
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <div class="row center">
                <div class="col-md-12">
                    <div class="emp-table">
                        <h3>Please follow the below steps to install KOT service in your computer:</h3>
                        <ul>
                            <li>1- Verify the latest software build on client</li>
                            <li>2-  Install Crystal Report Tool, Download 64 bit from <a href="https://downloads.i-theses.com/index.php?option=com_downloads&task=downloads&groupid=9&id=101" target="_blank">here</a> and 32 bit from <a href="https://downloads.i-theses.com/index.php?option=com_downloads&task=downloads&groupid=9&id=100" target="_blank">here</a></li>
                            <li>3- Click the Download KOT Service button and download KOT Service</li>
                            <li>4- Once downloaded, unzip </li>
                            <li>5- Stop Antivirus </li>
                            <li>6- Run as Administrator </li>
                            <li>7- After installation, download configuration file by clicking on Download Configuration File below. "CORNPOSKOTPrint.exe" will be downloaded</li>
                            <li>8- Copy the "CORNPOSKOTPrint.exe" from the downloaded folder. </li>
                            <li>9- Paste and Replace "CORNPOSKOTPrint.exe" file at path. C:\Program Files (x86)\CORN Point of Sale\CORN KOT Printer </li>
                            <li>10- Downoad logo of client to path. C:\Program Files (x86)\CORN Point of Sale\CORN KOT Printer </li>
                            <li>11- In CORNPOSKOTPrint.exe file set value of LogoPath with logo path</li>
                            <li>12- Verify Printing </li>
                        </ul>
                        <br />
                        <div class="col-md-4" style="margin-top: 23px;"><a class="btn btn-success" href="https://download.cornpos.com/cornkotservice/cornkot.zip">Download KOT Service</a></div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                    <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <fieldset style="border: 2px solid #ccc; padding: 10px; width: 100%;">
                                <legend style="font-weight: bold;">Section Wise Print Configuration</legend>
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:CheckBox ID="cbSectionWise" runat="server" Text="Section Wise Print" Checked="true" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:CheckBox ID="cbFullKOT" runat="server" Text="Full KOT" AutoPostBack="true" OnCheckedChanged="cbFullKOT_CheckedChanged" />
                                        <asp:RadioButtonList ID="rblFullKOT" runat="server" RepeatDirection="Horizontal" Width="100%" Visible="false">
                                            <asp:ListItem Value="1" Text="Section Wise" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Category Wise"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:CheckBox ID="cbLocationName" runat="server" Text="Print Location Name" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:CheckBox ID="cbExcludeCancelKOT" runat="server" Text="Exclude Item Less and Cancel" />
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <fieldset style="border: 2px solid #ccc; padding: 10px; width: 100%;">
                                <legend style="font-weight: bold;">Sticker Print Configuration</legend>
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtStickerPrinterName" runat="server" CssClass="form-control" placeholder="Sticker Printer Name" Style="margin-bottom: 5px;"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ddlStickerPrinter" runat="server" CssClass="form-control" Style="margin-bottom: 5px;">
                                            <asp:ListItem Value="0" Text="Select Sticker Print Option" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Print On Sticker & Section"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Coffee Cup Sticker"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Pizz Box Sticker"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <fieldset style="border: 2px solid #ccc; padding: 10px; width: 100%;">
                                <legend style="font-weight: bold;">Non-Section Wise Configuration</legend>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="upAdd" runat="server">
                                            <ContentTemplate>
                                                <table style="width: 100%" cellspacing="10" cellpadding="10">                                                    
                                                    <tr>
                                                        <td style="width:70%">
                                                            <asp:TextBox ID="txtPrinterName" runat="server" CssClass="form-control" placeholder="Enter Printer Name"></asp:TextBox>
                                                        </td>
                                                        <td align="center" style="width:30%" valign="top">
                                                            <asp:Button ID="btnAdd" OnClick="btnAdd_Click" runat="server" Text="Add Printer" CssClass="btn btn-success" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:GridView ID="gvPrinter" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvPrinter_RowDataBound"
                                                    CssClass="table table-striped table-bordered table-hover table-condensed cf">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Printer Name" DataField="PrinterName" ReadOnly="true">
                                                            <ItemStyle Width="30%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Full KOT">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbFullKOT" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="FullKOT" ReadOnly="true">
                                                            <ItemStyle CssClass="hidden"/>
                                                            <HeaderStyle CssClass="hidden"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Expeditor">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbExpeditor" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Expeditor" ReadOnly="true">
                                                            <ItemStyle CssClass="hidden"/>
                                                            <HeaderStyle CssClass="hidden"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Print Invoice">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbPrintInvoice" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PrintInvoice" ReadOnly="true">
                                                            <ItemStyle CssClass="hidden"/>
                                                            <HeaderStyle CssClass="hidden"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Location Name">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbLocationName" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="LocationName" ReadOnly="true">
                                                            <ItemStyle CssClass="hidden"/>
                                                            <HeaderStyle CssClass="hidden"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Exclude Item Cancel & Less">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="cbExclude" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Exclude" ReadOnly="true">
                                                            <ItemStyle CssClass="hidden"/>
                                                            <HeaderStyle CssClass="hidden"/>
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Full KOT Xpeditor Group Option">
                                                            <ItemTemplate>
                                                                <asp:RadioButtonList ID="rblXpeditor" runat="server" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Value="1" Text="Section Wise" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="2" Text="Category Wise"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12" style="text-align: right;">
                            <br />
                            <asp:Button ID="btnDownloadConnectionString" AccessKey="S" OnClick="btnConnectionString_Click" runat="server" Text="Download Configuration File" UseSubmitBehavior="False" CssClass="btn btn-success" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>