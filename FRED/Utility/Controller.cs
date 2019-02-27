using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FRED.Utility
{
    public class Controller
    {
        public int UserID { get; set; }
        public bool IsVerified { get; set; }
        
        public List<string> aliasList = new List<string>();
        public List<string> externalIPs = new List<string>();
        public List<string> internalIPs = new List<string>();
        public List<string> deviceStatusList = new List<string>();

        public string GetIP()
        {
            if (deviceStatusList[0] == "active")
                return internalIPs[0];
            else
                return "inactive";
        }

        public string GetDeviceStatus()
        {
            return deviceStatusList[0];
        }


        public void CheckID(string username, string password)
        {
            UserID = 0;
            using (SqlConnection myConn = new SqlConnection(Program.cs))
            {
                SqlCommand login = new SqlCommand();
                login.Connection = myConn;
                myConn.Open();
                login.Parameters.AddWithValue("@username", username);
                login.CommandText = ("[spLogin]");
                login.CommandType = System.Data.CommandType.StoredProcedure;
                var result = login.ExecuteScalar();

                if (result != null)
                {
                    // correct username check password
                    UserID = Convert.ToInt32(result);
                    SqlCommand verPass = new SqlCommand();
                    verPass.Connection = myConn;
                    verPass.Parameters.AddWithValue("@userID", UserID);
                    verPass.CommandText = ("[spVerPass]");
                    verPass.CommandType = System.Data.CommandType.StoredProcedure;
                    var pass = verPass.ExecuteScalar();
                    string hashPassword = Convert.ToString(pass);

                    if (Program.Password.Verify(password, hashPassword))
                    {
                        IsVerified = true;
                    }
                    else
                    {
                        UserID = 0;
                        IsVerified = false; // wrong password
                    }
                }
                else
                {
                    IsVerified = false;  // wrong username
                }

                myConn.Close();
            }
        }//checkID()

        public string GetRemoteToken()
        {
            string jsonString = "";
            string url = "https://api.remot3.it/apv/v27/user/login";

            HttpClient client = new HttpClient();
            HttpRequestMessage requestData = new HttpRequestMessage();

            //  Configure the HTTP requests's url, headers, and body
            requestData.Method = HttpMethod.Post;
            requestData.RequestUri = new Uri(url);
            requestData.Headers.Add("developerkey", Environment.GetEnvironmentVariable("REMOTEIT_DEVELOPER_KEY", EnvironmentVariableTarget.User));

            Dictionary<string, string> bodyData = new Dictionary<string, string>() {
                {"password", Environment.GetEnvironmentVariable("REMOTEIT_PASSWORD", EnvironmentVariableTarget.User) },
                {"username", Environment.GetEnvironmentVariable("REMOTEIT_USERNAME", EnvironmentVariableTarget.User) }
            };

            string jsonFormattedBody = JsonConvert.SerializeObject(bodyData);
            requestData.Content = new StringContent(jsonFormattedBody);

            try
            {
                // Send the HTTP request and run the inner block upon recieveing a response
                var response = client.SendAsync(requestData).ContinueWith((taskMessage) =>
                {
                    var result = taskMessage.Result;
                    var jsonTask = result.Content.ReadAsStringAsync();
                    jsonTask.Wait();

                    // Store the body of API response
                    jsonString = jsonTask.Result;
                });
                response.Wait();
            }
            catch (HttpRequestException e)
            {
                // Triggered when the API returns a non-200 response code
                jsonString = e.Message;
            }
            
            JsonObject jsonDoc = (JsonObject)JsonValue.Parse(jsonString);
            jsonDoc.TryGetValue("token", out JsonValue token);
            string authToken = token.ToString();
            authToken = authToken.Substring(1, authToken.Length - 2);

            return authToken;
        }

        public void GetDeviceList()
        {
            string jsonString = "";
            string url = "https://api.remot3.it/apv/v27/device/list/all";

            HttpClient client = new HttpClient();
            HttpRequestMessage requestData = new HttpRequestMessage();

            //  Configure the HTTP requests's url, headers, and body
            requestData.Method = HttpMethod.Get;
            requestData.RequestUri = new Uri(url);

            requestData.Headers.Add("developerkey", Environment.GetEnvironmentVariable("REMOTEIT_DEVELOPER_KEY", EnvironmentVariableTarget.User));
            requestData.Headers.Add("token", GetRemoteToken());

            try
            {
                // Send the HTTP request and run the inner block upon recieveing a response
                var response = client.SendAsync(requestData).ContinueWith((taskMessage) =>
                {
                    var result = taskMessage.Result;
                    var jsonTask = result.Content.ReadAsStringAsync();
                    jsonTask.Wait();

                    // Store the body of API response
                    jsonString = jsonTask.Result;
                });
                response.Wait();
            }
            catch (HttpRequestException e)
            {
                // Triggered when the API returns a non-200 response code
                jsonString = e.Message;
            }

            JsonObject jsonDoc = (JsonObject)JsonValue.Parse(jsonString);
            JsonArray jsonArray = (JsonArray)jsonDoc["devices"];
            string temp;

            foreach (JsonObject obj in jsonArray)
            {                
                obj.TryGetValue("devicealias", out JsonValue devicealias);
                temp = devicealias.ToString();
                aliasList.Add(temp.Substring(1, temp.Length - 2));
                obj.TryGetValue("devicelastip", out JsonValue devicelastip);
                temp = devicelastip.ToString();
                externalIPs.Add(temp.Substring(1, temp.Length - 2));
                obj.TryGetValue("lastinternalip", out JsonValue lastinternalip);
                temp = lastinternalip.ToString();
                internalIPs.Add(temp.Substring(1, temp.Length - 2));
                obj.TryGetValue("devicestate", out JsonValue devicestate);
                temp = devicestate.ToString();
                deviceStatusList.Add(temp.Substring(1, temp.Length - 2));                
            }

        }
    }
}
