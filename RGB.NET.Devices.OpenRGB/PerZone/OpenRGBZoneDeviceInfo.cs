using RGB.NET.Core;
using System.Collections.Generic;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBZoneDeviceInfo : AbstractOpenRGBDeviceInfo
    {
        public OpenRGBZoneDeviceInfo(OpenRGBDevice device) : base(device)
        { }
    }
}