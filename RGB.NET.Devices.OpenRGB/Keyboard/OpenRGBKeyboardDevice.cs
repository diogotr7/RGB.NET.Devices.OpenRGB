using OpenRGB.NET.Enums;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBKeyboardDevice : OpenRGBRGBDevice<OpenRGBDeviceInfo>
    {
        public OpenRGBKeyboardDevice(OpenRGBDeviceInfo info) : base(info) { }

        protected override void InitializeLayout()
        {
            int y = 0;
            LedId initial = LedId.Keyboard_Custom1;
            //keyboards should be Matrix zone types
            foreach (var zone in DeviceInfo.OpenRGBDevice.Zones)
            {
                if (zone.Type == ZoneType.Matrix)
                {
                    for (int row = 0; row < zone.MatrixMap.Height; row++)
                    {
                        for (int column = 0; column < zone.MatrixMap.Width; column++)
                        {
                            var index = zone.MatrixMap.Matrix[row, column];

                            //will be max value if the position does not have an associated key
                            if (index == uint.MaxValue)
                                continue;

                            if (KeyboardLedMapping.Default.TryGetValue(DeviceInfo.OpenRGBDevice.Leds[index].Name, out var ledid))
                            {
                                InitializeLed(ledid, new Point(20 * column, 20 * row), new Size(19));
                            }
                        }
                    }
                    y += (int)(zone.MatrixMap.Height * 20);
                }
                else if (zone.Type == ZoneType.Linear)
                {
                    for (int i = 0; i < zone.LedCount; i++)
                    {
                        InitializeLed(initial++, new Point(i * 20, y), new Size(19));
                    }
                }
                else if (zone.Type == ZoneType.Single)
                {
                    InitializeLed(initial++, new Point(0, y), new Size(19));
                }
                y += 20;
            }
            //TODO: layout?
        }
    }
}