<%@ Page Title="CORN :: Prmotion Wizard Step 1" Language="C#" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmPromotionWizardSetp1.aspx.cs" Inherits="Forms_frmPromotionWizardSetp1" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="JavaScript" type="text/javascript">

        function ValidateForm() {
            var str;

            str = document.getElementById('<%=txtStartDate.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must select From Date');
                return false;
            }
            str = document.getElementById('<%=txtEndDate.ClientID%>').value;
            if (str == null || str.length == 0) {
                alert('Must select To Date');
                return false;
            }
            var str2 = Newtxt.GetValue();
            if (str2 == null || str2.length == 0) {
                alert('Must enter Scheme Name');
                return false;
            }
            return true;
        }

        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
        function BlockEndDateKeyPress() {
            document.getElementById('<%=txtEndDate.ClientID%>').value = '';
            alert('Click Clender Button Select Date');
        }
        function BlockStartDateKeyPress() {
            document.getElementById('<%=txtStartDate.ClientID%>').value = '';
            alert('Click Clender Button Select Date');
        }
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <h1 class="modal-title" id="myModalLabel">
                <span></span>Promotion Wizard Step 1
            </h1>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnl_rpt" runat="server">
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Promotion</label>
                                <dx:ASPxTextBox ID="txtPromotionName" CssClass="form-control" runat="server" ClientInstanceName="PromotionNametxt">
                                </dx:ASPxTextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Description</label>
                                <dx:ASPxMemo ID="txtPromotionDescription" CssClass="form-control" runat="server" Height="70">
                                </dx:ASPxMemo>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-10">
                                <fieldset>
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>From Date</label>
                                                    <asp:TextBox ID="txtStartDate" runat="server" MaxLength="10" onkeyup="BlockStartDateKeyPress()"
                                                        CssClass="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2" style="margin-top: 27px">
                                                    <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                        Width="30px" />
                                                </div>
                                                <div class="col-md-5">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>From Time</label>
                                                    <dx:ASPxComboBox ID="txtTimeFrom" runat="server" CssClass="form-control"
                                                        SelectedIndex="0">
                                                        <Items>
                                                            <dx:ListEditItem Text="12:00AM" Value="12:00AM" />
                                                            <dx:ListEditItem Text="12:30AM" Value="12:30AM" />
                                                            <dx:ListEditItem Text="1:00AM" Value="1:00AM" />
                                                            <dx:ListEditItem Text="1:30AM" Value="1:30AM" />
                                                            <dx:ListEditItem Text="2:00AM" Value="2:00AM" />
                                                            <dx:ListEditItem Text="2:30AM" Value="2:30AM" />
                                                            <dx:ListEditItem Text="3:00AM" Value="3:00AM" />
                                                            <dx:ListEditItem Text="3:30AM" Value="3:30AM" />
                                                            <dx:ListEditItem Text="4:00AM" Value="4:00AM" />
                                                            <dx:ListEditItem Text="4:30AM" Value="4:30AM" />
                                                            <dx:ListEditItem Text="5:00AM" Value="5:00AM" />
                                                            <dx:ListEditItem Text="5:30AM" Value="5:30AM" />
                                                            <dx:ListEditItem Text="6:00AM" Value="6:00AM" />
                                                            <dx:ListEditItem Text="6:30AM" Value="6:30AM" />
                                                            <dx:ListEditItem Text="7:00AM" Value="7:00AM" />
                                                            <dx:ListEditItem Text="7:30AM" Value="7:30AM" />
                                                            <dx:ListEditItem Text="8:00AM" Value="8:00AM" />
                                                            <dx:ListEditItem Text="8:30AM" Value="8:30AM" />
                                                            <dx:ListEditItem Text="9:00AM" Value="9:00AM" />
                                                            <dx:ListEditItem Text="9:30AM" Value="9:30AM" />
                                                            <dx:ListEditItem Text="10:00AM" Value="10:00AM" />
                                                            <dx:ListEditItem Text="10:30AM" Value="10:30AM" />
                                                            <dx:ListEditItem Text="11:00AM" Value="11:00AM" />
                                                            <dx:ListEditItem Text="11:30AM" Value="11:30AM" />
                                                            <dx:ListEditItem Text="12:00PM" Value="12:00PM" />
                                                            <dx:ListEditItem Text="12:30PM" Value="12:30PM" />
                                                            <dx:ListEditItem Text="1:00PM" Value="1:00PM" />
                                                            <dx:ListEditItem Text="1:30PM" Value="1:30PM" />
                                                            <dx:ListEditItem Text="2:00PM" Value="2:00PM" />
                                                            <dx:ListEditItem Text="2:30PM" Value="2:30PM" />
                                                            <dx:ListEditItem Text="3:00PM" Value="3:00PM" />
                                                            <dx:ListEditItem Text="3:30PM" Value="3:30PM" />
                                                            <dx:ListEditItem Text="4:00PM" Value="4:00PM" />
                                                            <dx:ListEditItem Text="4:30PM" Value="4:30PM" />
                                                            <dx:ListEditItem Text="5:00PM" Value="5:00PM" />
                                                            <dx:ListEditItem Text="5:30PM" Value="5:30PM" />
                                                            <dx:ListEditItem Text="6:00PM" Value="6:00PM" />
                                                            <dx:ListEditItem Text="6:30PM" Value="6:30PM" />
                                                            <dx:ListEditItem Text="7:00PM" Value="7:00PM" />
                                                            <dx:ListEditItem Text="7:30PM" Value="7:30PM" />
                                                            <dx:ListEditItem Text="8:00PM" Value="8:00PM" />
                                                            <dx:ListEditItem Text="8:30PM" Value="8:30PM" />
                                                            <dx:ListEditItem Text="9:00PM" Value="9:00PM" />
                                                            <dx:ListEditItem Text="9:30PM" Value="9:30PM" />
                                                            <dx:ListEditItem Text="10:00PM" Value="10:00PM" />
                                                            <dx:ListEditItem Text="10:30PM" Value="10:30PM" />
                                                            <dx:ListEditItem Text="11:00PM" Value="11:00PM" />
                                                            <dx:ListEditItem Text="11:30PM" Value="11:30PM" />
                                                        </Items>
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>To Date</label>
                                                    <asp:TextBox ID="txtEndDate" runat="server" onkeyup="BlockEndDateKeyPress()"
                                                        CssClass="form-control" MaxLength="10"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2" style="margin-top: 27px">
                                                    <asp:ImageButton ID="ibnEndDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                                        Width="30px" />
                                                </div>
                                                <div class="col-md-5">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>To Time</label>
                                                    <dx:ASPxComboBox ID="txtTimeTo" runat="server" CssClass="form-control">
                                                        <Items>
                                                            <dx:ListEditItem Text="12:00AM" Value="23:59:59" Selected="true" />
                                                            <dx:ListEditItem Text="12:30AM" Value="0:29:59" />
                                                            <dx:ListEditItem Text="1:00AM" Value="0:59:59" />
                                                            <dx:ListEditItem Text="1:30AM" Value="1:29:59" />
                                                            <dx:ListEditItem Text="2:00AM" Value="1:59:59" />
                                                            <dx:ListEditItem Text="2:30AM" Value="2:29:59" />
                                                            <dx:ListEditItem Text="3:00AM" Value="2:59:59" />
                                                            <dx:ListEditItem Text="3:30AM" Value="2:29:59" />
                                                            <dx:ListEditItem Text="4:00AM" Value="3:59:59" />
                                                            <dx:ListEditItem Text="4:30AM" Value="3:29:59" />
                                                            <dx:ListEditItem Text="5:00AM" Value="4:59:59" />
                                                            <dx:ListEditItem Text="5:30AM" Value="5:29:59" />
                                                            <dx:ListEditItem Text="6:00AM" Value="5:59:59" />
                                                            <dx:ListEditItem Text="6:30AM" Value="6:29:59" />
                                                            <dx:ListEditItem Text="7:00AM" Value="6:59:59" />
                                                            <dx:ListEditItem Text="7:30AM" Value="7:29:59" />
                                                            <dx:ListEditItem Text="8:00AM" Value="7:59:59" />
                                                            <dx:ListEditItem Text="8:30AM" Value="8:29:59" />
                                                            <dx:ListEditItem Text="9:00AM" Value="8:59:59" />
                                                            <dx:ListEditItem Text="9:30AM" Value="9:29:59" />
                                                            <dx:ListEditItem Text="10:00AM" Value="9:59:59" />
                                                            <dx:ListEditItem Text="10:30AM" Value="10:29:59" />
                                                            <dx:ListEditItem Text="11:00AM" Value="10:59:59" />
                                                            <dx:ListEditItem Text="11:30AM" Value="11:29:59" />
                                                            <dx:ListEditItem Text="12:00PM" Value="11:59:59" />
                                                            <dx:ListEditItem Text="12:30PM" Value="12:29:59" />
                                                            <dx:ListEditItem Text="1:00PM" Value="12:59:59" />
                                                            <dx:ListEditItem Text="1:30PM" Value="13:29:59" />
                                                            <dx:ListEditItem Text="2:00PM" Value="13:59:59" />
                                                            <dx:ListEditItem Text="2:30PM" Value="14:29:59" />
                                                            <dx:ListEditItem Text="3:00PM" Value="14:59:59" />
                                                            <dx:ListEditItem Text="3:30PM" Value="15:29:59" />
                                                            <dx:ListEditItem Text="4:00PM" Value="15:59:59" />
                                                            <dx:ListEditItem Text="4:30PM" Value="16:29:59" />
                                                            <dx:ListEditItem Text="5:00PM" Value="16:59:59" />
                                                            <dx:ListEditItem Text="5:30PM" Value="17:29:59" />
                                                            <dx:ListEditItem Text="6:00PM" Value="17:59:59" />
                                                            <dx:ListEditItem Text="6:30PM" Value="18:29:59" />
                                                            <dx:ListEditItem Text="7:00PM" Value="18:59:59" />
                                                            <dx:ListEditItem Text="7:30PM" Value="19:29:59" />
                                                            <dx:ListEditItem Text="8:00PM" Value="19:59:59" />
                                                            <dx:ListEditItem Text="8:30PM" Value="20:29:59" />
                                                            <dx:ListEditItem Text="9:00PM" Value="20:59:59" />
                                                            <dx:ListEditItem Text="9:30PM" Value="21:29:59" />
                                                            <dx:ListEditItem Text="10:00PM" Value="21:59:59" />
                                                            <dx:ListEditItem Text="10:30PM" Value="22:29:59" />
                                                            <dx:ListEditItem Text="11:00PM" Value="22:59:59" />
                                                            <dx:ListEditItem Text="11:30PM" Value="23:29:59" />
                                                        </Items>
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-1"></div>
                                        <div class="col-md-5">
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <label><span class="fa fa-caret-right rgt_cart"></span>Days</label>
                                                    <asp:CheckBoxList ID="cblDays" runat="server">
                                                        <asp:ListItem Value="1" Text="Sunday" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Monday" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="Tuesday" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="Wednesday" Selected="True"></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </div>
                                                <div class="col-md-5">
                                                    <br />
                                                    <asp:CheckBoxList ID="cblDays2" runat="server">
                                                        <asp:ListItem Value="5" Text="Thursday" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="6" Text="Friday" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="7" Text="Saturday" Selected="True"></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                            </div>
                            </fieldset>
                        </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <br />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass=" btn btn-success" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnNext" runat="server" Text="Next" CssClass=" btn btn-success" OnClick="btnNext_Click" />
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
    </div>
</asp:Content>