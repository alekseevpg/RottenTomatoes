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

            _table = new UITableView(new RectangleF(0, 0, 320, 480 + Device.PhoneHeightOffset));
            _table.RegisterClassForCellReuse(typeof(MovieTableCell), MovieTableCell.CellId);
            _table.RegisterClassForCellReuse(typeof(MovieInfoTableCell), MovieInfoTableCell.CellId);
            _table.RegisterClassForCellReuse(typeof(ActorTableCell), ActorTableCell.CellId);
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
            _movieSource = new MovieTableSource(_movie);
            Container.Resolve<IServerApi>().GetMovieInfo(_movie.Id, mInfo =>
            {
                InvokeOnMainThread(() =>
                {
                    _movieSource.UpdateMovieInfo(mInfo);
                    TryShowTable();
                });
            });

            Container.Resolve<IServerApi>().GetMovieCast(_movie.Id, cast =>
            {
                InvokeOnMainThread(() =>
                {
                    _movieSource.UpdateMovieCast(cast);
                    TryShowTable();
                });
            });
        }

        public void InitWithMovie(Movie movie)
        {
            _movie = movie;
        }

        private void TryShowTable()
        {
            if (_movieSource.IsSourceLoaded)
            {
                InvokeOnMainThread(() =>
                {
                    _progressView.StopAnimating();
                    _table.Source = _movieSource;
                    _table.ReloadData();
                    _table.Hidden = false;
                    _stabView.Hidden = true;
                });
            }
        }
    }

    public class MovieTableSource : UITableViewSource
    {
        private Movie _movie;
        private MovieInfoView _mInfoView;
        private MovieCast _cast;

        public bool IsSourceLoaded
        {
            get
            {
                if (_movie != null && _mInfoView != null && _cast != null)
                    return true;
                return false;
            }
        }

        public MovieTableSource(Movie movie)
        {
            _movie = movie;
        }

        public void UpdateMovieInfo(MovieInfo mInfo)
        {
            _mInfoView = new MovieInfoView(_movie, mInfo);
            _mInfoView.SizeToFit();
        }

        public void UpdateMovieCast(MovieCast cast)
        {
            _cast = cast;
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
                case 2:
                    return _cast.Cast.Count;
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
                case 2:
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
                    MovieInfoTableCell infoCell = (MovieInfoTableCell)tableView.DequeueReusableCell(MovieInfoTableCell.CellId);
                    infoCell.UpdateCell(_mInfoView);
                    return infoCell;
                case 2:
                    ActorTableCell actorCell = (ActorTableCell)tableView.DequeueReusableCell(ActorTableCell.CellId);
                    actorCell.UpdateActor(_cast.Cast[indexPath.Row]);
                    return actorCell;
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

