namespace RGB.NET.Devices.OpenRGB
{
    internal class OpenRGBMouseDevice : IOpenRGBDevice
    {
        private OpenRGBDeviceInfo openRGBDeviceInfo;

        public OpenRGBMouseDevice(OpenRGBDeviceInfo openRGBDeviceInfo)
        {
            this.openRGBDeviceInfo = openRGBDeviceInfo;
        }
    }
}