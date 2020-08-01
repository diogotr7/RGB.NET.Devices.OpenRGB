using OpenRGB.NET;
using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenRGBColor = OpenRGB.NET.Models.Color;
using OpenRGBDevice = OpenRGB.NET.Models.Device;


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
        private readonly OpenRGBColor[] _colors;
        private readonly OpenRGBDevice _device;

        private readonly List<LedId> Mapping;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AsusUpdateQueue"/> class.
        /// </summary>
        /// <param name="updateTrigger">The update trigger used by this queue.</param>
        public OpenRGBUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceid, OpenRGBClient client, OpenRGBDevice device)
            : base(updateTrigger)
        {
            this._deviceid = deviceid;
            this._openRGB = client;
            this._colors = Enumerable.Range(0, device.Colors.Length).Select(_ => new OpenRGBColor()).ToArray();
            this._device = device;

            Mapping = new List<LedId>();

            for (int i = 0; i < _device.Leds.Length; i++)
            {
                if(KeyboardLedMapping.Names.TryGetValue(_device.Leds[i].Name, out var ledId))
                {
                    Mapping.Add(ledId);
                }
                else
                {
                    Mapping.Add(LedId.Invalid);
                }
            }
        }

        #endregion

        #region Methods

        protected override void Update(Dictionary<object, Color> dataSet)
        {
            for (int i = 0; i < _device.Leds.Length; i++)
            {
                if(dataSet.TryGetValue(Mapping[i], out var clr))
                {
                    _colors[i] = new OpenRGBColor(clr.GetR(), clr.GetG(), clr.GetB());
                }
            }

            _openRGB.UpdateLeds(_deviceid, _colors);
        }

        #endregion
    }
}
