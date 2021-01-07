using RGB.NET.Core;
using System;
using System.Collections.Generic;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    public abstract class AbstractOpenRGBDeviceInfo : IRGBDeviceInfo
    {
        public RGBDeviceType DeviceType { get; protected set; }
        public string DeviceName { get; protected set; }
        public string Manufacturer { get; protected set; }
        public string Model { get; protected set; }
        public RGBDeviceLighting Lighting { get; protected set; }
        public Uri Image { get; set; }
        public OpenRGBDevice OpenRGBDevice { get; protected set; }
        public int OpenRGBDeviceIndex { get; protected set; }


        protected virtual string GetUniqueModelName(Dictionary<string, int> modelCounter)
        {
            if (modelCounter.ContainsKey(Model))
            {
                int counter = ++modelCounter[Model];
                return $"{Manufacturer} {Model} {counter}";
            }
            else
            {
                modelCounter.Add(Model, 1);
                return $"{Manufacturer} {Model}";
            }
        }
    }
}
