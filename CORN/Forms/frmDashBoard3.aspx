<%@ Page Language="C#" Title="CORN :: Dashboard" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmDashBoard3.aspx.cs" Inherits="Forms_frmDashBoard3" %>
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
            <td style="width:25%;" valign="top">
                <strong>Item</strong>
            </td>
            <td style="width:20%;">
                <strong>From Date</strong>
            </td>
            <td style="width:5%;" valign="top">
                
            </td>
            <td style="width:20%;">
                <strong>To Date</strong>
            </td>
            <td style="width:5%;" valign="top">
                
            </td>
            <td style="width:25%;"></td>
        </tr>
        <tr>
            <td style="width:25%;" valign="top">
                <dx2:ASPxComboBox runat="server" ID="ddlItem" CssClass="form-control" Width="250px">
                                    </dx2:ASPxComboBox>
            </td>
            <td style="width:20%;">
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" MaxLength="10" Height="25px" Width="200px"></asp:TextBox>                
            </td>
            <td style="width:5%;" valign="top">
                <asp:ImageButton ID="ibFromDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Height="20px" />
                <cc1:CalendarExtender ID="ceFromDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibFromDate"
                TargetControlID="txtFromDate" ></cc1:CalendarExtender>
            </td>
            <td style="width:20%;">
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" MaxLength="10" Height="25px" Width="200px"></asp:TextBox>                
            </td>
            <td style="width:5%;" valign="top">
                <asp:ImageButton ID="ibToDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Height="20px" />
                <cc1:CalendarExtender ID="ceToDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibToDate"
                TargetControlID="txtToDate" ></cc1:CalendarExtender>
            </td>
            <td style="width:25%;" valign="top">
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
