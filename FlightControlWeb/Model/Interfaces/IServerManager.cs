using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Managers;

namespace FlightControlWeb.Model.Interfaces
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