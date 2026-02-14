$(document).ready(function () {
    GetStockDemandNotification();
});
function GetStockDemandNotification()
{
    
    if ($("[id$='lblTransferOutForm']").text() != '') {
        GetStockDemadData();        
    }
    else
    {
        var notification = '<a href="#" class="dropdown-toggle" data-toggle="dropdown">';
        notification += '<i class="fa fa-bell"></i>';
        notification += '<span class="label label-danger"></span>';
        notification += '</a>';
        $("#dvNotification").html(notification);
    }
}
function GetStockDemadData(saleInvoiceMasterId) {

    $.ajax
        (
            {
                type: "POST",
                url: "frmOrderPOS.aspx/GetStockDemandData",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: LoadStockDemandData
            }
        );
}
function LoadStockDemandData(products) {
    products = JSON.stringify(products);
    var result = jQuery.parseJSON(products.replace(/&quot;/g, '"'));
    products = eval(result.d);
    if (products.length > 0) {
        var notification = '<a href="#" class="dropdown-toggle" data-toggle="dropdown">';
        notification += '<i class="fa fa-bell"></i>';
        notification += '<span class="label label-danger">' + products.length  + '</span>';
        notification += '</a>';
        notification += '<ul class="dropdown-menu">';
        notification += '<li class="header">You have ' + products.length + ' tasks</li>';
        notification += '<li>';
        notification += '<ul class="menu">';
        for (var i = 0, len = products.length; i < len; ++i) {
            notification += '<li>';
            notification += '<a href="' + $("[id$='lblTransferOutForm']").text() + '?LevelType=3&LevelID=30">';
            notification += '<h3>';
            notification += 'Stock Demand Received from ' + products[i].DISTRIBUTOR_NAME;
            notification += '</h3>';
            notification += '<div class="progress xs">';
            notification += '<div class="progress-bar progress-bar-aqua" style="width: 100%" role="progressbar" aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">';
            notification += '<span class="sr-only"></span>';
            notification += '</div>';
            notification += '</div>';
            notification += '</a>';
            notification += '</li>';

        }
        notification += '</ul>';
        notification += '</li>';
        notification += '<li class="footer">';
        notification += '<a href="#">View all tasks</a>';
        notification += '</li>';
        notification += '</ul>';
        $("#dvNotification").html(notification);
    }
    else {
        var notification = '<a href="#" class="dropdown-toggle" data-toggle="dropdown">';
        notification += '<i class="fa fa-bell"></i>';
        notification += '<span class="label label-danger"></span>';
        notification += '</a>';
        $("#dvNotification").html(notification);
    }
}