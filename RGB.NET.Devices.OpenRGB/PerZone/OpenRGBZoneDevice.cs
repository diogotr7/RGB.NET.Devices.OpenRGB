using OpenRGB.NET.Enums;
using OpenRGB.NET.Models;
using RGB.NET.Core;
using System.IO;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBZoneDevice : AbstractOpenRGBDevice<OpenRGBZoneDeviceInfo>
    {
        private readonly int _initialLed;
        private readonly Zone _zone;

        public OpenRGBZoneDevice(OpenRGBZoneDeviceInfo info, int initialLed, Zone zone) : base(info)
        {
            _initialLed = initialLed;
            _zone = zone;
        }

        protected override void InitializeLayout()
        {
            var ledSize = new Size(19);
            const int ledSpacing = 20;
            var initial = Helper.GetInitialLedIdForDeviceType(DeviceInfo.DeviceType);

            if (_zone.Type == ZoneType.Matrix)
            {
                for (int row = 0; row < _zone.MatrixMap.Height; row++)
                {
                    for (int column = 0; column < _zone.MatrixMap.Width; column++)
                    {
                        var index = _zone.MatrixMap.Matrix[row, column];

                        //will be max value if the position does not have an associated key
                        if (index == uint.MaxValue)
                            continue;

                        var ledId = StandardKeyNames.Default.TryGetValue(DeviceInfo.OpenRGBDevice.Leds[index].Name, out var l)
                            ? l
                            : initial++;

                        if (!_indexMapping.ContainsKey(ledId))
                            _indexMapping.Add(ledId, (int)index);

                        AddLed(ledId, new Point(ledSpacing * column, ledSpacing * row), ledSize);
                    }
                }
            }
            else
            {
                for (int i = 0; i < _zone.LedCount; i++)
                {
                    var ledId = initial++;

                    if (!_indexMapping.ContainsKey(ledId))
                        _indexMapping.Add(ledId, _initialLed + i);

                    AddLed(ledId, new Point(ledSpacing * i, 0), ledSize);
                }
            }
        }
    }
}