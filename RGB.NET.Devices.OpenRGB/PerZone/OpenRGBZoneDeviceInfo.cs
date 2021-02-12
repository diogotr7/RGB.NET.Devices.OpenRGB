using RGB.NET.Core;
using System;
using System.Collections.Generic;
using OpenRGBDevice = OpenRGB.NET.Models.Device;

namespace RGB.NET.Devices.OpenRGB
{
    public class OpenRGBZoneDeviceInfo : AbstractOpenRGBDeviceInfo
    {
        public string ZoneName { get; }

        public OpenRGBZoneDeviceInfo(int deviceIndex, RGBDeviceType deviceType, OpenRGBDevice device, Dictionary<string, int> modelCounter, int zoneIndex)
        {
            OpenRGBDeviceIndex = deviceIndex;
            DeviceType = deviceType;
            OpenRGBDevice = device;
            ZoneName = device.Zones[zoneIndex].Name;

            if (!string.IsNullOrEmpty(OpenRGBDevice.Vendor))
            {
                Manufacturer = OpenRGBDevice.Vendor;
                Model = OpenRGBDevice.Name.Replace(OpenRGBDevice.Vendor, "").Trim();
            }
            else
            {
                Manufacturer = "OpenRGB";
                Model = OpenRGBDevice.Name;
            }

            DeviceName = GetUniqueModelName(modelCounter);
        }

        protected override string GetUniqueModelName(Dictionary<string, int> modelCounter)
        {
            if (!modelCounter.TryGetValue(Model, out var Count))
                modelCounter.Add(Model, 0);

            Count++;
            modelCounter[Model] = Count;

            if (Count <= OpenRGBDevice.Zones.Length)
                return $"{Manufacturer} {Model} - {ZoneName}";
            else
                return $"{Manufacturer} {Model} {((Count - 1) / OpenRGBDevice.Zones.Length) + 1} - {ZoneName}";
        }
    }
}