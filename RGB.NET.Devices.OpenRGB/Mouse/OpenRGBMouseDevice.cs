using OpenRGB.NET.Enums;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBMouseDevice : OpenRGBRGBDevice<OpenRGBDeviceInfo>
    {
        public OpenRGBMouseDevice(OpenRGBDeviceInfo info) : base(info) { }

        protected override void InitializeLayout()
        {
            LedId initial = LedId.Mouse1;
            int y = 0;

            foreach (var zone in DeviceInfo.OpenRGBDevice.Zones)
            {
                if (zone.Type == ZoneType.Single)
                {
                    InitializeLed(initial++, new Point(0, y), new Size(19));
                }
                else if (zone.Type == ZoneType.Linear)
                {
                    int x = 0;
                    for (int i = 0; i < zone.LedCount; i++)
                    {
                        InitializeLed(initial++, new Point(x += 20, y), new Size(19));
                    }
                }

                //we'll just set each zone in its own row for now,
                //with each led for that zone being horizontally distributed
                y += 20;
            }
        }
    }
}