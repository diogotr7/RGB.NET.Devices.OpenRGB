namespace RGB.NET.Devices.OpenRGB
{
    internal class OpenRGBKeyboardDevice : IOpenRGBDevice
    {
        private OpenRGBDeviceInfo openRGBDeviceInfo;

        public OpenRGBKeyboardDevice(OpenRGBDeviceInfo openRGBDeviceInfo)
        {
            this.openRGBDeviceInfo = openRGBDeviceInfo;
        }
    }
}