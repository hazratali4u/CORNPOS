<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmCustomer.aspx.cs" Inherits="frmCustomer" Title="CORN :: Add Customer" %>

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
            str = document.getElementById('<%=txtName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Customer name is required');
                return false;
            }
            str = document.getElementById('<%=txtContact.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Contact no. is required');
                return false;
            }
            str = document.getElementById('<%=txtAddress.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Address is required');
                return false;
            }
        }
        function ValidateFormFamily() {
            var str;
            str = document.getElementById('<%=txtChildName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Child name is required');
                return false;
            }
            str = document.getElementById('<%=txtChildDOB.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Child Date Of Birth is required');
                return false;
            }
            str = document.getElementById('<%=txtName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Customer Name is required');
                return false;
            }

            return true;
        }
        function ValidateFormAddress() {
            var str;
            str = document.getElementById('<%=txtAddress1.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Address is required');
                return false;
            }
            str = document.getElementById('<%=txtName.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Customer Name is required');
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
                        <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn btn-success" CausesValidation="true" ValidationGroup="emailvalidate" />

                        <asp:LinkButton CssClass="btn btn-danger" runat="server" ID="btnClose" Text="Add"
                            OnClick="btnClose_Click">
                               <span class="fa fa-close"></span>Close
                        </asp:LinkButton>
                        <asp:LinkButton class="btn btn-success" OnClick="btnActive_Click" ID="btnActive" runat="server"
                            OnClientClick="javasacript:return confirm('Are you sure you want to perform this action?'); return false;">
                                    <span class="fa fa-check"></span>Active</asp:LinkButton>
                    </div>
                </div>
             </asp:Panel>
            </div>

            <div class="row top" runat="server" id="contentBox" style="margin-left: 1px; margin-right: 5px;">

                <div class="row">
                    <div class="col-md-12">
                        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0"
                            Height="100%" Width="100%">
                            <ajaxToolkit:TabPanel ID="TabPanelGeneral" runat="server" Height="100%">
                                <HeaderTemplate>
                                    General
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="main-contents" style="height: 100%;">
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-md-8">
                                                                    <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label><br />
                                                                </div>
                                                                <asp:HiddenField ID="hfStatus" runat="server" />
                                                                <asp:HiddenField ID="hfCustomerID" runat="server" />
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-4" id="rowLocation" runat="server" visible="false">
                                                                    <label id="lbldesignationID"><span class="fa fa-caret-right rgt_cart"></span>Base Location</label>

                                                                    <dx:ASPxComboBox ID="ddDistributorId" runat="server" CssClass="form-control"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="ddDistributorId_SelectedIndexChanged">
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Customer Group</label>
                                                                    <dx:ASPxComboBox ID="ddlCustomerGroup" runat="server" CssClass="form-control">
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Name</label>
                                                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control "></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-4">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Membership No</label>
                                                                    <asp:TextBox ID="txtMembershipNo" runat="server" CssClass="form-control "></asp:TextBox>
                                                                </div>

                                                                <div class="col-md-4">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Primary Contact</label>
                                                                    <asp:TextBox ID="txtContact" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftetxtPhoneNumber" runat="server" FilterType="Custom"
                                                                        TargetControlID="txtContact" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Secondary Contact</label>
                                                                    <asp:TextBox ID="txtContact2" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredtxtFaxNumber" runat="server" FilterType="Custom"
                                                                        TargetControlID="txtContact2" ValidChars="0123456789"></cc1:FilteredTextBoxExtender>
                                                                </div>
                                                               
                                                            </div>
                                                            <div class="row">
                                                                 <div class="col-md-3">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Email</label>
                                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control "></asp:TextBox>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                                        ErrorMessage="Invalid Email" ControlToValidate="txtEmail" ValidationGroup="emailvalidate"
                                                                        ValidationExpression="^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$"
                                                                        Display="Dynamic"></asp:RegularExpressionValidator>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>CNIC</label>
                                                                    <asp:TextBox ID="txtCNIC" runat="server" CssClass="form-control "></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Custom"
                                                                        TargetControlID="txtCNIC" ValidChars="0123456789+-"></cc1:FilteredTextBoxExtender>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Address</label>
                                                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control "></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row">

                                                                <div class="col-md-3" style="display: none;">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Nature</label>
                                                                    <asp:TextBox ID="txtNature" runat="server" CssClass="form-control "></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Date of Birth</label>
                                                                    <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control "></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender2" OnClientShown="calendarShown" runat="server"
                                                                        Format="dd-MMM-yyyy" PopupButtonID="ImgBntFromCalc" PopupPosition="TopLeft" TargetControlID="txtDOB"></cc1:CalendarExtender>
                                                                </div>
                                                                <div class="col-md-1" style="margin-top: 25px;">
                                                                    <asp:ImageButton ID="ibtnDOB" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                                        Width="30px" />
                                                                    </div>
                                                                 <cc1:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnDOB"
                                                                    TargetControlID="txtDOB" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>
                                                                <div class="col-md-2">
                                                                    <label style="font-size:13px;"><span class="fa fa-caret-right rgt_cart"></span>Date of Membership</label>
                                                                    <asp:TextBox ID="dtpMembership" runat="server" CssClass="form-control "></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtender1" OnClientShown="calendarShown" runat="server"
                                                                        Format="dd-MMM-yyyy" PopupButtonID="ImgBntFromCalc" PopupPosition="TopLeft" TargetControlID="dtpMembership"></cc1:CalendarExtender>
                                                                </div>
                                                                    <div class="col-md-1" style="margin-top: 25px;">
                                                                    <asp:ImageButton ID="ibtnMembershipDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                                        Width="30px" />
                                                                    </div>
                                                                 <cc1:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnMembershipDate"
                                                                    TargetControlID="dtpMembership" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>
                                                                <div class="col-md-3">
                                                                    <asp:Literal ID="Literal1" runat="server"><label><span class="fa fa-caret-right rgt_cart"></span>Spouse Name</label></asp:Literal>
                                                                    <asp:TextBox ID="txtSpouse" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Profession</label>
                                                                    <dx:ASPxComboBox ID="ddlProfession" runat="server" CssClass="form-control">
                                                                        <Items>
                                                                        </Items>
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-3">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Category</label>
                                                                    <dx:ASPxComboBox ID="drpCategory" runat="server" CssClass="form-control">
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Status</label>
                                                                    <dx:ASPxComboBox ID="drpStatus" runat="server" CssClass="form-control" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="drpStatus_SelectedIndexChanged">
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span></label>
                                                                   <dx:ASPxRadioButtonList runat="server" ID="rdoGolfer" Width="100%" 
                                                                       RepeatDirection="Horizontal" Font-Size="12pt" Border-BorderStyle="Dotted">
                                                                       <Items>
                                                                           <dx:ListEditItem Selected ="true" Value="False" Text="Non-Golfer" />
                                                                           <dx:ListEditItem Value="True" Text="Golfer" />
                                                                       </Items>
                                                                   </dx:ASPxRadioButtonList>
                                                                </div>
                                                            </div>
                                                            <div class="row" runat="server" id="dormant_row" visible="false">
                                                                <div class="col-md-2">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>From Date</label>
                                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control "></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-1" style="margin-top: 25px;">
                                                                    <asp:ImageButton ID="ibtnStart" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                                        Width="30px" />
                                                                </div>
                                                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStart"
                                                                    TargetControlID="txtFromDate" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>

                                                                <div class="col-md-2">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>To Date</label>
                                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control "></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-1" style="margin-top: 25px;">
                                                                    <asp:ImageButton ID="ibtnEnd" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                                        Width="30px" />
                                                                </div>
                                                                <cc1:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnEnd"
                                                                    TargetControlID="txtToDate" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-3">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Opening Points</label>
                                                                    <asp:TextBox ID="txtOpeningAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Custom"
                                                                        TargetControlID="txtOpeningAmount" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Sales %</label>
                                                                    <asp:TextBox ID="txtSalesPer" runat="server" CssClass="form-control "></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom"
                                                                        TargetControlID="txtSalesPer" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Card Type</label>
                                                                    <dx:ASPxComboBox ID="drpCardType" runat="server" CssClass="form-control"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="drpCardType_SelectedIndexChanged">
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:HiddenField ID="hfLoyaltyCardType" Value ="0" runat="server" />
                                                                    <asp:Literal ID="ltrlCardNo" runat="server"><label><span class="fa fa-caret-right rgt_cart"></span>Card No</label></asp:Literal>
                                                                    <asp:TextBox ID="txtCardNo" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-3" style="display: none;">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Card ID</label>
                                                                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:Literal ID="ltrlCard" runat="server"><label><span class="fa fa-caret-right rgt_cart"></span>Discount (%)</label></asp:Literal>
                                                                    <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control" Text="0"
                                                                        onkeypress="return onlyDotsAndNumbers(txt, event);"></asp:TextBox>
                                                                    <asp:TextBox ID="txtPurchasing" runat="server" CssClass="form-control" Text="0" Visible="false" Enabled="false"
                                                                        onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>

                                                                </div>
                                                                <div class="col-md-3" style="display: none;">
                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Amount Limit</label>
                                                                    <asp:TextBox ID="txtAmountLimit" runat="server" CssClass="form-control"
                                                                        onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>

                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:Literal ID="ltrlPoints" runat="server"><label><span class="fa fa-caret-right rgt_cart"></span>Points</label></asp:Literal>
                                                                    <asp:TextBox ID="txtPoints" runat="server" CssClass="form-control" Visible="false" Enabled="false"
                                                                        onkeypress="return onlyDotsAndNumbers(this,event);"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <strong>Import Customer</strong>
                                                            <asp:FileUpload ID="FupVendor" runat="server" Width="250px" />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:Button ID="btnExportOpeningTemplate" OnClick="btnExportOpeningTemplate_Click" runat="server" Text="Download Template" CssClass="btn btn-primary" />
                                                            <asp:Button ID="btnImport" OnClick="btnImport_Click" runat="server" Style="margin-right: 5px" Text="Import" CssClass="btn btn-danger" CausesValidation="false" ValidationGroup="emailvalidate" />
                                                            <%--<asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false" ValidationGroup="emailvalidate" />--%>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanelFamily" runat="server" Height="400px">
                                <HeaderTemplate>
                                    Family Detail
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="main-contents" style="overflow-y: scroll; height: 380px;">
                                        <div class="modal-body">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Child Name</label>
                                                            <asp:TextBox ID="txtChildName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Date of Birth</label>
                                                            <asp:TextBox ID="txtChildDOB" runat="server" CssClass="form-control "></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-1" style="margin-top: 27px;margin-left: -25px;">
                                                            <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                                Width="30px" />
                                                        </div>
                                                        <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                                                            TargetControlID="txtChildDOB" PopupPosition="TopLeft" OnClientShown="calendarShown"></cc1:CalendarExtender>
                                                    
                                                        <div class="col-md-3">
                                                             <label><span class="fa fa-caret-right rgt_cart"></span>Gender</label>
                                                            <dx:ASPxComboBox ID="drpGender" runat="server" CssClass="form-control">
                                                                <Items>
                                                                    <dx:ListEditItem Text="Male" Selected="true" Value="1" />
                                                                    <dx:ListEditItem Text="Female" Value="2" />
                                                                </Items>
                                                            </dx:ASPxComboBox>
                                                        </div>
                                                        <div class="col-md-1" style="margin-top: 22px;">
                                                            <asp:Button ID="btnFamilyAdd" runat="server" Text="Add" CssClass="btn btn-success"
                                                                OnClick="btnAddFamily_Click" OnClientClick="return ValidateFormFamily();" />
                                                        </div>
                                                        <asp:HiddenField ID="RowId" runat="server" Value="0" />
                                                    </div>

                                                    <div class="row">
                                                        <asp:Panel ID="pnlSKU" runat="server" Width="98%" Height="180px"
                                                            ScrollBars="Vertical" BorderColor="Silver" BorderStyle="Groove"
                                                            BorderWidth="1px">
                                                            <asp:GridView Width="100%" ID="gvSKU" runat="server" Class="table table-striped table-bordered table-hover table-condensed cf"
                                                                AutoGenerateColumns="False" OnRowDeleting="gvSKU_RowDeleting" OnRowEditing="gvSKU_RowEditing"
                                                                ShowHeader="true">

                                                                <Columns>
                                                                    <asp:BoundField DataField="CUSTOMER_ID" HeaderText="CUSTOMER_ID" ReadOnly="true">
                                                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                        <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer" ReadOnly="true">
                                                                        <ItemStyle Width="30%"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CHILD_NAME" HeaderText="Child Name" ReadOnly="true">
                                                                        <ItemStyle Width="30%"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CHILD_DOB" HeaderText="Date Of Birth" ReadOnly="true">
                                                                        <ItemStyle Width="20%"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CHILD_GENDER" HeaderText="Gender" ReadOnly="true">
                                                                        <ItemStyle Width="15%"></ItemStyle>
                                                                    </asp:BoundField>

                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="fa fa-pencil" CommandName="Edit" ToolTip="Edit">
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />

                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="glyphicon glyphicon-trash"
                                                                                OnClientClick="javascript:return confirm('Are you sure you want to delete?');return false;"
                                                                                ToolTip="Delete">
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </div>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanelInvoiceCalculation" runat="server" Height="400px" Visible="false">
                                <HeaderTemplate>
                                    Invoice Calculation
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="main-contents" style="overflow-y: scroll; height: 380px;">
                                        <div class="modal-body">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div class="col-md-2">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>GST</label>
                                                            <asp:TextBox ID="txtGST" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Custom"
                                                                TargetControlID="txtGST" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>GST Type</label>
                                                            <asp:RadioButtonList ID="rblGST" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="0" Text="%" Selected="True"></asp:ListItem>
                                                                <%--<asp:ListItem Value="1" Text="Value"></asp:ListItem>--%>                                                                
                                                            </asp:RadioButtonList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Discount</label>
                                                            <asp:TextBox ID="txtDiscountPOS" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Custom"
                                                                TargetControlID="txtDiscountPOS" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Discount Type</label>
                                                            <asp:RadioButtonList ID="rblDiscount" runat="server" RepeatDirection="Horizontal">                                                                
                                                                <asp:ListItem Value="0" Text="%" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Value"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Service Charges</label>
                                                            <asp:TextBox ID="txtServiceCharges" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Custom"
                                                                TargetControlID="txtServiceCharges" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Ser. Charges Type</label>
                                                            <asp:RadioButtonList ID="rblServiceCharges" runat="server" RepeatDirection="Horizontal">                                                                
                                                                <asp:ListItem Value="0" Text="%" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Value="1" Text="Value"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                    </div>                                                    
                                                    <div class="row">
                                                        
                                                    </div>                                                    
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel ID="TabPanelAddress" runat="server" Height="400px">
                                <HeaderTemplate>
                                    Address
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="main-contents" style="overflow-y: scroll; height: 380px;">
                                        <div class="modal-body">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                       <div class="col-md-9">
                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Address</label>
                                                            <asp:TextBox ID="txtAddress1" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                         <div class="col-md-1" style="margin-top: 22px;">
                                                            <asp:Button ID="btnAddress" runat="server" Text="Add" CssClass="btn btn-success"
                                                                OnClick="btnAddress_Click" OnClientClick="return ValidateFormAddress();" />
                                                        </div>
                                                        <asp:HiddenField ID="AddressRowId" runat="server" Value="0" />
                                                        </div> 
                                                    <div class="row">
                                                        <asp:Panel ID="Panel1" runat="server" Width="98%" Height="180px"
                                                            ScrollBars="Vertical" BorderColor="Silver" BorderStyle="Groove"
                                                            BorderWidth="1px">
                                                            <asp:GridView Width="100%" ID="GridAdress" runat="server" Class="table table-striped table-bordered table-hover table-condensed cf"
                                                                AutoGenerateColumns="False" OnRowDeleting="GridAdress_RowDeleting" OnRowEditing="GridAdress_RowEditing"
                                                                ShowHeader="true">

                                                                <Columns>
                                                                    <asp:BoundField DataField="CUSTOMER_ID" HeaderText="CUSTOMER_ID" ReadOnly="true">
                                                                        <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                                        <ItemStyle CssClass="HidePanel"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Customer" ReadOnly="true">
                                                                        <ItemStyle Width="30%"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CUSTOMER_ADDRESS" HeaderText="Address" ReadOnly="true">
                                                                        <ItemStyle Width="60%"></ItemStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="fa fa-pencil" CommandName="Edit" ToolTip="Edit">
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />

                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemStyle Width="5%" HorizontalAlign="Center"></ItemStyle>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="glyphicon glyphicon-trash"
                                                                                OnClientClick="javascript:return confirm('Are you sure you want to delete?');return false;"
                                                                                ToolTip="Delete">
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </div>                  
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </ajaxToolkit:TabPanel>
                        </ajaxToolkit:TabContainer>
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
                                     &nbsp; &nbsp;  <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                         <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="CUSTOMER_ID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_CODE" HeaderText="Card ID" ReadOnly="true">
                                    <ItemStyle Width="10%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CUSTOMER_NAME" HeaderText="Name" ReadOnly="true">
                                    <HeaderStyle Width="30%"></HeaderStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CONTACT_NUMBER" HeaderText="Contact" ReadOnly="true">
                                    <ItemStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EMAIL_ADDRESS" HeaderText="Email" ReadOnly="true">
                                    <ItemStyle Width="15%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ADDRESS" HeaderText="Address" ReadOnly="true">
                                    <ItemStyle Width="30%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CONTACT2" HeaderText="" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="REGDATE" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BARCODE" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>

                                <asp:BoundField DataField="IS_ACTIVE1" HeaderText="Status" ReadOnly="true">
                                    <ItemStyle Width="8%" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NATURE" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="OPENING_AMOUNT" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CNIC" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DISCOUNT" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="PURCHASING" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="POINTS" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AMOUNT_LIMIT" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="strCardNo" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CARD_TYPE_ID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>

                                <asp:BoundField DataField="DISTRIBUTOR_ID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NotEditable" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SALES_PER" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Membership_Date" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Spouse_Name" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Profession" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Membership_No" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>                                                                
                                 <asp:BoundField DataField="CUSTOMER_CATEGORY_ID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="CUSTOMER_STATUS_ID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                 <asp:BoundField DataField="IsGolfer" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>

                                <asp:BoundField DataField="FROM_DATE" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TO_DATE" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GroupID" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GST" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GST_Type" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DISCOUNTPOS" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DISCOUNT_Type" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SERVICE_CHARGES" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SERVICE_CHARGES_Type" ReadOnly="true">
                                    <ItemStyle CssClass="HidePanel" />
                                    <HeaderStyle CssClass="HidePanel" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LOYALTY_CARD_ID" ReadOnly="true">
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
