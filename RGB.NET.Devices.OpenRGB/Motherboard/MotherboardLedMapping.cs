using RGB.NET.Core;
using System.Collections.Generic;

namespace RGB.NET.Devices.OpenRGB
{
    public static class MotherboardLedMapping
    {
        public static readonly Dictionary<string, LedId> Names = new Dictionary<string, LedId>()
        {
            { "MSI LED", LedId.Mainboard1 },
        };
    }
}
