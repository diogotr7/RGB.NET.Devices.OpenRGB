using RGB.NET.Core;
using System;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        public int OpenRGBDeviceIndex { get; }

        public RGBDeviceType DeviceType { get; }

        public string DeviceName => OpenRGBDevice.Name;

        public string Manufacturer => "OpenRGB";

        public string Model => OpenRGBDevice.Name;

        public Uri Image { get; set; }

        public bool SupportsSyncBack => false;

        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;

        public OpenRGBDevice OpenRGBDevice { get; }
        #endregion 

        #region Constructors

        internal OpenRGBDeviceInfo(int deviceIndex, RGBDeviceType deviceType, OpenRGBDevice device)
        {
            this.OpenRGBDeviceIndex = deviceIndex;
            this.DeviceType = deviceType;
            this.OpenRGBDevice = device;
        }

        #endregion
    }
}