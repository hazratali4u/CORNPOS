<%@ Page Language="C#" Title="CORN :: Dashboard" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmDashBoard5.aspx.cs" Inherits="Forms_frmDashBoard5" %>
<%@ Register Assembly="DevExpress.Dashboard.v16.1.Web, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <script type="text/javascript">
        function pageLoad()
        {
            document.getElementsByClassName('dx-dashboard-title-arguments')[0].innerHTML = '';
        }
    </script>
    <table width="100%">
        <tr>
            <td colspan="11" style="width:100%;">
                <asp:CheckBox ID="cbModifier" runat="server" Text="With Modifiers" Visible="false"/>
            </td>
        </tr>
        <tr>
            <td style="width:13%;">
                <strong>Dashboard Type</strong>
            </td>
            <td style="width:2%;" valign="top">                
            </td>
            <td style="width:10%;">
                <strong>Post From Date</strong>
            </td>
            <td style="width:5%;" valign="top">                
            </td>
            <td style="width:10%;">
                <strong>Past To Date</strong>
            </td>
            <td style="width:5%;" valign="top">                
            </td>
            <td style="width:10%;">
                <strong>Present From Date</strong>
            </td>
            <td style="width:5%;" valign="top">                
            </td>
            <td style="width:10%;">
                <strong>Present To Date</strong>
            </td>
            <td style="width:5%;" valign="top">                
            </td>
            <td style="width:20%;"></td>
        </tr>
        <tr>
            <td style="width:13%;">
                <asp:DropDownList ID="ddlDashboardType" runat="server" Width="100%" CssClass="form-control"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlDashboardType_SelectedIndexChanged">
                    <asp:ListItem Value="1" Text="Deal Wise" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Item Wise"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Category Wise"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width:2%;" valign="top">                
            </td>
            <td style="width:10%;">
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" MaxLength="10" Height="25px" Width="150px"></asp:TextBox>                
            </td>
            <td style="width:5%;" valign="top">
                <asp:ImageButton ID="ibFromDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Height="20px" />
                <cc1:CalendarExtender ID="ceFromDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibFromDate"
                TargetControlID="txtFromDate" ></cc1:CalendarExtender>
            </td>
            <td style="width:10%;">
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" MaxLength="10" Height="25px" Width="150px"></asp:TextBox>                
            </td>
            <td style="width:5%;" valign="top">
                <asp:ImageButton ID="ibToDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Height="20px" />
                <cc1:CalendarExtender ID="ceToDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibToDate"
                TargetControlID="txtToDate" ></cc1:CalendarExtender>
            </td>
            <td style="width:10%;">
                <asp:TextBox ID="txtFromDate2" runat="server" CssClass="form-control" MaxLength="10" Height="25px" Width="150px"></asp:TextBox>                
            </td>
            <td style="width:5%;" valign="top">
                <asp:ImageButton ID="ibFromDate2" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Height="20px" />
                <cc1:CalendarExtender ID="ceFromDate2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibFromDate2"
                TargetControlID="txtFromDate2" ></cc1:CalendarExtender>
            </td>
            <td style="width:10%;">
                <asp:TextBox ID="txtToDate2" runat="server" CssClass="form-control" MaxLength="10" Height="25px" Width="150px"></asp:TextBox>                
            </td>
            <td style="width:5%;" valign="top">
                <asp:ImageButton ID="ibToDate2" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Height="20px" />
                <cc1:CalendarExtender ID="ceToDate2" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibToDate2"
                TargetControlID="txtToDate2" ></cc1:CalendarExtender>
            </td>
            <td style="width:20%;" valign="top">
                <asp:Button AccessKey="L" ID="btnLoad" OnClick="btnLoad_Click" runat="server" Text="Load" CssClass="btn btn-success" />
            </td>
        </tr>
    </table>
    <dx:ASPxDashboardViewer ID="dshViewer" runat="server"
        OnDashboardLoading="dshViewer_DashboardLoading"
        OnDataLoading="dshViewer_DataLoading"
        Width="100%" Height="800px">
    </dx:ASPxDashboardViewer>
</asp:Content>
