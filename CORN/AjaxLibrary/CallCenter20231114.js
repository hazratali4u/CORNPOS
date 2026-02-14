function waitLoading(msg) {
    new $.Zebra_Dialog('<strong>' + msg + '</strong>', {
        'buttons': false,
        'position': ['top + 120'],
        'auto_close': 600
    });
}


function EnableDisableService() {

    $('#lnkDineIn').hide();
    $('#lnkDelivery').hide();
    $('#lnkTakeaway').hide();

    if (document.getElementById("hfCan_DineIn").value == "True") {
        $('#lnkDineIn').show();
    }
    if (document.getElementById("hfCan_Delivery").value == "True") {
        $('#lnkDelivery').show();
    }
    if (document.getElementById("hfCan_TakeAway").value == "True") {
        $('#lnkTakeaway').show();
    }
}
//========== #region Load===================\\

//=======Discount User
function loadUsers(obj) {
    document.getElementById("dvLoyaltyCard").setAttribute("style", "display:none;");
    document.getElementById("dvDiscountUser").setAttribute("style", "display:none;");
    document.getElementById("dvAuthorityUser").setAttribute("style", "display:none;");
    document.getElementById("percentage").disabled = true;
    document.getElementById("value").disabled = true;
    document.getElementById("percentage").style["background-color"] = "#919399";
    document.getElementById("value").style["background-color"] = "#919399";

    $('#txtLoyaltyCustomer').val('');
    $('#txtDiscount').val('');
    $('#txtTotalPoints').val('');
    $('#txtRedeemedPoints').val('');
    $('#txtBalancePoints').val('');
    $('#txtAvailableDiscount').val('');
    $('#txtAllowedLimit').val('');
    $('#txtDiscountAvail').val('');
    $('#txtDiscountBalance').val('');

    if (obj.value == "2") {
        document.getElementById('txtDiscount').disabled = true;
        document.getElementById("dvLoyaltyCard").setAttribute("style", "display:block;");
        document.getElementById('txtLoyaltyCard').focus();
    }
    else if (obj.value == "1") {
        document.getElementById("dvDiscountUser").setAttribute("style", "display:block;");
        document.getElementById("dvAuthorityUser").setAttribute("style", "display:block;");
        loadLimit(document.getElementById("ddlDiscountUser"));
        loadPassword(document.getElementById("ddlDiscountUser2"));
        DiscType(document.getElementById("value"));
        document.getElementById('txtDiscount').value = $('#lblLimit').text();
        CalculateBalance();
    }
    else if (obj.value == "0") {

        if (document.getElementById("hfCustomerType").value == "Takeaway") {
            document.getElementById("percentage").disabled = false;
            document.getElementById("value").disabled = false;
        }

        document.getElementById('txtDiscount').value = document.getElementById('txtDiscount2').value;
        if (document.getElementById('txtDiscount').value != "") {
            if ($('#hfchkDiscountType').val() == 0) {
                DiscType(document.getElementById("percentage"));
            }
            else if ($('#hfchkDiscountType').val() == 1) {
                DiscType(document.getElementById("value"));
            }
        }
        else {

            document.getElementById("percentage").style["background-color"] = "#919399";
            document.getElementById("value").style["background-color"] = "#919399";
        }
        CalculateBalance();
        document.getElementById('txtDiscount').disabled = true;
    }
}
//=======Discount User On Print Invoice Screen
function loadUsers2(obj) {
    document.getElementById("dvLoyaltyCard2").setAttribute("style", "display:none;");
    document.getElementById("dvDiscountUser2").setAttribute("style", "display:none;");
    document.getElementById("dvAuthorityUser2").setAttribute("style", "display:none;");
    document.getElementById("percentage2").disabled = true;
    document.getElementById("value2").disabled = true;
    //---on print invoice popup
    document.getElementById("percentage2").style["background-color"] = "#919399";
    document.getElementById("value2").style["background-color"] = "#919399";

    $('#txtLoyaltyCustomer2').val('');
    $('#txtDiscount2').val('');
    $('#txtNoOfVisits2').val('');
    $('#txtTotalPurchased2').val('');
    $('#txtTotalLoyaltyDiscount2').val('');
    $('#txtLoyaltyQuantity2').val('');

    $('#txtTotalPoints2').val('');
    $('#txtRedeemedPoints2').val('');
    $('#txtBalancePoints2').val('');
    $('#txtAvailableDiscount2').val('');
    $('#txtAvailableCash2').val('');

    $('#txtAllowedLimit2').val('');
    $('#txtDiscountAvail2').val('');
    $('#txtDiscountBalance2').val('');

    if (obj.value == "2") {
        document.getElementById('txtDiscount2').disabled = true;
        document.getElementById('txtDiscountReason2').disabled = true;
        document.getElementById("dvLoyaltyCard2").setAttribute("style", "display:block;");
        document.getElementById('txtLoyaltyCard2').focus();
    }
    else if (obj.value == "1") {
        document.getElementById("dvDiscountUser2").setAttribute("style", "display:block;");
        document.getElementById("dvAuthorityUser2").setAttribute("style", "display:block;");
        loadLimit2(document.getElementById("ddlDiscountUser2"));
        loadPassword(document.getElementById("ddlDiscountUser4"));
        DiscType(document.getElementById("value2"));
        document.getElementById('txtDiscount2').value = $('#lblLimit2').text();
        CalculateBalance2();
    }
    else if (obj.value == "0") {
        document.getElementById("percentage2").disabled = false;
        document.getElementById("value2").disabled = false;

        if (document.getElementById('txtDiscount2').value != "") {
            if ($('#hfchkDiscountType').val() == 0) {
                DiscType(document.getElementById("percentage2"));
            }
            else if ($('#hfchkDiscountType').val() == 1) {
                DiscType(document.getElementById("value2"));
            }
        }
        else {

            document.getElementById("percentage2").style["background-color"] = "#919399";
            document.getElementById("value2").style["background-color"] = "#919399";
        }
        CalculateBalance2();
        document.getElementById('txtDiscount2').disabled = true;
        document.getElementById('txtDiscountReason2').disabled = true;
    }

}

function loadLimit(obj) {
    $('#lblLimit').text(0);
    $('#tble-discount-limit').find('tr').each(function () {
        if (obj.value == $(this).find("td:eq(0)").text()) {
            $('#lblLimit').text($(this).find("td:eq(1)").text());
            return;
        }
    });
}

function loadLimit2(obj) {
    $('#lblLimit2').text(0);
    $('#tble-discount-limit2').find('tr').each(function () {
        if (obj.value == $(this).find("td:eq(0)").text()) {
            $('#lblLimit2').text($(this).find("td:eq(1)").text());
            return;
        }
    });
}

function loadPassword(obj) {
    $('#hfManagerPassword').val('');
    $('#tble-discount-user').find('tr').each(function () {
        if (obj.value == $(this).find("td:eq(0)").text()) {
            $('#hfManagerPassword').val($(this).find("td:eq(1)").text());
            return;
        }
    });

    $('#tble-discount-user2').find('tr').each(function () {
        if (obj.value == $(this).find("td:eq(0)").text()) {
            $('#hfManagerPassword').val($(this).find("td:eq(1)").text());
            return;
        }
    });
}

function loadDiscountUser() {
    $.ajax
       (
           {
               type: "POST", //HTTP method
               url: "frmCallCenterOld.aspx/LoadDiscountUser", //page/method name
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: addDiscountUser
           }
       );
}

function addDiscountUser(response) {

    var xmlDoc = $.parseXML(response.d);
    var xml = $(xmlDoc);
    var Users = xml.find("Table");


    $("#ddlDiscountUser").empty();
    $("#ddlDiscountUser3").empty();
    $('#tble-discount-limit').empty();
    $('#tble-discount-limit2').empty();

    var listItems = "";
    $(Users).each(function () {
        listItems += "<option value='" + $(this).find("USER_ID").text() + "'>" + $(this).find("USER_NAME").text() + "</option>";
        var row = $('<tr ><td>' + $(this).find("USER_ID").text() + '</td><td>' + $(this).find("EMC_LimitPerDay").text() + '</td></tr>');
        $('#tble-discount-limit').append(row);
        $('#tble-discount-limit2').append(row);
    });

    $("#ddlDiscountUser").html(listItems);
    $("#ddlDiscountUser3").html(listItems);
    //------------------------------------------------------------------------------------------------------------------

    var DiscountUser = xml.find("Table1");
    $('#tble-discount-user').empty();
    $('#tble-discount-user2').empty();

    var listItems2 = "";
    $(DiscountUser).each(function () {
        listItems2 += "<option value='" + $(this).find("USER_ID").text() + "'>" + $(this).find("USER_NAME").text() + "</option>";
        var row = $('<tr ><td>' + $(this).find("USER_ID").text() + '</td><td>' + $(this).find("PASSWORD").text() + '</td></tr>');
        $('#tble-discount-user').append(row);
        $('#tble-discount-user2').append(row);
    });

    $("#ddlDiscountUser2").html(listItems2);
    $("#ddlDiscountUser4").html(listItems2);

}

//=======Product and Category
function loadAllProducts() {
    $.ajax
        (
            {
                type: "POST", //HTTP method
                url: "frmCallCenterOld.aspx/LoadAllProducts", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ LocationID: ddlLocation.GetValue() }),
                success: sethfProductValue,
                error: OnError
            }
        );
}

function LoadAllModifiers()
{
    $.ajax
        (
            {
                type: "POST", //HTTP method
                url: "frmCallCenterOld.aspx/LoadModifierItems", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: setModifierItems,
                error: OnError
            }
        );
}

function setModifierItems(products) {
    products = JSON.stringify(products);
    var result = jQuery.parseJSON(products.replace(/&quot;/g, '"'));
    products = eval(result.d);
    products = JSON.stringify(products);
    document.getElementById("hfModifierItems").value = products;
}

function sethfProductValue(products) {

    products = JSON.stringify(products);

    var result = jQuery.parseJSON(products.replace(/&quot;/g, '"'));

    products = eval(result.d);
    products = JSON.stringify(products);
    document.getElementById("hfProduct").value = products;

    loadProductCategory("btnMenuItem");

}

function loadProductCategory(obj) {

    var CategoryType = false;
    $("#hfCategoryType").val("0");

    //#region Modifier Button
    if (obj.id == "btnModifierItem") {
        if ($("#hfSkuId").val() == "") {
            Error("Plz Select Item in Menu");
            return;
        }
       
        CategoryType = true;
        $("#hfCategoryType").val("1");
        document.getElementById('dvDealQty').style.display = "none";
        document.getElementById('dvDealUpdate').style.display = "none";
        document.getElementById('dv_lstModifyCategory').style.display = "block";
        document.getElementById('dv_lstModifyProducts').style.display = "block";
        document.getElementById('btnMenuItem').style.backgroundColor = "#eff2bd";
        document.getElementById('btnModifierItem').style.backgroundColor = "#d4def7";
        $('#divModifierParentName').html('');
        $('#divModifier').show("slow");

        $.ajax
     (
      {
          type: "POST", //HTTP method
          url: "frmCallCenterOld.aspx/LoadProductCategroy", //page/method name
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          data: JSON.stringify({ ItemType: CategoryType, ItemId: $("#hfSkuId").val() }),
          success: addModifierCategories,
          error: OnError
      }
     );
    }

        //#region Menu Button
    else {

        resetColor();
        $("#hfRow").val('');
        $("#hfRowIndex").val('');

        document.getElementById('btnModifierItem').style.backgroundColor = "#eff2bd";
        document.getElementById('btnMenuItem').style.backgroundColor = "#d4def7";
        document.getElementById('dv_lstModifyCategory').style.display = "none";
        document.getElementById('dv_lstModifyProducts').style.display = "none";
        document.getElementById('dv_lstCategory').style.display = "block";
        document.getElementById('dv_lstProducts').style.display = "block";
        a = document.getElementById("dv_lstCategory").children;
        if (a.length == 0) {
            $.ajax
           (
            {
                type: "POST", //HTTP method
                url: "frmCallCenterOld.aspx/LoadProductCategroy", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ ItemType: CategoryType, ItemId: 0 }),
                success: addCategories,
                error: OnError
            }
           );
        }
    }
}

//=======Product Closing Status on Item Click
function LoadProductStatus(skuId) {
    if ($("#hfStockStatus").val() == "True") {
        document.getElementById('dvClosingStock').style.display = "block";

        $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmCallCenterOld.aspx/LoadProductStatus", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ id: skuId, LocationID: ddlLocation.GetValue() }),
                    success: ProductStatus,
                    error: OnError
                }
            );
    }
}

function ProductStatus(productStatus) {
    productStatus = JSON.stringify(productStatus);
    var result = jQuery.parseJSON(productStatus.replace(/&quot;/g, '"'));
    productStatus = eval(result.d);

    $("#txtClosingStock").val(0);

    if (productStatus.length != "0") {
        $("#txtClosingStock").val(productStatus[0].CLOSING_STOCK);
    }
}
/* function ValidateClosing(skuId, qty) {
     if ($("#hfOversale").val() == "False") {

         LoadProductStatus(skuId);

         if (parseFloat($('#hfClosingStock').val()) < qty) {
             Error("Plz check your Closing Stock");
             return false;
         }

         if ($("#tble-ordered-products tr").length > 0) {
             var flag = true;
             $('#tble-ordered-products').find('tr').each(function () {
                 if (checkVoid($(this).find("td:eq(13)").text())) {//CHECK IS VOID OR NOT
                     if (skuId == $(this).find("td:eq(0)").text()) {
                         if (parseFloat($('#hfClosingStock').val()) < parseFloat($(this).find("td:eq(3) input").val()) + 1) {
                             flag = false;
                         }
                         else {
                             flag = true;
                         }
                         return;
                     }
                 }
             });
             if (flag) {
                 return true;
             }
             else {
                 Error("Plz check your Closing Stock");
                 return false;
             }
         }
     }
     return true;
 }
 */
//=======Selected Bill Detail
function GetPendingBill(saleInvoiceMasterId) {

    $.ajax
        (
            {
                type: "POST", //HTTP method
                url: "frmCallCenterOld.aspx/GetPendingBill", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{'saleInvoiceMasterId':'" + saleInvoiceMasterId + "'}",
                success: LoadPendingBill
            }
        );
}

function LoadPendingBill(products) {

    products = JSON.stringify(products);
    var result = jQuery.parseJSON(products.replace(/&quot;/g, '"'));
    products = eval(result.d);

    if (products.length > 0) {
        $('#txtRemarks').val(products[0].REMARKS);
        $('#txtManualOrderNo').val(products[0].MANUAL_ORDER_NO);
        $('#hfCustomerName').val(products[0].CUSTOMER_NAME);
        $('#hfCustomerIDNew').val(products[0].CUSTOMER_ID);
    }

    $("#tble-ordered-products").empty();
    $("#hfCounter").val(0);
    for (var i = 0, len = products.length; i < len; ++i) {
        addProductToOrderedProduct(products, i, $("#OrderNo1").text());
    }
    var tableData = storeTblValues();
    tableData = JSON.stringify(tableData);
    document.getElementById('hfOrderedproducts').value = tableData;
    setTotals();
}

//=======Pending Bills
function GetPendingBills() {

    $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmCallCenterOld.aspx/SelectPendingBills", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ customerType: 'Delivery', LocationID: ddlLocation.GetValue() }),
                    success: LoadPendingBills
                }
            );
}

function LoadPendingBills(pendingBills) {
    var data = $.parseJSON(pendingBills.d);
    var data2 = data.Table1;
    pendingBills = data.Table;

    $("#tbl-pending-bills").empty();
    var row = "";
    for (var i = 0, len = pendingBills.length; i < len; i++) {
        if (pendingBills[i].INVOICE_ID > 0) {
            if (pendingBills[i].SERVICE_TYPE == 'Delivery') {
                if (pendingBills[i].Delivery_Status == null)
                    pendingBills[i].Delivery_Status = "Pending";
                var deliveryTime = pendingBills[i].DeliveryTimeStamp != null && pendingBills[i].DeliveryTimeStamp != "" && pendingBills[i].DeliveryTimeStamp != undefined ? getDateDiff(new Date(parseInt(pendingBills[i].DeliveryTimeStamp.substr(6)))) : "";
                if (pendingBills[i].LOCKED || pendingBills[i].Order_Delivery_Status_ID ===3) {
                    row = $('<tr ><td style="display:none;">' + pendingBills[i].INVOICE_ID + '</td><td class="  " style="background-color: #f4e27e;">' + pendingBills[i].TABLE_NO + '</td><td class="rightval" style="background-color: #f4e27e;">' + pendingBills[i].AMOUNTDUE + '</td><td style="display:none;">' + pendingBills[i].DISCOUNT + '</td><td style="display:none;">' + pendingBills[i].DISCOUNT_TYPE + '</td><td style="display:none;">' + pendingBills[i].coverTable + '</td><td style="display:none;">' + pendingBills[i].orderBookerId + '</td><td style="display:none;">' + pendingBills[i].approvedBy + '</td><td style="display:none;">' + pendingBills[i].SERVICE_CHARGES + '</td> <td style="font-size: 12px;color: red;background-color: #f4e27e">' + pendingBills[i].Delivery_Status + '</td> <td style="font-size: 12px;color: red;background-color: #f4e27e;">' + deliveryTime + '</td> <td style="font-size: 12px;color: red;background-color: #f4e27e">' + diff(pendingBills[i].TIME_STAMP, document.getElementById('ct').innerHTML) + '</td><td style="display:none;">' + pendingBills[i].TABLE_NO2 + '</td><td style="display:none;">' + pendingBills[i].CUSTOMER_ID + '</td><td style="display:none;">' + pendingBills[i].CONTACT_NUMBER + '</td><td style="display:none;">' + pendingBills[i].ORDER_NO + '</td><td style="display:none;">' + pendingBills[i].TABLE_ID + '</td><td style="display:none;">' + pendingBills[i].CONTACT_NUMBER + '</td><td style="display:none;">' + pendingBills[i].CARD_NO + '</td><td style="display:none;">' + pendingBills[i].CARD_TYPE_ID + '</td><td style="display:none;">' + pendingBills[i].CARD_POINTS + '</td><td style="display:none;">' + pendingBills[i].PURCHASING + '</td><td style="display:none;">' + pendingBills[i].AMOUNT_LIMIT + '</td><td style="display:none;">' + pendingBills[i].EmpDiscountType + '</td><td style="display:none;">' + pendingBills[i].LOCKED + '</td><td style="display:none;">' + pendingBills[i].LOCKED_BY + '</td><td style="display:none;">' + pendingBills[i].Order_Delivery_Status_ID + '</td><td style="background-color: #f4e27e;" align="center" onclick="SendSMS(this);"><a href="#"><span class="fa fa-mobile"></span></a></td></tr>');
                }
                else {
                    row = $('<tr ><td style="display:none;">' + pendingBills[i].INVOICE_ID + '</td><td class="leftval">' + pendingBills[i].TABLE_NO + '</td><td class="rightval">' + pendingBills[i].AMOUNTDUE + '</td><td style="display:none;">' + pendingBills[i].DISCOUNT + '</td><td style="display:none;">' + pendingBills[i].DISCOUNT_TYPE + '</td><td style="display:none;">' + pendingBills[i].coverTable + '</td><td style="display:none;">' + pendingBills[i].orderBookerId + '</td><td style="display:none;">' + pendingBills[i].approvedBy + '</td><td style="display:none;">' + pendingBills[i].SERVICE_CHARGES + '</td> <td style="font-size: 12px;color: red;">' + pendingBills[i].Delivery_Status + '</td> <td style="font-size: 12px;color: red;">' + deliveryTime + '</td> <td style="font-size: 12px; color: red;">' + diff(pendingBills[i].TIME_STAMP, document.getElementById('ct').innerHTML) + '</td><td style="display:none;">' + pendingBills[i].TABLE_NO2 + '</td><td style="display:none;">' + pendingBills[i].CUSTOMER_ID + '</td><td style="display:none;">' + pendingBills[i].CONTACT_NUMBER + '</td><td style="display:none;">' + pendingBills[i].ORDER_NO + '</td><td style="display:none;">' + pendingBills[i].TABLE_ID + '</td><td style="display:none;">' + pendingBills[i].CONTACT_NUMBER + '</td><td style="display:none;">' + pendingBills[i].CARD_NO + '</td><td style="display:none;">' + pendingBills[i].CARD_TYPE_ID + '</td><td style="display:none;">' + pendingBills[i].CARD_POINTS + '</td><td style="display:none;">' + pendingBills[i].PURCHASING + '</td><td style="display:none;">' + pendingBills[i].AMOUNT_LIMIT + '</td><td style="display:none;">' + pendingBills[i].EmpDiscountType + '</td><td style="display:none;">' + pendingBills[i].LOCKED + '</td><td style="display:none;">' + pendingBills[i].LOCKED_BY + '</td><td style="display:none;">' + pendingBills[i].Order_Delivery_Status_ID + '</td><td align="center" onclick="SendSMS(this);"><a href="#"><span class="fa fa-mobile"></span></a></td></tr>');
                }
            }
            $('#tbl-pending-bills').append(row);
        }
    }
    $("#lblCounter1").text("(" + 0 + ")");
    $("#lblCounter2").text("(" + 0 + ")");
    $("#lblCounter3").text("(" + 0 + ")");

    if (data2.length != "0") {
        $("#lblCounter1").text("(" + data2[0].Takeaway + ")");
        $("#lblCounter2").text("(" + data2[0].Delivery + ")");
        $("#lblCounter3").text("(" + data2[0].DineIn + ")");
    }
}

