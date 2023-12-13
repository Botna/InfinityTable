using PiLed.Devices.Implementations;
using PiLed.Display;
using PiLed.Models;
using System;

namespace infinityTableWebsite.Services
{
    public interface IPiLedService
    {
        void SetColor(string color);
        void SetColor(int hue, int saturation, int value); 
    }
    public class PiLedService : IPiLedService
    {
        private readonly WS2801PixelDevice _device;
        public PiLedService()
        {
            try
            {
                var numLeds = 50;
                _device = new WS2801PixelDevice(new PiLed.Devices.Config.PixelConfig() { FlushRate = 10, NumLeds = numLeds });
                //var demoMode = new DemoMode(device);
                //demoMode.Start();
                //var singleColorMode = new SolidColorDisplay(device, new PixelColor(120, 1, 1));
                //singleColorMode.Start();
            }
            catch{ }
        }

        public void SetColor(string color)
        {
            color = color ?? string.Empty;
            if(color.Equals("blue", StringComparison.InvariantCultureIgnoreCase))
            {
                var singleColorMode = new SolidColorDisplay(_device, new PixelColor(240, 1, 1));
                singleColorMode.Start();
            }
            if (color.Equals("green", StringComparison.InvariantCultureIgnoreCase))
            {
                var singleColorMode = new SolidColorDisplay(_device, new PixelColor(120, 1, 1));
                singleColorMode.Start();
            }
            if (color.Equals("red", StringComparison.InvariantCultureIgnoreCase))
            {
                var singleColorMode = new SolidColorDisplay(_device, new PixelColor(0, 1, 1));
                singleColorMode.Start();
            }
            if (color.Equals("blank", StringComparison.InvariantCultureIgnoreCase))
            {
                var singleColorMode = new SolidColorDisplay(_device, new PixelColor(0, 0, 0));
                singleColorMode.Start();
            }
        }

        public void SetColor(int hue, int saturation, int value)
        {
            var singleColorMode = new SolidColorDisplay(_device, new PixelColor(hue, saturation, value));
            singleColorMode.Start();
        }
    }
}
