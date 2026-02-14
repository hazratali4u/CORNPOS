<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmMenuCard.aspx.cs" Inherits="Forms_frmMenuCard" Title="CORN :: Menu Card" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">

    <link href="../css/chosen.css" rel="stylesheet" type="text/css" />
    <link href="../css/zebra_dialog.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-2.0.2.min.js" type="text/javascript"></script>
    <script src="../js/plugins/misc/chosen.jquery.js" type="text/javascript"></script>
    <script src="../AjaxLibrary/zebra_dialog.js" type="text/javascript"></script>
    <script src="../js/plugins/Block/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../AjaxLibrary/Forms/MenuCard20210527.js" type="text/javascript"></script>

    <script type="text/javascript">
        function SetCategoryId(s, e) {
            debugger;

            document.getElementById('<%=hfCategoryID.ClientID%>').value = s.GetCurrentValue();
            changeCategory(s.GetCurrentValue());
            //getRiderTrackingDetails(orderbookerId);
        }
    </script>

    <style type="text/css">
        .center-block {
            text-align: center;
        }
        .menu-body {
            width: 100%;
            padding: 5px;
            float: left;
            margin-bottom: 10px;
        }
        .menu-body .menu-thumbnail {
            float: left;
            width: 20%;
        }
        .menu-body .menu-thumbnail img {
            width: 250px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

        .menu-body .menu-details {
            float: left;
            width: 80%;
            display: table-cell;
            padding: 0px 8px 0px 8px;
        }

        .menu-body .menu-title {
            margin-bottom: 20px;
            display: block;
            border-bottom: 1px dashed #909090;
            padding-bottom: 10px;
        }
        .menu-body .menu-title h4 {
            display: inline-block;
        }
        .menu-body .menu-title h4, .menu-body .menu-details p {
            color: #1e2327;
        }
        .menu-body .menu-title .price {
            float: right;
            color: #e93e21;
            font-size: 20px;
            line-height: 40px;
            font-weight: 300;
        }

    </style>

    <style>
        @media (max-width: 1600px) {

            .our-menu .object-top-bottom .object-right {
                width: 7%;
            }

                .our-menu .object-top-bottom .object-right img {
                    width: 100%;
                }
        }

        @media (max-width: 1350px) {
            .object-bottom-top, .object-top-bottom, .object-top, .object-bottom {
                display: none;
            }
        }

        @media (max-width: 1272px) {
            .our-menu.dark .object-top-bottom .object-right {
                display: none;
            }
        }

        @media (max-width: 1199px) {
            .our-menu .menu-body {
                margin-bottom: 50px;
            }

            .our-menu.middle-img .menu-body:last-child {
                margin-bottom: 60px;
            }

            .our-menu .middle-image {
                display: none;
            }
        }

        @media only screen and (min-width: 768px) and (max-width: 999px) {
            .container {
                width: 96%;
            }
        }

        @media( max-width:992px) {
            .our-menu .middle-image {
                display: none;
            }
        }

        @media( max-width:991px) {
            /* Home 3 */
            .our-menu.belief .container-fuild {
                padding: 0 30px;
            }

            .our-menu .object-top .object-right {
                display: none;
            }

            .our-menu.belief.parallax .row-eq-height {
                display: block;
            }

            .our-menu.parallax.page-section-pt.pb-30 {
                padding-bottom: 10px !important;
            }
        }

        @media( max-width:767px) {
            .our-menu .menu-body, .our-menu .menu-body:last-child {
                margin-bottom: 30px;
            }
        }

        @media( max-width:479px) {
            .our-menu #tabs .tabs li i::before {
                font-size: 40px;
            }
        }
    </style>

    <section class="our-menu white-bg  page-section-pt" style="min-height: 250px;">
        
    <div id="blocker">
        <div><img src="../OrderPOS/images/wheel.gif"></div>
    </div>
        <div class="container">
            <div class="row">
                <div class="col-md-6">
                    <asp:HiddenField ID="hfCategoryID" runat="server" Value="0" />
                    <label><span class="fa fa-caret-right rgt_cart"></span>Category</label>
                    
                    <dx:ASPxComboBox ID="ddlCategory" runat="server" CssClass="form-control" AutoPostBack="false">
                        <ClientSideEvents SelectedIndexChanged="SetCategoryId" />
                    </dx:ASPxComboBox>               
                </div>
                <div class="col-md-6">
                    <label style="display: none;"><span class="fa fa-caret-right rgt_cart"></span>Search Item</label>
                    <input type="text" id="txtItem" class="form-control" style="display: none;" />
                </div>
            </div>
            <div class="row">
                <div id="menu-container"></div>
            </div>
        </div>
    </section>
    
    <div id="imagemodal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Image Viewer</h4>
                </div>
                <div class="modal-body">
                    <p>
                        <img class="img-responsive imagepreview" style="width: 100%;" >
                    </p>
                </div>
            </div><!-- /.modal-content -->
        </div><!-- /.modal-dialog -->
    </div><!-- /.modal -->

    <%--Start:Templates--%>
    <div style="display: none;">
        <select class="ddlTemplate">
            <option value="0"></option>
        </select>

        <div class="menu-body MenuItemTemplate">
            <div class="menu-thumbnail">
                <img class="img-responsive center-block menu-image" alt="">
            </div>
            <div class="menu-details">
                <div class="menu-title clearfix">
                    <h4 class="menu-header">Breakfast Complete</h4>
                    <span class="menu-price price">Rs. 99.0</span>
                </div>
                <div class="menu-description">
                    <p>item description comes here ...</p>
                </div>
            </div>
        </div>

    </div>

    <%--End:Templates--%>


    <script type="text/javascript">

    </script>

</asp:Content>