//========== #endregion Load===================\\

$(document).ready(function () {
    document.getElementById("percentage2").disabled = true;
    document.getElementById("value2").disabled = true;
    document.getElementById("percentage").disabled = true;
    document.getElementById("value").disabled = true;
    LoadLocationDropDown();
    if ($("#hfIS_CanGiveDiscount").val() == 'False')
    {
        $("#ddlDiscountType option[value='0']").remove();
        $("#ddlDiscountType option[value='1']").remove();
        $("#ddlDiscountType2 option[value='0']").remove();
        $("#ddlDiscountType2 option[value='1']").remove();
    }

    $('#txtCustomerSearch').keyup(function () {

        if ($('#txtCustomerSearch').val().length > 3) {
            LoadAllCustomers();
        }
        else if ($('#txtCustomerSearch').val().length < 4) {
            $("#tbl-customers").empty();
        }
    });

    $('#txtVacantTable').keyup(function () {

        $('#dv_lstTable').children('input').each(function () {
            if (this.value.toUpperCase().includes($('#txtVacantTable').val().toUpperCase())) {
                this.style = "display:block;";
            }
            else
            {
                this.style = "display:none;";
            }
        });
    });

    waitLoading('Loading');

    $(".nav-ic img").click(function () {
        $(".exit-text").slideToggle("slow", function () {
        });
    });

    //On Pending Bill row Click
    $("#tbl-pending-bills").delegate("tr", "click", function () {
        ShowBill(this);
    });

    LoadModifiers();
});

function LoadModifiers() {
    $.ajax
        (
            {
                type: "POST", //HTTP method
                url: "frmOrderPOS.aspx/LoadModifiers", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: sethfModifiers,
                error: OnError
            }
        );
}
function sethfModifiers(products) {
    products = JSON.stringify(products);

    var result = jQuery.parseJSON(products.replace(/&quot;/g, '"'));

    products = eval(result.d);
    products = JSON.stringify(products);
    document.getElementById("hfModifiers").value = products;
}
//#region Service Type Call in ShowBill
function activeLink() {

    $("#lnkDineIn").removeClass("active");
    $("#lnkTakeaway").removeClass("active");
    $("#lnkDelivery").removeClass("active");

    if ($("#TableNo1").text() == "DLY") {
        $("#lnkDelivery").addClass("active");
        document.getElementById("hfCustomerType").value = "Delivery";

    } else if ($("#TableNo1").text() == "TKY") {

        $("#lnkTakeaway").addClass("active");
        document.getElementById("hfCustomerType").value = "Takeaway";

    } else {

        $("#lnkDineIn").addClass("active");
        document.getElementById("hfCustomerType").value = "Dine In";
    }
}

//Used in addProductToOrderTable(),LoadPendingBill(), On Payment Click, Print Invoice Click
function calculateDealPrice() {

    var orderedProducts = $("#hfOrderedproducts").val();
    orderedProducts = eval(orderedProducts);

    var uniqueDeals = $.unique(orderedProducts.map(function (d) { return d.I_D_ID; }));
    uniqueDeals = uniqueDeals.sort();
    uniqueDeals = unique(uniqueDeals);

    var totalamount = 0;

    for (var j = 0; j < uniqueDeals.length; j++) {
        if (uniqueDeals[j] != 0) {
            var count = 0;
            $('#tble-ordered-products').find('tr').each(function () {
                if (checkVoid($(this).find("td:eq(13)").text())) {//CHECK IS VOID OR NOT
                    if (uniqueDeals[j] == $(this).find("td:eq(21)").text()) {
                        if (count == 0) {
                            count += 1;
                            totalamount += parseFloat($(this).find("td:eq(20)").text()) * parseFloat($(this).find("td:eq(27)").text());
                        }
                        return;
                    }
                }
            });
        }
    }
    $("#DealPrice").text(totalamount);
}

function checkVoid(color) //check is item void or not //used on plus, delete, settotals
{
    if (color == "false") {
        return true;
    }
    else {
        return false;
    }
}

function setTotals() {

    var subtotal = 0.0;
    var grandTotal;
    var salesTax = 0;// document.getElementById("hfSalesTax").value;

    if ($('#hfServiceCharges').val() == "False") {
        salesTax = document.getElementById("hfSalesTax").value;
    }
    else {
        salesTax = document.getElementById("txtService").value;
    }
    if (salesTax == "") {
        salesTax = 0;
    }

    var discountType = document.getElementById("hfDiscountType").value;
    var discount = document.getElementById('txtDiscount').value;
    var balance = 0;

    calculateDealPrice();

    subtotal = parseFloat($("#DealPrice").text());



    $('#tble-ordered-products tr').each(function (row, tr) {
        if (parseFloat($(tr).find("td:eq(21)").text()) == 0) {
            if (checkVoid($(tr).find('td:eq(13)').text())) {
                subtotal += parseFloat($(tr).find("td:eq(6)").text());
            }
        }
    });



    if (discount > 0) {
        if (discountType == 0) {

            grandTotal = subtotal;

            discount = grandTotal * (discount / 100);
            if ($('#hfServiceCharges').val() == "False") {
                salesTax = parseFloat(salesTax) / 100 * (parseFloat(grandTotal) - parseFloat(discount));
            }
            balance = Math.round((grandTotal - discount + parseFloat(salesTax)), 0);

        }
        else if (discountType == 1 || discountType == 2) {

            grandTotal = subtotal;
            if ($('#hfServiceCharges').val() == "False") {
                salesTax = parseFloat(salesTax) / 100 * (grandTotal - discount);
            }
            balance = Math.round((grandTotal - discount + parseFloat(salesTax)), 0);
        }
    }
    else {

        grandTotal = subtotal;
        if ($('#hfServiceCharges').val() == "False") {
            salesTax = (parseFloat(salesTax) / 100) * subtotal;
        }
        balance = Math.round((parseFloat(salesTax) + subtotal), 0);
    }


    $("#lblTotalGrossAmount").text(balance);

    //----------Payment PopUp--------------\\
    document.getElementById("subTotal").innerHTML = Math.round(subtotal, 0);
    document.getElementById("lblGSTTotal").innerHTML = Math.round(salesTax, 0);
    document.getElementById("GrandTotal").innerHTML = Math.round(grandTotal, 0);
    document.getElementById("lblBalance").innerHTML = "0"; Math.round(0);
    document.getElementById("lblPaymentDue").innerHTML = Math.round(balance);

    //----------Print Invoice PopUp--------------\\
    document.getElementById("subTotal2").innerHTML = Math.round(subtotal, 0);
    document.getElementById("lblGSTTotal2").innerHTML = Math.round(salesTax, 0);
    document.getElementById("GrandTotal2").innerHTML = Math.round(grandTotal, 0);
    document.getElementById("lblPaymentDue2").innerHTML = Math.round(balance);


}

function UnlockRecord() {
    $.ajax
        (
            {
                type: "POST", //HTTP method
                url: "frmCallCenterOld.aspx/UnlockRecord", //page/method name
                contentType: "application/json; charset=utf-8",
                success: UnloclRecordSuccess,
            }
        );
}

function UnloclRecordSuccess() {
}

//#region Modifier Button
function addModifierCategories(categories) {

    var dv_lstModifyCategory = document.getElementById("dv_lstModifyCategory");
    while (dv_lstModifyCategory.hasChildNodes()) {
        dv_lstModifyCategory.removeChild(dv_lstModifyCategory.lastChild);
    }

    categories = JSON.stringify(categories);
    var result = jQuery.parseJSON(categories.replace(/&quot;/g, '"'));
    categories = eval(result.d);

    var catId = 0;
    for (var i = 0, len = categories.length; i < len; ++i) {
        if (i == 0) {
            catId = categories[i].CAT_ID;
        }
        createModifierCategoriesButtons(categories[i].CAT_NAME, categories[i].CAT_ID);
    }
    
    createModifierProductButtons(catId, $("#hfCategoryType").val(), $("#hfSkuId").val());
}

function createModifierCategoriesButtons(value, id) {

    var element = document.createElement("input");
    element.setAttribute("type", "button");
    element.setAttribute("value", value);
    element.setAttribute("name", value);
    element.setAttribute("id", id);
    element.setAttribute("style", "width:99%;");
    element.setAttribute("class", "box-sm");

    element.onclick = function () { // Note this is a function

        document.getElementById('dvDealQty').style.display = "none";
        document.getElementById('dvDealUpdate').style.display = "none";

        createModifierProductButtons(this.id, $("#hfCategoryType").val(), $("#hfSkuId").val());
        changeClass(this);
    };

    var dv_lstModifyCategory = document.getElementById("dv_lstModifyCategory");
    dv_lstModifyCategory.appendChild(element);
}

function createModifierProductButtons(catId, type, skuId) {

    var dv_lstModifyProducts = document.getElementById("dv_lstModifyProducts");
    while (dv_lstModifyProducts.hasChildNodes()) {
        dv_lstModifyProducts.removeChild(dv_lstModifyProducts.lastChild);
    }

    var lstProducts = document.getElementById("hfProduct").value;
    lstProducts = eval(lstProducts);
    var lstModifierProducts = document.getElementById("hfModifiers").value;
    lstModifierProducts = eval(lstModifierProducts);

    var arr = [];
    for (var i = 0, len = lstProducts.length; i < len; ++i) {
        //------------------------------------------------------------------
        if (lstProducts[i].I_D_ID == 0) {
            if (lstProducts[i].CAT_ID == catId && lstProducts[i].MODIFIER == type && lstProducts[i].SKU_ID == skuId) {

                //Create an input type dynamically.   
                var element = document.createElement("input");
                //Assign different attributes to the element. 
                element.setAttribute("type", "button");
                element.setAttribute("value", lstProducts[i].SKU_NAME);
                element.setAttribute("name", lstProducts[i].SKU_NAME);
                element.setAttribute("id", lstProducts[i].ModifierSKU_ID);
                arr.push(lstProducts[i].ModifierSKU_ID);
                element.setAttribute("style", "width:19.7%;background-color:" + lstProducts[i].BUTTON_COLOR);
                element.setAttribute("class", "box-sm3");
                element.onclick = function () { // Note this is a function
                    addProductToOrderTableModifier(this.id, catId, $("#hfModifierItemParentDealID").val());
                };
                dv_lstModifyProducts.appendChild(element);
            }
        }

    }

    //Packages   
    for (var i = 0, len = lstModifierProducts.length; i < len; ++i) {
        if (lstModifierProducts[i].I_D_ID == 0) {
            var id = 0;
            if (lstModifierProducts[i].CAT_ID == catId && lstModifierProducts[i].MODIFIER == type && lstModifierProducts[i].SKU_ID == skuId) {
                var bool = true;
                for (var arri = 0; arri < arr.length; arri++) {
                    if (arr[arri] == lstModifierProducts[i].ModifierSKU_ID) {
                        bool = false;
                        break;
                    }
                }
                if (bool) {//Create an input type dynamically.   
                    var element = document.createElement("input");
                    //Assign different attributes to the element. 
                    element.setAttribute("type", "button");
                    element.setAttribute("value", lstModifierProducts[i].SKU_NAME);
                    element.setAttribute("name", lstModifierProducts[i].SKU_NAME);
                    element.setAttribute("id", lstModifierProducts[i].ModifierSKU_ID);
                    element.setAttribute("style", "width:23%;background-color:" + lstModifierProducts[i].BUTTON_COLOR);
                    element.setAttribute("class", "box-sm3");
                    element.onclick = function () { // Note this is a function
                        addProductToOrderTableModifierPackage(this.id, catId, $("#hfModifierItemParentDealID").val());
                    };
                    dv_lstModifyProducts.appendChild(element);
                }
            }
        }
    }
}
//-----------------------------------------------------------\\

//#region Menu Button
function addCategories(categories) {

    var dv_lstCategory = document.getElementById("dv_lstCategory");
    while (dv_lstCategory.hasChildNodes()) {
        dv_lstCategory.removeChild(dv_lstCategory.lastChild);
    }

    categories = JSON.stringify(categories);
    var result = jQuery.parseJSON(categories.replace(/&quot;/g, '"'));
    categories = eval(result.d);

    var catId = 0;
    var IsDealCategory = 0;
    for (var i = 0, len = categories.length; i < len; ++i) {
        if (i == 0) {
            catId = categories[i].CAT_ID;
            IsDealCategory = categories[i].IsDealCategory;
        }
        createCategoriesButtons(categories[i].CAT_NAME, categories[i].CAT_ID, categories[i].IsDealCategory);
    }
    createProductButtons(catId, $("#hfCategoryType").val(),IsDealCategory);
}

function createCategoriesButtons(value, id, IsDealCategory) {

    var element = document.createElement("input");
    element.setAttribute("type", "button");
    element.setAttribute("value", value);
    element.setAttribute("name", IsDealCategory);
    element.setAttribute("id", id);
    if (IsDealCategory === 1) {
        element.setAttribute("class", "box-sm4");
    }
    else {
        element.setAttribute("class", "box-sm");
    }
    element.onclick = function () { // Note this is a function

        if ($("#hfDealId").val() == "0") {

            document.getElementById('dvDealQty').style.display = "none";
            document.getElementById('dvDealUpdate').style.display = "none";

            createProductButtons(this.id, $("#hfCategoryType").val(),IsDealCategory);
            changeClass2(this);
        }
        else {
            if (CheckCatDealQty($("#hfDealId").val())) {
                document.getElementById('dvDealQty').style.display = "none";
                document.getElementById('dvDealUpdate').style.display = "none";
                createProductButtons(this.id, $("#hfCategoryType").val(),IsDealCategory);
                changeClass2(this);
            }
        }
    };

    var dvLstCategory = document.getElementById("dv_lstCategory");
    dvLstCategory.appendChild(element);
}

function createProductButtons(catId, type, IsDealCategory) {
    if (IsDealCategory == 0) {
        dvDealPanel.style.display = "none";
        dvProductsPanel.style.width = "49.5%";
    }
    else {
        dvDealPanel.style.display = "block";
        dvProductsPanel.style.width = "32.95%";
    }
    $('#hfDefaultCategoryID').val(catId);
    var dvLstProducts = document.getElementById("dv_lstProducts");
    while (dvLstProducts.hasChildNodes()) {
        dvLstProducts.removeChild(dvLstProducts.lastChild);
    }

    var dvLstSubCategory = document.getElementById("dv_lstSubCategory");
    while (dvLstSubCategory.hasChildNodes()) {
        dvLstSubCategory.removeChild(dvLstSubCategory.lastChild);
    }
    var lstProducts = document.getElementById("hfProduct").value;
    lstProducts = eval(lstProducts);

    for (var i = 0, len = lstProducts.length; i < len; ++i) {
        //------------------------------------------------------------------
        if (lstProducts[i].I_D_ID == 0) {
            if (lstProducts[i].CAT_ID == catId && lstProducts[i].MODIFIER == type) {
                var dealId = lstProducts[i].I_D_ID;
                //Create an input type dynamically.   
                var element = document.createElement("input");
                //Assign different attributes to the element. 
                element.setAttribute("type", "button");
                element.setAttribute("value", lstProducts[i].SKU_NAME);
                element.setAttribute("name", lstProducts[i].SKU_NAME);
                element.setAttribute("id", lstProducts[i].SKU_ID);
                element.setAttribute("class", "box-sm3");
                element.onclick = function () { // Note this is a function
                    LoadProductStatus(this.id);
                    $("#hfSkuId").val(this.id);//FOR MODIFIER ITEMS

                    addProductToOrderTable(this.id, catId, dealId);
                    changeProductClass(this);
                };
                dvLstProducts.appendChild(element);
            }
        }
            //------------------------------------------------------------------
        else if (lstProducts[i].I_D_ID != 0 && lstProducts[i].DEAL == 1) {
            if (lstProducts[i].I_D_ID == catId && lstProducts[i].MODIFIER == type) {
                var dealId = lstProducts[i].I_D_ID;
                var CatQty = lstProducts[i].Cat_Quantity;

                var element = document.createElement("input");
                element.setAttribute("type", "button");
                element.setAttribute("value", lstProducts[i].SKU_NAME);
                element.setAttribute("name", lstProducts[i].SKU_NAME);
                element.setAttribute("id", lstProducts[i].SKU_ID);
                element.setAttribute("class", "box-sm2");
                element.onclick = function () { // Note this is a function
                    //------------------------------------------------------------------
                    $('#hfDealId').val(dealId);
                    $("#hfcatId").val(this.id);
                    $('#txtDealQty').val(1);

                    document.getElementById('dvDealQty').style.display = 'block';
                    document.getElementById('dvDealUpdate').style.display = 'none';
                    $('#tble-ordered-products').find('tr').each(function () {
                        if (dealId == $(this).find("td:eq(21)").text()) {
                            document.getElementById('dvDealUpdate').style.display = 'block';
                            $('#txtDealQty').val(parseFloat($(this).find("td:eq(27)").text()));
                            return;
                        }
                    });

                    createSubProductButtons(this.id, dealId, $("#hfCategoryType").val());
                    changeSubCatClass(this)
                };

                dvLstSubCategory.appendChild(element);
            }
        }

    }
}

function createSubProductButtons(catId, dealId, type) {

    var dvLstProducts = document.getElementById("dv_lstProducts");
    while (dvLstProducts.hasChildNodes()) {
        dvLstProducts.removeChild(dvLstProducts.lastChild);
    }
    var lstProducts = document.getElementById("hfProduct").value;
    lstProducts = eval(lstProducts);

    for (var i = 0, len = lstProducts.length; i < len; ++i) {
        if (lstProducts[i].I_D_ID != 0 && lstProducts[i].DEAL == 0) {
            if (lstProducts[i].CAT_ID == catId && lstProducts[i].I_D_ID == dealId && lstProducts[i].MODIFIER == type) {
                var CatQty = lstProducts[i].Cat_Quantity;
                if (lstProducts[i].Is_ItemChoice == "0") {
                    if (ValidateDealQty()) {//First Validate
                        if (CheckItemDealQty(dealId, catId, lstProducts[i].SKU_ID, CatQty, lstProducts[i].QTY, 1)) {
                            addDealProductToOrderTable(lstProducts[i].SKU_ID, catId, dealId, $('#txtDealQty').val());
                        }
                        else {
                            break;
                        }
                    }
                }
                else {
                    //Create an input type dynamically.   
                    var element = document.createElement("input");
                    //Assign different attributes to the element. 
                    element.setAttribute("type", "button");
                    element.setAttribute("value", lstProducts[i].SKU_NAME);
                    element.setAttribute("name", lstProducts[i].QTY);
                    element.setAttribute("id", lstProducts[i].SKU_ID);
                    element.setAttribute("class", "box-sm3");
                    element.onclick = function () { // Note this is a function
                        LoadProductStatus(this.id);
                        $("#hfSkuId").val(this.id);
                        if (ValidateDealQty()) {//First Validate

                            if (CheckItemDealQty(dealId, catId, this.id, CatQty, parseFloat(this.name), 2)) {
                                $("#hfSkuId").val(this.id);
                                addDealProductToOrderTable(this.id, catId, dealId, $('#txtDealQty').val());
                                changeProductClass(this);
                            }
                            $('#tble-ordered-products').find('tr').each(function () {//Show Update button if Deal Exist
                                if (checkVoid($(this).find("td:eq(13)").text())) {//CHECK IS VOID OR NOT
                                    if (dealId == $(this).find("td:eq(21)").text()) {
                                        document.getElementById('dvDealUpdate').style.display = 'block';
                                        return;
                                    }
                                }
                            });
                        }
                    };
                    dvLstProducts.appendChild(element);
                }
            }
        }
    }
}
//-----------------------------------------------------------\\

