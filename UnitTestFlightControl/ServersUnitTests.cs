using FlightControlWeb.Model.ConcreteObjects;
using FlightControlWeb.Model.Interfaces;
using FlightControlWeb.Model.Managers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
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
            // creating the string that represents the json server file
            string serJson = "{'ServerId': '1',  'ServerURL': 'http://test.com'}";

            // mocking the server's GetAllExternalServers function
            Mock<IServerManager> Server_Mock = new Mock<IServerManager>();
            Server server = JsonConvert.DeserializeObject<Server>(serJson);
            List<Server> serversList = new List<Server>(1) { server };
            Server_Mock.Setup(x => x.GetAllExternalServers()).Returns(serversList);

            // what we expect to get and what the actual result
            var actual = (List<Server>)Server_Mock.Object.GetAllExternalServers();
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
            // creating the string that represents the json server file
            string serJson = "{'ServerId': '1',  'ServerURL': 'http://test.com'}";

            // mocking the server's GetAllExternalServers function
            Mock<IServerManager> Server_Mock = new Mock<IServerManager>();
            Server server = JsonConvert.DeserializeObject<Server>(serJson);
            Server_Mock.Setup(x => x.GetServer(server.ServerID)).Returns(server);

            // what we expect to get and what the actual result
            var actual = Server_Mock.Object.GetServer("1");
            var expected = server;
            var fakeServer = new Server { ServerID = "2", ServerURL = "http://fake.com" };

            Assert.AreEqual(actual, expected);
            Assert.AreNotEqual(actual, fakeServer);
        }

        [TestMethod]
        public void ShouldAddNewServer()
        {
            // creating the string that represents the json server file
            string serJson = "{'ServerId': '1',  'ServerURL': 'http://test.com'}";

            // mocking the server's GetAllExternalServers function
            Mock<IServerManager> Server_Mock = new Mock<IServerManager>();
            Server server = JsonConvert.DeserializeObject<Server>(serJson);
            Server_Mock.Setup(x => x.AddNewServer(server));
            Server_Mock.Setup(x => x.GetServer(server.ServerID)).Returns(server);

            // what we expect to get and what the actual result
            Server_Mock.Object.AddNewServer(server);
            var actual = Server_Mock.Object.GetServer("1");
            var expected = server;
            var fakeServer = new Server { ServerID = "2", ServerURL = "http://fake.com" };

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
            serverManager.AddNewServer(newServer);

            // deleting the server we just added
            serverManager.DeleteServer(newServer.ServerID);

            var actual = (List<Server>)serverManager.GetAllExternalServers();

            // the list should be empty
            Assert.IsTrue(actual != null);
            Assert.AreEqual(actual.Count, 0);
            Assert.AreNotEqual(actual.Count, 1);
        }
    }
}