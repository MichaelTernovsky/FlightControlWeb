let map;
let layerGroup;
let planeLayerGroup;
let markersMap = new Object();
let polyid = null;

function ParseDataToLine(data, id) {
    //parse the data to segments array and draw it
    let marray = [];
    let arr = data.segments;
    marray.push([data.initial_location["latitude"], data.initial_location["longitude"]]);
    for (let i = 0; i < arr.length; i++) {
        marray.push([data.segments[i]["latitude"], data.segments[i]["longitude"]]);
    };
    // layerGroup is create to able to dealte the polyline later.
    RemovePolyLine();
    AddHomeMarker(data.initial_location["latitude"], data.initial_location["longitude"]);
    AddDestMarker(data.segments[arr.length - 1]["latitude"], data.segments[arr.length - 1]["longitude"]);
    let polyline = L.polyline(marray, { color: 'red', dashArray: '5, 5', dashOffset: '0' }).addTo(map);
    polyline.addTo(layerGroup);
    polyid = id;
}
function DrawLine(id) {
    // use getJson to get a FlightPlan by id and then parse it to data and draw the line on the map.
    flightPlanUrl = "/api/FlightPlan/"
    $.getJSON(flightPlanUrl + id, function (data) {
        ParseDataToLine(data, id);
    });
}

function CreateMap() {
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
        let count = $('#tblDetails tr').length;
        if (count > 1) {
            document.getElementById("tblDetails").deleteRow(1);
        }
        //delete the polyline from the map.
        if (polyid != null) {
            RemovePolyLine();
        };
    })
}

function CreateIcon() {
    //icon
    let iconPlane = L.icon({
        iconUrl: 'https://img.icons8.com/color/48/000000/airplane-mode-on.png',
        iconSize: [50, 40],
        iconAnchor: [25, 25],
        popupAnchor: [-20, -20]
    })

    return iconPlane;
}


function CreateIconBold() {
    //icon
    let iconPlane2 = L.icon({
        iconUrl: 'https://img.icons8.com/nolan/48/airplane-mode-on.png',
        iconSize: [50, 40],
        iconAnchor: [25, 25],
        popupAnchor: [-20, -20]
    })

    return iconPlane2;
}

function CreateHomeIcon() {
    //icon
    let iconHome = L.icon({
        iconUrl: 'https://img.icons8.com/color/48/000000/order-delivered.png',
        iconSize: [48, 40],
        popupAnchor: [0, 0]
    })

    return iconHome;
}

function CreateDestIcon() {
    //icon
    let iconHome = L.icon({
        iconUrl: 'https://img.icons8.com/color/48/000000/filled-flag2.png',
        iconSize: [48, 40],
        popupAnchor: [0, 0]
    })

    return iconHome;
}

function ClearTables() {
    // delete the current values in both inetranl end external tables
    $('#tblInternalFlights tr:gt(0)').remove()
    $('#tblExternalFlights tr:gt(0)').remove()
}

function ClearFlightDetails(flightID) {
    //remove from flight details table
    let id = jQuery(".detailRow").find("td:eq(0)").text();

    if (id == flightID) {
        let count = $('#tblDetails tr').length;
        if (count > 1) {
            document.getElementById("tblDetails").deleteRow(1);
        }
    }
}

function DeleteOnClick(el) {
    //delete flight from evreywhere
    let row = $(el).closest('tr');
    row.remove();
    //get flight Id
    let firstID = row.find("td:first")[0].innerText;
    let urlDelete = "/api/Flights/" + firstID;

    //delete from the server
    $.ajax({
        url: urlDelete,
        method: 'delete',
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.clear = textStatus + ":" + jqXHR.status + " - " + "Could not delete the file";
        }
    });

    // removing flight details from the table of details
    ClearFlightDetails(firstID);

    // remove the marker
    let markerToDel = markersMap[firstID];
    map.removeLayer(markerToDel);
    if (polyid == firstID) {
        RemovePolyLine();
    }
    delete markerToDel;
}

function RemovePolyLine() {
    layerGroup.clearLayers();
    polyid = null;
}

