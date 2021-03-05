using RGB.NET.Core;
using System.Collections.Generic;

namespace RGB.NET.Devices.OpenRGB
{
    public abstract class AbstractOpenRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IOpenRGBDevice
        where TDeviceInfo : AbstractOpenRGBDeviceInfo
    {
        #region Properties & Fields

        protected readonly Dictionary<LedId, int> _indexMapping = new Dictionary<LedId, int>();

        #endregion

        #region Constructors

        protected AbstractOpenRGBDevice(TDeviceInfo info, IUpdateQueue updateQueue)
            : base(info, updateQueue)
        { }

        #endregion

        #region Methods

        protected override object? GetLedCustomData(LedId ledId)
        {
            if (!_indexMapping.TryGetValue(ledId, out int index))
                return null;

            return index;
        }

        #endregion
    }
}
