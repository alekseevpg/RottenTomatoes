﻿using System;
using MonoTouch.UIKit;
using System.Drawing;
using RottenApi;
using CoinKeeper.Logic.IoCContainer;
using MonoTouch.Foundation;
using System.Threading;

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
            _tableSource.ReloadSectionNeeded += () => InvokeOnMainThread(_table.ReloadData);
            _tableSource.MovieSelected += movie =>
            {
                _movieController = new MovieViewController();
                _movieController.InitWithMovie(movie);
                NavigationController.PushViewController(_movieController, true);
            };
            _tableSource.UpdateMovies();
            _table = new UITableView(new RectangleF(0, 20, 320, 460 + Device.PhoneHeightOffset), UITableViewStyle.Plain);
            _table.RegisterClassForCellReuse(typeof(MovieTableCell), MovieTableCell.CellId); 
            _table.BackgroundColor = UIColor.Clear;
            _table.Source = _tableSource;

            View.BackgroundColor = UIColor.FromRGB(66, 117, 2);
            View.Add(_table);
            UIRefreshControl refreshControl = new UIRefreshControl();
            refreshControl.TintColor = UIColor.White;
            refreshControl.AddTarget((sender, e) =>
            {
                _tableSource.UpdateMovies(() => InvokeOnMainThread(() =>
                {
                    refreshControl.EndRefreshing();
                }));
            }, UIControlEvent.ValueChanged);
            _table.Add(refreshControl);

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NavigationController.SetNavigationBarHidden(true, true);
        }
    }
}

