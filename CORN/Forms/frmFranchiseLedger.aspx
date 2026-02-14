<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmFranchiseLedger.aspx.cs" Inherits="Forms_frmFranchiseLedger" Title="CORN :: Franchise Closing Summary" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cphPage">
    <script language="javascript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
        function onCalendarHidden() {
            var cal = $find("calendar1");

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function onCalendarShown() {

            var cal = $find("calendar1");

            cal._switchMode("months", true);
            cal._popupBehavior._element.style.zIndex = 10005;

            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", call);
                    }
                }
            }
        }

        function call(eventElement) {
            var target = eventElement.target;
            switch (target.mode) {
                case "month":
                    var cal = $find("calendar1");
                    cal._visibleDate = target.date;
                    cal.set_selectedDate(target.date);
                    //cal._switchMonth(target.date);
                    cal._blur.post(true);
                    cal.raiseDateSelectionChanged();
                    break;
            }
        }
    </script>
    <div class="main-contents">
        <div class="container employee-infomation">
            <div class="row">
                <div class="col-md-8">
                    <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <label><span class="fa fa-caret-right rgt_cart"></span>Franchise / Branch</label>
                    <dx:ASPxComboBox ID="drpDistributor" runat="server" AutoPostBack="true"
                         OnSelectedIndexChanged="drpDistributor_SELECTEDINDEXCHANGED"
                         CssClass="form-control">
                    </dx:ASPxComboBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <label><span class="fa fa-caret-right rgt_cart"></span>As On Month</label>
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" MaxLength="10" onkeyup="BlockStartDateKeyPress()"></asp:TextBox>
                </div>
                <div class="col-md-1" style="margin-top: 27px">
                    <asp:ImageButton ID="ibtnStartDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                        Width="30px" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <asp:Button ID="btnView" runat="server" CssClass="btn btn-success" OnClick="btnView_Click" Text="View" />
                </div>
            </div>
            <cc1:CalendarExtender ID="calendar1" ClientIDMode="Static" runat="server" Format="MMM-yyyy" PopupButtonID="ibtnStartDate"
                TargetControlID="txtStartDate" OnClientShown="onCalendarShown" OnClientHidden="onCalendarHidden" DefaultView="Months"></cc1:CalendarExtender>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="emp-table">
                    <asp:Panel ID="Panel1" runat="server" Height="250px" ScrollBars="Auto" Width="100%" Style="margin-top: 10px">
                        <asp:GridView ID="GrdLedger" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-striped table-bordered table-hover table-condensed cf"
                            OnRowEditing="GrdLedger_RowEditing">
                            <PagerSettings FirstPageText="" LastPageText="" Mode="NextPrevious" NextPageText="Next"
                                PreviousPageText="Previous" />
                            <Columns>
                                <asp:BoundField DataField="Month" HeaderText="Month" ReadOnly="true">
                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="credit" HeaderText="Debit" ReadOnly="true">
                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="debit" HeaderText="Credit" ReadOnly="true">
                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="120px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="opening" HeaderText="Balance" ReadOnly="true">
                                    <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="200px" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnPrint" runat="server" CssClass="fa fa-print" CommandName="Edit" ToolTip="Print">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="grdDetail" HorizontalAlign="Center" Width="90px" />
                                </asp:TemplateField>
                                <%--<asp:CommandField HeaderText="Print" ShowEditButton="True" EditText="Print">
                                                        <ItemStyle BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" Width="40px" />
                                                    </asp:CommandField>--%>
                            </Columns>
                            <HeaderStyle CssClass="tblhead" />
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
