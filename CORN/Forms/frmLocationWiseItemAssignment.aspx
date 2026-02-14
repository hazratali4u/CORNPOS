<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmLocationWiseItemAssignment.aspx.cs"
    Inherits="Forms_frmLocationWiseItemAssignment" Title="CORN :: Location wise Item Assignment" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <link href="../css/Popup.css" rel="stylesheet" />

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

            <div>
                <asp:UpdateProgress ID="UpdateProgress" runat="server">
                    <ProgressTemplate>
                        <asp:ImageButton ID="ImageButton10" runat="server" ImageUrl="~/OrderPOS/images/wheel.gif"  />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:LinkButton runat="server" ID="hbtn"></asp:LinkButton>
                <ajaxToolkit:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="hbtn" PopupControlID="UpdateProgress" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
            </div>
            <asp:Panel ID="Panel1" runat="server">
                <div class="main-contents">
                    <div class="container user_assignment">
                        <div class="row top">
                            <div class="col-md-3">
                                <label>
                                    <span class="fa fa-caret-right rgt_cart"></span>Location</label>
                                
                                <dx:ASPxComboBox ID="ddlLocation" runat="server" CssClass="form-control" 
                                    TabIndex="1" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                    AutoPostBack="True"></dx:ASPxComboBox>
                            </div>
                        </div>
                        <div class="row bottom">
                            <div class="col-md-11">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <span class="fa fa-caret-right rgt_cart"></span><b>Item Assignment</b>
                                    </div>
                                    <div class="panel-body">

                                            <%--<label><span class="fa fa-caret-right rgt_cart"></span>Category</label>--%>

                                        <div class="row">
                                                <div class="col-md-5">
                                                <dx:ASPxComboBox ID="ddlCategory" runat="server" CssClass="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                                </dx:ASPxComboBox>
                                            </div>
                                        </div>
                                        <div class="row" style="margin-bottom:10px;">
                                                <div class="col-md-4">
                                               <asp:TextBox ID="TextBox1" placeholder="Type Item name" runat="server" Width="100%"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:Button runat="server" OnClick="Button1_Click" Text="Search" />
                                            </div>
                                        </div>

                                        <div class="module_contents">
                                            <div class="col-md-5">
                                                <asp:ListBox ID="lstUnAssignDistributor" runat="server"
                                                    Height="350px" Width="100%" CssClass="select" TabIndex="3"></asp:ListBox>
                                            </div>
                                            <div class="col-md-1">
                                                <div class="navs navslist">
                                                    <div class="nxt arrow">
                                                        <span>
                                                            <asp:LinkButton ID="btnAssignLocation" CssClass="fa fa-angle-right" OnClick="btnAssignLocation_Click"
                                                                runat="server" TabIndex="4" /></span>
                                                    </div>
                                                    <div class="nxt arrow">
                                                        <span>
                                                            <asp:LinkButton ID="btnUnAssignLocation" CssClass="fa fa-angle-left" OnClick="btnUnAssignLocation_Click"
                                                                runat="server" TabIndex="5" /></span>
                                                    </div>
                                                    <div class="nxt arrow">
                                                        <span>
                                                            <asp:LinkButton ID="btnAssignAllItems" OnClick="btnAssignAllItem_Click"
                                                                CssClass="fa fa-angle-double-right" runat="server" TabIndex="9" /></span>
                                                    </div>
                                                    <div class="nxt arrow">
                                                        <span>
                                                            <asp:LinkButton ID="btnUnAssignAllItems" CssClass="fa fa-angle-double-left"
                                                                OnClick="btnUnAssignAllItem_Click" runat="server" TabIndex="10" /></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-5">
                                                <asp:ListBox ID="lstAssignDistributor" runat="server"
                                                    Height="350px" Width="100%" CssClass="select" TabIndex="6"></asp:ListBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:LinkButton ID="LinkButton5" runat="server" UseSubmitBehavior="false" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
