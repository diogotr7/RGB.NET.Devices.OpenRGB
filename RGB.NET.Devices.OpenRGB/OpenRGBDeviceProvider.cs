using OpenRGB.NET;
using RGB.NET.Core;
using RGB.NET.Devices.OpenRGB.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenRGBDeviceType = OpenRGB.NET.Enums.DeviceType;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static OpenRGBDeviceProvider _instance;

        public static OpenRGBDeviceProvider Instance => _instance ?? new OpenRGBDeviceProvider();

        public static string ClientName { get; set; } = "RGB.NET";

        public bool IsInitialized { get; private set; }

        public bool HasExclusiveAccess => false;

        public IEnumerable<IRGBDevice> Devices { get; private set; }

        public DeviceUpdateTrigger UpdateTrigger { get; }

        private OpenRGBClient _openRgb;

        #endregion

        #region Constructors

        public OpenRGBDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(OpenRGBDeviceProvider)}");
            _instance = this;

            UpdateTrigger = new DeviceUpdateTrigger();
        }

        #endregion

        #region Methods

        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                UpdateTrigger?.Stop();
                _openRgb = new OpenRGBClient(name: ClientName);
                _openRgb.Connect();

                IList<IRGBDevice> devices = new List<IRGBDevice>();
                int deviceCount = _openRgb.GetControllerCount();

                for (int i = 0; i < deviceCount; i++)
                {
                    var device = _openRgb.GetControllerData(i);

                    //if the device doesn't have a direct mode, don't add it
                    if (!device.Modes.Any(m => m.Name == "Direct"))
                        continue;

                    IOpenRGBDevice rgbDevice = null;
                    switch (device.Type)
                    {
                        case OpenRGBDeviceType.Keyboard:
                            rgbDevice = new OpenRGBKeyboardDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.Keyboard, device));
                            break;
                        case OpenRGBDeviceType.Mouse:
                            rgbDevice = new GenericOpenRGBDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.Mouse, device));
                            break;
                        case OpenRGBDeviceType.Motherboard:
                            rgbDevice = new GenericOpenRGBDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.Mainboard, device));
                            break;
                        case OpenRGBDeviceType.Dram:
                            rgbDevice = new GenericOpenRGBDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.DRAM, device));
                            break;
                        case OpenRGBDeviceType.Gpu:
                            rgbDevice = new GenericOpenRGBDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.GraphicsCard, device));
                            break;
                        case OpenRGBDeviceType.Cooler:
                            rgbDevice = new GenericOpenRGBDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.Cooler, device));
                            break;
                        case OpenRGBDeviceType.Ledstrip:
                            rgbDevice = new GenericOpenRGBDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.LedStripe, device));
                            break;
                        case OpenRGBDeviceType.Mousemat:
                            rgbDevice = new GenericOpenRGBDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.Mousepad, device));
                            break;
                        case OpenRGBDeviceType.Headset:
                            rgbDevice = new GenericOpenRGBDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.Headset, device));
                            break;
                        case OpenRGBDeviceType.HeadsetStand:
                            rgbDevice = new GenericOpenRGBDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.HeadsetStand, device));
                            break;
                        default:
                            rgbDevice = new GenericOpenRGBDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.Unknown, device));
                            break;
                    }

                    if ((rgbDevice != null) && loadFilter.HasFlag(rgbDevice.DeviceInfo.DeviceType))
                    {
                        rgbDevice.Initialize(new OpenRGBUpdateQueue(UpdateTrigger, i, _openRgb, device));
                        devices.Add(rgbDevice);
                    }
                }

                UpdateTrigger?.Start();

                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
                IsInitialized = true;
            }
            catch
            {
                if (throwExceptions)
                    throw;
            }

            return true;
        }

        public void ResetDevices()
        { }

        public void Dispose()
        {
            _openRgb?.Dispose();
        }

        #endregion
    }
}
