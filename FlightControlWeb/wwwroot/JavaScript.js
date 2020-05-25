// global map
var map;
var globFlight;

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

function deleteFromServer(flightID) {
    $.ajax({
        type: "DELETE",
        url: "api/Flights/" + flightID
    });
}

function getFlightData() {



    //creating the icon
    var iconPlane = createIcon();

    //use the data from the server to fill the tables and mark the map 
    var url = "api/Flights?relative_to=";
    var currentTime = "2020-12-27T01:56:21";

    //list of markers
    let markersMap = new Map();


    $.getJSON(url + currentTime + "&sync_all", function (data) {
        // clear the table
        clearTables();
      
   
        data.forEach(function (flight) {
            globFlight = flight;
            // show the flights
            if (flight.is_external == false) {
                // all external flights
                $("#tblInternalFlights").append("<tr class=\"tableRow\" onclick=onClick()><td>" + flight.flight_id + "</td>" + "<td>" + flight.company_name + "</td>" + "<td>" + flight.is_external + "</td>" + "<td><input type=\"button\" value=\"Delete\"></td></tr>");
            }
            else {
                // all internal flights
                $("#tblExternalFlights").append("<tr class=\"tableRow\" onclick=onClick2()><td>" + flight.flight_id + "</td>" + "<td>" + flight.company_name + "</td>" + "<td>" + flight.is_external + "</td>" + "</tr > ");
            }
            // saving the values
            var longtitude = flight.longitude;
            var latitude = flight.latitude;
            var flightid = flight.flight_id;

                                      //test path line//

            var x = 40.444;
            var y = 74.333;
            // var point A = new L.LatLng(x, y);
         //   var layerGroup = L.layerGroup().addTo(map);
            function drawline(marray) {
                var polyline = L.polyline(marray, {color: 'red'}).addTo(map);
                polyline.addTo(layerGroup);
            }


                                     //test path line//

                //fill the map:
                var marker = L.marker([longtitude, latitude], { icon: iconPlane }).on('click', onClick);
                marker.id = flightid;
                markersMap.set(flightid, marker);

            function onClick(e) {
                console.log("CLICK");
                    //remove the old details
                    var count = $('#tblDetails tr').length;
                    if (count > 1) {
                        document.getElementById("tblDetails").deleteRow(1);
                    }
                    //write the new details
                    $("#tblDetails").append("<tr><td>" + flight.flight_id + "</td>" + "<td>" + flight.longitude + "</td>" + "<td>" + flight.latitude + "</td>" + "<td>" + flight.passengers + "</td>" + "<td>" + flight.company_name + "</td>" + "<td>" + flight.date_time + "</td>" + "<td>" + flight.is_external + "</td></tr>");
                }

                marker.bindPopup(flightid);
                marker.addTo(map)

                $('table').on('click', 'input[type="button"]', function (e) {
                    var r = (this).closest('tr');
                    var id = r.cells.item(0).innerHTML;

                    // delete the marker's image
                    markersMap.get(id).remove();
                    // delete the marker's object from the map
                    markersMap.delete(id);

                    $(this).closest('tr').remove();

                    // delete from server
                    deleteFromServer(id);
                })
            });
    });
}