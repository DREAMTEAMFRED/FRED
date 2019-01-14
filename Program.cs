using System;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

//install Naudio from Manage Nuget packages
namespace TextToSPeechApp
{
    class Program
    {
        public static NAudio.Wave.WaveFileReader wave = new NAudio.Wave.WaveFileReader(@"sample.wav");
        public static NAudio.Wave.DirectSoundOut output = null;
        static async Task Main(string[] args)
        {
            while (true)
            { 
                    // Prompts the user to input text for TTS conversion
                    Console.Write("What would you like to convert to speech? ");
                string text = Console.ReadLine();

                // Gets an access token
                string accessToken;
                Console.WriteLine("Attempting token exchange. Please wait...\n");

                // Add your subscription key here
                // If your resource isn't in WEST US, change the endpoint
                Authentication auth = new Authentication("https://westus.api.cognitive.microsoft.com/sts/v1.0/issueToken", "de48ef5b15d34f6498fbd831f5d72aec");
                try
                {
                    accessToken = await auth.FetchTokenAsync().ConfigureAwait(false);
                    Console.WriteLine("Successfully obtained an access token. \n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to obtain an access token.");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine(ex.Message);
                    return;
                }

                string host = "https://westus.tts.speech.microsoft.com/cognitiveservices/v1";

                string body = @"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>
                  <voice name='Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)'>" +
                  text + "</voice></speak>";

                using (var client = new HttpClient())
                {
                    using (var request = new HttpRequestMessage())
                    {
                        // Set the HTTP method
                        request.Method = HttpMethod.Post;
                        // Construct the URI
                        request.RequestUri = new Uri(host);
                        // Set the content type header
                        request.Content = new StringContent(body, Encoding.UTF8, "application/ssml+xml");
                        // Set additional header, such as Authorization and User-Agent
                        request.Headers.Add("Authorization", "Bearer " + accessToken);
                        request.Headers.Add("Connection", "Keep-Alive");
                        // Update your resource name
                        request.Headers.Add("User-Agent", "TextToSpeechApp");
                        request.Headers.Add("X-Microsoft-OutputFormat", "riff-24khz-16bit-mono-pcm");
                        // Create a request
                        Console.WriteLine("Calling the TTS service. Please wait... \n");
                        using (var response = await client.SendAsync(request).ConfigureAwait(false))
                        {
                            response.EnsureSuccessStatusCode();
                            // Asynchronously read the response
                            using (var dataStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                            {
                                Console.WriteLine("Your speech file is being written to file...");
                                wave.Close();
                                using (var fileStream = new FileStream(@"sample.wav", FileMode.Create, FileAccess.Write, FileShare.Write))
                                {
                                    await dataStream.CopyToAsync(fileStream).ConfigureAwait(false);
                                    fileStream.Close();
                                }
                                Console.WriteLine("\nYour file is ready. Press any key to exit.");
                                PlaySound();
                                //Console.ReadLine();
                            }
                        }
                    }
                }
            }
        }

        public static void PlaySound()
        {
            wave = new NAudio.Wave.WaveFileReader(@"sample.wav");
            output = new NAudio.Wave.DirectSoundOut();
            output.Init(new NAudio.Wave.WaveChannel32(wave));
            output.Play();
            if (output.PlaybackState == NAudio.Wave.PlaybackState.Stopped)
            {
                output.Stop();
                output.Dispose();
                //wave.Dispose();
                wave.Close();
                wave = null;
                output = null;
            }
        }
    }
}
