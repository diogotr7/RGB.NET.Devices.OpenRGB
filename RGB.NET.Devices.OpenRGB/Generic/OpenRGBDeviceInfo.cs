using RGB.NET.Core;
using System;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a MSI-<see cref="T:RGB.NET.Core.IRGBDevice" />.
    /// </summary>
    public class OpenRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        public int OpenRGBDeviceIndex { get; }

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string DeviceName => OpenRGBDevice.Name;

        /// <inheritdoc />
        public string Manufacturer => "OpenRGB";

        /// <inheritdoc />
        public string Model => OpenRGBDevice.Name;

        /// <inheritdoc />
        public Uri Image { get; set; }

        /// <inheritdoc />
        public bool SupportsSyncBack => false;

        /// <inheritdoc />
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