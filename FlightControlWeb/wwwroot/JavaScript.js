﻿// global map
var map;

function createMap() {
    /*create map*/
    map = L.map('map').setView([34.873331, 32.006333], 1.5);
    L.tileLayer('https://api.maptiler.com/maps/hybrid/{z}/{x}/{y}.jpg?key=z9JRmQouqskUAwB0autN', {
        attribution: '<a href="https://www.maptiler.com/copyright/" target="_blank">&copy; MapTiler</a> <a href="https://www.openstreetmap.org/copyright" target="_blank">&copy; OpenStreetMap contributors</a>',
    }).addTo(map);
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

function showFlightInTables(flight) {
    // show the flights
    if (flight.is_external == false) {
        // all external flights
        $("#tblInternalFlights").append("<tr><td>" + flight.flight_id + "</td>" + "<td>" + flight.company_name + "</td>" + "<td>" + flight.is_external + "</td>" + "<td><input type=\"button\" value=\"Delete\"></td></tr>");
    }
    else {
        // all internal flights
        $("#tblExternalFlights").append("<tr><td>" + flight.flight_id + "</td>" + "<td>" + flight.company_name + "</td>" + "<td>" + flight.is_external + "</td>" + "</tr > ");
    }
}

function deleteFlight() {
    $('table').on('click', 'input[type="button"]', function () {
        // get the row of the delete button that pressed
        var r = (this).closest('tr');
        // get the id of that row
        var id = r.cells.item(0).innerHTML;
        // delete the row
        r.remove();

        // delete the marker's image
        //markersMap.get(id).remove();
        // delete the marker's object from the map
        //markersMap.delete(id);

        // delete from server
        $.ajax({
            type: "DELETE",
            url: "api/Flights/" + id
        });
    })
}

function addRowOnClick(flight, i) {
    // create onclick to each row
    var table = document.getElementById("tblInternalFlights");
    //for (i = 1; i < rows.length; i++) {
    var currentRow = table.rows[i];
    var createClickHandler = function (row) {
        return function () {
            //remove the old details
            var count = $('#tblDetails tr').length;
            if (count > 1) {
                document.getElementById("tblDetails").deleteRow(1);
            }
            //write the new details
            $("#tblDetails").append("<tr><td>" + flight.flight_id + "</td>" + "<td>" + flight.longitude + "</td>" + "<td>" + flight.latitude + "</td>" + "<td>" + flight.passengers + "</td>" + "<td>" + flight.company_name + "</td>" + "<td>" + flight.date_time + "</td>" + "<td>" + flight.is_external + "</td></tr>");
        };
    };
    currentRow.onclick = createClickHandler(currentRow);
}

function getFlightData() {
    // assisting variable for adding onclick to each row
    var i = 1;

    //creating the icon
    var iconPlane = createIcon();

    var date = new Date().toISOString().substr(0, 19);
    //var url = "api/Flights?relative_to=" + date + "sync_all";

    //use the data from the server to fill the tables and mark the map 
    var url = "api/Flights?relative_to=2020-12-27T01:56:21&sync_all";

    $.getJSON(url, function (data) {
        //list of markers
        let markersMap = new Map();

        // clear the table
        clearTables();
        // show the flights in the tables
        data.forEach(function (flight) {

            showFlightInTables(flight);
            //addRowOnClick();

            // saving the values
            var longtitude = flight.longitude;
            var latitude = flight.latitude;
            var flightid = flight.flight_id;

            function onClick() {
                //remove the old details
                var count = $('#tblDetails tr').length;
                if (count > 1) {
                    document.getElementById("tblDetails").deleteRow(1);
                }
                //write the new details
                $("#tblDetails").append("<tr><td>" + flight.flight_id + "</td>" + "<td>" + flight.longitude + "</td>" + "<td>" + flight.latitude + "</td>" + "<td>" + flight.passengers + "</td>" + "<td>" + flight.company_name + "</td>" + "<td>" + flight.date_time + "</td>" + "<td>" + flight.is_external + "</td></tr>");
            }

            // adding onclick to each row
            addRowOnClick(flight, i);
            i++;

            //fill the map
            var marker = L.marker([longtitude, latitude], { icon: iconPlane }).on('click', onClick);
            marker.id = flightid;
            markersMap.set(flightid, marker);

            marker.bindPopup(flightid);
            marker.addTo(map)

            // delete the flight
            deleteFlight();
        });
    });
}