using OpenRGB.NET.Enums;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB.Generic
{
    public class OpenRGBGenericDevice : OpenRGBRGBDevice<OpenRGBDeviceInfo>
    {
        public OpenRGBGenericDevice(OpenRGBDeviceInfo info) : base(info) { }

        protected override void InitializeLayout()
        {
            LedId initial = Helper.GetInitialLedIdForDeviceType(DeviceInfo.DeviceType);

            int y = 0;
            var ledSize = new Size(19);
            const int ledSpacing = 20;

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
                                InitializeLed(ledid, new Point(ledSpacing * column, ledSpacing * row), ledSize);
                            }
                            else
                            {
                                InitializeLed(initial++, new Point(ledSpacing * column, y + (ledSpacing * row)), ledSize);
                            }
                        }
                    }
                    y += (int)(zone.MatrixMap.Height * ledSpacing);
                }
                else
                {
                    for (int i = 0; i < zone.LedCount; i++)
                    {
                        InitializeLed(initial++, new Point(i * ledSpacing, y), ledSize);
                    }
                }

                //we'll just set each zone in its own row for now,
                //with each led for that zone being horizontally distributed
                y += ledSpacing;
            }
        }
    }
}