//Employee Discount Validation 
function ValidateDiscount(Id) {
    if (Id === undefined || Id === null || Id === -1) {
        return true;
    }
    if (Id == "1") {
        if ($("#ddlDiscountUser").val() == null) {
            Error("Please select Employee");
            return false;
        }
        if ($("#ddlDiscountUser2").val() == null) {
            Error("Please select Authority Person");
            return false;
        }
        if (parseFloat($('#lblLimit').text()) < parseFloat($('#txtDiscount').val())) {
            Error("Discount Limit is:" + $('#lblLimit').text());
            return false;
        }
    }
    else if (Id == "2")
    {
        if ($('#hfCardTypeId').val() == "3") {
            if (parseFloat(document.getElementById('txtDiscount').value) > parseFloat(document.getElementById('txtDiscountBalance').value)) {
                Error('Discount can not be greater than Balance Discount');
                $('#txtDiscount').focus();
                return false;
            }
        }
    }
    return true;
}

function ValidateDiscount2(Id) {
    
    if (Id === undefined || Id === null)
    {   
        return true;
    }
    if (Id == "1") {
        if ($("#ddlDiscountUser2").val() == null) {
            Error("Please select Employee");
            return false;
        }

        if ($("#ddlDiscountUser3").val() == null) {
            Error("Please select Authority Person");
            return false;
        }

        if (parseFloat($('#lblLimit2').text()) < parseFloat($('#txtDiscount2').val())) {
            Error("Discount Limit is:" + $('#lblLimit2').text());
            return false;
        }
    }
    else if (Id == "2") {
        if ($('#hfCardTypeId').val() == "3") {
            if (parseFloat(document.getElementById('txtDiscount2').value) > parseFloat(document.getElementById('txtDiscountBalance2').value)) {
                Error('Discount can not be greater than Balance Discount');
                $('#txtDiscount2').focus();
                return false;
            }
        }
    }
    return true;
}

//Deal Validation----------------------------------------------------------\\
function ValidateDealQty() {
    if ($('#txtDealQty').val() == "") {
        Error("Must enter Deal Qty");
        return false;
    }
    if ($('#txtDealQty').val() == "0") {
        Error("Deal Qty should be greater than zero");
        return false;
    }
    return true;
}

//used on createCategoriesButtons
function CheckCatDealQty(dealId) {
    //------------------------------------------------------------------

    if (dealId == 0) {
        return true;
    }

    //------------------------------------------------------------------
    var lstProducts = document.getElementById("hfProduct").value;
    lstProducts = eval(lstProducts);

    var flag = false;

    var CatQty = 0;
    var TotalCatQty = 0;
    var TotalCatGridQty = 0;

    for (var i = 0, len = lstProducts.length; i < len; ++i) {
        if (lstProducts[i].DEAL == 1) {
            if (lstProducts[i].I_D_ID == dealId) {
                var count = 0;
                var CatGridQty = 0;
                var ItemGridQty = 0;

                TotalCatQty += lstProducts[i].Cat_Quantity * parseFloat($("#txtDealQty").val());

                $('#tble-ordered-products').find('tr').each(function () {
                    if (checkVoid($(this).find("td:eq(13)").text())) {//CHECK IS VOID OR NOT
                        if (dealId == $(this).find("td:eq(21)").text() && lstProducts[i].DIV_ID == $(this).find("td:eq(14)").text()) {
                            ItemGridQty += parseFloat($(this).find("td:eq(3) input").val());
                            if (count == 0) {
                                count++;
                                CatGridQty = parseFloat($(this).find("td:eq(27)").text()) * parseFloat($(this).find("td:eq(28)").text());
                                TotalCatGridQty += parseFloat($(this).find("td:eq(27)").text()) * parseFloat($(this).find("td:eq(28)").text());

                            }
                        }
                    }
                });
                if (ItemGridQty != CatGridQty) {
                    flag = true;
                    break;
                }
            }
        }
    }

    if (flag) {
        Error("Plz complete your Deal");
        return false;
    }

    if (TotalCatGridQty > 0) {
        if (TotalCatQty != TotalCatGridQty) {
            Error("Plz complete your Deal");
            return false;
        }
    }
    $("#hfDealId").val('0');
    return true;

}

//used in createSubProductButtons()
function CheckItemDealQty(dealId, catId, ItemId, CatQty, ItemQty, Type) {

    if ($("#tble-ordered-products tr").length > 0) {
        var flag = false;
        var flagmsg = "";
        var dealQty = 0;

        dealQty = $('#txtDealQty').val();

        $('#tble-ordered-products').find('tr').each(function () {
            if (checkVoid($(this).find("td:eq(13)").text())) {//CHECK IS VOID OR NOT
                if (dealId == $(this).find("td:eq(21)").text() && catId == $(this).find("td:eq(14)").text()) {
                    if ($(this).find("td:eq(31)").text() == "0") {
                        if (ItemId != $(this).find("td:eq(0)").text()) {
                            flag = true;
                            return;
                        }
                        else {
                            ItemQty += parseFloat($(this).find("td:eq(3) input").val());
                        }
                    }
                    else {
                        ItemQty += parseFloat($(this).find("td:eq(3) input").val());
                    }
                }
            }
        });

        dealQty = dealQty * CatQty;

        $('#tble-ordered-products').find('tr').each(function () {
            if (checkVoid($(this).find("td:eq(13)").text())) {//CHECK IS VOID OR NOT
                if (dealId == $(this).find("td:eq(21)").text() && catId == $(this).find("td:eq(14)").text()) {
                    dealQty = parseFloat($(this).find("td:eq(27)").text()) * parseFloat($(this).find("td:eq(28)").text());
                    return;
                }
            }
        });
        //--First Check Optional or Not
        if (flag) {

            Error("Cannot enter Optional Item");
            return false;
        }
        //--For Check Whole Deal
        if (ItemQty > dealQty) {
            ItemQty -= ItemQty;
            Error("Cannot enter Deal Qty is not equal with Item Qty");
            return false;
        }

        return true;
    }
    return true;
}

//Type used for checking click on Update button 
//1 for Updates
function CheckDealQty(dealId, catId, CatQty, Type) {

    var qty = 0;
    var dealQty = 0;

    $('#tble-ordered-products').find('tr').each(function () {
        if (checkVoid($(this).find("td:eq(13)").text())) {//CHECK IS VOID OR NOT
            if (dealId == $(this).find("td:eq(21)").text()) {
                qty += parseFloat($(this).find("td:eq(3) input").val());
            }
        }
    });
    //this type use on Update Deal Button Click
    if (Type == 1) {
        if (qty > $('#txtDealQty').val()) {
            Error("Deal Qty is not equal with Item Qty");
            return false;
        }
        return true;
    }


}

function UpdateDeal() {

    var dealId = $('#hfDealId').val();

    // if (CheckDealQty(dealId,0,0,1)) {

    var tableData;

    $('#tble-ordered-products').find('tr').each(function () {

        if (dealId == $(this).find("td:eq(21)").text()) {
            if (checkVoid($(this).find("td:eq(13)").text())) {//CHECK IS VOID OR NOT
                $(this).find("td:eq(27)").text($('#txtDealQty').val());

            }
        }
    });

    tableData = storeTblValues();
    tableData = JSON.stringify(tableData);
    document.getElementById('hfOrderedproducts').value = tableData;
    //}
}
//End Deal Validation----------------------------------------------------------\\

//Loyalty region

function LoadLoyaltyCardDetail() {
    if ($('#txtLoyaltyCard').val() != "") {
        $.ajax
           (
               {
                   type: "POST", //HTTP method
                   url: "frmCallCenterOld.aspx/LoadLoyaltyCardDetail", //page/method name
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   data: JSON.stringify({ cardNo: $('#txtLoyaltyCard').val() }),
                   success: LoyaltyCardDetail
               }
     );
    }
}

function LoadLoyaltyCardDetail2() {
    if ($('#txtLoyaltyCard2').val() != "") {
        $.ajax
           (
               {
                   type: "POST", //HTTP method
                   url: "frmCallCenterOld.aspx/LoadLoyaltyCardDetail", //page/method name
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   data: JSON.stringify({ cardNo: $('#txtLoyaltyCard2').val() }),
                   success: LoyaltyCardDetail2
               }
     );
    }
}

function LoyaltyCardDetail2(cardNo) {

    cardNo = JSON.stringify(cardNo);
    var result = jQuery.parseJSON(cardNo.replace(/&quot;/g, '"'));

    cardNo = eval(result.d);
    $('#hfCardNo').val('');
    $('#hfCardPurchasing').val('0');
    $('#hfCardTypeId').val('0');
    $('#hfCardPoints').val('0');
    $('#hfCardPurchasing').val('0');
    $('#hfCardAmountLimit').val('0');
    $('#txtLoyaltyCustomer2').val('');

    document.getElementById('txtDiscount2').value = '';
    CalculateBalance2();
    document.getElementById('rowPrivilege2').style.display = "none";
    document.getElementById('rowDirectorCard2').style.display = "none";
    if (cardNo.length > 0) {
        $('#hfCardTypeId').val(cardNo[0].CARD_TYPE_ID);

        if (cardNo[0].CARD_TYPE_ID == "1") {//Privilege Card for Customer
            document.getElementById('rowPrivilege2').style.display = "block";
            $('#txtLoyaltyCustomer2').val(cardNo[0].USER_NAME);
            $('#hfCustomerId').val(cardNo[0].USER_ID);
            DiscType(document.getElementById("percentage2"));
            $('#txtDiscount2').val(cardNo[0].DISCOUNT);
            $('#txtNoOfVisits2').val(cardNo[0].NoOfVisits);
            $('#txtTotalPurchased2').val(cardNo[0].TotalPurchase);
            $('#txtTotalLoyaltyDiscount2').val(cardNo[0].TotalDiscount);
            $('#txtLoyaltyQuantity2').val(cardNo[0].TotalQty);
            CalculateBalance2();
        }
        else if (cardNo[0].CARD_TYPE_ID == "2") {
            document.getElementById('rowRewardCard2').style.display = "block";
            $('#hfCardPoints').val(cardNo[0].POINTS);
            $('#hfCardPurchasing').val(cardNo[0].PURCHASING);
            $('#txtTotalPoints2').val(cardNo[0].TotalPurchase);
            $('#txtRedeemedPoints2').val(cardNo[0].POINTS);
            $('#hfCustomerId').val(cardNo[0].USER_ID);
            $('#txtLoyaltyCustomer2').val(cardNo[0].USER_NAME);
            $('#txtBalancePoints2').val(parseFloat(cardNo[0].TotalPurchase) - parseFloat(cardNo[0].POINTS));
            $('#txtAvailableDiscount2').val(cardNo[0].DISCOUNT);
            $('#txtAvailableCash2').val(cardNo[0].TotalQty);
            DiscType(document.getElementById("value"));
            if (parseFloat(cardNo[0].DISCOUNT) <= parseFloat($('#lblPaymentDue').text())) {
                $('#txtDiscount2').val(parseFloat(cardNo[0].DISCOUNT));
            }
            else {
                $('#txtDiscount2').val(parseFloat($('#lblPaymentDue').text()));
            }
            CalculateBalance2();
        }
        if (cardNo[0].CARD_TYPE_ID == "3") {//Director Card for employee
            document.getElementById('rowDirectorCard2').style.display = "block";
            $('#txtLoyaltyCustomer2').val(cardNo[0].USER_NAME);
            $('#hfCustomerId').val(cardNo[0].USER_ID);
            DiscType(document.getElementById("value2"));
            $('#txtAllowedLimit2').val(cardNo[0].AMOUNT_LIMIT);
            $('#txtDiscountAvail2').val(cardNo[0].TotalPurchase);
            $('#txtDiscountBalance2').val(cardNo[0].TotalDiscount);
            if (parseFloat(cardNo[0].TotalDiscount) < parseFloat($('#lblPaymentDue2').text())) {
                $('#txtDiscount2').val(parseFloat(cardNo[0].TotalDiscount));
            }
            else {
                $('#txtDiscount2').val(parseFloat($('#lblPaymentDue2').text()));
            }
            CalculateBalance2();
        }
        document.getElementById("txtDiscount2").disabled = true;
        document.getElementById('txtDiscountReason2').disabled = true;
        if (cardNo[0].CARD_TYPE_ID == "3" || cardNo[0].CARD_TYPE_ID == "2") {
            document.getElementById("txtDiscount2").disabled = false;
            document.getElementById('txtDiscountReason2').disabled = false;
        }
    }
    else {
        
    }
}

function LoyaltyCardDetail(cardNo) {

    cardNo = JSON.stringify(cardNo);
    var result = jQuery.parseJSON(cardNo.replace(/&quot;/g, '"'));

    cardNo = eval(result.d);
    $('#hfCardNo').val('');
    $('#hfCardPurchasing').val('0');
    $('#hfCardTypeId').val('0');
    $('#hfCardPoints').val('0');
    $('#hfCardPurchasing').val('0');
    $('#hfCardAmountLimit').val('0');
    $('#txtLoyaltyCustomer').val('');

    document.getElementById('txtDiscount').value = '';
    CalculateBalance();
    document.getElementById('rowPrivilege').style.display = "none";
    document.getElementById('rowDirectorCard').style.display = "none";
    document.getElementById('rowRewardCard').style.display = "none";
    if (cardNo.length > 0) {
        $('#hfCardTypeId').val(cardNo[0].CARD_TYPE_ID);
        $('#hfCardNo').val(cardNo[0].strCardNo);
        $('#hfCardPurchasing').val(cardNo[0].PURCHASING);
        if (cardNo[0].CARD_TYPE_ID == "1") {//Privilege Card for Customer
            document.getElementById('rowPrivilege').style.display = "block";
            $('#txtLoyaltyCustomer').val(cardNo[0].USER_NAME);
            $('#hfCustomerId').val(cardNo[0].USER_ID);
            DiscType(document.getElementById("percentage"));
            $('#txtDiscount').val(cardNo[0].DISCOUNT);
            CalculateBalance();
        }
        else if (cardNo[0].CARD_TYPE_ID == "2") {//Reward Card for Customer
            var CurrentPoints = 0;
            document.getElementById('rowRewardCard').style.display = "block";
            $('#hfCardPurchasing').val(cardNo[0].PURCHASING);
            $('#txtRedeemedPoints').val(cardNo[0].POINTS);
            $('#hfCustomerId').val(cardNo[0].USER_ID);
            $('#txtLoyaltyCustomer').val(cardNo[0].USER_NAME);
            $('#txtBalancePoints').val(parseFloat(cardNo[0].TotalPurchase) - parseFloat(cardNo[0].POINTS));
            $('#txtAvailableDiscount').val(cardNo[0].DISCOUNT);
            DiscType(document.getElementById("value"));
            CurrentPoints = Math.floor(parseInt($('#lblPaymentDue').text()) / parseInt(cardNo[0].PURCHASING)) * parseInt(cardNo[0].POINTS);
            $('#hfCardPoints').val(CurrentPoints);
            $('#txtOpeningPoints').val(cardNo[0].TotalPurchase);
            $('#txtTotalPoints').val(parseInt(CurrentPoints) + parseFloat(cardNo[0].TotalPurchase));
            CalculateBalance();
        }
        if (cardNo[0].CARD_TYPE_ID == "3") {//Director Card for employee
            document.getElementById('rowDirectorCard').style.display = "block";
            $('#txtLoyaltyCustomer').val(cardNo[0].USER_NAME);
            $('#hfCustomerId').val(cardNo[0].USER_ID);
            DiscType(document.getElementById("value"));
            $('#txtAllowedLimit').val(cardNo[0].AMOUNT_LIMIT);
            $('#txtDiscountAvail').val(cardNo[0].TotalDiscount);
            $('#txtDiscountBalance').val(parseFloat(cardNo[0].AMOUNT_LIMIT) - parseFloat(cardNo[0].TotalDiscount));
            if (parseFloat(cardNo[0].AMOUNT_LIMIT) - parseFloat(cardNo[0].TotalDiscount) < parseFloat($('#lblPaymentDue').text())) {
                $('#txtDiscount').val(parseFloat(cardNo[0].AMOUNT_LIMIT) - parseFloat(cardNo[0].TotalDiscount));
            }
            else {
                $('#txtDiscount').val(parseFloat($('#lblPaymentDue').text()));
            }
            CalculateBalance();
        }

        document.getElementById("txtDiscount").disabled = true;
        if (cardNo[0].CARD_TYPE_ID == "3" || cardNo[0].CARD_TYPE_ID == "2")
        {
            document.getElementById("txtDiscount").disabled = false;
        }
    }
    else {
        
    }
}

function DisableDiscount() {

    if ($('#txtLoyaltyCard').val() == "") {
        $('#dvDiscount2').find('*').prop('disabled', false);
        $('#txtDiscount2').val('');
        $('#hfCardTypeId').val('0');
        $('#hfCardPoints').val('0');
        $('#hfCardPurchasing').val('0');
        $('#hfCardAmountLimit').val('0');
        CalculateBalance2();
    }
}

function ValidateCardAmount() {
    if ($('#hfCardTypeId ').val() == "3") {
        if (parseFloat($('#txtDiscount2').val()) > parseFloat($('#hfCardAmountLimit').val())) {
            Error("Discount Limit is " + $('#hfCardAmountLimit').val());
            return false;
        }
    }
    return true;
}
//Loyalty endregion

///Qty [3] ,C1 [12], IS Void [13],Deal Price [21],A_Price[20], Deal ID [21], C2 [23], Deal Qty [27] ,Cat Qty [28], ,Cat Choice [29], Deal_name [32]
///UOM_ID": (33),"intStockMUnitCode": 34, "Stock_to_SaleOperator": 35, "Stock_to_SaleFactor": 36

//Products Grid---------------------------------------------------------------\\

