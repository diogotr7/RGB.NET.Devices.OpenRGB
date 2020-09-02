using OpenRGB.NET.Enums;
using RGB.NET.Core;
using System.Collections.Generic;
using System.Linq;

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

        protected virtual void InitializeLayout()
        {
            LedId initial = Helper.GetInitialLedIdForDeviceType(DeviceInfo.DeviceType);

            int y = 0;
            var ledSize = new Size(19);
            const int ledSpacing = 20;

            foreach (var zone in DeviceInfo.OpenRGBDevice.Zones)
            {
                switch (zone.Type)
                {
                    case ZoneType.Single:
                    case ZoneType.Linear:
                        for (int i = 0; i < zone.LedCount; i++)
                        {
                            InitializeLed(initial++, new Point(i * ledSpacing, y), ledSize);
                        }
                        break;
                    case ZoneType.Matrix:
                        for (int row = 0; row < zone.MatrixMap.Height; row++)
                        {
                            for (int column = 0; column < zone.MatrixMap.Width; column++)
                            {
                                var index = zone.MatrixMap.Matrix[row, column];

                                //will be max value if the position does not have an associated key
                                if (index == uint.MaxValue)
                                    continue;

                                if (KeyboardLedMapping.Default.TryGetValue(DeviceInfo.OpenRGBDevice.Leds[index].Name, out var ledid))
                                {
                                    InitializeLed(ledid, new Point(ledSpacing * column, ledSpacing * row), ledSize);
                                }
                                else
                                {
                                    InitializeLed(initial++, new Point(ledSpacing * column, y + (ledSpacing * row)), ledSize);
                                }
                            }
                        }
                        y += (int)(zone.MatrixMap.Height * ledSpacing);
                        break;
                }

                //we'll just set each zone in its own row for now,
                //with each led for that zone being horizontally distributed
                y += ledSpacing;
            }
        }

        protected override void UpdateLeds(IEnumerable<Led> ledsToUpdate) => UpdateQueue.SetData(ledsToUpdate.Where(x => x.Color.A > 0));

        #endregion
    }
}
