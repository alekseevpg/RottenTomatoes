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

    public class MovieInfoTableCell : UITableViewCell
    {
        public static readonly NSString CellId = new NSString("MovieInfoTableCell");
        private UILabel _synopsisLbl, _directorLbl, _ratedLbl, _runningTime, _genreLbl, _dvdLbl, _theatresLbl;

        public MovieInfoTableCell(IntPtr handle)
            : base(handle)
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            SeparatorInset = new UIEdgeInsets(0, 0, 0, 0);

            _synopsisLbl = CreateLabel(new RectangleF(5, 0, 315, 130));
            _synopsisLbl.AdjustsFontSizeToFitWidth = false;
            _synopsisLbl.Lines = 7;

//            var showMoreBtn = new UIButton(new RectangleF(5, 295, 315, 20));
//            showMoreBtn.BackgroundColor = UIColor.Red;
//            showMoreBtn.SetTitle("More..", UIControlState.Normal);
//            showMoreBtn.SetTitleColor(UIColor.Blue, UIControlState.Normal);
//            showMoreBtn.TouchUpInside += (sender, e) =>
//            {
//
//            };
//            Add(showMoreBtn);
//            BringSubviewToFront(showMoreBtn);

            _directorLbl = CreateLabel(new RectangleF(5, 135, 315, 20));

            _ratedLbl = CreateLabel(new RectangleF(5, 155, 255, 20));

            _runningTime = CreateLabel(new RectangleF(5, 175, 255, 20));

            _genreLbl = CreateLabel(new RectangleF(5, 195, 255, 20));

            _theatresLbl = CreateLabel(new RectangleF(5, 215, 255, 20));

            _dvdLbl = CreateLabel(new RectangleF(5, 235, 255, 20));
        }

        public void UpdateCell(Movie movie, MovieInfo mInfo)
        {
            UpdateText(_synopsisLbl, string.Format("Synopsis: {0}", mInfo.Synopsis), 10);

            UpdateText(_directorLbl, string.Format("Director: {0}", mInfo.GetFormattedDirector()), 10);

            UpdateText(_ratedLbl, string.Format("Rated: {0}", movie.MpaaRating), 6);

            UpdateText(_runningTime, string.Format("Running Time: {0}", movie.GetFormattedRuntime()), 13);

            UpdateText(_genreLbl, string.Format("Genre: {0}", mInfo.GetFormattedGenres()), 6);

            UpdateText(_theatresLbl, string.Format("Theater Release: {0}", movie.ReleaseDates.GetFormattedTheaterDate()), 16);

            UpdateText(_dvdLbl, string.Format("DVD Release: {0}", movie.ReleaseDates.GetFormattedDvdDate), 12);
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

        private void UpdateText(UILabel label, string text, int boldIndexRange)
        {
            var boldAttr = new UIStringAttributes
            {
                Font = UIFont.FromName("HelveticaNeue-Bold", 13)
            };
            var attrStr = new NSMutableAttributedString(text);
            attrStr.SetAttributes(boldAttr, new NSRange(0, boldIndexRange));
            label.AttributedText = attrStr;
        }

    }
}
