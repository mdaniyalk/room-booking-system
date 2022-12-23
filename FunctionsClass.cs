using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
//using Newtonsoft.Json;


namespace room_booking_system
{
    class FunctionsClass
    {
        public string apiUrl = "http://localhost:7260";
        
        public async Task<string> listRoom(string id)
        {
            var client = new HttpClient();

            var result = await client.GetStringAsync(apiUrl + "/Ruang/ListKetersediaan");

            Room roomList = JsonSerializer.Deserialize<Room>(result);
            return roomList.room;

        }

        public async Task<int> checkRoomTaken(string date, string roomName, string time)
        {
            var client = new HttpClient();

            var result = await client.GetStringAsync(apiUrl + "/Ruang/Ketersediaan/tanggal=" + date + "&ruang=" + roomName);

            Room roomStatus = JsonSerializer.Deserialize<Room>(result);

            if (time == "00")
            {
                if (roomStatus._00 == "1")
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else if (time == "01")
            {
                if (roomStatus._01 == "1")
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else if (time == "02")
            {
                if (roomStatus._02 == "1")
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            { return 1; }
            
        }

        public async Task<string[]> newBooking(string name, string idNumber, string room, string date, string time, string purpose, string numPerson)
        {
            var client = new HttpClient();
            string _00 = "0";
            string _01 = "0";
            string _02 = "0";
            if (time == "00")
            {
                _00 = "1"; 
            }
            else if (time == "01")
            {
                _01 = "1";
            }
            else if (time == "02")
            {
                _02 = "2";
            }
            else
            {
                string[] ret = { "0", "error" };
                return ret;
            }

            var data = new Dictionary<string, string>
            {
                {"name", name},
                {"idNumber", idNumber},
                {"room", room},
                {"date", date},
                {"_00", _00},
                {"_01", _01},
                {"_02", _02},
                {"purpose", purpose},
                {"numPerson", numPerson},
            };

            var result = await client.PostAsync(apiUrl+"/RuangBooking", new FormUrlEncodedContent(data));

            var content = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(content);
            result.EnsureSuccessStatusCode();

            if (result.IsSuccessStatusCode)
            {
                var res = await client.GetStringAsync(apiUrl + "/RuangBooking/"+ new FormUrlEncodedContent(data));

                Booking book = JsonSerializer.Deserialize<Booking>(res);
                string bookId = book.bookId;
                string[] ret = { "1", bookId};
                return ret;
            }
            else
            {
                string[] ret = { "0", "Error" }; 
                return ret;
            }
        }

        public async Task<int> rescheduleBooking(string bookId, string date, string room, string time)
        {
            var client = new HttpClient();
            string _00 = "0";
            string _01 = "0";
            string _02 = "0";
            if (time == "00")
            {
                _00 = "1";
            }
            else if (time == "01")
            {
                _01 = "1";
            }
            else if (time == "02")
            {
                _02 = "2";
            }
            else
            {
                return 0;
            }

            var data = new Dictionary<string, string>
            {
                {"room", room},
                {"date", date},
                {"_00", _00},
                {"_01", _01},
                {"_02", _02},
                
            };

            var result = await client.PutAsync(apiUrl + "/RuangBooking", new FormUrlEncodedContent(data));

            var content = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(content);
            result.EnsureSuccessStatusCode();

            if (result.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> cancelBooking(string bookId, string idNumber)
        {
            var client = new HttpClient();

            var result = await client.DeleteAsync(apiUrl + "/RuangBooking/bookId=" + bookId + "&idNumber=" +idNumber);

            var content = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(content);
            result.EnsureSuccessStatusCode();

            if (result.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> validateBooking(string bookId)
        {
            var client = new HttpClient();

            var result = await client.GetAsync(apiUrl + "/RuangBooking/bookId=" + bookId);

            var content = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(content);
            result.EnsureSuccessStatusCode();

            if (result.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> checkUserTakenAsync(string uName)
        {
            var client = new HttpClient();

            var result = await client.GetAsync(apiUrl+"/Users/"+uName);
            //Console.WriteLine(result.StatusCode);


            if (result.IsSuccessStatusCode)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public async Task<int> newUser(string name, string idNum, string uName, string password)
        {
            var client = new HttpClient();
            string email = uName + "@mail.ugm.ac.id";
            var data = new Dictionary<string, string>
            {
                {"username", uName},
                {"password", password},
                {"email", email},
                {"fullname", name},
                {"nim", idNum}
            };

            var result = await client.PostAsync(apiUrl, new FormUrlEncodedContent(data));

            var content = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(content);
            result.EnsureSuccessStatusCode();

            if (result.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> validateLogin(string uName, string pass)
        {
            var client = new HttpClient();

            var result = await client.GetAsync(apiUrl + "/auth/Login/username=" + uName +"&password=" + pass);
            //Console.WriteLine(result.StatusCode);


            if (result.IsSuccessStatusCode)
            {
                //var account = await client.GetStringAsync(apiUrl + "/auth/Login/username=" + uName + "&password=" + pass);
                //Profile profile = JsonSerializer.Deserialize<Profile>(account);
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
