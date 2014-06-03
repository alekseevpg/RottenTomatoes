using System;
using MonoTouch.UIKit;
using RottenApi;
using System.Drawing;
using MonoTouch.Foundation;
using CoinKeeper.Logic.IoCContainer;

namespace RottenTomatoes
{
    public class MovieViewController : UIViewController
    {
        private Movie _movie;

        public MovieViewController()
        {
        }

        MovieTableSource _movieSource;

        UIActivityIndicatorView _progressView;

        UITableView _table;

        UIView _stabView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBar.TintColor = UIColor.White;
            NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB(207, 68, 0);
            NavigationController.NavigationBar.SetTitleTextAttributes(new UITextAttributes(){ TextColor = UIColor.White });
            View.BackgroundColor = UIColor.White;

            _table = new UITableView(new RectangleF(0, 0, 320, 416 + Device.PhoneHeightOffset));
            _table.RegisterClassForCellReuse(typeof(MovieTableCell), MovieTableCell.CellId);
            _table.RegisterClassForCellReuse(typeof(MovieInfoTableCell), MovieInfoTableCell.CellId);
            _table.BackgroundColor = UIColor.Red;
            Add(_table);
            _stabView = new UIView(new RectangleF(0, 0, 320, 416 + Device.PhoneHeightOffset))
            {
                BackgroundColor = UIColor.FromWhiteAlpha(1, 0.7f),
            };
            _progressView = new UIActivityIndicatorView(new RectangleF(0, 0, 320, 416 + Device.PhoneHeightOffset));
            _progressView.Color = UIColor.Red;
            _stabView.Add(_progressView);
            Add(_stabView);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _table.Hidden = true;
            NavigationController.SetNavigationBarHidden(false, true);
            Title = _movie.Title;
            _progressView.StartAnimating();
            Container.Resolve<IServerApi>().GetMovieInfo(_movie.Id, mi =>
            {
                InvokeOnMainThread(() =>
                {
                    _movieSource = new MovieTableSource();
                    _table.Source = _movieSource;
                    _movieSource.UpdateMovie(_movie, mi);
                    _table.ReloadData();
                    _table.Hidden = false;
                    _stabView.Hidden = true;
                    _progressView.StopAnimating();
                });
            });
        }

        public void InitWithMovie(Movie movie)
        {
            _movie = movie;
        }
    }

    public class MovieTableSource : UITableViewSource
    {
        private Movie _movie;
        private MovieInfo _mInfo;
        private MovieInfoView _mInfoView;

        public void UpdateMovie(Movie movie, MovieInfo mInfo)
        {
            _mInfo = mInfo;
            _movie = movie;
            _mInfoView = new MovieInfoView(movie, mInfo);
            _mInfoView.SizeToFit();
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return 4;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            switch (section)
            {
                case 0:
                    return 1;
                case 1:
                    return 1;
                default:
                    return 5;
            }
        }

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    return 90.5f;
                case 1:
                    return _mInfoView.Height;
                default:
                    return 50;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    MovieTableCell movieCell = (MovieTableCell)tableView.DequeueReusableCell(MovieTableCell.CellId);
                    movieCell.UpdateCell(_movie);
                    return movieCell;
                case 1:
                    MovieInfoTableCell mInfoCell = (MovieInfoTableCell)tableView.DequeueReusableCell(MovieInfoTableCell.CellId);
                    mInfoCell.UpdateCell(_mInfoView);
                    return mInfoCell;
                default:
                    return new UITableViewCell();
            }
        }

        public override float GetHeightForHeader(UITableView tableView, int section)
        {
            return section == 0 ? 0.0001f : 40;
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

