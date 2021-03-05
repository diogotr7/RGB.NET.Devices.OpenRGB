using OpenRGB.NET.Enums;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB.Generic
{
    public class OpenRGBGenericDevice : AbstractOpenRGBDevice<OpenRGBDeviceInfo>
    {
        public OpenRGBGenericDevice(OpenRGBDeviceInfo info, IUpdateQueue updateQueue)
            : base(info, updateQueue)
        {
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            LedId initial = Helper.GetInitialLedIdForDeviceType(DeviceInfo.DeviceType);

            int y = 0;
            Size ledSize = new Size(19);
            int zoneLedIndex = 0;
            const int ledSpacing = 20;

            foreach (global::OpenRGB.NET.Models.Zone? zone in DeviceInfo.OpenRGBDevice.Zones)
            {
                if (zone.Type == ZoneType.Matrix)
                {
                    for (int row = 0; row < zone.MatrixMap.Height; row++)
                    {
                        for (int column = 0; column < zone.MatrixMap.Width; column++)
                        {
                            uint index = zone.MatrixMap.Matrix[row, column];

                            //will be max value if the position does not have an associated key
                            if (index == uint.MaxValue)
                                continue;

                            LedId ledId = StandardKeyNames.Default.TryGetValue(DeviceInfo.OpenRGBDevice.Leds[zoneLedIndex + index].Name, out LedId l)
                                ? l
                                : initial++;

                            if (!_indexMapping.ContainsKey(ledId))
                            {
                                _indexMapping.Add(ledId, zoneLedIndex + (int)index);
                                AddLed(ledId, new Point(ledSpacing * column, y + (ledSpacing * row)), ledSize);
                            }
                        }
                    }
                    y += (int)(zone.MatrixMap.Height * ledSpacing);
                }
                else
                {
                    for (int i = 0; i < zone.LedCount; i++)
                    {
                        LedId ledId = initial++;

                        if (!_indexMapping.ContainsKey(ledId))
                        {
                            _indexMapping.Add(ledId, zoneLedIndex + i);
                            AddLed(ledId, new Point(i * ledSpacing, y), ledSize);
                        }
                    }
                }

                //we'll just set each zone in its own row for now,
                //with each led for that zone being horizontally distributed
                y += ledSpacing;
                zoneLedIndex += (int)zone.LedCount;
            }
        }
    }
}
