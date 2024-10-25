using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace BKD_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });



        //        Host.CreateDefaultBuilder(args)
        //                .ConfigureServices((hostContext, services) =>
        //                {
        //            services.AddHostedService<Worker>();
        //        }).UseWindowsService()
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //    webBuilder.UseUrls("https://*:8081", "http://*:8080");
        //    webBuilder.UseStartup<Startup>();
        //}).UseSerilog();
    }
}
