using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
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
