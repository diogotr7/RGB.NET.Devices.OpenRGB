using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        public IRGBDeviceProvider GetDeviceProvider() => OpenRGBDeviceProvider.Instance;

        #endregion
    }
}
