<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMain.master" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="User" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cDefault" ContentPlaceHolderID="cphChild" Runat="Server">        
    <style type="text/css">
        .HidePanel {
            display: none;
        }
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
              <div class="span9">
                  <strong>Select Database</strong>
                  <br />
                  <asp:DropDownList ID="ddlDB" runat="server" AutoPostBack="true"
                      OnSelectedIndexChanged="ddlDB_SelectedIndexChanged"></asp:DropDownList>
                <asp:Panel ID="pnlDb" runat="server" Height="250" ScrollBars="Vertical" BorderColor="Black" Width="100%" Style="overflow-y: scroll;">                        
                    <asp:GridView runat="server" ID="gvUser" AutoGenerateColumns="False"
                        OnRowDataBound="gvUser_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="USER_ID" HeaderText="USER_ID" ReadOnly="true">
                                <HeaderStyle CssClass="HidePanel" />
                                <ItemStyle CssClass="HidePanel" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="User Name" DataField="LOGIN_ID" ReadOnly="true">
                                <ItemStyle Width="20%" />
                                <HeaderStyle HorizontalAlign ="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Insight">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbMobile" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="15%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Void GST">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbVoidGST" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="15%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dates On POS Reports">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbShowDatesOnPOSReports" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="IsMobileInsightAllowed" HeaderText="IsMobileInsightAllowed" ReadOnly="true">
                                <HeaderStyle CssClass="HidePanel" />
                                <ItemStyle CssClass="HidePanel" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CanVoidGST" HeaderText="CanVoidGST" ReadOnly="true">
                                <HeaderStyle CssClass="HidePanel" />
                                <ItemStyle CssClass="HidePanel" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ShowDatesOnPOSReports" HeaderText="CanVoidGST" ReadOnly="true">
                                <HeaderStyle CssClass="HidePanel" />
                                <ItemStyle CssClass="HidePanel" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Tax Integration">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbCanTaxIntegrate" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="CanTaxIntegrate" HeaderText="CanTaxIntegrate" ReadOnly="true">
                                <HeaderStyle CssClass="HidePanel" />
                                <ItemStyle CssClass="HidePanel" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    </asp:Panel>    
              </div>                   
            </div>
               <div class="row" style="margin-bottom: 0px;text-align:center;">
                   <div class="span9">
                   <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update User" class="btn btn-primary"/>
                       </div>
               </div>              
          </div>
          <!-- end span12 -->
        </div>
      </div>
    </section>
</asp:Content>