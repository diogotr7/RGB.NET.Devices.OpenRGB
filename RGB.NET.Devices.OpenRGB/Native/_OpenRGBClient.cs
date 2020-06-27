// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RGB.NET.Core;
using OpenRGB.NET;

namespace RGB.NET.Devices.OpenRGB.Native
{
    internal static class _OpenRGBClient
    {
        #region Libary Management

        public static void Reload()
        {
            UnloadSDK();
            LoadSDK();
        }

        public static void LoadSDK()
        {
        
        }

        public static void UnloadSDK()
        {

        }

        #endregion
    }
}
