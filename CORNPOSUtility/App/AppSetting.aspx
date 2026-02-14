<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMain.master" AutoEventWireup="true" CodeFile="AppSetting.aspx.cs" Inherits="AppSetting" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cDefault" ContentPlaceHolderID="cphChild" Runat="Server">    
    <style type="text/css">
    label { display: inline-block;margin-left:5px;padding-left:5px; }
</style>
    <section id="content">
      <div class="container">
        <div class="row demobtn">
          <div class="span12">
              <div class="row">
                  <div class="span6">
                      <strong><asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label> </strong>
                  </div>
              </div>
            <div class="row" style="margin-bottom: 0px;">
              <div class="span6">
                  <strong>Select Database</strong>
                  <br />
                  <asp:DropDownList ID="ddlDB" runat="server" AutoPostBack="true"
                      OnSelectedIndexChanged="ddlDB_SelectedIndexChanged"></asp:DropDownList>
                <br />
                  <strong>Select App Setting</strong>
                  <br />
                      <asp:DropDownList ID="ddlSetting" runat="server" AutoPostBack="true"
                      OnSelectedIndexChanged="ddlSetting_SelectedIndexChanged"></asp:DropDownList>
                  <br />
                  <asp:Label ID="lblRemarks" runat="server">Remarks</asp:Label>
                  <br />
                  <strong><asp:Label ID="lblText" runat="server" Text="Select Value"></asp:Label> </strong>
                  <br />
                  <asp:DropDownList ID="ddlAvailableValues" runat="server"></asp:DropDownList>
                  <asp:TextBox ID="txtValue" runat="server" Visible="false"></asp:TextBox>
                  <div id="dvDailySalesReportColumnsCheckBoxes" runat="server" visible="false">
                      <asp:CheckBox ID="cbComplimentarySales" runat="server" Text="ComplimentarySales" />
                      <br />
                      <asp:CheckBox ID="cbVoidSales" runat="server" Text="VoidSales"/>
                      <br />
                      <asp:CheckBox ID="cbVoidOrder" runat="server" Text="VoidOrder" />
                      <br />
                      <asp:CheckBox ID="cbItemLess" runat="server" Text="ItemLess"/>
                      <br />
                      <asp:CheckBox ID="cbItemCancel" runat="server" Text="ItemCancel"/>
                  </div>
                  <div id="dvHiddenReportsDetailCheckBoxes" runat="server" visible="false">
                      <asp:CheckBox ID="cbDailySummaryPOS" runat="server" Text="Daily Summary POS" />
                      <br />
                      <asp:CheckBox ID="cbSalesSummaryPOS" runat="server" Text="Sales Summary POS" />
                      <br />
                      <asp:CheckBox ID="cbCashRegisterClosingForm" runat="server" Text="Cash Register Closing Form" />
                  </div>
                  <br />
                  <br />
                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update App Setting" class="btn btn-primary"/>
              </div>
            </div>              
          </div>
          <!-- end span12 -->
        </div>
      </div>
    </section>
</asp:Content>