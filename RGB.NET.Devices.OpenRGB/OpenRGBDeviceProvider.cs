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

        private static OpenRGBDeviceProvider? _instance;

        public static OpenRGBDeviceProvider Instance => _instance ?? new OpenRGBDeviceProvider();

        public bool IsInitialized { get; private set; }

        public IEnumerable<IRGBDevice> Devices { get; private set; } = Enumerable.Empty<IRGBDevice>();

        public List<OpenRGBServerDefinition> DeviceDefinitions { get; } = new List<OpenRGBServerDefinition>();

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

        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                UpdateTrigger.Stop();

                IList<IRGBDevice> devices = new List<IRGBDevice>();

                var modelCounter = new Dictionary<string, int>();

                foreach (var deviceDefinition in DeviceDefinitions)
                {
                    try
                    {
                        var openRgb = new OpenRGBClient(ip: deviceDefinition.Ip, port: deviceDefinition.Port, name: deviceDefinition.ClientName, autoconnect: true);

                        int deviceCount = openRgb.GetControllerCount();

                        for (int i = 0; i < deviceCount; i++)
                        {
                            var device = openRgb.GetControllerData(i);

                            //if the device doesn't have a direct mode, don't add it
                            if (!device.Modes.Any(m => m.Name == "Direct"))
                                continue;

                            if (!loadFilter.HasFlag(Helper.GetRgbNetDeviceType(device.Type)))
                                continue;

                            OpenRGBUpdateQueue? updateQueue = null;
                            foreach (var dev in GetRGBDevice(i, device, modelCounter))
                            {
                                if (updateQueue is null)
                                    updateQueue = new OpenRGBUpdateQueue(UpdateTrigger, i, openRgb, device);

                                dev.Initialize(updateQueue);
                                devices.Add(dev);
                            }
                        }
                    }
                    catch when (!throwExceptions) { }
                }

                UpdateTrigger?.Start();

                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
                IsInitialized = true;
            }
            catch when (!throwExceptions) { }

            return true;
        }

        public void Dispose()
        {
            try { UpdateTrigger.Dispose(); }
            catch { /* at least we tried */ }

            Devices = Enumerable.Empty<IRGBDevice>();
        }

        private static IEnumerable<IOpenRGBDevice> GetRGBDevice(int i, Device device, Dictionary<string, int> modelCounter)
        {
            var type = Helper.GetRgbNetDeviceType(device.Type);
            var totalLedCount = 0;

            //should probably make this an option
            if (type == RGBDeviceType.LedStripe)
            {
                for (int zoneIndex = 0; zoneIndex < device.Zones.Length; zoneIndex++)
                {
                    Zone zone = device.Zones[zoneIndex];

                    if (zone.LedCount == 0)
                        continue;

                    yield return new OpenRGBZoneDevice(new OpenRGBZoneDeviceInfo(i, type, device, modelCounter, zoneIndex), totalLedCount, zone);
                    totalLedCount += (int)zone.LedCount;
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
