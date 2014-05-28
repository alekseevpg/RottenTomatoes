using System;
using MonoTouch.UIKit;
using System.Drawing;
using RottenApi;
using CoinKeeper.Logic.IoCContainer;
using MonoTouch.Foundation;

namespace RottenTomatoes
{
    public class BoxOfficeViewController : UIViewController
    {
        private BoxOfficeTableSource _tableSource;
        private UITableView _table;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _tableSource = new BoxOfficeTableSource();
            _tableSource.ReloadSectionNeeded += section =>
            {
                InvokeOnMainThread(() =>
                {
                    _table.ReloadData();
                });
            };
            _tableSource.InitSource();
            _table = new UITableView(new RectangleF(0, 20, 320, 460 + Device.PhoneHeightOffset), UITableViewStyle.Plain);
            _table.RegisterClassForCellReuse(typeof(BoxOfficeTableCell), BoxOfficeTableCell.CellId); 
            _table.BackgroundColor = UIColor.Red;
            _table.Source = _tableSource;


            View.BackgroundColor = UIColor.Green;
            View.Add(_table);
        }
    }
}

