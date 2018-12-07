using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FRED.Utility;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FRED
{
    public class Program
    {
        public static string cs = "Server=(localdb)\\ProjectsV13;Database=FredDB;Trusted_Connection=true;MultipleActiveResultSets=true";

        public static Controller Controller = new Controller();
        public static Password Password = new Password();

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
