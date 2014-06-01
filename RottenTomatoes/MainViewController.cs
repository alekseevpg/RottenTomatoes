using System;
using MonoTouch.UIKit;
using System.Drawing;
using RottenApi;
using CoinKeeper.Logic.IoCContainer;
using MonoTouch.Foundation;

namespace RottenTomatoes
{
    public class MainViewController : UIViewController
    {
        private MoviesTableSource _tableSource;
        private UITableView _table;

        private MovieViewController _movieController;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            EdgesForExtendedLayout = UIRectEdge.None;

            _tableSource = new MoviesTableSource();
            _tableSource.ReloadSectionNeeded += section => InvokeOnMainThread(_table.ReloadData);
            _tableSource.MovieSelected += movie =>
            {
                if (_movieController == null)
                    _movieController = new MovieViewController();
                _movieController.InitWithMovie(movie);
                NavigationController.PushViewController(_movieController, true);
            };
            _tableSource.InitSource();
            _table = new UITableView(new RectangleF(0, 20, 320, 460 + Device.PhoneHeightOffset), UITableViewStyle.Plain);
            _table.RegisterClassForCellReuse(typeof(BoxOfficeTableCell), BoxOfficeTableCell.CellId); 
            _table.BackgroundColor = UIColor.Clear;
            _table.Source = _tableSource;

            View.BackgroundColor = UIColor.FromRGB(66, 117, 2);
            View.Add(_table);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.SetNavigationBarHidden(true, true);
        }
    }
}