function addProductToOrderTable(skuId, catId, dealId) {
    $("#hfModifierItemParent").val(skuId);
    var tableData;
    var lstProducts = document.getElementById("hfProduct").value;
    lstProducts = eval(lstProducts);

    for (var i = 0, len = lstProducts.length; i < len; ++i) {
        var flage = false;

        if (lstProducts[i].SKU_ID == skuId && lstProducts[i].CAT_ID == catId && lstProducts[i].I_D_ID == dealId) {
            $('#tble-ordered-products').find('tr').each(function () {

                var td1 = $(this).find("td:eq(0)").text();
                var tdcat = $(this).find("td:eq(14)").text();
                var tdDeal = -1;
                if ($(this).find("td:eq(21)").text() != "") {
                    tdDeal = $(this).find("td:eq(21)").text();//finding dealID
                }
                if (skuId == td1 && catId == tdcat && tdDeal == dealId) {

                    var color = $(this).find("td:eq(1)").css("color");//finding color for modifier 
                    var next = $(this).next().find("td:eq(0)").text();//finding for modifier next row 
                    
                    if (checkVoid($(this).find("td:eq(13)").text())) {//CHECK IS VOID OR NOT
                        if ($("#hfRow").val() == "") {//check row is selected or not if select not then update previous else add new
                            if (color == "rgb(255, 0, 0)") {
                                if (next != "") {
                                    flage = false;
                                }
                                else {
                                    if ($(this).find("td:eq(21)").text() == "0") {//when item_deal_id=0
                                        $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + 1);
                                        $(this).find("td:eq(6)").text(parseFloat($(this).find("td:eq(5)").text()) * parseInt($(this).find("td:eq(3) input").val()));
                                    }
                                    else {
                                        if (lstProducts[i].Is_CatChoice == "0") {
                                            $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + lstProducts[i].QTY);
                                        }
                                        else {
                                            $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + 1);
                                        }
                                    }

                                    tableData = storeTblValues();
                                    tableData = JSON.stringify(tableData);
                                    document.getElementById('hfOrderedproducts').value = tableData;
                                    flage = true;
                                }
                            }
                            else {//check for Modifier if row exist after then add new else update previous

                                if ($(this).find("td:eq(19)").text() == "2") {//WHEN DIVISION IS VALUE BASE

                                    $(this).find("td:eq(5) input").val(parseFloat($(this).find("td:eq(5) input").val()) + parseFloat($(this).find("td:eq(20)").text()));
                                    $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(5) input").val()) / parseFloat($(this).find("td:eq(20)").text()));
                                }
                                else {

                                    if (dealId == "0") {//when item_deal_id=0
                                        $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + 1);
                                        $(this).find("td:eq(6)").text(parseFloat($(this).find("td:eq(5)").text()) * parseInt($(this).find("td:eq(3) input").val()));
                                    }
                                    else {
                                        if (lstProducts[i].Is_CatChoice == "1") {
                                            $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + lstProducts[i].QTY);
                                        }
                                        else {
                                            $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + 1);
                                        }
                                    }
                                }
                                tableData = storeTblValues();
                                tableData = JSON.stringify(tableData);
                                document.getElementById('hfOrderedproducts').value = tableData;
                                flage = true;
                            }
                        }
                    }
                }
            });

            if (flage) break;
            addProductToOrderedProduct(lstProducts, i, $("#OrderNo1").text());
            break;
        }
    }
    tableData = storeTblValues();
    tableData = JSON.stringify(tableData);
    document.getElementById('hfOrderedproducts').value = tableData;

    setTotals();
}
function addProductToOrderedProduct(lstProducts, i, invoiceId) {

    //--------------------For Maintaing Counter-----By Hassan------------------------------------------------\\
    //#region Counter
    var rowNo = $("#hfRow").val();//get which row is selected

    if ($("#OrderNo1").text() == "N/A") {//Check when it's new order
        if ($("#hfRowIndex").val() != "") {//Check row selection
            var x = document.getElementById("tble-ordered-products").rows[rowNo].cells;
            x[23].innerHTML = parseFloat(parseFloat(x[23].innerHTML) + 0.1).toFixed(2);//Update C2

            var y = document.getElementById("tble-ordered-products").rows[rowNo].cells;
            lstProducts[i].C1 = parseFloat((parseFloat(y[12].innerHTML) + parseFloat(x[23].innerHTML))).toFixed(2);
            lstProducts[i].C2 = -1;//FOR CHECK COUNT ON EDIT

        }
        else {

            $("#hfCounter").val(parseInt($("#hfCounter").val()) + 1);
            lstProducts[i].C1 = parseFloat($("#hfCounter").val()) + parseFloat(lstProducts[i].C1);
        }
    }
    else {
        if ($("#hfRowIndex").val() != "") {//Check row selection
            var x = document.getElementById("tble-ordered-products").rows[rowNo].cells;
            x[23].innerHTML = parseFloat(parseFloat(x[23].innerHTML) + 0.1).toFixed(2);//Update C2

            var y = document.getElementById("tble-ordered-products").rows[rowNo].cells;
            lstProducts[i].C1 = parseFloat((parseFloat(y[12].innerHTML) + parseFloat(x[23].innerHTML))).toFixed(2);
            lstProducts[i].C2 = -1;//FOR CHECK COUNT ON EDIT
        }
        else {
            if (lstProducts[i].C2 > 0 || lstProducts[i].C2 == 0) {
                $("#hfCounter").val(parseInt($("#hfCounter").val()) + 1);
                lstProducts[i].C1 = parseFloat($("#hfCounter").val());
            }
        }
    }
    //#endregion Counter
    //-----------------------------------------------------------------------------------------------\\

    var row = "";
    if (lstProducts[i].DIV_ID == "2") {//WHEN DIVISION IS VALUE BASE FOR ALIALFAZAL
        row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;" disabled ></td><td align="center"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td><input type="text" size="4" onkeyup="plusAmount(this);" onkeypress="return onlyDotsAndNumbers(this, event);" value="' + lstProducts[i].T_PRICE + '"></td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
    }
    else {
        if (lstProducts[i].I_D_ID == "0") {//WHEN DEALS IS OFF 

            if (lstProducts[i].MODIFIER == "0") {//CHECK IS MODIFIER OR NOT IF NOT THEN

                row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;" disabled ></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + lstProducts[i].T_PRICE + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
            }
            else {
                if (lstProducts[i].T_PRICE == "0") {
                    row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2" style="color: red" colspan="6">' + lstProducts[i].SKU_NAME + '</td><td style="display:none;" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td style="display:none;"><input type="text" size="2" value="' + lstProducts[i].DEFAULT_QTY + '"  style="text-align: center;" disabled ></td><td style="display:none;" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td style="display:none;" class="table-text2">' + lstProducts[i].T_PRICE + '</td><td style="display:none;" class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
                }
                else {
                    row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td style="color: red" class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].DEFAULT_QTY + '"  style="text-align: center;" disabled ></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + lstProducts[i].T_PRICE + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');

                }
            }
        }
        else {
            if (lstProducts[i].MODIFIER == "1") {
                if (lstProducts[i].T_PRICE == "0") {
                    row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2" style="color: red">' + lstProducts[i].SKU_NAME + '</td><td style="display: none;" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + 1 + '"  style="text-align: center;" disabled ></td><td style="display: none;" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td style="display: none;" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
                }
                else {
                    row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td style="color: red" class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;" disabled ></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
                }
            }
            else {

                if (lstProducts[i].Is_ItemChoice == "1") {//WHEN DEALS IS ON AND IS_DEFAULT SET TO TRUE  
                    row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;"  disabled></td><td align="center" onclick="decreaseItem(this);" ><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
                }
                else {

                    row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;"  disabled></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
                }
            }
        }
    }

    if ($("#hfRow").val() != "") {//in case of product is selected in grid (AddRow(this));

        var i = $("#hfRow").val();
        $('#tble-ordered-products > tr').eq(i).after(row);

    }

    else {
        $("#tble-ordered-products").append(row);
    }

    if (lstProducts[i].INVOICE_ID == 'N/A' || parseFloat(lstProducts[i].INVOICE_ID) <= 0) {
        if (lstProducts[i].IS_HasMODIFIER == "1") {
            $('#btnModifierItem').trigger("click");
            $('#divModifier').show("slow");
            $('#divModifierParentName').html(lstProducts[i].SKU_NAME);
        }
    }
}
function addDealProductToOrderTable(skuId, catId, dealId, DealQty) {
    $("#hfModifierItemParent").val(skuId);
    var tableData;
    var lstProducts = document.getElementById("hfProduct").value;
    lstProducts = eval(lstProducts);

    for (var i = 0, len = lstProducts.length; i < len; ++i) {
        var flag = false;

        if (lstProducts[i].SKU_ID == skuId && lstProducts[i].CAT_ID == catId && lstProducts[i].I_D_ID == dealId) {

            $('#tble-ordered-products').find('tr').each(function () {

                var td1 = $(this).find("td:eq(0)").text();
                var tdcat = $(this).find("td:eq(14)").text();
                var tddealId = $(this).find("td:eq(21)").text();

                if (skuId == td1 && catId == tdcat && dealId == tddealId) {

                    var color = $(this).find("td:eq(1)").css("color");//finding color for modifier 
                    var next = $(this).next().find("td:eq(0)").text();//finding for modifier next row 

                    if (checkVoid($(this).find("td:eq(13)").text())) {//CHECK IS VOID OR NOT
                        if ($("#hfRow").val() == "") {//check row is selected or not if select not then update previous else add new
                            if (color == "rgb(255, 0, 0)") {
                                if (next != "") {
                                    flag = false;
                                }
                                else {

                                    if (lstProducts[i].Is_CatChoice == "0") {
                                        $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + lstProducts[i].QTY);
                                    }
                                    else {
                                        $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + lstProducts[i].QTY);
                                    }


                                    tableData = storeTblValues();
                                    tableData = JSON.stringify(tableData);
                                    document.getElementById('hfOrderedproducts').value = tableData;
                                    flag = true;
                                }
                            }
                            else {//check for Modifier if row exist after then add new else update previous


                                if (lstProducts[i].Is_CatChoice == "1") {
                                    $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + +lstProducts[i].QTY);
                                }
                                else {
                                    $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + +lstProducts[i].QTY);
                                }

                                tableData = storeTblValues();
                                tableData = JSON.stringify(tableData);
                                document.getElementById('hfOrderedproducts').value = tableData;
                                flag = true;
                            }
                        }
                    }
                }
            });


            if (flag) break;


            addDealProductToOrderedProduct(lstProducts, i, $("#OrderNo1").text(), DealQty);

            break;
        }
    }
    tableData = storeTblValues();
    tableData = JSON.stringify(tableData);
    document.getElementById('hfOrderedproducts').value = tableData;

    setTotals();
}
function addDealProductToOrderedProduct(lstProducts, i, invoiceId, DealQty) {

    //--------------------For Maintaing Counter-----By Hassan---------------------------------------\\
    //#region Counter
    var rowNo = $("#hfRow").val();//get which row is selected
    if ($("#OrderNo1").text() == "N/A") {//Check when it's new order
        if ($("#hfRowIndex").val() != "") {//Check row selection
            var x = document.getElementById("tble-ordered-products").rows[rowNo].cells;
            x[23].innerHTML = parseFloat(parseFloat(x[23].innerHTML) + 0.1).toFixed(2);//Update C2

            var y = document.getElementById("tble-ordered-products").rows[rowNo].cells;
            lstProducts[i].C1 = parseFloat((parseFloat(y[12].innerHTML) + parseFloat(x[23].innerHTML))).toFixed(2);
            lstProducts[i].C2 = -1;//FOR CHECK COUNT ON EDIT
        }
        else {

            $("#hfCounter").val(parseInt($("#hfCounter").val()) + 1);
            lstProducts[i].C1 = parseFloat($("#hfCounter").val()) + parseFloat(lstProducts[i].C1);
        }
    }
    else {
        if ($("#hfRowIndex").val() != "") {//Check row selection
            var x = document.getElementById("tble-ordered-products").rows[rowNo].cells;
            x[23].innerHTML = parseFloat(parseFloat(x[23].innerHTML) + 0.1).toFixed(2);//Update C2

            var y = document.getElementById("tble-ordered-products").rows[rowNo].cells;
            lstProducts[i].C1 = parseFloat((parseFloat(y[12].innerHTML) + parseFloat(x[23].innerHTML))).toFixed(2);
            lstProducts[i].C2 = -1;//FOR CHECK COUNT ON EDIT
        }
        else {
            if (lstProducts[i].C2 > 0 || lstProducts[i].C2 == 0) {
                $("#hfCounter").val(parseInt($("#hfCounter").val()) + 1);
                lstProducts[i].C1 = parseFloat($("#hfCounter").val());
            }
        }
    }
    //#endregion Counter
    //-----------------------------------------------------------------------------------------------\\

    var row = "";

    if (lstProducts[i].I_D_ID != "0") {//WHEN DEALS IS ON 
        if (lstProducts[i].MODIFIER == "1") {
            if (lstProducts[i].T_PRICE == "0") {
                row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2" style="color: red">' + lstProducts[i].SKU_NAME + '</td><td style="display: none;" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + 1 + '"  style="text-align: center;" disabled ></td><td style="display: none;" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td style="display: none;" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + DealQty + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
            }
            else {
                row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td style="color: red" class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;" disabled ></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + DealQty + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
            }
        }
        else {
            if (lstProducts[i].Is_ItemChoice == "1") {//WHEN DEALS IS ON AND IS_DEFAULT SET TO TRUE  
                row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center" ><span style="margin-right: 5px; cursor: pointer;" ><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;"  disabled></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;" ><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + DealQty + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
            }
            else {
                row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center"  ><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;" disabled ></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + DealQty + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstProducts[i].DEFAULT_QTY + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
            }
        }
    }
    if ($("#hfRow").val() != "") {//in case of product is selected in grid (AddRow(this));

        var i = $("#hfRow").val();
        $('#tble-ordered-products > tr').eq(i).after(row);

    }
    else {
        $("#tble-ordered-products").append(row);
    }

    if (lstProducts[i].INVOICE_ID == 'N/A' || parseFloat(lstProducts[i].INVOICE_ID) <= 0) {
        if (lstProducts[i].IS_HasMODIFIER == "1") {
            $('#btnModifierItem').trigger("click");
            $('#divModifier').show("slow");
            $('#divModifierParentName').html(lstProducts[i].SKU_NAME);
        }
    }
}
function addProductToOrderTableModifier(skuId, catId, dealId) {
    var tableData;
    var lstProducts = document.getElementById("hfProduct").value;
    lstProducts = eval(lstProducts);
    var ItemName = "";
    var ModifierQty = 0;
    var UOM = "";
    var lstModifer = document.getElementById("hfModifierItems").value;
    lstModifer = eval(lstModifer);

    for (var j = 0, len2 = lstModifer.length; j < len2; ++j) {
        for (var i = 0, len = lstProducts.length; i < len; ++i) {
            var flage = false;

            if (lstProducts[i].SKU_ID == skuId && lstProducts[i].CAT_ID == catId && lstProducts[i].I_D_ID == dealId && $("#hfModifierItemParent").val() == lstModifer[j].SKU_ID && skuId == lstModifer[j].ModifierSKU_ID) {
                ItemName = lstProducts[i].SKU_NAME;
                UOM = lstModifer[j].Stock_Unit;
                $('#tble-ordered-products').find('tr').each(function () {

                    var td1 = $(this).find("td:eq(0)").text();
                    var tdcat = $(this).find("td:eq(14)").text();
                    var tdDeal = -1;
                    if ($(this).find("td:eq(21)").text() != "") {
                        tdDeal = $(this).find("td:eq(21)").text();//finding dealID
                    }
                    if (skuId == td1 && catId == tdcat && tdDeal == dealId) {

                        var color = $(this).find("td:eq(1)").css("color");//finding color for modifier 
                        var next = $(this).next().find("td:eq(0)").text();//finding for modifier next row 

                        if (checkVoid($(this).find("td:eq(13)").text())) {//CHECK IS VOID OR NOT
                            if ($("#hfRow").val() == "") {//check row is selected or not if select not then update previous else add new
                                if (color == "rgb(255, 0, 0)") {
                                    if ($(this).find("td:eq(21)").text() == "0") {//when item_deal_id=0
                                        $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + parseFloat(lstModifer[j].Default_Qty));
                                        $(this).find("td:eq(6)").text(parseFloat($(this).find("td:eq(5)").text()) * parseInt($(this).find("td:eq(3) input").val()));
                                    }
                                    else {
                                        if (lstProducts[i].Is_CatChoice == "0") {
                                            $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + lstProducts[i].QTY);
                                        }
                                        else {
                                            $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + parseFloat(lstModifer[j].Default_Qty));
                                        }
                                    }

                                    tableData = storeTblValues();
                                    tableData = JSON.stringify(tableData);
                                    document.getElementById('hfOrderedproducts').value = tableData;
                                    flage = true;
                                }
                                else {//check for Modifier if row exist after then add new else update previous

                                    if ($(this).find("td:eq(19)").text() == "2") {//WHEN DIVISION IS VALUE BASE

                                        $(this).find("td:eq(5) input").val(parseFloat($(this).find("td:eq(5) input").val()) + parseFloat($(this).find("td:eq(20)").text()));
                                        $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(5) input").val()) / parseFloat($(this).find("td:eq(20)").text()));
                                    }
                                    else {

                                        if (dealId == "0") {//when item_deal_id=0
                                            $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + parseFloat(lstModifer[j].Default_Qty));
                                            $(this).find("td:eq(6)").text(parseFloat($(this).find("td:eq(5)").text()) * parseInt($(this).find("td:eq(3) input").val()));
                                        }
                                        else {
                                            if (lstProducts[i].Is_CatChoice == "1") {
                                                $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + lstProducts[i].QTY);
                                            }
                                            else {
                                                $(this).find("td:eq(3) input").val(parseFloat($(this).find("td:eq(3) input").val()) + parseFloat(lstModifer[j].Default_Qty));
                                            }
                                        }
                                    }
                                    tableData = storeTblValues();
                                    tableData = JSON.stringify(tableData);
                                    document.getElementById('hfOrderedproducts').value = tableData;
                                    flage = true;
                                }
                            }
                        }
                        ModifierQty = parseFloat($(this).find("td:eq(3) input").val());
                        $("#modifierMessage").css("display", "block");
                        $("#modifierMessage").html(' ' + ItemName + ' ' + ModifierQty + ' ' + UOM + ' added successfully.');
                        $("#modifierMessage").delay(500).fadeOut("slow");
                    }
                });

                if (flage) break;
                addProductToOrderedProductModifier(lstProducts, i, $("#OrderNo1").text());
                break;
            }
        }
    }
    tableData = storeTblValues();
    tableData = JSON.stringify(tableData);
    document.getElementById('hfOrderedproducts').value = tableData;
    setTotals();
}
function addProductToOrderedProductModifier(lstProducts, i, invoiceId) {
    var lstModifer = document.getElementById("hfModifierItems").value;
    lstModifer = eval(lstModifer);
    var ItemName = "";
    var UOM = "";
    var ModifierQty = 0;
    for (var j = 0, len = lstModifer.length; j < len; ++j) {
        if (lstModifer[j].ModifierSKU_ID == lstProducts[i].SKU_ID && $("#hfModifierItemParent").val() == lstModifer[j].SKU_ID) {

            ItemName = lstProducts[i].SKU_NAME;
            UOM = lstModifer[j].Stock_Unit;
            ModifierQty = lstModifer[j].Default_Qty;

            //--------------------For Maintaing Counter-----By Hassan------------------------------------------------\\
            //#region Counter
            var rowNo = $("#hfRow").val();//get which row is selected

            if ($("#OrderNo1").text() == "N/A") {//Check when it's new order
                if ($("#hfRowIndex").val() != "") {//Check row selection
                    var x = document.getElementById("tble-ordered-products").rows[rowNo].cells;
                    x[23].innerHTML = parseFloat(parseFloat(x[23].innerHTML) + 0.1).toFixed(2);//Update C2

                    var y = document.getElementById("tble-ordered-products").rows[rowNo].cells;
                    lstProducts[i].C1 = parseFloat((parseFloat(y[12].innerHTML) + parseFloat(x[23].innerHTML))).toFixed(2);
                    lstProducts[i].C2 = -1;//FOR CHECK COUNT ON EDIT

                }
                else {

                    $("#hfCounter").val(parseInt($("#hfCounter").val()) + 1);
                    lstProducts[i].C1 = parseFloat($("#hfCounter").val()) + parseFloat(lstProducts[i].C1);
                }
            }
            else {
                if ($("#hfRowIndex").val() != "") {//Check row selection
                    var x = document.getElementById("tble-ordered-products").rows[rowNo].cells;
                    x[23].innerHTML = parseFloat(parseFloat(x[23].innerHTML) + 0.1).toFixed(2);//Update C2

                    var y = document.getElementById("tble-ordered-products").rows[rowNo].cells;
                    lstProducts[i].C1 = parseFloat((parseFloat(y[12].innerHTML) + parseFloat(x[23].innerHTML))).toFixed(2);
                    lstProducts[i].C2 = -1;//FOR CHECK COUNT ON EDIT
                }
                else {
                    if (lstProducts[i].C2 > 0 || lstProducts[i].C2 == 0) {
                        $("#hfCounter").val(parseInt($("#hfCounter").val()) + 1);
                        lstProducts[i].C1 = parseFloat($("#hfCounter").val());
                    }
                }
            }
            //#endregion Counter
            //-----------------------------------------------------------------------------------------------\\

            var row = "";
            if (lstProducts[i].DIV_ID == "2") {//WHEN DIVISION IS VALUE BASE FOR ALIALFAZAL
                row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;" disabled ></td><td align="center"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td><input type="text" size="4" onkeyup="plusAmount(this);" onkeypress="return onlyDotsAndNumbers(this, event);" value="' + lstProducts[i].T_PRICE + '"></td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
            }
            else {
                if (lstProducts[i].I_D_ID == "0") {//WHEN DEALS IS OFF 

                    if (lstProducts[i].MODIFIER == "0") {//CHECK IS MODIFIER OR NOT IF NOT THEN

                        row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;" disabled ></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + lstProducts[i].T_PRICE + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
                    }
                    else {
                        if (lstProducts[i].T_PRICE == "0") {
                            row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2" style="color: red" colspan="6">' + lstProducts[i].SKU_NAME + '</td><td style="display:none;" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td style="display:none;"><input type="text" size="2" value="' + lstModifer[j].Default_Qty + '"  style="text-align: center;" disabled ></td><td style="display:none;" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td style="display:none;" class="table-text2">' + lstProducts[i].T_PRICE + '</td><td style="display:none;" class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
                        }
                        else {
                            row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td style="color: red" class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstModifer[j].Default_Qty + '"  style="text-align: center;" disabled ></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + lstProducts[i].T_PRICE + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');

                        }
                    }
                }
                else {
                    if (lstProducts[i].MODIFIER == "1") {
                        if (lstProducts[i].T_PRICE == "0") {
                            row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2" style="color: red">' + lstProducts[i].SKU_NAME + '</td><td style="display: none;" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstModifer[j].Default_Qty + '"  style="text-align: center;" disabled ></td><td style="display: none;" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td style="display: none;" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
                        }
                        else {
                            row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td style="color: red" class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstModifer[j].Default_Qty + '"  style="text-align: center;" disabled ></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
                        }
                    }
                    else {

                        if (lstProducts[i].Is_ItemChoice == "1") {//WHEN DEALS IS ON AND IS_DEFAULT SET TO TRUE  
                            row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstModifer[j].Default_Qty + '"  style="text-align: center;"  disabled></td><td align="center" onclick="decreaseItem(this);" ><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
                        }
                        else {

                            row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2">' + lstProducts[i].SKU_NAME + '</td><td align="center" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;"  disabled></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + lstProducts[i].AMOUNT + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + lstProducts[i].I_D_ID + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + lstProducts[i].Cat_Quantity + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td></tr>');
                        }
                    }
                }
            }

            if ($("#hfRow").val() != "") {//in case of product is selected in grid (AddRow(this));

                var i = $("#hfRow").val();
                $('#tble-ordered-products > tr').eq(i).after(row);

            }

            else {
                $("#tble-ordered-products").append(row);
            }

            if (lstProducts[i].INVOICE_ID == 'N/A' || parseFloat(lstProducts[i].INVOICE_ID) <= 0) {
                if (lstProducts[i].IS_HasMODIFIER == "1") {
                    $('#btnModifierItem').trigger("click")
                    $('#divModifier').show("slow");
                }
            }

            $("#modifierMessage").css("display", "block");
            $("#modifierMessage").html(' ' + ItemName + ' ' + ModifierQty + ' ' + UOM + ' added successfully.');
            $("#modifierMessage").delay(500).fadeOut("slow");

            break;
        }
    }
}
function addProductToOrderTableModifierPackage(skuId, catId, dealId) {
    var tableData;
    var lstProducts = document.getElementById("hfProduct").value;
    lstProducts = eval(lstProducts);
    var ItemName = "";
    var ModifierQty = 0;
    var UOM = "";
    var lstModifer = document.getElementById("hfModifiers").value;
    lstModifer = eval(lstModifer);

    for (var j = 0, len2 = lstModifer.length; j < len2; ++j) {
        for (var i = 0, len = lstProducts.length; i < len; ++i) {
            var flage = false;
            if (lstProducts[i].SKU_ID == parseInt(skuId) && lstProducts[i].CAT_ID == catId && dealId == $("#hfDealId").val() && $("#hfModifierItemParent").val() == lstModifer[j].SKU_ID && parseInt(skuId) == lstModifer[j].ModifierSKU_ID) {
                ItemName = lstProducts[i].SKU_NAME;
                UOM = lstModifer[j].Stock_Unit;
                if (flage) break;
                addProductToOrderedProductModifierPackage(lstProducts, i, $("#OrderNo1").text(), $('#txtDealQty').val());
                break;
            }
        }
    }
    tableData = storeTblValues();
    tableData = JSON.stringify(tableData);
    document.getElementById('hfOrderedproducts').value = tableData;
    setTotals();
}
function addProductToOrderedProductModifierPackage(lstProducts, i, invoiceId, dealQty) {
    var lstModifer = document.getElementById("hfModifierItems").value;
    lstModifer = eval(lstModifer);
    var ItemName = "";
    var UOM = "";
    var ModifierQty = 0;
    var gstamount = 0;
    gstamount = parseFloat(lstProducts[i].GSTPER) * parseFloat(lstProducts[i].AMOUNT) / 100;
    var discount = 0;
    discount = CalculatePromotion(lstProducts[i].SKU_ID, 1, lstProducts[i].T_PRICE);
    var amount = parseFloat(lstProducts[i].AMOUNT) - parseFloat(discount);
    for (var j = 0, len = lstModifer.length; j < len; ++j) {
        if (lstModifer[j].ModifierSKU_ID == lstProducts[i].SKU_ID && $("#hfModifierItemParent").val() == lstModifer[j].SKU_ID) {
            ItemName = lstProducts[i].SKU_NAME;
            UOM = lstModifer[j].Stock_Unit;
            ModifierQty = lstModifer[j].Default_Qty;

            //--------------------For Maintaing Counter-----By Hassan------------------------------------------------\\
            //#region Counter
            var rowNo = $("#hfRow").val();//get which row is selected

            if ($("#OrderNo1").text() == "N/A") {//Check when it's new order
                if ($("#hfRowIndex").val() != "") {//Check row selection
                    var x = document.getElementById("tble-ordered-products").rows[rowNo].cells;
                    x[23].innerHTML = parseFloat(parseFloat(x[23].innerHTML) + 0.1).toFixed(2);//Update C2

                    var y = document.getElementById("tble-ordered-products").rows[rowNo].cells;
                    lstProducts[i].C1 = parseFloat((parseFloat(y[12].innerHTML) + parseFloat(x[23].innerHTML))).toFixed(2);
                    lstProducts[i].C2 = -1;//FOR CHECK COUNT ON EDIT

                }
                else {

                    $("#hfCounter").val(parseInt($("#hfCounter").val()) + 1);
                    lstProducts[i].C1 = parseFloat($("#hfCounter").val()) + parseFloat(lstProducts[i].C1);
                }
            }
            else {
                if ($("#hfRowIndex").val() != "") {//Check row selection
                    var x = document.getElementById("tble-ordered-products").rows[rowNo].cells;
                    x[23].innerHTML = parseFloat(parseFloat(x[23].innerHTML) + 0.1).toFixed(2);//Update C2

                    var y = document.getElementById("tble-ordered-products").rows[rowNo].cells;
                    lstProducts[i].C1 = parseFloat((parseFloat(y[12].innerHTML) + parseFloat(x[23].innerHTML))).toFixed(2);
                    lstProducts[i].C2 = -1;//FOR CHECK COUNT ON EDIT
                }
                else {
                    if (lstProducts[i].C2 > 0 || lstProducts[i].C2 == 0) {
                        $("#hfCounter").val(parseInt($("#hfCounter").val()) + 1);
                        lstProducts[i].C1 = parseFloat($("#hfCounter").val());
                    }
                }
            }
            //#endregion Counter
            //-----------------------------------------------------------------------------------------------\\

            var row = "";
            if (lstProducts[i].DIV_ID == "2") {//WHEN DIVISION IS VALUE BASE FOR ALIALFAZAL
                row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2" style="color: red" ondblclick="AddRowFree(this);">' + lstProducts[i].SKU_NAME + '</td><td align="center"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstProducts[i].QTY + '"  style="text-align: center;" disabled ></td><td align="center"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td><input type="text" size="4" onkeyup="plusAmount(this);" onkeypress="return onlyDotsAndNumbers(this, event);" value="' + lstProducts[i].T_PRICE + '"></td><td class="table-text2">' + amount + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + $("#hfModifierItemParentDealID").val() + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + dealQty + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td><td style="display:none;">0</td><td style="display:none;">0</td><td style="display:none;">0</td><td style="display:none;">' + lstProducts[i].ORIGINAL_QTY + '</td><td style="display:none;">' + lstProducts[i].PRINT_QTY + '</td><td style="display:none;">' + lstProducts[i].MODIFIER + '</td><td style="display:none;">' + $("#hfModifierItemParent").val() + '</td><td style="display:none;">' + 0 + '</td><td style="display:none;">' + lstProducts[i].SortOrder + '</td><td style="display:none;">' + $('#hfModifierParetn_Row_ID ').val() + '</td><td style="display:none;">' + lstProducts[i].GSTPER + '</td><td style="display:none;">' + gstamount + '</td><td style="display:none;">' + discount + '</td><td align="center" onclick="AddComment(this);"><a href="#"><span class="fa fa-pencil"></span></a></td></tr>');
            }
            else {
                if (lstProducts[i].I_D_ID == "0") {//WHEN DEALS IS OFF 
                    if (lstProducts[i].T_PRICE == "0" || lstProducts[i].IsPackage) {
                        row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2" style="color: red" colspan="6" ondblclick="AddRowFree(this);">' + lstProducts[i].SKU_NAME + '</td><td style="display:none;" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td style="display:none;"><input  type="text" size="2" value="' + lstModifer[j].Default_Qty + '"  style="text-align: center;" disabled ></td><td style="display:none;" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td style="display:none;" class="table-text2">' + lstProducts[i].T_PRICE + '</td><td style="display:none;" class="table-text2">' + amount + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + $("#hfModifierItemParentDealID").val() + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + dealQty + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td><td style="display:none;">0</td><td style="display:none;">0</td><td style="display:none;">0</td><td style="display:none;">' + lstProducts[i].ORIGINAL_QTY + '</td><td style="display:none;">' + lstProducts[i].PRINT_QTY + '</td><td style="display:none;">' + lstProducts[i].MODIFIER + '</td><td style="display:none;">' + $("#hfModifierItemParent").val() + '</td><td style="display:none;">' + 0 + '</td><td style="display:none;">' + lstProducts[i].SortOrder + '</td><td style="display:none;">' + $('#hfModifierParetn_Row_ID ').val() + '</td><td style="display:none;">' + lstProducts[i].GSTPER + '</td><td style="display:none;">' + gstamount + '</td><td style="display:none;">' + discount + '</td><td align="center" onclick="AddComment(this);"><a href="#"><span class="fa fa-pencil"></span></a></td></tr>');
                    }
                    else {
                        row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td style="color: red" class="table-text2" ondblclick="AddRowFree(this);">' + lstProducts[i].SKU_NAME + '</td><td align="center" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstModifer[j].Default_Qty + '"  style="text-align: center;" disabled ></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + lstProducts[i].T_PRICE + '</td><td class="table-text2">' + amount + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + $("#hfModifierItemParentDealID").val() + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + dealQty + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td><td style="display:none;">0</td><td style="display:none;">0</td><td style="display:none;">0</td><td style="display:none;">' + lstProducts[i].ORIGINAL_QTY + '</td><td style="display:none;">' + lstProducts[i].PRINT_QTY + '</td><td style="display:none;">' + lstProducts[i].MODIFIER + '</td><td style="display:none;">' + $("#hfModifierItemParent").val() + '</td><td style="display:none;">' + 0 + '</td><td style="display:none;">' + lstProducts[i].SortOrder + '</td><td style="display:none;">' + $('#hfModifierParetn_Row_ID ').val() + '</td><td style="display:none;">' + lstProducts[i].GSTPER + '</td><td style="display:none;">' + gstamount + '</td><td style="display:none;">' + discount + '</td><td align="center" onclick="AddComment(this);"><a href="#"><span class="fa fa-pencil"></span></a></td></tr>');
                    }
                }
                else {
                    if (lstProducts[i].T_PRICE == "0" || lstProducts[i].IsPackage) {
                        row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td class="table-text2" style="color: red" colspan="6" ondblclick="AddRowFree(this);">' + lstProducts[i].SKU_NAME + '</td><td style="display: none;" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td style="display: none;" align="center"><input type="text" size="2" value="' + lstModifer[j].Default_Qty + '"  style="text-align: center;" disabled ></td><td style="display: none;" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td style="display: none;" class="table-text2">' + 0 + '</td><td style="display: none;" class="table-text2">' + amount + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + $("#hfModifierItemParentDealID").val() + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + dealQty + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td><td style="display:none;">0</td><td style="display:none;">0</td><td style="display:none;">0</td><td style="display:none;">' + lstProducts[i].ORIGINAL_QTY + '</td><td style="display:none;">' + lstProducts[i].PRINT_QTY + '</td><td style="display:none;">' + lstProducts[i].MODIFIER + '</td><td style="display:none;">' + $("#hfModifierItemParent").val() + '</td><td style="display:none;">' + 0 + '</td><td style="display:none;">' + lstProducts[i].SortOrder + '</td><td style="display:none;">' + $('#hfModifierParetn_Row_ID ').val() + '</td><td style="display:none;">' + lstProducts[i].GSTPER + '</td><td style="display:none;">' + gstamount + '</td><td style="display:none;">' + discount + '</td><td align="center" onclick="AddComment(this);"><a href="#"><span class="fa fa-pencil"></span></a></td></tr>');
                    }
                    else {
                        row = $('<tr><td style="display:none;">' + lstProducts[i].SKU_ID + '</td><td style="color: red" class="table-text2" ondblclick="AddRowFree(this);">' + lstProducts[i].SKU_NAME + '</td><td align="center" onclick="plusQty(this);"><span style="margin-right: 5px; cursor: pointer;"><img src="../images/plus.png"></span></td> <td align="center"><input type="text" size="2" value="' + lstModifer[j].Default_Qty + '"  style="text-align: center;" disabled ></td><td align="center" onclick="decreaseItem(this);"><span style="margin-left: 5px; cursor: pointer;"><img src="../images/minus.png"></span></td><td class="table-text2">' + 0 + '</td><td class="table-text2">' + amount + '</td><td align="center" onclick="deleteItem(this);"><a href="#"><span class="fa fa-times"></span></a></td><td style="display:none;">' + lstProducts[i].PRINTER + '</td><td style="display:none;">' + lstProducts[i].PR_COUNT + '</td><td style="display:none;">' + lstProducts[i].SEC_ID + '</td><td style="display:none;">' + lstProducts[i].SECTION + '</td><td style="display:none;">' + lstProducts[i].C1 + '</td><td style="display:none;">' + lstProducts[i].VOID + '</td><td style="display:none;">' + lstProducts[i].CAT_ID + '</td><td style="display:none;">' + invoiceId + '</td><td style="display:none;">' + lstProducts[i].IS_DESC + '</td><td style="display:none;">' + lstProducts[i].DESC + '</td><td style="display:none;">' + lstProducts[i].PRINT + '</td><td style="display:none;">' + lstProducts[i].DIV_ID + '</td><td style="display:none;">' + lstProducts[i].A_PRICE + '</td><td style="display:none;">' + $("#hfModifierItemParentDealID").val() + '</td><td align="center" onclick="AddRow(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td><td style="display:none;">' + lstProducts[i].C2 + '</td><td style="display:none;">' + lstProducts[i].H + '</td><td style="display:none;">' + lstProducts[i].intDealID + '</td><td style="display:none;">' + lstProducts[i].lngDealDetailID + '</td><td style="display:none;">' + lstProducts[i].DEAL_QTY + '</td><td style="display:none;">' + dealQty + '</td><td style="display:none;">' + lstProducts[i].Is_CatChoice + '</td><td style="display:none;">' + lstProducts[i].Is_ItemChoice + '</td><td style="display:none;">' + lstProducts[i].Is_Optional + '</td><td style="display:none;">' + lstProducts[i].DEAL_NAME + '</td><td style="display:none;">' + lstProducts[i].UOM_ID + '</td><td style="display:none;">' + lstProducts[i].intStockMUnitCode + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleOperator + '</td><td style="display:none;">' + lstProducts[i].Stock_to_SaleFactor + '</td><td style="display:none;">' + lstModifer[j].Default_Qty + '</td><td style="display:none;">' + lstProducts[i].IS_HasMODIFIER + '</td><td style="display:none;">0</td><td style="display:none;">0</td><td style="display:none;">0</td><td style="display:none;">' + lstProducts[i].ORIGINAL_QTY + '</td><td style="display:none;">' + lstProducts[i].PRINT_QTY + '</td><td style="display:none;">' + lstProducts[i].MODIFIER + '</td><td style="display:none;">' + $("#hfModifierItemParent").val() + '</td><td style="display:none;">' + 0 + '</td><td style="display:none;">' + lstProducts[i].SortOrder + '</td><td style="display:none;">' + $('#hfModifierParetn_Row_ID ').val() + '</td><td style="display:none;">' + lstProducts[i].GSTPER + '</td><td style="display:none;">' + gstamount + '</td><td style="display:none;">' + discount + '</td><td align="center" onclick="AddComment(this);"><a href="#"><span class="fa fa-pencil"></span></a></td></tr>');
                    }
                }
            }

            if ($("#hfRow").val() != "") {//in case of product is selected in grid (AddRow(this));
                var i = $("#hfRow").val();
                $('#tble-ordered-products > tr').eq(i).after(row);
                $('#hfIsNewItemAdded').val('1');
            }
            else {
                $("#tble-ordered-products").append(row);
                $('#hfIsNewItemAdded').val('1');
            }

            if (lstProducts[i].INVOICE_ID == 'N/A' || parseFloat(lstProducts[i].INVOICE_ID) <= 0) {
                if (lstProducts[i].IS_HasMODIFIER == "1") {
                    $('#btnModifierItem').trigger("click")
                    $('#divModifier').show("slow");
                }
            }

            var obj = {};
            obj["ItemID"] = lstProducts[i].SKU_ID;
            obj["ParentID"] = $("#hfModifierItemParent").val();
            obj["ItemName"] = ItemName;
            obj["Price"] = lstProducts[i].T_PRICE;
            obj["Qty"] = lstProducts[i].QTY;
            obj["ModifierParetn_Row_ID"] = $('#hfModifierParetn_Row_ID ').val();
            Modifierparent.push(obj);

            $("#modifierMessage").css("display", "block");
            $("#modifierMessage").html(' ' + ItemName + ' ' + ModifierQty + ' ' + UOM + ' added successfully.');
            $("#modifierMessage").delay(500).fadeOut("slow");

            break;
        }
    }
}
//WHEN DIVISION IS VALUE BASE
function plusAmount(obj) {

    rowIndex = obj.closest('tr');

    var price = parseFloat($(rowIndex).find('td:eq(5) input').val());

    if (obj.value != "") {

        var tprice = parseFloat($(rowIndex).find('td:eq(20)').text());

        $(rowIndex).find('td:eq(3) input').val(parseFloat(price / tprice).toFixed(2));

        var amount = parseFloat(price).toFixed(2);

        $(rowIndex).find('td:eq(6)').text(amount);



    }
    else {
        $(rowIndex).find('td:eq(3) input').val(0);
        $(rowIndex).find('td:eq(6)').text(0);
    }

    var tableData = storeTblValues();
    tableData = JSON.stringify(tableData);
    document.getElementById('hfOrderedproducts').value = tableData;

    $('#hfDisable').val("0");

}

