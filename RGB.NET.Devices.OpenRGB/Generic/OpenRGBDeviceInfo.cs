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

        public string Manufacturer { get; }

        public string Model { get; }

        public Uri Image { get; set; }

        public bool SupportsSyncBack => false;

        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;

        public OpenRGBDevice OpenRGBDevice { get; }

        public int OpenRGBDeviceIndex { get; }
        #endregion 

        #region Constructors

        internal OpenRGBDeviceInfo(int deviceIndex, RGBDeviceType deviceType, OpenRGBDevice device, Dictionary<string, int> modelCounter)
        {
            OpenRGBDeviceIndex = deviceIndex;
            DeviceType = deviceType;
            OpenRGBDevice = device;

            if (!string.IsNullOrEmpty(OpenRGBDevice.Vendor))
            {
                Manufacturer = OpenRGBDevice.Vendor;
                Model = OpenRGBDevice.Name.Replace(OpenRGBDevice.Vendor, "").Trim();
            }
            else
            {
                Manufacturer = "OpenRGB";
                Model = OpenRGBDevice.Name;
            }

            DeviceName = GetUniqueModelName(modelCounter);
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