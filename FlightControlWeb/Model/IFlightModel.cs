using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    interface IFlightModel
    {
        IEnumerable<Server> getAllExternalServers();
        void addNewServer(string serverID, string serverURL);
        void deleteServer(string serverID);
        Server getServer(string serverID);
    }
}