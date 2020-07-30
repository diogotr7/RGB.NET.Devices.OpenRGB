using OpenRGB.NET;
using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenRGBColor = OpenRGB.NET.Models.Color;

namespace RGB.NET.Devices.OpenRGB
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the update-queue performing updates for asus devices.
    /// </summary>
    public class OpenRGBUpdateQueue : UpdateQueue
    {
        #region Properties & Fields
        private readonly int _deviceid;

        private readonly OpenRGBClient _openRGB;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsusUpdateQueue"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        public OpenRGBUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceid, OpenRGBClient client)
            : base(updateTrigger)
        {
            this._deviceid = deviceid;
            this._openRGB = client;
        }

        #endregion

        #region Methods

        protected override void Update(Dictionary<object, Color> dataSet)
        {
            var colors = Enumerable.Range(0, dataSet.Count).Select(_ => new OpenRGBColor()).ToArray();
            foreach(var data in dataSet)
            {
                colors[(int)data.Key] = new OpenRGBColor(data.Value.GetR(), data.Value.GetG(), data.Value.GetB());
            }

            _openRGB.UpdateLeds(_deviceid, colors);
        }

        #endregion
    }
}
