using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlightControlWeb.Model.Managers
{
    /*
     * The concrete server manager of our web app
     */
    public class ServerManager : IServerManager
    {
        // the list of servers
        private static List<Server> serversList = new List<Server>();

        // Servers method implemantaion
        public void addNewServer(Server newServer)
        {
            serversList.Add(newServer);
        }

        public void deleteServer(string serverID)
        {
            Server s = serversList.Where(x => String.Equals(x.ServerID, serverID)).FirstOrDefault();
            if (s != null)
                serversList.Remove(s);
        }

        public IEnumerable<Server> getAllExternalServers()
        {
            return serversList;
        }

        public Server getServer(string serverID)
        {
            Server s = serversList.Where(x => String.Equals(x.ServerID, serverID)).FirstOrDefault();
            return s;
        }
    }
}