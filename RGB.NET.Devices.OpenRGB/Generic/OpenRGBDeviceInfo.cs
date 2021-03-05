using RGB.NET.Core;
using System.Collections.Generic;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBDeviceInfo : AbstractOpenRGBDeviceInfo
    {
        public OpenRGBDeviceInfo(int deviceIndex, RGBDeviceType deviceType, OpenRGBDevice device, Dictionary<string, int> modelCounter)
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
    }
}