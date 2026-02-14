<%@ Page Language="C#" MasterPageFile="~/Forms/PageMaster.master" AutoEventWireup="true"
    CodeFile="frmRiderTracking.aspx.cs" Inherits="frmRiderTracking" Title="CORN :: Rider Tracking" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPage" runat="Server">   
    <script src="../js/jquery-1.10.2.js" type="text/javascript"></script>
    
    <script language="JavaScript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) {
            var oControl = args.get_postBackElement();
            oControl.value = "Wait...";
            oControl.disabled = true;
        }

        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 10005;
        }
       </script>

    <script type="text/javascript">
        var teamSuiteAPIKey = 'AIzaSyAXW_iXapYcV2gf_W1_i6zB570VneCtHIw';

        var pathCoordinatesArray = [];
        var pathStr = '';
        var distanceDetailArray = [];
        var roadCoordinatesArray = [];
        var startTime;
        var endTime;
        var map = '';

            // Initialize and add the map
        function initMap() {
            debugger;
                // The location of Pakistan
                var paklatlong = { lat: 30.389367, lng: 70.512716 };
                // The map, centered at Pakistan
                map = new google.maps.Map(
                    document.getElementById('map'), { zoom: 7, center: paklatlong });
              
        }

        function setMapByLocation(fromLocation, toLocation) {
            debugger;

            pathStr = [parseFloat(fromLocation), parseFloat(toLocation)] + '|';

            pathCoordinatesArray.push({ lat: parseFloat(fromLocation), lng: parseFloat(toLocation) });

            if (pathStr != null && pathStr.length > 10) {


                //$.get('https://roads.googleapis.com/v1/snapToRoads', {
                //    interpolate: true,
                //    key: teamSuiteAPIKey,
                //    path: pathStr
                //}, function (dataRoad) {
                //    debugger;
                //    //processSnapToRoadResponse(data);
                //    distanceDetailArray = [];
                //    if (dataRoad != null && dataRoad.snappedPoints != null) {

                //        var locationObj = dataRoad.snappedPoints;

                //        for (var ii = 0; ii < locationObj.length; ii++) {
                //            roadCoordinatesArray.push({ lat: locationObj[ii].location.latitude, lng: locationObj[ii].location.longitude });
                //            distanceDetailArray.push({ lat: locationObj[ii].location.latitude, lng: locationObj[ii].location.longitude });
                //        }
                //    }




                pathGooglePolyline = new google.maps.Polyline({
                    path: pathCoordinatesArray,
                    strokeColor: '#FF0000',
                    strokeOpacity: 1.0,
                    strokeWeight: 2
                });

                //if (pathCoordinatesArray.length > 0) {
                if (pathCoordinatesArray.length > 0) {
                    //get map
                    map = new google.maps.Map(
                                    document.getElementById('map'),
                                    { zoom: 10, center: pathCoordinatesArray[0] }
                               );

                    //set data on map
                    pathGooglePolyline.setMap(map);

                    var marker, i;

                    //for (i = 0; i < pathCoordinatesArray.length; i++) {
                    for (var i = 0; i < pathCoordinatesArray.length; i++) {

                        if (i == 0) {

                            marker = new google.maps.Marker({
                                //position: pathCoordinatesArray[i],
                                position: pathCoordinatesArray[i],
                                map: map,
                                Title: startTime
                            });
                        }
                        else if (i == pathCoordinatesArray.length - 1) {

                            marker = new google.maps.Marker({
                                //position: pathCoordinatesArray[i],
                                position: pathCoordinatesArray[i],
                                map: map,
                                Title: endTime
                            });
                        }
                        else {
                            marker = new google.maps.Marker({
                                //position: pathCoordinatesArray[i],
                                position: pathCoordinatesArray[i],
                                map: map//,
                                ///Title: $.format.date(new Date(data[i].TimeStamp.match(/\d+/)[0] * 1), "MM/dd/yyyy HH:mm:ss a")
                            });
                        }
                    }

                    //var marker = new google.maps.Marker({ position: pathCoordinatesArray[0], map: map });
                }
            }
                //});
        }

        
        function liveTracking(details) {
            // Get the user's current location.

            $.each(details, function (index, val) {
                debugger;
                if (val.Latitude != null && val.Longitude != null) {
                    var marker = new google.maps.Marker({
                        position: { lat: parseFloat(val.Latitude), lng: parseFloat(val.Longitude) },
                        map: map,
                        title: "Live Tracker",
                        center: { lat: parseFloat(val.Latitude), lng: parseFloat(val.Longitude) }
                    });

                    pathCoordinatesArray.push({ lat: parseFloat(val.Latitude), lng: parseFloat(val.Longitude) });

                    pathStr = [parseFloat(val.Latitude), parseFloat(val.Longitude)] + '|';
                }
            });

            pathGooglePolyline = new google.maps.Polyline({
                path: pathCoordinatesArray,
                strokeColor: '#FF0000',
                strokeOpacity: 1.0,
                strokeWeight: 2
            });

            pathGooglePolyline.setMap(map);
            
            //// Function to update marker position
            //function updateMarkerPosition(lat, lng) {
            //    var newPosition = new google.maps.LatLng(lat, lng);
            //    marker.setPosition(newPosition);
            //    map.setCenter(newPosition);
            //}

            if ($("#livePreview").prop("checked") == true) {
                //setInterval(function () {
                //    LoadOrderBookerData();
               // }, 10000);
            }
        }
