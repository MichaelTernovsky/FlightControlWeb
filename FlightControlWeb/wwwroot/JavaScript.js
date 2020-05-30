let map;
var layerGroup;
var planeLayerGroup;
var markersMap = new Object();
var polyid = null;

function ParseDataToLine(data,id) {
    //parse the data to segments array and draw it
    var marray = [];
    let arr = data.segments;
    marray.push([data.initial_location["latitude"], data.initial_location["longitude"]]);
    for (let i = 0; i < arr.length; i++) {
        marray.push([data.segments[i]["latitude"], data.segments[i]["longitude"]]);
    };
    //layerGroup is create to able to dealte the polyline later.
    removePolyLine();
    addHomeMarker(data.initial_location["latitude"], data.initial_location["longitude"]);
    addDestMarker(data.segments[arr.length - 1]["latitude"], data.segments[arr.length - 1]["longitude"]);
    var polyline = L.polyline(marray, { color: 'red', dashArray: '20, 20', dashOffset: '0'}).addTo(map);
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
    planeLayerGroup = L.layerGroup().addTo(map);


    map.on('click', function (e) {
                //click on the map event
        //remove data from details tbl
        var count = $('#tblDetails tr').length;
        if (count > 1) {
            document.getElementById("tblDetails").deleteRow(1);
        }
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
        iconAnchor: [25, 25],
        popupAnchor: [0, 0]
    })

    return iconPlane;
}

function createHomeIcon() {
    //icon
    var iconHome = L.icon({
        iconUrl: 'https://img.icons8.com/color/48/000000/order-delivered.png',
        iconSize: [48, 40],
        popupAnchor: [0, 0]
    })

    return iconHome;
}

function createDestIcon() {
    //icon
    var iconHome = L.icon({
        iconUrl: 'https://img.icons8.com/color/48/000000/filled-flag2.png',
        iconSize: [48, 40],
        popupAnchor: [0, 0]
    })

    return iconHome;
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
            $("#tblDetails").append("<tr class=\"detailRow\"><td>" + id + "</td>" + "<td>" + initialLon + "</td>" + "<td>" + initialLat + "</td>" + "<td>" + flightPlan.initial_location.date_time + "</td>" + "<td>" + flightPlan.passengers + "</td>" + "<td>" + flightPlan.company_name + "</td>" + "<td>" + finalLon + "</td>" + "<td>" + finalLat + "</td>" + "<td>" + endlTime + "</td>" + "<td></tr>");
        },
        error: function (jqXHR, textSatus, errorThrown) {
            alert(textStatus + ":" + jqXHR.status + " " + errorThrown)
        }
    });
}

function addMarkerToMap(lat, lon, id, angle) {
    //creating the icon
    var iconPlane = createIcon();
    let marker = L.marker([lat, lon], { icon: iconPlane, rotationAngle: angle }).addTo(map);
    marker.addTo(planeLayerGroup);
    marker.bindPopup(id);
        marker.on("click", function () {
            flightOnClick(id, 1);
        });
    markersMap[id] = marker;
}

function ifOnLine(linep1x, linep1y, linep2x, linep2y, px, py) {
            //check if point is between 2 point(on line).
    var distance = function (p1x, p1y, p2x, p2y) {
        return Math.sqrt(Math.pow(p1x - p2x, 2) + Math.pow(p1y - p2y, 2));
    };
    var dist_sum = Math.round(distance(linep1x, linep1y, px, py) + distance(linep2x, linep2y, px, py));
    return true ? dist_sum == Math.round(distance(linep1x, linep1y, linep2x, linep2y)) : false;
}

function calcAngle(lat,lon,segArr) {
//calc the current angle of the plane
    current_lat = lat;
    current_lon = lon;

    var str = "BLA";
    for (var i = 0; i < segArr.length - 1; i++) {
       
        //check of evrey seg if its the current seg
        if (ifOnLine(segArr[i][0], segArr[i][1], segArr[i + 1][0], segArr[i + 1][1], current_lat, current_lon) == true) {
            str = "BLO";
            break;
        }
    }
    //calc the angle in degrees
    var angleDeg = -1 * Math.atan2(segArr[i + 1][0] - segArr[i][0], segArr[i + 1][1] - segArr[i][1]) * 180 / Math.PI; 
    return angleDeg;
}

