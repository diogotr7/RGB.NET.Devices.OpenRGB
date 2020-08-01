using RGB.NET.Core;
using System.Collections.Generic;
using OpenRGBDeviceType = OpenRGB.NET.Enums.DeviceType;

namespace RGB.NET.Devices.OpenRGB.Generic
{
    public static class LedMappings
    {
        public static Dictionary<OpenRGBDeviceType, Dictionary<string, LedId>> Mappings = new Dictionary<OpenRGBDeviceType, Dictionary<string, LedId>>()
        {
            [OpenRGBDeviceType.Keyboard] = KeyboardLedMapping.Names,
            [OpenRGBDeviceType.Mouse] = MouseLedMapping.Names,
            [OpenRGBDeviceType.Motherboard] = MotherboardLedMapping.Names,
        };
    }
}
