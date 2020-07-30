using RGB.NET.Core;
using System.Collections.Generic;
using System.Linq;
using OpenRGBLed = OpenRGB.NET.Models.Led;

namespace RGB.NET.Devices.OpenRGB
{
    public abstract class OpenRGBRGBDevice<TDeviceInfo> : AbstractRGBDevice<TDeviceInfo>, IOpenRGBDevice
        where TDeviceInfo : OpenRGBDeviceInfo
    {
        #region Properties & Fields

        public override TDeviceInfo DeviceInfo { get; }

        private OpenRGBUpdateQueue UpdateQueue { get; set; }

        #endregion

        #region Constructors

        protected OpenRGBRGBDevice(TDeviceInfo info)
        {
            this.DeviceInfo = info;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the device.
        /// </summary>
        public void Initialize(OpenRGBUpdateQueue updateQueue)
        {
            UpdateQueue = updateQueue;

            InitializeLayout();

            if (Size == Size.Invalid)
            {
                Rectangle ledRectangle = new Rectangle(this.Select(x => x.LedRectangle));
                Size = ledRectangle.Size + new Size(ledRectangle.Location.X, ledRectangle.Location.Y);
            }
        }

        protected abstract void InitializeLayout();

        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(ledsToUpdate.Where(x => x.Color.A > 0));

        #endregion
    }
}
