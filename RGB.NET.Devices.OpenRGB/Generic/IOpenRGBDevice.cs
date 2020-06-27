using System;
using System.Collections.Generic;
using System.Text;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    internal interface IOpenRGBDevice : IRGBDevice
    {
        void Initialize(OpenRGBUpdateQueue updateQueue, int ledcount);
    }
}
