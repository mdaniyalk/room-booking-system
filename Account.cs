using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace room_booking_system
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
}
