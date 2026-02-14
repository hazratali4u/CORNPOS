$(document).ready(function () {
    $('.key3').click(function () {
        var txtKeyBoad = document.getElementById('txtKeyBoad');
        if (this.innerHTML == '0') {
            if (txtKeyBoad.value.length > 0)
                txtKeyBoad.value = txtKeyBoad.value + this.innerHTML;
        }
        else {
            if ($("#hfKeyBoadTextBoxID").val() == "txtCoverTable") {
                if (txtKeyBoad.value.length < 4) {
                    txtKeyBoad.value = txtKeyBoad.value + this.innerHTML;
                }
            }
            else {
                txtKeyBoad.value = txtKeyBoad.value + this.innerHTML;
            }
        }
        event.stopPropagation();
    });

    $('.key3Decimal').click(function () {
        var txtKeyBoadDecimal = document.getElementById('txtKeyBoadDecimal');
        if (this.innerHTML == '.') {
            if (txtKeyBoadDecimal.value.indexOf(".") === -1)
            {
                txtKeyBoadDecimal.value = txtKeyBoadDecimal.value + this.innerHTML;
            }
        }
        else if (this.innerHTML == '0') {
            if (txtKeyBoadDecimal.value.length > 0)
                txtKeyBoadDecimal.value = txtKeyBoadDecimal.value + this.innerHTML;
        }
        else {
            if ($("#hfKeyBoadTextBoxID").val() == "txtCoverTable") {
                if (txtKeyBoadDecimal.value.length < 4) {
                    txtKeyBoadDecimal.value = txtKeyBoadDecimal.value + this.innerHTML;
                }
            }
            else {
                txtKeyBoadDecimal.value = txtKeyBoadDecimal.value + this.innerHTML;
            }
        }
        event.stopPropagation();
    });

    $('.Custombtn3').click(function () {
        document.getElementById('txtKeyBoad').value = '';
        event.stopPropagation();
    });

    $('.Custombtn3Decimal').click(function () {
        document.getElementById('txtKeyBoadDecimal').value = '';
        event.stopPropagation();
    });

    $('.Custombtn23').click(function () {
        $("#" + $("#hfKeyBoadTextBoxID").val() + "").val($("#txtKeyBoad").val());
        $('#Customkeyboad').hide("slow");
        if ($("#hfKeyBoadTextBoxID").val() == "txtDiscount")
        {
            $("#txtDiscount").keyup();
        }
        if ($("#hfKeyBoadTextBoxID").val() == "txtDiscount2") {
            $("#txtDiscount2").keyup();
        }
        if ($("#hfKeyBoadTextBoxID").val() == "txtCashRecieved") {
            $("#txtCashRecieved").keyup();
        }
        $("#txtKeyBoad").val('');
    });
    $('.Custombtn23Decimal').click(function () {
        $("#" + $("#hfKeyBoadTextBoxIDDecimal").val() + "").val($("#txtKeyBoadDecimal").val());
        $('#CustomkeyboadDecimal').hide("slow");
        if ($("#hfKeyBoadTextBoxIDDecimal").val() == "txtDiscount") {
            $("#txtDiscount").keyup();
        }
        if ($("#hfKeyBoadTextBoxIDDecimal").val() == "txtDiscount2") {
            $("#txtDiscount2").keyup();
        }
        if ($("#hfKeyBoadTextBoxIDDecimal").val() == "txtCashRecieved") {
            $("#txtCashRecieved").keyup();
        }
        $("#txtKeyBoadDecimal").val('');
    });
});

function ShowCustomKeyBoad(event) {
    $('#Customkeyboad').show("slow");    
    $("#hfKeyBoadTextBoxID").val(event.id);
    if ($("#hfKeyBoadTextBoxID").val() == "txtManualOrderNo") {
        $("#txtKeyBoad").attr("placeholder", "Enter Manual KOT No").blur();
    }
    else if ($("#hfKeyBoadTextBoxID").val() == "txtCoverTable") {
        if (document.getElementById("hfCustomerType").value == "Takeaway")
        {
            $("#txtKeyBoad").attr("placeholder", "Enter Token ID").blur();
        }
        else {
            if ($("#hfEatIn").val() == "1") {
                $("#txtKeyBoad").attr("placeholder", "Enter Token ID").blur();
            }
            else {
                $("#txtKeyBoad").attr("placeholder", "Enter Cover Table").blur();
            }
        }
    }
    else if ($("#hfKeyBoadTextBoxID").val() == "txtDiscount") {
        $("#txtKeyBoad").attr("placeholder", "Enter Discount").blur();
    }
    else if ($("#hfKeyBoadTextBoxID").val() == "txtCashRecieved") {
        $("#txtKeyBoad").attr("placeholder", "Enter Payment Received").blur();
    }
    else if ($("#hfKeyBoadTextBoxID").val() == "txtDiscount2") {
        $("#txtKeyBoad").attr("placeholder", "Enter Discount").blur();
    }
    document.getElementById('txtKeyBoad').focus();
}
function ShowCustomKeyBoadDecimal(event) {
    $('#CustomkeyboadDecimal').show("slow");
    $("#hfKeyBoadTextBoxIDDecimal").val(event.id);
    if ($("#hfKeyBoadTextBoxIDDecimal").val() == "txtManualOrderNo") {
        $("#txtKeyBoadDecimal").attr("placeholder", "Enter Manual KOT No").blur();
    }
    else if ($("#hfKeyBoadTextBoxIDDecimal").val() == "txtCoverTable") {
        $("#txtKeyBoadDecimal").attr("placeholder", "Enter Cover Table").blur();
    }
    else if ($("#hfKeyBoadTextBoxIDDecimal").val() == "txtDiscount") {
        $("#txtKeyBoadDecimal").attr("placeholder", "Enter Discount").blur();
    }
    else if ($("#hfKeyBoadTextBoxIDDecimal").val() == "txtCashRecieved") {
        $("#txtKeyBoadDecimal").attr("placeholder", "Enter Payment Received").blur();
    }
    else if ($("#hfKeyBoadTextBoxIDDecimal").val() == "txtDiscount2") {
        $("#txtKeyBoadDecimal").attr("placeholder", "Enter Discount").blur();
    }
    document.getElementById('txtKeyBoadDecimal').focus();
}