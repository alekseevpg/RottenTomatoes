using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using CoinKeeper.Logic.IoCContainer;
using RottenApi;
using System.Net;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace RottenTomatoes
{

    public enum MoviesType
    {
        Opening = 0,
        BoxOffice = 1,
        InTheaters = 2,
    }

    public class MoviesTableSource : UITableViewSource
    {
        public event Action<int> ReloadSectionNeeded = delegate {};
        public event Action<Movie> MovieSelected = delegate {};

        private SortedDictionary <MoviesType, MovieList> _movies = new SortedDictionary<MoviesType, MovieList>();

        public MoviesTableSource()
        {
        }

        public void InitSource()
        {
            Container.Resolve<IServerApi>().GetOpeningThisWeek(movies =>
            {
                _movies.Add(MoviesType.Opening, movies);
                ReloadSectionNeeded(0);
            });

            Container.Resolve<IServerApi>().GetBoxOfficeMovies(movies =>
            {
                _movies.Add(MoviesType.BoxOffice, movies);
                ReloadSectionNeeded(0);
            });

            Container.Resolve<IServerApi>().GetAlsoInTheaters(movies =>
            {
                _movies.Add(MoviesType.InTheaters, movies);
                ReloadSectionNeeded(0);
            });
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return _movies.Count;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            if (_movies.ContainsKey((MoviesType)section))
                return _movies[(MoviesType)section].Movies.Count;
            return 0;
        }

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 90.5f;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            BoxOfficeTableCell cell = (BoxOfficeTableCell)tableView.DequeueReusableCell(BoxOfficeTableCell.CellId, indexPath);

            cell.UpdateCell(_movies[(MoviesType)indexPath.Section].Movies[indexPath.Row]);
            return cell;
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

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            MovieSelected(_movies[(MoviesType)indexPath.Section].Movies[indexPath.Row]);
        }
    }

    public class BoxOfficeTableCell : UITableViewCell
    {
        public static readonly NSString CellId = new NSString("BoxOfficeTableCell");
        private UIImageView _thumbnailView, _freshView;
        private UILabel _titleLbl, _ratingLbl, _actorsLbl, _timingLbl, _releaseLbl;

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
                    _freshView.Image = Images.Get("Content/CF_300x300.png");
                    break;
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

            _timingLbl.Text = string.Format("{0}, {1}hr. {2}min.", movie.MpaaRating, movie.Runtime / 60, movie.Runtime % 60);

            _releaseLbl.Text = movie.ReleaseDates.Theater.ToShortDateString();

        }
    }
}

