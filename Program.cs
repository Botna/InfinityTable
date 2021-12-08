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
            try
            {
                SetupPiLeds();
            }
            catch (Exception ex)
            { }

            CreateHostBuilder(args).Build().Run();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void SetupPiLeds()
        {
            var numLeds = 50;
            var device = new WS2801PixelDevice(new PiLed.Devices.Config.PixelConfig() { FlushRate = 10, NumLeds = numLeds });
            //var demoMode = new DemoMode(device);
            //demoMode.Start();
            var singleColorMode = new SolidColorDisplay(device, new PixelColor(0, 1, 1));
            singleColorMode.Start();
        }
    }
}