function ShowFlightInTables(flight) {
    // show the flights
    if (flight.is_external === false) {
        $("#tblInternalFlights").append(`<tr class=\"tableRow\" id=${flight.flight_id} tabindex="0"><td onclick = FlightOnClick(this,0)>` + flight.flight_id + "</td>" + "<td onclick = FlightOnClick(this,0)>" + flight.company_name + "</td>" +
            "<td><button class=\"btn\" onclick = DeleteOnClick(this)><i class=\"fa fa-trash\"></i></button></td>" + "</tr>");
    } else {
        $("#tblExternalFlights").append(`<tr class=\"tableRow\" id=${flight.flight_id} tabindex="0"><td onclick = FlightOnClick(this,0)>` + flight.flight_id + "</td>" + "<td onclick=FlightOnClick(this,0)>" + flight.company_name + "</td><td></td><td></td>");
    }
}

function FlightOnClick(e, flag) {
    let id;

    if (flag == 0) {
        // get the id of the clicked flight
        let row = $(e).closest('tr');
        id = row.find("td:first")[0].innerText;
    }
    else {
        id = e;
    }

    // draw the path
    DrawLine(id);

    // mark the line in the table
    if (tblInternalFlights.rows[id]) {
        rows = tblInternalFlights.getElementsByTagName('tr');
        rows[id].style.background = "#73b6e6";
    }
    if (tblExternalFlights.rows[id]) {
        rows = tblExternalFlights.getElementsByTagName('tr');
        rows[id].style.background = "#73b6e6";
    }


    // show the marker popup
    let marker = markersMap[id];
    let boldIcon = CreateIconBold();
    marker.openPopup();
    marker.setIcon(boldIcon);
    // createt thr url
    let url = "/api/FlightPlan/" + id;

    $.ajax({
        url: url,
        method: 'GET',
        success: function (flightPlan) {

            // show the details in the flight details table
            let count = $('#tblDetails tr').length;
            if (count > 1) {
                document.getElementById("tblDetails").deleteRow(1);
            }

            let finalLat;
            let finalLon;
            let endlTime = new Date(flightPlan.initial_location.date_time);

            let initialLat = flightPlan.initial_location.latitude;
            let initialLon = flightPlan.initial_location.longitude;

            flightPlan.segments.forEach(function (seg) {
                finalLat = seg.latitude;
                finalLon = seg.longitude;
                let addTime = endlTime.getSeconds() + parseFloat(seg.timespan_seconds);

                endlTime.setSeconds(addTime);
            })

            let stringDate = endlTime.toString().substr(0, 25);

            //write the new details
            $("#tblDetails").append("<tr class=\"detailRow\"><td>" + id + "</td>" + "<td>" + initialLon + "</td>" + "<td>" + initialLat + "</td>" + "<td>" + flightPlan.initial_location.date_time + "</td>" + "<td>" + flightPlan.passengers + "</td>" + "<td>" + flightPlan.company_name + "</td>" + "<td>" + finalLon + "</td>" + "<td>" + finalLat + "</td>" + "<td>" + stringDate + "</td>" + "<td></tr>");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            toastr.error = textStatus + ":" + jqXHR.status + " - " + "Could not get the flight details";
        }
    });
}

function AddMarkerToMap(lat, lon, id, angle) {
    //creating the icon
    let iconPlane = CreateIcon();
    let marker = L.marker([lat, lon], { icon: iconPlane, rotationAngle: angle }).addTo(map);
    marker.addTo(planeLayerGroup);
    marker.bindPopup(id);
    marker.on("click", function () {
        FlightOnClick(id, 1);
    });
    markersMap[id] = marker;
}

function IfOnLine(linep1x, linep1y, linep2x, linep2y, px, py) {
    //check if point is between 2 point(on line).
    let distance = function (p1x, p1y, p2x, p2y) {
        return Math.sqrt(Math.pow(p1x - p2x, 2) + Math.pow(p1y - p2y, 2));
    };
    let dist_sum = Math.round(distance(linep1x, linep1y, px, py) + distance(linep2x, linep2y, px, py));
    return true ? dist_sum == Math.round(distance(linep1x, linep1y, linep2x, linep2y)) : false;
}

function CalcAngle(lat, lon, segArr) {
    //calc the current angle of the plane
    current_lat = lat;
    current_lon = lon;
    let i = 0;
    for (i = 0; i < segArr.length - 1; i++) {

        //check of evrey seg if its the current seg
        if (IfOnLine(segArr[i][0], segArr[i][1], segArr[i + 1][0], segArr[i + 1][1], current_lat, current_lon) == true) {
            break;
        }
    }
    //calc the angle in degrees
    let angleDeg = -1 * Math.atan2(segArr[i + 1][0] - segArr[i][0], segArr[i + 1][1] - segArr[i][1]) * 180 / Math.PI;
    return angleDeg;
}

