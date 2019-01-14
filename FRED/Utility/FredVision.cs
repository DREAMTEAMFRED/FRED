using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Json;
using System.IO;
using System.Net;
using System.Net.Http.Headers;

namespace FRED.Utility
{
    public class FredVision
    {
        public static string fredSees;
        
        public async Task GetDescription()
        {
            string serverIP = Program.Temp.GetIP();
            string data = null;
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "266478d1a0f44c8abb164af3c768a48d");

            // Request parameters
            queryString["maxCandidates"] = "1";
            var uri = "https://eastus.api.cognitive.microsoft.com/vision/v1.0/describe?" + queryString;
                        
            // download image from web cam  
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile("http://" + serverIP + ":8080/?action=snapshot", "fredSees.jpg");
            }            
            byte[] img = File.ReadAllBytes("fredSees.jpg");
                                        
            HttpResponseMessage response;

            // Request body
            using (var content = new ByteArrayContent(img))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
            }

            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsStringAsync();
            }
            else
            {
                // do nothing yet
            }

            // read json data 
            JsonObject jsonDoc = (JsonObject)JsonValue.Parse(data);
            JsonArray jsonArray = (JsonArray)jsonDoc["description"]["captions"];

            foreach (JsonObject obj in jsonArray)
            {
                JsonValue text;
                obj.TryGetValue("text", out text);
                fredSees = text.ToString();
            }            
        }

        public string FredSees()
        {            
            return fredSees;
        }
    }
}
