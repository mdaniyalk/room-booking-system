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
using System.Text;

//using Newtonsoft.Json;


namespace RoomBookingSystem
{
    public class FunctionsClass
    {
        public string apiUrl = "https://ruangapi20221223140142.azurewebsites.net";
        
        public async Task<int> checkRoomTaken(string date, string roomName, string time)
        {
            var client = new HttpClient();

            var result = await client.GetStringAsync(apiUrl + "/RuangBooking?date=" + date + "&room=" + roomName);

            var userdata = JsonSerializer.Deserialize<List<Room>>(result);

            foreach (var roomStatus in userdata)
            {
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
                
            }

            return 1;

            
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
                _02 = "1";
            }
            else
            {
                string[] ret = { "0", "error" };
                return ret;
            }

            var data = new Dictionary<string, string>
            {
                {"bookId", "0"},
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

            var jsonDictionary = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonDictionary, Encoding.UTF8, "application/json");

            var result = await client.PostAsync(apiUrl+"/RuangBooking", content);

            //var content = await result.Content.ReadAsStringAsync();
            //Console.WriteLine(content);
            result.EnsureSuccessStatusCode();

            if (result.IsSuccessStatusCode)
            {
                var res = await client.GetStringAsync(apiUrl + "/RuangBooking?date="+date+"&room="+room);
                var userdata = JsonSerializer.Deserialize<List<Booking>>(res);
                string bookId = "";
                foreach (var book in userdata)
                {
                    if (book.name == name && book.idNumber == name && book.purpose == purpose)
                    {
                        bookId = book.bookId;
                    }
                }
                
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

            var jsonDictionary = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonDictionary, Encoding.UTF8, "application/json");
            
            var result = await client.PutAsync(apiUrl + "/RuangBooking", content);

            //var content = await result.Content.ReadAsStringAsync();
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

            var result = await client.DeleteAsync(apiUrl + "/RuangBooking?bookId=" + bookId + "&idNumber=" +idNumber);

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

            var result = await client.GetAsync(apiUrl + "/RuangBooking?bookId=" + bookId);

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

        public async Task viewBooking(string bookId)
        {
            var client = new HttpClient();
            var result = await client.GetAsync(apiUrl + "/RuangBooking?bookId=" + bookId);

            

            //Console.WriteLine(content);
            result.EnsureSuccessStatusCode();

            if (result.IsSuccessStatusCode)
            {
                var content = await client.GetStringAsync(apiUrl + "/RuangBooking?bookId=" + bookId);
                Booking book = JsonSerializer.Deserialize<Booking>(content);
                string time = "";
                if (book._00 == "1")
                {
                    time = "08:00-10:00";
                } 
                else if (book._01 == "1")
                {
                    time = "10:15-12:15";
                }
                else if (book._02 == "1")
                {
                    time = "13:00-15:00";
                }

                new PopupMessage("Booking ID: \t" + book.bookId + "\nName: \t" + book.name +
                    "\nID Number: \t" + book.idNumber + "\nRoom: \t" + book.room + "\nDate: \t" + book.date +
                    "\nTime: \t" + time + "\nPurpose: \t" + book.purpose + "\nNumber of Person: \t" + book.numPerson).ShowDialog();
            }
            else
            {
                new PopupMessage("Booking not found!").ShowDialog();
            }
        }

        public async Task<int> checkUserTakenAsync(string uName)
        {
            var client = new HttpClient();

            var result = await client.GetStringAsync(apiUrl+"/Users");
            //Console.WriteLine(result.StatusCode);
            var userdata = JsonSerializer.Deserialize<List<UserProfile>>(result);

            foreach (var data in userdata)
            {
                if (data.username == uName)
                {
                    return 0;
                }
            }

            return 1;
            
        }

        public async Task<int> newUser(string name, string idNum, string uName, string password)
        {
            var client = new HttpClient();
            string email = uName + "@mail.ugm.ac.id";
            var data = new Dictionary<string, string>
            {
                {"id", "0" },
                {"username", uName},
                {"password", password},
                {"email", email},
                {"fullname", name},
                {"nim", idNum}
            };
            var jsonDictionary = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonDictionary, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(apiUrl+"/Users", content);

            //var content = await result.Content.ReadAsStringAsync();
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

            var result = await client.GetAsync(apiUrl + "/auth/Login?username=" + uName +"&password=" + pass);
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
