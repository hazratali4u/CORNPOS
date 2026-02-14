<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmEcommWebImages.aspx.cs" Inherits="Forms_frmEcommWebImages" Title="CORN :: Web Images" %>
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

            <div class="row top" style="display:none;">
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
                                        <%--<asp:TemplateField>
                                            <HeaderTemplate>
                                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChbIsAssigned" runat="server" onclick="Check_Click(this)" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>
                                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true">
                                             <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ImageUrl" HeaderText="Image Url" ReadOnly="true">
                                             <HeaderStyle Width="55%" />
                                            <ItemStyle Width="55%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Interval" HeaderText="Interval" ReadOnly="true">
                                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Height" HeaderText="Height" ReadOnly="true">
                                             <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Width" HeaderText="Width" ReadOnly="true">
                                             <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ImageType" HeaderText="Image Type" ReadOnly="true">
                                             <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
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
