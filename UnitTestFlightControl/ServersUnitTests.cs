using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using FlightControlWeb.Model.Managers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTestFlightControl
{
    [TestClass]
    public class ServersUnitTests
    {
        [TestMethod]
        public void ShouldReturnServersList()
        {
            // creating the list of servers
            List<Server> serversList = new List<Server>();

            // creating new server
            Server newServer1 = new Server { ServerID = "id1", ServerURL = "url1" };
            Server newServer2 = new Server { ServerID = "id2", ServerURL = "url2" };

            // adding the servers to the list
            serversList.Add(newServer1);
            serversList.Add(newServer2);
            // creating server manager object
            IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
            // adding the lists to the cache
            _cache.Set("servers", serversList);
            // creating the manager
            IServerManager serverManager = new ServerManager(_cache);

            var actual = (List<Server>)serverManager.getAllExternalServers();
            var expected = serversList;

            // the list should contain the same number of elements
            Assert.IsTrue(actual != null);
            Assert.AreEqual(actual.Count, expected.Count);

            // the elements in both lists should be the same
            for (int i = 0; i < expected.Count; i++)
                Assert.AreEqual(actual[i].ServerID, expected[i].ServerID);
        }

        [TestMethod]
        public void ShouldReturnServersByID()
        {
            // creating the list of servers
            List<Server> serversList = new List<Server>();

            // creating new server
            Server newServer = new Server { ServerID = "111", ServerURL = "111" };

            // adding the server to the list
            serversList.Add(newServer);

            // creating server manager object
            IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
            // adding the lists to the cache
            _cache.Set("servers", serversList);
            // creating the manager
            IServerManager serverManager = new ServerManager(_cache);

            var actual = serverManager.getServer(newServer.ServerID);
            var expected = newServer;
            var fakeServer = new Server { ServerID = "111", ServerURL = "111" };

            Assert.AreEqual(actual, expected);
            Assert.AreNotEqual(actual, fakeServer);
        }

        [TestMethod]
        public void ShouldAddNewServer()
        {
            // creating the list of servers
            List<Server> serversList = new List<Server>();

            // creating new server
            Server newServer = new Server { ServerID = "111", ServerURL = "111" };

            // creating server manager object
            IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
            // adding the lists to the cache
            _cache.Set("servers", serversList);
            // creating the manager
            IServerManager serverManager = new ServerManager(_cache);

            // adding the server to the list
            serverManager.addNewServer(newServer);

            var actual = serverManager.getServer(newServer.ServerID);
            var expected = newServer;
            var fakeServer = new Server { ServerID = "111", ServerURL = "111" };

            Assert.AreEqual(actual, expected);
            Assert.AreNotEqual(actual, fakeServer);
        }

        [TestMethod]
        public void ShouldDeleteServer()
        {
            // creating the list of servers
            List<Server> serversList = new List<Server>();

            // creating new server
            Server newServer = new Server { ServerID = "111", ServerURL = "111" };

            // creating server manager object
            IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
            // adding the lists to the cache
            _cache.Set("servers", serversList);
            // creating the manager
            IServerManager serverManager = new ServerManager(_cache);

            // adding the server to the list
            serverManager.addNewServer(newServer);

            // deleting the server we just added
            serverManager.deleteServer(newServer.ServerID);

            var actual = (List<Server>)serverManager.getAllExternalServers();

            // the list should be empty
            Assert.IsTrue(actual != null);
            Assert.AreEqual(actual.Count, 0);
            Assert.AreNotEqual(actual.Count, 1);
        }
    }
}