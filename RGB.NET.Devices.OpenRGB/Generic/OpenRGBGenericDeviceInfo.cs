using RGB.NET.Core;
using System.Collections.Generic;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBGenericDeviceInfo : AbstractOpenRGBDeviceInfo
    {
        public OpenRGBGenericDeviceInfo(OpenRGBDevice device) : base(device)
        { }
    }
}