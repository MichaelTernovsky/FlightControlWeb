using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Model
{
    public class Server
    {
        // fields
        private string serverID;
        private string serverURL;

        // CTOR
        public Server(string serverID, string serverURL)
        {
            this.serverID = serverID;
            this.serverURL = serverURL;
        }

        // Properties
        public string ServerID
        {
            get
            {
                return this.serverID;
            }
            set
            {
                this.serverID = value;
            }
        }

        public string ServerURL
        {
            get
            {
                return this.serverURL;
            }
            set
            {
                this.serverURL = value;
            }
        }
    }
}