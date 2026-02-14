<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmEcommCatgoryWiseItem.aspx.cs" Inherits="frmEcommCatgoryWiseItem" Title="CORN :: Category Wise Item" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">   
    
    <script language="JavaScript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }

        function bindSKUImage() {
            debugger;
            var val = document.getElementById('<%=hidSKUImageName.ClientID%>').value;

            if (val == '' || val == null) {
                $('#pic').attr('src', '../images/no_image.gif').width(266).height(130);
            } else {
                var logo = val;
                $('#pic').attr('src', '../UserImages/Category/' + logo).width(266).height(130);
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

   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="btndummy">
                <div class="main-contents">
                    <div class="container role_management">
                        <div class="row center">
                                <div class="col-md-3">
                                <label>
                                    <span class="fa fa-caret-right rgt_cart"></span> POS Category</label>
                                
                                <dx:ASPxComboBox  ID="ddlCategory1" runat="server" CssClass="form-control" 
                                    OnSelectedIndexChanged="ddlCategory1_SelectedIndexChanged"
                                    AutoPostBack="True" TabIndex="2"></dx:ASPxComboBox>
                            </div>
                            <div class="col-md-3"></div>
                            <div class="col-md-3">
                                <label>
                                    <span class="fa fa-caret-right rgt_cart"></span> Ecommerce Category</label>
                                
                                <dx:ASPxComboBox  ID="ddlCategory" runat="server" CssClass="form-control" 
                                    OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                    AutoPostBack="True" TabIndex="2"></dx:ASPxComboBox>
                            </div>
                             <div class="col-md-1" style="margin-top: 26px;width:40px;">
                                 <asp:LinkButton ID="btnEdit" runat="server" CssClass="fa fa-pencil" Font-Size="30px" OnClick="btnEdit_Click" ToolTip="Edit">
                                 </asp:LinkButton>
                            </div>
                            <div class="col-md-2" style="margin-top: 26px;">
                                <button class="btn btn-primary" type="button" runat="server" id="btnAdd">Add new Category</button>
                            </div>
                        </div>

                        <asp:HiddenField ID="hidSKUImageName" runat="server" />
                    <asp:HiddenField ID="hidSKUImageSource" runat="server" />

                        <!-- Add POP UP MODEL-->
                                            <cc1:ModalPopupExtender ID="mPopUpCategory" runat="server" PopupControlID="pnlParameters"
                                                TargetControlID="btnAdd" BehaviorID="ModelPopup2" BackgroundCssClass="modal-background"
                                                CancelControlID="btnClose">
                                            </cc1:ModalPopupExtender>
                                            <asp:Panel ID="pnlParameters" runat="server" Style="display: none; width: 30%" ScrollBars="Auto" DefaultButton="btnSaveCategory">
                                                <div class="modal-dialog" style="width: 100%">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" id="btnClose" class="close" runat="server" onserverclick="btnClose_Click">
                                                                <span>&times;</span><span class="sr-only">Close</span></button>
                                                            <h1 class="modal-title" id="myModalLabel">
                                                                <span></span>Add New Category</h1>
                                                        </div>
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <div class="modal-body">
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Parent Category</label>
                                                                            <dx:ASPxComboBox ID="drpParentCategory" runat="server" CssClass="form-control">
                                                                            </dx:ASPxComboBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span> Category Name </label>

                                                                            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <label><span class="fa fa-caret-right rgt_cart"></span>Sort Order</label>
                                                                            <asp:TextBox ID="txtSortOrder" Text="1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server"
                                                                                FilterType="Custom" ValidChars="0123456789." TargetControlID="txtSortOrder"></ajaxToolkit:FilteredTextBoxExtender>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">
                                                                            <div id="skuImageUploadArea" runat="server" visible="true">
                                                                            <label>
                                                                                <span class="fa fa-caret-right rgt_cart"></span>Upload Image
                                                                                    &nbsp;(<%=allowed_extensions%>)
                                                                            </label>
                                                                            <label for="<%=fuPic.ClientID %>" style="cursor: pointer;">
                                                                                <img alt="CategoryImage" src="../images/no_image.gif" id="pic" name="pic" width="266" height="130" />
                                                                            </label>
                                                                            <asp:FileUpload ID="fuPic" onchange="readImageURL(this);" Style="display: none;" runat="server"></asp:FileUpload>
                                                                        </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="row" style="margin-top: 20px; margin-bottom: 20px;">
                                                                        <div class="col-md-7">
                                                                        </div>
                                                                        <div class="col-md-12" align="right">
                                                                            <asp:HiddenField ID="hfStatus" runat="server" Value="Active" />
                                                                            <asp:HiddenField ID="hfcategoryId" runat="server" Value="0" />
                                                                            <asp:Button ID="btnSaveCategory" OnClick="btnSaveCategory_Click" runat="server" Text="Save" CssClass="btn btn-success" />
                                                                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </asp:Panel>


                        <div class="row bottom1">
                            <label>
                                <span class="fa fa-caret-right rgt_cart"></span>Item Assignment</label>
                        </div>
                        <div class="row bottom">
                            <div class="module_contents">
                                <div class="col-md-5">
                                    <asp:ListBox ID="lstUnAssignModule" runat="server" CssClass="select" TabIndex="5" ></asp:ListBox>
                                </div>
                                <div class="col-md-1">
                                    <div class="navs navslist" style="padding-top:60px;">
                                        <div class="nxt arrow">
                                            <a href="#"><span>
                                                <asp:LinkButton ID="btnAssign" runat="server" OnClick="btnAssign_Click" CssClass="fa fa-angle-right" TabIndex="6"/>
                                            </span></a>
                                        </div>
                                        <div class="nxt arrow">
                                            <span>
                                                <asp:LinkButton ID="btnAssignAllItem" OnClick="btnAssignAllItem_Click"
                                                    CssClass="fa  fa-angle-double-right" runat="server" /></span>
                                        </div>
                                        <div class="nxt arrow">
                                            <span>
                                                <asp:LinkButton ID="btnUnAssignAllItem" CssClass="fa  fa-angle-double-left"
                                                    OnClick="btnUnAssignAllItem_Click" runat="server" /></span>
                                        </div>
                                        <div class="nxt arrow">
                                            <a href="#"><span>
                                                <asp:LinkButton ID="btnUnAssign" runat="server" OnClick="btnUnAssign_Click" CssClass="fa fa-angle-left" TabIndex="7"/>
                                            </span></a>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <asp:ListBox ID="lstAssignModule" CssClass="select" runat="server" TabIndex="8"></asp:ListBox>
                                </div>
                                <div class="col-md-1">
                                    <table style="margin-top: 70px;">
                                    <td style="width: 39%">

                        <asp:ImageButton ID="ibSubCatTop" runat="server" Height="20"
                        ImageUrl="~/images/ButtonUpall.png"
                        ToolTip="Move Up" Width="20" OnClick="ibItemTop_Click" />
                        <br />
                        <br />
                        <asp:ImageButton ID="ibSubCatUp" runat="server" Height="20px"
                        ImageUrl="~/images/Up_Button.png"
                        ToolTip="Move Up" Width="20px" OnClick="ibItemUp_Click" />
                        <br />
                        <br />
                        <asp:ImageButton ID="ibSubCatDown" runat="server" Height="20"
                        ImageUrl="~/images/ButtonDown.png"
                        ToolTip="Move down" Width="20" OnClick="ibItemDown_Click"  />
                        <br />
                        <br />
                        <asp:ImageButton ID="ibSubCatBottom" runat="server" Height="20"
                        ImageUrl="~/images/ButtonDownall.png"
                        ToolTip="Move Down" Width="20" OnClick="ibItemBottom_Click"  />
                        </td>
                                        </table>
                                </div>
                            </div>
                            <%--<div class="btnlist pull-right">
                                <asp:Button class="btn btn-success" OnClick="btnReport_Click" Text="View Report"
                                    runat="server" ID="btnReport" TabIndex="9"></asp:Button>
                            </div>--%>
                        </div>
                    </div>
                </div>
                <asp:LinkButton ID="btndummy" runat="server" UseSubmitBehavior="false" />
            </asp:Panel>
        </ContentTemplate>
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnReport" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
