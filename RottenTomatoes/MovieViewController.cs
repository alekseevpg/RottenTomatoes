using System;
using MonoTouch.UIKit;
using RottenApi;
using System.Drawing;

namespace RottenTomatoes
{
    public class MovieViewController : UITableViewController
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

            var _movieSource = new MovieTableSource();
            TableView.Source = _movieSource;

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

    public class MovieTableSource : UITableViewSource
    {
        public override int NumberOfSections(UITableView tableView)
        {
            return 4;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return 5;
        }

        public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
        {
            return new UITableViewCell();
        }

        public override float GetHeightForHeader(UITableView tableView, int section)
        {
            if (section == 0)
                return 0.0001f;
            return 40;
        }

        public override UIView GetViewForHeader(UITableView tableView, int section)
        {
            if (section == 0)
                return new UIView();
            var header = new UIView(new RectangleF(0, 0, 320, 40));
            header.BackgroundColor = UIColor.Black;

            var headerLabel = new UILabel(new RectangleF(10, 0, 320, 40))
            {
                Font = UIFont.FromName("HelveticaNeue-Bold", 15),
                TextColor = UIColor.White
            };

            switch (section)
            {
                case 1: 
                    headerLabel.Text = "Movie info";
                    break;
                case 2: 
                    headerLabel.Text = "Cast";
                    break;
                case 3: 
                    headerLabel.Text = "Critic reviews";
                    break;
                default:
                    headerLabel.Text = "";
                    break;
            }
            header.Add(headerLabel);
            return header;
        }
    }
}

