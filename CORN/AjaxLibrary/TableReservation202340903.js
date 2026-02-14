var TableIDs = [];
var ReservationDates = [];
$(document).ready(function () {
    var lstResevationDate = document.getElementById("hfReservationDate").value;
    lstResevationDate = eval(lstResevationDate);
    for (var i = 0, len = lstResevationDate.length; i < len; ++i) {
        ReservationDates.push(lstResevationDate[i].ReservationDate);
    }
    LoadLocationDropDown();
    document.getElementById('btnShowReservationCancelModal').style.visibility = 'hidden';
    var currentdate = new Date();
    var currentmonth = (currentdate.getMonth() + 1);    
    var dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    var d = new Date(currentdate).getDay();
    $('#lblDate').text(dayNames[d] + ', ' + currentdate.getDate() + '-' + currentmonth + '-' + currentdate.getFullYear());

    $(function () {        
        $("#datepicker").datepicker({            
            onSelect: function (date) {
                var currentdate = new Date(date);
                var currentmonth = (currentdate.getMonth() + 1);
                var dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
                var d = new Date(currentdate).getDay();
                $('#lblDate').text(dayNames[d] + ', ' + currentdate.getDate() + '-' + currentmonth + '-' + currentdate.getFullYear());
                if ($("#btnSave").html() === 'Save') {
                    ClearControls();
                    GetTableReservation();
                }
            },
            dateFormat: 'yy-mm-dd',
            beforeShowDay: noWeekendsOrHolidaysOrBlockedDates
        });
    });            
    $("#tbl-reservation").delegate("tr", "click", function () {
        EditRecrod(this);
    });    
});
function noWeekendsOrHolidaysOrBlockedDates(date) {
    return setHoliDays(date);
}

