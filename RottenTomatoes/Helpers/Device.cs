using System;
using MonoTouch.UIKit;

namespace RottenTomatoes
{
    public static class Device
    {
        public static int PhoneHeightOffset { get; set; }

        static Device()
        {
            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone && UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale >= 1136)
                PhoneHeightOffset = 88;
        }
    }
}

