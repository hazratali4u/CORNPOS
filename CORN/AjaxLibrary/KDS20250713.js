var sale_Invice_ID = 0;
$(document).ready(function () {
    LoadDefault();
});

function LoadDefault() {
    trHtml = '';
    trHtmlCallback = '';
    orderNotes = '';
    GetPendingOrders();
    GetPendingAndCompeletdOrdersCount();
    setTimeout(function () {
        LoadDefault();
    }, 5000);
}

function GetPendingOrders() {
    $.ajax({
        type: "POST",
        url: "frmKitchenOrderTaking.aspx/SelectPendingBills",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: DecorateItems
    });
}

var trHtml = '';
var trHtmlCallback = '';
var orderNotes = '';
var sectioncount = 0;    
function DecorateItems(Items) {        
    LoadItem();
    Items = JSON.parse(Items.d);
    if (sectioncount < Items.length) {
        var audio = new Audio("../images/beep.mp3");
        audio.play();
    }
    var lstProducts = document.getElementById("hfItem").value;

    sectioncount = Items.length;
    $.each(Items, function (index, value) {
        var showBox = true;

        if (value.HideKDSMarkCompleteBtn == 1) {
            products = eval(lstProducts);
            var pendingProducts = products.filter(row => row.IsReady == false &&
            row.INVOICE_ID == value.INVOICE_ID);

            if (pendingProducts.length == 0)
            {
                showBox = false;
            }
        }
            
        if (showBox) {
            trHtml += '<div class="col-lg-3 column-card" style="display: inline-block;vertical-align: top;">' +
               '<div class="card">';
            if (value.SERVICE_TYPE == 'Delivery') {
                trHtml += '<div class="card-header style-1">' +
                '<h3><i class="fa fa-motorcycle"></i> Kot No. ' + value.ORDER_NO + ' </h3>';
            }
            else if (value.SERVICE_TYPE == 'Takeaway') {
                trHtml += '<div class="card-header style-2">' +
                '<h3><i class="fa fa-child"></i> Kot No. ' + value.ORDER_NO + ' </h3>';
            }
            else if (value.SERVICE_TYPE == 'DineIn') {
                trHtml += '<div class="card-header teststyle78">' +
                '<h3><i class="fas fa-utensils"></i> Kot No. ' + value.ORDER_NO + ' </h3>';
            }

            if (value.SERVICE_TYPE == "DineIn") {
                trHtml += '<p style="font-size:19px;"> Dine In - Table No: ' + value.TABLE_NO2 + '</p>';
            }
            else {
                trHtml += '<table style="width=100%;">'
                trHtml += '<tr>'
                trHtml += '<td style=width:40%;>'
                trHtml += '<p style="font-size:19px;">' + value.SERVICE_TYPE + '</p>';
                trHtml += '</td>'
                trHtml += '<td style=width:60%;>'
                trHtml += '<span style=display:none>' + new Date(parseInt(value.ORDER_DATE_TIME.substr(6))) + '</span>' +
                      '<p>' + value.ORDER_TIME + ' - <strong style=color:#de2727;font-size:17px;font-family:calibri;>' + getDateDiff(new Date(parseInt(value.ORDER_DATE_TIME.substr(6)))) + '</strong></p>';
                trHtml += '</td>'
                trHtml += '</tr>'                
                trHtml += '</table>';
                if (value.SERVICE_TYPE == 'Delivery') {
                    trHtml += '</div>';
                }
            }
            if (value.SERVICE_TYPE !== 'Delivery') {
                if (value.SERVICE_TYPE == 'DineIn') {
                    trHtml += '<span style=display:none>' + new Date(parseInt(value.ORDER_DATE_TIME.substr(6))) + '</span>' +
                     '<p>' + value.ORDER_TIME + ' - <strong style=color:#de2727;font-size:17px;font-family:calibri;>' + getDateDiff(new Date(parseInt(value.ORDER_DATE_TIME.substr(6)))) + '</strong></p>' +
                     '<p>Order Taker: ' + value.ORDER_BOOKER_NAME + ' </p>' +
                     '<p>Covers: ' + value.covertable + ' </p>' +
                 '</div>';
                }
                else {
                    trHtml += '<span style=display:none>' + new Date(parseInt(value.ORDER_DATE_TIME.substr(6))) + '</span>' +
                     '<p>' + value.ORDER_TIME + ' - <strong style=color:#de2727;font-size:17px;font-family:calibri;>' + getDateDiff(new Date(parseInt(value.ORDER_DATE_TIME.substr(6)))) + '</strong></p>' +
                     '<p>Order Taker: ' + value.ORDER_BOOKER_NAME + ' </p>' +
                 '</div>';
                }
            }
            sale_Invice_ID = value.INVOICE_ID;
            orderNotes = value.REMARKS;

            var now = new Date().getTime();
            var diffValue = now - (new Date(parseInt(value.ORDER_DATE_TIME.substr(6))));
            var minC = diffValue / (1000 * 60);
            var color = '';
            if (parseInt(minC) > 20)
            {
                color = 'red';
            }
            LoadItemInfo(value.INVOICE_ID, value.HideKDSMarkCompleteBtn,color);
            trHtml += '</div></div>';            
        }
        else {//Recall
            trHtmlCallback += '<div class="col-lg-3 column-card" style="display: inline-block;vertical-align: top;">' +
               '<div class="card">';
            if (value.SERVICE_TYPE == 'Delivery') {
                trHtmlCallback += '<div class="card-header style-1">' +
                '<h3><i class="fa fa-motorcycle"></i> Kot No. ' + value.ORDER_NO + ' </h3>';
            }
            else if (value.SERVICE_TYPE == 'Takeaway') {
                trHtmlCallback += '<div class="card-header style-2">' +
                '<h3><i class="fa fa-child"></i> Kot No. ' + value.ORDER_NO + ' </h3>';
            }
            else if (value.SERVICE_TYPE == 'DineIn') {
                trHtmlCallback += '<div class="card-header teststyle78">' +
                '<h3><i class="fas fa-utensils"></i> Kot No. ' + value.ORDER_NO + ' </h3>';
            }

            if (value.SERVICE_TYPE == "DineIn") {
                trHtmlCallback += '<p style="font-size:19px;"> Dine In - Table No: ' + value.TABLE_NO2 + '</p>';
            }
            else {
                trHtmlCallback += '<table style="width=100%;">'
                trHtmlCallback += '<tr>'
                trHtmlCallback += '<td style=width:40%;>'
                trHtmlCallback += '<p style="font-size:19px;">' + value.SERVICE_TYPE + '</p>';
                trHtmlCallback += '</td>'
                trHtmlCallback += '<td style=width:60%;>'
                trHtmlCallback += '<span style=display:none>' + new Date(parseInt(value.ORDER_DATE_TIME.substr(6))) + '</span>' +
                      '<p>' + value.ORDER_TIME + ' - <strong style=color:#de2727;font-size:17px;font-family:calibri;>' + getDateDiff(new Date(parseInt(value.ORDER_DATE_TIME.substr(6)))) + '</strong></p>';
                trHtmlCallback += '</td>'
                trHtmlCallback += '</tr>'
                trHtmlCallback += '</table>';
                if (value.SERVICE_TYPE == 'Delivery') {
                    trHtmlCallback += '</div>';
                }
            }
            if (value.SERVICE_TYPE !== 'Delivery') {
                if (value.SERVICE_TYPE == 'DineIn') {
                    trHtmlCallback += '<span style=display:none>' + new Date(parseInt(value.ORDER_DATE_TIME.substr(6))) + '</span>' +
                     '<p>' + value.ORDER_TIME + ' - <strong style=color:#de2727;font-size:17px;font-family:calibri;>' + getDateDiff(new Date(parseInt(value.ORDER_DATE_TIME.substr(6)))) + '</strong></p>' +
                     '<p>Order Taker: ' + value.ORDER_BOOKER_NAME + ' </p>' +
                     '<p>Covers: ' + value.covertable + ' </p>' +
                 '</div>';
                }
                else {
                    trHtmlCallback += '<span style=display:none>' + new Date(parseInt(value.ORDER_DATE_TIME.substr(6))) + '</span>' +
                     '<p>' + value.ORDER_TIME + ' - <strong style=color:#de2727;font-size:17px;font-family:calibri;>' + getDateDiff(new Date(parseInt(value.ORDER_DATE_TIME.substr(6)))) + '</strong></p>' +
                     '<p>Order Taker: ' + value.ORDER_BOOKER_NAME + ' </p>' +
                 '</div>';
                }
            }
            sale_Invice_ID = value.INVOICE_ID;
            orderNotes = value.REMARKS;

            var now = new Date().getTime();
            var diffValue = now - (new Date(parseInt(value.ORDER_DATE_TIME.substr(6))));
            var minC = diffValue / (1000 * 60);
            var color = '';
            if (parseInt(minC) > 20) {
                color = 'red';
            }
            LoadItemInfoCallback(value.INVOICE_ID, value.HideKDSMarkCompleteBtn,color);
            trHtmlCallback += '</div></div>';            
        }
    });

    $("#order-table").html(trHtml);
    $("#order-table-callback").html(trHtmlCallback);
}

