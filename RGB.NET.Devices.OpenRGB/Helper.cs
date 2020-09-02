using OpenRGB.NET.Enums;
using RGB.NET.Core;

namespace RGB.NET.Devices.OpenRGB
{
    public static class Helper
    {
        public static LedId GetInitialLedIdForDeviceType(RGBDeviceType type) => type switch
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
            RGBDeviceType.Keyboard => LedId.Keyboard_Custom1,
            _ => LedId.Custom1
        };

        public static RGBDeviceType GetRgbNetDeviceType(DeviceType type) => type switch
        {
            DeviceType.Motherboard => RGBDeviceType.Mainboard,
            DeviceType.Dram => RGBDeviceType.DRAM,
            DeviceType.Gpu => RGBDeviceType.GraphicsCard,
            DeviceType.Cooler => RGBDeviceType.Cooler,
            DeviceType.Ledstrip => RGBDeviceType.LedStripe,
            DeviceType.Keyboard => RGBDeviceType.Keyboard,
            DeviceType.Mouse => RGBDeviceType.Mouse,
            DeviceType.Mousemat => RGBDeviceType.Mousepad,
            DeviceType.Headset => RGBDeviceType.Headset,
            DeviceType.HeadsetStand => RGBDeviceType.HeadsetStand,
            _ => RGBDeviceType.Unknown
        };

        public static LedId GetInitialLedIdForDeviceType(DeviceType type) =>
            GetInitialLedIdForDeviceType(GetRgbNetDeviceType(type));
    }
}
