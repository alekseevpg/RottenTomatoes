using Foundation;
using UIKit;
using TinyIoC;
using RottenApi;
using RottenTomatoes.Helpers.IoCContainer;

namespace RottenTomatoes
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {

        private UIWindow window;
        private MainViewController _mainController;
        private UINavigationController _navController;

        static void Main(string[] args)
        {
            UIApplication.Main(args, null, "AppDelegate");
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {

            TinyIoCContainer container = TinyIoCContainer.Current;
            Container.Init(new TinyIocAdapter(container));

            container.Register<IServerApi, ServerApi>().AsSingleton();

            window = new UIWindow(UIScreen.MainScreen.Bounds);

            _mainController = new MainViewController();

            _navController = new UINavigationController(_mainController);
            window.RootViewController = _navController;

            window.MakeKeyAndVisible();

            return true;
        }
    }
}