using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{

    public class OpenRGBUnspecifiedRGBDevice : OpenRGBRGBDevice<OpenRGBDeviceInfo>
    {
        #region Properties & Fields

        private LedId _baseLedId;

        #endregion

        #region Constructors


        internal OpenRGBUnspecifiedRGBDevice(OpenRGBDeviceInfo info)
            : base(info)
        {

        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void InitializeLayout(int ledcount)
        {
            for (int i = 0; i < ledcount; i++)
            {
                InitializeLed(LedId.Custom1 + i, new Rectangle(i * 10, 0, 10, 10));
            }


            //TODO DarthAffe 19.05.2019: Add a way to define a layout for this kind of devies
        }

        /// <inheritdoc />
        protected override object CreateLedCustomData(LedId ledId) => (int)ledId - (int)_baseLedId;

        #endregion
    }
}