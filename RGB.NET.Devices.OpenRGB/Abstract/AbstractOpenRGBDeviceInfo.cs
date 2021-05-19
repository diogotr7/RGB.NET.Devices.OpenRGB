using RGB.NET.Core;
using System.Collections.Generic;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    public abstract class AbstractOpenRGBDeviceInfo : IRGBDeviceInfo
    {
        public RGBDeviceType DeviceType { get; }
        public string DeviceName { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public object? LayoutMetadata { get; set; }
        public OpenRGBDevice OpenRGBDevice { get; }

        protected AbstractOpenRGBDeviceInfo(OpenRGBDevice openRGBDevice)
        {
            OpenRGBDevice = openRGBDevice;
            DeviceType = Helper.GetRgbNetDeviceType(openRGBDevice.Type);
            Manufacturer = Helper.GetVendorName(openRGBDevice);
            Model = Helper.GetModelName(openRGBDevice);
            DeviceName = DeviceHelper.CreateDeviceName(Manufacturer, Model);
        }
    }
}
