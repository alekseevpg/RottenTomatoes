using System;
using UIKit;
using CoreGraphics;
using RottenApi;
using Foundation;
using RottenTomatoes.Helpers.Helpers;

namespace RottenTomatoes.TableCells
{
    public class ReviewTableCell : UITableViewCell
    {
        public static readonly NSString CellId = new NSString("ReviewTableCell");
        private UILabel _titleLbl, _reviewLbl;
        private UIImageView _freshView;
        private Review _review;

        public NSUrl ReviewUrl
        {
            get
            { 
                if (_review != null && !string.IsNullOrEmpty(_review.Links.Review))
                    return new NSUrl(_review.Links.Review);
                return null;
            }
        }

        public ReviewTableCell(IntPtr handle)
            : base(handle)
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            Accessory = UITableViewCellAccessory.DisclosureIndicator;
            _freshView = new UIImageView(new CGRect(5, 20, 15, 15));
            Add(_freshView);


            _titleLbl = new UILabel(new CGRect(25, 5, 270, 20))
            {
                Font = UIFont.FromName("HelveticaNeue-italic", 13),
                TextColor = UIColor.Black,
            };
            Add(_titleLbl);

            _reviewLbl = new UILabel(new CGRect(25, 25, 270, 20))
            {
                Font = UIFont.FromName("HelveticaNeue", 13),
                TextColor = UIColor.Black,
            };
            Add(_reviewLbl);
        }

        public void UpdateReview(Review review)
        {
            _review = review;
            switch (review.Freshness)
            {
                case "certified fresh":
                    _freshView.Image = Images.Get("Content/CF_300x300.png");
                    break;
                case "fresh":
                    _freshView.Image = Images.Get("Content/fresh.png");
                    break;
                case "rotten":
                    _freshView.Image = Images.Get("Content/rotten.png");
                    break;
                default:
                    _freshView.Image = null;
                    break;
            }
            _titleLbl.Text = string.Format("{0}, {1}", review.Critic, review.Publication);
            _reviewLbl.Text = review.Quote;
        }
    }
}