function AddHomeMarker(lat, lon) {
    let iconHome = CreateHomeIcon();
    let marker = L.marker([lat, lon], { icon: iconHome }).addTo(map);
    marker.addTo(layerGroup);
}

function AddDestMarker(lat, lon) {
    let iconDest = CreateDestIcon();
    let marker = L.marker([lat, lon], { icon: iconDest }).addTo(map);
    marker.addTo(layerGroup);
}

function GetAngleBySegArr(latitude, longtitude, flightId) {
    let flag = 0;
    let url = "/api/FlightPlan/" + flightId;
    let answer = [];
    $.ajax({
        url: url,
        method: 'GET',
        async: false,
        success: function (flightPlan) {

            let initialLat = flightPlan.initial_location.latitude;
            let initialLon = flightPlan.initial_location.longitude;
            init = [initialLat, initialLon];
            answer.push(init);
            //get all the segments include the initialLocation
            flightPlan.segments.forEach(function (seg) {
                let segLat = seg.latitude;
                let segLon = seg.longitude;
                seg = [segLat, segLon];
                answer.push(seg);
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            flag = 1;
            toastr.error = textStatus + ":" + jqXHR.status + " - " + "Could not get the flight details";
        }
    });

    if (flag == 0) {
        angle = CalcAngle(latitude, longtitude, answer);
        return angle;
    }
}

function GetFlightData() {

    let date = new Date().toISOString().substr(0, 19);
    //use the data from the server to fill the tables and mark the map 
    let url = "/api/Flights?relative_to=";
    let currentDate = date;

    $.getJSON(url + currentDate + "&sync_all", function (data) {
        let flag = 0;

        //clear all the planes markers from the map
        planeLayerGroup.clearLayers();

        // clear the table
        ClearTables();

        // get the id from the details table
        let id = jQuery(".detailRow").find("td:eq(0)").text();

        // show the flights in the tables
        data.forEach(function (flight) {
            // showing the flights in the correct tables
            ShowFlightInTables(flight);

            // delete details if needed
            if (id == flight.flight_id) {
                flag = 1;
            }

            // saving the values
            let longtitude = flight.longitude;
            let latitude = flight.latitude;
            let flightid = flight.flight_id;
            let angle = GetAngleBySegArr(latitude, longtitude, flight.flight_id);
            AddMarkerToMap(latitude, longtitude, flightid, angle);
        });

        if (flag == 0) {
            ClearFlightDetails(id);
            RemovePolyLine(id);
        }
    });
}

let jsondrop = function (elem, inputElem, options) {
    this.element = document.getElementById(elem);
    this.inputElement = document.getElementById(inputElem);
    this.options = options || {};
    this.name = 'jsondrop';
    this.files = [];
    this._addEventHandlers();
}

jsondrop.prototype._readFiles = function (files) {
    let _this = this;
    for (i = 0; i < files.length; i++) {
        (function (file) {
            let fr = new FileReader();
            fr.readAsText(file, 'UTF-8');
            fr.onload = function () {
                let json = JSON.parse(fr.result);
                $.ajax({
                    type: "POST",
                    url: "api/FlightPlan",
                    data: JSON.stringify(json),
                    contentType: "application/json",
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        toastr.error("Request: " + XMLHttpRequest + "\n\nStatus: " + textStatus + "\n\nError: " + "Problem in Json file");
                    }

                });
            };
        })(files[i]);
    }
}

jsondrop.prototype._addEventHandlers = function () {

    // bind jsondrop to _this for use in 'ondrop'
    let _this = this;

    this.element.addEventListener('dragover', OnDragOver, false);
    this.element.addEventListener('dragleave', OnDragLeave, false);
    this.element.addEventListener('drop', OnDrop, false);
    this.inputElement.onchange = function (e) {
        e = document.getElementById('fileElem');
        e = e || event;
        this.className = 'list_in';
        _this._readFiles(e.files);
    };


    function OnDragOver(e) {
        $("#tblInternalFlights").hide();
        $("#choina").hide();
        e = e || event;
        e.preventDefault();
        this.className = 'dragging';
    }

    function OnDragLeave(e) {
        e = e || event;
        e.preventDefault();
        this.className = 'list_in';
        $("#tblInternalFlights").show();
        $("#choina").show();
    }

    function OnDrop(e) {
        e = e || event;
        e.preventDefault();
        this.className = 'list_in';
        files = e.dataTransfer.files;
        _this._readFiles(files);
        $("#tblInternalFlights").show();
        $("#choina").show();
    }
}