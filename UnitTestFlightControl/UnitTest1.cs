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
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1(IMemoryCache memoryCache)
        {
            IFlightManager flightManager = new FlightManager(memoryCache);
            DateTime currTime = DateTime.Now;

            // create new flight
            FlightPlan newFlightPlan = new FlightPlan { Passengers = 0, CompanyName = "CheckCompanyName", InitialLocation = new InitialLocation { Longitude = 50.244, Latitude = 40.244 } };

            // get the new flight list
            //Flight getFlight = flightManager.isOccuringAtDateTime(newFlightPlan, currTime);

            //Assert.AreEqual(getFlight, null);
        }
    }
}