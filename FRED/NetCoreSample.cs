using System;
using System.Threading;
using NetCoreAudio;

namespace DemoHarnessUpd
{
    public class NetCoreSample
    {
        public static Player player = new Player();
        public static void Audio(string path)
        {
            player.PlaybackFinished += OnPlaybackFinished;

                try
                {
                    player.Play(path).Wait();
                    while(player.Playing)
                    {
                        Thread.Sleep(1000);
                    }
                    //Console.WriteLine(player.Playing ? "Playback started" : "Could not start the playback");
                    player.Stop().Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
        }

        private static void OnPlaybackFinished(object sender, EventArgs e)
        {
            //Console.WriteLine("Playback finished");
        }
    }
}