var audio = new Audio('audio_file2.mp3');
var Modifierparent = [];
var IsPageLoad = true;
function waitLoading() {
    new $.Zebra_Dialog('<strong>Loading</strong>', {
        'buttons': false,
        'position': ['top + 150'],
        'auto_close': 500
    });
}
$(document).ready(function () {   
    waitLoading();
    $('#btnCancelTakeawayOrder').click(function (e) {
        $('#VoidValidation').hide("slow");
    });
    $('#btnVoidTakeawayOrder').click(function (e) {
       
        $.ajax
            ({
                type: "POST", //HTTP method
                url: "frmOrderPOS.aspx/VoidOrder", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ orderId: $("#hfInvoiceId").val(), VoidReasonID: $('select#ddlVoidReason option:selected').val() }),
                error: OnError,
                complete: function () {
                    $('#VoidValidation').hide("slow");
                    GetPendingBills();                    
                }
            });

    });
    $(".nav-ic img").click(function () {
        $(".exit-text").slideToggle("slow", function () {
        });
    });
    GetRejectReason();
    GetPendingBills();
});
//Products Grid---------------------------------------------------------------\\
//-----------------------------------------------------------------------------\\
function GetPendingBills() {
    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmOnlineOrders.aspx/SelectPendingBills", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: LoadPendingBills
                }
            );
}
function LoadPendingBills(pendingBills) {
    pendingBills = JSON.stringify(pendingBills);
    pendingBills = jQuery.parseJSON(pendingBills.replace(/&quot;/g, '"'));
    pendingBills = eval(pendingBills.d);
    if (IsPageLoad) {
        StopSound();
        IsPageLoad = false;
        document.getElementById("hfOldOrdersCount").value = pendingBills.length;
    }
    else {
        if (pendingBills.length > parseInt(document.getElementById("hfOldOrdersCount").value)) {
            PlaySound();
            document.getElementById("hfOldOrdersCount").value = pendingBills.length;
            document.getElementById("hfNotificationStop").value = "0";
        }
        else {
            document.getElementById("hfOldOrdersCount").value = '0';
            if (document.getElementById("hfNotificationStop").value == "0") {
                PlaySound();
            }
            else {
                StopSound();
            }
        }
    }
    $("#tbl-online-orders").empty();
    for (var i = 0, len = pendingBills.length; i < len; i++) {
        var row = $('<tr ><td style="display:none;">' + pendingBills[i].INVOICE_ID + '</td><td class="leftval" style="width:15%;" onclick="StopSound()">' + pendingBills[i].DISTRIBUTOR_NAME + '</td><td class="leftval" style="width:10%;" onclick="StopSound()">' + pendingBills[i].CustomerName + '</td><td class="leftval" style="width:10%;" onclick="StopSound()">' + pendingBills[i].CONTACT_NUMBER + '</td><td class="leftval" style="width:20%;" onclick="StopSound()">' + pendingBills[i].ADDRESS + '</td><td class="leftval" style="width:15%;" onclick="StopSound()">' + pendingBills[i].REMARKS + '</td><td class="leftval" style="width:10%;text-align: center;"><a href="#" onclick="GetOrder(' + pendingBills[i].INVOICE_ID + ');">Print Order</a></td><td class="leftval" style="width:10%;text-align: center;"><a href="#" onclick="ConfirmOrder(' + pendingBills[i].INVOICE_ID + ');">Confirm Order</a></td><td class="leftval" style="width:10%;text-align: center;"><a href="#" onclick="RejectOrder(' + pendingBills[i].INVOICE_ID + ');">Reject Order</a></td></tr>');
        $('#tbl-online-orders').append(row);
    }
}
//Call on Pending Bills Row Click, And on HoldOrder Save When Takeaway
function Error(Msg) {

    $.Zebra_Dialog(Msg, { 'title': 'Error', 'type': 'error' });
}

function ConfirmOrder(OrderID) {
    $.ajax
    (
        {
            type: "POST", //HTTP method
            url: "frmOnlineOrders.aspx/UpdateOrder", //page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ InvoiceId: OrderID }),
            success: OrderUpdated,
            error: function () { Error("An error occurred"); }
        }
    );
}

function OrderUpdated(tblProducts) {    
    alert('Order confirmed.');
    GetPendingBills();
    StopSound();
}

function GetOrder(OrderID)
{
    $.ajax
    (
        {
            type: "POST", //HTTP method
            url: "frmOnlineOrders.aspx/GetOrderData", //page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ InvoiceId: OrderID }),
            success: PrintOrder,
            error: function () { Error("An error occurred"); }
        }
    );
}

