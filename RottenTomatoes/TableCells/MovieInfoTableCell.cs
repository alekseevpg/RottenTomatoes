using System;
using UIKit;
using CoreGraphics;
using Foundation;
using RottenApi;

namespace RottenTomatoes.TableCells
{
    public class MovieInfoTableCell : UITableViewCell
    {
        public static readonly NSString CellId = new NSString("MovieInfoTableCell");

        private MovieInfoView _view;

        public MovieInfoTableCell(IntPtr handle)
            : base(handle)
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            SeparatorInset = new UIEdgeInsets(0, 0, 0, 0);
        }

        public void UpdateCell(MovieInfoView view)
        {
            if (_view != null)
            {
                _view.RemoveFromSuperview();
                _view.Dispose();
                _view = null;
            }
            _view = view;
            Add(_view);
        }
    }

    public class MovieInfoView : UIView
    {
        private UILabel _synopsisLbl, _directorLbl, _ratedLbl, _runningTime, _genreLbl, _dvdLbl, _theatresLbl;

        public nfloat Height
        {
            get
            {
                return _synopsisLbl.Frame.Height + _directorLbl.Frame.Height + _ratedLbl.Frame.Height + _runningTime.Frame.Height + _genreLbl.Frame.Height
                + _dvdLbl.Frame.Height + _theatresLbl.Frame.Height + 10;
            }
        }

        public MovieInfoView(Movie movie, MovieInfo mInfo)
        {
            _synopsisLbl = CreateLabel(new CGRect(5, 0, 315, 130));
            _synopsisLbl.AdjustsFontSizeToFitWidth = false;
            _synopsisLbl.Lines = 1000;

            UpdateText(_synopsisLbl, string.Format("Synopsis: {0}", mInfo.Synopsis), 10);
            _synopsisLbl.SizeToFit();

            _directorLbl = CreateLabel(new CGRect(5, _synopsisLbl.Frame.Bottom + 5, 315, 20));

            _ratedLbl = CreateLabel(new CGRect(5, _directorLbl.Frame.Bottom + 5, 255, 20));

            _runningTime = CreateLabel(new CGRect(5, _ratedLbl.Frame.Bottom + 5, 255, 20));

            _genreLbl = CreateLabel(new CGRect(5, _runningTime.Frame.Bottom + 5, 255, 20));

            _theatresLbl = CreateLabel(new CGRect(5, _genreLbl.Frame.Bottom + 5, 255, 20));

            _dvdLbl = CreateLabel(new CGRect(5, _theatresLbl.Frame.Bottom + 5, 255, 20));

            UpdateText(_directorLbl, string.Format("Director: {0}", mInfo.GetFormattedDirector()), 10);

            UpdateText(_ratedLbl, string.Format("Rated: {0}", movie.MpaaRating), 6);

            UpdateText(_runningTime, string.Format("Running Time: {0}", movie.GetFormattedRuntime()), 13);

            UpdateText(_genreLbl, string.Format("Genre: {0}", mInfo.GetFormattedGenres()), 6);

            UpdateText(_theatresLbl, string.Format("Theater Release: {0}", movie.ReleaseDates.GetFormattedTheaterDate()), 16);

            UpdateText(_dvdLbl, string.Format("DVD Release: {0}", movie.ReleaseDates.GetFormattedDvdDate()), 12);
        }

        private UILabel CreateLabel(CGRect frame)
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
