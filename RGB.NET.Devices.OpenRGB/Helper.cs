using OpenRGB.NET.Enums;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    public static class Helper
    {
        public static LedId GetInitialLedIdForDeviceType(RGBDeviceType type) =>
        type switch
        {
            RGBDeviceType.Mouse => LedId.Mouse1,
            RGBDeviceType.Headset => LedId.Headset1,
            RGBDeviceType.Mousepad => LedId.Mousepad1,
            RGBDeviceType.LedStripe => LedId.LedStripe1,
            RGBDeviceType.LedMatrix => LedId.LedMatrix1,
            RGBDeviceType.Mainboard => LedId.Mainboard1,
            RGBDeviceType.GraphicsCard => LedId.GraphicsCard1,
            RGBDeviceType.DRAM => LedId.DRAM1,
            RGBDeviceType.HeadsetStand => LedId.HeadsetStand1,
            RGBDeviceType.Keypad => LedId.Keypad1,
            RGBDeviceType.Fan => LedId.Fan1,
            RGBDeviceType.Speaker => LedId.Speaker1,
            RGBDeviceType.Cooler => LedId.Cooler1,
            _ => LedId.Custom1
        };

        public static LedId GetInitialLedIdForDeviceType(DeviceType type) =>
        type switch
        {
            DeviceType.Mouse => LedId.Mouse1,
            DeviceType.Headset => LedId.Headset1,
            DeviceType.Mousemat => LedId.Mousepad1,
            DeviceType.Motherboard => LedId.Mainboard1,
            DeviceType.Gpu => LedId.GraphicsCard1,
            DeviceType.Dram => LedId.DRAM1,
            DeviceType.HeadsetStand => LedId.HeadsetStand1,
            DeviceType.Cooler => LedId.Cooler1,
            _ => LedId.Custom1
        };
    }
}