function RejectOrder(OrderID)
{
    StopSound();
    $("#hfInvoiceId").val(OrderID);
    $('#btnVoidTakeawayOrder').text('Reject');
    document.getElementById("VoidValidation").style["top"] = "30%";
    $('#lblVoidReason').text('Reject Reason');
    $('#VoidValidation').show("slow");    
}
function PrintOrder(tblProducts) {
    tblProducts = JSON.stringify(tblProducts);
    var result = jQuery.parseJSON(tblProducts.replace(/&quot;/g, '"'));
    tblProducts = eval(result.d);
    Modifierparent = [];
    $('#hfInvoiceNo2').val(0);
    if (tblProducts.length > 0) {
        document.getElementById('hfTableNo').value = tblProducts[0].CUSTOMER_NAME;
        $("#InvoiceTable").text(tblProducts[0].CONTACT_NUMBER);
        for (var i = 0, len = tblProducts.length; i < len; ++i) {
            var obj = {};
            obj["ItemID"] = tblProducts[i].SKU_ID;
            obj["ParentID"] = tblProducts[i].MODIFIER_PARENT_ID;
            obj["ItemName"] = tblProducts[i].SKU_NAME;
            obj["Price"] = tblProducts[i].T_PRICE;;
            obj["Qty"] = tblProducts[i].QTY;
            obj["Amount"] = tblProducts[i].AMOUNT;
            obj["ModifierParetn_Row_ID"] = tblProducts[i].ModifierParetn_Row_ID;
            obj["SALE_INVOICE_DETAIL_ID"] = tblProducts[i].SALE_INVOICE_DETAIL_ID;
            Modifierparent.push(obj);
        }
    }

    if ($('#hfInvoiceFooterType').val() == '1') {
        PrintSaleInvoiceCashCreditCardBoth(tblProducts);
    }
    else {
        PrintSaleInvoice(tblProducts);
    }
    StopSound();
}
//=======Item Less/Cancel Reason
function GetRejectReason() {
    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmOrderPOS.aspx/GetItemLessCancelReason", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: LoadRejectReason,
                    error: OnError
                }
            );
}

function LoadRejectReason(tblReason) {
    tblReason = JSON.stringify(tblReason);
    var result = jQuery.parseJSON(tblReason.replace(/&quot;/g, '"'));
    tblReason = eval(result.d);
    var listItems2 = "";
    for (var i = 0; i < tblReason.length; i++) {
        if (tblReason[i].TYPE_ID === 5) {
            listItems2 += "<option value='" + tblReason[i].REASON_ID + "'>" + tblReason[i].REASON_DESC + "</option>";
        }        
    }
    $("#ddlVoidReason").html(listItems2);
}
function Clear() {
    $('#hfOrderedproducts').val('');
    $("#OrderNo1").text("N/A");
    $("#MaxOrderNo").text("N/A");
    $("#txtCoverTable").val('');
    $("select#ddlOrderBooker").prop('selectedIndex', 0);
}

function ClearOnCancel() {
    $('#hfOrderedproducts').val('');
    $("#hfTableNo").val("");
    $("#OrderNo1").text("N/A");
    $("#MaxOrderNo").text("N/A");
    $("#txtCoverTable").val('');
   $("select#ddlOrderBooker").prop('selectedIndex', 0);
}

//#region Time Calculation

function BillTime() {
    var refresh = $("#hfPendigBillRefreshTime").val()// Refresh rate in milli seconds
    mytime = setTimeout('display_ct()', refresh)    
    GetPendingBills();
}

//--Used in BillTime();
function display_ct() {
    var strcount
    var d = new Date()

    var h = addZero(d.getHours());
    var m = addZero(d.getMinutes());

    document.getElementById('ct').innerHTML = h + ":" + m;
    tt = BillTime();
}

//--Used In display_ct();
function addZero(i) {//Used For Adding 0 Before Hour When Hour is Less than 10
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}

//--Used In GetPendingBills();
function diff(start, end) {//Get The updated Time For Hold Orders 
    start = start.split(":");
    end = end.split(":");
    var startDate = new Date(0, 0, 0, start[0], start[1], 0);
    var endDate = new Date(0, 0, 0, end[0], end[1], 0);
    var diff = endDate.getTime() - startDate.getTime();
    var hours = Math.floor(diff / 1000 / 60 / 60);
    diff -= hours * 1000 * 60 * 60;
    var minutes = Math.floor(diff / 1000 / 60);

    return (hours < 10 ? "0" : "") + hours + ":" + (minutes < 10 ? "0" : "") + minutes;
}

function unique(list) {
    var result = [];
    $.each(list, function (i, e) {
        if ($.inArray(e, result) == -1)
            result.push(e);
    });
    return result;
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
//#endregion Time Calculation
function PlaySound() {
    audio.play();
}
function StopSound() {
    audio.pause();
    audio.currentTime = 0.0;
    document.getElementById("hfNotificationStop").value = "1";
    var table = document.getElementById('tbl-online-orders');
    document.getElementById("hfOldOrdersCount").value = table.rows.length;
}