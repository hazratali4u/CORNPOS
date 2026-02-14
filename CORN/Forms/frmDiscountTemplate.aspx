<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmDiscountTemplate.aspx.cs" Inherits="Form_frmDiscountTemplate " Title="CORN :: Discount Template" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script src="../AjaxLibrary/1.8.3jquery.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../js/angular.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
    <script language="JavaScript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        function ValidateForm() {
            var str;
            str = document.getElementById('<%=txtDiscountName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Discount Name is required');
                return false;
            }

            str = document.getElementById('<%=txtDiscount.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Discount value is required');
                return false;
            }
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
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
    </script>
    <div class="main-contents">
        <div class="container">
            <div class="row">
                <asp:Panel ID="Panel4" runat="server" DefaultButton="btnsearch">
                    <div class="col-md-4" runat="server" id="searchBox">
                        <div class="search">
                            <asp:TextBox ID="txtSearch" runat="server" placeholder="Search" CssClass="form-control"
                                TabIndex="0"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-1" runat="server" id="searchBtn" style="margin-left: -60px;">
                        <asp:LinkButton ID="btnsearch" OnClick="btnFilter_Click" runat="server" Text="Search"
                            CssClass="btn btn-success"><i class="fa fa-search"  style="font-size:20px;"></i></asp:LinkButton>
                        <asp:LinkButton ID="btndummy" runat="server" UseSubmitBehavior="false" />
                    </div>


                    <div class="col-md-6" style="right: 5px; float: right;">
                        <div class="btnlist pull-right">
                            <asp:LinkButton CssClass="btn btn-warning" runat="server" ID="btnAdd" Text="Add"
                                OnClick="btnAdd_Click">
                               <span class="fa fa-plus-circle"></span>Add
                            </asp:LinkButton>

                            <asp:LinkButton class="btn btn-success" OnClick="btnActive_Click" ID="btnActive" runat="server"
                                OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="row top" runat="server" id="contentBox" style="margin: 0 0 0 0;">

                <div class="row">
                    <div class="col-md-12">
                        <div class="main-contents" style="height: 100%;">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label id="lbldesignationID"><span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                                    <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control">
                                                    </dx:ASPxComboBox>
                                                    <asp:HiddenField ID="hfBankDiscount" runat="server" Value="0"/>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Discount Name</label>
                                                    <asp:TextBox ID="txtDiscountName" runat="server" CssClass="form-control "></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <label id="lblType"><span class="fa fa-caret-right rgt_cart"></span>Discount Type</label>
                                                    <dx:ASPxComboBox ID="ddlDiscountType" runat="server" CssClass="form-control">
                                                        <Items>
                                                            <dx:ListEditItem Value="1" Text="%" Selected="true"/>
                                                            <dx:ListEditItem Value="2" Text="Value" />
                                                        </Items>
                                                    </dx:ASPxComboBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <label>
                                                        <span class="fa fa-caret-right rgt_cart"></span>Discount Value</label>
                                                    <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom"
                                                        TargetControlID="txtDiscount" ValidChars="1234567890."></cc1:FilteredTextBoxExtender>
                                                </div>                                                
                                            </div>                                            
                                            </div>                                            
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-10"></div>
                                <div class="col-md-2" style="padding: 15px 25px 0 15px">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn btn-success" CausesValidation="true" ValidationGroup="emailvalidate" />
                                    <asp:LinkButton CssClass="btn btn-danger" runat="server" ID="btnClose" Text="Cancel"
                                        OnClick="btnClose_Click">
                               <span class="fa fa-close"></span>Cancel
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div style="z-index: 101; left: 50%; width: 100px; position: absolute; top: 10px; height: 90px">
                &nbsp;
                        <asp:Panel ID="Panel21" runat="server">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                <ProgressTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </asp:Panel>
            </div>

            <div class="row center" runat="server" id="lookupBox">
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:GridView ID="GrdCustomer" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                            AllowPaging="true" AutoGenerateColumns="False" OnRowEditing="GrdCustomer_RowEditing" OnPageIndexChanging="grdData_PageIndexChanging"
                            EmptyDataText="No Record exist"
                            PageSize="8">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        &nbsp; &nbsp; &nbsp;
                                        <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="EmployeeDiscountTypeID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DistributorID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DiscountTypeName" HeaderText="Discount Name" ReadOnly="true">
                                    <ItemStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location" ReadOnly="true">
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DiscountValue" HeaderText="Discount" ReadOnly="true">
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ActiveInActive" HeaderText="Status" ReadOnly="true">
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" CommandArgument='<%# Eval("EmployeeDiscountTypeID" )%>' ToolTip="Edit">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="cf head"></HeaderStyle>
                            <PagerSettings PageButtonCount="10" NextPageText=">" PreviousPageText="<" />
                            <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>