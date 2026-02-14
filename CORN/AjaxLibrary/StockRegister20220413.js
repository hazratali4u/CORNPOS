$(document).ready(function () {
});
function SKUCodeKeyPress(event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode == 13) {
        $("[id$='btnAdd']").trigger("click");
        return false;
    }
    else {
        return true;
    }
}
function CalculateNetAmount() {
    var GrossAmount = $("[id$='txtTotalAmount']").val();
    var Discount = $("[id$='txtDiscount']").val();
    var Gst = $("[id$='txtGstAmount']").val();
    var freight = $("[id$='txtFreight']").val();

    if (GrossAmount == "") {
        GrossAmount = 0;
    }
    if (Discount == "") {
        Discount = 0;
    }
    if (Gst == "") {
        Gst = 0;
    }
    if (freight == "") {
        freight = 0;
    }
    $("[id$='txtNetAmount']").val((parseFloat(GrossAmount) + parseFloat(Gst) - parseFloat(Discount) + parseFloat(freight)).toFixed(2));
}
function CalculateAmount() {
    var Qty = $("[id$='txtQuantity']").val();
    var Rate = $("[id$='txtPrice']").val();
    $("[id$='txtAmount']").val((Qty * Rate).toFixed(2));
}
function ValidateForm() {
    var str;
    str = $("[id$='txtQuantity']").val();
    if ((str == null || str.length == 0) && DocNo.GetText() == "New") {
        alert('Must Enter Quantity');
        $("[id$='txtQuantity']").focus();
        return false;
    }
    str = $("[id$='txtPrice']").val();
    if ((str == null || str.length == 0) && DocNo.GetText() == "New") {
        if (DrpDocument.GetValue() == 2) {
            alert('Must Enter Price');
            $("[id$='txtPrice']").focus();
            return false;
        }
    }
    str = $("[id$='txtDocumentNo']").val();
    var lblInvoice = $("[id$='lblInvoice']").text();
    if (str == null || str.length == 0) {
        if (lblInvoice == 'Driver Name') {
            $("[id$='txtDocumentNo']").focus();
            alert('Driver Name is required');
        } else {
            $("[id$='txtDocumentNo']").focus();
            alert('Must Enter Invoice/DC No');
        }
        return false;
    }
    return true;
}
function QtyKeyPress(txt, event) {
    var charCode = (event.which) ? event.which : event.keyCode;
    if (hfInventoryType == '0') {
        if (charCode == 9 || charCode == 8) {
            return true;
        }
        if (charCode == 46) {
            if (txt.value.indexOf(".") < 0)
                return true;
            return false;
        }
        if (charCode == 31 || charCode < 48 || charCode > 57)
            return false;
    }
    else {
        return false
    }
    return true;
}