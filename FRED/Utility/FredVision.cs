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
using System.Text;

namespace FRED.Utility
{
    public class FredVision
    {
        private HttpClient client = new HttpClient();
        private string data = null;        
        private JsonNinja jNinja;

        private string badPic;

        private void SetData(string data) { this.data = data; }
        public string GetData() { return data; }

        // GetPerson()
        private List<string> faces = new List<string>();
        private List<string> names = new List<string>();
        public void ClearNames() { this.names.Clear(); }
        public List<string> GetNames() { return names; }
        

        private string name;
        private void SetName(string name) { this.name = name; }
        public string GetName() { return name; }

        public async Task GetVision(string type, string personID)
        {
            client.DefaultRequestHeaders.Clear();
            string serverIP = Program.Temp.GetIP();            
            var uri = "";
            
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            
            // Request headers and parameters
            switch (type)
            {
                case "describe":
                    {
                        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "266478d1a0f44c8abb164af3c768a48d");
                        queryString["maxCandidates"] = "1";
                        uri = "https://eastus.api.cognitive.microsoft.com/vision/v1.0/describe?" + queryString;
                        break;
                    }
                case "read":
                    {
                        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "266478d1a0f44c8abb164af3c768a48d");
                        queryString["language"] = "unk";
                        queryString["detectOrientation "] = "true";
                        uri = "https://eastus.api.cognitive.microsoft.com/vision/v1.0/ocr?" + queryString;
                        break;
                    }
                case "emotion":
                    {
                        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e97b9e9e76314914b139b73a8a2148cb");
                        uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?" + queryString;
                        break;
                    }
                case "detect":
                    {
                        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e97b9e9e76314914b139b73a8a2148cb");
                        queryString["returnFaceId"] = "true";
                        queryString["returnFaceLandmarks"] = "false";
                        //queryString["returnFaceAttributes"] = "false";
                        uri = "https://eastus.api.cognitive.microsoft.com/face/v1.0/detect?" + queryString;
                        break;
                    }
                case "addFace":
                    {
                        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e97b9e9e76314914b139b73a8a2148cb");
                        //personID = personID.Substring(1, personID.Length - 2);
                        uri = "https://eastus.api.cognitive.microsoft.com/face/v1.0/persongroups/1111/persons/" + personID + "/persistedFaces?" + queryString;
                        break;
                    }
            }
                                    
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
                SetData(data);
            }
            else
            {
                if (type == "addFace")
                    badPic = "The picture was bad please take another one";
            }
            
        }

        public string FredSees()
        {
            string fredSees = "";
            JsonObject jsonDoc = (JsonObject)JsonValue.Parse(data);
            JsonArray jsonArray = (JsonArray)jsonDoc["description"]["captions"];

            foreach (JsonObject obj in jsonArray)
            {
                JsonValue text;
                obj.TryGetValue("text", out text);
                fredSees = text.ToString();
            }
            return fredSees;
        }
               
        public string FredReads()
        {            
            List<string> tempWords = new List<string>();
            List<string> words = new List<string>();

            jNinja = new JsonNinja(data);
            string filterCollection = jNinja.GetInfo("\"regions\"");
            jNinja = new JsonNinja(filterCollection);
            filterCollection = jNinja.GetInfo("\"lines\"");
            jNinja = new JsonNinja(filterCollection);
            List<string> filterCollections = jNinja.GetInfoList("\"words\"");

            for (int i = 0; i < filterCollections.Count; i++)
            {
                jNinja = new JsonNinja(filterCollections[i]);
                tempWords = jNinja.GetInfoList("\"text\"");
                foreach (string word in tempWords)
                {
                    words.Add(word);
                }               
            }

            string fredReads = string.Join(" ", words);

            return fredReads;
        }     
             
        public async Task DetectFace()
        {
            client.DefaultRequestHeaders.Clear();
            List<string> faceIDs = new List<string>();
            
            JsonArray jsonArray = (JsonArray)JsonValue.Parse(data);
            foreach (JsonObject obj in jsonArray)
            {
                JsonValue id;
                obj.TryGetValue("faceId", out id);
                faceIDs.Add(id.ToString());
            }
            
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e97b9e9e76314914b139b73a8a2148cb");

            var uri = "https://eastus.api.cognitive.microsoft.com/face/v1.0/identify?" + queryString;

            HttpResponseMessage response;

            // Request body
            if (faceIDs.Count > 0)
            {
                string faceID = "";
                if (faceIDs.Count != 1)
                {
                    foreach (string face in faceIDs)
                    {
                        faceID += face + ",";
                    }
                }
                else
                {
                    faceID = faceIDs[0];
                }
                               
                byte[] byteData = Encoding.UTF8.GetBytes("{\"PersonGroupId\": \"1111\", \"faceIds\": [" +
                                                        faceID + "]," +
                                                        "\"maxNumOfCandidatesReturned\": 1," +
                                                        "\"confidenceThreshold\": 0.5}");

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
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
                                
                jNinja = new JsonNinja(data);
                List<string> filterCollections = jNinja.GetInfoList("\"candidates\"");
                for (int i = 0; i < filterCollections.Count; i++)
                {
                    jNinja = new JsonNinja(filterCollections[i]);
                    if (filterCollections[i] == "")
                        names.Add("dont recognize");
                    else
                        faces.Add(jNinja.GetInfo("\"personId\""));
                }
                
                
                foreach (string face in faces)
                {
                    await GetPerson(face);
                }
                faces.Clear();
            }
            else
            {
                faces.Clear();
                names.Add("no face");
                //SetName("no face");
            }

            

        }//DectectFace

        public async Task GetPerson(string personID)
        {
            client.DefaultRequestHeaders.Clear();
            string personsName;            
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e97b9e9e76314914b139b73a8a2148cb");

            if (personID != "")
            {
                personID = personID.Substring(1, 36);

                var uri = "https://eastus.api.cognitive.microsoft.com/face/v1.0/persongroups/1111/persons/" +
                    personID + "?" + queryString;

                var response = await client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    JsonObject jsonDoc = (JsonObject)JsonValue.Parse(data);
                    JsonValue name;
                    jsonDoc.TryGetValue("name", out name);
                    personsName = name.ToString();
                    personsName = personsName.Substring(1, personsName.Length - 2);
                    names.Add(personsName);
                    //SetName(personsName);
                }
                else
                {
                    // do nothing yet
                }               
                
            }
            else
            {
                //faces.Add("dont recgonize");
                SetName("dont recgonize");
            }
            
        }// GetPerson()

        public async Task GetFacesList()
        {
            client.DefaultRequestHeaders.Clear();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e97b9e9e76314914b139b73a8a2148cb");
            
            var uri = "https://eastus.api.cognitive.microsoft.com/face/v1.0/persongroups/1111/persons?" + queryString;

            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsStringAsync();
                List<string> nams = new List<string>();
                List<string> descriptions = new List<string>();
                List<int> faceCounts = new List<int>();
                List<string> personIDs = new List<string>();

                JsonArray jsonArray = (JsonArray)JsonValue.Parse(data);
                foreach (JsonObject obj in jsonArray)
                {
                    JsonValue nam;
                    JsonValue desc;
                    JsonValue ids;
                    JsonValue personID;
                    obj.TryGetValue("name", out nam);
                    obj.TryGetValue("userData", out desc);
                    obj.TryGetValue("personId", out personID);
                    nams.Add(nam.ToString());
                    descriptions.Add(desc.ToString());
                    string temp = personID.ToString().Substring(1, personID.ToString().Length - 2);
                    personIDs.Add(temp);

                    obj.TryGetValue("persistedFaceIds", out ids);
                    JsonArray face = (JsonArray)ids;
                    int count = 0;
                    foreach (string id in face)
                    {
                        count++;
                    }
                    faceCounts.Add(count);
                }

                Program.Temp.SetListNames(nams);
                Program.Temp.SetListDesc(descriptions);
                Program.Temp.SetListFaceCounts(faceCounts);
                Program.Temp.SetListPersonIDs(personIDs);
            }
            else
            {
                // do nothing yet
            }
                        
            await GetTrainingStatus();
        }

        public async Task CreatePerson(string name, string desc)
        {
            client.DefaultRequestHeaders.Clear();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e97b9e9e76314914b139b73a8a2148cb");

            var uri = "https://eastus.api.cognitive.microsoft.com/face/v1.0/persongroups/1111/persons?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{\"name\": \"" + name + "\"," +
                                                "\"userData\": \"" + desc + "\"}");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }            
        }

        public async Task DeletePerson(string personID)
        {
            client.DefaultRequestHeaders.Clear();
            var queryString = HttpUtility.ParseQueryString(string.Empty);          
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e97b9e9e76314914b139b73a8a2148cb");
            var uri = "https://eastus.api.cognitive.microsoft.com/face/v1.0/persongroups/1111/persons/"
                + personID + "?" + queryString;

            var response = await client.DeleteAsync(uri);            
        }

        public async Task GetTrainingStatus()
        {
            client.DefaultRequestHeaders.Clear();
            var queryString = HttpUtility.ParseQueryString(string.Empty);            
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e97b9e9e76314914b139b73a8a2148cb");
            var uri = "https://eastus.api.cognitive.microsoft.com/face/v1.0/persongroups/1111/training?" + queryString;
            var response = await client.GetAsync(uri);
            
            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsStringAsync();
                JsonObject jsonDoc = (JsonObject)JsonValue.Parse(data);
                JsonValue stat;
                jsonDoc.TryGetValue("status", out stat);
                string status = stat.ToString();
                status = status.Substring(1, status.Length - 2);
                Program.Temp.SetTrainingStatus(status);
            }
            else
            {
                // do nothing yet
            }                  
            
        }

        public async Task TrainFace()
        {
            client.DefaultRequestHeaders.Clear();
            var queryString = HttpUtility.ParseQueryString(string.Empty);                        
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "e97b9e9e76314914b139b73a8a2148cb");
            var uri = "https://eastus.api.cognitive.microsoft.com/face/v1.0/persongroups/1111/train?" + queryString;
            HttpResponseMessage response;
            
            byte[] byteData = Encoding.UTF8.GetBytes("");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await client.PostAsync(uri, content);
            }            
        }
    }
}
