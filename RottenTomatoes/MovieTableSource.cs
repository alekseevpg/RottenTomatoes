using System;
using MonoTouch.UIKit;
using RottenApi;
using System.Drawing;
using MonoTouch.Foundation;
using CoinKeeper.Logic.IoCContainer;
using RottenTomatoes.TableCells;

namespace RottenTomatoes
{

    public class MovieTableSource : UITableViewSource
    {
        private Movie _movie;
        private MovieInfoView _mInfoView;
        private MovieCast _cast;
        private ReviewList _reviews;

        public bool IsSourceLoaded
        {
            get
            {
                if (_movie != null && _mInfoView != null && _cast != null && _reviews != null)
                    return true;
                return false;
            }
        }

        public MovieTableSource(Movie movie)
        {
            _movie = movie;
        }

        public void UpdateMovieInfo(MovieInfo mInfo)
        {
            _mInfoView = new MovieInfoView(_movie, mInfo);
            _mInfoView.SizeToFit();
        }

        public void UpdateMovieCast(MovieCast cast)
        {
            _cast = cast;
        }

        public void UpdateMovieReviews(ReviewList reviews)
        {
            _reviews = reviews;
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return 4;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            switch (section)
            {
                case 0:
                    return 1;
                case 1:
                    return 1;
                case 2:
                    return _cast.Cast.Count;
                case 3:
                    return _reviews.Reviews.Count;
                default:
                    return 5;
            }
        }

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    return 90.5f;
                case 1:
                    return _mInfoView.Height;
                default:
                    return 50;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    MovieTableCell movieCell = (MovieTableCell)tableView.DequeueReusableCell(MovieTableCell.CellId);
                    movieCell.UpdateCell(_movie);
                    return movieCell;
                case 1:
                    MovieInfoTableCell infoCell = (MovieInfoTableCell)tableView.DequeueReusableCell(MovieInfoTableCell.CellId);
                    infoCell.UpdateCell(_mInfoView);
                    return infoCell;
                case 2:
                    ActorTableCell actorCell = (ActorTableCell)tableView.DequeueReusableCell(ActorTableCell.CellId);
                    actorCell.UpdateActor(_cast.Cast[indexPath.Row]);
                    return actorCell;

                case 3:
                    ReviewTableCell reviewCell = (ReviewTableCell)tableView.DequeueReusableCell(ReviewTableCell.CellId);
                    reviewCell.UpdateReview(_reviews.Reviews[indexPath.Row]);
                    return reviewCell;
                default:
                    return new UITableViewCell();
            }
        }

        public override float GetHeightForHeader(UITableView tableView, int section)
        {
            return section == 0 ? 0.0001f : 40;
        }

        public override UIView GetViewForHeader(UITableView tableView, int section)
        {
            if (section == 0)
                return new UIView();
            var header = new UIView(new RectangleF(0, 0, 320, 40));
            header.BackgroundColor = UIColor.Black;

            var headerLabel = new UILabel(new RectangleF(10, 0, 320, 40))
            {
                Font = UIFont.FromName("HelveticaNeue-Bold", 15),
                TextColor = UIColor.White
            };

            switch (section)
            {
                case 1: 
                    headerLabel.Text = "Movie info";
                    break;
                case 2: 
                    headerLabel.Text = "Cast";
                    break;
                case 3: 
                    headerLabel.Text = "Critic reviews";
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
            switch (indexPath.Section)
            {
                case 3:
                    UIApplication.SharedApplication.OpenUrl(((ReviewTableCell)tableView.CellAt(indexPath)).ReviewUrl);
                    break;
            }
        }
    }
}
