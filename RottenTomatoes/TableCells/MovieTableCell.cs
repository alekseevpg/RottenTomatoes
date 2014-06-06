using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using RottenApi;
using System.Net;

namespace RottenTomatoes.TableCells
{
    public class MovieTableCell : UITableViewCell
    {
        public static readonly NSString CellId = new NSString("MovieTableCell");
        private UIImageView _thumbnailView, _freshView;
        private UILabel _titleLbl, _ratingLbl, _actorsLbl, _timingLbl, _releaseLbl;

        public MovieTableCell(IntPtr handle)
            : base(handle)
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            SeparatorInset = new UIEdgeInsets(0, 0, 0, 0);
            _thumbnailView = new UIImageView(new RectangleF(0, 0, 60, 90));
            Add(_thumbnailView);

            _titleLbl = new UILabel(new RectangleF(65, 5, 255, 17))
            {
                AdjustsFontSizeToFitWidth = true,
                Font = UIFont.FromName("HelveticaNeue-Bold", 15),
                TextColor = UIColor.FromRGB(40, 92, 171),
            };
            Add(_titleLbl);

            _freshView = new UIImageView(new RectangleF(65, 25, 15, 15));
            Add(_freshView);

            _ratingLbl = CreateLabel(new RectangleF(85, 25, 220, 15));

            _actorsLbl = CreateLabel(new RectangleF(65, 40, 255, 15));

            _timingLbl = CreateLabel(new RectangleF(65, 55, 255, 15));

            _releaseLbl = CreateLabel(new RectangleF(65, 70, 255, 15));
        }

        private UILabel CreateLabel(RectangleF frame)
        {
            var label = new UILabel(frame)
            {
                AdjustsFontSizeToFitWidth = true,
                Font = UIFont.FromName("HelveticaNeue", 13),
                TextColor = UIColor.Black,
            };
            Add(label);
            return label;
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
            AccessibilityLabel = movie.Ratings.CriticsRating;
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
            _actorsLbl.Text = movie.GetFormattedCast();
            _timingLbl.Text = string.Format("{0}, {1}", movie.MpaaRating, movie.GetFormattedRuntime());
            _releaseLbl.Text = movie.ReleaseDates.GetFormattedTheaterDate();
        }
    }
}