</script>
<!--Load the API from the specified URL
* The async attribute allows the browser to render the page while the API loads
* The key parameter will contain your own API key (which is not needed for this tutorial)
* The callback parameter executes the initMap() function
-->
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=&callback=initMap" defer="defer">
</script>

<style type="text/css">
table {
  font-family: arial, sans-serif;
  border-collapse: collapse;
  width: 100%;
}

#map-table td, th {
  border: 1px solid #dddddd;
  text-align: left;
  padding: 8px;
}

#map-table tr:nth-child(even) {
  background-color: #dddddd;
}

 #map { margin: 0; padding: 0; height: 500px !important; width: 100%; }

.switch {
  position: relative;
  display: inline-block;
  width: 60px;
  height: 34px;
}

.switch input { 
  opacity: 0;
  width: 0;
  height: 0;
}

.slider {
  position: absolute;
  cursor: pointer;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #ccc;
  -webkit-transition: .4s;
  transition: .4s;
}

.slider:before {
  position: absolute;
  content: "";
  height: 26px;
  width: 26px;
  left: 4px;
  bottom: 4px;
  background-color: white;
  -webkit-transition: .4s;
  transition: .4s;
}

input:checked + .slider {
  background-color: #2196F3;
}

input:focus + .slider {
  box-shadow: 0 0 1px #2196F3;
}

input:checked + .slider:before {
  -webkit-transform: translateX(26px);
  -ms-transform: translateX(26px);
  transform: translateX(26px);
}

/* Rounded sliders */
.slider.round {
  border-radius: 34px;
}

