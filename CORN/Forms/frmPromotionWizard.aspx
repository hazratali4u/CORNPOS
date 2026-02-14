<%@ Page Title="CORN :: Prmotion Wizard" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmPromotionWizard.aspx.cs" Inherits="Forms_frmPromotionWizard" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnl_rpt" runat="server">
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>From Date</label>
                                <asp:TextBox ID="txtStartDate" runat="server" MaxLength="10" onkeyup="BlockStartDateKeyPress()"
                                    CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-1" style="margin-top: 27px">
                                <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                    Width="30px" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>To Date</label>
                                <asp:TextBox ID="txtEndDate" runat="server" onkeyup="BlockEndDateKeyPress()"
                                    CssClass="form-control" MaxLength="10"></asp:TextBox>
                            </div>
                            <div class="col-md-1" style="margin-top: 27px">
                                <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                    Width="30px" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3 ">
                                <asp:CheckBox ID="cbActive" runat="server" Text=" Is Active"></asp:CheckBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3 ">
                                <asp:Button ID="btnGetPromotion" runat="server" Text="Get Promotion" CssClass=" btn btn-success" OnClick="btnGetPromotion_Click" />
                                <asp:Button ID="btnNewPromotion" runat="server" Text="New Promotion" CssClass=" btn btn-success" OnClick="btnNewPromotion_Click" />
                            </div>
                        </div>
                        <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
                        <cc1:CalendarExtender ID="CEStartDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnStartDate"
                            TargetControlID="txtStartDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                        <cc1:CalendarExtender ID="CEEndDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibnEndDate"
                            TargetControlID="txtEndDate" OnClientShown="calendarShown" PopupPosition="TopLeft"></cc1:CalendarExtender>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
        <br />
        <div class="row center">
            <div class="col-md-12">
                <div class="emp-table">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-2">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Search Type</label>
                                </div>
                                <div class="col-md-3">                                    
                                    <dx:ASPxComboBox ID="ddSearchType" runat="server" CssClass="form-control">
                                        <Items>
                                            <dx:ListEditItem Value="SKU_code" Text="All Records" Selected="true" />
                                            <dx:ListEditItem Value="PROMOTION_ID" Text="Promotion ID" />
                                            <dx:ListEditItem Value="PROMOTION_CODE" Text="Promotion Name" />
                                            <dx:ListEditItem Value="PROMOTION_DESCRIPTION" Text="Description" />
                                        </Items>
                                </dx:ASPxComboBox>
                                </div>
                                <div class="col-md-2">
                                    <dx:ASPxTextBox ID="txtSeach" CssClass="form-control" runat="server">
                                    </dx:ASPxTextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass=" btn btn-success" OnClick="btnFilter_Click" />
                                </div>
                            </div>
                            <asp:Panel ID="Panel2" runat="server" Width="100%" Height="300px" ScrollBars="Vertical"
                                BorderColor="Silver" BorderStyle="Groove" BorderWidth="1px">
                                <asp:GridView ID="Grid_pricedetails" runat="server" Width="100%" HorizontalAlign="Center"
                                    AutoGenerateColumns="False" OnRowEditing="Grid_pricedetails_RowEditing" OnRowDeleting="Grid_pricedetails_RowDeleting"
                                    Class="table table-striped table-bordered table-hover table-condensed cf">
                                    <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                        PreviousPageText="Previous"></PagerSettings>
                                    <AlternatingRowStyle BackColor="#E0E0E0" />
                                    <Columns>
                                        <asp:BoundField DataField="PROMOTION_ID" HeaderText="Promotion ID">
                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PROMOTION_CODE" HeaderText="Promotion Name">
                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PROMOTION_DESCRIPTION" HeaderText="Description">
                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="START_DATE" HeaderText="Start Date">
                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="END_DATE" HeaderText="End Date">
                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:BoundField>                                        
                                        <asp:BoundField DataField="IS_ACTIVE" HeaderText="Status">
                                            <ItemStyle BorderColor="Silver" BorderWidth="1px" BorderStyle="Solid"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Left" />
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
                                    <HeaderStyle CssClass="tblhead"></HeaderStyle>
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
