using MonoTouch.UIKit;
using RottenApi;
using System.Drawing;
using CoinKeeper.Logic.IoCContainer;
using RottenTomatoes.TableCells;

namespace RottenTomatoes
{
    public class MovieViewController : UIViewController
    {
        private Movie _movie;
        private MovieTableSource _movieSource;
        private UIActivityIndicatorView _progressView;
        private UITableView _table;
        private UIView _stabView;

        public void InitWithMovie(Movie movie)
        {
            _movie = movie;
        }

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
            _table.RegisterClassForCellReuse(typeof(ReviewTableCell), ReviewTableCell.CellId);
            _table.BackgroundColor = UIColor.White;
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

            Container.Resolve<IServerApi>().GetMovieReviews(_movie.Id, reviews =>
            {
                InvokeOnMainThread(() =>
                {
                    _movieSource.UpdateMovieReviews(reviews);
                    TryShowTable();
                });
            });
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
}

