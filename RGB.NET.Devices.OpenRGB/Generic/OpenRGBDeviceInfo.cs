using RGB.NET.Core;
using System;
using System.Collections.Generic;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields
        public RGBDeviceType DeviceType { get; }

        public string DeviceName { get; }

        public string Manufacturer => "OpenRGB";

        public string Model => OpenRGBDevice.Name;

        public Uri Image { get; set; }

        public bool SupportsSyncBack => false;

        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;

        public OpenRGBDevice OpenRGBDevice { get; }

        public int OpenRGBDeviceIndex { get; }
        #endregion 

        #region Constructors

        internal OpenRGBDeviceInfo(int deviceIndex, RGBDeviceType deviceType, OpenRGBDevice device, Dictionary<string, int> modelCounter)
        {
            this.OpenRGBDeviceIndex = deviceIndex;
            this.DeviceType = deviceType;
            this.OpenRGBDevice = device;

            this.DeviceName = GetUniqueModelName(modelCounter);
        }

        #endregion

        #region Methods
        private string GetUniqueModelName(Dictionary<string, int> modelCounter)
        {
            if (modelCounter.TryGetValue(Model, out int counter))
            {
                counter = ++modelCounter[Model];
                return $"{Manufacturer} {Model} {counter}";
            }
            else
            {
                modelCounter.Add(Model, 1);
                return $"{Manufacturer} {Model}";
            }
        }
        #endregion
    }
}