let map;
var layerGroup;
var markersMap = new Object();
var polyid = null;

function ParseDataToLine(data,id) {
    //parse the data to segments array and draw it
    var marray = [];
    let arr = data.segments;
    for (let i = 0; i < arr.length; i++) {
        marray.push([data.segments[i]["latitude"], data.segments[i]["longitude"]]);
    };
    //layerGroup is create to able to dealte the polyline later.
    removePolyLine();
    var polyline = L.polyline(marray, { color: 'red' }).addTo(map);
    polyline.addTo(layerGroup);
    polyid = id;
}
function drawLine(id) {
    //use getJson to get a FlightPlan by id and then parse it to data and draw the line on the map.
    flightPlanUrl = "/api/FlightPlan/"
    $.getJSON(flightPlanUrl + id, function (data) {
        ParseDataToLine(data,id);
    });
}

function createMap() {
    //create map
    map = L.map('map').setView([34.873331, 32.006333], 1.5);
    L.tileLayer('https://api.maptiler.com/maps/hybrid/{z}/{x}/{y}.jpg?key=z9JRmQouqskUAwB0autN', {
        attribution: '<a href="https://www.maptiler.com/copyright/" target="_blank">&copy; MapTiler</a> <a href="https://www.openstreetmap.org/copyright" target="_blank">&copy; OpenStreetMap contributors</a>',
    }).addTo(map);

    layerGroup = L.layerGroup().addTo(map);

    map.on('click', function (e) {
        //click on the map event
        //delete the polyline from the map.
        if (polyid != null) {
            removePolyLine();
        };
    })
}

function createIcon() {
    //icon
    var iconPlane = L.icon({
        iconUrl: 'https://img.icons8.com/color/48/000000/airplane-mode-on.png',
        iconSize: [50, 40],
        iconAnchor: [0, 0],
        popupAnchor: [0, 0]
    })

    return iconPlane;
}

function clearTables() {
    // delete the current values in both inetranl end external tables
    $('#tblInternalFlights tr:gt(0)').remove()
    $('#tblExternalFlights tr:gt(0)').remove()
}

function clearFlightDetails(flightID) {
    //remove from flight details table
    var id = jQuery(".detailRow").find("td:eq(0)").text();

    if (id == flightID) {
        var count = $('#tblDetails tr').length;
        if (count > 1) {
            document.getElementById("tblDetails").deleteRow(1);
        }
    }
}

function deleteOnClick(el) {
    //delete flight from evreywhere
    var row = $(el).closest('tr');
    row.remove();
    //get flight Id
    var firstID = row.find("td:first")[0].innerText;
    var urlDelete = "/api/Flights/" + firstID;

    //delete from the server
    $.ajax({
        url: urlDelete,
        method: 'delete'
    });

    // removing flight details from the table of details
    clearFlightDetails(firstID);

    // remove the marker
    var markerToDel = markersMap[firstID];
    //markerToDel.closePopup();
    //markersMap.delete(firstID);
    map.removeLayer(markerToDel);
    if (polyid == firstID) {
        removePolyLine();
    }
    delete markerToDel;
}

function removePolyLine() {
    layerGroup.clearLayers();
    polyid = null;
}

function showFlightInTables(flight) {
    // show the flights
    if (flight.is_external === false) {
        $("#tblInternalFlights").append(`<tr class=\"tableRow\" id=${flight.flight_id}><td onclick = flightOnClick(this,0)>` + flight.flight_id + "</td>" + "<td onclick = flightOnClick(this,0)>" + flight.company_name + "</td>" +
            "<td onclick = flightOnClick(this,0)>" + flight.is_external + "</td>" + "<td><button onclick = deleteOnClick(this)>delete</button></td>" + "</tr>");
    } else {
        $("#tblExternalFlights").append(`<tr class=\"tableRow\" id=${flight.flight_id}><td onclick = flightOnClick(this,0)>` + flight.flight_id + "</td>" + "<td onclick=flightOnClick(this,0)>" + flight.company_name + "</td>" +
            "<td onclick=flightOnClick(this,0)>" + flight.is_external);
    }
}

function flightOnClick(e, flag) {
    let id;
        
    if (flag == 0) {
        // get the id of the clicked flight
        var row = $(e).closest('tr');
        id = row.find("td:first")[0].innerText;
    }
    else {
        id = e;
    }

    // draw the path
    drawLine(id);

    // show the marker popup
    var marker = markersMap[id];
    marker.openPopup();

    // createt thr url
    var url = "/api/FlightPlan/" + id;

    $.ajax({
        url: url,
        method: 'GET',
        success: function (flightPlan) {

            // show the details in the flight details table
            var count = $('#tblDetails tr').length;
            if (count > 1) {
                document.getElementById("tblDetails").deleteRow(1);
            }

            var finalLat;
            var finalLon;
            var endlTime = new Date(flightPlan.initial_location.date_time);

            var initialLat = flightPlan.initial_location.latitude;
            var initialLon = flightPlan.initial_location.longitude;

            flightPlan.segments.forEach(function (seg) {
                finalLat = seg.latitude;
                finalLon = seg.longitude;
                var addTime = endlTime.getSeconds() + parseFloat(seg.timeSpan_seconds);
                endlTime.setSeconds(addTime);
            })

            //write the new details
            $("#tblDetails").append("<tr class=\"detailRow\"><td>" + flightPlan.flight_id + "</td>" + "<td>" + initialLon + "</td>" + "<td>" + initialLat + "</td>" + "<td>" + flightPlan.initial_location.date_time + "</td>" + "<td>" + flightPlan.passengers + "</td>" + "<td>" + flightPlan.company_name + "</td>" + "<td>" + finalLon + "</td>" + "<td>" + finalLat + "</td>" + "<td>" + endlTime + "</td>" + "<td></tr>");
        }
    });
}

function addMarkerToMap(lon, lat, id) {
    //creating the icon
    var iconPlane = createIcon();

    if (markersMap.hasOwnProperty(id)) {
        markersMap[id].setLatLng([lat, lon]).update();
    }
    else {
        let marker = L.marker([lon, lat], { icon: iconPlane }).addTo(map);
        marker.on("click", function () {
            flightOnClick(id, 1)
        });
        markersMap[id] = marker;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////
    //marker.bindPopup(id).openPopup();
}

function getFlightData() {
    var date = new Date().toISOString().substr(0, 19);
    //var url = "api/Flights?relative_to=" + date + "sync_all";

    //use the data from the server to fill the tables and mark the map 
    var url = "api/Flights?relative_to=";
    var currentDate = "2020-12-27T01:56:21";

    $.getJSON(url + currentDate + "&sync_all", function (data) {

        // clear the table
        clearTables();
        // show the flights in the tables
        data.forEach(function (flight) {

            // showing the flights in the correct tables
            showFlightInTables(flight);

            // saving the values
            var longtitude = flight.longitude;
            var latitude = flight.latitude;
            var flightid = flight.flight_id;

            addMarkerToMap(longtitude, latitude, flightid);
        });
    });
}