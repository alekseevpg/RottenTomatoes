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
            _actorsLbl.Text = movie.GetFormattedCast();
            _timingLbl.Text = string.Format("{0}, {1}", movie.MpaaRating, movie.GetFormattedRuntime());
            _releaseLbl.Text = movie.ReleaseDates.GetFormattedTheaterDate();
        }
    }
}
