<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMain.master" AutoEventWireup="true" CodeFile="LicenseDate.aspx.cs" Inherits="LastDayClose" %>
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
              <div class="span8">
                  <asp:GridView ID="gvLicnes" runat="server" ShowFooter="false" AutoGenerateColumns="false" OnRowDataBound="gvLicnes_RowDataBound">
                      <Columns>
                          <asp:BoundField HeaderText="S.No" ReadOnly="true">
                              <ItemStyle Width="10%" HorizontalAlign="Center"></ItemStyle>
                          </asp:BoundField>
                          <asp:BoundField DataField="dbName" HeaderText="DB Name" ReadOnly="true">
                              <ItemStyle Width="20%"></ItemStyle>
                          </asp:BoundField>
                          <asp:BoundField DataField="LocationName" HeaderText="Location Name" ReadOnly="true">
                              <ItemStyle Width="30%"></ItemStyle>
                          </asp:BoundField>
                          <asp:BoundField DataField="LastDayClose" HeaderText="Last Day Close" ReadOnly="true"
                              DataFormatString="{0:dd-MMM-yyyy}">
                              <ItemStyle Width="20%"></ItemStyle>
                          </asp:BoundField>
                          <asp:BoundField DataField="LicenseDate" HeaderText="License Date" ReadOnly="true" DataFormatString="{0:dd-MMM-yyyy}">
                              <ItemStyle Width="20%"></ItemStyle>
                          </asp:BoundField>
                      </Columns>
                  </asp:GridView>
              </div>
            </div>              
          </div>
          <!-- end span12 -->
        </div>
      </div>
    </section>
</asp:Content>