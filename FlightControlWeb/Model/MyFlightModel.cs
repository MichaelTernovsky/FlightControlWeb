using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    public class MyFlightModel : IFlightModel
    {
        // the list of servers
        private List<Server> serversList = new List<Server>();

        public void addNewServer(string serverID, string serverURL)
        {
            this.serversList.Add(new Server(serverID, serverURL));
        }

        public void deleteServer(string serverID)
        {
            foreach (Server s in serversList)
            {
                if (String.Equals(s.ServerID, serverID))
                {
                    this.serversList.Remove(s);
                }
            }
        }

        public IEnumerable<Server> getAllExternalServers()
        {
            return this.serversList;
        }

        public Server getServer(string serverID)
        {
            foreach (Server s in serversList)
            {
                if (String.Equals(s.ServerID, serverID))
                {
                    return s;
                }
            }
            return null;
        }
    }
}