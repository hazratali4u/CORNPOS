function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
$('#btnShowReports').click(function (e) {    
    const ddlFromDate = document.getElementById('ddlFromDate');
    ddlFromDate.options.length = 0;
    const today = new Date($("#hfCurrentWorkDate").val());
    for (let i = 0; i < 31; i++) {
        const date = new Date($("#hfCurrentWorkDate").val());
        date.setDate(today.getDate() - i);

        const day = String(date.getDate()).padStart(2, '0');
        const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun","Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        const month = months[date.getMonth()];
        const year = date.getFullYear();
        // Final format: 01-Sep-2025
        const formattedDate = `${day}-${month}-${year}`;
        const option = document.createElement('option');
        option.value = formattedDate;
        option.textContent = formattedDate;

        ddlFromDate.appendChild(option);
    }

    $('#txtCreditCardAccountTile').val('');
    $('#hfOrderedproducts').val('');
    NewOrder();
    if (IsUserValidationPopup == 1) {
        return;
    }
    if (document.getElementById('hfReport').value != "") {

        $('#reports-buttons').lightbox_me({
            centered: true,
            onLoad: function () {
                $('#reports-buttons').find('input:first').focus();
            }
        });

        var obj = jQuery.parseJSON(document.getElementById('hfReport').value);

        for (var i = 0; i < obj.length; i++) {
            var item = obj[i];
            $("#reports-buttons").find("#" + item.MODULE_KEY).css("display", "block");
        }

    }
    else {
        waitLoading('No report assign');

    }

    if ($('#hfShowDatesOnPOSReports').val() == '1') {
        $("#controls-sales-summary").css("display", "block");
    }
    else {
        $("#controls-sales-summary").css("display", "none");
    }

    e.preventDefault();
});
$('#sales-summary-2').click(function (e) {

    $.ajax
               (
                   {
                       type: "POST", //HTTP method
                       url: "frmOrderPOS.aspx/SelectSalesSummary", //page/method name
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       data: JSON.stringify({ fromDate: document.getElementById("ddlFromDate").value, toDate: document.getElementById("ddlFromDate").value }),
                       async: false,
                       success: ShowSalesSummary2
                   }
               );
});
function ShowSalesSummary2(data) {
    $('#delivery-channel-data').empty();

    data = $.parseJSON(data.d);
    var lstDCData = data.Table1;
    var lstData = data.Table2;
    var lstComplimentary = data.Table3;
    var tbl5 = data.Table4;
    data = data.Table;
    if (data.length > 0) {
        var currentdate = new Date();
        var datetime = currentdate.getDate() + "/"
                        + (currentdate.getMonth() + 1) + "/"
                        + currentdate.getFullYear() + " "
                        + moment().format('hh:mm A')
        var total = 0;
        if (lstDCData.length > 0) {
            for (var i = 0, len = lstDCData.length; i < len; ++i) {
                var row = $(' <tr><td style="width:1.5in;"><strong>' + lstDCData[i].ChannelType + '</strong></td><td><span>: ' + lstDCData[i].NetAmunt + '</span></td></tr>');
                $('#delivery-channel-data').append(row);
                if (lstDCData[i].Cash_Impact === false) {
                    total += lstDCData[i].NetAmunt;
                }
            }
        }

        $("#lblAddress").text($("#hfAddress").val() + ' Ph: ' + $("#hfPhoneNo").val());
        $("#DateTimeSummary").text(datetime);
        $("#No-Invoices").text(": " + numberWithCommas(parseFloat(data[0].NoOfInvoices)));
        $("#g-sale").text(": " + numberWithCommas(parseFloat(data[0].GROSS_AMOUNT).toFixed(2)));
        $("#gst-amount").text(": " + numberWithCommas(parseFloat(data[0].GST_AMOUNT).toFixed(2)));
        if ($('#hfServiceCharges').val() === "True" || $('#hfIsDeliveryCharges').val() == "True") {
            $("#service-amount").text(": " + numberWithCommas(parseFloat(data[0].SERVICE_CHARGES).toFixed(2)))
        }
        else {
            document.getElementById("ServiceRow").style.display = "none";
        }
        $("#totalAmount").text(": " + numberWithCommas(data[0].TOTAL_AMOUNT));
        $("#discount-amount").text(": " + numberWithCommas(parseFloat(data[0].DISCOUNT).toFixed(0)) + "-" + numberWithCommas(parseFloat(data[0].NoOfDiscountInvoices).toFixed(0)));
        $("#net-amount").text(": " + numberWithCommas(parseFloat(data[0].NET_TOTAL).toFixed(2)));
        var cashsale = 0;
        cashsale = parseFloat(data[0].CashSales) + parseFloat(data[0].CashInHand) - parseFloat(total);
        $("#cash-sales").text(": " + numberWithCommas(parseFloat(cashsale).toFixed(2)));
        $("#credit-card-sales").text(": " + numberWithCommas(parseFloat(data[0].CreditCardSales).toFixed(2)));
        $("#credit-sales").text(": " + numberWithCommas(parseFloat(data[0].CreditSales).toFixed(2)));
        $("#cash-skimmed").text(": " + numberWithCommas(parseFloat(data[0].CashSkimmed).toFixed(2)));
        $("#cash-realiz").text(": " + numberWithCommas(parseFloat(data[0].CashRealization).toFixed(2)));
        $("#advance-realiz").text(": " + numberWithCommas(parseFloat(data[0].AdvanceRealization).toFixed(2)));
        var cashinhand = 0;
        cashinhand = parseFloat(data[0].CashSales) + parseFloat(data[0].CashInHand) -
            parseFloat(data[0].CashSkimmed) - parseFloat(total) + parseFloat(data[0].CashRealization);
        $("#cash-in-hand").text(": " + numberWithCommas(parseFloat(cashinhand).toFixed(2)));
        $("#location-name-sales-summary").text(data[0].DISTRIBUTOR_NAME);
        $("#current-work-date-sales-summary").text(document.getElementById("ddlFromDate").value);
        $("#cashierSummary").text($("#user-detail-bold").text());
        $("#complimentary-sales").text(" : " + numberWithCommas(parseFloat(data[0].ComplimentaryItmsAmnt).toFixed(2)));
        $("#void-sales").text(" : " + numberWithCommas(parseFloat(data[0].VoidSales).toFixed(2)));
        $("#item-less").text(" : " + numberWithCommas(parseFloat(data[0].ItemLessAmount).toFixed(0)) + ", Qty-" + numberWithCommas(parseFloat(data[0].ItemLessQty).toFixed(0)));
        $("#item-cancel").text(" : " + numberWithCommas(parseFloat(data[0].ItemCancelAmount).toFixed(0)) + ", Qty-" + numberWithCommas(parseFloat(data[0].ItemCancelQty).toFixed(0)));
        $("#void-order").text(" : " + numberWithCommas(parseFloat(data[0].Void_Order).toFixed(2)));
        $("#rptSalesSummary-covers").text(" : " + numberWithCommas(parseFloat(data[0].covertable).toFixed(0)));
        if (parseFloat(data[0].covertable) > 0) {
            $("#rptSalesSummary-averagecheck").text(" : " + numberWithCommas((parseFloat(parseFloat(data[0].GROSS_AMOUNT) / parseFloat(data[0].NoOfInvoices)).toFixed(2))));
        }
        else {
            $("#rptSalesSummary-averagecheck").text(" : 0.00");
        }
        $('#cash-realization-data').empty();
        lstData = eval(lstData);
        if (lstData.length > 0) {
            for (var i = 0, len = lstData.length; i < len; ++i) {
                var row = $('<tr><td style="width:1.7in;"><strong>' + lstData[i].CUSTOMER_NAME + '</strong></td><td style="text-align:left;"><span>: ' + numberWithCommas(lstData[i].DEBIT) + '</span></td></tr>');
                $('#cash-realization-data').append(row);
            }
            $("#realzSec").show();
        }
        else {
            $("#realzSec").hide();
        }
        if (lstComplimentary.length > 0) {
            for (var i = 0, len = lstComplimentary.length; i < len; ++i) {
                var row = $('<tr><td style="width:1.7in;"><strong>' + lstComplimentary[i].REASON_DESC + '</strong></td><td style="text-align:left;"><span>: ' + numberWithCommas(lstComplimentary[i].Amount) + '</span></td></tr>');
                $('#complimentary-reason-data').append(row);
            }

            $("#complimentaryreasonSec").show();
        }
        else {
            $("#complimentaryreasonSec").hide();
        }
        document.getElementById("trvoid-order").style.display = "none";
        document.getElementById("trcomplimentary-sales").style.display = "none";
        document.getElementById("trvoid-sales").style.display = "none";
        document.getElementById("tritem-less").style.display = "none";
        document.getElementById("tritem-cancel").style.display = "none";
        var DailySalesReportColumns = $("#hfDailySalesReportColumns").val().split(",");
        for (var j = 0, lenj = DailySalesReportColumns.length; j < lenj; ++j) {
            if (DailySalesReportColumns[j] == "1") {
                document.getElementById("trcomplimentary-sales").style.display = "table-row";
            }
            if (DailySalesReportColumns[j] == "2") {
                document.getElementById("trvoid-sales").style.display = "table-row";
            }
            if (DailySalesReportColumns[j] == "3") {
                document.getElementById("trvoid-order").style.display = "table-row";
            }
            if (DailySalesReportColumns[j] == "4") {
                document.getElementById("tritem-less").style.display = "table-row";
            }
            if (DailySalesReportColumns[j] == "5") {
                document.getElementById("tritem-cancel").style.display = "table-row";
            }
        }

        if (tbl5.length > 0) {
            var row0Discount = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%;font-weight:bold; color:white !important;">' + 'Discount Summary' + '</td></tr>');
            $('#tbl-summary-discount').append(row0Discount);
            var row1Discount = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Cash' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].DiscountCash + '</td></tr>');
            $('#tbl-summary-discount').append(row1Discount);
            var row2Discount = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Credit Card' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].DiscountCreditCard + '</td></tr>');
            $('#tbl-summary-discount').append(row2Discount);
            var row3Discount = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Credit' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].DiscountCredit + '</td></tr>');
            $('#tbl-summary-discount').append(row3Discount);
            var row4Discount = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Bank Discount' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].BankDiscount + '</td></tr>');
            $('#tbl-summary-discount').append(row4Discount);
        }

        if (tbl5.length > 0) {
            var row0GST = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%;font-weight:bold; color:white !important;">' + 'VAT/GST Breakdown' + '</td></tr>');
            $('#tbl-summary-discount').append(row0GST);
            var row1GST = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Cash' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].GSTCash + '</td></tr>');
            $('#tbl-summary-discount').append(row1GST);
            var row2GST = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Credit Card' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].GSTCreditCard + '</td></tr>');
            $('#tbl-summary-discount').append(row2GST);
            var row3GST = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Credit' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].GSTCredit + '</td></tr>');
            $('#tbl-summary-discount').append(row3GST);
            var row4GST = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Bank Discount' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].GSTBankDiscount + '</td></tr>');
            $('#tbl-summary-discount').append(row4GST);
        }

        if (tbl5.length > 0) {
            var row0Payment = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%;font-weight:bold; color:white !important;">' + 'Payment Breakdown' + '</td></tr>');
            $('#tbl-summary-discount').append(row0Payment);
            var row1Payment = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'Cash' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].PaymentCash + '</td></tr>');
            $('#tbl-summary-discount').append(row1Payment);
            var row2Payment = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'Credit Card' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].PaymentCreditCard + '</td></tr>');
            $('#tbl-summary-discount').append(row2Payment);
            var row3Payment = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'Credit' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].PaymentCredit + '</td></tr>');
            $('#tbl-summary-discount').append(row3Payment);
        }

        if (tbl5.length > 0) {
            var row0Cash = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%;font-weight:bold; color:white !important;">' + 'Cash In/Out Summary' + '</td></tr>');
            $('#tbl-summary-discount').append(row0Cash);
            var row1Cash = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'Cash In' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].CashIn + '</td></tr>');
            $('#tbl-summary-discount').append(row1Cash);
            var row2Cash = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'Cash Out' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + tbl5[0].CashOut + '</td></tr>');
            $('#tbl-summary-discount').append(row2Cash);
        }

        $.print("#sales-summary-report");
    }
}
$('#sales-summary').click(function (e) {

$.ajax
           (
               {
                   type: "POST", //HTTP method
                   url: "frmOrderPOS.aspx/SelectSalesSummary", //page/method name
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   data: JSON.stringify({ fromDate: document.getElementById("ddlFromDate").value, toDate: document.getElementById("ddlFromDate").value }),
                   async: false,
                   success: ShowSalesSummary
               }
           );
});
function ShowSalesSummary(data) {
    $('#delivery-channel-data').empty();

    data = $.parseJSON(data.d);
    var lstDCData = data.Table1;
    var lstData = data.Table2;
    var lstComplimentary = data.Table3;
    var tbl5 = data.Table4;
    data = data.Table;
    if (data.length > 0) {
        var currentdate = new Date();
        var datetime = currentdate.getDate() + "/"
                        + (currentdate.getMonth() + 1) + "/"
                        + currentdate.getFullYear() + " "
                        + moment().format('hh:mm A')
        var total = 0;
        if (lstDCData.length > 0) {
            for (var i = 0, len = lstDCData.length; i < len; ++i) {
                var row = $(' <tr><td style="width:1.5in;"><strong>' + lstDCData[i].ChannelType + '</strong></td><td><span>: ' + lstDCData[i].NetAmunt + '</span></td></tr>');
                $('#delivery-channel-data').append(row);
                if (lstDCData[i].Cash_Impact === false) {
                    total += lstDCData[i].NetAmunt;
                }
            }
        }

        $("#lblAddress").text($("#hfAddress").val() + ' Ph: ' + $("#hfPhoneNo").val());
        $("#DateTimeSummary").text(datetime);
        $("#No-Invoices").text(": " + numberWithCommas(parseFloat(data[0].NoOfInvoices)));
        $("#g-sale").text(": " + numberWithCommas(parseFloat(data[0].GROSS_AMOUNT).toFixed(2)));
        $("#gst-amount").text(": " + numberWithCommas(parseFloat(data[0].GST_AMOUNT).toFixed(2)));
        if ($('#hfServiceCharges').val() === "True" || $('#hfIsDeliveryCharges').val() == "True") {
            $("#service-amount").text(": " + numberWithCommas(parseFloat(data[0].SERVICE_CHARGES).toFixed(2)))
        }
        else {
            document.getElementById("ServiceRow").style.display = "none";
        }
        $("#totalAmount").text(": " + numberWithCommas(data[0].TOTAL_AMOUNT));
        $("#discount-amount").text(": " + numberWithCommas(parseFloat(data[0].DISCOUNT).toFixed(0)) + "-" + numberWithCommas(parseFloat(data[0].NoOfDiscountInvoices).toFixed(0)));
        $("#net-amount").text(": " + numberWithCommas(parseFloat(data[0].NET_TOTAL).toFixed(2)));
        var cashsale = 0;
        cashsale = parseFloat(data[0].CashSales) + parseFloat(data[0].CashInHand) - parseFloat(total);
        $("#cash-sales").text(": " + numberWithCommas(parseFloat(cashsale).toFixed(2)));
        $("#credit-card-sales").text(": " + numberWithCommas(parseFloat(data[0].CreditCardSales).toFixed(2)));
        $("#credit-sales").text(": " + numberWithCommas(parseFloat(data[0].CreditSales).toFixed(2)));
        $("#cash-skimmed").text(": " + numberWithCommas(parseFloat(data[0].CashSkimmed).toFixed(2)));
        $("#cash-realiz").text(": " + numberWithCommas(parseFloat(data[0].CashRealization).toFixed(2)));
        $("#advance-realiz").text(": " + numberWithCommas(parseFloat(data[0].AdvanceRealization).toFixed(2)));
        var cashinhand = 0;
        cashinhand = parseFloat(data[0].CashSales) + parseFloat(data[0].CashInHand) -
            parseFloat(data[0].CashSkimmed) - parseFloat(total) + parseFloat(data[0].CashRealization);
        $("#cash-in-hand").text(": " + numberWithCommas(parseFloat(cashinhand).toFixed(2)));
        $("#location-name-sales-summary").text(data[0].DISTRIBUTOR_NAME);
        $("#current-work-date-sales-summary").text(document.getElementById("ddlFromDate").value);
        $("#cashierSummary").text($("#user-detail-bold").text());
        $("#complimentary-sales").text(" : " + numberWithCommas(parseFloat(data[0].ComplimentaryItmsAmnt).toFixed(2)));
        $("#void-sales").text(" : " + numberWithCommas(parseFloat(data[0].VoidSales).toFixed(2)));
        $("#item-less").text(" : " + numberWithCommas(parseFloat(data[0].ItemLessAmount).toFixed(0)) + ", Qty-" + numberWithCommas(parseFloat(data[0].ItemLessQty).toFixed(0)));
        $("#item-cancel").text(" : " + numberWithCommas(parseFloat(data[0].ItemCancelAmount).toFixed(0)) + ", Qty-" + numberWithCommas(parseFloat(data[0].ItemCancelQty).toFixed(0)));
        $("#void-order").text(" : " + numberWithCommas(parseFloat(data[0].Void_Order).toFixed(2)));
        $("#rptSalesSummary-covers").text(" : " + numberWithCommas(parseFloat(data[0].covertable).toFixed(0)));
        if (parseFloat(data[0].covertable) > 0) {
            $("#rptSalesSummary-averagecheck").text(" : " + numberWithCommas((parseFloat(parseFloat(data[0].GROSS_AMOUNT) / parseFloat(data[0].NoOfInvoices)).toFixed(2))));
        }
        else {
            $("#rptSalesSummary-averagecheck").text(" : 0.00");
        }
        $('#cash-realization-data').empty();
        lstData = eval(lstData);
        if (lstData.length > 0) {
            for (var i = 0, len = lstData.length; i < len; ++i) {
                var row = $('<tr><td style="width:1.7in;"><strong>' + lstData[i].CUSTOMER_NAME + '</strong></td><td style="text-align:left;"><span>: ' + numberWithCommas(lstData[i].DEBIT) + '</span></td></tr>');
                $('#cash-realization-data').append(row);
            }
            $("#realzSec").show();
        }
        else {
            $("#realzSec").hide();
        }
        if (lstComplimentary.length > 0)
        {
            for (var i = 0, len = lstComplimentary.length; i < len; ++i) {
                var row = $('<tr><td style="width:1.7in;"><strong>' + lstComplimentary[i].REASON_DESC + '</strong></td><td style="text-align:left;"><span>: ' + numberWithCommas(lstComplimentary[i].Amount) + '</span></td></tr>');
                $('#complimentary-reason-data').append(row);
            }

            $("#complimentaryreasonSec").show();
        }
        else {
            $("#complimentaryreasonSec").hide();
        }
        document.getElementById("trvoid-order").style.display = "none";
        document.getElementById("trcomplimentary-sales").style.display = "none";
        document.getElementById("trvoid-sales").style.display = "none";
        document.getElementById("tritem-less").style.display = "none";
        document.getElementById("tritem-cancel").style.display = "none";
        var DailySalesReportColumns = $("#hfDailySalesReportColumns").val().split(",");
        for (var j = 0, lenj = DailySalesReportColumns.length; j < lenj; ++j) {
            if (DailySalesReportColumns[j] == "1") {
                document.getElementById("trcomplimentary-sales").style.display = "table-row";
            }
            if (DailySalesReportColumns[j] == "2") {
                document.getElementById("trvoid-sales").style.display = "table-row";
            }
            if (DailySalesReportColumns[j] == "3")
            {
                document.getElementById("trvoid-order").style.display = "table-row";
            }
            if (DailySalesReportColumns[j] == "4") {
                document.getElementById("tritem-less").style.display = "table-row";
            }
            if (DailySalesReportColumns[j] == "5") {
                document.getElementById("tritem-cancel").style.display = "table-row";
            }
        }

        $.print("#sales-summary-report");
    }
}
$('#sales-detail').click(function (e) {

    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmOrderPOS.aspx/SelectSalesDetail", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ fromDate: document.getElementById("ddlFromDate").value, toDate: document.getElementById("ddlFromDate").value }),
                    success: ShowSalesDetail
                }
            );
});
function ShowSalesDetail(data) {
    data = JSON.stringify(data);
    data = jQuery.parseJSON(data.replace(/&quot;/g, '"'));
    data = eval(data.d);
    if (data.length > 0) {
        var currentdate = new Date();
        var datetime = currentdate.getDate() + "/"
                        + (currentdate.getMonth() + 1) + "/"
                        + currentdate.getFullYear() + " "
                        + moment().format('hh:mm A')
        $("#DateTimeDetail").text(datetime);
        $("#lblAddress2").text($("#hfAddress").val() + ' Ph: ' + $("#hfPhoneNo").val());
        $("#cashierDetail").text($("#user-detail-bold").text());
        $("#item-No-Invoices").text(numberWithCommas(parseFloat(data[0].NoOfInvoices)));
        $('#sales-detail-data').empty(); // clear all skus  from invoice
        var grandTotal = 0;
        for (var i = 0, len = data.length; i < len; i++) {
            var row = $(' <tr><td>' + data[i].TableDefination_No + '</td><td class="text-right">' + numberWithCommas(parseFloat(data[i].NET_TOTAL).toFixed(2)) + '</td></tr>');
            $('#sales-detail-data').append(row);
            grandTotal += data[i].NET_TOTAL;
        }
        $("#location-name-sales-detail").text(data[0].DISTRIBUTOR_NAME);
        $("#current-work-date-sales-detail").text(document.getElementById("ddlFromDate").value);
        $('#sales-detail-grand-total').text(numberWithCommas(parseFloat(grandTotal).toFixed(2)));
        $.print("#sales-detail-report");
    }
}
$('#item-sales-detail').click(function (e) {

    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmOrderPOS.aspx/SelectItemSalesDetail", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ fromDate: document.getElementById("ddlFromDate").value, toDate: document.getElementById("ddlFromDate").value }),
                    success: ShowItemSalesDetail
                }
            );
});
$('#item-sales-detailKFFoods').click(function (e) {

    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmOrderPOS.aspx/SelectItemSalesDetail", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ fromDate: document.getElementById("ddlFromDate").value, toDate: document.getElementById("ddlFromDate").value }),
                    success: ShowItemSalesDetailKFFoods
                }
            );
});
$('#category-sales').click(function (e) {

    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmOrderPOS.aspx/SelectItemSalesDetail", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ fromDate: document.getElementById("ddlFromDate").value, toDate: document.getElementById("ddlFromDate").value }),
                    success: ShowCategorySalesDetail
                }
            );
});
function ShowCategorySalesDetail(data) {
    data = $.parseJSON(data.d);
    data = data.Table;
    if (data.length > 0) {
        var currentdate = new Date();
        var datetime = currentdate.getDate() + "/"
                        + (currentdate.getMonth() + 1) + "/"
                        + currentdate.getFullYear() + " "
                        + moment().format('hh:mm A')
        $("#DateTimeCategoryDetail").text(datetime);
        $("#lblAddressCategory").text($("#hfAddress").val() + ' Ph: ' + $("#hfPhoneNo").val());
        $("#cashierCategoryDetail").text($("#user-detail-bold").text());
        $('#category-sales-detail-data').empty(); // clear all skus  from invoice
        var netTotal = 0;
        var netQtyTotal = 0;
        var uniqueCategories = $.unique(data.map(function (d) { return d.CATEGORY_NAME; }));
        uniqueCategories = uniqueCategories.sort();
        uniqueCategories = unique(uniqueCategories);
        var row2 = $('<tr style="background-color:black !important;"><td style="width:58%;color:white !important;font-weight:bold;">Category</td>' +
              '<td  style="width:18%; text-align:center;color:white !important;font-weight:bold;" >Qty</td><td style="width:24%;color:white !important;font-weight:bold;" class="text-right">Amount</td></tr>');
        $('#category-sales-detail-data').append(row2);
        for (var j = 0; j < uniqueCategories.length; j++) {
            var grandTotal = 0;
            var grandQtyTotal = 0;
            for (var i = 0, len = data.length; i < len; i++) {
                if (data[i].CATEGORY_NAME == uniqueCategories[j]) {
                    grandQtyTotal += data[i].QUANTITY;
                    grandTotal += data[i].NET_TOTAL;
                    netQtyTotal += data[i].QUANTITY;
                    netTotal += data[i].NET_TOTAL;
                }
            }
            var row3 = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:58%;">' + uniqueCategories[j] + '' +
                '</td><td  style="width:18%; text-align:center;" >' + grandQtyTotal + '</td><td style="width:24%;" class="text-right">' + numberWithCommas(parseFloat(grandTotal).toFixed(0)) + '</td></tr>');
            $('#category-sales-detail-data').append(row3);
            var line2 = $('<tr style="background-color:black !important; height:3px;"><td colspan="3"></td></tr>');
            $('#category-sales-detail-data').append(line2);
            var line3 = $('<tr style="height:7px;"><td colspan="3"></td></tr>');
            $('#category-sales-detail-data').append(line3);
        }
        $("#location-name-category-sales-detail").text(data[0].DISTRIBUTOR_NAME);
        $("#current-work-date-category-sales-detail").text(document.getElementById("ddlFromDate").value);
        $('#category-sales-detail-grand-qty-total').text(numberWithCommas(netQtyTotal));
        $('#category-sales-detail-grand-total').text(numberWithCommas(parseFloat(netTotal).toFixed(0)));
        $.print("#category-sales-detail-report");
    }
}
function ShowItemSalesDetail(data) {
    data = $.parseJSON(data.d);
    var data2 = data.Table1;
    var data3 = data.Table2;
    var data4 = data.Table3;
    data = data.Table;
    if (data.length > 0) {
        var currentdate = new Date();
        var datetime = currentdate.getDate() + "/"
                        + (currentdate.getMonth() + 1) + "/"
                        + currentdate.getFullYear() + " "
                        + moment().format('hh:mm A')
        $("#lblCoverTables").text("Covers: " + data[0].covertable);
        $("#lblNoOfInvoices").text("No Of Invoices: " + data[0].NoOfInvoices);
        $("#DateTimeItemDetail").text(datetime);
        $("#lblAddress3").text($("#hfAddress").val() + ' Ph: ' + $("#hfPhoneNo").val());
        $("#cashierItemDetail").text($("#user-detail-bold").text());
        $('#item-sales-detail-data').empty(); // clear all skus  from invoice
        $('#item-sales-detail-data-complementary').empty();
        $('#item-sales-detail-data-cancel').empty();
        $("#item-sales-detail-table-cancel").hide();
        $('#item-sales-detail-data-less').empty();
        $("#item-sales-detail-table-less").hide();
        $("#item-sales-detail-table-less").hide();
        $("#item-sales-detail-table-complementary").hide();
        var netTotal = 0;
        var netQtyTotal = 0;
        var uniqueCategories = $.unique(data.map(function (d) { return d.CATEGORY_NAME; }));
        uniqueCategories = uniqueCategories.sort();
        uniqueCategories = unique(uniqueCategories);
        for (var j = 0; j < uniqueCategories.length; j++) {

            var grandTotal = 0;
            var grandQtyTotal = 0;

            var row1 = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%; color:white !important;">' + uniqueCategories[j] + '</td></tr>');
            $('#item-sales-detail-data').append(row1);

            var row2 = $('<tr><td style="width:60%;">Item Name</td><td  style="width:15%; text-align:center;" >Qty</td><td style="width:25%;" class="text-right">Amount</td></tr>');
            $('#item-sales-detail-data').append(row2);

            var line = $('<tr style="background-color:black !important; height:3px;"><td colspan="3"></td></tr>');
            $('#item-sales-detail-data').append(line);


            for (var i = 0, len = data.length; i < len; i++) {
                if (data[i].CATEGORY_NAME == uniqueCategories[j]) {

                    var row3 = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + data[i].SKU_NAME + '</td><td  style="width:15%; text-align:center;" >' + data[i].QUANTITY + '</td><td style="width:25%;" class="text-right">' + numberWithCommas(parseFloat(data[i].NET_TOTAL).toFixed(0)) + '</td></tr>');
                    $('#item-sales-detail-data').append(row3);
                    grandQtyTotal += data[i].QUANTITY;
                    grandTotal += data[i].NET_TOTAL;
                    netQtyTotal += data[i].QUANTITY;
                    netTotal += data[i].NET_TOTAL;
                }
            }

            var line2 = $('<tr style="background-color:black !important; height:3px;"><td colspan="3"></td></tr>');
            $('#item-sales-detail-data').append(line2);

            var row4 = $('<tr><td></td><td  style="text-align:center;">' + numberWithCommas(grandQtyTotal) + '</td><td class="text-right" >' + numberWithCommas(parseFloat(grandTotal).toFixed(0)) + '</td></tr>');
            $('#item-sales-detail-data').append(row4);

            var line3 = $('<tr style="height:7px;"><td colspan="3"></td></tr>');
            $('#item-sales-detail-data').append(line3);
        }
        $("#location-name-item-sales-detail").text(data[0].DISTRIBUTOR_NAME);
        $("#current-work-date-item-sales-detail").text(document.getElementById("ddlFromDate").value);
        $('#item-sales-detail-grand-qty-total').text(numberWithCommas(netQtyTotal));
        $('#item-sales-detail-grand-total').text(numberWithCommas(parseFloat(netTotal).toFixed(0)));

        grandQtyTotal = 0;
        grandTotal = 0;
        if (data2.length > 0) {
            var rowComplementary = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%; color:white !important;">Complementary Items</td></tr>');
            $('#item-sales-detail-data-complementary').append(rowComplementary);
            for (var i = 0, len = data2.length; i < len; i++) {
                var row3 = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:50%;">' + data2[i].SKU_NAME + '</td><td  style="width:25%;  text-align:right;" >' + data2[i].QUANTITY + '</td><td style="width:25%; text-align:right;">' + numberWithCommas(parseFloat(data2[i].NET_TOTAL).toFixed(0)) + '</td></tr>');
                $('#item-sales-detail-data-complementary').append(row3);
                grandQtyTotal += data2[i].QUANTITY;
                grandTotal += data2[i].NET_TOTAL;
            }
            $('#item-sales-detail-grand-qty-total-complementary').text(numberWithCommas(grandQtyTotal));
            $('#item-sales-detail-grand-total-complementary').text(numberWithCommas(parseFloat(grandTotal).toFixed(0)));
            $("#item-sales-detail-table-complementary").show();
        }

        grandQtyTotal = 0;
        grandTotal = 0;

        if (data3.length > 0) {
            var rowCancel = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%; color:white !important;">Cancel Items</td></tr>');
            $('#item-sales-detail-data-cancel').append(rowCancel);
            for (var i = 0, len = data3.length; i < len; i++) {
                var row4 = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:50%;">' + data3[i].SKU_NAME + '</td><td  style="width:25%; text-align:right;" >' + data3[i].QUANTITY + '</td><td style="width:25%; text-align:right;">' + numberWithCommas(parseFloat(data3[i].NET_TOTAL).toFixed(0)) + '</td></tr>');
                $('#item-sales-detail-data-cancel').append(row4);
                grandQtyTotal += data3[i].QUANTITY;
                grandTotal += data3[i].NET_TOTAL;
            }
            $('#item-sales-detail-grand-qty-total-cancel').text(numberWithCommas(grandQtyTotal));
            $('#item-sales-detail-grand-total-cancel').text(numberWithCommas(parseFloat(grandTotal).toFixed(0)));
            $("#item-sales-detail-table-cancel").show();
        }
        grandQtyTotal = 0;
        grandTotal = 0;

        if (data4.length > 0) {
            var rowCancel = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%; color:white !important;">Less Items</td></tr>');
            $('#item-sales-detail-data-less').append(rowCancel);
            for (var i = 0, len = data4.length; i < len; i++) {
                var row4 = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:50%;">' + data4[i].SKU_NAME + '</td><td  style="width:25%; text-align:right;" >' + data4[i].QUANTITY + '</td><td style="width:25%; text-align:right;">' + numberWithCommas(parseFloat(data4[i].NET_TOTAL).toFixed(0)) + '</td></tr>');
                $('#item-sales-detail-data-less').append(row4);
                grandQtyTotal += data4[i].QUANTITY;
                grandTotal += data4[i].NET_TOTAL;
            }
            $('#item-sales-detail-grand-qty-total-less').text(numberWithCommas(grandQtyTotal));
            $('#item-sales-detail-grand-total-less').text(numberWithCommas(parseFloat(grandTotal).toFixed(0)));
            $("#item-sales-detail-table-less").show();
        }

        $.print("#item-sales-detail-report");
    }
}
function ShowItemSalesDetailKFFoods(data) {
    data = $.parseJSON(data.d);
    var data2 = data.Table1;
    var data3 = data.Table2;
    var data4 = data.Table3;
    data = data.Table;
    if (data.length > 0) {
        var currentdate = new Date();
        var datetime = currentdate.getDate() + "/"
                        + (currentdate.getMonth() + 1) + "/"
                        + currentdate.getFullYear() + " "
                        + moment().format('hh:mm A')
        $("#lblCoverTables").text("Covers: " + data[0].covertable);
        $("#lblNoOfInvoices").text("No Of Invoices: " + data[0].NoOfInvoices);
        $("#DateTimeItemDetail").text(datetime);
        $("#lblAddress3").text($("#hfAddress").val() + ' Ph: ' + $("#hfPhoneNo").val());
        $("#cashierItemDetail").text($("#user-detail-bold").text());
        $('#item-sales-detail-data').empty(); // clear all skus  from invoice
        $('#item-sales-detail-data-complementary').empty();
        $('#item-sales-detail-data-cancel').empty();
        $("#item-sales-detail-table-cancel").hide();
        $('#item-sales-detail-data-less').empty();
        $("#item-sales-detail-table-less").hide();
        $("#item-sales-detail-table-less").hide();
        $("#item-sales-detail-table-complementary").hide();
        $("#tditem-sales-detail-grand-total").hide();
        var netTotal = 0;
        var netQtyTotal = 0;
        var uniqueCategories = $.unique(data.map(function (d) { return d.CATEGORY_NAME; }));
        uniqueCategories = uniqueCategories.sort();
        uniqueCategories = unique(uniqueCategories);
        for (var j = 0; j < uniqueCategories.length; j++) {

            var grandTotal = 0;
            var grandQtyTotal = 0;

            var row1 = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%; color:white !important;">' + uniqueCategories[j] + '</td></tr>');
            $('#item-sales-detail-data').append(row1);

            var row2 = $('<tr><td style="width:70%;">Item Name</td><td  style="width:30%; text-align:center;" >Qty</tr>');
            $('#item-sales-detail-data').append(row2);

            var line = $('<tr style="background-color:black !important; height:3px;"><td colspan="3"></td></tr>');
            $('#item-sales-detail-data').append(line);


            for (var i = 0, len = data.length; i < len; i++) {
                if (data[i].CATEGORY_NAME == uniqueCategories[j]) {

                    var row3 = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:70%;">' + data[i].SKU_NAME + '</td><td  style="width:30%; text-align:center;" >' + data[i].QUANTITY + '</td></tr>');
                    $('#item-sales-detail-data').append(row3);
                    grandQtyTotal += data[i].QUANTITY;
                    netQtyTotal += data[i].QUANTITY;
                }
            }

            var line2 = $('<tr style="background-color:black !important; height:3px;"><td colspan="3"></td></tr>');
            $('#item-sales-detail-data').append(line2);

            var row4 = $('<tr><td></td><td  style="text-align:center;">' + numberWithCommas(grandQtyTotal) + '</td></tr>');
            $('#item-sales-detail-data').append(row4);

            var line3 = $('<tr style="height:7px;"><td colspan="3"></td></tr>');
            $('#item-sales-detail-data').append(line3);
        }
        $("#location-name-item-sales-detail").text(data[0].DISTRIBUTOR_NAME);
        $("#current-work-date-item-sales-detail").text(document.getElementById("ddlFromDate").value);
        $('#item-sales-detail-grand-qty-total').text(numberWithCommas(netQtyTotal));

        grandQtyTotal = 0;
        grandTotal = 0;
        if (data2.length > 0) {
            var rowComplementary = $('<tr style="text-align:center; background-color:black !important;"><td colspan="2" style="width:100%; color:white !important;">Complementary Items</td></tr>');
            $('#item-sales-detail-data-complementary').append(rowComplementary);
            for (var i = 0, len = data2.length; i < len; i++) {
                var row3 = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:70%;">' + data2[i].SKU_NAME + '</td><td  style="width:30%;  text-align:center;" >' + data2[i].QUANTITY + '</td></tr>');
                $('#item-sales-detail-data-complementary').append(row3);
                grandQtyTotal += data2[i].QUANTITY;                
            }
            $('#item-sales-detail-grand-qty-total-complementary').text(numberWithCommas(grandQtyTotal));
            $("#item-sales-detail-table-complementary").show();
        }

        grandQtyTotal = 0;
        grandTotal = 0;

        if (data3.length > 0) {
            var rowCancel = $('<tr style="text-align:center; background-color:black !important;"><td colspan="2" style="width:100%; color:white !important;">Cancel Items</td></tr>');
            $('#item-sales-detail-data-cancel').append(rowCancel);
            for (var i = 0, len = data3.length; i < len; i++) {
                var row4 = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:70%;">' + data3[i].SKU_NAME + '</td><td  style="width:30%; text-align:center;" >' + data3[i].QUANTITY + '</td></tr>');
                $('#item-sales-detail-data-cancel').append(row4);
                grandQtyTotal += data3[i].QUANTITY;
            }
            $('#item-sales-detail-grand-qty-total-cancel').text(numberWithCommas(grandQtyTotal));
            $("#item-sales-detail-table-cancel").show();
        }
        grandQtyTotal = 0;
        grandTotal = 0;

        if (data4.length > 0) {
            var rowCancel = $('<tr style="text-align:center; background-color:black !important;"><td colspan="2" style="width:100%; color:white !important;">Less Items</td></tr>');
            $('#item-sales-detail-data-less').append(rowCancel);
            for (var i = 0, len = data4.length; i < len; i++) {
                var row4 = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:70%;">' + data4[i].SKU_NAME + '</td><td  style="width:30%; text-align:center;" >' + data4[i].QUANTITY + '</td></tr>');
                $('#item-sales-detail-data-less').append(row4);
                grandQtyTotal += data4[i].QUANTITY;
            }
            $('#item-sales-detail-grand-qty-total-less').text(numberWithCommas(grandQtyTotal));
            $("#item-sales-detail-table-less").show();
        }

        $.print("#item-sales-detail-report");
    }
}
$('#daily-sales').click(function (e) {

    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmOrderPOS.aspx/GetDailySaleData", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ fromDate: document.getElementById("ddlFromDate").value, toDate: document.getElementById("ddlFromDate").value }),
                    success: ShowDailySaleData
                }
            );
});
function ShowDailySaleData(data) {
    data = JSON.stringify(data);
    data = jQuery.parseJSON(data.replace(/&quot;/g, '"'));
    data = eval(data.d);
    if (data.length > 0) {
        var netsale = 0;
        var discper = 0;
        var discvalue = 0;
        var gst = 0;
        var gross = 0;
        var service = 0;
        var withservice = 0;
        var cover = 0
        var currentdate = new Date();
        var datetime = currentdate.getDate() + "/"
                        + (currentdate.getMonth() + 1) + "/"
                        + currentdate.getFullYear() + " "
                        + moment().format('hh:mm A')
        $("#DateTimeItemDaily").text(datetime);
        $("#lblAddressDailySaleData").text($("#hfAddress").val() + ' Ph: ' + $("#hfPhoneNo").val());
        $("#cashierDailySaleData").text($("#user-detail-bold").text());
        $("#dayname").text(weekAndDay($("#hfCurrentWorkDate").val())[0].dayName);
        $('#daily-sale-data').empty();
        var uniqueCategories = $.unique(data.map(function (d) { return d.intDCText; }));
        uniqueCategories = uniqueCategories.sort();
        uniqueCategories = unique(uniqueCategories);
        for (var j = 0; j < uniqueCategories.length; j++) {
            if (uniqueCategories[j] != 'Covers') {
                var row1 = $('<tr style="text-align:center; background-color:black !important;"><td colspan="5" style="width:100%; color:white !important;">' + uniqueCategories[j] + '</td></tr>');
                $('#daily-sale-data').append(row1);
                var row2 = $('<tr><td style="width:20%;">Sub Total</td><td  style="width:20%; text-align:center;" >Dis. %</td><td style="width:20%;" class="text-right">Dis. Rs.</td><td style="width:20%;" class="text-right">GST</td><td style="width:20%;" class="text-right">Bill Total</td></tr>');
                $('#daily-sale-data').append(row2);
                var line = $('<tr style="background-color:black !important; height:3px;"><td colspan="5"></td></tr>');
                $('#daily-sale-data').append(line);
                for (var i = 0, len = data.length; i < len; i++) {
                    if (data[i].intDCText == uniqueCategories[j]) {
                        var billtotal = parseFloat(data[i].SubTotal) - parseFloat(data[i].DiscountPer) - parseFloat(data[i].DiscountValue) + parseFloat(data[i].GST);
                        var row3 = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:20%;">' + data[i].SubTotal + '</td><td  style="width:20%; text-align:center;" >' + data[i].DiscountPer + '</td><td style="width:20%;" class="text-right">' + data[i].DiscountValue + '</td><td style="width:20%;" class="text-right">' + data[i].GST + '</td><td style="width:20%;" class="text-right">' + billtotal + '</td></tr>');
                        $('#daily-sale-data').append(row3);
                        var row32 = $('<tr><td></td><td></td><td colspan="2" class="text-right">Service Charges</td><td class="text-right">' + data[i].ServiceCharges + '</td></tr>');
                        $('#daily-sale-data').append(row32);
                        netsale += parseFloat(data[i].SubTotal);
                        discper += parseFloat(data[i].DiscountPer);
                        discvalue += parseFloat(data[i].DiscountValue);
                        gst += parseFloat(data[i].GST);
                        service += parseFloat(data[i].ServiceCharges);
                        cover += parseFloat(data[i].Covers);
                    }
                }
                var line2 = $('<tr style="background-color:black !important; height:3px;"><td colspan="5"></td></tr>');
                $('#daily-sale-data').append(line2);
            }
            else {
                for (var i = 0, len = data.length; i < len; i++) {
                    if (data[i].intDCText == uniqueCategories[j]) {
                        cover += parseFloat(data[i].Covers);
                    }
                }
            }
        }
        gross = netsale + gst - discper - discvalue;
        withservice = gross + service;
        $("#location-name-daily-sale-data").text(data[0].DISTRIBUTOR_NAME);
        $("#current-work-date-daily-sale-data").text(document.getElementById("ddlFromDate").value);
        $('#daily-sale-data-net-sale').text(numberWithCommas(netsale));
        $('#daily-sale-data-sub-total').text(numberWithCommas(netsale));
        $('#daily-sale-data-dis-per').text(numberWithCommas(discper));
        $('#daily-sale-data-dis-value').text(numberWithCommas(discvalue));
        $('#daily-sale-data-gst').text(numberWithCommas(gst));
        $('#daily-sale-data-gross').text(numberWithCommas(gross));
        $('#daily-sale-data-service-charges').text(numberWithCommas(service));
        $('#daily-sale-data-with-service-charges').text(numberWithCommas(withservice));
        $('#daily-sale-data-covers').text(numberWithCommas(cover));
        var avgcheck = 0;
        if (parseFloat(cover) > 0) {
            avgcheck = parseFloat(gross) / parseFloat(cover);
        }
        $('#daily-sale-data-AvgCheck').text(numberWithCommas(avgcheck));
        $.print("#daily-sale-report");
    }
}
$('#daily-summary').click(function (e) {

    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmOrderPOS.aspx/GetDailySummary", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ fromDate: document.getElementById("ddlFromDate").value, toDate: document.getElementById("ddlFromDate").value }),
                    success: ShowDailySummary
                }
            );
});
function ShowDailySummary(data) {
    data = $.parseJSON(data.d);
    var data2 = data.Table1;
    data = data.Table;    
    if (data.length > 0) {        
        $('#daily-summary-data').empty(); // clear all skus  from invoice
        var netTotal = 0;
        var netQtyTotal = 0;
        var uniqueCategories = $.unique(data.map(function (d) { return d.CATEGORY_NAME; }));
        uniqueCategories = uniqueCategories.sort();
        uniqueCategories = unique(uniqueCategories);
        var row0 = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%;font-weight:bold; color:white !important;">' + 'Sale Summary By Items' + '</td></tr>');
        $('#daily-summary-data').append(row0);
        var row01 = $('<tr><td style="width:60%;"></td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right"></td></tr>');
        $('#daily-summary-data').append(row01);
        for (var j = 0; j < uniqueCategories.length; j++) {
            var grandTotal = 0;
            var grandQtyTotal = 0;
            var row1 = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%;font-weight:bold; color:white !important;">' + uniqueCategories[j] + '</td></tr>');
            $('#daily-summary-data').append(row1);
            var row2 = $('<tr><td style="width:60%;">Item Name</td><td  style="width:15%; text-align:center;" >Qty</td><td style="width:25%;" class="text-right">Amount</td></tr>');
            $('#daily-summary-data').append(row2);
            var line = $('<tr style="background-color:black !important; height:3px;"><td colspan="3"></td></tr>');
            $('#daily-summary-data').append(line);
            for (var i = 0, len = data.length; i < len; i++) {
                if (data[i].CATEGORY_NAME == uniqueCategories[j]) {
                    var row3 = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + data[i].SKU_NAME + '</td><td  style="width:15%; text-align:center;" >' + data[i].QUANTITY + '</td><td style="width:25%;" class="text-right">' + numberWithCommas(parseFloat(data[i].NET_TOTAL).toFixed(0)) + '</td></tr>');
                    $('#daily-summary-data').append(row3);
                    grandQtyTotal += data[i].QUANTITY;
                    grandTotal += data[i].NET_TOTAL;
                    netQtyTotal += data[i].QUANTITY;
                    netTotal += data[i].NET_TOTAL;
                }
            }
            var line2 = $('<tr style="background-color:black !important; height:3px;"><td colspan="3"></td></tr>');
            $('#daily-summary-data').append(line2);
            var row4 = $('<tr><td></td><td  style="text-align:center;">' + numberWithCommas(grandQtyTotal) + '</td><td class="text-right" >' + numberWithCommas(parseFloat(grandTotal).toFixed(0)) + '</td></tr>');
            $('#daily-summary-data').append(row4);
            var line3 = $('<tr style="height:7px;"><td colspan="3"></td></tr>');
            $('#daily-summary-data').append(line3);
        }
        var rowTotal = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;"><strong>Total</strong></td><td  style="width:15%; text-align:center;" ><strong>' + numberWithCommas(netQtyTotal) + '</strong></td><td style="width:25%;" class="text-right"><strong>' + numberWithCommas(parseFloat(netTotal).toFixed(0)) + '</strong></td></tr>');
        $('#daily-summary-data').append(rowTotal);
        var rowBlank = $('<tr ><td colspan="3"></td></tr>');
        $('#daily-summary-data').append(rowBlank);

        var row0Discount = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%;font-weight:bold; color:white !important;">' + 'Discount Summary' + '</td></tr>');
        $('#daily-summary-data').append(row0Discount);
        if (data2.length > 0) {
            var row1Discount = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Cash' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].DiscountCash + '</td></tr>');
            $('#daily-summary-data').append(row1Discount);
        }
        if (data2.length > 0) {
            var row2Discount = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Credit Card' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].DiscountCreditCard + '</td></tr>');
            $('#daily-summary-data').append(row2Discount);
        }
        if (data2.length > 0) {
            var row3Discount = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Credit' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].DiscountCredit + '</td></tr>');
            $('#daily-summary-data').append(row3Discount);
        }
        if (data2.length > 0) {
            var row4Discount = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Bank Discount' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].BankDiscount + '</td></tr>');
            $('#daily-summary-data').append(row4Discount);
        }

        var row0GST = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%;font-weight:bold; color:white !important;">' + 'VAT/GST Breakdown' + '</td></tr>');
        $('#daily-summary-data').append(row0GST);
        if (data2.length > 0) {
            var row1GST = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Cash' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].GSTCash + '</td></tr>');
            $('#daily-summary-data').append(row1GST);
        }
        if (data2.length > 0) {
            var row2GST = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Credit Card' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].GSTCreditCard + '</td></tr>');
            $('#daily-summary-data').append(row2GST);
        }
        if (data2.length > 0) {
            var row3GST = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Credit' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].GSTCredit + '</td></tr>');
            $('#daily-summary-data').append(row3GST);
        }
        if (data2.length > 0) {
            var row4GST = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'On Bank Discount' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].GSTBankDiscount + '</td></tr>');
            $('#daily-summary-data').append(row4GST);
        }

        var row0Payment = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%;font-weight:bold; color:white !important;">' + 'Payment Breakdown' + '</td></tr>');
        $('#daily-summary-data').append(row0Payment);
        if (data2.length > 0) {
            var row1Payment = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'Cash' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].PaymentCash + '</td></tr>');
            $('#daily-summary-data').append(row1Payment);
        }
        if (data2.length > 0) {
            var row2Payment = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'Credit Card' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].PaymentCreditCard + '</td></tr>');
            $('#daily-summary-data').append(row2Payment);
        }
        if (data2.length > 0) {
            var row3Payment = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'Credit' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].PaymentCredit + '</td></tr>');
            $('#daily-summary-data').append(row3Payment);
        }

        var row0Cash = $('<tr style="text-align:center; background-color:black !important;"><td colspan="3" style="width:100%;font-weight:bold; color:white !important;">' + 'Cash In/Out Summary' + '</td></tr>');
        $('#daily-summary-data').append(row0Cash);
        if (data2.length > 0) {
            var row1Cash = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'Cash In' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].CashIn + '</td></tr>');
            $('#daily-summary-data').append(row1Cash);
        }
        if (data2.length > 0) {
            var row2Cash = $('<tr style="padding-top:3px;padding-bottom:3px;"><td style="width:60%;">' + 'Cash Out' + '</td><td  style="width:15%; text-align:center;" ></td><td style="width:25%;" class="text-right">' + data2[0].CashOut + '</td></tr>');
            $('#daily-summary-data').append(row2Cash);
        }


        if (data2.length > 0) {
            $("#current-work-date-daily-summary").text(document.getElementById("ddlFromDate").value);
            $("#location-name-daily-summary").text($("#hfLocationName").val());
            $("#cashierdailysummary").text($("#user-detail-bold").text());
            $("#lblDayStart").text("Day Start: " + data2[0].StartDate);
            $("#lblDayEnd").text("Day End: " + data2[0].EndDate);
        }


        $.print("#daily-summary-report");
    }    
}
$('#service-type').click(function (e) {

    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmOrderPOS.aspx/GetServiceWiseSales", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ fromDate: document.getElementById("ddlFromDate").value, toDate: document.getElementById("ddlFromDate").value }),
                    success: ShowServiceWiseSales
                }
            );
});
function ShowServiceWiseSales(data)
{
    data = $.parseJSON(data.d);
    var data2 = data.Table1;
    var data3 = data.Table2;
    var data4 = data.Table3;
    var data5 = data.Table4;
    var data6 = data.Table5;
    var data7 = data.Table6;
    data = data.Table;
    var TotalNoOfInvoices = 0;
    var TotalGross = 0.0;
    var TotalSales = 0.0;
    var TotalTax = 0.0;
    if (data.length > 0)
    {
        TotalNoOfInvoices += parseInt(data[0].NoOfInvoices);
        TotalGross += parseFloat(data[0].Gross);
        TotalSales += parseFloat(data[0].Sales);
        TotalTax += parseFloat(data[0].Tax);
        var avgperinvoice = 0.0;
        var avgperguest = 0.0;
        if (parseFloat(data[0].NoOfInvoices) > 0)
        {
            avgperinvoice = parseFloat(data[0].Sales) / parseFloat(data[0].NoOfInvoices);
        }
        if (parseFloat(data[0].covertable) > 0) {
            avgperguest = parseFloat(data[0].Sales) / parseFloat(data[0].covertable);
        }
        $("#service-type-dine-no-of-invoices").text(data[0].NoOfInvoices);
        $("#service-type-dine-no-of-guests").text(data[0].covertable);
        $("#service-type-dine-avg-per-invoice").text(avgperinvoice.toFixed(2));
        $("#service-type-dine-avg-per-guest").text(avgperguest.toFixed(2));
    }
    if (data2.length > 0) {
        TotalNoOfInvoices += parseInt(data2[0].NoOfInvoices);
        TotalGross += parseFloat(data2[0].Gross);
        TotalSales += parseFloat(data2[0].Sales);
        TotalTax += parseFloat(data2[0].Tax);
        var avgperinvoice = 0.0;
        if (parseFloat(data2[0].NoOfInvoices) > 0) {
            avgperinvoice = parseFloat(data2[0].Sales) / parseFloat(data2[0].NoOfInvoices);
        }
        $("#service-type-delivery-no-of-invoices").text(data2[0].NoOfInvoices);
        $("#service-type-delivery-avg-per-invoice").text(avgperinvoice.toFixed(2));
    }
    if (data3.length > 0) {
        TotalNoOfInvoices += parseInt(data3[0].NoOfInvoices);
        TotalGross += parseFloat(data3[0].Gross);
        TotalSales += parseFloat(data3[0].Sales);
        TotalTax += parseFloat(data3[0].Tax);
        var avgperinvoice = 0.0;
        if (parseFloat(data3[0].NoOfInvoices) > 0) {
            avgperinvoice = parseFloat(data3[0].Sales) / parseFloat(data3[0].NoOfInvoices);
        }
        $("#service-type-takeaway-no-of-invoices").text(data3[0].NoOfInvoices);
        $("#service-type-takeaway-avg-per-invoice").text(avgperinvoice.toFixed(2));
    }
    $("#service-type-total-no-of-invoices").text(TotalNoOfInvoices);
    $("#service-type-total-gross-sales").text(TotalGross.toFixed(2));
    $("#service-type-total-net-sales").text(TotalSales.toFixed(2));
    $("#service-type-total-tax").text(TotalTax.toFixed(2));

    //Third Party Delivery Data
    $('#service-type-delivery-channel-data').empty();
    if(data4.length > 0)
    {
        for (var i = 0, len = data4.length; i < len; ++i) {
            var row = $(' <tr><td style="width:60%;">' + data4[i].ChannelType + '</td><td style="width:15%;" align="right">' + data4[i].NoOfInvoices + ' x </td><td style="width:25%;"  align="right">' + data4[i].NetAmunt + '</td></tr>');
            $('#service-type-delivery-channel-data').append(row);
        }
    }

    //Discount Detail
    $('#service-type-discount-data').empty();
    if (data5.length > 0)
    {
        for (var i = 0, len = data5.length; i < len; ++i) {
            var row = $(' <tr><td style="width:60%;">' + data5[i].DiscountTypeName + '</td><td style="width:15%;" align="right">' + data5[i].NoOfInvoices + ' x </td><td style="width:25%;"  align="right">' + data5[i].DISCOUNT + '</td></tr>');
            $('#service-type-discount-data').append(row);
        }
    }

    //Payment Mode
    $('#service-type-payment-mode').empty();
    if (data6.length > 0) {
        for (var i = 0, len = data6.length; i < len; ++i) {
            var row = $(' <tr><td style="width:60%;">' + data6[i].ACCOUNT_NAME + '</td><td style="width:15%;" align="right">' + data6[i].NoOfInvoices + ' x </td><td style="width:25%;"  align="right">' + data6[i].NetAmunt + '</td></tr>');
            $('#service-type-payment-mode').append(row);
        }
    }

    //GST Detail
    $('#service-type-gst-data').empty();
    if (data7.length > 0) {
        for (var i = 0, len = data7.length; i < len; ++i) {
            var row = $(' <tr><td style="width:60%;">' + data7[i].GSTPER + ' %</td><td style="width:15%;" align="right">' + data7[i].NoOfInvoices + ' x </td><td style="width:25%;" align="right">' + data7[i].NetAmunt + '</td></tr>');
            $('#service-type-gst-data').append(row);
        }
    }

    var currentdate = new Date();
    var datetime = currentdate.getDate() + "/"
                    + (currentdate.getMonth() + 1) + "/"
                    + currentdate.getFullYear() + " "
                    + moment().format('hh:mm A')
    $("#current-work-date-service-wise-sales").text(datetime);
    $("#location-name-service-wise-sales").text($("#hfLocationName").val());
    $("#cashierservice-wise-sales").text($("#user-detail-bold").text());

    $.print("#service-wise-report");
}
function weekAndDay(date) {
    var arrayWeek = [];
    var date = new Date(date),
    days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
    prefixes = ['', 'First', 'Second', 'Third', 'Fourth', 'Fifth'];
    if (date.getDate() % 7 == 0) {
        var prefixes2 = date.getDate() / 7;
    } else {
        var prefixes2 = Math.ceil(date.getDate() / 7);
    }
    arrayWeek.push({
        'dayPos': prefixes[prefixes2],
        'dayName': days[date.getDay()]
    });
    return arrayWeek;
}