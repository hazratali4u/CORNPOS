<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeFile="frmTableReservation.aspx.cs" Inherits="Forms.frmTableReservation" EnableEventValidation="false" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<!doctype html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" charset="utf-8" />
    <meta name="robots" content="noindex, nofollow" />
    <title>CORN :: Table Reservation</title>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.13.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/zebra_dialog.css" rel="stylesheet" type="text/css" />
    <link href="../css/tree-style.css" rel="stylesheet" type="text/css" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="icon" type="image/x-icon" href="../images/icon.ico">
    <link href="../css/calender.css" rel="stylesheet" type="text/css" />
    <link href="../css/CustomKeyBoard.css" rel="stylesheet" type="text/css" />
    <style>
        body {
    line-height: 1.0;
}
        html {
            height: 100vh;
            overflow: hidden;
        }

        .container {
            margin-top: 1vh;
            margin-bottom: 0px !important;
            height: 100vh;
            min-height: 98vh;
            max-height: 98vh;
        }

        .bg-product {
            height: 43vh;
        }

        .box-last-vc {
            width: 70px;
        }

        .ui-datepicker {
            width: 21em;
        }

        .modal {
            display: none;
            position: fixed;
            z-index: 1;
            padding-top: 100px;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: #00000026;
        }

        /* Modal Content */
        .modal-content {
            position: relative;
            background-color: #fefefe;
            margin: auto;
            padding: 0;
            width: 42%;
            max-width: 1200px;
            padding: 20px;
        }

        .specialDate {
            background-color: red !important;
        }

            .specialDate a {
                background: red !important;
            }
    </style>
