using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolBlock
{
    class UserInfo
    {
        public int ID {get;set;}
        public string IPAddress {get;set;}
        public string ChannelID {get;set;}
        public string Time {get;set;}
        public DateTime FirstConnect {get;set;}
        public DateTime LastUpdate {get;set;}
        public string Status{get;set;}

        static int localID = 0;

        public UserInfo()
        {
            ID = localID++;
            IPAddress = "127.0.0.1";
            ChannelID = "000000";
        }
    }
}
