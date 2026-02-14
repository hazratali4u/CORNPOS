<%@ Page Title="CORN :: Kitchen Display Terminal" Language="C#" AutoEventWireup="true"
    CodeFile="frmKitchenOrderTakingQA.aspx.cs" Inherits="Forms_frmKitchenOrderTakingQA" %>
<!doctype html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" charset="utf-8" />
    <meta name="robots" content="noindex, nofollow" />
    <title>CORN :: Kitchen Order Taking</title>
    <link rel="icon" type="image/x-icon" href="../images/icon.ico">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.3.1/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.0/css/all.css" />
    <link href="../css/KitchenDisplayTerminal.css" rel="stylesheet" />
    <%--<script src="../AjaxLibrary/jquery-1.7.1.min.js"></script>--%>
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="../js/jquery-2.0.2.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../AjaxLibrary/jquery.lightbox_me.js"></script>
    <script type="text/javascript" src="../AjaxLibrary/KDSQA20250713.js"></script>
</head>
<body>
    <form id="Form1" method="POST" runat="server">
        <asp:HiddenField ID="hfItem" runat="server" Value="" />
        <div class="row main-body" style="background-color: #6e909b;margin-top:0px;margin-right:0px;">
            <div class="col-md-12">
                <div class="col-md-3 col-lg-2 mb-3 mr-auto" style="display: inline-block; padding-left: 15px;">
                    <asp:DropDownList Width="82%" style="text-align: center;padding: 2px 10px 2px 2px;" runat="server" ID="ddlCustomerType" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomerType_SelectedIndexChanged">
                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Delivery" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Takeaway" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Dine In" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3 col-lg-2 mb-3 mr-auto" style="display: inline-block; padding-left: 0px;">
                    <button type="button" class="btn btn-white w-100 align-items-stretch d-flex">
                        <div class="icon icon-left icon-secondary d-flex align-items-center justify-content-center">
                            <h3 style="text-align: center; margin-bottom: 0px; color: white; font-weight: bold;" id="pending-orders">0</h3>
                        </div>
                        <div class="text text-right py-2 pr-3">
                            <span style="text-align: center; font-size: 14px">Open Orders</span>
                        </div>
                    </button>
                </div>
                <div class="col-md-3 col-lg-2 mb-3 mx-auto" style="display: inline-block; padding-left: 0px;">
                    <button type="button" class="btn btn-white w-100 align-items-stretch d-flex">
                        <div class="icon icon-left icon-secondary d-flex align-items-center justify-content-center">
                            <h3 style="text-align: center; margin-bottom: 0px; color: white; font-weight: bold;" id="completed-orders">0</h3>
                        </div>
                        <div class="text text-right py-2 pr-3">

                            <span style="text-align: center; font-size: 14px">Completed</span>
                        </div>
                    </button>
                </div>
                <div class="col-md-4" style="display: inline-block; padding-left: 0px;">
                    <div class="user-detail" style="color: white; font-size: 18px;">
                        <asp:Literal ID="lblDateTime" runat="server" Text="Label"></asp:Literal>
                        <div style="display: inline-block; margin-left: 20px;">
                            <asp:LinkButton runat="server" ID="lnkExit" OnClick="lnkExit_OnClick" ForeColor="White"><i class="fa fa-power-off"></i> Sign out</asp:LinkButton>
                        </div>
                        <div style="display: inline-block; margin-left: 20px;">
                            <button onclick="btnCallBack_Click();" type="button" id="btnCallBack" style="padding-top: 7px; padding-bottom: 7px; background-color: #6e909b; color: white; border: none; cursor: pointer;">
                                <i class="fa fa-fast-backward"></i>Recall</button>
                        </div>
                    </div>

                </div>             
            </div>
        </div>
        <div class="row main-body">
            <span id='ct' style="display: none;"></span>
            <div class="col-md-12" id="order-table">
            </div>
        </div>

    <%-- <div class="toast" data-autohide="false" style="position: absolute; top: 0; right: 0;">
    <div class="toast-header">
      <strong class="mr-auto text-primary">Message</strong>
      <small class="text-muted"></small>
      <button type="button" class="ml-2 mb-1 close" data-dismiss="toast">&times;</button>
    </div>
    <div class="toast-body">
      Some text inside the toast body
    </div>
  </div>--%>

        <div id="toast-container" style="position: absolute; top: 0; right: 0;"></div>

    </form>
    <div id="kds-call-back" style="width: 90%; height: 530px; padding: 10px; display: none; background-color: #d6dbe7;overflow: auto; ">
        <div class="col-md-12" id="order-table-callback">
        </div>
    </div>
</body>
</html>