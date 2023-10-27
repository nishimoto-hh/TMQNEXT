using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebTemplate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               //.UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   var env = hostingContext.HostingEnvironment;

                   config.SetBasePath(Directory.GetCurrentDirectory());
                   config.AddJsonFile("connectionsettings.json", optional: true, reloadOnChange: true)
                         .AddJsonFile("connectionsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                         .AddJsonFile("logsettings.json", optional: true, reloadOnChange: true)
                         .AddJsonFile("logsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                         .AddEnvironmentVariables();
               })
               .UseIISIntegration()
               .UseStartup<Startup>();
    }
}
