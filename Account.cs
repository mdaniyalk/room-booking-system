using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingSystem
{
    class Account
    {
        public string username {get; set;}
    }

    class Profile : Account
    {
        public string name { get; set;} 
        public string idNumber { get; set;}
    }

    class UserProfile
    {
        public string id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string fullname { get; set; }
        public string nim { get; set; }

    }
}
