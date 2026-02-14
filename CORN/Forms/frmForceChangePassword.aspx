<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="frmForceChangePassword.aspx.cs" Inherits="Forms_frmForceChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
    <head runat="server">
        <title> CORN :: Change Password </title>
        <meta name="robots" content="noindex, nofollow" />
         <link href="OrderPOS/css/bootstrap.min.css" rel="stylesheet" type="text/css" />

                <script src="AjaxLibrary/jquery-1.10.2.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js" type="text/javascript"></script>
    <script src="AjaxLibrary/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }
    </script>

            <script type="text/javascript">
        function pageLoad() {
            addBubbleMouseovers("CurrentPassword");
            addBubbleMouseovers("NewPassword");
            addBubbleMouseovers("ConfirmPassword");
        }

        //--indicates the mouse is currently over a div
        var onDiv = false;
        //--indicates the mouse is currently over a link
        var onLink = false;
        //--indicates that the bubble currently exists
        var bubbleExists = false;
        //--this is the ID of the timeout that will close the window if the user mouseouts the link
        var timeoutID;
        var ClassName;
        var DivText;

        function addBubbleMouseovers(mouseoverClass) {
            $("." + mouseoverClass).mouseover(function (event) {
                if (onDiv || onLink) {
                    return false;
                }
                ClassName = mouseoverClass;
                onLink = true;
                showBubble.call(this, event);
            });

            $("." + mouseoverClass).mouseout(function () {
                onLink = false;
                timeoutID = setTimeout(hideBubble, 150);
            });
        }

        function hideBubble() {
            clearTimeout(timeoutID);
            //--if the mouse isn't on the div then hide the bubble
            if (bubbleExists && !onDiv) {
                $("#bubbleID").remove();
                bubbleExists = false;
            }
        }

        function showBubble(event) {
            if (bubbleExists) {
                hideBubble();
            }
            if (ClassName == 'CurrentPassword') {
                DivText = '<u><b>Current Password</b></u><br/>'
            + 'The password you use for login to CORN';
            }
            else if (ClassName == 'NewPassword') {
                DivText = '<u><b>New Password</b></u><br/>'
            + 'Max length:50<br>';
            }
            else if (ClassName == 'ConfirmPassword') {
                DivText = '<u><b>Confirm Password</b></u><br/>'
           + 'Same as new password';
            }

            var tPosX = event.pageX - 175;
            var tPosY = event.pageY + 2;
            $('<div ID="bubbleID"'
        + 'style="top:' + tPosY + 'px;'
        + 'left:' + tPosX + 'px;'
        + 'position: absolute;'
        + 'display: inline;'
        + 'border: 2px;'
        + 'width: 170px;'
        + 'height: 65px;'
        + 'padding:5px;'
        + 'color:White;'
        + 'background-color: #007395;">'
        + DivText
        + '</div>').mouseover(keepBubbleOpen).mouseout(letBubbleClose).appendTo('body');

            bubbleExists = true;
        }

        function keepBubbleOpen() {
            onDiv = true;
        }

        function letBubbleClose() {
            onDiv = false;

            hideBubble();
        }

        function ValidatePassword() {

            var strCurrentPassword;
            var strNewPassword;
            var strChangePassword;

            strCurrentPassword = document.getElementById('<%=txtCurrentPassword.ClientID%>').value;
            if (strCurrentPassword == null || strCurrentPassword.length == 0) {
                alert('Please enter current password ');
                document.getElementById('<%=txtCurrentPassword.ClientID%>').focus();
                return false;
            }
            strNewPassword = document.getElementById('<%=txtNewPassword.ClientID%>').value;
            if (strNewPassword == null || strNewPassword.length == 0) {
                alert('Please enter new password ');
                document.getElementById('<%=txtNewPassword.ClientID%>').focus();
                return false;
            }
            strChangePassword = document.getElementById('<%=txtConfirmNewPassword.ClientID%>').value;
            if (strChangePassword == null || strChangePassword.length == 0) {
                alert('Please enter confirm new password ');
                document.getElementById('<%=txtConfirmNewPassword.ClientID%>').focus();
                return false;
            }
            if (strNewPassword == strChangePassword) {
                return true;
            }
            else {
                alert('New Password does not match');
                return false;
            }

            return true;
        } 

    </script>

        <style>
            body{
                height: 100vh;
                min-height: 100vh;
                background: #edf2f3;
            }

            form{
                display: flex;
                justify-content: center;
                align-content: center;
                padding: 15vh;
            }

            .d-inline{
                display: block;
            }

            .row{
                padding-bottom: 25px;
            }
            .form-control{
                width: 350px;
    padding: 10px;
    border-radius: 8px;
    line-height: 14px;
    font-size: 12px;
    margin-top: 8px;
            }

            .btn-success{
                width: 150px;
    height: 27px;
    background: #31de31;
    border: 2px solid #73c673;
    border-radius: 4px;
            }

            .btn-danger{
                background: red;
    border: #df3c3c;
     width: 150px;
    height: 27px;
    border-radius: 4px;
            }

            .btn{
                cursor: pointer;
            }

           .btn:hover{
                color: white;
            }
        </style>
        </head>

    <body>
    <div class="main-contents">
            <form runat="server">
                 <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container employee-infomation">
                        <div class="row">
                            <div class="col-md-12">
                            </div>
                            
                             <div class="col-md-12">
                                 <img src="../images/logo.png" class="img-responsive" alt="">
                                <h2>For better data security, you are requested to change your password now!</h2>
                            </div>
                             <div class="col-md-12">
                                <asp:Label runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                 <div class="col-md-2 d-inline">
                                <label class="lbl-ps">Current Password</label>
                                     </div>
                                 <div class="col-md-4 d-inline">
                                <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-control" TextMode="Password" TabIndex="1"></asp:TextBox>
                                <span class="fa fa-caret-right rgt_cart"></span>
                                     
                                <a class="CurrentPassword" href="javascript:void(0)">
                                    <img height="15" alt="" src="../App_Themes/Granite/Images/help.jpg" /></a>
                                         
                            </div>
                        </div>
                            </div>
                        <div class="row">
                            <div class="col-md-12">
                                 <div class="col-md-2 d-inline">
                                <label id="lbltoLocation" class="lbl-ps">New Password</label>
                                     </div>
                                 <div class="col-md-4 d-inline">
                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" TabIndex="2"></asp:TextBox>
                                <span class="fa fa-caret-right rgt_cart"></span>
                                <a class="CurrentPassword" href="javascript:void(0)">
                                    <img height="15" alt="" src="../App_Themes/Granite/Images/help.jpg" /></a>
                            </div>
                            </div>
                          
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                 <div class="col-md-2 d-inline">
                                <label class="lbl-ps">Confirm New Password</label>
                                     </div>
                                 <div class="col-md-4 d-inline">
                                <asp:TextBox ID="txtConfirmNewPassword" runat="server" CssClass="form-control" TextMode="Password" TabIndex="2"></asp:TextBox>
                                <span class="fa fa-caret-right rgt_cart"></span>
                                     <a class="ConfirmPassword" href="javascript:void(0)">
                                    <img height="15" alt="" src="../App_Themes/Granite/Images/help.jpg" /></a>
                                     </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-offset-1 col-md-3 ">
                                <div class="btnlist">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn btn-success" TabIndex="3" />
                                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel" CssClass="btn btn-danger" TabIndex="4" />
                                </div>
                            </div>
                        </div>
            <asp:TextBox ID="hiddenPassword" runat="server" Visible="False" Width="97px"></asp:TextBox>
        </div>
            </form>

    </div>
        </body>
</html>
