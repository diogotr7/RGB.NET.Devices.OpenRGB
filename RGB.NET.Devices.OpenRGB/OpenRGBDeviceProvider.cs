using OpenRGB.NET;
using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenRGBDeviceType = OpenRGB.NET.Enums.DeviceType;
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

        /// <inheritdoc />
        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                UpdateTrigger?.Stop();
                _openRgb = new OpenRGBClient(name: "RGB.NET");
                _openRgb.Connect();

                IList<IRGBDevice> devices = new List<IRGBDevice>();
                int deviceCount = _openRgb.GetControllerCount();

                for (int i = 0; i < deviceCount; i++)
                {
                    var device = _openRgb.GetControllerData(i);

                    IOpenRGBDevice rgbDevice = null;
                    switch (device.Type)
                    {
                        case OpenRGBDeviceType.Keyboard:
                            rgbDevice = new OpenRGBKeyboardDevice(new OpenRGBDeviceInfo(i, RGBDeviceType.Keyboard, device));
                            break;
                            //TODO: other device types
                        default:
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