function LoadItem() {
    $.ajax({
        type: "POST", //HTTP method
        url: "frmKitchenOrderTaking.aspx/GetPendingBill", //page/method name
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: LoadItemDetail
    });
}

function LoadItemDetail(products)
{        
    products = JSON.stringify(products);
    var result = jQuery.parseJSON(products.replace(/&quot;/g, '"'));
    products = eval(result.d);
    products = JSON.stringify(products);
    document.getElementById("hfItem").value = products;        
}

function LoadItemInfo(sale_Invice_ID, HideMarkCompleteBtn,color) {
    var lstProducts = document.getElementById("hfItem").value;        
    var voidCount = 0;
    var newDivCount = 0;
    var counter = 0;
    products = eval(lstProducts);
    trHtml += '<div class="card-body" style="color:' + color + '">';
    
    
    products = products.filter(row => row.INVOICE_ID == sale_Invice_ID);

    if (products[0].ShowSelectAllOnKDS == "1") {
        trHtml += '<div style="text-align:left;"> <input style="margin-left:-10px;" id="select-all" data-id="' + sale_Invice_ID + '" type="checkbox"'
        + '> <span style="color:forestgreen;font-weight:500">Mark All</span></div>';
    }

    for (var i = 0; i < products.length; i++) {
        if (counter % 5 == 0 && counter !== 0 && products[i].ScrollOnKDSOrder == "0") {
            counter = 0;
            trHtml += '<div class="card-footer"><p>Continued <i class="fa fa-arrow-right"></i></p></div></div></div></div><div class="col-lg-3 column-card" style="display: inline-block; padding-left: 0px;">' +
            '<div class="card">' +
                '<div class="card-header" style="padding: 0px; padding-bottom: .75rem;">' +
                    '<p>Continued <i class="fa fa-arrow-right"></i></p>' +
                '</div>' +
                '<div class="card-body" style="color:' + color + '">';
            newDivCount++;
            console.log("newdiv:" + newDivCount + ", counter: " + counter);
        }

        //First time order
        if (products[i].IsNewAdded == true) {
            if (products[i].VOID == false) {
                var readyHtml = '';
                //if (products[i].MODIFIER == 0) {
                readyHtml += '<input class="item-ready" data-id="' + products[i].SALE_INVOICE_DETAIL_ID + '" type="checkbox">';
                if (products[i].IsReady == true)
                    readyHtml = '<input class="item-ready" data-id="' + products[i].SALE_INVOICE_DETAIL_ID + '" type="checkbox" checked="checked">';
                // }
                if (products[i].PreparationTime > 0) {
                    trHtml += '<table width="100%"><tr>';

                    if (products[i].MODIFIER == 0) {
                        trHtml += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                        '<span class="checkmark"></span></label></td>' +
                        '<td style="width:75%;"><p>' + products[i].QTY + ' x ' + products[i].SKU_NAME + '</p></td>'+
                        '<td style="width:20%;" align="right"><p style="color:red">' + products[i].PreparationTime + ' Min</p></td></tr>';
                        if (products[i].ORDER_NOTES != '')
                        {
                            trHtml += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else{
                            trHtml += '</table>';
                        }
                    }
                    else {
                        trHtml += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark"></span></label></td>' +
                        '<td style="width:75%;"><p style="color:blue">' + products[i].QTY + ' x ' + products[i].SKU_NAME + '</p></td><td style="width:20%;" align="right"><p style="color:red">' + products[i].PreparationTime + ' Min</p></td></tr>';

                        if (products[i].ORDER_NOTES != '') {
                            trHtml += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else {
                            trHtml += '</table>';
                        }
                    }
                }
                else {
                    trHtml += '<table width="100%"><tr>';
                    if (products[i].MODIFIER == 0) {
                        trHtml += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                        '<span class="checkmark"></span></label></td>' +
                        '<td style="width:75%;"><p>' + products[i].QTY + ' x ' + products[i].SKU_NAME + '</p></td><td style="width:20%;" align="right"></td></tr>';
                            
                        if (products[i].ORDER_NOTES != '') {
                            trHtml += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else {
                            trHtml += '</table>';
                        }
                    }
                    else {
                        trHtml += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark"></span></label></td>' +
                        '<td style="width:75%;"><p style="color:blue">' + products[i].QTY + ' x ' + products[i].SKU_NAME + '</p></td><td style="width:20%;" align="right"></td></tr>';
                            
                        if (products[i].ORDER_NOTES != '') {
                            trHtml += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else {
                            trHtml += '</table>';
                        }
                    }
                }
            }
        }
        if (i == products.length - 1) {
            //KDS History Table Items
            var runningCount = 0;
            for (var j = 0; j < products.length; j++) {
                if (counter % 5 == 0 && counter !== 0 && products[j].ScrollOnKDSOrder == "0") {
                    counter = 0;
                    trHtml += '<div class="card-footer"><p>Continued <i class="fa fa-arrow-right"></i></p></div></div></div></div><div class="col-lg-3 column-card" style="display: inline-block; padding-left: 0px;">' +
                    '<div class="card">' +
                        '<div class="card-header" style="padding: 0px; padding-bottom: .75rem;">' +
                            '<p>Continued <i class="fa fa-arrow-right"></i></p>' +
                        '</div>' +
                        '<div class="card-body" style="color:' + color + '">';
                    newDivCount++;
                    console.log("newdiv:" + newDivCount + ", counter: " + counter);
                }

                if (products[j].VOID == false) {
                    if (products[j].IsNewAdded == false) {
                        if (runningCount == 0) {
                            trHtml += '<p class="heading" style="background-color: green;"><i class="fa fa-plus"></i>Running Order - ' + formatMicrosoftDate(products[j].LASTUPDATE_DATE) + '</p>';
                        }
                        runningCount++;

                        var readyHtml = '';
                        //if (products[j].MODIFIER == 0) {
                        readyHtml += '<input class="item-ready" data-id="' + products[j].SALE_INVOICE_DETAIL_ID + '" type="checkbox">';
                        if (products[j].IsReady == true)
                            readyHtml = '<input class="item-ready" data-id="' + products[j].SALE_INVOICE_DETAIL_ID + '" type="checkbox" checked="checked">';
                        //}
                        if (products[j].PreparationTime > 0) {
                            trHtml += '<table width="100%"><tr>';

                            if (products[j].MODIFIER == 0) {
                                trHtml += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark"></span></label></td>' +
                                '<td style="width:75%;"><p>' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"><p style="color:red">' + products[j].PreparationTime + ' Min</p></td></tr>';
                                    
                                if (products[i].ORDER_NOTES != '') {
                                    trHtml += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red";text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                                }
                                else {
                                    trHtml += '</table>';
                                }
                            }
                            else {
                                trHtml += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark"></span></label></td>' +
                                '<td style="width:75%;"><p style="color:blue">' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"><p style="color:red">' + products[j].PreparationTime + ' Min</p></td></tr>';

                                if (products[i].ORDER_NOTES != '') {
                                    trHtml += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red";text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                                }
                                else {
                                    trHtml += '</table>';
                                }
                            }
                        }
                        else {
                            trHtml += '<table width="100%"><tr>';
                            if (products[j].MODIFIER == 0) {
                                trHtml += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark"></span></label></td>' +
                                '<td style="width:75%;"><p>' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"></td></tr>';

                                if (products[i].ORDER_NOTES != '') {
                                    trHtml += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                                }
                                else {
                                    trHtml += '</table>';
                                }
                            }
                            else {
                                trHtml += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark"></span></label></td>' +
                                '<td style="width:75%;"><p style="color:blue">' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"></td></tr>';

                                if (products[i].ORDER_NOTES != '') {
                                    trHtml += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                                }
                                else {
                                    trHtml += '</table>';
                                }
                            }
                        }
                    }
                }

                counter = $($(trHtml).find('.card-body').last()).find('tr').length;
            }
            //VOID Items
            for (var j = 0; j < products.length; j++) {
                if (counter % 5 == 0 && counter !== 0 && products[j].ScrollOnKDSOrder == "0") {
                    counter = 0;
                    trHtml += '<div class="card-footer"><p>Continued <i class="fa fa-arrow-right"></i></p></div></div></div></div><div class="col-lg-3 column-card" style="display: inline-block; padding-left: 0px;">' +
                    '<div class="card">' +
                        '<div class="card-header" style="padding: 0px; padding-bottom: .75rem;">' +
                            '<p>Continued <i class="fa fa-arrow-right"></i></p>' +
                        '</div>' +
                        '<div class="card-body" style="color:' + color + '">';
                    newDivCount++;
                    console.log("newdiv:" + newDivCount + ", counter: " + counter);
                }

                if (products[j].VOID == true) {
                    if (voidCount == 0) {
                        trHtml += '<p class="heading"><i class="fa fa-exclamation-circle"></i>Void</p>';
                        voidCount++;
                    }
                    if (products[j].PreparationTime > 0) {
                        trHtml += '<table width="100%"><tr><td style="width:80%;"><p class="void-order">' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"><p class="void-order" style="color:red">' + products[j].PreparationTime + ' Min</p></td></tr>';

                        if (products[i].ORDER_NOTES != '') {
                            trHtml += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else {
                            trHtml += '</table>';
                        }
                    }
                    else {
                        trHtml += '<table width="100%"><tr><td style="width:80%;"><p class="void-order">' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"></td></tr>';

                        if (products[i].ORDER_NOTES != '') {
                            trHtml += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else {
                            trHtml += '</table>';
                        }
                    }
                }

                counter = $($(trHtml).find('.card-body').last()).find('tr').length;
            }
        }
        counter = $($(trHtml).find('.card-body').last()).find('tr').length;
    }

    trHtml += '</div>';
    if (orderNotes != null && orderNotes != undefined && orderNotes != '') {
        trHtml += '<b style="color: white; background: chocolate">Order Notes: </b><div><p class="multi-overflow">' + orderNotes + '</p></div>';
    }

    if (HideMarkCompleteBtn == 0) {

        var pendingProducts = products.filter(row => row.IsReady == false);

        if (pendingProducts.length > 0) {
            trHtml += '<div class="card-footer">' +
                '<button disabled onclick="alert("All Items are not ready");" type="button" id="mybutton" data-Id="' + sale_Invice_ID + '" class="btn btn-primary btn-outline btn-lg btn-block" style="padding-top: 7px; padding-bottom: 7px;">' +
                '<i class="fa fa-check" style="margin-right: 10px;"></i>Mark Completed</button>' +
                        '</div>';
        }
        else {
            trHtml += '<div class="card-footer">' +
                '<button type="button" id="mybutton" data-Id="' + sale_Invice_ID + '" class="btn btn-primary btn-outline btn-lg btn-block" style="padding-top: 7px; padding-bottom: 7px;">' +
                '<i class="fa fa-check" style="margin-right: 10px;"></i>Mark Completed</button>' +
                        '</div>';
        }
    }
}
function LoadItemInfoCallback(sale_Invice_ID, HideMarkCompleteBtn,color) {
    var lstProducts = document.getElementById("hfItem").value;
    var voidCount = 0;
    var newDivCount = 0;
    var counter = 0;
    products = eval(lstProducts);
    trHtmlCallback += '<div class="card-body" style="color:' + color + '">';
    

    products = products.filter(row => row.INVOICE_ID == sale_Invice_ID);

    //if (products[0].ShowSelectAllOnKDS == "1") {
    //    trHtmlCallback += '<input id="select-all" data-id="' + sale_Invice_ID + '" type="checkbox"'
    //    + 'style="margin: 0px 0px 12px -150px;"> <span style="color:forestgreen;font-weight:500">Mark All</span>';
    //}

    for (var i = 0; i < products.length; i++) {
        if (counter % 5 == 0 && counter !== 0 && products[i].ScrollOnKDSOrder == "0") {
            counter = 0;
            trHtmlCallback += '<div class="card-footer"><p>Continued <i class="fa fa-arrow-right"></i></p></div></div></div></div><div class="col-lg-3 column-card" style="display: inline-block; padding-left: 0px;">' +
            '<div class="card">' +
                '<div class="card-header" style="padding: 0px; padding-bottom: .75rem;">' +
                    '<p>Continued <i class="fa fa-arrow-right"></i></p>' +
                '</div>' +
                '<div class="card-body" style="color:' + color + '">';
            newDivCount++;
            console.log("newdiv:" + newDivCount + ", counter: " + counter);
        }

        //First time order
        if (products[i].IsNewAdded == true) {
            if (products[i].VOID == false) {
                var readyHtml = '';
                readyHtml = '<input disabled class="item-ready" data-id="' + products[i].SALE_INVOICE_DETAIL_ID + '" type="checkbox" checked="checked">';
                if (products[i].PreparationTime > 0) {
                    trHtmlCallback += '<table width="100%"><tr>';

                    if (products[i].MODIFIER == 0) {
                        trHtmlCallback += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                        '<span class="checkmark" style="background-color: green;"></span></label></td>' +
                        '<td style="width:75%;"><p>' + products[i].QTY + ' x ' + products[i].SKU_NAME + '</p></td>' +
                        '<td style="width:20%;" align="right"><p style="color:red">' + products[i].PreparationTime + ' Min</p></td></tr>';
                        if (products[i].ORDER_NOTES != '') {
                            trHtmlCallback += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else {
                            trHtmlCallback += '</table>';
                        }
                    }
                    else {
                        trHtmlCallback += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark" style="background-color: green;"></span></label></td>' +
                        '<td style="width:75%;"><p style="color:blue">' + products[i].QTY + ' x ' + products[i].SKU_NAME + '</p></td><td style="width:20%;" align="right"><p style="color:red">' + products[i].PreparationTime + ' Min</p></td></tr>';

                        if (products[i].ORDER_NOTES != '') {
                            trHtmlCallback += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else {
                            trHtmlCallback += '</table>';
                        }
                    }
                }
                else {
                    trHtmlCallback += '<table width="100%"><tr>';
                    if (products[i].MODIFIER == 0) {
                        trHtmlCallback += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                        '<span class="checkmark" style="background-color: green;"></span></label></td>' +
                        '<td style="width:75%;"><p>' + products[i].QTY + ' x ' + products[i].SKU_NAME + '</p></td><td style="width:20%;" align="right"></td></tr>';

                        if (products[i].ORDER_NOTES != '') {
                            trHtmlCallback += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else {
                            trHtmlCallback += '</table>';
                        }
                    }
                    else {
                        trHtmlCallback += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark" style="background-color: green;"></span></label></td>' +
                        '<td style="width:75%;"><p style="color:blue">' + products[i].QTY + ' x ' + products[i].SKU_NAME + '</p></td><td style="width:20%;" align="right"></td></tr>';

                        if (products[i].ORDER_NOTES != '') {
                            trHtmlCallback += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else {
                            trHtmlCallback += '</table>';
                        }
                    }
                }
            }
        }
        if (i == products.length - 1) {
            //KDS History Table Items
            var runningCount = 0;
            for (var j = 0; j < products.length; j++) {
                if (counter % 5 == 0 && counter !== 0 && products[j].ScrollOnKDSOrder == "0") {
                    counter = 0;
                    trHtmlCallback += '<div class="card-footer"><p>Continued <i class="fa fa-arrow-right"></i></p></div></div></div></div><div class="col-lg-3 column-card" style="display: inline-block; padding-left: 0px;">' +
                    '<div class="card">' +
                        '<div class="card-header" style="padding: 0px; padding-bottom: .75rem;">' +
                            '<p>Continued <i class="fa fa-arrow-right"></i></p>' +
                        '</div>' +
                        '<div class="card-body" style="color:' + color + '">';
                    newDivCount++;
                    console.log("newdiv:" + newDivCount + ", counter: " + counter);
                }

                if (products[j].VOID == false) {
                    if (products[j].IsNewAdded == false) {
                        if (runningCount == 0) {
                            trHtmlCallback += '<p class="heading" style="background-color: green;"><i class="fa fa-plus"></i>Running Order - ' + formatMicrosoftDate(products[j].LASTUPDATE_DATE) + ')</p>';
                        }
                        runningCount++;

                        var readyHtml = '';
                        readyHtml = '<input disabled class="item-ready" data-id="' + products[j].SALE_INVOICE_DETAIL_ID + '" type="checkbox" checked="checked">';
                        if (products[j].PreparationTime > 0) {
                            trHtmlCallback += '<table width="100%"><tr>';

                            if (products[j].MODIFIER == 0) {
                                trHtmlCallback += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark" style="background-color: green;"></span></label></td>' +
                                '<td style="width:75%;"><p>' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"><p style="color:red">' + products[j].PreparationTime + ' Min</p></td></tr>';

                                if (products[i].ORDER_NOTES != '') {
                                    trHtmlCallback += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' +products[i].ORDER_NOTES + '</td></tr></table>'
                                }
                                else {
                                    trHtmlCallback += '</table>';
                                }
                            }
                            else {
                                trHtmlCallback += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark" style="background-color: green;"></span></label></td>' +
                                '<td style="width:75%;"><p style="color:blue">' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"><p style="color:red">' + products[j].PreparationTime + ' Min</p></td></tr>';

                                if (products[i].ORDER_NOTES != '') {
                                    trHtmlCallback += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                                }
                                else {
                                    trHtmlCallback += '</table>';
                                }
                            }
                        }
                        else {
                            trHtmlCallback += '<table width="100%"><tr>';
                            if (products[j].MODIFIER == 0) {
                                trHtmlCallback += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark" style="background-color: green;"></span></label></td>' +
                                '<td style="width:75%;"><p>' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"></td></tr>';

                                if (products[i].ORDER_NOTES != '') {
                                    trHtmlCallback += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                                }
                                else {
                                    trHtmlCallback += '</table>';
                                }
                            }
                            else {
                                trHtmlCallback += '<td style="width:5%;"><label class="mycontainer">' + readyHtml +
                                '<span class="checkmark" style="background-color: green;"></span></label></td>' +
                                '<td style="width:75%;"><p style="color:blue">' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"></td></tr>';

                                if (products[i].ORDER_NOTES != '') {
                                    trHtmlCallback += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                                }
                                else {
                                    trHtmlCallback += '</table>';
                                }
                            }
                        }
                    }
                }

                counter = $($(trHtmlCallback).find('.card-body').last()).find('tr').length;
            }
            //VOID Items
            for (var j = 0; j < products.length; j++) {
                if (counter % 5 == 0 && counter !== 0 && products[j].ScrollOnKDSOrder == "0") {
                    counter = 0;
                    trHtmlCallback += '<div class="card-footer"><p>Continued <i class="fa fa-arrow-right"></i></p></div></div></div></div><div class="col-lg-3 column-card" style="display: inline-block; padding-left: 0px;">' +
                    '<div class="card">' +
                        '<div class="card-header" style="padding: 0px; padding-bottom: .75rem;">' +
                            '<p>Continued <i class="fa fa-arrow-right"></i></p>' +
                        '</div>' +
                        '<div class="card-body" style="color:' + color + '">';
                    newDivCount++;
                    console.log("newdiv:" + newDivCount + ", counter: " + counter);
                }

                if (products[j].VOID == true) {
                    if (voidCount == 0) {
                        trHtmlCallback += '<p class="heading"><i class="fa fa-exclamation-circle"></i>Void</p>';
                        voidCount++;
                    }
                    if (products[j].PreparationTime > 0) {
                        trHtmlCallback += '<table width="100%"><tr><td style="width:80%;"><p class="void-order">' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"><p class="void-order" style="color:red">' + products[j].PreparationTime + ' Min</p></td></tr>';

                        if (products[i].ORDER_NOTES != '') {
                            trHtmlCallback += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else {
                            trHtmlCallback += '</table>';
                        }
                    }
                    else {
                        trHtmlCallback += '<table width="100%"><tr><td style="width:80%;"><p class="void-order">' + products[j].QTY + ' x ' + products[j].SKU_NAME + '</p></td><td style="width:20%;" align="right"></td></tr>';

                        if (products[i].ORDER_NOTES != '') {
                            trHtmlCallback += '<tr><td colspan="3" style="font-size:13px;font-style:italic;color:red;text-align:left"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + products[i].ORDER_NOTES + '</td></tr></table>'
                        }
                        else {
                            trHtmlCallback += '</table>';
                        }
                    }
                }

                counter = $($(trHtmlCallback).find('.card-body').last()).find('tr').length;
            }
        }
        counter = $($(trHtmlCallback).find('.card-body').last()).find('tr').length;
    }

    trHtmlCallback += '</div>';
    if (orderNotes != null && orderNotes != undefined && orderNotes != '') {
        trHtmlCallback += '<b style="color: white; background: chocolate">Order Notes: </b><div><p class="multi-overflow">' + orderNotes + '</p></div>';
    }

    var pendingProducts = products.filter(row => row.IsReady == false);

    if (pendingProducts.length > 0) {
        trHtmlCallback += '<div class="card-footer">' +
            '<button disabled onclick="alert("All Items are not ready");" type="button" id="mybutton" data-Id="' + sale_Invice_ID + '" class="btn btn-primary btn-outline btn-lg btn-block" style="padding-top: 7px; padding-bottom: 7px;">' +
            '<i class="fa fa-check" style="margin-right: 10px;"></i>Mark Completed</button>' +
                    '</div>';
    }
    else {
        trHtmlCallback += '<div class="card-footer">' +
            '<button type="button" id="mybuttonCallback" data-Id="' + sale_Invice_ID + '" class="btn btn-primary btn-outline btn-lg btn-block" style="padding-top: 7px; padding-bottom: 7px;">' +
            '<i class="fa fa-check" style="margin-right: 10px;"></i>Recall</button>' +
                    '</div>';
    }
}

$(document).on('click', '#mybutton', function () {
    var invoice_ID = $(this).attr('data-Id');
    MarkOrderASComplete(invoice_ID);
});

$(document).on('click', '#mybuttonCallback', function () {
    var invoice_ID = $(this).attr('data-Id');
    CallbackInvoice(invoice_ID);
});

$(document).on('click', '.item-ready', function () {
    var invoice_Detail_Id = $(this).attr('data-id');
    var isChecked = $(this).prop('checked');
    MarkItemAsReady(invoice_Detail_Id, isChecked);
});


$(document).on('click', '#select-all', function () {
    var isChecked = $(this).prop('checked');

    // Get the closest card-body container to limit the scope
    var $container = $(this).closest('.card-body');

    // Find all .item-ready checkboxes inside that card-body
    $container.find('.item-ready:checkbox').each(function () {
        $(this).prop('checked', isChecked);

        var invoice_Detail_Id = $(this).data('id');
        MarkItemAsReady(invoice_Detail_Id, isChecked);
    });
});


function GetPendingAndCompeletdOrdersCount() {
    $.ajax({
        type: "POST",
        url: "frmKitchenOrderTaking.aspx/GetPendingAndCompeletdOrdersCount",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: DisplayCount
    });
}

function MarkOrderASComplete(sale_Invice_ID) {
    $.ajax({
        type: "POST",
        url: "frmKitchenOrderTaking.aspx/UpdateInvoicePaid",
        contentType: "application/json; charset=utf-8",
        data: "{'InvoiceId':'" + sale_Invice_ID + "'}",
        dataType: "json",
        async: true,
        success: LoadDefault
    });
}

function MarkItemAsReady(invoice_Detail_Id, checked) {
    var params =
{
    "SALE_INVOICE_DETAIL_ID": invoice_Detail_Id,
    "isReady": checked
};
    $.ajax({
        type: "POST",
        url: "frmKitchenOrderTaking.aspx/UpdateItemAsReady",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(params),
        dataType: "json",
        async: true,
        success: function (data) {
        }
    });
}

function CallbackInvoice(sale_Invice_ID) {
    $.ajax({
        type: "POST",
        url: "frmKitchenOrderTaking.aspx/InvoiceCallBack",
        contentType: "application/json; charset=utf-8",
        data: "{'InvoiceId':'" + sale_Invice_ID + "'}",
        dataType: "json",
        async: true,
        success: LoadDefault
    });
}

function DisplayCount(count) {
    count = JSON.parse(count.d);
    $("#completed-orders").html(count[0].CompletedOrders);
    $("#pending-orders").html(count[0].PendingOrders);
}

function btnCallBack_Click()
{
    $('#kds-call-back').lightbox_me({
        centered: true,
        onLoad: function () {

        }
    });
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
function formatMicrosoftDate(msDateString) {
    if (!msDateString) return '';
    const match = /\/Date\((\d+)\)\//.exec(msDateString);
    if (!match) return '';
    const timestamp = parseInt(match[1], 10);
    if (isNaN(timestamp)) return '';
    const date = new Date(timestamp);
    let hours = date.getHours();
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');
    const ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // 0 becomes 12
    const hoursStr = String(hours).padStart(2, '0');
    return `${hoursStr}:${minutes}:${seconds} ${ampm}`;
}