.slider.round:before {
  border-radius: 50%;
}
</style>

    <script type="text/javascript">
        function SetOrderBookerId(s, e) {
            debugger;

            document.getElementById('<%=hfRiderID.ClientID%>').value = s.GetCurrentValue();
            //getRiderTrackingDetails(orderbookerId);
        }

        $(document).ready(function () {
            LoadOrderBooker(document.getElementById('<%=hflocationID.ClientID%>').value);
        });

        function SetLocationId(s, e) {
            debugger;

            document.getElementById('<%=hflocationID.ClientID%>').value = s.GetCurrentValue();
            TestComboBox.ClearItems();
            LoadOrderBooker(s.GetCurrentValue());
        }

        function LoadOrderBooker(locationId) {
            $.ajax({
                type: "POST", //HTTP method
                url: "frmRiderTracking.aspx/ddlLocation_Changed", //page/method name
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{'locationId':'" + locationId + "'}",
                async: false,
                success: setOrderBookers
            });
        }

        function setOrderBookers(data) {
            data = JSON.parse(data.d);
            $.each(data, function (index, val) {
                TestComboBox.AddItem(val.USER_NAME.trim(), val.USER_ID);
                if (index == 0) {
                    document.getElementById('<%=hfRiderID.ClientID%>').value = val.USER_ID;
                    TestComboBox.SetSelectedIndex(index);
                }
            });
        }

        function LoadOrderBookerData() {
            var orderBookerId = document.getElementById('<%=hfRiderID.ClientID%>').value;
            var locationId = document.getElementById('<%=hflocationID.ClientID%>').value;
            
            var fromDate = document.getElementById('<%=txtFromDate.ClientID%>').value;
             var toDate= document.getElementById('<%=txtToDate.ClientID%>').value;

            getRiderTrackingDetails(orderBookerId, locationId, fromDate, toDate);
        }

        function getRiderTrackingDetails(orderBookerId, locationId, fromDate, toDate) {
                $.ajax({
                    type: "POST", //HTTP method
                    url: "frmRiderTracking.aspx/GetRiderTrackingDetails", //page/method name
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify({
                        orderBookerId: orderBookerId, locationId: locationId,
                        fromDate: fromDate, toDate: toDate
                    }),
                    async: false,
                    success: FillTrackingDetails
                });
        }

        function FillTrackingDetails(details) {
            details = JSON.parse(details.d);
            var trHtml = '';

            var filteredDetails = [];
            var uniqueObjects = [];

            for (var i = 0; i < details.length; i++) {

                var distance = 0;
                if (details[i].Distance_KM == null || details[i].Distance_KM == '' || isNaN(details[i].Distance_KM)) {
                    distance = 0;
                }
                else {
                    distance = parseFloat(details[i].Distance_KM).toFixed(4);
                }

                var newelement = {};
                newelement.Order_Date = details[i].Order_Date;
                newelement.Order_Time = details[i].Order_Time;
                newelement.FromLatitude = details[i].FromLatitude;
                newelement.FromLongitude = details[i].FromLongitude;
                newelement.ToLatitude = details[i].ToLatitude;
                newelement.ToLongitude = details[i].ToLongitude;
                newelement.ORDER_NO = details[i].ORDER_NO;
                newelement.CUSTOMER_NAME = details[i].CUSTOMER_NAME;
                newelement.Distance_KM = distance;
                newelement.ADDRESS = details[i].ADDRESS;

                var isDuplicate = uniqueObjects.some(existingObject => {
                    return JSON.stringify(existingObject) === JSON.stringify(newelement);
                });

                if (!isDuplicate) {
                    uniqueObjects.push(newelement);
                    filteredDetails.push(newelement);
                }
            }

            for (var i = 0; i < filteredDetails.length; i++) {
                trHtml += '<tr> <td> ' + filteredDetails[i].Order_Date + ' </td><td> ' + filteredDetails[i].Order_Time + ' </td><td style="display:none;"> ' +
                       filteredDetails[i].FromLatitude + ' ' + filteredDetails[i].FromLongitude + ' </td>' +
                       '<td style="display:none;">' + filteredDetails[i].ToLatitude + ' ' + filteredDetails[i].ToLongitude + '</td>' +
                       '<td>' + filteredDetails[i].ORDER_NO + '</td><td>' + filteredDetails[i].CUSTOMER_NAME + '</td>' +
                       '<td>' + filteredDetails[i].ADDRESS + '</td><td>' + filteredDetails[i].Distance_KM + '</td> </tr>';
            }

            //console.log(trHtml);

            $("#map-table tbody").html(trHtml);

            if ($("#livePreview").prop("checked") == true) {
                liveTracking(details);
            }


            $("#map-table tr").on('click', function () {
                var fromLocation = $(this).find('td:nth-child(3)').html().trim();
                var toLocation = $(this).find('td:nth-child(4)').html().trim();

                setMapByLocation(fromLocation, toLocation);
            });
        }

    </script>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" DefaultButton="btndummy">
                <div class="main-contents">
                    <div class="container role_management">
                        <div class="row">
                             <div class="col-md-9"></div>
                            <div class="col-md-3">
                                <span>Live Tracking </span>
                                <label class="switch">
                                    <input type="checkbox" id="livePreview">
                                    <span class="slider round"></span>
                                </label>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-md-5">
                                <div class="col-md-6">
                                    <asp:HiddenField ID="hflocationID" runat="server" Value="0" />
                                    <label>
                                        <span class="fa fa-caret-right rgt_cart"></span>Location</label>

                                    <dx:ASPxComboBox ID="ddlLocation" runat="server" CssClass="form-control">
                                        <ClientSideEvents SelectedIndexChanged="SetLocationId" />
                                    </dx:ASPxComboBox>
                                </div>

                                <div class="col-md-6">
                                    <asp:HiddenField ID="hfRiderID" runat="server" Value="0" />
                                    <label>
                                        <span class="fa fa-caret-right rgt_cart"></span>Rider</label>

                                    <dx:ASPxComboBox ID="ddlOrderBooker" runat="server" CssClass="form-control" ClientInstanceName="TestComboBox" EnableCallbackMode="false" EnableSynchronization="True">
                                        <ClientSideEvents SelectedIndexChanged="SetOrderBookerId" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="col-md-9">
                                    <label><span class="fa fa-caret-right rgt_cart"></span>From Date:</label>
                                    <asp:TextBox ID="txtFromDate" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3" style="margin-top: 25px">
                                    <asp:ImageButton ID="ibtnFromDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                        Width="30px" />
                                    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                    <cc1:CalendarExtender ID="CEFromDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnFromDate"
                                        TargetControlID="txtFromDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                                </div>
                            </div>

                            <div class="col-md-2">
                                <label><span class="fa fa-caret-right rgt_cart"></span>To Date:</label>
                                <asp:TextBox ID="txtToDate" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-1" style="margin-top: 25px">
                                <asp:ImageButton ID="ibtnToDate" runat="server" ImageUrl="~/App_Themes/Granite/Images/date.gif"
                                    Width="30px" />
                                <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
                                <cc1:CalendarExtender ID="CEToDate" runat="server" Format="dd-MMM-yyyy" PopupButtonID="ibtnToDate"
                                    TargetControlID="txtToDate" OnClientShown="calendarShown"></cc1:CalendarExtender>
                            </div>

                            <div class="col-md-1" style="margin-top: 25px">
                                <button type="button" class="btn btn-primary" onclick="LoadOrderBookerData()">Show</button>
                            </div>

                        </div>
 
                        

                        <div class="row">
                         <div class="col-md-12" style="width: 96.5%; margin-left: 1.7%">
                             <div id="map"></div>
                         </div>
                        </div>

                        <div class="row bottom">
                            <table id="map-table">
                                <thead>
                                    <tr>
                                    <th>Order Date</th>
                                    <th>Order Time</th>
                                    <th style="display: none;">From Location</th>
                                    <th style="display:none;">To Location</th>
                                    <th>Order No.</th>
                                    <th> Customer</th>
                                    <th> Address</th>
                                    <th>Distance Travelled (KM)</th>
                                        </tr>
                                </thead>
                                <tbody>

                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <asp:LinkButton ID="btndummy" runat="server" UseSubmitBehavior="false" />
            </asp:Panel>
        </ContentTemplate>
 <%--           <Triggers>
                <asp:AsyncPostbackTrigger ControlID="ddlLocation" EventName="SelectedIndexChanged" />
            </Triggers>--%>
    </asp:UpdatePanel>

    <script type="text/javascript">


</script>

</asp:Content>

