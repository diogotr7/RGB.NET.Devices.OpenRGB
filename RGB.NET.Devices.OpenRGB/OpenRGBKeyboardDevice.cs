using RGB.NET.Core;
using RGB.NET.Devices;
using System.Linq;
using OpenRGB.NET.Enums;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBKeyboardDevice : OpenRGBRGBDevice<OpenRGBDeviceInfo>
    {
        public OpenRGBKeyboardDevice(OpenRGBDeviceInfo info) : base(info) { }

        protected override void InitializeLayout()
        {
            //keyboards should be Matrix zone types
            foreach(var zone in DeviceInfo.OpenRGBDevice.Zones)
            {
                if(zone.Type == ZoneType.Matrix)
                {
                    for (int row = 0; row < zone.MatrixMap.Height; row++)
                    {
                        for (int column = 0; column < zone.MatrixMap.Width; column++)
                        {
                            var index = zone.MatrixMap.Matrix[row, column];

                            //will be max value if the position does not have an associated key
                            if (index == uint.MaxValue)
                                continue;

                            if (KeyboardLedMapping.Names.TryGetValue(DeviceInfo.OpenRGBDevice.Leds[index].Name, out var ledid))
                            {
                                InitializeLed(ledid, new Point(19 * column, 19 * row), new Size(19 * 19));
                            }
                        }
                    }
                }
            }
            //TODO: layout?
        }
    }
}