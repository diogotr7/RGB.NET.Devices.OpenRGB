using System;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = RGB.NET.Core.RGBSurface.Instance;

            s.LoadDevices(RGB.NET.Devices.OpenRGB.OpenRGBDeviceProvider.Instance, throwExceptions: true);

            foreach (var d in s.Devices)
            {
                foreach (var led in d)
                {
                    led.Color = new RGB.NET.Core.Color(255, 255, 0);
                }
            }

            s.Update();
            Console.ReadLine();
        }
    }
}