function setHoliDays(date) {
    var flag = false;
    var date = date.getDate() + "-" + (date.getMonth() + 1) + "-" + date.getFullYear();
    for (var i in ReservationDates) {
        if(ReservationDates[i] == date)
        {
            flag = true;
            break;
        }
    }
    if (flag) {
        return [true, 'specialDate', 'Red'];
    }
    else {
        return [true, ''];
    }
}
function LoadLocationDropDown() {
    $.ajax
       (
           {
               type: "POST", //HTTP method
               url: "frmTableReservation.aspx/LoadLocation", //page/method name
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: LocationDropDown
           }
 );
}
function LocationDropDown(locations) {
    locations = JSON.stringify(locations);
    var result = jQuery.parseJSON(locations.replace(/&quot;/g, '"'));
    locations = eval(result.d);
    
    Locationddl.ClearItems();
    Locationddl.BeginUpdate();
    for (var i = 0, len = locations.length; i < len; i++) {
        Locationddl.AddItem(locations[i].DISTRIBUTOR_NAME, locations[i].DISTRIBUTOR_ID);
    }
    Locationddl.EndUpdate();
    Locationddl.SetSelectedIndex(0);
    GetFloors();
    GetTimeSlot();
    GetTableReservation();
}
function GetFloors() {
    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmTableReservation.aspx/GetFloors", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ DistributorID: Locationddl.GetValue() }),
                    success: LoadFloors,
                    error: OnError
                }
            );
}
function GetTables() {
    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmTableReservation.aspx/GetTables", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ DistributorID: Locationddl.GetValue()}),
                    success: LoadTables,
                    error: OnError
                }
            );
}
function LoadTables(tables) {
    var tables = JSON.stringify(tables);
    var result = jQuery.parseJSON(tables.replace(/&quot;/g, '"'));
    tables = eval(result.d);
    tables = JSON.stringify(tables);
    document.getElementById("hfTables").value = tables;        
}
function LoadFloors(floors) {
    var floors = JSON.stringify(floors);
    var result = jQuery.parseJSON(floors.replace(/&quot;/g, '"'));
    floors = eval(result.d);
    floors = JSON.stringify(floors);
    document.getElementById("hfFloors").value = floors;

    GetTables();
}
function createFloorLabels(value, id) {
    var element = document.createElement("input");
    element.setAttribute("type", "button");
    element.setAttribute("value", value);
    element.setAttribute("name", value);
    element.setAttribute("id", id);
    element.setAttribute("width", 100);
    element.style["margin-bottom"] = "5px";    
    var dv_lstTable = document.getElementById("dv_lstTable");
    dv_lstTable.appendChild(element);    
}
function createTableButtons(value, id) {
    var element = document.createElement("input");
    element.setAttribute("type", "button");
    element.setAttribute("value", value);
    element.setAttribute("name", value);
    element.setAttribute("id", id);
    element.setAttribute("width", 100);
    element.setAttribute("class", "box-last-vc");
    element.onclick = function () {
        changeTableClass(this);
    };
    var dv_lstTable = document.getElementById("dv_lstTable");
    dv_lstTable.appendChild(element);
}
function changeTableClass(btn) {
    var a, n;
    var a = document.getElementById("dv_lstTable").children;    
    if (btn.style["background-color"] == 'rgb(83, 180, 181)') {
        btn.style["background-color"] = "#d4def7";
        for (var i in TableIDs) {
            if (TableIDs[i] == btn.id) {
                TableIDs.splice(i, 1);
                break;
            }
        }
    }
    else {
        btn.style["background-color"] = "#53b4b5";
        TableIDs.push(btn.id);
    }
}
function changeTableClass2(id) {
    var a, n;
    var a = document.getElementById("dv_lstTable").children;
    for (n = 0; n < a.length; n++) {
        if (a[n].id === id.toString()) {
            a[n].style["background-color"] = "#53b4b5";
            break;
        }
    }    
}
function GetTimeSlot() {
    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmTableReservation.aspx/GetTimeSlot", //page/method name
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ DistributorID: Locationddl.GetValue() }),
                    dataType: "json",
                    success: LoadTimeSlot,
                    error: OnError
                }
            );
}
function LoadTimeSlot(timslots) {
    timslots = JSON.stringify(timslots);
    var timslots = jQuery.parseJSON(timslots.replace(/&quot;/g, '"'));
    timslots = eval(timslots.d);
    TimeSlotddl.ClearItems();
    TimeSlotddl.BeginUpdate();
    for (var i = 0, len = timslots.length; i < len; i++) {
        TimeSlotddl.AddItem(timslots[i].TimeSlot, timslots[i].BookingSlotID);
    }
    TimeSlotddl.EndUpdate();
    TimeSlotddl.SetSelectedIndex(0);
}
function storeTblIDs() {
    var tableData = new Array();
    for (var i in TableIDs) {
        tableData[i] = {
            "TableID": TableIDs[i]
        }
    }
    return tableData;
}
function SaveReservation() {
    if ($('#txtGuest').val() == '') {
        Error('Must enter No Of Guests.!');
        return;
    }    
    if (TableIDs.length > 0) {
        var tableData = storeTblIDs();
        tableData = JSON.stringify(tableData);
        document.getElementById('hfTableIDs').value = tableData;
        
        var selecteddate = $("#datepicker").datepicker("getDate");
        if ($("#btnSave").html() === 'Update') {
            $("#btnSave").attr("disabled", true);
            $.ajax
                   ({
                       type: "POST",
                       url: "frmTableReservation.aspx/UpdateReservation",
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       data: JSON.stringify({ TableReservationMasterCode: $('#hfTableReservationMasterID').val(), ReservationDate: selecteddate, ReservatkionTime: Timeddl.GetValue(), NoOfGuest: $('#txtGuest').val(), BookinSlot: TimeSlotddl.GetValue(), Remarks: $('#txtRemarks').val(), Source: Sourceddl.GetValue(), CustomerTypeID: CustomerTypeddl.GetValue(), dtTableIDs: document.getElementById('hfTableIDs').value }),
                       success: ReservationSaved,
                       error: OnError,
                       complete: function () {
                           $("#btnSave").attr("disabled", false);
                           location.reload();
                       }
                   });
        }
        else {
            if ($('#txtName').val() == '') {
                Error('Must enter Name.!');
                return;
            }
            if ($('#txtMobile').val() == '' && $('#txtWhatsApp').val() == '') {
                Error('Mobile No or WhatsApp is required.!');
                return;
            }
            $("#btnSave").attr("disabled", true);
            $.ajax
                   ({
                       type: "POST",
                       url: "frmTableReservation.aspx/SaveReservation",
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       data: JSON.stringify({ LocationID: Locationddl.GetValue(), ReservationDate: selecteddate, ReservatkionTime: Timeddl.GetValue(), NoOfGuest: $('#txtGuest').val(), BookinSlot: TimeSlotddl.GetValue(), Name: $('#txtName').val(), MobileNo: $('#txtMobile').val(), WhatsApp: $('#txtWhatsApp').val(), Email: $('#txtEmail').val(), Remarks: $('#txtRemarks').val(), Source: Sourceddl.GetValue(), CustomerTypeID: CustomerTypeddl.GetValue(), dtTableIDs: document.getElementById('hfTableIDs').value, CustomerID: $('#hfCustomerID').val(), DOB: $('#txtDOB').val() }),
                       success: ReservationSaved,
                       error: OnError,
                       complete: function () {
                           $("#btnSave").attr("disabled", false);
                           location.reload();
                       }
                   });
        }
    }
    else {
        Error('No Table selected.!');
    }
}
function ShowCancelReservationModal()
{
    document.getElementById("myModal").style.display = "block";
    $('#txtCancelReason').focus();
}
function CancelReservation() {
    var UserId = document.getElementById('txtUserID').value;
    var UserPassword = document.getElementById('txtPassword').value;
    if (UserId.length > 0 && UserPassword.length > 0) {
        UserValidationInDataBase();
    }
    else {
        Error('Enter User ID and Password!')
    }
}
function CancelReson() {
    document.getElementById("myModal").style.display = "none";
}
function ReservationSaved() {
    SucessMsg('Table reservation saved successfully.!')
    ClearControls();
    GetTableReservation();
}
function ClearControls()
{
    var a, n;
    var a = document.getElementById("dv_lstTable").children;
    for (n = 0; n < a.length; n++) {
        a[n].style["background-color"] = "#d4def7";
    }

    TableIDs = [];
    $('#hfCustomerID').val(0);
    $('#txtGuest').val('');
    $('#txtName').val('');
    $('#txtMobile').val('');
    $('#txtWhatsApp').val('');
    $('#txtEmail').val('');
    $('#txtRemarks').val('');
    $('#txtCancelReason').val('')
    $('#txtDOB').val('')
    $("#btnSave").html('Save');
    $('#hfTableReservationMasterID').val('0');
    $("#txtName").removeAttr("disabled");
    $("#txtMobile").removeAttr("disabled");
    $("#txtWhatsApp").removeAttr("disabled");
    $("#txtEmail").removeAttr("disabled");
    $("#txtDOB").removeAttr("disabled");
    document.getElementById('btnShowReservationCancelModal').style.visibility = 'hidden';
    LoadAvailableTables();
}
function GetTableReservation() {
    var selecteddate = $("#datepicker").datepicker("getDate");
    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmTableReservation.aspx/GetTableReservation",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ DistributorID: Locationddl.GetValue(), ReservationDate: selecteddate }),
                    success: LoadTableReservation,
                    error: OnError
                }
            );
}
function LoadTableReservation(tblReservation) {
    var tblReservation2 = JSON.stringify(tblReservation);
    var result = jQuery.parseJSON(tblReservation2.replace(/&quot;/g, '"'));
    tblReservation2 = eval(result.d);
    tblReservation2 = JSON.stringify(tblReservation2);
    document.getElementById("hfReservation").value = tblReservation2;

    tblReservation = JSON.stringify(tblReservation);
    var tblReservation = jQuery.parseJSON(tblReservation.replace(/&quot;/g, '"'));
    tblReservation = eval(tblReservation.d);

    $("#tbl-reservation").empty();
    $("#tbl-TableDetail").empty();
    var OldTime = '';
    var row = "";
    var row2 = "";
    var bookings = 0;
    var guests = 0;

    for (var i = 0, len = tblReservation.length; i < len; i++) {        
        if (tblReservation[i].Status == 'Reserved') {
            guests += parseInt(tblReservation[i].NoOfGuest);
            bookings++;
            if (OldTime === '') {
                row = $('<tr style="cursor:pointer;"><td style="display:none;">' + tblReservation[i].lngTableReservationMasterCode + '</td><td colspan="2"><span style="font-weight:bold;">' + tblReservation[i].TimeSlot + '</span>: ' + tblReservation[i].CUSTOMER_NAME + ' ' + tblReservation[i].NoOfGuest + ' <span style="color:red;font-style:italic;"> Pers</span></td></tr>');
            }
            else {
                if (OldTime === tblReservation[i].TimeSlot) {
                    row = $('<tr style="cursor:pointer;"><td style="display:none;">' + tblReservation[i].lngTableReservationMasterCode + '</td><td style="width:22%;"></td><td>' + tblReservation[i].CUSTOMER_NAME + ' ' + tblReservation[i].NoOfGuest + ' <span style="color:red;font-style:italic;"> Pers</span></td></tr>');
                }
                else {
                    row = $('<tr style="cursor:pointer;"><td style="display:none;">' + tblReservation[i].lngTableReservationMasterCode + '</td><td  colspan="2"><span style="font-weight:bold;">' + tblReservation[i].TimeSlot + '</span>: ' + tblReservation[i].CUSTOMER_NAME + ' ' + tblReservation[i].NoOfGuest + ' <span style="color:red;font-style:italic;"> Pers</span></td></tr>');
                }
            }
            OldTime = tblReservation[i].TimeSlot;
            $("#tbl-reservation").append(row);
        }
        row2 = $('<tr style="cursor:pointer;"><td style="display:none;">' + tblReservation[i].lngTableReservationMasterCode + '</td><td>' + tblReservation[i].TableNo + '</td><td>' + tblReservation[i].CUSTOMER_NAME + '</td><td>' + tblReservation[i].CONTACT_NUMBER + '</td><td>' + tblReservation[i].ReservationDate2 + '</td><td>' + tblReservation[i].TimeSlot + '</td><td>' + tblReservation[i].NoOfGuest + '</td><td>' + tblReservation[i].Remarks + '</td><td>' + tblReservation[i].Status + '</td><td>' + tblReservation[i].USER_NAME + '</td></tr>');
        $("#tbl-TableDetail").append(row2);
    }
    $('#lblTotalBookings').text('0 bookings, 0 guests');
    if (guests > 0) {
        $('#lblTotalBookings').text(bookings + ' bookings, ' + guests + ' guests');
    }

    GetReseveredTable();    
}
function GetReseveredTable() {
    var selecteddate = $("#datepicker").datepicker("getDate");
    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmTableReservation.aspx/GetReseveredTable",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ DistributorID: Locationddl.GetValue(), ReservationDate: selecteddate }),
                    success: LoadReseveredTable,
                    error: OnError
                }
            );
}
function LoadReseveredTable(tblReservedTable) {
    var tblReservedTable = JSON.stringify(tblReservedTable);
    var result = jQuery.parseJSON(tblReservedTable.replace(/&quot;/g, '"'));
    tblReservedTable = eval(result.d);
    tblReservedTable = JSON.stringify(tblReservedTable);
    document.getElementById("hfReservedTable").value = tblReservedTable;

    LoadAvailableTables();
}
function EditRecrod(tr) {
    var TableReservationMasterID  = $(tr).find("td:eq(0)").text();
    var lstResevation = document.getElementById("hfReservation").value;
    lstResevation = eval(lstResevation);
    for (var i = 0, len = lstResevation.length; i < len; ++i) {
        if (lstResevation[i].lngTableReservationMasterCode.toString() === TableReservationMasterID) {
            $('#hfTableReservationMasterID').val(TableReservationMasterID);
            $('#txtName').val(lstResevation[i].CUSTOMER_NAME);
            $('#txtMobile').val(lstResevation[i].CONTACT_NUMBER);
            $('#txtWhatsApp').val(lstResevation[i].WHATSAPP_NO);
            $('#txtEmail').val(lstResevation[i].EMAIL_ADDRESS);
            $('#txtRemarks').val(lstResevation[i].Remarks);
            $('#txtGuest').val(lstResevation[i].NoOfGuest);
            $('#txtDOB').val(lstResevation[i].DOB);
            $("#btnSave").html('Update');
            TimeSlotddl.SetValue(lstResevation[i].BooingSlot);
            Sourceddl.SetValue(lstResevation[i].SourceID);
            CustomerTypeddl.SetValue(lstResevation[i].CustomerTypeID);
            $("#txtName").attr("disabled", "disabled");
            $("#txtMobile").attr("disabled", "disabled");
            $("#txtWhatsApp").attr("disabled", "disabled");
            $("#txtEmail").attr("disabled", "disabled");
            $("#txtDOB").attr("disabled", "disabled");
            document.getElementById('btnShowReservationCancelModal').style.visibility = 'visible';
            break;
        }
    }

        $.ajax
                (
                    {
                        type: "POST", //HTTP method
                        url: "frmTableReservation.aspx/GetTableReservationDetail",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({ TableReservationMasterCode: TableReservationMasterID }),
                        success: LoadTableReservationDetail,
                        error: OnError
                    }
                );
    
}
function LoadTableReservationDetail(tblReservation) {
    tblReservation = JSON.stringify(tblReservation);
    var tblReservation = jQuery.parseJSON(tblReservation.replace(/&quot;/g, '"'));
    tblReservation = eval(tblReservation.d);
    LoadAvailableTables();
    TableIDs = [];
    var a, n;
    var a = document.getElementById("dv_lstTable").children;
    for (n = 0; n < a.length; n++) {
        a[n].style["background-color"] = "#d4def7";
    }    
    for (var i = 0, len = tblReservation.length; i < len; i++) {
        TableIDs.push(tblReservation[i].TableID);
        changeTableClass2(tblReservation[i].TableID);
    }
}
function LoadAvailableTables() {
    var lstFloors = document.getElementById("hfFloors").value;
    lstFloors = eval(lstFloors);

    var lstTables = document.getElementById("hfTables").value;
    lstTables = eval(lstTables);

    var lstReservedTable = document.getElementById("hfReservedTable").value;
    lstReservedTable = eval(lstReservedTable);

    var dv_lstTable = document.getElementById("dv_lstTable");
    while (dv_lstTable.hasChildNodes()) {
        dv_lstTable.removeChild(dv_lstTable.lastChild);
    }

    for (var ii = 0, lenn = lstFloors.length; ii < lenn; ++ii) {
        createFloorLabels(lstFloors[ii].FloorName, lstFloors[ii].FloorID);
        if ($("#btnSave").html() === 'Save') {//All Available Tables
            for (var i = 0, len = lstTables.length; i < len; ++i) {
                if (lstFloors[ii].FloorID == lstTables[i].FloorID) {
                    if (IsTableAvailable(lstReservedTable, lstTables[i].TABLE_ID)) {
                        createTableButtons(lstTables[i].TABLE_NO, lstTables[i].TABLE_ID);
                    }
                }
            }
        }
        else//All Available Tables + Editing Reservation Tables
        {
            for (var i = 0, len = lstTables.length; i < len; ++i) {
                if (lstFloors[ii].FloorID == lstTables[i].FloorID) {
                    {
                        if (IsTableAvailable(lstReservedTable, lstTables[i].TABLE_ID)) {
                            createTableButtons(lstTables[i].TABLE_NO, lstTables[i].TABLE_ID);
                        }
                    }
                }
            }
        }
    }
}
function IsTableAvailable(lstReservedTable, TableID)
{
    var flag = true;
    for (var i = 0, len = lstReservedTable.length; i < len; ++i) {
        if (lstReservedTable[i].TableID === TableID && lstReservedTable[i].BooingSlot == TimeSlotddl.GetValue() && lstReservedTable[i].lngTableReservationMasterCode.toString() != $('#hfTableReservationMasterID').val())
        {
            flag = false;
            break;
        }
    }
    return flag;
}
function CopyToWahtsApp()
{
    $('#txtWhatsApp').val($('#txtMobile').val());
}
function LocationChanged(SelectedValue) {
    GetFloors();
    GetTimeSlot();
    GetTables();
    GetTableReservation();
    GetTimeSlot();    
}
function TimeSlotChanged(SelectedValue) {
    LoadAvailableTables();
}
function SearchCustomer()
{
    if($('#txtMobile').val().length > 0)
    {
        $.ajax
      (
          {
              type: "POST", //HTTP method
              url: "frmTableReservation.aspx/SearchCustomer", //page/method name
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              data: JSON.stringify({ FieldValue: $('#txtMobile').val(), FieldName: 'MobileNo' }),
              success: LoadSearchCustomer
          }
);
    }
    else {
        Error('Enter Mobile No.!');
    }
}
function LoadSearchCustomer(customers) {
    $('#hfCustomerID').val(0);
    $('#txtName').val('');
    $('#txtWhatsApp').val('');
    $('#txtEmail').val('');
    $('#lblNoOfVisits').text('No Of Visits: 0');
    $('#txtDOB').val('');    
    customers = JSON.stringify(customers);
    var result = jQuery.parseJSON(customers.replace(/&quot;/g, '"'));
    customers = eval(result.d);
    if(customers.length > 0)
    {
        $('#hfCustomerID').val(customers[0].CUSTOMER_ID);
        $('#txtName').val(customers[0].CUSTOMER_NAME);
        $('#txtWhatsApp').val(customers[0].WHATSAPP_NO);
        $('#txtEmail').val(customers[0].EMAIL_ADDRESS);
        $('#lblNoOfVisits').text('No Of Visits: ' + customers[0].NoOfVisits);
        if (customers[0].REGDATE.length > 0) {
            $('#txtDOB').val(customers[0].REGDATE);
        }
        $("#txtName").attr("disabled", "disabled");
        $("#txtWhatsApp").attr("disabled", "disabled");
        $("#txtEmail").attr("disabled", "disabled");        
    }
    else {
        $("#txtName").removeAttr("disabled");
        $("#txtWhatsApp").removeAttr("disabled");
        $("#txtEmail").removeAttr("disabled");        
    }
}
function UserValidationInDataBase() {
    var UserId = document.getElementById('txtUserID').value;
    var UserPassword = document.getElementById('txtPassword').value;    
    $.ajax
        (
            {
                type: "POST", //HTTP method
                url: "frmTableReservation.aspx/ValidateUser", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ UserId: UserId, UserPass: UserPassword, UserAction: 'CanCancelTableReservation' }),
                success: UserValidated,
                error: function () { Error("Authorization Failed!"); }
            }
        );
}
function UserValidated(UserAuth) {
    UserAuth = JSON.stringify(UserAuth);
    var result = jQuery.parseJSON(UserAuth.replace(/&quot;/g, '"'));
    UserAuth = eval(result.d);
    if (UserAuth != null) {
        if (UserAuth.length > 0) {
            $('#hfCancelledBy').val(UserAuth[0].USER_ID);
            document.getElementById("myModal").style.display = "none";
            $("#btnCancelReservation").attr("disabled", true);
            $.ajax
                   ({
                       type: "POST",
                       url: "frmTableReservation.aspx/CancelReservation",
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       data: JSON.stringify({ TableReservationMasterCode: $('#hfTableReservationMasterID').val(), CancelationReason: $('#txtCancelReason').val(), CancelledBy: $('#hfCancelledBy').val() }),
                       success: ReservationCancelled,
                       error: OnError,
                       complete: function () {
                           $("#btnCancelReservation").attr("disabled", false);
                       }
                   });
        }
        else {
            Error('This user can not Cancel Reservation!');
        }
    }
    else {
        Error('This user can not Cancel Reservation!');
    }
}
function ReservationCancelled() {
    SucessMsg('Table reservation cancelled successfully.!')
    location.reload();
}
function OnError(xhr, errorType, exception) {
    var responseText;
    try {
        responseText = jQuery.parseJSON(xhr.responseText);
        $.Zebra_Dialog(responseText.Message, { 'title': 'Error', 'type': 'error' });

    } catch (e) {
        responseText = xhr.responseText;
        $.Zebra_Dialog(responseText, { 'title': 'Error', 'type': 'error' });
    }

}
function Error(Msg) {

    $.Zebra_Dialog(Msg, { 'title': 'Error', 'type': 'error' });
}
function SucessMsg(Msg) {

    $.Zebra_Dialog(Msg, { 'title': 'Confirmation', 'type': 'confirmation' });
}
function onlyNumbers(txt, event) {
    var charCode = (event.which) ? event.which : event.keyCode;

    if (charCode == 9 || charCode == 8) {
        return true;
    }
    if (charCode == 46) {
        return false;
    }
    if (charCode == 13) {
        if ($("#hfKeyBoadTextBoxID").val() == "txtCashRecieved") {
            event.preventDefault();
            $('.Custombtn23').click();
        }
        return true;
    }

    if (charCode == 31 || charCode < 48 || charCode > 57)
        return false;
    return true;
}