function plusQty(obj) {

    var rowIndex = $(obj).parent();
    if (checkVoid($(rowIndex).find('td:eq(13)').text())) {

        var qty = parseFloat($(rowIndex).find('td:eq(3) input').val());
        qty = qty + 1;

        $(rowIndex).find('td:eq(3) input').val(qty);
        var price = $(rowIndex).find('td:eq(5)').text();
        var amount = parseFloat(qty * price).toFixed(2);
        $(rowIndex).find('td:eq(6)').text(amount);

        var tableData = storeTblValues();
        tableData = JSON.stringify(tableData);
        document.getElementById('hfOrderedproducts').value = tableData;

        setTotals();

        $('#hfDisable').val("0");
    }
}

function resetColor() {
    $('#tble-ordered-products tr').removeClass("selected");
}

//Add new Row on Specific index for Nos and Extras
function AddRow(obj) {

    var rowNo = $(obj).closest('tr').index();//for getting on which row no click

    if ($("#hfRow").val() != rowNo) {
        resetColor();

    }

    $("#hfRow").val(rowNo);

    var rowIndex = $(obj).parent();
    $("#hfRowIndex").val(rowIndex);

    //$("#hfCounter").val($(rowIndex).find('td:eq(23)').text());

    $(obj).closest('tr').addClass('selected');


    //now call on addProductToOrderedProduct

}
//-----------------------------------------------------------------------------\\

