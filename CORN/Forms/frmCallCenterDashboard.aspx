<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false"
    CodeFile="frmCallCenterDashboard.aspx.cs"
    Inherits="Forms.frmCallCenterDashboard"
    EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!doctype html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" charset="utf-8" />
    <meta name="robots" content="noindex, nofollow" />
    <title>CORN :: Call Center</title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/zebra_dialog.css" rel="stylesheet" type="text/css" />
    <link href="../css/tree-style.css" rel="stylesheet" type="text/css" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="icon" type="image/x-icon" href="../images/icon.ico">
    <link href="../css/calender.css" rel="stylesheet" type="text/css" />
    <link href="../css/CustomKeyBoard.css" rel="stylesheet" type="text/css" />
      <style>
    .square-box {
        width: 200px;
        height: 150px;
        border: 1px solid #ccc;
        display: inline-block;
    }
</style>
</head>

<body>

    <form id="Form1" method="POST" runat="server">
       <div class="row" style="margin-top: 10px;">
    <div class="col-6 col-md-2">
        <div class="square-box">
            <div class="row" style="margin-left: 1px;width:99.5%">
                <div class="col-md-12" style="background-color:orange">
                    <h4 style="color:white;text-align:center"> Barkat Market </h4>
                </div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 10px;">
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Call Center</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Food Panda</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-12" style="background-color:lightseagreen"><span style="color:white">TakeAway</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">In Progress:</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white;float: right;">0</span></div>
            </div>
        </div>
    </div>
    <div class="col-6 col-md-2">
        <div class="square-box">
            <div class="row" style="margin-left: 1px;width:99.5%">
                <div class="col-md-12" style="background-color:orange">
                    <h4 style="color:white;text-align:center"> Iqbal Town </h4>
                </div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 10px;">
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Call Center</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Food Panda</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-12" style="background-color:lightseagreen"><span style="color:white">TakeAway</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">In Progress:</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white;float: right;">0</span></div>
            </div>
        </div>
    </div>
    <div class="col-6 col-md-2">
        <div class="square-box">
            <div class="row" style="margin-left: 1px;width:99.5%">
                <div class="col-md-12" style="background-color:orange">
                    <h4 style="color:white;text-align:center"> DHA </h4>
                </div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 10px;">
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Call Center</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Food Panda</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-12" style="background-color:lightseagreen"><span style="color:white">TakeAway</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">In Progress:</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white;float: right;">0</span></div>
            </div>
        </div>
    </div>
    <div class="col-6 col-md-2">
        <div class="square-box">
            <div class="row" style="margin-left: 1px;width:99.5%">
                <div class="col-md-12" style="background-color:orange">
                    <h4 style="color:white;text-align:center"> Main Market </h4>
                </div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 10px;">
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Call Center</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Food Panda</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-12" style="background-color:lightseagreen"><span style="color:white">TakeAway</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">In Progress:</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white;float: right;">0</span></div>
            </div>
        </div>
    </div>
    <div class="col-6 col-md-2">
        <div class="square-box">
            <div class="row" style="margin-left: 1px;width:99.5%">
                <div class="col-md-12" style="background-color:orange">
                    <h4 style="color:white;text-align:center"> Mall Road </h4>
                </div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 10px;">
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Call Center</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Food Panda</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-12" style="background-color:lightseagreen"><span style="color:white">TakeAway</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">In Progress:</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white;float: right;">0</span></div>
            </div>
        </div>
    </div>
    <div class="col-6 col-md-2">
        <div class="square-box">
            <div class="row" style="margin-left: 1px;width:99.5%">
                <div class="col-md-12" style="background-color:orange">
                    <h4 style="color:white;text-align:center"> Johar Town </h4>
                </div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 10px;">
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Call Center</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">Food Panda</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-12" style="background-color:lightseagreen"><span style="color:white">TakeAway</span></div>
            </div>
            <div class="row" style="margin-left: 1px;width:99.5%;margin-top: 5px;">
                 <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white">In Progress:</span></div>
                <div class="col-md-6" style="background-color:lightseagreen"><span style="color:white;float: right;">0</span></div>
            </div>
        </div>
    </div>
</div>
       
    </form>
</body>
</html>
