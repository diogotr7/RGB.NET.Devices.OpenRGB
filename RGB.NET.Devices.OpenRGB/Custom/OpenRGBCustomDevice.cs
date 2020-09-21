using OpenRGB.NET.Enums;
using OpenRGB.NET.Models;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBCustomDevice : OpenRGBRGBDevice<OpenRGBDeviceInfo>
    {
        private LedId _initial;
        private Zone _zone;

        public OpenRGBCustomDevice(OpenRGBDeviceInfo info, LedId initial, Zone zone) : base(info)
        {
            _initial = initial;
            _zone = zone;
        }

        protected override void InitializeLayout()
        {
            var ledSize = new Size(19);
            const int ledSpacing = 20;

            if (_zone.Type == ZoneType.Matrix)
            {
                for (int row = 0; row < _zone.MatrixMap.Height; row++)
                {
                    for (int column = 0; column < _zone.MatrixMap.Width; column++)
                    {
                        InitializeLed(_initial++, new Point(ledSpacing * column, ledSpacing * row), ledSize);
                    }
                }
            }
            else
            {
                for (int i = 0; i < _zone.LedCount; i++)
                {
                    InitializeLed(_initial++, new Point(ledSpacing * i, 0), ledSize);
                }
            }
        }
    }
}