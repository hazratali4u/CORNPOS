<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmItemInfo.aspx.cs" Inherits="Forms_frmItemInfo" Title="CORN :: Item Information" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script src="../AjaxLibrary/1.8.3jquery.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../js/angular.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function IsModifierChange() {
            if (document.getElementById('<%=chkIsModifier.ClientID%>').checked) {
                document.getElementById('ctl00_ctl00_mainCopy_cphPage_TabContainer1_TabPanel1_divColorPicker').style.visibility = 'visible';
            } else {
                document.getElementById('ctl00_ctl00_mainCopy_cphPage_TabContainer1_TabPanel1_divColorPicker').style.visibility = 'hidden';
            }
        }
        function Message(msg) {
            alert(msg);
        }
        function ValidateForm() {

            str = document.getElementById('<%=txtskuname.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Item name is required')
                return false;
            }
            str = document.getElementById('<%=drpStockUnit.ClientID%>').GetValue();
            if (str == null || str.length == 0) {
                alert('Select Stock unit')
                return false;
            }
            str = document.getElementById('<%=drpSaleUnit.ClientID%>').GetValue();
            if (str == null || str.length == 0) {
                alert('Select Sale unit')
                return false;
            }
            str = document.getElementById('<%=drpPurchaseUnit.ClientID%>').GetValue();
            if (str == null || str.length == 0) {
                alert('Select Purchase unit')
                return false;
            }

            return true;
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
        function ChkSerialization(chk) {
            if (chk == document.getElementById('<%=chkIsSerialized.ClientID%>')) {
                if (document.getElementById('<%=chkIsSerialized.ClientID%>').checked == true) {
                    document.getElementById('<%=txtSerialCode.ClientID%>').disabled = false;

                }
                else {
                    document.getElementById('<%=txtSerialCode.ClientID%>').disabled = true;
                    document.getElementById('<%=txtSerialCode.ClientID%>').value = "";
                }
            }
            if (chk == document.getElementById('<%=chkIsFEDItem.ClientID%>')) {
                if (document.getElementById('<%=chkIsFEDItem.ClientID%>').checked == true) {
                    document.getElementById('<%=txtFEDPercentage.ClientID%>').disabled = false;

                }
                else {
                    document.getElementById('<%=txtFEDPercentage.ClientID%>').disabled = true;
                    document.getElementById('<%=txtFEDPercentage.ClientID%>').value = "";
                }
            }
            if (chk == document.getElementById('<%=chkIsWHTItem.ClientID%>')) {
                if (document.getElementById('<%=chkIsWHTItem.ClientID%>').checked == true) {
                    document.getElementById('<%=txtWHTPercentage.ClientID%>').disabled = false;

                }
                else {
                    document.getElementById('<%=txtWHTPercentage.ClientID%>').disabled = true;
                    document.getElementById('<%=txtWHTPercentage.ClientID%>').value = "";
                }
            }
        }
    </script>
    <script type="text/javascript" language="javascript">

        function bindSKUImage() {
            debugger;
            var val = document.getElementById('<%=hidSKUImageName.ClientID%>').value;

            if (val == '' || val == null) {
                $('#pic').attr('src', '../images/no_image.gif').width(266).height(130);
            } else {
                var logo = val;
                $('#pic').attr('src', '../UserImages/Sku/' + logo).width(266).height(130);
            }
        }
        function readImageURL(input) {
            var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png)$/;
            if (input.files && input.files[0]) {

                var file = input.files[0];
                if (regex.test(file.name.toLowerCase())) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#pic').attr('src', e.target.result).width(266).height(130);
                        document.getElementById('<%=hidSKUImageSource.ClientID%>').value = e.target.result;
                    };
                    document.getElementById('<%=hidSKUImageName.ClientID%>').value = file.name;

                    reader.readAsDataURL(file);
                }
                else {
                    alert(file.name + " is not a valid image file.\n Only extentions .jpg|.jpeg|.gif|.png are supported.");
                    return false;
                }
            }
        }
    </script>

    <style>
        .form-group {
            border: 1px solid #ccc;
        }

        .btn-file {
            position: relative;
            overflow: hidden;
            float: left;
        }

            .btn-file input[type=file] {
                position: absolute;
                top: 0;
                right: 0;
                min-width: 90%;
                min-height: 90%;
                font-size: 100px;
                text-align: right;
                filter: alpha(opacity=0);
                opacity: 0;
                outline: none;
                background: white;
                cursor: inherit;
                display: block;
            }

        #img-upload, #img-upload2 {
            width: 100%;
        }
    </style>

    <style>
        .switch {
            position: relative;
            display: inline-block;
            width: 60px;
            height: 34px;
        }

            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 26px;
                width: 26px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }
    </style>

    <script type="text/javascript">
        var textSeparator = ";";
        function updateText() {
            var selectedItems = checkListBox.GetSelectedItems();
            checkComboBox.SetText(getSelectedItemsText(selectedItems));
        }
        function synchronizeListBoxValues(dropDown, args) {
            //checkListBox.UnselectAll();
            var texts = dropDown.GetText().split(textSeparator);
            var values = getValuesByTexts(texts);
            checkListBox.SelectValues(values);
            updateText(); // for remove non-existing texts
        }
        function getSelectedItemsText(items) {
            var texts = [];
            for (var i = 0; i < items.length; i++)
                texts.push(items[i].text);
            return texts.join(textSeparator);
        }
        function getValuesByTexts(texts) {
            var actualValues = [];
            var item;
            for (var i = 0; i < texts.length; i++) {
                item = checkListBox.FindItemByText(texts[i]);
                if (item != null)
                    actualValues.push(item.value);
            }
            return actualValues;
        }


        var bulkSavebtn = document.getElementById('<%= btnSaveBulk.ClientID %>');

        // Now we need to implement the KeysShortcut
        <%--        $("#ctl00_ctl00_mainCopy_cphPage_btnSaveBulk").onkeypress = function (event) {
    debugger;
    if (event.keyCode == 13 && event.altKey)
    {
      document.getElementById('<%= btnSaveBulk.ClientID %>').click();
    }
        };--%>

        $(document).on('keydown', function (e) {
            var code = e.keyCode || e.charCode;
            if ((e.altKey == true && code == 78) || (e.altKey == true && code == 110)) {
                document.getElementById('<%= ButtonAdd.ClientID %>').click();
            }
            else if ((e.altKey == true && code == 83) || (e.altKey == true && code == 115)) {
                document.getElementById('<%= btnSaveBulk.ClientID %>').click();
            }
        });
    </script>

    <div class="main-contents">
        <div class="container">
            <div class="row top">
                <asp:Panel ID="Panel4" runat="server" DefaultButton="btnsearch">
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
                <div class="col-md-offset-4 col-md-3" style="float: right;">
                    <div class="btnlist pull-right">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfStockSale" Value="0" runat="server" />
                                <asp:HiddenField ID="hfStockPurchase" Value="0" runat="server" />
                                <asp:HiddenField ID="hfSalePurchase" Value="0" runat="server" />
                                <asp:HiddenField ID="hfSaleStock" Value="0" runat="server" />
                                <asp:HiddenField ID="hfPurchaseSale" Value="0" runat="server" />
                                <asp:HiddenField ID="hfPurchaseStock" Value="0" runat="server" />

                                <asp:LinkButton CssClass="btn btn-info" runat="server" ID="btnBulkAdd" Text="Import Items"
                                    OnClick="btnBulkAdd_Click" Visible="false">
                               <span class="fa fa-plus-circle"></span> Import Items
                                </asp:LinkButton>

                                <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd" Text="Add"
                                    OnClick="btnAdd_Click">
                               <span class="fa fa-plus-circle"></span>Add
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="hbutton"></asp:LinkButton>
                                <asp:LinkButton class="btn btn-success" OnClick="btnActive_Click" ID="btnActive" runat="server"
                                    OnClientClick="javasacript:return confirm('Are your sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>

                                <!-- Bulk Add Modal -->
                                <ajaxToolkit:ModalPopupExtender ID="mPOPBulkAdd" runat="server" PopupControlID="pnlBulkAdd"
                                    TargetControlID="btnBulkAdd" BehaviorID="ModelPopupBulk" BackgroundCssClass="modal-background"
                                    CancelControlID="btnBulkClose">
                                </ajaxToolkit:ModalPopupExtender>

                                <asp:Panel ID="pnlBulkAdd" DefaultButton="btnSaveBulk" runat="server" Style="display: none; width: 30%" ScrollBars="Auto">
                                    <div class="modal-dialog2" style="width: 100%">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" id="btnBulkClose" class="close" runat="server" onserverclick="btnBulkClose_Click">
                                                    <span>&times;</span><span class="sr-only">Close</span></button>
                                                <h1 class="modal-title" id="myModalLabel1">
                                                    <span></span>Import Items</h1>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="main-contents" style="height: 100%;">
                                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <div class="modal-body">
                                                                        <asp:GridView ID="Gridview1" runat="server" ShowFooter="false" Visible="false"
                                                                            OnRowDataBound="BulkAdd_RowDataBound" OnRowCreated="GridView1_RowCreated" AutoGenerateColumns="false">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Category Type">
                                                                                    <ItemTemplate>
                                                                                        <dx:ASPxComboBox ID="ddlType1" runat="server" AutoPostBack="true"
                                                                                            CssClass="form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged1">
                                                                                        </dx:ASPxComboBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Item Section">
                                                                                    <ItemTemplate>
                                                                                        <dx:ASPxComboBox ID="ddlSection1" runat="server" CssClass="form-control">
                                                                                        </dx:ASPxComboBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Item Category">
                                                                                    <ItemTemplate>
                                                                                        <dx:ASPxComboBox ID="ddlCategory1" runat="server" CssClass="form-control">
                                                                                        </dx:ASPxComboBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Item Name">
                                                                                    <ItemStyle Width="200px" />
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtskuname1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        <%--<asp:TextBox ID="txtskucode" runat="server" CssClass="form-control" Style="display: none;"></asp:TextBox>--%>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Default Qty">
                                                                                    <ItemStyle Width="20px" />
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtDefaultQty1" Text="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Unit of Measure">
                                                                                    <ItemStyle Width="100px" />
                                                                                    <ItemTemplate>
                                                                                        <dx:ASPxComboBox ID="ddlUOM1" runat="server" CssClass="form-control">
                                                                                        </dx:ASPxComboBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Print Description">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtDescription1" runat="server" Rows="1" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Description">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtDescription3" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Attribute">
                                                                                    <ItemTemplate>
                                                                                        <dx:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ASPxDropDownEdit1" Width="285px" runat="server" AnimationType="None">
                                                                                            <DropDownWindowStyle BackColor="#EDEDED" />
                                                                                            <DropDownWindowTemplate>
                                                                                                <dx:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn"
                                                                                                    runat="server" Height="200" EnableSelectAll="true">
                                                                                                    <%--<FilteringSettings ShowSearchUI="true"/>--%>
                                                                                                    <Border BorderStyle="None" />
                                                                                                    <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                                                                                    <Items>
                                                                                                        <dx:ListEditItem Value="IsInventoryItem" Text="Is Inventory Item" />
                                                                                                        <dx:ListEditItem Value="IsRecipe" Text="Is Recipe" />
                                                                                                        <dx:ListEditItem Value="OversaleAllowed" Text="Oversale Allowed" />
                                                                                                        <dx:ListEditItem Value="IsHasModifier" Text="Is Has Modifier" />
                                                                                                        <dx:ListEditItem Value="IsInventoryByWeight" Text="Is Inventory By Weight" />
                                                                                                        <dx:ListEditItem Value="IsSaleByWeight" Text="Is Sale By Weight" />
                                                                                                        <dx:ListEditItem Value="IsDealItem" Text="Is Deal Item" />
                                                                                                        <dx:ListEditItem Value="IsModifier" Text="Is Modifier" />
                                                                                                        <dx:ListEditItem Value="UnGroupItemonPOS" Text="UnGroup Item on POS" />
                                                                                                        <dx:ListEditItem Value="IsPackageItem" Text="Is Package Item" />
                                                                                                    </Items>
                                                                                                    <ClientSideEvents SelectedIndexChanged="updateText" Init="updateText" />
                                                                                                </dx:ASPxListBox>
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="padding: 4px">
                                                                                                            <dx:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                                                                                <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                                                                            </dx:ASPxButton>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </DropDownWindowTemplate>
                                                                                            <ClientSideEvents TextChanged="synchronizeListBoxValues" DropDown="synchronizeListBoxValues" />
                                                                                        </dx:ASPxDropDownEdit>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Delete">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="bulkDeletebtn" OnClick="Gridview1_RowDeleting" runat="server">
                                                                                            <img src="../images/delete.gif" alt="Delete" />
                                                                                        </asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>

                                                                        </asp:GridView>
                                                                        <div class="row">
                                                                            <div class="col-md-3"></div>
                                                                            <div class="col-md-3"></div>
                                                                            <div class="col-md-3"></div>
                                                                            <div class="col-md-3">
                                                                                <asp:Button ID="ButtonAdd" OnClick="ButtonAdd_Click" runat="server" Text="Add New Row" Visible="false"/>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row">
                                                                        <div class="col-md-12">
                                                                            <div id="Div1" runat="server" visible="true">
                                                                            <label>
                                                                                <span class="fa fa-caret-right rgt_cart"></span>Upload Excel
                                                                                    &nbsp;(.xlsx | .xls)
                                                                            </label>
                                                                            <asp:FileUpload id="FileUpload1" runat="server" />
                                                                        </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px;">
                                                                        <div class="col-md-12" align="right">
                                                                            <asp:Button ID="btnExportCategoryTemplate" OnClick="btnExportCategoryTemplate_Click" runat="server" Text="Download Template" CssClass="btn btn-primary" />
                                                                            <asp:Button ID="btnImportCategory" OnClick="btnImportCategory_Click" runat="server" Style="margin-left: 5px" Text="Import" CssClass="btn btn-success" />
                                                                        </div>
                                                                    </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <%--<asp:AsyncPostBackTrigger ControlID = "btnImportCategory" EventName = "Click" />--%>
                                                                    <asp:PostBackTrigger ControlID="ButtonAdd" />
                                                                </Triggers>

                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="display: none;">
                                                        <div class="col-md-11" style="text-align: right; margin-top: 20px; margin-right: 40px">
                                                            <asp:Button ID="btnSaveBulk" OnClick="btnSaveBulk_Click" runat="server" Text="Save" CssClass="btn btn-success" />
                                                            <asp:Button ID="btnCancelBulk" OnClick="btnCancelBulk_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>


                                <!-- POP UP MODEL-->
                                <ajaxToolkit:ModalPopupExtender ID="mPopUpLocation" runat="server" PopupControlID="pnlParameters"
                                    TargetControlID="btnAdd" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                                    CancelControlID="btnClose">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="pnlParameters" DefaultButton="btnSave" runat="server" Style="display: none; width: 100%" ScrollBars="Auto">
                                    <div class="modal-dialog2" style="width: 70%">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_Click">
                                                    <span>&times;</span><span class="sr-only">Close</span></button>
                                                <h1 class="modal-title" id="myModalLabel">
                                                    <span></span>Add New Item Information</h1>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0"
                                                            Height="400px" Width="100%">
                                                            <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" Height="100%">
                                                                <HeaderTemplate>
                                                                    General
                                                                </HeaderTemplate>
                                                                <ContentTemplate>
                                                                    <div class="main-contents" style="overflow-y: auto; height: 100%;">
                                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:HiddenField ID="hidSKUImageName" runat="server" />
                                                                                <asp:HiddenField ID="hidSKUImageSource" runat="server" />
                                                                                <div class="modal-body">
                                                                                    <div class="row">
                                                                                        <div class="col-md-4">
                                                                                            <asp:HiddenField ID="hfSkuId" runat="server" />
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Category Type</label>
                                                                                            <dx:ASPxComboBox ID="ddlType" runat="server" AutoPostBack="true"
                                                                                                CssClass="form-control" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                                                            </dx:ASPxComboBox>
                                                                                        </div>
                                                                                        <div class="col-md-4">
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Item Section</label>
                                                                                            <dx:ASPxComboBox ID="ddlSection" runat="server" CssClass="form-control">
                                                                                            </dx:ASPxComboBox>
                                                                                        </div>
                                                                                        <div class="col-md-4">
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Item Category</label>
                                                                                            <dx:ASPxComboBox ID="ddlCategory" runat="server"
                                                                                                CssClass="form-control">
                                                                                            </dx:ASPxComboBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="row">
                                                                                        <div class="col-md-4">
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Item Name</label>
                                                                                            <asp:TextBox ID="txtskuname" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            <asp:TextBox ID="txtskucode" runat="server" CssClass="form-control" Style="display: none;"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="col-md-4">
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Default Qty</label>
                                                                                            <asp:TextBox ID="txtDefaultQty" Text="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server"
                                                                                                FilterType="Custom" ValidChars="0123456789." TargetControlID="txtDefaultQty"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                        </div>
                                                                                        <div class="col-md-4">
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Unit Of Measure</label>
                                                                                            <dx:ASPxComboBox ID="ddlUOM" runat="server" CssClass="form-control">
                                                                                            </dx:ASPxComboBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="row">
                                                                                        <div class="col-md-4">
                                                                                            <asp:Label runat="server" ID="lblMin"><span class="fa fa-caret-right rgt_cart"></span>Min Level</asp:Label>
                                                                                            <asp:TextBox ID="txtMinLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="ftbetxtMinLevel" runat="server"
                                                                                                FilterType="Custom" ValidChars="0123456789." TargetControlID="txtMinLevel"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                        </div>
                                                                                        <div class="col-md-4">
                                                                                            <asp:Label runat="server" ID="lblMax"><span class="fa fa-caret-right rgt_cart"></span>Max Level</asp:Label>
                                                                                            <asp:TextBox ID="txtMaxLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                                                                                FilterType="Custom" ValidChars="0123456789." TargetControlID="txtMaxLevel"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                        </div>
                                                                                        <div class="col-md-4">
                                                                                            <asp:Label runat="server" ID="lblReorder"><span class="fa fa-caret-right rgt_cart"></span>Reorder Level</asp:Label>
                                                                                            <asp:TextBox ID="txtReorderLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="ftbeReorderLevel" runat="server"
                                                                                                FilterType="Custom" ValidChars="0123456789." TargetControlID="txtReorderLevel"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="row">
                                                                                        <div class="col-md-4">
                                                                                            <div id="itemDescriptionPrint" class="row" runat="server">
                                                                                                <div class="col-md-12">
                                                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Print Description
                                                                                                        <asp:CheckBox ID="cbKOT" runat="server" Text="Show on KOT" Font-Bold="true" Visible="false"></asp:CheckBox></label>
                                                                                                    <asp:TextBox ID="txtDescription" runat="server" Rows="1" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-md-4">
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Description</label>
                                                                                            <asp:TextBox ID="txtDescription2" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="col-md-4">
                                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>KOT Description</label>
                                                                                            <asp:TextBox ID="txtKOTDescription" MaxLength="100" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="row">
                                                                                        <div class="col-md-6">
                                                                                            <div class="row" runat="server" visible="false">
                                                                                                <div class="col-md-6" runat="server" visible="false" id="trItemType">
                                                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Item Type</label>

                                                                                                    <dx:ASPxComboBox ID="drpProductType" runat="server"
                                                                                                        CssClass="form-control" SelectedIndex="0">
                                                                                                        <Items>
                                                                                                            <dx:ListEditItem Text="Quantity" Value="1" Selected="True"></dx:ListEditItem>
                                                                                                            <dx:ListEditItem Text="Value" Value="2"></dx:ListEditItem>
                                                                                                        </Items>
                                                                                                    </dx:ASPxComboBox>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="row">
                                                                                                <div class="col-md-6">
                                                                                                    <asp:CheckBox ID="chkIsInventory" runat="server" Text=" Is Inventory Item" Checked="false"
                                                                                                        Font-Bold="true"></asp:CheckBox>
                                                                                                </div>
                                                                                                <div class="col-md-6">
                                                                                                    <asp:CheckBox ID="chkIsRecipe" runat="server" Text=" Is Recipe" Checked="true"
                                                                                                        Font-Bold="true" />
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="row">
                                                                                                <div class="col-md-6">
                                                                                                    <asp:CheckBox ID="chkIsOverSaleAllowed" runat="server" Text="OverSale Allowed"
                                                                                                        Checked="false" Font-Bold="true" />
                                                                                                </div>
                                                                                                <div class="col-md-6">
                                                                                                    <asp:CheckBox ID="chkIsHasModifier" runat="server" Text=" Is has Modifier"
                                                                                                        Checked="false" Font-Bold="true" />
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="row">
                                                                                                <div class="col-md-6">
                                                                                                    <asp:CheckBox ID="chkIsInventoryWeight" runat="server" Text="Is Inventory By Weight" Checked="false" Font-Bold="true"></asp:CheckBox>
                                                                                                </div>
                                                                                                <div class="col-md-6">
                                                                                                    <asp:CheckBox ID="chkIsSaleWeight" runat="server" Text="Is Sale By Weight" Checked="false" Font-Bold="true"></asp:CheckBox>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="row">
                                                                                                <div class="col-md-6">
                                                                                                    <asp:CheckBox ID="chkIsDeal" runat="server" Text=" Is Deal Item" Checked="false" Font-Bold="true"></asp:CheckBox>
                                                                                                </div>
                                                                                                <div class="col-md-6">
                                                                                                    <asp:CheckBox ID="chkIsModifier" runat="server" Text=" Is Modifier" Checked="false" Font-Bold="true" onchange="IsModifierChange();"></asp:CheckBox>
                                                                                                </div>

                                                                                            </div>
                                                                                            <div class="row">
                                                                                                <div class="col-md-6">
                                                                                                    <asp:CheckBox ID="chkIsGroup" runat="server" Text="UnGroup Item on POS" Checked="false" Font-Bold="true"></asp:CheckBox>
                                                                                                </div>
                                                                                                <div class="col-md-6">
                                                                                                    <asp:CheckBox ID="chIsPackage" Style="display: none" runat="server" Text="Is Package Item" Checked="false" Font-Bold="true"></asp:CheckBox>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="row" style="display: none">
                                                                                                <div class="col-md-12" id="divColorPicker" runat="server" style="visibility: hidden;">
                                                                                                    <dx:ASPxColorEdit ID="ceModifier" runat="server" Color="#e2e3e8"></dx:ASPxColorEdit>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-md-4">
                                                                                            <div id="skuImageUploadArea" runat="server" visible="true">
                                                                                                <label>
                                                                                                    <span class="fa fa-caret-right rgt_cart"></span>Upload Image
                                                                                                        &nbsp;(<%=allowed_extensions%>)
                                                                                                </label>
                                                                                                <label for="<%=fuPic.ClientID %>" style="cursor: pointer;">
                                                                                                    <img src="../images/no_image.gif" id="pic" alt="No Image" name="pic" width="266" height="130" />
                                                                                                </label>
                                                                                                <asp:FileUpload ID="fuPic" onchange="readImageURL(this);" Style="display: none;" runat="server"></asp:FileUpload>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-md-2" id="cloudDiv" runat="server">
                                                                                            <asp:HiddenField ID="txtAccessKeyID" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="txtSecretAccessKey" runat="server" Value="" />
                                                                                            <asp:HiddenField ID="txtBucketName" runat="server" Value="" />
                                                                                            <label>
                                                                                                <span class="fa fa-caret-right rgt_cart"></span>Upload Image on Cloud?
                                                                                            </label>
                                                                                            <label class="switch">
                                                                                                <asp:CheckBox ID="chkUploadOnCloud" runat="server" />
                                                                                                <span class="slider round"></span>
                                                                                            </label>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </ajaxToolkit:TabPanel>
                                                            <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" Height="100%">
                                                                <HeaderTemplate>
                                                                    Detail
                                                                </HeaderTemplate>
                                                                <ContentTemplate>
                                                                    <div class="main-contents" style="overflow-y: auto; height: 100%;">
                                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                            <ContentTemplate>
                                                                                <div class="modal-body" ng-app="App" ng-controller="CtrlValidation">
                                                                                    <fieldset style="border-width: thin; border-color: #eae5e5">
                                                                                        <legend>Stock</legend>
                                                                                        <div class="row">
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Unit</label>
                                                                                                <dx:ASPxComboBox ID="drpStockUnit" AutoPostBack="true" OnSelectedIndexChanged="drpStockUnit_SelectedIndexChanged" runat="server" CssClass="form-control" ng-model="StockUnit">
                                                                                                </dx:ASPxComboBox>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Sale Opr</label>
                                                                                                <dx:ASPxComboBox ID="drpStockToSaleOperator" runat="server" CssClass="form-control">
                                                                                                </dx:ASPxComboBox>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Sale Factor</label>
                                                                                                <asp:TextBox ID="txtStockToSaleFactor" Text="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtStockToSaleFactor"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Purchase Opr</label>
                                                                                                <dx:ASPxComboBox ID="drpStockToPurchaseOperator" runat="server" CssClass="form-control">
                                                                                                </dx:ASPxComboBox>
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Purchase Factor</label>
                                                                                                <asp:TextBox ID="txtStockToPurchaseFactor" Text="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtStockToPurchaseFactor"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                    <fieldset style="border-width: thin; border-color: #eae5e5; margin-top: 5px">
                                                                                        <legend>Sale</legend>
                                                                                        <div class="row">
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Unit</label>
                                                                                                <dx:ASPxComboBox ID="drpSaleUnit" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpSaleUnit_SelectedIndexChanged" ng-model="SaleUnit" CssClass="form-control">
                                                                                                </dx:ASPxComboBox>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Purchase Opr</label>
                                                                                                <dx:ASPxComboBox ID="drpSaletoPurchaseOperator" runat="server" CssClass="form-control">
                                                                                                </dx:ASPxComboBox>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Purch. Factor</label>
                                                                                                <asp:TextBox ID="txtSaleToPurchaseFactor" Text="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtSaleToPurchaseFactor"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Stock Opr</label>
                                                                                                <dx:ASPxComboBox ID="drpSaleToStockOperator" runat="server" CssClass="form-control">
                                                                                                </dx:ASPxComboBox>
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Stock Factor</label>
                                                                                                <asp:TextBox ID="txtSaleToStockFactor" Text="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtSaleToStockFactor"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                    <fieldset style="border-width: thin; border-color: #eae5e5; margin-top: 5px">
                                                                                        <legend>Purchase</legend>
                                                                                        <div class="row">
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Unit</label>
                                                                                                <dx:ASPxComboBox ID="drpPurchaseUnit" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpPurchaseUnit_SelectedIndexChanged" ng-model="PurchaseUnit" CssClass="form-control">
                                                                                                </dx:ASPxComboBox>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Sale Opr</label>
                                                                                                <dx:ASPxComboBox ID="drpPurchaseToSaleOperator" runat="server" CssClass="form-control">
                                                                                                </dx:ASPxComboBox>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Sale Factor</label>
                                                                                                <asp:TextBox ID="txtPurchaseToSaleFactor" Text="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtPurchaseToSaleFactor"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Stock Opr</label>
                                                                                                <dx:ASPxComboBox ID="drpPurchaseToStockOperator" runat="server" CssClass="form-control">
                                                                                                </dx:ASPxComboBox>
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Stock Factor</label>
                                                                                                <asp:TextBox ID="txtPurchaseToStockFactor" Text="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtPurchaseToStockFactor"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                    <fieldset style="border-width: thin; border-color: #eae5e5; margin-top: 5px">
                                                                                        <legend>Other</legend>
                                                                                        <div class="row">
                                                                                            <div class="col-md-4" style="text-align: right">
                                                                                                <label style="float: left"><span class="fa fa-caret-right rgt_cart"></span>Serial Code</label>
                                                                                                <asp:CheckBox ID="chkIsSerialized" onclick="ChkSerialization(this)" Text="Serialized" runat="server" Checked="false" Font-Bold="true" />
                                                                                                <asp:TextBox ID="txtSerialCode" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            </div>

                                                                                            <div class="col-md-4" style="text-align: right">
                                                                                                <label style="float: left"><span class="fa fa-caret-right rgt_cart"></span>FED %age</label>
                                                                                                <asp:CheckBox ID="chkIsFEDItem" onclick="ChkSerialization(this)" runat="server" Text="FED item" Checked="false" Font-Bold="true" />
                                                                                                <asp:TextBox ID="txtFEDPercentage" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtFEDPercentage"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                            <div class="col-md-4" style="text-align: right">
                                                                                                <label style="float: left"><span class="fa fa-caret-right rgt_cart"></span>WHT %age</label>
                                                                                                <asp:CheckBox ID="chkIsWHTItem" runat="server" onclick="ChkSerialization(this)" Text="WHT Item" Checked="false" Font-Bold="true" />
                                                                                                <asp:TextBox ID="txtWHTPercentage" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtWHTPercentage"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row">
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Bar Code</label>
                                                                                                <asp:TextBox ID="txtERPCode" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Unit Life Code</label>
                                                                                                <dx:ASPxComboBox ID="drpUnitLifeCode" runat="server" CssClass="form-control">
                                                                                                </dx:ASPxComboBox>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Full Age</label>
                                                                                                <asp:TextBox ID="txtAgeInDays" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtAgeInDays"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Shelf Age</label>
                                                                                                <asp:TextBox ID="txtShelfAgeInDays" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtShelfAgeInDays"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                            <div class="col-md-2">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Status</label>
                                                                                                <dx:ASPxComboBox ID="drpStatus" runat="server" CssClass="form-control">
                                                                                                    <Items>
                                                                                                        <dx:ListEditItem Text="Active" Value="Active" Selected="True"></dx:ListEditItem>
                                                                                                        <dx:ListEditItem Text="Inactive" Value="In - Active"></dx:ListEditItem>
                                                                                                        <dx:ListEditItem Text="Out of Service" Value="Out of Service"></dx:ListEditItem>
                                                                                                        <dx:ListEditItem Text="Sold" Value="Sold"></dx:ListEditItem>
                                                                                                        <dx:ListEditItem Text="Transferred" Value="Transferred"></dx:ListEditItem>
                                                                                                    </Items>
                                                                                                </dx:ASPxComboBox>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row">
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Hazardous</label>
                                                                                                <asp:CheckBox ID="chkIsHazardous" runat="server" Checked="false" Font-Bold="true" />
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Batch Item</label>
                                                                                                <asp:CheckBox ID="chkIsBatchItem" runat="server" Checked="false" Font-Bold="true" />
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <asp:HiddenField ID="_ExpiryAllowed" runat="server" Value="False" />
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Expiry Allowed</label>
                                                                                                <asp:CheckBox ID="chkIsExpiryAllowed" runat="server" Checked="false" Font-Bold="true" />
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Warehouse Item</label>
                                                                                                <asp:CheckBox ID="chkIsWarehouseItem" runat="server" Checked="false" Font-Bold="true" />
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Market Item</label>
                                                                                                <asp:CheckBox ID="chkIsMarketItem" runat="server" Checked="false" Font-Bold="true" />
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Replaceable</label>
                                                                                                <asp:CheckBox ID="chkIsReplaceable" runat="server" Checked="false" Font-Bold="true" />
                                                                                            </div>
                                                                                        </div>

                                                                                        <div class="row">
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Sort Order</label>
                                                                                                <asp:TextBox ID="txtSortOrder" Text="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtSortOrder"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Preparation Time</label>
                                                                                                <asp:TextBox ID="txtPrepartionTime" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtPrepartionTime"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Unit In Case</label>
                                                                                                <asp:TextBox ID="txtunitincase" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789" TargetControlID="txtunitincase"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <label><span class="fa fa-caret-right rgt_cart"></span>Pack Size</label>
                                                                                                <asp:TextBox ID="txtPackSize" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server"
                                                                                                    FilterType="Custom" ValidChars="0123456789." TargetControlID="txtPackSize"></ajaxToolkit:FilteredTextBoxExtender>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="row">
                                                                                            <div class="col-md-3">
                                                                                                <asp:CheckBox ID="cbValidateStockSplitItem" runat="server" Text="Validate Stck On Split Item" Checked="true" Font-Bold="true"></asp:CheckBox>
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <%--<label><span class="fa fa-caret-right rgt_cart"></span>Is Sticker Print</label>--%>
                                                                                                <asp:CheckBox ID="chkIsStickerPrint" Text=" Is Sticker Print" runat="server" Checked="false" Font-Bold="true" />
                                                                                            </div>
                                                                                            <div class="col-md-3">
                                                                                                <asp:CheckBox ID="chkRunOut" Text="Run-Out" runat="server" Checked="false" Font-Bold="true"/>
                                                                                            </div>
                                                                                        </div>
                                                                                    </fieldset>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </ajaxToolkit:TabPanel>

                                                        </ajaxToolkit:TabContainer>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-11" style="text-align: right; margin-top: 20px; margin-right: 40px">
                                                            <asp:HiddenField ID="hfStatus" runat="server" Value="Active" />
                                                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn btn-success" />
                                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif"></asp:ImageButton>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div class="row center">
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>

                                <asp:GridView ID="grdSKUData" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                    HorizontalAlign="Center"
                                    AutoGenerateColumns="False" OnPageIndexChanging="grdSKUData_PageIndexChanging" OnRowEditing="grdSKUData_RowEditing"
                                    AllowPaging="true" PageSize="20" EmptyDataText="No Record exist">

                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle />
                                            <HeaderTemplate>
                                                &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server" onclick="Check_Click(this)" />
                                                <asp:HiddenField ID="hidSKUImageName" runat="server" Value='<%# Eval("SKU_IMAGE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Principal_Id" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Division_Id" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Category_Id" HeaderText="Category_Id" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Brand_Id" HeaderText="Brand_Id" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SKU_ID" HeaderText="SKU_ID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Principal" HeaderText="Supplier" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Division" HeaderText="Division" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Category" HeaderText="Category" ReadOnly="true">
                                            <ItemStyle Width="20%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Brand" HeaderText="Brand" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SKU_CODE" HeaderText="Code" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="SKU_NAME" HeaderText="Name" ReadOnly="true">
                                            <ItemStyle Width="50%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UOM" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UNITS_IN_CASE" HeaderText="UIC" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="GST_ON" HeaderText="GST" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SECTION_ID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IS_DESC" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ISEXEMPTED" ReadOnly="true" HeaderText="Is Inventory"></asp:BoundField>
                                        <asp:BoundField DataField="ISACTIVE" HeaderText="Status" ReadOnly="true">
                                            <ItemStyle Width="8%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IS_DEAL" ReadOnly="true" HeaderText="Is Deal"></asp:BoundField>

                                        <asp:BoundField DataField="IS_MODIFIER" ReadOnly="true" HeaderText="Is Modifier"></asp:BoundField>
                                        <asp:BoundField DataField="MIN_LEVEL" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REORDER_LEVEL" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IS_Recipe" ReadOnly="true" HeaderText="Is Recipe"></asp:BoundField>
                                        <asp:BoundField DataField="IS_HasMODIFIER" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MAX_LEVEL" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="intSaleMUnitCode" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Sale_to_PurchaseOperator" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Sale_to_PurchaseFactor" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Purchase_to_SaleOperator" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Purchase_to_SaleFactor" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="intPurchaseMUnitCode" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Sale_to_StockOperator" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Sale_to_StockFactor" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Purchase_to_StockFactor" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="intStockMUnitCode" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Default_Qty" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Stock_to_SaleOperator" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Stock_to_SaleFactor" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Stock_to_PurchaseOperator" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Stock_to_PurchaseFactor" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="SECTION_NAME" HeaderText="Section" ReadOnly="true">
                                            <ItemStyle Width="10%"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsSerialized" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="strSerialCode" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsHazardous" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="strStatus" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsBatchItem" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="strERPCode" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsOverSaleAllowed" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsExpiryAllowed" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="IsWarehouseItem" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsMarketItem" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsReplaceable" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsFEDItem" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="fltFEDPercentage" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsWHTItem" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="fltWHTPercentage" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="fltAgeInDays" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="fltShelfAgeInDays" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="intMUnitLifeCode" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Purchase_to_StockOperator" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="STOCK_REGISTER_STATUS" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BILL_OF_MATERIAL_STATUS" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsInventoryWeight" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DescOnKOT" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BUTTON_COLOR" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsSaleWeight" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsUnGroup" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsPackage" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="strDescription" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:BoundField DataField="SortOrder" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PreparationTime" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PackSize" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ValidateStockOnSplitItem" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Is_StickerPrint" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="strKOTDescription" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsRunOut" ReadOnly="true">
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            <HeaderStyle CssClass="HidePanel" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="fa fa-pencil" CommandName="Edit" ToolTip="Edit">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerSettings PageButtonCount="10" />
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