<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmCafeTimeSchedule.aspx.cs" Inherits="Forms_frmCafeTimeSchedule" Title="CORN :: Cafe Time Schedule" %>
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

        function ValidateForm() {
            var str;
          <%--  str = document.getElementById('<%=txtQuantity.ClientID%>').value;
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
            }--%>

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
                                

                                <cc1:ModalPopupExtender ID="mPopUpLocation" runat="server" PopupControlID="pnlParameters"
                                    TargetControlID="hbtn" BehaviorID="ModelPopup" BackgroundCssClass="modal-background"
                                    CancelControlID="btnClose">
                                </cc1:ModalPopupExtender>
                                <asp:Panel ID="pnlParameters" runat="server" DefaultButton="btnSaveDocument" Style="display: none; width: 100%" ScrollBars="Auto">
                                    <div class="modal-dialog2" style="width: 80%";>
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_Click">
                                                    <span>&times;</span><span class="sr-only">Close</span></button>
                                                <h1 class="modal-title" id="myModalLabel">
                                                    <span></span>Open / Close Time</h1>
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
                                                                <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0"
                                                                    Height="100%" Width="100%">
                                                                    <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" Height="100%">
                                                                        <HeaderTemplate>
                                                                            Time Schedule
                                                                        </HeaderTemplate>
                                                                        <ContentTemplate>
                                                                            <div class="main-contents" style="height: 100%;">
                                                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <div class="modal-body">
                                                                                            <div class="row">
                                                                                                <div class="col-md-4">
                                                                                                    <asp:HiddenField ID="cafeScheduleID" runat="server" Value="0" />
                                                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>

                                                                                                    <dx:ASPxComboBox ID="ddlLocation" runat="server" CssClass="form-control"
                                                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                                                                                    </dx:ASPxComboBox>
                                                                                                </div>
                                                                                            </div>


                                                                                            <div class="row">
                                                                                                <div class="col-md-8">
                                                                                                    <asp:GridView ID="Gridview1" runat="server" ShowFooter="false" AutoGenerateColumns="false">

                                                                                                        <Columns>

                                                                                                            <asp:BoundField DataField="DayName" HeaderText="Day" ReadOnly="true">
                                                                                                                <ItemStyle Width="400px"></ItemStyle>
                                                                                                                <HeaderStyle Height="35px"></HeaderStyle>
                                                                                                            </asp:BoundField>

                                                                                                            <asp:TemplateField HeaderText="Time From">
                                                                                                                <ItemStyle Width="350px" />
                                                                                                                <HeaderStyle Height="35px"></HeaderStyle>
                                                                                                                <ItemTemplate>
                                                                                                                    <cc1:TimeSelector ID="tsFrom" runat="server" Width="100%" Style="vertical-align: baseline; margin-top: 10px"
                                                                                                                        DisplaySeconds="false" SelectedTimeFormat="Twelve" DisplayButtons="true">
                                                                                                                    </cc1:TimeSelector>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>

                                                                                                            <asp:TemplateField HeaderText="Time To">
                                                                                                                <ItemStyle Width="350px" />
                                                                                                                <HeaderStyle Height="35px"></HeaderStyle>
                                                                                                                <ItemTemplate>
                                                                                                                    <cc1:TimeSelector ID="tsTo" runat="server" Width="100%" Style="vertical-align: baseline; margin-top: 10px"
                                                                                                                        DisplaySeconds="false" SelectedTimeFormat="Twelve" DisplayButtons="true">
                                                                                                                    </cc1:TimeSelector>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>

                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                            </div>

                                                                                        </div>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </ajaxToolkit:TabPanel>
                                                                    <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" Height="400px">
                                                                        <HeaderTemplate>
                                                                            Temporarily Closed
                                                                        </HeaderTemplate>
                                                                        <ContentTemplate>
                                                                            <div class="main-contents" style="overflow-y: scroll; height: 380px;">
                                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                                                    <ContentTemplate>
                                                                                        <div class="modal-body">

                                                                                            <div class="row">
                                                                                                <div class="col-md-4">
                                                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Location</label>

                                                                                                    <dx:ASPxComboBox ID="ddlLocation1" runat="server" CssClass="form-control">
                                                                                                    </dx:ASPxComboBox>
                                                                                                </div>
                                                                                            
                                                                                                <div class="col-md-2">
                                                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>From Date:</label>
                                                                                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                </div>
                                                                                                <div class="col-md-1" style="margin-top: 25px;">
                                                                                                    <asp:ImageButton ID="ibtnFromDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                                                                        Width="30px" />
                                                                                                    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                                                                                    <cc1:CalendarExtender ID="CEFromDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnFromDate"
                                                                                                        TargetControlID="txtFromDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                                                                                                </div>

                                                                                            
                                                                                                <div class="col-md-2">
                                                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>To Date:</label>
                                                                                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                                                                </div>
                                                                                                <div class="col-md-1" style="margin-top: 25px;">
                                                                                                    <asp:ImageButton ID="ibtnToDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                                                                        Width="30px" />
                                                                                                    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                                                                                    <cc1:CalendarExtender ID="CEToDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnToDate"
                                                                                                        TargetControlID="txtToDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                                                                                                </div>

                                                                                                </div>

                                                                                            <div class="row">
                                                                                                <div class="col-md-2">
                                                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Time From</label>
                                                                                                    <cc1:TimeSelector ID="tsFrom1" runat="server" Width="100%" Style="vertical-align: baseline; margin-top: 10px"
                                                                                                        DisplaySeconds="false" SelectedTimeFormat="Twelve" DisplayButtons="true">
                                                                                                    </cc1:TimeSelector>
                                                                                                </div>
                                                                                                <div class="col-md-2">
                                                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Time To</label>
                                                                                                    <cc1:TimeSelector ID="tsTo1" runat="server" Width="100%" Style="vertical-align: baseline; margin-top: 10px"
                                                                                                        DisplaySeconds="false" SelectedTimeFormat="Twelve" DisplayButtons="true">
                                                                                                    </cc1:TimeSelector>
                                                                                                </div>

                                                                                                <div class="col-md-3" style="margin-top:30px;"> 
                                                                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Is Temporary closed</label>
                                                                                                    <dx:ASPxCheckBox ID="chkIsTemporaryClosed" runat="server" CheckState="Unchecked">
                                                                                                    </dx:ASPxCheckBox>
                                                                                                </div>
                                                                                            </div>

                                                                                                <div class="row" style="margin-top: 25px;">
                                                                                                    <div class="col-md-8">
                                                                                                        <label><span class="fa fa-caret-right rgt_cart"></span>Message</label>
                                                                                                        <asp:TextBox ID="txtMessage" runat="server" Rows="1" CssClass="form-control" MaxLength="200"></asp:TextBox>
                                                                                                    </div>
                                                                                                </div>
                                                                                        </div>

                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </ajaxToolkit:TabPanel>

                                                                </ajaxToolkit:TabContainer>
                                                                </div>
                                                <div class="row" style="margin-top: 20px; margin-right: 40px;">
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

            <div class="row center" style="margin-left:1px; margin-right:-10px;">
                <asp:HiddenField ID="_rowNo" runat="server" Value="0" />
                <div class="col-md-12">
                    <div class="emp-table">
                        <asp:UpdatePanel ID="UpdatePanelDetail" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="Grid_users" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                    AutoGenerateColumns="False" OnPageIndexChanging="Grid_users_PageIndexChanging" PageSize="20" AllowPaging="true"
                                    OnRowEditing="GrdPurchase_RowEditing" EmptyDataText="No Record exist"
                                    OnRowDataBound="Grid_users_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DISTRIBUTOR_ID" HeaderText="Distributor" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Message" HeaderText="Message" ReadOnly="true">
                                            <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            <ItemStyle CssClass="HidePanel "></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DISTRIBUTOR_NAME" HeaderText="Location" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Day" HeaderText="Day" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="From" HeaderText="From" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="15%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="To" HeaderText="To" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="18%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="IsTemporaryClosed" HeaderText="Is Temporary Closed" ReadOnly="true">
                                            <ItemStyle CssClass="grdDetail" Width="18%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="grdHead" />
                                        </asp:BoundField>
                                        
                                       <%-- <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" class="fa fa-pencil" CommandName="Edit" ToolTip="Edit">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
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
