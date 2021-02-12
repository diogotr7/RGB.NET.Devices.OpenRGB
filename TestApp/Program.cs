﻿using System;
using RGB.NET.Devices.OpenRGB;
using RGB.NET.Core;

namespace TestApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            var s = new RGBSurface();

            try
            {
                OpenRGBDeviceProvider.Instance.DeviceDefinitions.Add(new OpenRGBServerDefinition { ClientName = "TestProgram", Ip = "127.0.0.1", Port = 6742 });
                OpenRGBDeviceProvider.Instance.Initialize();
                s.Attach(OpenRGBDeviceProvider.Instance.Devices);

                foreach (var d in s.Devices)
                {
                    Console.WriteLine($"Found {d.DeviceInfo.DeviceName}");
                    foreach (var led in d)
                    {
                        led.Color = new Color(255, 255, 255);
                    }
                }

                s.Update();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e}");
            }
        }
    }
}
