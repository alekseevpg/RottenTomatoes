using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using CoinKeeper.Logic.IoCContainer;
using RottenApi;

namespace RottenTomatoes
{
    public class BoxOfficeTableSource : UITableViewSource
    {
        public event Action<int> ReloadSectionNeeded = delegate {};

        MovieList _openingMovies = null;
        MovieList _boxMovies = null;

        public BoxOfficeTableSource()
        {
          
        }

        public void InitSource()
        {
            Container.Resolve<IServerApi>().GetOpeningThisWeek(movies =>
            {
                _openingMovies = movies;
                ReloadSectionNeeded(0);
            });

            Container.Resolve<IServerApi>().GetBoxOfficeMovies(movies =>
            {
                _boxMovies = movies;
                ReloadSectionNeeded(0);
            });
        }

        public override int NumberOfSections(UITableView tableView)
        {
            int sections = 0;
            if (_openingMovies != null && _openingMovies.Movies.Count > 0)
                sections++;
            if (_boxMovies != null && _boxMovies.Movies.Count > 0)
                sections++;
            return sections;
        }

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 90.5f;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            BoxOfficeTableCell cell = (BoxOfficeTableCell)tableView.DequeueReusableCell(BoxOfficeTableCell.CellId, indexPath);

            switch (indexPath.Section)
            {
                case 0:
                    cell.UpdateCell(_openingMovies.Movies[indexPath.Row]);
                    break;
                case 1:
                    cell.UpdateCell(_boxMovies.Movies[indexPath.Row]);
                    break;

            }
            return cell;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            switch (section)
            {
                case 0: 
                    return 2;
                case 1: 
                    return 5;
                case 2: 
                    return 10;
                default:
                    return 0;
            }
        }

        public override float GetHeightForHeader(UITableView tableView, int section)
        {
            return 50;
        }

        public override UIView GetViewForHeader(UITableView tableView, int section)
        {
            var header = new UIView(new RectangleF(0, 0, 320, 50));
            header.BackgroundColor = UIColor.Black;

            var headerLabel = new UILabel(new RectangleF(10, 0, 320, 50))
            {
                Font = UIFont.FromName("HelveticaNeue-Bold", 15),
                TextColor = UIColor.White
            };

            switch (section)
            {
                case 0: 
                    headerLabel.Text = "Opening This Week";
                    break;
                case 1: 
                    headerLabel.Text = "Top Box Office";
                    break;
                case 2: 
                    headerLabel.Text = "Also in Theaters";
                    break;
                default:
                    headerLabel.Text = "";
                    break;
            }
            header.Add(headerLabel);
            return header;
        }
    }

    public class BoxOfficeTableCell : UITableViewCell
    {
        public static readonly NSString CellId = new NSString("BoxOfficeTableCell");
        private UIImageView _thumbnail;

        public BoxOfficeTableCell(IntPtr handle)
            : base(handle)
        {
            SeparatorInset = new UIEdgeInsets(0, 0, 0, 0);
            _thumbnail = new UIImageView(new RectangleF(0, 0, 60, 90));
            Add(_thumbnail);
        }

        public void UpdateCell(Movie movie)
        {
            using (var url = new NSUrl(movie.Posters.Profile))
            {
                using (var data = NSData.FromUrl(url))
                {
                    _thumbnail.Image = UIImage.LoadFromData(data);
                }
            }
        }
    }
}

