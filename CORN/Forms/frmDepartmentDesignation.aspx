<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmDepartmentDesignation.aspx.cs" Inherits="Forms_frmDepartmentDesignation" Title="CORN :: Department & Designation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">
     <script language="JavaScript" type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
     </script>
    <style type="text/css">
        .cmp input {
            width: 90%;
        }

        .cmp select {
            width: 90%;
        }

        .list input {
            width: 20%;
        }

        .list {
            width: 90%;
        }

        .tblheading {
            background: #006699;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
        }

            .tblheading td {
                color: #ffffff;
                padding: 5px 5px 5px 5px;
            }
    </style>
    
    <cc1:TabContainer ID="TabContainer1" runat="server" Height="450px" Width="65%"
        ActiveTabIndex="0">
        <cc1:TabPanel ID="TabPanel1" runat="server">
            <HeaderTemplate>
                Designation
            </HeaderTemplate>
            <ContentTemplate>

                <asp:UpdatePanel ID="UpdatePanel6" runat="server" >
                    <ContentTemplate>
                        <asp:HiddenField ID="RefId" runat="server" Value="0" />
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSaveChannelType">
                       
                             <div class="row">
                             <div class="col-md-6">
                                 <asp:Literal ID="lblErrorMsg" runat="server"  Visible="false"></asp:Literal>
                                  </div>
                        </div>

                        <div class="row">
                           
                            <div class="col-md-6" style="display: none">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Code</label>
                                <asp:TextBox ID="txtChannelCode" Enabled="False" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Name</label>
                                <asp:TextBox ID="txtChannelName" Enabled="False" runat="server" CssClass="form-control"></asp:TextBox>
                              </div>
                             <div class="col-md-6" align="right" style="margin-top: 25px">
                                <asp:Button ID="btnSaveChannelType" OnClick="btnSaveChannelType_Click" Width="125px" runat="server" Text="New" ValidationGroup="vg" CssClass="btn btn-success" />
                                <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server"  Width="125px" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" />
                            </div>
                            
                        </div>
                       
                        <hr />
                            </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="row">
                    <div class="col-md-12">
                        <div class="emp-table">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="grdChannelData" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                        AllowPaging="true" PageSize="7" OnPageIndexChanging="grdChannelData_PageIndexChanging"
                                        AutoGenerateColumns="False" OnRowEditing="grdChannelData_RowEditing" OnRowDeleting="grdChannelData_RowDeleting">

                                        <Columns>
                                            <asp:BoundField DataField="REF_ID" HeaderText="Id" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SLASH_CODE" HeaderText="Code" ReadOnly="true">
                                                <ItemStyle CssClass="HidePanel" Width="20%"></ItemStyle>
                                                 <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SLASH_DESC" HeaderText="Name" ReadOnly="true">
                                                <ItemStyle CssClass="grdDetail" Width="60%"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" ToolTip="Edit" class="fa fa-pencil">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" CssClass="grdDetail" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Center" CssClass="grdHead" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle CssClass="grdDetail" Width="10%" HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDelete" runat="server" class="fa fa-trash-o" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this?');return false;"
                                                        ToolTip="Delete">            
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel ID="TabPanel2" runat="server">
            <HeaderTemplate>
                Department
            </HeaderTemplate>
            <ContentTemplate>

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnSaveBusType">
                        <div class="row">
                            <div class="col-md-10">
                              
                            </div>
                        </div>
                             <div class="row">
                            <div class="col-md-6">
                                  <asp:Literal ID="lblErrorMsgDivsion" runat="server" Visible="false"></asp:Literal>
                                </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6" style="display: none">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Code</label>
                                <asp:TextBox ID="txtbustypeCode" Enabled="False" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Name</label>
                                <asp:TextBox ID="txtbustypeName" Enabled="False" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                             <div class="col-md-6" align="right" style="margin-top: 25px">
                                <asp:Button ID="btnSaveBusType" OnClick="btnSaveBusType_Click" Width="125px" runat="server" Text="New" ValidationGroup="vg" CssClass="btn btn-success" />
                                <asp:Button ID="btncancelDestype" OnClick="btncancelDestype_Click" Width="125px" runat="server" Style="margin-left: 5px" Text="Cancel" CssClass="btn btn-danger" />
                            </div>
                           
                        </div>
                       
                            <hr />
                            </asp:Panel>                      
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="row">
                    <div class="col-md-12">
                        <div class="emp-table">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="GrdBusType" runat="server" CssClass="table table-striped table-bordered table-hover table-condensed cf"
                                        HorizontalAlign="Center" AutoGenerateColumns="False" AllowPaging="true" PageSize="7"
                                        OnRowEditing="GrdBusType_RowEditing" OnRowDeleting="GrdBusType_RowDeleting" OnPageIndexChanging="GrdBusType_PageIndexChanging">

                                        <Columns>
                                            <asp:BoundField DataField="REF_ID" HeaderText="Id" ReadOnly="true">
                                                <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                                <ItemStyle CssClass="HidePanel"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SLASH_CODE" HeaderText="Code" ReadOnly="true">
                                                <ItemStyle CssClass="HidePanel" Width="20%"></ItemStyle>
                                                 <HeaderStyle CssClass="HidePanel"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SLASH_DESC" HeaderText="Name" ReadOnly="true">
                                                <ItemStyle CssClass="grdDetail" Width="58%"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" class="fa fa-pencil" ToolTip="Edit">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" CssClass="grdDetail" Width="10%" />
                                                <HeaderStyle HorizontalAlign="Center" CssClass="grdHead" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle CssClass="grdDetail" Width="10%" HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" class="fa fa-trash-o" OnClientClick="javascript:return confirm('Are you sure you want to delete this?');return false;"
                                                        ToolTip="Delete">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="GridPager" HorizontalAlign="Right" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>

</asp:Content>
