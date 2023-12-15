using PiLed.Devices.Implementations;
using PiLed.Display;
using PiLed.Models;
using PiLed;
using System;
using System.Diagnostics;

using System.Drawing;
using static PiLed.Display.RainbowColorDisplay;

namespace blazorSite.Data;

    public class PiLedService
    {
        private readonly WS2801PixelDevice _device;

        private CancellationTokenSource ctSource;
        private bool throbState = false;
        private bool isInitialized = false;

        public PiLedService()
        {
            try
            {
                var numLeds = 50;
                _device = new WS2801PixelDevice(new PiLed.Devices.Config.PixelConfig() { FlushRate = 10, NumLeds = numLeds });
            }
            catch{ }
        }

        public void Initialize()
        {
            if(!isInitialized){
                // Console.WriteLine("Initializing");
                var defaultColor = new SolidColorDisplay(_device, new PixelColor(0, 0, 0));
                defaultColor.Start();
                isInitialized = !isInitialized;
                // Console.WriteLine("Generating new toekn from Init");
                RunDemo();
            }
        }

        public void RunDemo(){
            CancelCurrentToken();
            var token  = GenerateNewToken();

            Task.Run(() => DemoMode(token), token);
        }

        private CancellationToken GenerateNewToken(){
            ctSource = new CancellationTokenSource();
            return ctSource.Token;
        }

        private void CancelCurrentToken(){
            if(ctSource != null){
                ctSource.Cancel();
            }
        }

        public void SetColor(string color)
        {
            CancelCurrentToken();
            if (color.Equals("blank", StringComparison.InvariantCultureIgnoreCase))
            {
                //  Console.WriteLine("Setting color of blank");
                var singleColorMode = new SolidColorDisplay(_device, new PixelColor(0, 0, 0));
                singleColorMode.Start();
            }
        }

        public void SetThrobColorFromHex(string hexColor)
        {
            CancelCurrentToken();

            // Console.WriteLine("Setting throb color from hexValue: " + hexColor);```
            Color color = ColorTranslator.FromHtml(hexColor);
            double hue;
            double saturation;
            double value;
            ColorToHSV(color, out hue, out saturation, out value); 
            var throbColorMode = new ThrobColorDisplay(_device, new PixelColor(hue, 1.0, 1.0));

            // Console.WriteLine("Generating new toekn from SetThrobColor");
            var token  = GenerateNewToken();
            throbState = true;
            Task.Run(() => throbColorMode.Start(token), token);
        }

        public void CancelCurrentMode(){
            // Console.WriteLine("Cancelling current mode and setting throb to false");
            CancelCurrentToken();
            throbState = false;
        }

        public void SetColorFromHex(string hexColor){

            // Console.WriteLine("Cacnelling token at the top of SetColorFromHex");

            CancelCurrentToken();
            
            if(throbState){
                SetThrobColorFromHex(hexColor);
            }
            else{
                // Console.WriteLine("Starting a single color from hex value: "+ hexColor);
                Color color = ColorTranslator.FromHtml(hexColor);
                double hue;
                double saturation;
                double value;
                ColorToHSV(color, out hue, out saturation, out value); 

                SetColor(hue, 1.0, 1.0);
            }
        }

        public void SetRainbow(string rainbowType){
            // Console.WriteLine("Cancelling current mode from rainbow method");
            CancelCurrentMode();
            // Console.WriteLine("Generating new toekn from Rainbow");
            var token  = GenerateNewToken();
            // Console.WriteLine("Starting a new rainbow");
            RainbowType type;
            if(rainbowType == "Led"){
                type = RainbowType.Led;
            }
            else{
                type = RainbowType.ColorWheel;
            }
            var rainbowMode = new RainbowColorDisplay(_device, type);
            Task.Run(() => rainbowMode.Start(token), token);
        }

        private void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public void SetColor(double hue, double saturation, double value)
        {
            // Console.WriteLine("Setting a solid color only");
            var singleColorMode = new SolidColorDisplay(_device, new PixelColor(hue, saturation, value));
            singleColorMode.Start();
        }

        public void DemoMode(CancellationToken demoToken = default){
            List<IPixelDisplay> _displays = new List<IPixelDisplay>
            {
                new ThrobColorDisplay(_device, new PixelColor(0, 1, 1)),
                new RainbowColorDisplay(_device, RainbowType.Led),
                new ThrobColorDisplay(_device, new PixelColor(120, 1, 1)),
                new RainbowColorDisplay(_device, RainbowType.ColorWheel),
                new ThrobColorDisplay(_device, new PixelColor(240, 1, 1)),
                new RainbowColorDisplay(_device, RainbowType.Led)
            };

             var iterator = 0;

            while (!demoToken.IsCancellationRequested)
            {
                // Console.WriteLine("Starting a new demo mode color");
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                var task = Task.Run(() => _displays[iterator].Start(token), token);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                while(sw.ElapsedMilliseconds < 10000 && !demoToken.IsCancellationRequested){}
                // Console.WriteLine("Broke out of Demo task wait after " + sw.ElapsedMilliseconds + "with my token cancellation request of " + demoToken.IsCancellationRequested);
                sw.Stop();

                tokenSource.Cancel();
                iterator = (iterator + 1) % _displays.Count;
            }
        }
    }

