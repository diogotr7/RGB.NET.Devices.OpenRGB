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
            switch (_zone.Type)
            {
                case ZoneType.Single:
                case ZoneType.Linear:
                    for (int i = 0; i < _zone.LedCount; i++)
                    {
                        InitializeLed(_initial++, new Point(20 * i, 0), new Size(19));
                    }
                    break;
                case ZoneType.Matrix:
                    for (int row = 0; row < _zone.MatrixMap.Height; row++)
                    {
                        for (int column = 0; column < _zone.MatrixMap.Width; column++)
                        {
                            InitializeLed(_initial++, new Point(20 * column,  (20 * row)), new Size(19));
                        }
                    }
                    break;
            }
        }
    }
}