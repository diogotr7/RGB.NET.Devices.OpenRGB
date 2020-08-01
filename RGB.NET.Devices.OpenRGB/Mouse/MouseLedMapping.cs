using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RGB.NET.Devices.OpenRGB
{
    public static class MouseLedMapping
    {
        public static readonly Dictionary<string, LedId> Names = new Dictionary<string, LedId>()
        {
            { "Logo LED" , LedId.Mouse1 },
            { "DPI LED"  , LedId.Mouse2 },
        };
    }
}
