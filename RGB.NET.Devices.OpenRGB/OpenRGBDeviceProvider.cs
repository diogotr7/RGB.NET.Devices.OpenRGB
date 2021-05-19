using OpenRGB.NET;
using OpenRGB.NET.Models;
using RGB.NET.Core;
using RGB.NET.Devices.OpenRGB.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBDeviceProvider : AbstractRGBDeviceProvider
    {
        #region Properties & Fields

        private readonly List<OpenRGBClient> _clients = new List<OpenRGBClient>();

        private static OpenRGBDeviceProvider? _instance;

        public static OpenRGBDeviceProvider Instance => _instance ?? new OpenRGBDeviceProvider();

        public List<OpenRGBServerDefinition> DeviceDefinitions { get; } = new List<OpenRGBServerDefinition>();

        public bool ForceAddAllDevices { get; set; }

        public RGBDeviceType PerZoneDeviceFlag { get; } = RGBDeviceType.LedStripe | RGBDeviceType.Mainboard;

        #endregion

        #region Constructors

        public OpenRGBDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(OpenRGBDeviceProvider)}");
            _instance = this;
        }

        #endregion

        #region Methods

        protected override void InitializeSDK()
        {
            foreach (OpenRGBServerDefinition? deviceDefinition in DeviceDefinitions)
            {
                try
                {
                    OpenRGBClient? openRgb = new OpenRGBClient(ip: deviceDefinition.Ip, port: deviceDefinition.Port, name: deviceDefinition.ClientName, autoconnect: true);
                    _clients.Add(openRgb);
                    deviceDefinition.Connected = true;
                }
                catch (Exception e)
                {
                    deviceDefinition.Connected = false;
                    deviceDefinition.LastError = e.Message;
                    Throw(e, false);
                }
            }
        }

        protected override IEnumerable<IRGBDevice> LoadDevices()
        {
            foreach (OpenRGBClient? openRgb in _clients)
            {
                int deviceCount = openRgb.GetControllerCount();

                for (int i = 0; i < deviceCount; i++)
                {
                    Device? device = openRgb.GetControllerData(i);

                    //if the device doesn't have a direct mode, don't add it
                    if (!device.Modes.Any(m => m.Name == "Direct") && !ForceAddAllDevices)
                        continue;

                    OpenRGBUpdateQueue? updateQueue = new OpenRGBUpdateQueue(GetUpdateTrigger(), i, openRgb, device);

                    if (PerZoneDeviceFlag.HasFlag(Helper.GetRgbNetDeviceType(device.Type)))
                    {
                        int totalLedCount = 0;

                        for (int zoneIndex = 0; zoneIndex < device.Zones.Length; zoneIndex++)
                        {
                            Zone zone = device.Zones[zoneIndex];

                            if (zone.LedCount == 0)
                                continue;

                            yield return new OpenRGBZoneDevice(new OpenRGBZoneDeviceInfo(device), totalLedCount, zone, updateQueue);
                            totalLedCount += (int)zone.LedCount;
                        }
                    }
                    else
                    {
                        yield return new OpenRGBGenericDevice(new OpenRGBGenericDeviceInfo(device), updateQueue);
                    }
                }
            }
        }

        public override void Dispose()
        {
            foreach (OpenRGBClient? client in _clients)
            {
                try { client?.Dispose(); }
                catch { /* at least we tried */ }
            }

            _clients.Clear();
            DeviceDefinitions.Clear();
            Devices = Enumerable.Empty<IRGBDevice>();
        }
        #endregion
    }
}
