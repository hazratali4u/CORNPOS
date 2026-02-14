<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmPromotionalVoucher.aspx.cs" Inherits="frmPromotionalVoucher" Title="CORN :: Promotional Voucher" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script src="../AjaxLibrary/1.8.3jquery.min.js" type="text/javascript"></script>
    <script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="../js/angular.min.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../AjaxLibrary/ValidateDotsAndNumbers.js"></script>
    <script language="JavaScript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
        function ValidateForm() {
            var str;
            <%--str = document.getElementById('<%=txtCode.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Code is required');
                return false;
            }--%>
            str = document.getElementById('<%=txtPrefix.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Prefix is required');
                return false;
            }
            str = document.getElementById('<%=txtCheckfrom.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Serial From is required');
                return false;
            }
            str = document.getElementById('<%=txtcheckto.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Serial To is required');
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
                    <div class="col-md-4" runat="server" ID="searchBox">
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
               

                <div class="col-md-6" style="right:5px;float:right;">
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
                                                <div class="col-md-8">
                                                    <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label><br />
                                                </div>
                                                <asp:HiddenField ID="hfCustomerID" runat="server" />
                                            </div>

                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label id="lbldesignationID"><span class="fa fa-caret-right rgt_cart"></span>Location</label>

                                                    <dx:ASPxComboBox ID="drpDistributor" runat="server" CssClass="form-control"
                                                        AutoPostBack="True" OnSelectedIndexChanged="drpDistributor_SelectedIndexChanged">
                                                    </dx:ASPxComboBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Customer</label>
                                                    <dx:ASPxComboBox ID="ddlCustomer" runat="server" CssClass="form-control">
                                                    </dx:ASPxComboBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Voucher Code</label>
                                                    <asp:TextBox ID="txtVoucherCode" runat="server" CssClass="form-control "></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Voucher Name</label>
                                                    <asp:TextBox ID="txtVoucherName" runat="server" CssClass="form-control "></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-2">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Expiry Date</label>
                                                    <asp:TextBox ID="txtExpiry" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server"
                                                        Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate" PopupPosition="TopLeft" TargetControlID="txtExpiry"></cc1:CalendarExtender>
                                                </div>
                                                <div class="col-md-1" style="margin-top: 27px;">
                                                    <asp:ImageButton ID="ibnEndDate" Style="margin-left: -22px;" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                        Width="30px" />
                                                </div>
                                                <div class="col-md-3">
                                                    <label>
                                                        <span class="fa fa-caret-right rgt_cart"></span>Prefix</label>
                                                    <asp:TextBox ID="txtPrefix" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom"
                                                        TargetControlID="txtPrefix" ValidChars="QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklxcvbnm"></cc1:FilteredTextBoxExtender>
                                                </div>
                                                <div class="col-md-3">
                                                    <label>
                                                        <span class="fa fa-caret-right rgt_cart"></span>Serial From</label>
                                                    <asp:TextBox ID="txtCheckfrom" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredtxtFaxNumber" runat="server" FilterType="Custom"
                                                        TargetControlID="txtCheckfrom" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                                </div>
                                                <div class="col-md-3">
                                                    <label>
                                                        <span class="fa fa-caret-right rgt_cart"></span>Serial To</label>
                                                    <asp:TextBox ID="txtcheckto" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Custom"
                                                        TargetControlID="txtcheckto" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <label>
                                                        <span class="fa fa-caret-right rgt_cart"></span>Remarks</label>
                                                    <asp:TextBox ID="txtremarks" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-10"></div>
                                <div class="col-md-2" style="padding:15px 25px 0 15px">
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
                                       &nbsp; &nbsp; &nbsp; <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%"/>
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="PROMOTION_VOUCHER_ID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DISTRIBUTOR_ID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_ID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location" ReadOnly="true">
                                    <ItemStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer" ReadOnly="true">
                                    <ItemStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="VOUCHER_CODE" HeaderText="Voucher Code" ReadOnly="true">
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="VOUCHER_NAME" HeaderText="Voucher Name" ReadOnly="true">
                                    <HeaderStyle Width="30%"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="EXPIRY_DATE" HeaderText="Expiry Date" ReadOnly="true">
                                    <ItemStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SERIAL_FROM" HeaderText="" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SERIAL_TO" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PREFIX" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="REMARKS" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" CommandArgument='<%# Eval("CUSTOMER_ID" )%>' ToolTip="Edit">
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
                <%--<Triggers>
                                <asp:PostBackTrigger ControlID="btnImport" />
                            </Triggers>--%>
            </div>
        </div>
    </div>
</asp:Content>
