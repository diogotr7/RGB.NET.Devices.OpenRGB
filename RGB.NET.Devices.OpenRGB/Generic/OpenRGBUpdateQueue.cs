using OpenRGB.NET;
using RGB.NET.Core;
using System;
using System.Collections.Generic;

namespace RGB.NET.Devices.OpenRGB
{
    /// <inheritdoc />
    /// <summary>
    /// Represents the update-queue performing updates for asus devices.
    /// </summary>
    public class OpenRGBUpdateQueue : UpdateQueue
    {
        #region Properties & Fields

        /// <summary>
        /// The device to be updated.
        /// </summary>
        /// 
        //OpenRGBDeviceProvider _dp = new OpenRGBDeviceProvider();

        private int _deviceid;

        private OpenRGBClient _openRGB;

        private int _ledcount;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsusUpdateQueue"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        public OpenRGBUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceid, OpenRGBClient client, int ledcount)
            : base(updateTrigger)
        {
            this._deviceid = deviceid;
            this._openRGB = client;
            this._ledcount = ledcount;
        }

        #endregion

        #region Methods



        protected override void Update(Dictionary<object, Color> dataSet)
        {
            try
            {
                OpenRGBColor[] list = new OpenRGBColor[_ledcount];
                for (int i = 0; i < _ledcount; i++)
                {
                    list[i] = new OpenRGBColor();
                }

                //int index;

                foreach (KeyValuePair<object, Color> data in dataSet)
                {
                    //index = (int)data.Key;
                    //list[index] = new OpenRGBColor(data.Value.GetR(), data.Value.GetG(), data.Value.GetB());


                    for (int j = 0; j < _ledcount; j++)
                    {
                        list[j] = new OpenRGBColor(data.Value.GetR(), data.Value.GetG(), data.Value.GetB());
                    }

                }


                _openRGB.UpdateLeds(_deviceid, list);
            }
            catch (Exception ex)
            { throw; }
        }

        #endregion
    }
}
