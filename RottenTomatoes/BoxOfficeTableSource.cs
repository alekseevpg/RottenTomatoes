using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using CoinKeeper.Logic.IoCContainer;
using RottenApi;
using System.Net;
using System.IO;

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
        private UIImageView _thumbnailView;
        private UILabel _titleLbl;
        UIImageView _freshView;
        UILabel _ratingLbl;
        UILabel _actorsLbl;
        UILabel _timingLbl;
        UILabel _releaseLbl;

        public BoxOfficeTableCell(IntPtr handle)
            : base(handle)
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            SeparatorInset = new UIEdgeInsets(0, 0, 0, 0);
            _thumbnailView = new UIImageView(new RectangleF(0, 0, 60, 90));
            Add(_thumbnailView);

            _titleLbl = new UILabel(new RectangleF(65, 5, 255, 15))
            {
                AdjustsFontSizeToFitWidth = true,
                Font = UIFont.FromName("HelveticaNeue-Bold", 15),
                TextColor = UIColor.FromRGB(40, 92, 171),
            };
            Add(_titleLbl);

            _freshView = new UIImageView(new RectangleF(65, 25, 15, 15));
            Add(_freshView);

            _ratingLbl = new UILabel(new RectangleF(85, 25, 220, 15))
            {
                AdjustsFontSizeToFitWidth = true,
                Font = UIFont.FromName("HelveticaNeue", 13),
                TextColor = UIColor.Black,
            };
            Add(_ratingLbl);


            _actorsLbl = new UILabel(new RectangleF(65, 40, 255, 15))
            {
                AdjustsFontSizeToFitWidth = true,
                Font = UIFont.FromName("HelveticaNeue", 13),
                TextColor = UIColor.Black,
            };
            Add(_actorsLbl);

            _timingLbl = new UILabel(new RectangleF(65, 55, 255, 15))
            {
                AdjustsFontSizeToFitWidth = true,
                Font = UIFont.FromName("HelveticaNeue", 13),
                TextColor = UIColor.Black,
            };
            Add(_timingLbl);

            _releaseLbl = new UILabel(new RectangleF(65, 70, 255, 15))
            {
                AdjustsFontSizeToFitWidth = true,
                Font = UIFont.FromName("HelveticaNeue", 13),
                TextColor = UIColor.Black,
            };
            Add(_releaseLbl);
        }

        public void UpdateCell(Movie movie)
        {
            var webClient = new WebClient();
            webClient.DownloadDataCompleted += (s, e) =>
            {
                InvokeOnMainThread(() =>
                {
                    _thumbnailView.Image = null;
                    if (e.Error == null)
                    {
                        _thumbnailView.Image = UIImage.LoadFromData(NSData.FromArray(e.Result));
                    }
                });
            };
            webClient.DownloadDataAsync(new Uri(movie.Posters.Profile));
            _titleLbl.Text = movie.Title;
            switch (movie.Ratings.CriticsRating)
            {
                case "Certified Fresh":
                case "Fresh":
                    _freshView.Image = Images.Get("Content/fresh.png");
                    break;
                case "Rotten":
                    _freshView.Image = Images.Get("Content/rotten.png");
                    break;
                default:
                    _freshView.Image = null;
                    break;
            }

            _ratingLbl.Text = string.Format("{0}%", movie.Ratings.CriticsScore);

            _actorsLbl.Text = string.Empty;
            if (movie.AbridgedCast.Count != 0)
            {
                _actorsLbl.Text = movie.AbridgedCast.Count == 1
                    ? movie.AbridgedCast[0].Name 
                    : string.Format("{0}, {1}", movie.AbridgedCast[0].Name, movie.AbridgedCast[1].Name);
            }

            _timingLbl.Text = string.Format("{0}, {1}", movie.MpaaRating, movie.Runtime);

            _releaseLbl.Text = movie.ReleaseDates.Theater.ToShortDateString();

        }
    }
}

