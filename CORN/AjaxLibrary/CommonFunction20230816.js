$(window).keydown(function (event) {
    if (event.altKey && (event.which == 72)) {//Alt + H (Hold Order)
        $('#dvHold').trigger("click")
        event.preventDefault();
    }
    else if (event.altKey && (event.which == 78)) {//Alt + N (New Order)
        NewOrder();
        event.preventDefault();
    }
    else if (event.altKey && (event.which == 79)) {//Alt + P (Print Order)
        PrintOrder();
        event.preventDefault();
    }
    else if (event.altKey && (event.which == 73)) {//Alt + I (Print Invoice)
        $('#PrintInvoice').trigger("click")
        event.preventDefault();
    }
    else if (event.altKey && (event.which == 80)) {//Alt + P (Payments)
        $('#Payments').trigger("click")
        event.preventDefault();
    }
    else if (event.altKey && (event.which == 82)) {//Alt + R (Reports)
        $('#btnShowReports').trigger("click")
        event.preventDefault();
    }
    else if (event.altKey && (event.which == 75)) {//Alt+ K (Manual KOT No)
        document.getElementById('txtManualOrderNo').focus();
        document.getElementById("txtManualOrderNo").click();
        event.preventDefault();
    }
    else if (event.altKey && (event.which == 18)) {//Alt + S (Save 2)
        if (document.getElementById("hfHiddenReports").value == '1') {
            if ($('#payment').is(":visible")) {
                SaveInvoice(1);
            }
            event.preventDefault();
        }
    }
    else if (event.altKey && (event.which == 67)) {//Alt+ C (Item Code)
        try {
            document.getElementById('ddlItem').focus();
        } catch (e) {

        }
        try {
            document.getElementById('txtSKUCode').focus();
        } catch (e) {

        }
        event.preventDefault();
    }
    else if (event.which == 27) {//Esc Close Short Cut Keys Popup
        CloseShortKeyHelp();
        event.preventDefault();
    }
    else if (event.altKey && (event.which == 112)) {//Alt + F1 (Short Cut Keys Popup)
        ShwoShortKeyHelp();
        event.preventDefault();
    }
    else if (event.altKey && (event.which == 76)) {//Alt + L (Cover Tables)
        document.getElementById('txtCoverTable').focus();
        event.preventDefault();
    }
    else if (event.altKey && (event.which == 68)) {//Alt + D (Orderbooker ddl)
        document.getElementById('ddlOrderBooker').focus();
        document.getElementById("ddlOrderBooker").click();
        event.preventDefault();
    }
    else if (event.altKey && (event.which == 84)) {//
        document.getElementById('txtVacantTable').focus();
        event.preventDefault();
    }
});

function ShwoShortKeyHelp() {
    $('#ShortCutKeyHelp').show("slow");
}
function CloseShortKeyHelp() {
    $('#ShortCutKeyHelp').hide("slow");
}
function ShwoOpenItemPopup() {
    $('#dvOpenItem').show("slow");
}
function CloseOpenItemPopup() {
    $("#txtOpentItemName").val('');
    $("#txtQuantityOpenItem").val('');
    $("#txtPriceOpenItem").val('');
    $('#dvOpenItem').hide("slow");
}