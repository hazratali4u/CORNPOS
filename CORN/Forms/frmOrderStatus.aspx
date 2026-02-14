<%@ Page Title="CORN :: Order Status" Language="C#" AutoEventWireup="true" CodeFile="frmOrderStatus.aspx.cs" Inherits="Forms_frmOrderStatus" %>

<!doctype html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" charset="utf-8" />
    <meta name="robots" content="noindex, nofollow" />
    <title>CORN :: Kitchen Order Taking</title>
    <link rel="icon" type="image/x-icon" href="../images/icon.ico">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.1.3/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.0/css/all.css" />
    <link href="../css/KitchenDisplayTerminal.css" rel="stylesheet" />
    <script src="../AjaxLibrary/jquery-1.7.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>
</head>
<body style="background-color: white;">
    <form id="Form1" method="POST" runat="server">
        
        <div class="row main-body" style="text-align:center;">
            <span id='ct' style="display: none;"></span>
                <div class="col-md-4" style="font-size:40px;">Now Serving</div>
                <div class="col-md-4" style="font-size:40px;">Now Preparing</div>
                <div class="col-md-4"></div>
            <div class="col-md-4" id="order-serving" style="white-space: nowrap;">
            </div>
            <div class="col-md-4" id="order-preparing" style="white-space: nowrap;">
            </div>
            <div class="col-md-4"  style="white-space: nowrap;">
            </div>
        </div>
    </form>
</body>
</html>

<script>
    $(document).ready(function () {
        LoadDefault();
        LoadDefault2();
        updateDate();
    });

    function LoadDefault() {
        trHtml = '';
        setTimeout(function () {
            GetPendingOrders();            
        }, 510);

        setTimeout(function () {
            LoadDefault();
        }, 15000);
    }
    function LoadDefault2() {
        trHtmlcom = '';
        setTimeout(function () {
            GetCompleteOrders();
        }, 510);

        setTimeout(function () {
            LoadDefault2();
        }, 15000);
    }
    function GetPendingOrders() {
        $.ajax({
            type: "POST", //HTTP method
            url: "frmOrderStatus.aspx/SelectPendingBills",//page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: DecorateItems
        });
    }
    function GetCompleteOrders() {
        $.ajax({
            type: "POST", //HTTP method
            url: "frmOrderStatus.aspx/SelectBillsComplete",//page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: DecorateItemsComplete
        });
    }
    var trHtml = '';
    function DecorateItems(Items) {
        Items = JSON.parse(Items.d);
        $.each(Items, function (index, value) {            
                trHtml += '<div class="rows"> <div class="col-lg-3" style="display: inline-block;">' +
                   '<div style="margin-bottom:-8%">';
                trHtml += '<div style="font-weight:bold;font-size:35px;">' + value.ORDER_NO;
                trHtml += '</div>';
                trHtml += '</div>' +
        '</div></div>';
        });
        $("#order-preparing").html(trHtml);
    }
    var trHtmlcom = '';
    $("#order-serving").html(trHtmlcom);
    var sectioncount = 0;
    function DecorateItemsComplete(Items) {
        Items = JSON.parse(Items.d);
        if (sectioncount < Items.length) {
            var audio = new Audio("../images/beep.mp3");
            audio.play();
        }
        sectioncount = Items.length;
        $.each(Items, function (index, value) {
            trHtmlcom += '<div class="rows"> <div class="col-lg-3" style="display: inline-block;">';
            trHtmlcom += '<div style="margin-bottom:-8%">';
            trHtmlcom += '<div style="font-weight:bold;font-size:35px;">';
            trHtmlcom += '<table width="100%"><tr><td style="width:40%">' + value.ORDER_NO + '</td><td style="width:30%">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td style="width:30%"><a href="#" onclick="RemoveOrder(' + value.INVOICE_ID + ')"><span class="fa fa-times"></span></a></td></tr></table>'
            //trHtmlcom += '<a href="#" onclick="RemoveOrder(' + value.INVOICE_ID + ')"><span class="fa fa-times"></span></a> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
            //trHtmlcom += value.ORDER_NO;            
            trHtmlcom += '</div>';
            trHtmlcom += '</div></div></div>';
        });
        $("#order-serving").html(trHtmlcom);
    }
    function RemoveOrder(InvoiceiD)
    {
        $.ajax({
            type: "POST", //HTTP method
            url: "frmOrderStatus.aspx/UpdateTakeawayOrderStatus",//page/method name
            contentType: "application/json; charset=utf-8",
            data: "{'InvoiceId':'" + InvoiceiD + "'}",
            dataType: "json",
            async: false,
            success: OrderRemoved
        });
    }
    function OrderRemoved()
    {

    }
    function BillTime() {
        var refresh = 500; // Refresh rate in milli seconds
        mytime = setTimeout('display_ct()', refresh)
    }
    function display_ct() {
        var strcount
        var d = new Date();
        var h = addZero(d.getHours());
        var m = addZero(d.getMinutes());
        document.getElementById('ct').innerHTML = h + ":" + m;
        tt = BillTime();
    }
    function addZero(i) {//Used For Adding 0 Before Hour When Hour is Less than 10
        if (i < 10) {
            i = "0" + i;
        }
        return i;
    }
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
    function updateDate() {

        setTimeout(function () {
            updateDate();
        }, 60000);

        $.each($(".card-header"), function (index, val) {
            // debugger;
            var timeSpan = $(this).find('span').html();

            if (timeSpan != null && timeSpan != undefined) {
                var result = getDateDiff(new Date(timeSpan));
                $(this).find('strong').html(result);
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
</script>