function storeTblValues() {

    var tableData = new Array();

    $('#tble-ordered-products tr').each(function (row, tr) {
        if ($(tr).find('td:eq(19)').text() == "2") {

            tableData[row] = {
                "SKU_ID": $(tr).find('td:eq(0)').text(),
                "SKU_NAME": $(tr).find('td:eq(1)').text(),
                //2 for +
                "QTY": $(tr).find('td:eq(3) input').val(),
                //4 for -
                "T_PRICE": $(tr).find('td:eq(5) input').val(),
                "AMOUNT": $(tr).find('td:eq(6)').text(),
                //7 column is of delete button
                "PRINTER": $(tr).find('td:eq(8)').text(),
                "PR_COUNT": $(tr).find('td:eq(9)').text(),
                "SEC_ID": $(tr).find('td:eq(10)').text(),
                "SECTION": $(tr).find('td:eq(11)').text(),
                "C1": $(tr).find('td:eq(12)').text(),
                "VOID": $(tr).find('td:eq(13)').text(),
                "CAT_ID": $(tr).find('td:eq(14)').text(),
                "INVOICE_ID": $(tr).find('td:eq(15)').text(),
                "DESC": $(tr).find('td:eq(17)').text(),
                "PRINT": $(tr).find('td:eq(18)').text(),
                "A_PRICE": $(tr).find('td:eq(20)').text(),
                "I_D_ID": $(tr).find('td:eq(21)').text(),
                //22 column for Selection
                "C2": $(tr).find('td:eq(23)').text(),
                "intDealID": $(tr).find('td:eq(25)').text(),
                "lngDealDetailID": $(tr).find('td:eq(26)').text(),
                "DEAL_QTY": $(tr).find('td:eq(27)').text(),
                "DEAL_NAME": $(tr).find('td:eq(32)').text(),
                "UOM_ID": $(tr).find('td:eq(33)').text(),
                "intStockMUnitCode": $(tr).find('td:eq(34)').text(),
                "Stock_to_SaleOperator": $(tr).find('td:eq(35)').text(),
                "Stock_to_SaleFactor": $(tr).find('td:eq(36)').text(),
                "DEFAULT_QTY": $(tr).find('td:eq(37)').text(),
                "IS_HasMODIFIER": $(tr).find('td:eq(38)').text(),
                "IS_FREE": 0,
                "ORIGINAL_QTY": $(tr).find('td:eq(3) input').val()
            }
        }
        else {

            tableData[row] = {
                "SKU_ID": $(tr).find('td:eq(0)').text(),
                "SKU_NAME": $(tr).find('td:eq(1)').text(),
                //2 for +
                "QTY": $(tr).find('td:eq(3) input').val(),
                //4 for-
                "T_PRICE": $(tr).find('td:eq(5)').text(),
                "AMOUNT": $(tr).find('td:eq(6)').text(),
                //7 column is of delete button
                "PRINTER": $(tr).find('td:eq(8)').text(),
                "PR_COUNT": $(tr).find('td:eq(9)').text(),
                "SEC_ID": $(tr).find('td:eq(10)').text(),
                "SECTION": $(tr).find('td:eq(11)').text(),
                "C1": $(tr).find('td:eq(12)').text(),
                "VOID": $(tr).find('td:eq(13)').text(),
                "CAT_ID": $(tr).find('td:eq(14)').text(),
                "INVOICE_ID": $(tr).find('td:eq(15)').text(),
                "DESC": $(tr).find('td:eq(17)').text(),
                "PRINT": $(tr).find('td:eq(18)').text(),
                "A_PRICE": $(tr).find('td:eq(20)').text(),
                "I_D_ID": $(tr).find('td:eq(21)').text(),
                //22 column for Selection
                "C2": $(tr).find('td:eq(23)').text(),
                "intDealID": $(tr).find('td:eq(25)').text(),
                "lngDealDetailID": $(tr).find('td:eq(26)').text(),
                "DEAL_QTY": $(tr).find('td:eq(27)').text(),
                "DEAL_NAME": $(tr).find('td:eq(32)').text(),
                "UOM_ID": $(tr).find('td:eq(33)').text(),
                "intStockMUnitCode": $(tr).find('td:eq(34)').text(),
                "Stock_to_SaleOperator": $(tr).find('td:eq(35)').text(),
                "Stock_to_SaleFactor": $(tr).find('td:eq(36)').text(),
                "DEFAULT_QTY": $(tr).find('td:eq(37)').text(),
                "IS_HasMODIFIER": $(tr).find('td:eq(38)').text(),
                "IS_FREE": 0,
                "ORIGINAL_QTY": $(tr).find('td:eq(3) input').val()
            }
        }
    });

    return tableData;
}

//Call on Pending Bills Row Click, And on HoldOrder Save When Takeaway
function ShowBill(tr) {
    if ($(tr).find("td:eq(24)").text() == "1") {
        Error('This record is locked');
        return;
    }
    if ($(tr).find("td:eq(26)").text() == "3") {//Rider is on the way
        Error('This record is locked by rider');
        return;
    }
    $("#hfVoidBy").val('');
    $("#txtLoyaltyCard").val('');
    $("#txtLoyaltyCard2").val('');
    $("#hfCardTypeId").val(0);
    $("#hfCardPoints").val(0);
    $("#hfCardPurchasing").val(0);
    $('#hfCardAmountLimit').val(0);
    $("#InvoiceTable").text('');
    $('#hfDiscountType').val('');
    $('#hfchkDiscountType').val('');
    $('#ddlDiscountType').val(0);
    $('#ddlDiscountType2').val(0);
    $('#txtLoyaltyCustomer2').val('');
    $('#txtLoyaltyCustomer').val('');
    $('#txtAllowedLimit').val('');
    $('#txtDiscountAvail').val('');
    $('#txtDiscountBalance').val('');
    $('#txtAllowedLimit2').val('');
    $('#txtDiscountAvail2').val('');
    $('#txtDiscountBalance2').val('');

    if (document.getElementById('hfCheckSMS').value == "0") {    
        $("#OrderNo1").text($(tr).find("td:eq(0)").text());
        $("#MaxOrderNo").text($(tr).find("td:eq(13)").text());
        $("#hfTableId").val($(tr).find("td:eq(14)").text());
        $("#hfTableNo").val($(tr).find("td:eq(1)").text());
        $("#txtTakeawayCustomer").val($(tr).find("td:eq(1)").text());
        var amountDue = $(tr).find("td:eq(2)").text();
        var discount = $(tr).find("td:eq(3)").text();
        $("#TableNo1").text($(tr).find("td:eq(10)").text());
        $("#hfCustomerId").val($(tr).find("td:eq(11)").text());
        $("#hfCustomerNo").val($(tr).find("td:eq(15)").text());
        //region Loyalty
        $("#txtLoyaltyCard").val($(tr).find("td:eq(16)").text());
        $("#txtLoyaltyCard2").val($(tr).find("td:eq(16)").text());
        $("#hfCardTypeId").val($(tr).find("td:eq(17)").text());
        $("#hfCardPoints").val($(tr).find("td:eq(18)").text());
        $("#hfCardPurchasing").val($(tr).find("td:eq(19)").text());
        $('#hfCardAmountLimit').val($(tr).find("td:eq(20)").text());
        //endregion Loyalty
        $("#txtLoyaltyCard").blur();
        $("#txtLoyaltyCard2").blur();
        $("#InvoiceTable").text($(tr).find("td:eq(12)").text());
        $('#hfDiscountType').val($(tr).find("td:eq(4)").text());
        $('#hfchkDiscountType').val($(tr).find("td:eq(4)").text());
        try {
            $('#ddlDiscountType').val($(tr).find("td:eq(4)").text());
            $('#ddlDiscountType2').val($(tr).find("td:eq(4)").text());

        } catch (e) {
            $('#ddlDiscountType').val(-1);
            $('#ddlDiscountType2').val(-1);
        }
        
        $('#ddlDiscountType').change();
        $('#ddlDiscountType2').change();

        if ($(tr).find("td:eq(7)").text() != "null") {
            $('#txtDiscountReason2').val($(tr).find("td:eq(7)").text());
            $('#txtDiscountReason').val($(tr).find("td:eq(7)").text());            
        }
        else {
            $('#txtDiscountReason2').val('');
            $('#txtDiscountReason').val('');
        }
        if ($(tr).find("td:eq(8)").text() != "0") {

            $('#txtService').val($(tr).find("td:eq(8)").text());
            $('#txtService2').val($(tr).find("td:eq(8)").text());
        }
        else {

            $('#txtService').val('');
            $('#txtService2').val('');
        }

        activeLink(); //Active css of Main Buttons on selection of pending bills

        if (amountDue > 0) {

            GetPendingBill($(tr).find("td:eq(0)").text());
            calculateDiscount(discount, amountDue);
        }
        else {//For Delivery Type
            $("#lblTotalGrossAmount").text("0");
            $('#tble-ordered-products').empty();
            $('#hfOrderedproducts').val('');
        }

        $('#hfDisable').val("1");
    }
    document.getElementById('hfCheckSMS').value = "0";
}

function NewOrder() {
    $('#txtRemarks').val('');
    $("#txtManualOrderNo").val('');
    $('#tble-ordered-products').empty();
    $('#hfOrderedproducts').val('');
    GetPendingBills();
    $("#hfTableNo").val("");
    $("#hfTableId").val("0");
    $("#hfCustomerId").val("0");
    if (document.getElementById("hfCustomerType").value == "Dine In") {
        $("#TableNo1").text("N/A");
    }
    $("#OrderNo1").text("N/A");
    $("#lblTotalGrossAmount").text("");
    $("#txtTakeawayCustomer").val('');
    $("#hfCounter").val(0);
    document.getElementById('dvDealUpdate').style.display = 'none';
    $("#txtDealQty").val('1');
    UnlockRecord();
}
//#region Printing

//For Removing Duplicates an Array Used in PrintOrder(), UniqueSECTN()
function unique(list) {
    var result = [];
    $.each(list, function (i, e) {
        if ($.inArray(e, result) == -1)
            result.push(e);
    });
    return result;
}

var sort_by = function (field, reverse, primer) {

    var key = primer ?
        function (x) { return primer(x[field]) } :
        function (x) { return x[field] };

    reverse = !reverse ? 1 : -1;

    return function (a, b) {
        return a = key(a), b = key(b), reverse * ((a > b) - (b > a));
    }
}

function PrintOrder() {
    $("#OrderprintType").text('');
    $("#divLess").css("display", "none");
    $("#divCancel").css("display", "none");
    $("#divAdd").css("display", "none");
    $("#divDuplicate").css("display", "none");
    $("#tblCancelItem").css("display", "none");
    $("#tblLessItem").css("display", "none");
    if ($("#hfDisable").val() == "1") {
        var orderedProducts = $("#hfOrderedproducts").val();
        orderedProducts = eval(orderedProducts);
        orderedProducts = orderedProducts.sort(sort_by('C1', false, parseFloat));
        var uniqueSections = $.unique(orderedProducts.map(function (d) { return d.SEC_ID; }));
        uniqueSections = uniqueSections.sort();
        uniqueSections = unique(uniqueSections);
        for (var j = 0; j < uniqueSections.length; j++) {
            $('#detail-section-skus').empty(); // clear all skus  from invoice
            for (var i = 0, len = orderedProducts.length; i < len; i++) {
                if (orderedProducts[i].PRINT == "true") {
                    if (orderedProducts[i].SEC_ID == uniqueSections[j]) {
                        var qty = orderedProducts[i].QTY;
                        //if (qty > 0) {
                        $("#SectionName").text(orderedProducts[i].SECTION);
                        $("#lblOrderNotes2").text($("#txtRemarks").val());
                        $("#CustoerType").text(document.getElementById("hfCustomerType").value);
                        $("#TableNo").text("Table No : " + $("#hfTableNo").val());
                        $("#kitchenOrderTaker").text("O-T : " + '');
                        if ($("#hfCustomerType").val() == "Takeaway") {
                            $("#TableNo").text("Customer :" + $("#txtTakeawayCustomer").val());
                        }
                        else if ($("#hfCustomerType").val() == "Delivery") {
                            $("#TableNo").text("Customer :" + $("#hfTableNo").val());
                            $("#kitchenOrderTaker").text("D-M : " + '');
                        }
                        if ($("#txtManualOrderNo").val() != "") {
                            $("#PrintMaxOrderNo").text($("#MaxOrderNo").text() + "-" + $("#txtManualOrderNo").val());
                        }
                        else {
                            $("#PrintMaxOrderNo").text($("#MaxOrderNo").text());
                        }
                        $("#Date").text($("#hfCurrentWorkDate").val());
                        $("#Time").text(moment().format('hh:mm A'));
                        if ($("#txtManualOrderNo").val() != "") {
                            $("#PrintMaxOrderNo").text($("#MaxOrderNo").text() + "-" + $("#txtManualOrderNo").val());
                        }
                        else {
                            $("#PrintMaxOrderNo").text($("#MaxOrderNo").text());
                        }
                        var row = $('<tr ><td>' + orderedProducts[i].C1 + '</td><td>' + orderedProducts[i].DESC + '</td><td class="text-right">' + qty + '</td></tr>');
                        $('#detail-section-skus').append(row);
                    }
                }
            }
            if ($("#detail-section-skus tr").length > 0) {
                $("#divDuplicate").html('<hr>DUPLICATE ORDER<br><hr>');
                $("#divDuplicate").css("display", "block");
                PrintInvoiceOfSection();
            }
        }

        $('#tble-ordered-products').empty();
        $('#hfOrderedproducts').val('');
        $('#hfCustomerId').val('0');
        $("#hfTableNo").val("");
        $("#hfTableId").val("0");
        $('#hfDisable').val("0");
        if (document.getElementById("hfCustomerType").value == "Dine In") {
            $("#TableNo1").text("N/A");
        }
        $("#OrderNo1").text("N/A");
        $("#lblTotalGrossAmount").text("");
        $('#txtRemarks').val('');
        $('#txtManualOrderNo').val('');
        $("#hfCounter").val(0);
        document.getElementById('dvDealUpdate').style.display = 'none';
        $("#txtDealQty").val('1');
        UnlockRecord();
    }
}

//#endregion Printing Section 

//function checkTable()
//{
//    $('#tble-ordered-products tr').each(function (row, tr) {
//        if ($(tr).find('td:eq(19)').text() == "2") {
//            if ($(tr).find('td:eq(5) input').val() == "") {
//                  return -1;
//            }
//        }
//    });
//    return 0;
//}

// #region HoldOrder
function HoldOrder() {
    //When Dine IN Active
    if (($("#lnkDineIn").attr("class")) == "box active") {

        if (($("#hfTableNo").val() == "") || ($("#hfTableNo").val() == "DLY") || ($("#hfTableNo").val() == "TKY")) {

            Error("Table not selected");

            return;
        }
    }
    if (($("#lnkDelivery").attr("class")) == "box active") {
        if (($("#hfCustomerId").val() == "0")) {

            Error("Please select customer");

            return;
        }
    }
    SaveOrder();
}

$('#dvHold').click(function (e) {
    if (CheckCatDealQty($("#hfDealId").val())) {
        if (($("#lnkDelivery").attr("class")) == "box active") {
           
            if (($('#hfCustomerIDNew').val() == 0)) {
                Error("Please select customer");
                return;
            }

        }

        var table = document.getElementById('tble-ordered-products');
        if (table.rows.length == 0) {

            Error("Please enter Items");
            return;
        }

        var me = $(this);
        e.preventDefault();

        if (me.data('requestRunning')) {
            return;
        }

        me.data('requestRunning', true);
        $("#dvHold").attr("disabled", true);

        $.ajax
                ({
                    type: "POST", //HTTP method
                    url: "frmCallCenterOld.aspx/HoldOrder", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ orderedProducts: $('#hfOrderedproducts').val(), orderBooker: 0, coverTable: 0, customerType: "Delivery", CustomerName: $('#hfCustomerIDNew').val(), maxOrderNo: $("#MaxOrderNo").text(), printType: document.getElementById("hfBookingType").value, tableName: 0, takeAwayCustomer: $("#txtTakeawayCustomer").val(), bookerName: '', tabId: 0, CustomerNo: document.getElementById("hfCustomerNo").value, VoidBy: $('#hfVoidBy').val(), manualOrderNo: $("#txtManualOrderNo").val(), remarks: $("#txtRemarks").val(), LocationID: ddlLocation.GetValue() }),
                    success: OrderSaved,
                    error: OnError,
                    complete: function () {
                        $("#dvHold").attr("disabled", false);
                        me.data('requestRunning', false);

                    }
                });
    }
});

function PrintInvoiceOfSection() {
    $("#hfDisable").val("0");
    $.print("#invoice-kitchen");
}

function SaveOrder() {
    if (($("#lnkDelivery").attr("class")) == "box active") {

        if (($("#hfCustomerId").val() == "0")) {

            Error("Please select customer");

            return;
        }

    }
    if (CheckCatDealQty($("#hfDealId").val())) {
        $("#dvHold").attr("disabled", true);

        $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmCallCenterOld.aspx/HoldOrder", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    data: JSON.stringify({ orderedProducts: $('#hfOrderedproducts').val(), orderBooker: Id, coverTable: 0, customerType: document.getElementById("hfCustomerType").value, CustomerName: document.getElementById("hfCustomerId").value, maxOrderNo: $("#MaxOrderNo").text(), printType: document.getElementById("hfBookingType").value, tableName: 0, takeAwayCustomer: $("#txtTakeawayCustomer").val(), bookerName: '', tabId: 0, CustomerNo: document.getElementById("hfCustomerNo").value, VoidBy: $('#hfVoidBy').val(), manualOrderNo: $("#txtManualOrderNo").val(), remarks: $("#txtRemarks").val(), LocationID: ddlLocation.GetValue() }),
                    success: function () {
                        //this case use for Delivery bcz when table is empty order saved function (unique Section) was not working 
                        if ($("#tble-ordered-products tr").length > 0) {//When Table is not empty for Delivery  else load tables
                            OrderSaved();
                        }
                        else {
                            GetPendingBills();
                        }
                        if (document.getElementById("hfCustomerType").value == "Delivery") {
                            SMS(document.getElementById('hfCustomerNo').value, "1");
                        }
                    },
                    error: OnError,
                    complete: function () {
                        $("#dvHold").attr("disabled", false);
                    }

                }
            );
    }
}

function OrderSaved() {
    ClearOrder();    
}

function Error(Msg) {

    $.Zebra_Dialog(Msg, { 'title': 'Error', 'type': 'error' });
}

function Succes(Msg) {

    $.Zebra_Dialog(Msg, { 'title': 'Success', 'type': 'information' });
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
// #endregion HoldOrder

//----------------on Payment Type  button Click---------------------------------\\
function PayType(myPayType) {

    document.getElementById("cash").style["background-color"] = "#919399";
    document.getElementById("credit").style["background-color"] = "#919399";

    myPayType.style["background-color"] = "#7dab49";

    var currentValue = myPayType.value;
    $('#hfPaymentType').val(currentValue);

    CalculateBalance();

}

$('#btnVoid').click(function (e) {
    
    var me = $(this);
    e.preventDefault();

    if (me.data('requestRunning')) {
        return;
    }

    me.data('requestRunning', true);

    $.ajax
            ({
                type: "POST", //HTTP method
                url: "frmCallCenterOld.aspx/VoidOrder", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ orderId: $("#OrderNo1").text() }),
               // success: ClearOnCancel,
                error: OnError,
                complete: function () {
                    ClearOnCancel();
                    me.data('requestRunning', false);
                }
            });
   
});
//----------------on Discount Type button Click---------------------------------\\
function DiscType(myDiscType) {

    document.getElementById("percentage").style["background-color"] = "#919399";
    document.getElementById("value").style["background-color"] = "#919399";

    //---on print invoice popup
    document.getElementById("percentage2").style["background-color"] = "#919399";
    document.getElementById("value2").style["background-color"] = "#919399";
    //-----------------------------------------------------------------------\\

    myDiscType.style["background-color"] = "#7dab49";
    var currentValue = myDiscType.value;
     document.getElementById('txtDiscount').disabled = false;
     document.getElementById('txtDiscount2').disabled = false;
     document.getElementById('txtDiscountReason2').disabled = false;
    
    $('#hfDiscountType').val(currentValue);

    if (myDiscType.id == 'percentage2' || myDiscType.id == 'value2') {

        CalculateBalance2();
    }
    else {

        CalculateBalance();
    }

}

//-----------------Calcualte on discount and cash Recd on Payment Click PopUp----\\ 

