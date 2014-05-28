using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using CoinKeeper.Logic.IoCContainer;
using TinyIoC;
using RottenApi;

namespace RottenTomatoes
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }

        private UIWindow window;
        private BoxOfficeViewController _mainController;

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {

            TinyIoCContainer container = TinyIoCContainer.Current;
            Container.Init(new TinyIocAdapter(container));

            container.Register<IServerApi, ServerApi>().AsSingleton();

            // create a new window instance based on the screen size
            window = new UIWindow(UIScreen.MainScreen.Bounds);
			
            _mainController = new BoxOfficeViewController();
            window.RootViewController = _mainController;
			
            // make the window visible
            window.MakeKeyAndVisible();
			
            return true;
        }
    }
}

