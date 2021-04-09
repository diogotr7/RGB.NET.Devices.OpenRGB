using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    public abstract class AbstractOpenRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IOpenRGBDevice
        where TDeviceInfo : AbstractOpenRGBDeviceInfo
    {
        #region Constructors

        protected AbstractOpenRGBDevice(TDeviceInfo info, IUpdateQueue updateQueue)
            : base(info, updateQueue)
        { }

        #endregion
    }
}
