using OpenRGB.NET;
using OpenRGB.NET.Enums;
using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenRGBColor = OpenRGB.NET.Models.Color;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBUpdateQueue : UpdateQueue
    {
        #region Properties & Fields
        private readonly int _deviceid;

        private readonly OpenRGBClient _openRGB;
        private readonly OpenRGBDevice _device;
        private readonly OpenRGBColor[] _colors;
        #endregion

        #region Constructors

        public OpenRGBUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceid, OpenRGBClient client, OpenRGBDevice device)
            : base(updateTrigger)
        {
            _deviceid = deviceid;
            _openRGB = client;
            _device = device;
            _colors = Enumerable.Range(0, _device.Colors.Length)
                                .Select(_ => new OpenRGBColor())
                                .ToArray();
        }

        #endregion

        #region Methods

        protected override void Update(Dictionary<object, Color> dataSet)
        {
            foreach(var data in dataSet)
            {
                _colors[(int)data.Key] = new OpenRGBColor(data.Value.GetR(), data.Value.GetG(), data.Value.GetB());
            }

            _openRGB.UpdateLeds(_deviceid, _colors);
        }

        #endregion
    }
}
