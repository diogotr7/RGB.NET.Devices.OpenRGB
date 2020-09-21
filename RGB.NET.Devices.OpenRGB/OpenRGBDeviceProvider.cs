using OpenRGB.NET;
using OpenRGB.NET.Enums;
using OpenRGB.NET.Models;
using RGB.NET.Core;
using RGB.NET.Devices.OpenRGB.Generic;
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

        public string ClientName { get; set; } = "RGB.NET";

        public string IpAddress { get; set; } = "127.0.0.1";

        public int Port { get; set; } = 6742;

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
                _openRgb = new OpenRGBClient(ip: IpAddress, port: Port, name: ClientName);
                _openRgb.Connect();

                IList<IRGBDevice> devices = new List<IRGBDevice>();
                int deviceCount = _openRgb.GetControllerCount();
                var modelCounter = new Dictionary<string, int>();

                for (int i = 0; i < deviceCount; i++)
                {
                    var device = _openRgb.GetControllerData(i);

                    //if the device doesn't have a direct mode, don't add it
                    if (!device.Modes.Any(m => m.Name == "Direct"))
                        continue;

                    if (!loadFilter.HasFlag(Helper.GetRgbNetDeviceType(device.Type)))
                        continue;

                    OpenRGBUpdateQueue updateQueue = null;
                    foreach(var dev in GetRGBDevice(i, device, modelCounter))
                    {
                        if (updateQueue is null)
                            updateQueue = new OpenRGBUpdateQueue(UpdateTrigger, i, _openRgb, device);

                        dev.Initialize(updateQueue);
                        devices.Add(dev);
                    }
                }

                UpdateTrigger?.Start();

                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
                IsInitialized = true;
            }
            catch when (!throwExceptions)
            {
            }

            return true;
        }

        public void ResetDevices()
        { }

        public void Dispose()
        {
            _openRgb?.Dispose();
        }

        private static IEnumerable<IOpenRGBDevice> GetRGBDevice( int i, Device device, Dictionary<string, int> modelCounter)
        {
            var type = Helper.GetRgbNetDeviceType(device.Type);
            if (type == RGBDeviceType.LedStripe)
            {
                var initial = LedId.LedStripe1;
                foreach (var zone in device.Zones)
                {
                    yield return new OpenRGBCustomDevice(new OpenRGBDeviceInfo(i, type, device, modelCounter), initial, zone);
                    initial += (int)zone.LedCount;
                }
            }
            else
            {
                yield return new OpenRGBGenericDevice(new OpenRGBDeviceInfo(i, type, device, modelCounter));
            }
        }

        #endregion
    }
}
