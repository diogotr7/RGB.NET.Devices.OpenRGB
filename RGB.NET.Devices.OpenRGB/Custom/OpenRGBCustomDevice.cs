using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBCustomDevice : OpenRGBRGBDevice<OpenRGBDeviceInfo>
    {
        private LedId _initial;
        private readonly uint _ledCount;

        public OpenRGBCustomDevice(OpenRGBDeviceInfo info, LedId initial, uint ledCount) : base(info)
        {
            _initial = initial;
            _ledCount = ledCount;
        }

        protected override void InitializeLayout()
        {
            for(int i = 0; i < _ledCount; i++)
            {
                InitializeLed(_initial++, new Point(20 * i, 0), new Size(19));
            }
        }
    }
}