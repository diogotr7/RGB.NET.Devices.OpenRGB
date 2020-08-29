using System;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = RGB.NET.Core.RGBSurface.Instance;

            s.LoadDevices(RGB.NET.Devices.OpenRGB.OpenRGBDeviceProvider.Instance, throwExceptions: true);

            Console.ReadLine();
        }
    }
}
