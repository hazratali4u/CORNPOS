<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmCustomerFeedback.aspx.cs" Inherits="Forms_frmCustomerFeedback" Title="CORN :: Customer Feedback" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content1" runat="server" ContentPlaceHolderID="cphPage">
    <script type="text/javascript" src="../AjaxLibrary/jquery-1.7.1.min.js"></script>
    <script language="JavaScript" type="text/javascript">
        function ShowPopup() {
            $('#myModal').modal('show');
        }

        function ValidateOtherMedium() {
            var chkBoxP = document.getElementById('<%= Hear6.ClientID %>');
            if (chkBoxP.checked == true) {
                Error("Hello");
                document.getElementById('<%= txtOther.ClientID %>').disabled = false;
                document.getElementById('<%= txtOther.ClientID %>').readOnly = false;
                document.getElementById('<%= txtOther.ClientID %>').value = "";
            }
            else {
                document.getElementById('<%= txtOther.ClientID %>').disabled = true;
                document.getElementById('<%= txtOther.ClientID %>').readOnly = true;
                document.getElementById('<%= txtOther.ClientID %>').value = "";
            }
        }
        var previousIdService;
        var previousIdFood;
        var previousIdEnvironment;
        var previousIdHear;
        var previousIdReturn;
        var previousIdVisit;

        function toggle(chkBox, ListName) {
            var chkHear = document.getElementById('<%= Hear6.ClientID %>');
            if (ListName == 'Service') {
                if (chkBox.checked) {
                    if (previousIdService) {
                        document.getElementById(previousIdService).checked = false;
                    }
                    previousIdService = chkBox.getAttribute('id');
                }
            }
            if (ListName == 'Food') {
                if (chkBox.checked) {
                    if (previousIdFood) {
                        document.getElementById(previousIdFood).checked = false;
                    }
                    previousIdFood = chkBox.getAttribute('id');
                }
            }
            if (ListName == 'Environment') {
                if (chkBox.checked) {
                    if (previousIdEnvironment) {
                        document.getElementById(previousIdEnvironment).checked = false;
                    }
                    previousIdEnvironment = chkBox.getAttribute('id');
                }
            }
            if (ListName == 'Overall') {
                if (chkBox.checked) {
                    if (previousIdOverall) {
                        document.getElementById(previousIdOverall).checked = false;
                    }
                    previousIdOverall = chkBox.getAttribute('id');
                }
            }
            if (ListName == 'Hear') {
                if (chkBox.checked) {
                    if (previousIdHear) {
                        document.getElementById(previousIdHear).checked = false;
                    }
                    previousIdHear = chkBox.getAttribute('id');
                }
                if (chkBox == chkHear) {
                    ValidateOtherMedium();
                }
                else {
                    document.getElementById('<%= txtOther.ClientID %>').disabled = true;
                    document.getElementById('<%= txtOther.ClientID %>').readOnly = true;
                    document.getElementById('<%= txtOther.ClientID %>').value = "";
                }
            }
            if (ListName == 'Return') {
                if (chkBox.checked) {
                    if (previousIdReturn) {
                        document.getElementById(previousIdReturn).checked = false;
                    }
                    previousIdReturn = chkBox.getAttribute('id');
                }
            }
            if (ListName == 'Visit') {
                if (chkBox.checked) {
                    if (previousIdVisit) {
                        document.getElementById(previousIdVisit).checked = false;
                    }
                    previousIdVisit = chkBox.getAttribute('id');
                }
            }
        }

        $(document).ready(function () {
            $.ajax({
                type: "POST", //HTTP method
                url: "frmCustomerFeedback.aspx/GetDistributorInfo", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: SetDistributorPic
            });

            function SetDistributorPic(data) {
                data = JSON.parse(data.d);

                var url = window.location.origin + '/Pics/';
                var picPath = url + data[0].PIC;
                $("#pic").attr('src', picPath);
            }

            document.querySelectorAll('.starRating').forEach(function (ratingContainer) {
                const stars = ratingContainer.querySelectorAll('.star');

                stars.forEach(function (star) {
                    star.addEventListener('mouseover', function () {
                        let value = parseInt(this.dataset.value);
                        highlightStars(ratingContainer, value);
                    });

                    star.addEventListener('mouseout', function () {
                        clearHighlights(ratingContainer);
                        restoreSelected(ratingContainer);
                    });

                    star.addEventListener('click', function () {
                        let value = parseInt(this.dataset.value);
                        selectStars(ratingContainer, value);
                    });
                });
            });
        });
        function highlightStars(container, value) {
            container.querySelectorAll('.star').forEach(function (star) {
                star.classList.toggle('hover', parseInt(star.dataset.value) <= value);
            });
        }

        function clearHighlights(container) {
            container.querySelectorAll('.star').forEach(function (star) {
                star.classList.remove('hover');
            });
        }

        function selectStars(container, value) {
            container.querySelectorAll('.star').forEach(function (star) {
                star.classList.toggle('selected', parseInt(star.dataset.value) <= value);
            });
        }

        function restoreSelected(container) {
            const selectedStars = container.querySelectorAll('.star.selected');
            const value = selectedStars.length > 0
                ? parseInt(selectedStars[selectedStars.length - 1].dataset.value)
                : 0;
            selectStars(container, value);
            var rating = value;
            const itemId = container.dataset.itemid;
            if (itemId == "101") {
                document.getElementById('<%=hfServiceRating.ClientID%>').value = rating;
            }
            else if (itemId == "102") {
                document.getElementById('<%=hfFoodRating.ClientID%>').value = rating;
            }
            else if (itemId == "103") {
                document.getElementById('<%=hfEnvironmentRating.ClientID%>').value = rating;
            }
            else if (itemId == "104") {
                document.getElementById('<%=hfOverallRating.ClientID%>').value = rating;
            }
}


    </script>
    <style type="text/css">
        .ListOrientation {
            text-orientation: sideways-left;
        }

            .ListOrientation input[type=checkbox] {
                margin-left: 10px;
            }

        .star {
            font-size: 30px;
            cursor: pointer;
            color: gray;
        }

            .star:hover,
            .star.selected {
                color: gold;
            }
    </style>
    <div class="main-contents">
        <div class="container employee-infomation">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:HiddenField ID="hfServiceRating" Value="0" runat="server" />
                    <asp:HiddenField ID="hfFoodRating" Value="0" runat="server" />
                    <asp:HiddenField ID="hfEnvironmentRating" Value="0" runat="server" />
                    <asp:HiddenField ID="hfOverallRating" Value="0" runat="server" />
                    <asp:Panel ID="Panel3" runat="server" BackColor="White" DefaultButton="btnSave">
                        <div class="row">
                            <div class="col-md-7" style="text-align: center;">
                                <label for="ctl00_ctl00_mainCopy_cphPage_fuPic" id="lbl"
                                    runat="server" style="cursor: pointer;">
                                    <img src="../images/watch.png" id="pic" name="pic" width="100" height="100" />
                                </label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-7" style="text-align: center;">
                                <asp:Label ID="lblTitle" Text="To our guest: We’ve been waiting for you! Please take a moment to complete this feedback
