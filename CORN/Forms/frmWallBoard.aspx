<%@ Page Title="CORN :: Wall Board" Language="C#" AutoEventWireup="true" CodeFile="frmWallBoard.aspx.cs" Inherits="Forms_frmWallBoard" %>

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
<body>
    <form id="Form1" method="POST" runat="server">
        <div class="row main-top">
            <div class="col-md-12" style="white-space: nowrap; width: 100%;">
                <div class="col-md-3 col-lg-4" style="display: inline-block;">
                    <!-- <a data-toggle="collapse" href="#topbar" role="button" aria-expanded="false" aria-controls="topbar"><i class="fa fa-arrow-down"></i></a> -->
                </div>
                <div class="col-md-3 col-lg-2 mb-3 mx-auto" style="display: inline-block; padding-left: 0px;">
                    <div class="user-detail" style="margin-left: 120px; color: white; font-size: 18px;">
                        <asp:Literal ID="lblDateTime" runat="server" Text="Label"></asp:Literal>
                        <div style="display: inline-block; margin-left: 20px;">
                            <asp:LinkButton runat="server" ID="lnkExit" OnClick="lnkExit_Click" ForeColor="White"><i class="fa fa-home"></i> Home</asp:LinkButton>                            
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div class="row main-body">
            <span id='ct' style="display: none;"></span>            
            <div class="row" style="width:100%;">
                <div class="col-md-12" id="lowstock-table" style="white-space: nowrap;">
            </div>
            </div>
        </div>
    </form>
</body>
</html>

<script>
    $(document).ready(function () {
        LoadDefault();
        updateDate();
    });

    function LoadDefault() {
        trHtml = '';
        setTimeout(function () {
            GetLowStock();            
        }, 510);

        setTimeout(function () {
            LoadDefault();
        }, 15000);
    }
    function GetLowStock() {        
        $.ajax({
            type: "POST", //HTTP method
            url: "frmWallBoard.aspx/GetLowStock",//page/method name
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: DecorateItems
        });
    }
    var trHtml = '';
    function DecorateItems(Items) {
        Items = JSON.parse(Items.d);        
        var SNo = 0;
        trHtml = '<table style="width:100%;" rowspan="5" >';
        trHtml += '<th style="width:5%;"></th>';
        trHtml += '<th style="width:5%;">S.No</th>';
        trHtml += '<th style="width:25%;">Item </th>';
        trHtml += '<th style="width:25%;">Section</th>';
        trHtml += '<th style="width:20%;">Low Level</th>';
        trHtml += '<th style="width:20%;">Closing Stock</th>';
        $.each(Items, function (index, value) {
            var stock = parseFloat(value.MIN_LEVEL) - parseFloat(value.CurrentStock);
            SNo = parseInt(SNo) + 1;
            if (stock < 1) {
                trHtml += '<tr style="background-color:rgb(134, 187, 211);color:white;">'
            }
            else {
                trHtml += '<tr style="background-color:rgb(218, 146, 123);color:white;">'
            }
            trHtml += '<td style="width:5%;">';
            trHtml += '</td>'
            trHtml += '<td style="width:5%;">' + SNo;
            trHtml += '</td>'
            trHtml += '<td style="width:25%;">' + value.SKU_NAME;
            trHtml += '</td>'
            trHtml += '<td style="width:25%;">' + value.SECTION_NAME;
            trHtml += '</td>'
            trHtml += '<td style="width:20%;">' + value.MIN_LEVEL;
            trHtml += '</td>'
            trHtml += '<td style="width:20%;">' + value.CurrentStock;
            trHtml += '</td>'
            trHtml += '</tr>'
        });
        trHtml += '</table>';
        $("#lowstock-table").html(trHtml);
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