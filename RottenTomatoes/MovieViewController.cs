using System;
using MonoTouch.UIKit;
using RottenApi;

namespace RottenTomatoes
{
    public class MovieViewController : UIViewController
    {
        private Movie _movie;

        public MovieViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBar.TintColor = UIColor.White;
            NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB(207, 68, 0);
            NavigationController.NavigationBar.SetTitleTextAttributes(new UITextAttributes(){ TextColor = UIColor.White });
            View.BackgroundColor = UIColor.White;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.SetNavigationBarHidden(false, true);
            Title = _movie.Title;
        }

        public void InitWithMovie(Movie movie)
        {
            _movie = movie;
        }
    }
}

