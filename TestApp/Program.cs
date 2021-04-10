using RGB.NET.Core;
using RGB.NET.Devices.OpenRGB;
using RGB.NET.Devices.Wooting;
using RGB.NET.Presets.Decorators;
using RGB.NET.Presets.Textures;
using RGB.NET.Presets.Textures.Gradients;
using System;
using System.Collections.Generic;

namespace TestApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                OpenRGBDeviceProvider.Instance.DeviceDefinitions.Add(new OpenRGBServerDefinition { ClientName = "TestProgram", Ip = "127.0.0.1", Port = 6742 });

                List<IRGBDeviceProvider> deviceProviders = new List<IRGBDeviceProvider>
                {
                    OpenRGBDeviceProvider.Instance,
                    WootingDeviceProvider.Instance
                };

                using RGBSurface surface = new RGBSurface();
                surface.RegisterUpdateTrigger(new TimerUpdateTrigger());

                foreach (IRGBDeviceProvider dp in deviceProviders)
                    surface.Load(dp);

                ListLedGroup ledgroup = new ListLedGroup(surface, surface.Leds);
                RainbowGradient gradient = new RainbowGradient();
                gradient.AddDecorator(new MoveGradientDecorator(surface));
                ledgroup.Brush = new TextureBrush(new LinearGradientTexture(new Size(1, 1), gradient));

                foreach (IRGBDevice d in surface.Devices)
                {
                    Console.WriteLine($"Found {d.DeviceInfo.DeviceName}");
                }

                Console.ReadLine();
                surface?.Dispose();

                foreach (IRGBDeviceProvider dp in deviceProviders)
                    dp.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }
    }
}