function CalculateBalance() {

    var balance = 0;
    var amountDue = 0;
    var discount = 0;
    var gst = 0;

    discount = document.getElementById('txtDiscount').value;

    var discountType = $('#hfDiscountType').val(); //  document.getElementById("hfDiscountType").value;
    var cashRcd = 0;

    cashRcd = document.getElementById('txtCashRecieved').value;

    var grandTotal = 0;
    grandTotal = $("#GrandTotal").text();


    if ($('#hfServiceCharges').val() == "False") {
        gst = document.getElementById("hfSalesTax").value;
    }
    else {
        gst = document.getElementById("txtService").value;
    }

    $("#lblBalance").text('');
    $("#lblPaymentDue").text('');

    if ((discount == "")) {
        discount = 0;
    }
    if ((cashRcd == "")) {
        cashRcd = 0;
    }
    if ((gst == "")) {
        gst = 0;
    }

    if (discountType == 0) {

        discount = parseFloat(grandTotal) * parseFloat(discount / 100);
        if ($('#hfServiceCharges').val() == "False") {
            gst = parseFloat(gst / 100) * (parseFloat(grandTotal) - parseFloat(discount));
        }
        // balance = parseFloat(grandTotal) + parseFloat(gst) - parseFloat(discount);

        balance = Math.round((grandTotal - discount + parseFloat(gst)), 0);

        amountDue = Math.round(balance, 0);

        balance = cashRcd - balance;


    }
    else if (discountType == 1) {
        if ($('#hfServiceCharges').val() == "False") {
            gst = parseFloat(gst / 100) * (parseFloat(grandTotal) - parseFloat(discount));
        }
        balance = parseFloat(grandTotal) + parseFloat(gst) - parseFloat(discount);
        amountDue = Math.round(balance, 0);

        balance = Math.round((cashRcd - balance), 0);
    }
    else {
        if ($('#hfServiceCharges').val() == "False") {
            gst = parseFloat(gst / 100) * (parseFloat(grandTotal - discount));
        }
        balance = parseFloat(grandTotal) + parseFloat(gst);

        amountDue = parseFloat(balance) - parseFloat(discount);

        balance = Math.round((cashRcd - amountDue), 0);
    }

    if (cashRcd == 0) {
        balance = 0;
    }

    $("#lblDiscountTotal").text(parseFloat(discount).toFixed(0));
    $("#lblGSTTotal").text(parseFloat(gst).toFixed(0));
    $("#lblBalance").text(parseFloat(balance).toFixed(0));
    $("#lblPaymentDue").text(parseFloat(amountDue).toFixed(0));

}

function SaveInvoice(type) {

    var EmpDiscount = 0;
    if ($('select#ddlDiscountType option:selected').val() === undefined || $('select#ddlDiscountType option:selected').val() === null || $('select#ddlDiscountType option:selected').val() === 'null' || $('select#ddlDiscountType option:selected').val() === '') {
        //Do nothing.
    }
    else {
        EmpDiscount = $('select#ddlDiscountType option:selected').val();
    }


    if (ValidateDiscount(EmpDiscount)) {


        //Payment Validation=====================
        if ($('#hfPaymentType').val() == '') {
            Error("First Select Payment Type");
            return;
        }
        if ($('#hfPaymentType').val() == '0' && parseFloat($('#lblPaymentDue').text()) > 0) {
            if ($('#txtCashRecieved').val() == '') {
                Error("Please enter amount");
                return;
            }
            if ($('#txtCashRecieved').val() == '0') {
                Error("Amount should be greater than zero");
                return;
            }

            if (parseFloat($('#txtCashRecieved').val()) < parseFloat($('#lblPaymentDue').text())) {
                Error("Amount should be greater than due Payment");
                return;
            }
        }
        //=======================================

        var salesTax = document.getElementById("hfSalesTax").value;

        var ManagerId = 0;
        var EM_UserID = 0;

        if (EmpDiscount == "1") {
            EM_UserID = $('select#ddlDiscountUser option:selected').val();
            ManagerId = $('select#ddlDiscountUser2 option:selected').val();
        }
        
        $.ajax
        (
            {
                type: "POST", //HTTP method
                url: "frmCallCenterOld.aspx/InsertInvoice", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ orderedProducts: $('#hfOrderedproducts').val(), Type: $('#hfCustomerType ').val(), amountDue: $("#GrandTotal").text(), discount: $('#txtDiscount').val(), paidIn: $('#txtCashRecieved').val(), payType: $('#hfPaymentType').val(), Gst: $("#lblGSTTotal").text(), DiscType: $('#hfDiscountType').val(), approvedBy: '', gstPerAge: salesTax, Service: $('#txtService').val(), takeAwayCustomer: $("#txtTakeawayCustomer").val(), empDiscType: EmpDiscount, EMC_UserID: EM_UserID, Manager_UserID: ManagerId, PASSWORD: $('#hfManagerPassword').val(), customerID: $('#hfCustomerId').val(), cardNo: $('#hfCardNo').val(), purchasing: $('#hfCardPurchasing').val(), manualOrderNo: $("#txtManualOrderNo").val(), remarks: $("#txtRemarks").val(), CustomerNo: document.getElementById("hfCustomerNo").value }),
                success: InvoiceSaved,
                error: OnError
            }
        );
    }
}

function InvoiceSaved() {

    PrintPaymentInvoice();

    Clear();

    GetPendingBills();

    $("#hfTableNo").val("");
    $("#hfTableId").val("0");
    UnlockRecord();
}

function changeTableClass(btn) {
    var a, n;
    var a = document.getElementById("dv_lstTable").children;
    for (n = 0; n < a.length; n++) {
        a[n].style["background-color"] = "#d4def7";
    }
    btn.style["background-color"] = "#53b4b5";

}

function changeClass(btn) {
    var a, n;
    a = document.getElementById("dv_lstCategory").children;
    for (n = 0; n < a.length; n++) {
        a[n].style["background-color"] = "#d4def7";
    }
    btn.style["background-color"] = "#7DAB49";

    a = document.getElementById("dv_lstModifyCategory").children;
    for (n = 0; n < a.length; n++) {
        a[n].style["background-color"] = "#d4def7";
    }
    btn.style["background-color"] = "#7DAB49";


}
function changeClass2(btn) {
    var a, n;
    a = document.getElementById("dv_lstCategory").children;
    for (n = 0; n < a.length; n++) {
        if (a[n].name == '0') {
            a[n].style["background-color"] = "#d4def7";
        }
        else {
            a[n].style["background-color"] = "#e2e3e8";
        }
    }

    btn.style["background-color"] = "#7DAB49";

    a = document.getElementById("dv_lstModifyCategory").children;
    for (n = 0; n < a.length; n++) {
        a[n].style["background-color"] = "#d4def7";
    }
    btn.style["background-color"] = "#7DAB49";
}

function changeSubCatClass(btn) {
    var a, n;
    var a = document.getElementById("dv_lstSubCategory").children;
    for (n = 0; n < a.length; n++) {
        a[n].style["background-color"] = "#d4def7";
    }
    btn.style["background-color"] = "#7DAB49";

}

function changeProductClass(btn) {
    var a, n;
    a = document.getElementById("dv_lstProducts").children;
    for (n = 0; n < a.length; n++) {
        a[n].style["background-color"] = "#e2e3e8";
    }
    btn.style["background-color"] = "#7DAB49";

    a = document.getElementById("dv_lstModifyProducts").children;
    for (n = 0; n < a.length; n++) {
        a[n].style["background-color"] = "#e2e3e8";
    }
    btn.style["background-color"] = "#7DAB49";

}

// #region Item Deletion

function UserValidationInDataBase(obj) {

    $("#hfVoidBy").val('');
    var UserId = document.getElementById('txtUserID').value;
    var UserPassword = document.getElementById('txtUserPass').value;
    var UserClick = document.getElementById('hfUserClick').value;

    //UserPassword.setAttribute('type', 'text');

    $.ajax
        (
            {
                type: "POST", //HTTP method
                url: "frmCallCenterOld.aspx/ValidateUser", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ UserId: UserId, UserPass: UserPassword, UserClick: UserClick }),
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
            $("#hfVoidBy").val(UserAuth[0].USER_ID);
            if ($("#hfUserClick").val() == "Decrease") {
                if (UserAuth[0].IsLessRight == true) {
                    $('#UserValidation').hide("slow");
                    document.getElementById('txtUserID').value = "";
                    document.getElementById('txtUserPass').value = "";
                    $('#DecreaseConfirmation').show("slow");
                }
                else {
                    Error('This user Can not less Item!');
                }
            }
            else {
                if (UserAuth[0].IsDelRight == true) {
                    $('#UserValidation').hide("slow");
                    document.getElementById('txtUserID').value = "";
                    document.getElementById('txtUserPass').value = "";
                    $('#DeletionConfirmation').show("slow");
                }
                else {
                    Error('This user can not delete Item!');
                }
            }
        }
        else {
            if ($("#hfUserClick").val() == "Decrease") {
                Error('This user Can not less Item!');
            }
            else {
                Error('This user can not delete Item!');
            }
        }
    }
    else
    {
        if ($("#hfUserClick").val() == "Decrease") {
            Error('This user Can not less Item!');
        }
        else
        {
            Error('This user can not delete Item!');
        }
    }
}

function CancelUserValidation() {

    $('#UserValidation').hide("slow");
}

function deleteItem(btn) {

    var rowIndex = $(btn).parent();

    if (checkVoid($(rowIndex).find('td:eq(13)').text())) {

        var Sku_id = $(rowIndex).find('td:eq(0)').text();
        var SkuType = $(rowIndex).find('td:eq(24)').text();//Check Hold or Not for new '' for already Hold 'H'
        var Cat_id = $(rowIndex).find('td:eq(14)').text();

        $("#hfskuId").val(Sku_id);
        $("#hfcatId").val(Cat_id);
        $("#hfDelIndex").val($(rowIndex).find('td:eq(12)').text());


        if (SkuType == "") {
            deleteQty();
        }
        else {
            document.getElementById('txtUserID').value = "";
            document.getElementById('txtUserPass').value = "";


            $("#hfUserClick").val('Delete');
            $('#UserValidation').show("slow");

            $('#txtUserID').focus();
        }
    }
}

function deleteQty() {
    var Sku_Id = $("#hfskuId").val();
    var Cat_Id = $("#hfcatId").val();
    var C1 = $("#hfDelIndex").val();

    $('#tble-ordered-products tr').each(function (row, tr) {
        if ($(tr).find('td:eq(0)').text() == Sku_Id && $(tr).find('td:eq(14)').text() == Cat_Id && $(tr).find('td:eq(12)').text() == C1) {
            $(tr).find('td:eq(13)').text(true);
            $(tr).find('td:eq(7)').css('background-color', '#b20505');

        }
    });
    var tableData = storeTblValues();
    tableData = JSON.stringify(tableData);
    document.getElementById('hfOrderedproducts').value = tableData;

    setTotals();

    $("#hfskuId").val('');
    $("#hfcatId").val('');
    $("#hfDelIndex").val('')
}

function DeletionProceed() {
    $('#DeletionConfirmation').hide("slow");
    deleteQty();
    $('#hfDisable').val("0");
}

function DeletionCancel() {
    $('#DeletionConfirmation').hide("slow");
    $("#hfskuId").val('');
    $("#hfcatId").val('');
    $("#hfUserClick").val('');
    $("#hfDelIndex").val('');

}

// #endregion Item Deletion

// #region Decrease Qty

function decreaseItem(btn) {

    var rowIndex = $(btn).parent();
    var Sku_id = $(rowIndex).find('td:eq(0)').text();
    var Cat_id = $(rowIndex).find('td:eq(14)').text();
    var SkuType = $(rowIndex).find('td:eq(24)').text();//Check Hold or Not for new '' and for already Hold 'H'

    var qty = $(rowIndex).find('td:eq(3) input').val();


    if (qty > 1) {
        $("#hfskuId").val(Sku_id);
        $("#hfcatId").val(Cat_id);
        $("#hfDelId").val($(rowIndex).find('td:eq(21)').text());
        $("#hfDelIndex").val($(rowIndex).find('td:eq(12)').text());

        if (SkuType == "") {
            minusQty();
        }
        else {
            document.getElementById('txtUserID').value = "";
            document.getElementById('txtUserPass').value = "";
            $("#hfUserClick").val('Decrease');
            $('#UserValidation').show("slow");
            $('#txtUserID').focus();
        }
    }
}

function minusQty() {

    var Sku_Id = $("#hfskuId").val();
    var Cat_Id = $("#hfcatId").val();
    var Deal_Id = $("#hfDelId").val();
    var C1 = $("#hfDelIndex").val();

    var qty = 1;
    var nQty = 0;
    $('#tble-ordered-products tr').each(function (row, tr) {
        if ($(tr).find('td:eq(0)').text() == Sku_Id && $(tr).find('td:eq(14)').text() == Cat_Id && $(tr).find('td:eq(12)').text() == C1) {
            qty = $(tr).find('td:eq(3) input').val();
            if (Deal_Id != 0) {
                if (qty > parseFloat($(tr).find('td:eq(27)').text()) * parseFloat($(tr).find('td:eq(28)').text())) {
                    nQty = qty - 1;
                    $(tr).find('td:eq(3) input').val(nQty);
                }
            }
            else {

                if (qty == "1") {

                } else {

                    nQty = qty - 1;
                    $(tr).find('td:eq(3) input').val(nQty);
                }
            }
            var price = $(tr).find('td:eq(5)').text();
            var amount = parseFloat(nQty * price).toFixed(2);
            $(tr).find('td:eq(6)').text(amount);

            setTotals();
        }
    });
    var tableData = storeTblValues();
    tableData = JSON.stringify(tableData);
    document.getElementById('hfOrderedproducts').value = tableData;

    $("#hfskuId").val('');
    $("#hfcatId").val('');
    $("#hfDelId").val('');
    $("#hfDelIndex").val('');
}

function DecreaseProceed() {
    $('#DecreaseConfirmation').hide("slow");
    minusQty();
    $('#hfDisable').val("0");
}

function DecreaseCancel() {
    $('#DecreaseConfirmation').hide("slow");
    $("#hfskuId").val('');
    $("#hfcatId").val('');
    $("#hfUserClick").val('');
    $("#hfDelIndex").val('');
    $("#hfDelId").val('');

}

// #endregion Decrease Qty

//------------------------on Print INvoice Click ---------------\\

//-------get Discount,Discount Type from dataBase on Bill selectoin and set on Print Invoice,Payment PopUp--\\

function isDisable(flag) {

    if (flag == 'true') {


        document.getElementById("percentage").disabled = true;
        document.getElementById("value").disabled = true;
    }
    else {

        if (document.getElementById("hfCustomerType").value == "Takeaway") {

            document.getElementById("percentage").disabled = false;
            document.getElementById("value").disabled = false;

        }
    }
}

function isColored(flag) {
    if (flag == 'true') {

        document.getElementById("percentage2").style["background-color"] = "#7dab49";
        document.getElementById("percentage").style["background-color"] = "#7dab49";

        document.getElementById("value2").style["background-color"] = "#919399";
        document.getElementById("value").style["background-color"] = "#919399";
    }
    else {
        document.getElementById("value2").style["background-color"] = "#7dab49";
        document.getElementById("value").style["background-color"] = "#7dab49";

        document.getElementById("percentage2").style["background-color"] = "#919399";
        document.getElementById("percentage").style["background-color"] = "#919399";
    }
}

function calculateDiscount(discount, amountDue) {

    $('#lblDiscountTotal').text('0');
    $("#lblDiscountTotal2").text('0');

    document.getElementById('txtDiscount').value = "";
    document.getElementById('txtDiscount2').value = "";

    var salesTax = 0;
    if ($('#hfServiceCharges').val() == "False") {
        salesTax = document.getElementById("hfSalesTax").value;
        salesTax = parseFloat(salesTax) / 100 * amountDue;
    }
    else {
        salesTax = document.getElementById("txtService2").value;
    }
    if (salesTax == "") {
        salesTax = 0;
    }
    var Total = Math.round(parseFloat(amountDue), 2);

    if (discount > 0) {

        document.getElementById('txtDiscount').value = discount;
        document.getElementById('txtDiscount2').value = discount;


        if ($('#hfDiscountType').val() == 0) {

            discount = Total * (discount / 100);

            $("#lblDiscountTotal2").text(Math.round(discount, 0));
            $("#lblDiscountTotal").text(Math.round(discount, 0));


            isColored('true');

        }
        else {

            $("#lblDiscountTotal2").text(Math.round(discount, 0));
            $("#lblDiscountTotal").text(Math.round(discount, 0));

            isColored('false');

        }
        document.getElementById('txtDiscount2').disabled = true;
        document.getElementById('txtDiscountReason2').disabled = true;
        if (document.getElementById("hfCustomerType").value == "Takeaway") {

            isDisable('false');
        }
        else {

            isDisable('true');
        }
        document.getElementById("lblGSTTotal").innerHTML = Math.round(salesTax);
        document.getElementById("lblGSTTotal2").innerHTML = Math.round(salesTax);
    }
    else {
        isDisable(false);
    }
}

//--------------------Calcualte on discount  on Print Invoice Click PopUp----\\ 
function CalculateBalance2() {
    var balance = 0;
    var amountDue = 0;
    var discount = 0;
    discount = document.getElementById('txtDiscount2').value;
    discount = document.getElementById('txtDiscountReason2').value;    
    var discountType = $('#hfDiscountType').val();
    var grandTotal = $("#GrandTotal2").text();

    var gst = 0;
    if ($('#hfServiceCharges').val() == "False") {
        gst = document.getElementById("hfSalesTax").value;
    }
    else {
        gst = document.getElementById("txtService2").value;
    }

    $("#lblBalance").text('');
    $("#lblPaymentDue").text('');

    if (discount == "") {
        discount = 0;
    }

    if (gst == "") {
        gst = 0;
    }


    if (discountType == 0) {

        discount = parseFloat(grandTotal) * parseFloat(discount) / 100;
        if ($('#hfServiceCharges').val() == "False") {
            gst = parseFloat(gst / 100) * (parseFloat(grandTotal) - parseFloat(discount));
        }
        
        amountDue = parseFloat(grandTotal) + parseFloat(gst) - parseFloat(discount);
        amountDue = Math.round(amountDue, 0);
    }
    else {

        if ($('#hfServiceCharges').val() == "False") {

            gst = parseFloat(gst / 100) * (parseFloat(grandTotal - discount));
        }
        
        amountDue = parseFloat(grandTotal) + parseFloat(gst) - parseFloat(discount);
        amountDue = Math.round(amountDue, 0);
    }

    $("#lblPaymentDue2").text(Math.round(amountDue, 0));
    $("#lblDiscountTotal2").text(Math.round(discount, 0));
    $("#lblGSTTotal2").text(Math.round(gst, 0));
}

function UpdateOrder() {
    var EmpDiscount = 0;
    if ($('#hfDiscountType').val() === undefined || $('#hfDiscountType').val() === null || $('#hfDiscountType').val() === 'null' || $('#hfDiscountType').val() === '') {
        //Do nothing.
    }
    else {
        EmpDiscount = $('select#ddlDiscountType2 option:selected').val();
    }

    if (ValidateDiscount2(EmpDiscount)) {

        var salesTax = document.getElementById("hfSalesTax").value;

        $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmCallCenterOld.aspx/UpdateOrder", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ orderedProducts: $('#hfOrderedproducts').val(), amountDue: $("#GrandTotal2").text(), discount: $('#txtDiscount2').val(), Gst: $("#lblGSTTotal2").text(), DiscType: $('#hfDiscountType').val(), gstPerAge: salesTax, Service: $('#txtService2').val(), takeAwayCustomer: $("#txtTakeawayCustomer").val(), cardType: $('#hfCardTypeId').val(), cardNo: $("#txtLoyaltyCard2").val(), points: $("#hfCardPoints").val(), purchasing: $("#hfCardPurchasing").val(), customerID: $('#hfCustomerId').val(), manualOrderNo: $("#txtManualOrderNo").val(), remarks: $("#txtRemarks").val() }),
                    success: invoiceSaved2,
                    error: OnError
                }
            );
    }
}

function invoiceSaved2() {
    PrintSaleInvoice();
    Clear2();
    GetPendingBills();
    $("#hfTableNo").val("");
    $("#hfTableId").val("0");
    UnlockRecord();
}

//----------------Can enter only numbers and decimal ---------------------------\\

function onlyNumbers(txt, event) {
    var charCode = (event.which) ? event.which : event.keyCode;

    if (charCode == 9 || charCode == 8) {
        return true;
    }
    if (charCode == 46) {
        return false;
    }
    if (charCode == 31 || charCode < 48 || charCode > 57)
        return false;

    return true;
}

function onlyDotsAndNumbers(txt, event) {

    var charCode = (event.which) ? event.which : event.keyCode;

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

    return true;
}

//------------------------------------------------------------------------------\\

//#region Time Calculation

