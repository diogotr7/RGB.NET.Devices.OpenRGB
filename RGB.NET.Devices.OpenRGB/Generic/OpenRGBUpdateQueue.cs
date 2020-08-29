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

        private readonly ReadOnlyCollection<LedId> Mapping;
        #endregion

        #region Constructors

        public OpenRGBUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceid, OpenRGBClient client, OpenRGBDevice device)
            : base(updateTrigger)
        {
            this._deviceid = deviceid;
            this._openRGB = client;
            this._device = device;
            this._colors = Enumerable.Range(0, device.Colors.Length).Select(_ => new OpenRGBColor()).ToArray();
            var map = new LedId[device.Colors.Length];

            if (_device.Type == DeviceType.Keyboard)
            {
                LedId initial = LedId.Keyboard_Custom1;
                uint zoneLedCount = 0;
                foreach (var zone in _device.Zones)
                {
                    if (zone.Type == ZoneType.Matrix)
                    {
                        for (int row = 0; row < zone.MatrixMap.Height; row++)
                        {
                            for (int column = 0; column < zone.MatrixMap.Width; column++)
                            {
                                var index = zone.MatrixMap.Matrix[row, column];

                                //will be max value if the position does not have an associated key
                                if (index == uint.MaxValue)
                                    continue;

                                if (KeyboardLedMapping.Default.TryGetValue(_device.Leds[index].Name, out var ledid))
                                {
                                    map[(int)index] = ledid;
                                }
                                else
                                {
                                    map[(int)index] = LedId.Invalid;
                                }
                            }
                        }
                    }
                    else if (zone.Type == ZoneType.Linear)
                    {
                        for (var j = 0; j < zone.LedCount; j++)
                        {
                            map[(int)(zoneLedCount + j)] = initial++;
                        }
                    }
                    else if (zone.Type == ZoneType.Single)
                    {
                        map[(int)zoneLedCount] = initial++;
                    }

                    zoneLedCount += zone.LedCount;
                }
            }
            else
            {
                LedId initial = Helper.GetInitialLedIdForDeviceType(device.Type);
                for (int i = 0; i < _device.Leds.Length; i++)
                {
                    map[(int)i] = initial++;
                }
            }

            Mapping = Array.AsReadOnly(map);
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