</head>
<body>
    <form id="Form1" method="POST" runat="server">
        <asp:ScriptManager ID="MainScriptManager" runat="server" AsyncPostBackTimeout="30000" EnablePartialRendering="true" />
        <asp:HiddenField ID="hfTableIDs" runat="server" />
        <asp:HiddenField ID="hfReservation" runat="server" />
        <asp:HiddenField ID="hfReservationDate" runat="server" />
        <asp:HiddenField ID="hfTableReservationMasterID" runat="server" Value="0" />
        <asp:HiddenField ID="hfReservedTable" runat="server" />
        <asp:HiddenField ID="hfTables" runat="server" />
        <asp:HiddenField ID="hfFloors" runat="server" />
        <asp:HiddenField ID="hfCustomerID" runat="server" Value="0" />
        <asp:HiddenField ID="hfCancelledBy" runat="server" Value="0" />
            <div class="container" style="height: 120vh;min-height:118vh;max-height: 118vh;margin-top: 0vh;">
                <div class="s1-left">
                    <div class="logom logo_main" style="width: 10%;">
                        <img id="imgLogo" src="../images/watch.png" runat="server" alt="../images/watch.png" />
                        <input type="text" id="username" style="width: 0; height: 0; visibility: hidden; position: absolute; left: 0; top: 0" />
                        <input type="password" style="width: 0; height: 0; visibility: hidden; position: absolute; left: 0; top: 0" />
                    </div>
                    <div class="s1-right" style="width: 25%;">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 85%; vertical-align: top">
                                    <div class="user-detail" style="width: 95%;">
                                        Welcome - <span class="user-detail-bold" id="user-detail-bold">
                                            <asp:Literal ID="lblUserName" runat="server" Text="Label"></asp:Literal></span><br />
                                        <asp:Literal ID="lblDateTime" runat="server" Text="Label"></asp:Literal>
                                    </div>
                                </td>
                                <td style="width: 15%; vertical-align: top;">
                                    <div class="nav-ic">
                                        <img src="../images/nav-opt.png" alt="Nav" />
                                        <div class="exit-text">
                                            <asp:LinkButton runat="server" ID="lnkExit" OnClick="lnkExit_OnClick">Exit</asp:LinkButton>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="s1-center">
                        <div class="clear"></div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="col-md-3">
                        <div class="bg-product scrolla col" style="padding-bottom: 0px; height: 70vh;">
                            <br />
                            <div id="datepicker"></div>
                            <br />
                            <div id="dvRecords">
                                <table id="tbl-reservation" style="width: 100%;">
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1">
                        <div class="bg-product scrolla col" style="padding-bottom: 0px; height: 70vh; width: 115px;">
                            <div id="dv_lstTable">
                            </div>
                        </div>
                    </div>
                    <div class="col-md-8">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 100%;" colspan="2">
                                    <strong style="font-size: large;">
                                        <asp:Label ID="lblTotalBookings" runat="server">0 bookins, 0 guests</asp:Label>
                                    </strong>
                                </td>
                            </tr>
                            <tr style="height: 20px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 12%;">
                                    <strong>Location:</strong>
                                </td>
                                <td style="width: 88%;">
                                    <dx:ASPxComboBox ID="ddlLocation" runat="server" CssClass="form-control" ClientInstanceName="Locationddl" Width="38%"
                                        ClientSideEvents-SelectedIndexChanged="function(s, e) { LocationChanged(s.GetSelectedItem().value); }">
                                        <Items>
                                            <dx:ListEditItem Value="1" Text="Loading.." Selected="true" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr style="height: 20px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 12%;">
                                    <strong>Arrival Date :</strong>
                                </td>
                                <td style="width: 88%;">
                                    <strong>
                                        <asp:Label ID="lblDate" runat="server"></asp:Label>
                                    </strong>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 12%;">
                                    <strong>Bookings Slot</strong>
                                </td>
                                <td style="width: 20%;">
                                    <dx:ASPxComboBox ID="ddlTimeSlot" runat="server" CssClass="form-control" ClientInstanceName="TimeSlotddl" Width="80%"
                                        ClientSideEvents-SelectedIndexChanged="function(s, e) { TimeSlotChanged(s.GetSelectedItem().value); }">
                                        <Items>
                                            <dx:ListEditItem Value="1" Text="Loading.." Selected="true" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                                <td style="width: 5%;">
                                    <strong>Guest</strong>
                                </td>
                                <td style="width: 10%;">
                                    <asp:TextBox ID="txtGuest" runat="server" Width="88%" onkeypress="return onlyNumbers(this,event);"></asp:TextBox>
                                </td>
                                <td style="width: 53%;">

                                </td>
                            </tr>
                            <tr style="display:none;">
                                <td style="width: 15%;">
                                    <dx:ASPxComboBox ID="ddlTime" runat="server" CssClass="form-control" ClientInstanceName="Timeddl" Width="80%">
                                        <Items>
                                            <dx:ListEditItem Text="12:00AM" Value="12:00AM" Selected="true" />
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
                                            <dx:ListEditItem Text="12:00PM" Value="12:00PM" />
                                            <dx:ListEditItem Text="12:30PM" Value="12:30PM" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                                <td style="width: 10%;">
                                    <strong>Bookings Slot</strong>
                                </td>
                                <td style="width: 20%;">
                                    
                                </td>
                                <td style="width: 5%;">
                                    <strong>Guest</strong>
                                </td>
                                <td style="width: 10%;">
                                    
                                </td>

                                <td style="width: 10%;"></td>
                            </tr>
                        </table>
                        <table style="width: 60%;">
                            <tr style="height: 10px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                    <strong>
                                        <asp:Label ID="lblMobile" runat="server">Mobile No:</asp:Label></strong>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="txtMobile" runat="server" Width="70%" onblur="CopyToWahtsApp();" MaxLength="11"></asp:TextBox>
                                    &nbsp;
                                    <button class="btn btn-toolbar" type="button" style="min-width: 50px; margin-left: 5px; color: #FFF; background-color: #3c8d75;" id="btnSearch" onclick="SearchCustomer();">Search</button>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                    <strong>
                                        <asp:Label ID="lblName" runat="server">Name:</asp:Label></strong>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="txtName" runat="server" Width="70%"></asp:TextBox>
                                    &nbsp;<strong><asp:Label ID="lblNoOfVisits" runat="server">No Of Visits: 0</asp:Label></strong>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                    <strong>
                                        <asp:Label ID="lblWhatsApp" runat="server">WhatsApp No:</asp:Label></strong>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="txtWhatsApp" runat="server" Width="70%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                    <strong>
                                        <asp:Label ID="lblEmail" runat="server">Email:</asp:Label></strong>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="txtEmail" runat="server" Width="70%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                    <strong>
                                        <asp:Label ID="lblDOB" runat="server">Date Of Birth:</asp:Label>
                                    </strong>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox ID="txtDOB" runat="server" Width="70%" placeholder="dd-mm-yyyy"></asp:TextBox>                                                                        
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                    <strong>
                                        <asp:Label ID="lblCustomerType" runat="server">Customer Type:</asp:Label></strong>
                                </td>
                                <td style="width: 80%;">
                                    <dx:ASPxComboBox ID="ddlCustomerType" runat="server" CssClass="form-control" ClientInstanceName="CustomerTypeddl" Width="80%">
                                        <Items>
                                            <dx:ListEditItem Text="Walking Customer" Value="1" Selected="true" />
                                            <dx:ListEditItem Text="Reserved" Value="2" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                    <strong>
                                        <asp:Label ID="lblSource" runat="server">Source:</asp:Label></strong>
                                </td>
                                <td style="width: 80%;">
                                    <dx:ASPxComboBox ID="ddlSource" runat="server" CssClass="form-control" ClientInstanceName="Sourceddl" Width="80%">
                                        <Items>
                                            <dx:ListEditItem Text="Facebook" Value="1" Selected="true" />
                                            <dx:ListEditItem Text="Instagram" Value="2" />
                                            <dx:ListEditItem Text="Email" Value="3" />
                                            <dx:ListEditItem Text="Website" Value="4" />
                                            <dx:ListEditItem Text="Reference" Value="5" />
                                            <dx:ListEditItem Text="Personal" Value="6" />
                                            <dx:ListEditItem Text="Phone Call" Value="7" />
                                        </Items>
                                    </dx:ASPxComboBox>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                    <strong>
                                        <asp:Label ID="lblRemarks" runat="server">Remarks:</asp:Label></strong>
                                </td>
                                <td style="width: 80%;">
                                    <asp:TextBox TextMode="MultiLine" ID="txtRemarks" runat="server" Rows="3" Width="92%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 10px;">
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 20%;"></td>
                                <td style="width: 80%;">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 30%;">
                                                <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #3c8d75;" id="btnSave" onclick="SaveReservation();">Save</button>
                                            </td>
                                            <td style="width: 30%;">
                                                <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #f94d72;" id="btnCancel" onclick="ClearControls();">Cancel</button>
                                            </td>
                                            <td style="width: 40%;">
                                                <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #f94d72;" id="btnShowReservationCancelModal" onclick="ShowCancelReservationModal();">Cancel Reservation</button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>                    
                </div>              
                <div class="col-md-12">
                    <div class="table-bar" style="width: 100%;">
                        <div class="emp-table scrolla col" style="width: 100%;height: 15vh">
                            <table class="table table-striped table-bordered table-hover table-condensed cf">
                                <thead class="cf head" style="background-color: #7dab49;">
                                    <tr>
                                        <th class="numeric table-text-head">Table No
                                        </th>
                                        <th class="numeric table-text-head">Customer Name
                                        </th>
                                        <th class="numeric table-text-head">Contact No
                                        </th>
                                        <th class="numeric table-text-head">Date
                                        </th>
                                        <th class="numeric table-text-head">Booking Slot
                                        </th>
                                        <th class="numeric table-text-head">No Of Guests
                                        </th>
                                        <th class="numeric table-text-head">Remarks
                                        </th>
                                        <th class="numeric table-text-head">Status
                                        </th>
                                        <th class="numeric table-text-head">Reserved By
                                        </th>
                                    </tr>
                                </thead>
                                <tbody id="tbl-TableDetail">
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>  
                <div id="myModal" class="modal">
                    <div class="modal-content">
                        <div class="row">
                            <div class="col-md-4">
                                <strong>Reason Of Cancelation:</strong>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtCancelReason" runat="server" Rows="5" TextMode="MultiLine" Width="100%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                               <strong>User ID:</strong>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtUserID" runat="server" Width="100%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                               <strong>Password:</strong>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtPassword" runat="server" Width="100%" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                
                            </div>
                            <div class="col-md-8">
                                <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #3c8d75;" id="btnCancelReservation" onclick="CancelReservation();">Save</button>
                                <button class="btn btn-toolbar" type="button" style="min-width: 100px; font-size: 15px; color: #FFF; background-color: #f94d72; margin-left: 85px;" id="btnCancelReason" onclick="CancelReson();">Cancel</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        <script src="../js/jquery-1.10.2.js"></script>
        <script src="../js/jquery-2.0.2.min.js"></script>
        <script src="https://code.jquery.com/jquery-3.6.0.js"></script>
        <script src="https://code.jquery.com/ui/1.13.1/jquery-ui.js"></script>
        <script type="text/javascript" src="../AjaxLibrary/moment-with-locales.js"></script>
        <script type="text/javascript" src="../AjaxLibrary/jQuery.print.js"></script>
        <script type="text/javascript" src="../AjaxLibrary/zebra_dialog.js"></script>
        <script src="../js/plugins/Block/jquery.blockUI.js"></script>
        <script type="text/javascript" src="../AjaxLibrary/TableReservation202340903.js"></script>
    </form>
</body>
</html>