function BillTime() {
    var refresh = 1000; // Refresh rate in milli seconds
    mytime = setTimeout('display_ct()', refresh);
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

function SaveCustomer() {

    if (ValidateCustomer()) {

        $.ajax
            (
                {
                    type: "POST", //HTTP method
                    url: "frmCallCenterOld.aspx/InsertCustomer", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({ cardID: $('#txtCustomerCardNo').val(), CNIC: $('#txtCustomerCNIC').val(), contactNumer: $('#txtPrimaryContact').val(), contactNumer2: $('#txtOtherContact').val(), customerName: $('#txtCustomerName').val(), address: $('#txtCustomerAddress').val(), DOB: $('#txtCustomerDOB').val(), OpeningAmount: $('#txtOpeningAmount').val(), Nature: $('#txtNature').val(), email: $('#txtEmail').val(), LocationID: ddlLocation.GetValue() }),
                    success: ClearCustomer,
                    error: OnError
                }
            );
    }
}

function ValidateCustomer() {
    if ($('#txtCustomerName').val() == "") {
        Error("Must enter Customer Name");

        return false;
    }
    if ($('#txtPrimaryContact').val() == "") {
        Error("Must enter Primary Contact");
        return false;
    }
    if ($('#txtCustomerAddress').val() == "") {
        Error("Must enter Address");
        return false;
    }
    return true;
}

//On KEY UP txtCustomerSearch
function LoadAllCustomers() {
    $.ajax
       (
           {
               type: "POST", //HTTP method
               url: "frmCallCenterOld.aspx/LoadAllCustomers", //page/method name
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               data: JSON.stringify({ customerName: $('#txtCustomerSearch').val(), type: "Search", LocationID: ddlLocation.GetValue() }),
               success: LoadAllCustomer
           }
 );
}

function LoadAllCustomer(customers) {

    customers = JSON.stringify(customers);
    var result = jQuery.parseJSON(customers.replace(/&quot;/g, '"'));

    customers = eval(result.d);

    $("#tbl-customers").empty();

    for (var i = 0, len = customers.length; i < len; i++) {
        var row = $('<tr ><td style="display:none;">' + customers[i].CUSTOMER_ID + '</td><td class="leftval">' + customers[i].CUSTOMER_NAME + '</td><td class="leftval">' + customers[i].CARD_NO + '</td><td class="leftval">' + customers[i].CONTACT_NUMBER + '</td><td class="leftval"  style="display:none;">' + customers[i].EMAIL_ADDRESS + '</td><td style="display:none;">' + customers[i].CNIC + '</td><td class="leftval"  style="display:none;">' + customers[i].REGDATE + '</td><td class="leftval">' + customers[i].ADDRESS + '</td><td style="display:none;" align="center" onclick="ShowCustomer(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td>' + '<td align="center" onclick="ShowCustomer(this);"><a href="#"><span class="fa fa-share-square-o"></span></a></td></tr>');
        $('#tbl-customers').append(row);
    }
}
//Load Location
function LoadLocationDropDown() {
    $.ajax
       (
           {
               type: "POST", //HTTP method
               url: "frmCallCenterOld.aspx/LoadLocation", //page/method name
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
    var locations2 = JSON.stringify(locations);
    document.getElementById("hfLocations").value = locations2;

    ddlLocation.ClearItems();
    ddlLocation.BeginUpdate();
    for (var i = 0, len = locations.length; i < len; i++) {
        ddlLocation.AddItem(locations[i].DISTRIBUTOR_NAME, locations[i].DISTRIBUTOR_ID);
    }
    ddlLocation.EndUpdate();
    ddlLocation.SetSelectedIndex(0);
    UnlockRecord();
    LoadAllModifiers();
    loadAllProducts();
    GetPendingBills();
}
//On Save Customer
function LoadCustomers() {
    $.ajax
        (
            {
                type: "POST", //HTTP method
                url: "frmCallCenterOld.aspx/LoadAllCustomers", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ customerName: "", type: "Max", LocationID: ddlLocation.GetValue() }),
                success: LoadLastCustomer
            }
        );
}

function LoadLastCustomer(customers) {

    customers = JSON.stringify(customers);
    var result = jQuery.parseJSON(customers.replace(/&quot;/g, '"'));

    customers = eval(result.d);

    $("#tbl-customers").empty();

    for (var i = 0, len = customers.length; i < len; i++) {

        var row = $('<tr ><td style="display:none;">' + customers[i].CUSTOMER_ID + '</td><td class="leftval">' + customers[i].CUSTOMER_NAME + '</td><td class="leftval">' + customers[i].CARD_NO + '</td><td class="leftval">' + customers[i].CONTACT_NUMBER + '</td><td class="leftval"  style="display:none;">' + customers[i].EMAIL_ADDRESS + '</td><td class="leftval"  style="display:none;">' + customers[i].CNIC + '</td><td class="leftval" style="display:none;">' + customers[i].REGDATE + '</td><td class="leftval">' + customers[i].ADDRESS + '</td><td style="display:none;" align="center" onclick="ShowCustomer(this);"><a href="#"><span class="fa fa-pencil-square-o"></span></a></td>' + '<td align="center" onclick="ShowCustomer(this);"><a href="#"><span class="fa fa-share-square-o"></span></a></td></tr>');
        $('#tbl-customers').append(row);
    }

    var table = document.getElementById('tbl-customers');
    var lastRow = table.rows[table.rows.length - 1];

    ShowCustomer2(lastRow);
    HoldOrder();
    LoadLastBill();
}

//After Insertion Select Last Customer
function ShowCustomer2(rowIndex) {
    document.getElementById('hfCustomerId').value = $(rowIndex).find('td:eq(0)').text();
    document.getElementById('hfCustomerIDNew').value = $(rowIndex).find('td:eq(0)').text();
    document.getElementById('hfCustomerNo').value = $(rowIndex).find('td:eq(3)').text();
    document.getElementById('hfTableNo').value = $(rowIndex).find('td:eq(1)').text();//Customer name used For Printing on Unique Section and Print Order
    document.getElementById('txtCustomerNameNew').value = $(rowIndex).find('td:eq(1)').text();
    document.getElementById('divCustomer').style.visibility = 'hidden';

    $("#tbl-customers").empty();
}

//Get Customer Detail on Selection Button on row click
function ShowCustomer(obj) {
    var rowIndex = $(obj).parent();

    document.getElementById('hfCustomerId').value = $(rowIndex).find('td:eq(0)').text();
    document.getElementById('hfCustomerIDNew').value = $(rowIndex).find('td:eq(0)').text();
    document.getElementById('hfCustomerNo').value = $(rowIndex).find('td:eq(3)').text();
    document.getElementById('hfTableNo').value = $(rowIndex).find('td:eq(1)').text();//Customer name used For Printing on Unique Section and Print Order
    document.getElementById('txtCustomerNameNew').value = $(rowIndex).find('td:eq(1)').text();
    document.getElementById('divCustomer').style.visibility = 'hidden';

    $('#txtCustomerSearch').val('');
    $("#tbl-customers").empty();

    HoldOrder();
    LoadLastBill();
}

function LoadLastBill() {

    setTimeout(function () {
        var table = document.getElementById('tbl-pending-bills');
        var lastRow = table.rows[table.rows.length - 1];

        if ($(lastRow).find("td:eq(10)").text() == "DLY") {

            var amountDue = $(lastRow).find("td:eq(2)").text();

            if (amountDue == 0) {

                ShowBill(lastRow);
            }
        }
        else {
            ShowBill(lastRow);
        }
    }, 1000);
}

//#endregion

//#region SMS

function SendSMS(obj) {

    var rowIndex = $(obj).parent();
    document.getElementById('hfCustomerNo').value = $(rowIndex).find('td:eq(15)').text();

    SMS(document.getElementById('hfCustomerNo').value, "2");
    document.getElementById('hfCheckSMS').value = "1";
}

//msgType: 1 for hold, 2 for Ride
function SMS(Number, Type) {
    //$.blockUI();

    //$.ajax
    //       (
    //           {
    //               type: "POST", //HTTP method
    //               url: "frmCallCenterOld.aspx/SendSMS", //page/method name
    //               contentType: "application/json; charset=utf-8",
    //               dataType: "json",
    //               data: JSON.stringify({ customerNo: Number, msgType: Type }),
    //               success: function (data) {
    //                   if (data.d == "100" || data.d == "Message Sent To Telecom") {
    //                       Succes(data.d);
    //                   } else {
    //                       Error(data.d);
    //                   }
    //               },
    //               error: OnError,
    //               complete: function () {

    //                   $.unblockUI();
    //               }
    //           }
    //       );
}

//#endregion

//region Clear

//ON Hold Order
function ClearOrder() {
    $('#txtRemarks').val('');
    $("#txtManualOrderNo").val('');
    $('#tble-ordered-products').empty();
    $('#hfOrderedproducts').val('');
    $('#hfCustomerId').val('0');
    $('#txtVacantTable').val('');
    GetPendingBills();

    $("#hfTableNo").val("");
    $("#hfTableId").val("0");
    $('#hfDisable').val("0");

    if (document.getElementById("hfCustomerType").value == "Dine In") {
        $("#TableNo1").text("N/A");
    }
    $("#OrderNo1").text("N/A");
    
    $("#lblTotalGrossAmount").text("");
    $("#hfCounter").val(0);

    document.getElementById('dvDealUpdate').style.display = 'none';
    $("#txtDealQty").val('1');
    UnlockRecord();
}

//ON Payment 
function Clear() {
    $('#txtRemarks').val('');
    $("#txtManualOrderNo").val('');
    $('#txtVacantTable').val('');
    $('#tble-ordered-products').empty();
    $('#hfOrderedproducts').val('');
    $('#hfPaymentType').val('');
    $('#txtDiscount').val('');
    $('#txtCashRecieved').val('');
    $('#txtDiscountReason').val('');
    $('#txtDiscountReason2').val('');
    $("#lblDiscountTotal").text('0.00');
    $("#lblTotalGrossAmount").text("");
    $('#hfDisable').val("0");
    $('#hfDiscountType').val('');
    $('#hfchkDiscountType').val('');

    $('#payment').hide("slow");
    if (document.getElementById("hfCustomerType").value == "Dine In") {
        $("#TableNo1").text("N/A");
    }
    $("#OrderNo1").text("N/A");
    $('#hfCustomerId').val('0');
    $('#hfCustomerNo').val('');
    $("#txtService").val('');
    $("#txtService2").val('');
    $("#txtTakeawayCustomer").val('');
    $("#hfCounter").val(0);

    document.getElementById('dvDealUpdate').style.display = 'none';
    $("#txtDealQty").val('1');

    document.getElementById("percentage").style["background-color"] = "#919399";
    document.getElementById("value").style["background-color"] = "#919399";
    document.getElementById("cash").style["background-color"] = "#919399";
    document.getElementById("credit").style["background-color"] = "#919399";

    $("select#ddlDiscountType").prop('selectedIndex', 0);
    loadDiscountUser();
    loadUsers(document.getElementById("ddlDiscountType"));

    if ($('#hfModifiedItemsShown').val() == '1') {
        document.getElementById('dv_lstModifyCategory').style.display = "block";
        document.getElementById('dv_lstModifyProducts').style.display = "block";
    }
    else {
        document.getElementById('dv_lstModifyCategory').style.display = "none";
        document.getElementById('dv_lstModifyProducts').style.display = "none";
    }
    document.getElementById('dv_lstProducts').style.display = "block";
    document.getElementById('dv_lstCategory').style.display = "block";
}

function ClearOnCancel() {
    $("#txtManualOrderNo").val('');
    $('#txtRemarks').val('');
    $('#tble-ordered-products').empty();
    $('#hfOrderedproducts').val('');
    $("#lblTotalGrossAmount").text("");
    $('#hfPaymentType').val('');
    $('#txtDiscount').val('');
    $('#txtCashRecieved').val('');
    $('#hfDisable').val("0");
    $('#hfDiscountType').val('');
    $('#hfchkDiscountType').val('');
    $('#txtDiscountReason').val('');
    $('#txtDiscountReason2').val('');
    $('#payment').hide("slow");
    $("#hfTableNo").val("");
    $("#hfTableId").val("0");
    if (document.getElementById("hfCustomerType").value == "Dine In") {
        $("#TableNo1").text("N/A");
    }
    $("#OrderNo1").text("N/A");
    $('#hfCustomerId').val('0');
    $('#hfCustomerNo').val('');
    $("#txtService").val('');
    $("#txtService2").val('');
    $("#txtTakeawayCustomer").val('');
    $("#hfCounter").val(0);

    document.getElementById('dvDealUpdate').style.display = 'none';
    $("#txtDealQty").val('1');

    document.getElementById("percentage").style["background-color"] = "#919399";
    document.getElementById("value").style["background-color"] = "#919399";
    document.getElementById("cash").style["background-color"] = "#919399";
    document.getElementById("credit").style["background-color"] = "#919399";    
    UnlockRecord();
    $("select#ddlDiscountType").prop('selectedIndex', 0);
    loadDiscountUser();
    loadUsers(document.getElementById("ddlDiscountType"));

    if ($('#hfModifiedItemsShown').val() == '1') {
        document.getElementById('dv_lstModifyCategory').style.display = "block";
        document.getElementById('dv_lstModifyProducts').style.display = "block";
    }
    else {
        document.getElementById('dv_lstModifyCategory').style.display = "none";
        document.getElementById('dv_lstModifyProducts').style.display = "none";
    }
    document.getElementById('dv_lstProducts').style.display = "block";
    document.getElementById('dv_lstCategory').style.display = "block";
}

//ON Print Invoice
function Clear2() {
    $("#txtManualOrderNo").val('');
    $('#txtRemarks').val('');
    $('#tble-ordered-products').empty();
    $('#hfOrderedproducts').val('');
    $("#lblTotalGrossAmount").text("");
    $('#txtDiscount2').val('');
    $('#txtDiscountReason2').val('');
    $('#txtDiscountReason').val('');
    $('#hfDiscountType').val('');
    $('#hfchkDiscountType').val('');
    $('#hfDisable').val("0");
    if (document.getElementById("hfCustomerType").value == "Dine In") {
        $("#TableNo1").text("N/A");
    }
    $("#OrderNo1").text("N/A");
    $("#txtService").val('');
    $("#txtService2").val('');
    $('#hfCustomerId').val('0');
    $('#hfCustomerNo').val('');
    $("#txtTakeawayCustomer").val('');
    $('#payment2').hide("slow");

    document.getElementById('dvDealUpdate').style.display = 'none';
    $("#txtDealQty").val('1');
    document.getElementById("percentage2").style["background-color"] = "#919399";
    document.getElementById("value2").style["background-color"] = "#919399";
    $('#dvDiscount2').find('*').prop('disabled', false);
    document.getElementById('txtDiscount2').disabled = true;
    document.getElementById('txtDiscountReason2').disabled = true;
    document.getElementById('txtDiscount').disabled = true;
    $("#txtLoyaltyCard").val('');
    $('#hfCardTypeId').val('0');
    $('#hfCardPoints').val('0');
    $('#hfCardPurchasing').val('0');
    $('#hfCardAmountLimit').val('0');

    document.getElementById('dv_lstModifyCategory').style.display = "block";
    document.getElementById('dv_lstModifyProducts').style.display = "block";
    document.getElementById('dv_lstProducts').style.display = "block";
    document.getElementById('dv_lstCategory').style.display = "block";
}

function ClearOnCancel2() {
    $("#txtManualOrderNo").val('');
    $('#txtRemarks').val('');
    $('#tble-ordered-products').empty();
    $('#hfOrderedproducts').val('');
    $("#lblTotalGrossAmount").text("");
    $('#txtDiscount2').val('');
    $('#txtDiscount').val('');
    $('#hfDisable').val("0");
    $('#hfDiscountType').val('');
    $('#hfchkDiscountType').val('');
    $('#txtDiscountReason2').val('');
    $('#txtDiscountReason').val('');
    if (document.getElementById("hfCustomerType").value == "Dine In") {
        $("#TableNo1").text("N/A");
    }
    $("#OrderNo1").text("N/A");
    $('#hfCustomerId').val('0');
    $('#hfCustomerNo').val('');
    $("#txtService").val('');
    $("#txtService2").val('');
    $("#txtTakeawayCustomer").val('');

    $('#payment2').hide("slow");

    document.getElementById('dvDealUpdate').style.display = 'none';
    $("#txtDealQty").val('1');
    document.getElementById("percentage2").style["background-color"] = "#919399";
    document.getElementById("value2").style["background-color"] = "#919399";
    $('#dvDiscount2').find('*').prop('disabled', false);
    document.getElementById('txtDiscount2').disabled = true;
    document.getElementById('txtDiscountReason2').disabled = true;
    document.getElementById('txtDiscount').disabled = true;
    $("#txtLoyaltyCard").val('');
    $('#hfCardTypeId').val('0');
    $('#hfCardPoints').val('0');
    $('#hfCardPurchasing').val('0');
    $('#hfCardAmountLimit').val('0');
    UnlockRecord();
    document.getElementById('dv_lstModifyCategory').style.display = "block";
    document.getElementById('dv_lstModifyProducts').style.display = "block";
    document.getElementById('dv_lstProducts').style.display = "block";
    document.getElementById('dv_lstCategory').style.display = "block";
}

//ON Customer 
function ClearCustomer() {
    $('#txtCustomerCardNo').val('');
    $('#txtPrimaryContact').val('');
    $('#txtOtherContact').val('');
    $('#txtCustomerName').val('');
    $('#txtCustomerAddress').val('');
    $('#txtCustomerCNIC').val('');
    $('#txtCustomerDOB').val('');
    $('#txtCustomerSearch').val('');
    $('#txtEmail').val('');
    $('#txtOpeningAmount').val('');
    $('#txtNature').val('');
    $('#txtBarcode').val('');
    document.getElementById('divCustomer').style.visibility = 'hidden';
    LoadCustomers();
}

function CancelCustomer() {
    $('#txtVacantTable').val('');
    $('#txtCustomerCardNo').val('');
    $('#txtPrimaryContact').val('');
    $('#txtOtherContact').val('');
    $('#txtCustomerName').val('');
    $('#txtCustomerCNIC').val('');
    $('#txtCustomerDOB').val('');
    $('#txtCustomerAddress').val('');
    $('#txtEmail').val('');
    $('#txtOpeningAmount').val('');
    $('#txtNature').val('');
    $('#txtBarcode').val('');

    // $('#divCustomer').css({ opacity: 0, visibility: "visible" }).animate({ opacity: 1 }, 'slow');
    document.getElementById('divCustomer').style.visibility = 'hidden';
    // document.getElementById('divCustomer').className = "hideCustomer"; // Fade out

}
function PaymentReceivedPress(e) {
    var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
    if (key == 13) {
        e.preventDefault();
        $('#btnSave').focus();
    }
    else {
        if (key == 9 || key == 8) {
            e.preventDefault();
            return;
        }
        if (key == 46) {
            e.preventDefault();
            return;
        }
        if (key == 31 || key < 48 || key > 57) {
            e.preventDefault();
            return;
        }
    }
}
function ModifierDone()
{
    $('#btnMenuItem').trigger("click");
    //$('#' + $('#hfDefaultCategoryID').val() + '').trigger("click");
    $('#divModifier').hide("slow");
}
//----------------------------------------------------------
function LocationChanged(SelectedValue) {
    $('#hfCustomerAdd').val('0');
    UnlockRecord();

    //Clear Items
    var dvLstProducts = document.getElementById("dv_lstProducts");
    while (dvLstProducts.hasChildNodes()) {
        dvLstProducts.removeChild(dvLstProducts.lastChild);
    }
    LoadAllModifiers();
    loadAllProducts();
    GetPendingBills();
}
function ShowCustomerPopup() {
    $('#hfCustomerAdd').val('1');
    document.getElementById('divCustomer').style.visibility = 'visible';
}
var minute = 1000 * 60;
var hour = minute * 60;
var day = hour * 24;
var halfamonth = day * 15;
var month = day * 30;
function getDateDiff(dateTimeStamp) {
    var now = new Date().getTime();
    var diffValue = now - dateTimeStamp;
    if (diffValue < 0) {
        //If the date is different, the pop-up window will tell you
        //Alert (" end date cannot be less than start date!" );
    }
    var monthC = diffValue / month;
    var weekC = diffValue / (7 * day);
    var dayC = diffValue / day;
    var hourC = diffValue / hour;
    var minC = diffValue / minute;
    if (monthC >= 1) {
        result = parseInt(monthC) > 1 ? parseInt(monthC) + " months ago" : parseInt(monthC) + " month ago";
    }
    else if (weekC >= 1) {
        result = parseInt(weekC) > 1 ? parseInt(weekC) + " weeks ago" : parseInt(weekC) + " week ago";
    }
    else if (dayC >= 1) {
        result = parseInt(dayC) > 1 ? parseInt(dayC) + " days ago" : parseInt(dayC) + " day ago";
    }
    else if (hourC >= 1) {
        result = parseInt(hourC) > 1 ? parseInt(hourC) + " hours ago" : parseInt(hourC) + " hour ago";
    }
    else if (minC >= 1) {
        result = parseInt(minC) + " min ago";
    } else {
        result = " Just now";
    }

    return result;
}