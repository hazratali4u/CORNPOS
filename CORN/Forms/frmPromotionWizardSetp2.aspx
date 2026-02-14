<%@ Page Title="CORN :: Prmotion Wizard Step 2" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmPromotionWizardSetp2.aspx.cs" Inherits="Forms_frmPromotionWizardSetp2" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <div class="main-contents">
        <div class="container employee-infomation">
            <h1 class="modal-title" id="myModalLabel">
                <span></span>Promotion Wizard Step 2
            </h1>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-3">
                            <asp:CheckBox ID="ChbAllLocationType" runat="server" Text="All Type Location" AutoPostBack="True"
                                OnCheckedChanged="ChbAllLocationType_CheckedChanged"></asp:CheckBox>
                            <br />
                            <asp:Panel ID="Panel1" runat="server" Width="250px" Height="250px" ScrollBars="Vertical"
                                BorderStyle="Groove" BorderWidth="1px">
                                <asp:CheckBoxList ID="ChbDistributorType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ChbDistributorType_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </div>
                        <div class="col-md-3">
                            <asp:CheckBox ID="chkSelectAllDistributors" runat="server" Text="All Location" AutoPostBack="True"
                                OnCheckedChanged="chkSelectAllDistributors_CheckedChanged"></asp:CheckBox>
                            <br />
                            <asp:Panel ID="Panel6" runat="server" Width="250px" Height="250px" ScrollBars="Vertical"
                                BorderStyle="Groove" BorderWidth="1px">
                                <asp:CheckBoxList ID="chklDistributors" runat="server" CssClass="lblbox">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </div>
                        <div class="col-md-3">
                            <asp:CheckBox ID="ChbAllCustomerGroup" runat="server" Text="All Customer Group" CssClass="lblbox"
                                AutoPostBack="True" OnCheckedChanged="ChbAllCustomerGroup_CheckedChanged"></asp:CheckBox>
                            <br />
                            <asp:Panel ID="Panel8" runat="server" Width="250px" Height="250px" ScrollBars="Vertical"
                                BorderStyle="Groove" BorderWidth="1px">
                                <asp:CheckBoxList ID="ChbVolumClass" runat="server">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </div>
                        <div class="col-md-3">
                            <asp:CheckBox ID="chbAllServiceType" runat="server" Text="All Service Type" CssClass="lblbox"
                                AutoPostBack="True" OnCheckedChanged="chbAllServiceType_CheckedChanged" Checked="true"></asp:CheckBox>
                            <br />
                            <asp:Panel ID="Panel2" runat="server" Width="250px" Height="250px" ScrollBars="Vertical"
                                BorderStyle="Groove" BorderWidth="1px">
                                <asp:CheckBoxList ID="chbServiceType" runat="server">
                                    <asp:ListItem Value="1" Text="Dine In" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Delivery" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Takeaway" Selected="True"></asp:ListItem>
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <br />
                            <br />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass=" btn btn-success" OnClick="btnCancel_Click"/>                            
                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass=" btn btn-success" OnClick="btnBack_Click"/>
                            <asp:Button ID="btnNext" runat="server" Text="Next" CssClass=" btn btn-success" OnClick="btnNext_Click"/>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>