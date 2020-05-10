using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    /*
     * The concrete server manager of our web app
     */
    public class ServerManager : IServerManager
    {
        // the list of servers
        private static List<Server> serversList = new List<Server>();

        //MemoryCache MemoryCache = new MemoryCache(int id, Flight f);


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