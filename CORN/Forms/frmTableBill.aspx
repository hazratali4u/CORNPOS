<%@ Page Title="CORN :: Table Bill" Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true" CodeFile="frmTableBill.aspx.cs" Inherits="Forms_frmTableBill" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPage" Runat="Server">
    <script src="../js/jquery-2.0.2.min.js" type="text/javascript"></script>
    <script src="../AjaxLibrary/Forms/TableBill.js" type="text/javascript"></script>

    <script language="JavaScript" type="text/javascript">
        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }        function ValidateForm() {

            return true;
        }

    </script>

    <style>
        .table-boxes {
            display: flex;
        }
        .table-boxes > input[type=button] {
            /*flex: 1;
            display:inline-block;
            color:#444;
            border:1px solid #CCC;
            background:#DDD;
            box-shadow: 0 0 5px -1px rgba(0,0,0,0.2);
            cursor:pointer;
            vertical-align:middle;
            max-width: 100px;
            padding: 5px;
            text-align: center;
            margin: auto;*/

            width: 30.4%;
            background-color: #d4def7;
            float: left;
            padding: 10px 5px;
            border: 1px #3e9b9c;
            border-radius: 5px;
            margin-left: 5px;
            margin-right: 5px;
            margin-bottom: 10px;
            font-size: 13px;
            color: #434040;
            text-align: center;
            line-height: 18px;
        }

        .table-boxes > div:active {
            color:red;
            box-shadow: 0 0 5px -1px rgba(0,0,0,0.6);
        }

    </style>
    
    <div id="blocker">
        <div><img src="../OrderPOS/images/wheel.gif"></div>
    </div>
    <asp:HiddenField ID="hidDistributorId" runat="server" />
    <div class="container">
        <div class="row" style="display: none;">
            <div class="col-md-4">
                <label><span class="fa fa-caret-right rgt_cart"></span>Date</label>
                <asp:TextBox ID="txtDate" runat="server" ReadOnly="true"
                    CssClass="form-control" MaxLength="10"></asp:TextBox>
            </div>
            <div class="col-md-1" style="margin-top: 27px">
                <asp:ImageButton ID="ibnDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif" Width="30px" />
                 <cc1:CalendarExtender ID="calExtDate" runat="server" 
                     PopupButtonID="ibnDate" 
                     TargetControlID="txtDate" 
                     Format="dd-MMM-yyyy"
                     PopupPosition="TopLeft"
                     OnClientShown="calendarShown" />
            </div>
            <div class="col-md-7" style="margin-top: 26px">
                <input id="btnGetTables" value="Ok" type="button" class="btn btn-success" />
            </div>
        </div>
        <div class="row" style="display: none;">
            <div class="col-md-12">
                <hr />
            </div>
        </div>
        <div class="row">
            <div class="col-md-1"></div>
            <div class="col-md-10">
                <div id="dvDiningTables" class="table-boxes"></div>
            </div>
            <div class="col-md-1"></div>
        </div>
        <div class="row" style="margin-top: 22px;">
            <div class="col-md-2"></div>
            <div id="dvDiningTableBillReport" class="col-md-8" style="display: none">
                <table cellpadding="3" cellspacing="3" width="100%">
                    <tr>
                        <td colspan="2" style="text-align: center">
                            <label style="font-weight: bold">Provisional Bill</label>
                        </td>
                    </tr>
                    <tr>
                        <td id="table_billDate" colspan="2" style="text-align: center; font-weight: bold; font-size: 16px">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: left; font-weight: bold; font-size: 16px">
                            Dine In
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 10%">
                            <label style="font-weight: bold">O-T :</label>
                        </td>
                        <td>
                            <label id="table_orderTakerName"></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 10%">
                            <label style="font-weight: bold">Table :</label>
                        </td>
                        <td>
                            <label id="table_id"></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; width: 10%">
                            <label style="font-weight: bold">Table No :</label>
                        </td>
                        <td>
                            <label id="table_no"></label>
                        </td>
                    </tr>
                </table>

                <table class="table table-bordered table-hover">
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>Item Name</th>
                            <th>Quantity</th>
                            <th>Rate</th>
                            <th>Amount</th>
                        </tr>
                    </thead>
                    <tbody id="dvDiningTableBillDetail"></tbody>
                </table>

                <%--<div style="width: 100%; padding: 10px 5px 10px 10px; text-align: right;">
                    <input id="btnBillPaid" value="Paid" type="button" class="btn btn-success" />
                </div>--%>
            </div>
            <div class="col-md-2"></div>
        </div>
    </div>

    <%--Start:Templates--%>
    <div style="display: none;">
        <%--<div class="col-md-2 TableButtonTemplate">
            <input type="button" class="table-btn" />
        </div>--%>

        
        <div class="DiningTableBillDetailTemplate">
            <tr>
                <th scope="row">1</th>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </div>
    </div>
    <%--End:Templates--%>

</asp:Content>

