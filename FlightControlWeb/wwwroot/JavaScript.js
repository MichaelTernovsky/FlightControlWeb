// Getting Flight Data From Server And Add It To Tables And Map Script

function getFlightData() {
    /*create map*/
    var map = L.map('map').setView([34.873331,32.006333], 1.5);
    L.tileLayer('https://api.maptiler.com/maps/hybrid/{z}/{x}/{y}.jpg?key=z9JRmQouqskUAwB0autN', {
        attribution: '<a href="https://www.maptiler.com/copyright/" target="_blank">&copy; MapTiler</a> <a href="https://www.openstreetmap.org/copyright" target="_blank">&copy; OpenStreetMap contributors</a>',
    }).addTo(map);
    //icon
    var iconPlane = L.icon({
        iconUrl: 'https://img.icons8.com/color/48/000000/airplane-mode-on.png',
        iconSize: [50, 40],
        iconAnchor: [0, 0],
        popupAnchor: [0, 0]
    })
    /*
     * use the data from the server to fill the tables and mark the map.
     */
    var url = "api/Flights?relative_to=2020-12-27T01:56:21&sync_all";
    $.getJSON(url, function (data) {
        console.log(data);
        //list of markers
        let flightMap = new Map();
        data.forEach(function (flight) {
            //fill the tables:
            $("#tblFlights").append("<tr><td>" + flight.flight_ID + "</td>" + "<td>" + flight.company_Name + "</td>" + "<td>" + flight.is_External + "</td>" + "<td><input type=\"button\" value=\"Delete\"></td></tr>");
            var longtitude = flight.longitude;
            var latitude = flight.latitude;
            var flightid = flight.flight_ID;
            //fill the map:
            var marker = L.marker([longtitude, latitude], { icon: iconPlane }).on('click', onClick);
            marker.id = flightid;
            flightMap.set(flightid, marker);
            function onClick(e) {
                //remove the old details
                var count = $('#tblDetails tr').length;
                if (count > 1) {
                    document.getElementById("tblDetails").deleteRow(1);
                }
                //write the new details
                $("#tblDetails").append("<tr><td>" + flight.flight_ID + "</td>" + "<td>" + flight.longitude + "</td>" + "<td>" + flight.latitude + "</td>" + "<td>" + flight.passengers + "</td>" + "<td>" + flight.company_Name + "</td>" + "<td>" + flight.date_Time + "</td>" + "<td>" + flight.is_External + "</td></tr>");
            }
            marker.bindPopup(flightid);
            marker.addTo(map)
            $('table').on('click', 'input[type="button"]', function (e) {
                var r = (this).closest('tr');
                var id = r.cells.item(0).innerHTML;
                flightMap.get(id).remove(); // true
                flightMap.delete(id); // true
                $(this).closest('tr').remove();
            })
        });
    });
}