using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OpenRGB.NET;
using RGB.NET.Core;
using RGB.NET.Devices.OpenRGB.Native;

namespace RGB.NET.Devices.OpenRGB
{
   
    public class OpenRGBDeviceProvider : IRGBDeviceProvider
    {

            #region Properties & Fields

            private static OpenRGBDeviceProvider _instance;

            public static OpenRGBDeviceProvider Instance => _instance ?? new OpenRGBDeviceProvider();

            public bool IsInitialized { get; private set; }

            public bool HasExclusiveAccess { get; private set; }

            /// <inheritdoc />
            public IEnumerable<IRGBDevice> Devices { get; private set; }

            public OpenRGBClient openRgb { get; private set; }

            public DeviceUpdateTrigger UpdateTrigger { get;  }

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
                    openRgb = new OpenRGBClient(port: 1337, name: "JackNet RGBSync");
                    openRgb.Connect();
                    int controllerCount = openRgb.GetControllerCount();
                    var devices = new List<OpenRGBDevice>();
                    //IOpenRGBDevice _device = null;
                    IList<IRGBDevice> _devices = new List<IRGBDevice>();

                    for (int i = 0; i < controllerCount; i++)
                    {
                        devices.Add(openRgb.GetControllerData(i));
                    }

                    for (int i = 0; i < devices.Count; i++)
                    {
                        OpenRGBUpdateQueue updateQueue = new OpenRGBUpdateQueue(UpdateTrigger, i, openRgb, devices[i].leds.Length);
                        IOpenRGBDevice _device = new OpenRGBUnspecifiedRGBDevice(new OpenRGBDeviceInfo(RGBDeviceType.Unknown, i + " " + devices[i].name, "OpenRGB"));
                        _device.Initialize(updateQueue, devices[i].leds.Length);
                        _devices.Add(_device);

                        /*
                        var list = new OpenRGBColor[devices[i].leds.Length];
                        for (int j = 0; j < devices[i].leds.Length; j++)
                        {
                            list[j] = new OpenRGBColor(0, 255, 0);
                        }
                        openRgb.UpdateLeds(i, list);
                        */
                    }
                    UpdateTrigger?.Start();
                    Devices = new ReadOnlyCollection<IRGBDevice>(_devices);
                    IsInitialized = true;
                }
                catch (Exception ex)
                {
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
