<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmFranchiseSaleInvoice.aspx.cs" Inherits="Forms_frmFranchiseSaleInvoice" Title="CORN :: Franchise Sale Invoice" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
<script type="text/javascript" src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
    <script type="text/javascript" src="../AjaxLibrary/jquery-1.6.1.min.js"></script>

    <script type="text/javascript">
        function ddlItemIndexChanged(s, e) {
            hfInventoryType = document.getElementById('<%=hfInventoryType.ClientID%>').value;
            document.getElementById('<%=txtQuantity.ClientID%>').focus();
        }

        function QtyKeyPress(txt, event) {

            var charCode = (event.which) ? event.which : event.keyCode;

            if (hfInventoryType == '0') {
                if (charCode == 9 || charCode == 8) {
                return true;
            }
            if (charCode == 46) {
                if (txt.value.indexOf(".") < 0)
                    return true;
                return false;
            }
            if (charCode == 31 || charCode < 48 || charCode > 57)
                return false;
            }
            else
            {
                return false
            }
            
            return true;
        }

        function pageLoad() {
            hfInventoryType = document.getElementById('<%=hfInventoryType.ClientID%>').value;
            if (hfInventoryType == '0') {
                //Do nothing
            }
            else {
                $.ajax({
                    url: "http://localhost/CORNWeighingScale/Home/GetData",
                    data: { id: '1' },
                    type: "GET",
                    dataType: "jsonp",
                    jsonp: "callback",
                    success: function (data) {
                        if (data.indexOf("does not exist") == -1) {
                            data = data.replace(/[^\d.,]+/, '');
                            document.getElementById('<%=txtQuantity.ClientID%>').value = data.replace(/[^\d.,]+/, '');
                        }
                    }
                });
            }

            $("#button2").click(function () {
                $.getJSON("http://localhost/CORNWeighingScale/Home/GetData?id=1&callback=?", function (data) {
                    alert(data);
                });
            });
        }
    </script>

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

        function CalculateAmount() {

            var Qty = document.getElementById('<%=txtQuantity.ClientID%>').value;
            var Rate = document.getElementById('<%=txtPrice.ClientID%>').value;

            document.getElementById("<%= txtAmount.ClientID %>").value = (Qty * Rate).toFixed(2);
        }
        function ValidateForm() {
            var str;
            str = document.getElementById('<%=txtQuantity.ClientID%>').value;
            if ((str == null || str.length == 0) && DocNo.GetText() == "New") {
                alert('Must Enter Quantity');
                document.getElementById('<%=txtQuantity.ClientID%>').focus();
                return false;
            }

            str = document.getElementById('<%=txtPrice.ClientID%>').value;
            if ((str == null || str.length == 0) && DocNo.GetText() == "New") {
                alert('Must Enter Price');
                document.getElementById('<%=txtPrice.ClientID%>').focus();
                return false;
            }

            return true;
        }


    </script>
    <div class="main-contents">
        <div class="container">

            <div class="row top">
                <asp:Panel ID="Panel3" runat="server" DefaultButton="btnsearch">
                    <div class="col-md-4">
                        <div class="search">
                            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" CssClass="form-control"
                                TabIndex="0"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1" style="margin-left: -60px;">
                        <asp:LinkButton ID="btnsearch" OnClick="btnFilter_Click" runat="server" Text="Search"
                            CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                    </div>
                    <asp:LinkButton ID="btndummy" runat="server" UseSubmitBehavior="false" />
                </asp:Panel>
                <div class="col-md-offset-4 col-md-3" style="float:right;">
                    <div class="btnlist pull-right">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd" Text="Add"
                                    OnClick="btnAdd_Click">
                               <span class="fa fa-plus-circle"></span>Add
                                </asp:LinkButton>
                                <asp:LinkButton class="btn btn-success" ID="btnActive" runat="server"
                                    OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                                <asp:LinkButton runat="server" ID="hbtn"></asp:LinkButton>
                                <!-- POP UP MODEL-->
                                <cc1:ModalPopupExtender ID="mPopUpLocation" runat="server" PopupControlID="pnlParameters"
                                    TargetControlID="hbtn" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                                    CancelControlID="btnClose">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="pnlParameters" runat="server" Style="display: none;" ScrollBars="Auto" DefaultButton="btnSave">
                                    <div class="modal-dialog" style="width: 80%; margin-left: 0px">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_Click">
                                                    <span>&times;</span><span class="sr-only">Close</span></button>
                                                <h1 class="modal-title" id="myModalLabel">
                                                    <span></span>Add Invoice</h1>
                                            </div>
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                <ContentTemplate>
                                                    <div class="modal-body">
                                                        <div style="z-index: 101; left: 40px; width: 100px; position: absolute; top: 10px; height: 100px"
                                                            id="DIV1">
                                                            <asp:UpdateProgress ID="UpdateProgress5" runat="server">
                                                                <ProgressTemplate>
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif"></asp:ImageButton>&nbsp; Loading....
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-10">
                                                                <asp:Literal ID="lblErrorMsg" runat="server" Visible="false"></asp:Literal>
                                                            </div>
                                                        </div>
                                                        

                                                        <!--- FORM STARTS HERE ---->

                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                             <label><span class="fa fa-caret-right rgt_cart"></span></label>
                                                            <dx:ASPxRadioButtonList runat="server" ID="rdoDocType" AutoPostBack="true" Width="280px"
                                                                 RepeatDirection="Horizontal" Border-BorderStyle="None" OnSelectedIndexChanged="rdoDocType_SelectedIndexChanged">
                                                                <Items>
                                                                <dx:ListEditItem Value="0" Text=" None" Selected="True" />
                                                                <dx:ListEditItem Value="1" Text=" Demand No" />
                                                                </Items>
                                                            </dx:ASPxRadioButtonList>
                                                        </div>
                                                        <div class="col-md-6" runat="server" id="docRow" visible="false">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Demand No</label>
                                                            <dx:ASPxComboBox ID="drpDocumentNo" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="drpDocumentNo_SelectedIndexChanged">
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                             <asp:HiddenField ID="hfMaster_ID" runat="server" Value="0" />
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>

                                                            <dx:ASPxComboBox ID="ddlLocation" runat="server" CssClass="form-control"
                                                                OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true">
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Franchise</label>
                                                             (Sale % : <asp:Label ID="lblSalePer" runat="server"></asp:Label>)
                                                            <dx:ASPxComboBox ID="ddlCustomer" runat="server" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                                                            </dx:ASPxComboBox>

                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <span class="fa fa-caret-right rgt_cart"></span>
                                                            <asp:Label ID="Label2" runat="server" Text="Ref No" MaxLength="100" />
                                                            <asp:TextBox ID="txtRefNo" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <span class="fa fa-caret-right rgt_cart"></span>
                                                            <asp:Label ID="Label1" runat="server" Text="Remarks" />
                                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="1000" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
											
											<div>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <div class="row" runat="server" visible="false" id="qtyRow">
                                                            <div class="col-md-4 mymargines2">
                                                                </div>
                                                            <div class="col-md-1 mymargines">
                                                                </div>
                                                             <div class="col-md-3 mymargines" runat="server">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>
                                                                    <asp:Label ForeColor="Green" ID="deliveredQty" runat="server" Text="Delivered Qty: 0"></asp:Label> </label>
                                                            </div>
                                                            <asp:Label Visible="false" ID="txtQtyToDeliver" runat="server" Text="0"></asp:Label>
                                                        </div>
                                                       
                                                        <div class="row">
                                                            <div class="col-md-4 mymargines2">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Item Description</label>
                                                            </div>
                                                            <div class="col-md-1 mymargines">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>UOM</label>
                                                            </div>
                                                            <div class="col-md-1 mymargines">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Qty</label>
                                                            </div>
                                                            <div class="col-md-1 mymargines" id="divPrice" runat="server">
                                                                <span class="fa fa-caret-right rgt_cart"></span>
                                                                    <asp:Label ID="lblPrice" runat="server" Text="Avg. Price" />
                                                            </div>
                                                            <div class="col-md-1 mymargines" id="divAmount" runat="server">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Amount</label>
                                                            </div>
                                                            <div class="col-md-3 mymargines" runat="server">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span><asp:Label ForeColor="Red" ID="lblLastPrice" runat="server" Text="Last Price : 0"></asp:Label> </label>
                                                            </div>
                                                            <div class="col-md-1 mymargines">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Stock</label>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-4 mymargines2">
                                                                <dx:ASPxComboBox ID="ddlSkus" runat="server" CssClass="form-control"
                                                                    ClientInstanceName="ddlItem" ClientSideEvents-SelectedIndexChanged="function(s,e){ddlItemIndexChanged();}"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlSkus_SelectedIndexChanged">
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                            <div class="col-md-1 mymargines">
                                                                <asp:TextBox ID="txtUOM" runat="server" CssClass="form-control"
                                                                    Enabled="false"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-1 mymargines">
                                                                <asp:TextBox ID="txtQuantity" Width="100%" runat="server" CssClass="form-control"
                                                                    onkeypress="return QtyKeyPress(this,event);" onblur="CalculateAmount();"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-1 mymargines" id="divPrice2" runat="server">
                                                                <asp:TextBox ID="txtPrice" Width="100%" onblur="CalculateAmount();" runat="server"
                                                                    onkeypress="return onlyDotsAndNumbers(this,event);" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-1 mymargines" id="divAmount2" runat="server">
                                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2 mymargines" runat="server">
                                                                <dx:ASPxComboBox ID="ddlPriceLevel" AutoPostBack="true" 
                                                                    OnSelectedIndexChanged="ddlPriceLevel_SelectedIndexChanged"
                                                                     runat="server" CssClass="form-control">
                                                                </dx:ASPxComboBox>
                                                            </div>
                                                            <div class="col-md-1 mymargines">
                                                                <asp:HiddenField ID="hfInventoryType" runat="server" Value="0" />
                                                                <asp:Button AccessKey="A" ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Add" CssClass="btn btn-success" />
                                                            </div>
                                                            <div class="col-md-1 mymargines">
                                                                <label>
                                                                    <asp:Label ID="lblStock" runat="server" Text="0" style="color:#333"></asp:Label>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <div class="row center">
                                                            <div class="col-md-12">
                                                                <asp:HiddenField ID="_rowNo" runat="server" Value="0" />
                                                                <asp:HiddenField ID="_privouseQty" runat="server" Value="0" />
                                                                <div class="emp-table">
                                                                    <asp:Panel ID="Panel2" runat="server" Height="150px" Width="100%" ScrollBars="Vertical"
                                                                        BorderWidth="1px" BorderStyle="Groove" BorderColor="Silver">
                                                                        <asp:GridView ID="GrdPurchase" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center"
                                                                            CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                                                            OnRowDeleting="GrdPurchase_RowDeleting" OnRowEditing="GrdPurchase_RowEditing">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID" ReadOnly="true">
                                                                                    <HeaderStyle CssClass="HidePanel" />
                                                                                    <ItemStyle CssClass="HidePanel" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="SKU_CODE" ReadOnly="true">
                                                                                    <HeaderStyle CssClass="HidePanel" />
                                                                                    <ItemStyle CssClass="HidePanel" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="SKU_NAME" HeaderText="Item" ReadOnly="true">
                                                                                    <ItemStyle Width="35%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="UOM_DESC" HeaderText="UOM" ReadOnly="true">
                                                                                    <ItemStyle Width="10%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Quantity" HeaderText="Qty" DataFormatString="{0:f2}" ReadOnly="true">
                                                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="PRICE" HeaderText="Price" DataFormatString="{0:f2}" ReadOnly="true">
                                                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="SalePercentAmount" HeaderText="Sale % Amount" DataFormatString="{0:f2}" ReadOnly="true">
                                                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="PRICE_LEVEL_AMOUNT" HeaderText="Price Factor Amount" DataFormatString="{0:f2}" ReadOnly="true">
                                                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:f2}" ReadOnly="true">
                                                                                    <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="UOM_ID" ReadOnly="true">
                                                                                    <HeaderStyle CssClass="HidePanel" />
                                                                                    <ItemStyle CssClass="HidePanel" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="FRANCHISE_DETAIL_ID" HeaderText="FRANCHISE_DETAIL_ID" ReadOnly="true">
                                                                                    <HeaderStyle CssClass="HidePanel" />
                                                                                    <ItemStyle CssClass="HidePanel" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="PRICE_LEVEL" HeaderText="PRICE" ReadOnly="true">
                                                                                    <HeaderStyle CssClass="HidePanel" />
                                                                                    <ItemStyle CssClass="HidePanel" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="PRICE_LEVEL_FACTOR" HeaderText="PRICE_LEVEL_FACTOR" ReadOnly="true">
                                                                                    <HeaderStyle CssClass="HidePanel" />
                                                                                    <ItemStyle CssClass="HidePanel" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="SKU_PRICES_LEVEL_ID" HeaderText="SKU_PRICES_LEVEL_ID" ReadOnly="true">
                                                                                    <HeaderStyle CssClass="HidePanel" />
                                                                                    <ItemStyle CssClass="HidePanel" />
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" ToolTip="Edit">
                                                                                        </asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to Delete?');return false;"
                                                                                            class="fa fa-trash-o"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                            </div>
                                                            <div class="col-md-2">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Total Quantity</label>
                                                                <asp:TextBox ID="txtTotalQuantity" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2" id="divGrossAmount" runat="server">
                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Gross Amount</label>
                                                                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-7">
                                                            </div>

                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <div class="row">
                                                    <div class="col-md-offset-5 col-md-3 ">
                                                        <div class="btnlist pull-right">
                                                            <asp:Button ID="btnSaveDocument" OnClick="btnSave_Document" AccessKey="S" runat="server" Text="Save" UseSubmitBehavior="False" CssClass="btn btn-success" />
                                                            <asp:Button ID="btnCancel" AccessKey="C" runat="server" Text="Cancel" UseSubmitBehavior="False" CssClass="btn btn-danger" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="row center">
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanelDetail" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="Grid_users" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                    AutoGenerateColumns="False" OnPageIndexChanging="Grid_users_PageIndexChanging" PageSize="20" AllowPaging="true"
                                    OnRowEditing="Grid_users_RowEditing" EmptyDataText="No Record exist"
                                    OnRowDataBound="Grid_users_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                             &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FRANCHISE_MASTER_ID" HeaderText="FRANCHISE_MASTER_ID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Distributor_ID" HeaderText="Distributor" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUSTOMER_ID" HeaderText="CUSTOMER_ID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REMARKS" HeaderText="REMARKS" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REF_NO" HeaderText="Reference No" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TOTAL_AMOUNT" HeaderText="Total Amount" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="18%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DOCUMENT_DATE" HeaderText="Date" ReadOnly="true" DataFormatString="{0:dd-MMM-yyyy}">
                                            <ItemStyle CssClass="grdDetail" Width="18%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="STOCK_DEMAND_ID" ReadOnly="true">
                                           <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>                                         
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" class="fa fa-pencil" CommandName="Edit" ToolTip="Edit">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                                    <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