form. We value your opinion and thank you for helping us monitor the quality of our products
and service."
                                    runat="server" Width="600px" ForeColor="#A9473E" Font-Size="Large" Font-Bold="True"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="modal fade" id="myModal" role="dialog">
                                <div class="modal-dialog modal-sm">
                                    <!-- Modal content-->
                                    <div class="modal-content" style="border-radius: 73px;">
                                        <div class="modal-body" style="text-align: center; color: darkslateblue">
                                            <h1><i>Thank You !!!</i></h1>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-2">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Service</label>
                            </div>
                            <div class="col-md-4">
                                <div class="starRating" data-itemid="101">
                                    <span class="star" data-value="1">&#9733;</span>
                                    <span class="star" data-value="2">&#9733;</span>
                                    <span class="star" data-value="3">&#9733;</span>
                                    <span class="star" data-value="4">&#9733;</span>
                                    <span class="star" data-value="5">&#9733;</span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-2">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Food</label>
                            </div>
                            <div class="col-md-4">
                                <div class="starRating" data-itemid="102">
                                    <span class="star" data-value="1">&#9733;</span>
                                    <span class="star" data-value="2">&#9733;</span>
                                    <span class="star" data-value="3">&#9733;</span>
                                    <span class="star" data-value="4">&#9733;</span>
                                    <span class="star" data-value="5">&#9733;</span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-2">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Environment</label>
                            </div>
                            <div class="col-md-4">
                                <div class="starRating" data-itemid="103">
                                    <span class="star" data-value="1">&#9733;</span>
                                    <span class="star" data-value="2">&#9733;</span>
                                    <span class="star" data-value="3">&#9733;</span>
                                    <span class="star" data-value="4">&#9733;</span>
                                    <span class="star" data-value="5">&#9733;</span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-2">
                                <label><span class="fa fa-caret-right rgt_cart"></span>Overall Experience</label>
                            </div>
                            <div class="col-md-4">
                                <div class="starRating" data-itemid="104">
                                    <span class="star" data-value="1">&#9733;</span>
                                    <span class="star" data-value="2">&#9733;</span>
                                    <span class="star" data-value="3">&#9733;</span>
                                    <span class="star" data-value="4">&#9733;</span>
                                    <span class="star" data-value="5">&#9733;</span>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-7">
                                <label><span class="fa fa-caret-right rgt_cart"></span>We welcome your comments or any suggestions:</label>
                                <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-7">
                                <label id="lbltoLocation"><span class="fa fa-caret-right rgt_cart"></span>How did you hear about us?</label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-3">
                                <asp:CheckBox runat="server" ID="Hear1" Text="Facebook" onClick="toggle(this,'Hear');" />
                            </div>
                            <div class="col-md-3">
                                <asp:CheckBox runat="server" ID="Hear2" Text="Google Maps" onClick="toggle(this,'Hear');" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-3">
                                <asp:CheckBox runat="server" ID="Hear3" Text="Instagram" onClick="toggle(this,'Hear');" />
                            </div>
                            <div class="col-md-3">
                                <asp:CheckBox runat="server" ID="Hear4" Text="Word of mouth (friends/family" onClick="toggle(this,'Hear');" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-3">
                                <asp:CheckBox runat="server" ID="Hear7" Text="Walk-in" onClick="toggle(this,'Hear');" />
                            </div>
                            <div class="col-md-3">
                                <asp:CheckBox runat="server" ID="Hear5" Text="Visited with a friend" onClick="toggle(this,'Hear');" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-3">
                                <asp:CheckBox runat="server" ID="CheckBox2" Text="Visited with a friend" onClick="toggle(this,'Hear');" />
                            </div>
                            <div class="col-md-1">
                                <asp:CheckBox runat="server" ID="Hear6" Text="Others" onClick="toggle(this,'Hear');" />
                            </div>
                            <div class="col-md-5">
                                <asp:TextBox ID="txtOther" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-7" style="margin-top: 10px;">
                                <label id="lblReturnSuggestion"><span class="fa fa-caret-right rgt_cart"></span>Will you return?</label>
                                <br />
                                <asp:CheckBox runat="server" ID="Return1" Text="Yes" onClick="toggle(this,'Return');" />
                                <asp:CheckBox runat="server" ID="Return2" Text="No" onClick="toggle(this,'Return');" Style="margin-left: 10%" />
                                <asp:CheckBox runat="server" ID="Return3" Text="Maybe" onClick="toggle(this,'Return');" Style="margin-left: 10%" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-7" style="margin-top: 10px;">
                                <label id="lblVisit"><span class="fa fa-caret-right rgt_cart"></span>How often do you visit?</label>
                                <br />
                                <asp:CheckBox runat="server" ID="Visit1" Text="Once in two weeks" onClick="toggle(this,'Visit');" />
                                <asp:CheckBox runat="server" ID="Visit2" Text="Once a month" onClick="toggle(this,'Visit');" Style="margin-left: 10%" />
                                <asp:CheckBox runat="server" ID="Visit3" Text="Once in 6 months" onClick="toggle(this,'Visit');" Style="margin-left: 10%" />
                                <br />
                                <asp:CheckBox runat="server" ID="Visit4" Text="Once a year" onClick="toggle(this,'Visit');" />
                                <asp:CheckBox runat="server" ID="Visit5" Text="This is my first visit" onClick="toggle(this,'Visit');" Style="margin-left: 17%" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-7" style="margin-top: 10px;">
                                <label><span class="fa fa-caret-right rgt_cart"></span>What do you like about our Cafe/Restaurant?</label>
                                <asp:TextBox ID="txtLikeSuggestion" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                            </div>
                            <div class="col-md-7">
                                <label><span class="fa fa-caret-right rgt_cart"></span>How can we improve?</label>
                                <asp:TextBox ID="txtImproveSuggestion" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1"></div>
                            <div class="col-md-7">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>What items would you like to see added to our menu?</label>
                                    <asp:TextBox ID="txtMenuSuggestion" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-1"></div>
                            <div class="col-md-7">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Please share your detail:</label>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 5px">
                            <div class="col-md-1"></div>
                            <div class="col-md-7">
                                <div class="col-md-6">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Name</label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Contact No</label>
                                    <asp:TextBox ID="txtContact" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Email Address</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>Address</label>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>City</label>
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-offset-1 col-md-6 ">
                                <div class="btnlist pull-right">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Text="Save" CssClass="btn btn-success" TabIndex="3" />
                                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel" CssClass="btn btn-danger" TabIndex="4" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:TextBox ID="hiddenPassword" runat="server" Visible="False" Width="97px"></asp:TextBox>
        </div>
    </div>
</asp:Content>
