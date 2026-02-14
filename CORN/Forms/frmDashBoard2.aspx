<%@ Page Language="C#" Title="CORN :: Dashboard" MasterPageFile="~/Forms/PageMaster.master"
    AutoEventWireup="true" CodeFile="frmDashBoard2.aspx.cs" Inherits="Forms_frmDashBoard2" %>

<%@ Register Assembly="DevExpress.Dashboard.v16.1.Web, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
    <dx:ASPxDashboardViewer ID="dshViewer" runat="server"
        OnDashboardLoading="dshViewer_DashboardLoading"
        OnDataLoading="dshViewer_DataLoading"
        Width="100%" Height="800px">
    </dx:ASPxDashboardViewer>
</asp:Content>
