using System;
using System.Collections.Generic;
using System.Text;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        /// <inheritdoc />
        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        /// <inheritdoc />
        public IRGBDeviceProvider GetDeviceProvider() => OpenRGBDeviceProvider.Instance;

        #endregion
    }
}
