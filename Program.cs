using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PiLed.Devices.Implementations;
using PiLed.Display;
using PiLed.Models;
using System;

namespace infinityTableWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            try
            {
                SetupPiLeds();
            }
            catch (Exception ex)
            { }

            Console.WriteLine("Setup Pi, running host builder");
            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //webBuilder.UseUrls("http://192.168.0.29:5000");
                });

        public static void SetupPiLeds()
        {
            var numLeds = 50;
            var device = new WS2801PixelDevice(new PiLed.Devices.Config.PixelConfig() { FlushRate = 10, NumLeds = numLeds });
            //var demoMode = new DemoMode(device);
            //demoMode.Start();
            var singleColorMode = new SolidColorDisplay(device, new PixelColor(120, 1, 1));
            singleColorMode.Start();
        }
    }
}