function addHomeMarker(lat, lon) {
    let iconHome = createHomeIcon();
    let marker = L.marker([lat, lon], { icon: iconHome }).addTo(map);
    marker.addTo(layerGroup);
}

function addDestMarker(lat, lon) {
    let iconDest = createDestIcon();
    let marker = L.marker([lat, lon], { icon: iconDest }).addTo(map);
    marker.addTo(layerGroup);
}

function getAngleBySegArr(latitude,longtitude,flightId) {

    var url = "/api/FlightPlan/" + flightId;
    var answer =[];
    $.ajax({
        url: url,
        method: 'GET',
        async: false,
        success: function (flightPlan) {

            var initialLat = flightPlan.initial_location.latitude;
            var initialLon = flightPlan.initial_location.longitude;
            init = [initialLat, initialLon];
            answer.push(init);
            //get all the segments include the initialLocation
            flightPlan.segments.forEach(function (seg) {
                var segLat = seg.latitude;
                var segLon = seg.longitude;
                seg = [segLat, segLon];
                answer.push(seg);
            });
        },
        error: function (jqXHR, textSatus, errorThrown) {
            alert(textStatus + ":" + jqXHR.status + " " + errorThrown)
        }
    });
    angle = calcAngle(latitude, longtitude, answer);
    return angle;
}

function getFlightData() {

    var date = new Date().toISOString().substr(0, 19);
    //use the data from the server to fill the tables and mark the map 
    var url = "/api/Flights?relative_to=";
    var currentDate = date;

    $.getJSON(url + currentDate + "&sync_all", function (data) {
        //clear all the planes markers from the map
        planeLayerGroup.clearLayers();
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
            var angle = getAngleBySegArr(latitude,longtitude,flight.flight_id);
            addMarkerToMap(latitude, longtitude, flightid, angle);
        });
    });
}

var jsondrop = function (elem, inputElem, options) {
    this.element = document.getElementById(elem);
    this.inputElement = document.getElementById(inputElem);
    this.options = options || {};
    this.name = 'jsondrop';
    this.files = [];
    this._addEventHandlers();
}

jsondrop.prototype._readFiles = function (files) {
    var _this = this;
    for (i = 0; i < files.length; i++) {
        (function (file) {
            var fr = new FileReader();
            fr.readAsText(file, 'UTF-8');
            fr.onload = function () {
                var json = JSON.parse(fr.result);
                $.ajax({
                    type: "POST",
                    url: "api/FlightPlan",
                    data: JSON.stringify(json),
                    contentType: "application/json",
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                    }

                });
            };
        })(files[i]);
    }
}


jsondrop.prototype._addEventHandlers = function () {

    // bind jsondrop to _this for use in 'ondrop'
    var _this = this;

    this.element.addEventListener('dragover', ondragover, false);
    this.element.addEventListener('dragleave', ondragleave, false);
    this.element.addEventListener('drop', ondrop, false);
    this.inputElement.onchange = function (e) {
        e = document.getElementById('fileElem');
        e = e || event;
        this.className = 'list_in';
        _this._readFiles(e.files);
    };


    function ondragover(e) {
        $("#tblInternalFlights").hide();
        $("#choina").hide();
        e = e || event;
        e.preventDefault();
        this.className = 'dragging';
    }

    function ondragleave(e) {   //not working
        e = e || event;
        e.preventDefault();
        this.className = 'list_in';
        $("#tblInternalFlights").show();
        $("#choina").show();
    }

    function ondrop(e) {
        e = e || event;
        e.preventDefault();
        this.className = 'list_in';
        files = e.dataTransfer.files;
        _this._readFiles(files);
        $("#tblInternalFlights").show();
        $("#choina").show();
    }
}