﻿using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using Microsoft.Extensions.Caching.Memory;
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
        private IMemoryCache cache;
        public ServerManager(IMemoryCache cache)
        {
            this.cache = cache;
        }

        // Servers method implemantaion
        public void addNewServer(Server newServer)
        {
            // get the list from the cache
            var serversList = ((IEnumerable<Server>)cache.Get("servers")).ToList();

            serversList.Add(newServer);

            // insert the list to the cache
            cache.Set("servers", serversList);
        }

        public void deleteServer(string serverID)
        {
            // get the list from the cache
            var serversList = ((IEnumerable<Server>)cache.Get("servers")).ToList();

            Server s = serversList.Where(x => String.Equals(x.ServerID, serverID)).FirstOrDefault();
            if (s != null)
                serversList.Remove(s);

            // insert the list to the cache
            cache.Set("servers", serversList);
        }

        public IEnumerable<Server> getAllExternalServers()
        {
            // get the list from the cache
            var serversList = ((IEnumerable<Server>)cache.Get("servers")).ToList();

            return serversList;
        }

        public Server getServer(string serverID)
        {
            // get the list from the cache
            var serversList = ((IEnumerable<Server>)cache.Get("servers")).ToList();
            Server s = serversList.Where(x => String.Equals(x.ServerID, serverID)).FirstOrDefault();

            return s;
        }
    }
}