using System;
using RGB.NET.Devices.OpenRGB;
using RGB.NET.Core;

namespace TestApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            var s = RGBSurface.Instance;

            OpenRGBDeviceProvider.Instance.DeviceDefinitions.Add(new OpenRGBServerDefinition { ClientName = "TestProgram", Ip = "127.0.0.1", Port = 6742 });
            s.LoadDevices(OpenRGBDeviceProvider.Instance, throwExceptions: true);

            foreach (var d in s.Devices)
            {
                foreach (var led in d)
                {
                    led.Color = new Color(255, 0, 0);
                }
            }

            s.Update();
            Console.ReadLine();
        }
    }
}
