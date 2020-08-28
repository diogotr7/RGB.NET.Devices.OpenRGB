using OpenRGB.NET;
using OpenRGB.NET.Enums;
using RGB.NET.Core;
using System.Collections.Generic;
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

        private readonly IReadOnlyList<LedId> Mapping;
        #endregion

        #region Constructors

        public OpenRGBUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceid, OpenRGBClient client, OpenRGBDevice device)
            : base(updateTrigger)
        {
            this._deviceid = deviceid;
            this._openRGB = client;
            this._device = device;
            this._colors = Enumerable.Range(0, device.Colors.Length).Select(_ => new OpenRGBColor()).ToArray();
            var map = new List<LedId>();

            if (_device.Type == DeviceType.Keyboard)
            {
                for (int i = 0; i < _device.Leds.Length; i++)
                {
                    if (KeyboardLedMapping.Default.TryGetValue(_device.Leds[i].Name, out var ledId))
                    {
                        map.Add(ledId);
                    }
                    else
                    {
                        map.Add(LedId.Invalid);
                    }
                }
            }
            else
            {
                LedId initial = Helper.GetInitialLedIdForDeviceType(device.Type);
                for (int i = 0; i < _device.Leds.Length; i++)
                {
                    map.Add(initial++);
                }
            }

            Mapping = map.AsReadOnly();
        }

        #endregion

        #region Methods

        protected override void Update(Dictionary<object, Color> dataSet)
        {
            for (int i = 0; i < _device.Leds.Length; i++)
            {
                if (dataSet.TryGetValue(Mapping[i], out var clr))
                {
                    _colors[i] = new OpenRGBColor(clr.GetR(), clr.GetG(), clr.GetB());
                }
            }

            _openRGB.UpdateLeds(_deviceid, _colors);
        }

        #endregion
    }
}
