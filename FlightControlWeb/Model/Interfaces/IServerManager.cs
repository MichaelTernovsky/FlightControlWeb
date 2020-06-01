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
        IEnumerable<Server> GetAllExternalServers();
        void AddNewServer(Server newServer);
        void DeleteServer(string serverID);
        Server GetServer(string serverID);
    }
}