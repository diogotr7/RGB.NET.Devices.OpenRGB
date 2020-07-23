using OpenRGB.NET;
using OpenRGB.NET.Enums;
using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RGB.NET.Devices.OpenRGB
{

    public class OpenRGBDeviceProvider : IRGBDeviceProvider
    {

        #region Properties & Fields

        private static OpenRGBDeviceProvider _instance;

        public static OpenRGBDeviceProvider Instance => _instance ?? new OpenRGBDeviceProvider();

        public bool IsInitialized { get; private set; }

        public bool HasExclusiveAccess => false;

        public IEnumerable<IRGBDevice> Devices { get; private set; }

        public OpenRGBClient openRgb { get; private set; }

        public DeviceUpdateTrigger UpdateTrigger { get; }

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

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                UpdateTrigger?.Stop();
                openRgb = new OpenRGBClient(name: "RGB.NET");
                openRgb.Connect();

                IList<IRGBDevice> devices = new List<IRGBDevice>();

                foreach (var (index, device) in openRgb.GetAllControllerData().Select((value, i) => (i, value)))
                {
                    IOpenRGBDevice rgbDevice = null;
                    switch (device.Type)
                    {
                        case OpenRGBDeviceType.Keyboard:
                            rgbDevice = new OpenRGBKeyboardDevice(new OpenRGBDeviceInfo(RGBDeviceType.Keyboard, device.Name));
                            break;
                        case OpenRGBDeviceType.Mouse:
                            rgbDevice = new OpenRGBMouseDevice(new OpenRGBDeviceInfo(RGBDeviceType.Mouse, device.Name));
                            break;
                        default:
                            rgbDevice = new OpenRGBUnspecifiedRGBDevice(new OpenRGBDeviceInfo(RGBDeviceType.Unknown, device.Name));
                            break;
                    }

                    if ((rgbDevice != null) && loadFilter.HasFlag(rgbDevice.DeviceInfo.DeviceType))
                    {
                        rgbDevice.Initialize(new OpenRGBUpdateQueue(UpdateTrigger, index, openRgb, device.Leds.Length), device.Leds.Length);
                        devices.Add(rgbDevice);
                    }
                }

                UpdateTrigger?.Start();

                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
                IsInitialized = true;
            }
            catch (Exception ex)
            {
                if (throwExceptions)
                    throw;
            }

            return true;
        }

        /// <inheritdoc />
        public void ResetDevices()
        {
            //TODO DarthAffe 11.11.2017: Implement
        }

        /// <inheritdoc />
        public void Dispose()
        { }

        #endregion
    }
}
