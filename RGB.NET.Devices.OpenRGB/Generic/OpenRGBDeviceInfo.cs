using System;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a generic information for a MSI-<see cref="T:RGB.NET.Core.IRGBDevice" />.
    /// </summary>
    public class OpenRGBDeviceInfo : IRGBDeviceInfo
    {
        #region Properties & Fields

        /// <inheritdoc />
        public RGBDeviceType DeviceType { get; }

        /// <inheritdoc />
        public string DeviceName { get; }

        /// <inheritdoc />
        public string Manufacturer { get; }

        /// <inheritdoc />
        public string Model { get; }

        /// <inheritdoc />
        public Uri Image { get; set; }

        /// <inheritdoc />
        public bool SupportsSyncBack => false;

        /// <inheritdoc />
        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;


        #endregion

        #region Constructors


        internal OpenRGBDeviceInfo(RGBDeviceType deviceType, string model = null, string manufacturer = "OpenRGB")
        {
            this.DeviceType = deviceType;
            this.Model = model;
            this.Manufacturer = manufacturer;

            DeviceName = $"{Manufacturer} {Model}";
        }

        #endregion
    }
}