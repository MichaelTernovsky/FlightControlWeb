using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    /*
     * The interface of our models of our web app
     */
    public interface IServerManager
    {
        IEnumerable<Server> getAllExternalServers();
        void addNewServer(Server newServer);
        void deleteServer(string serverID);
        Server getServer(string serverID);
    }
}