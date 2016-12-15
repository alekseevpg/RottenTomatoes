using System;
using UIKit;
using CoreGraphics;
using Foundation;
using RottenApi;
using System.Collections.Generic;
using RottenTomatoes.TableCells;
using RottenTomatoes.Enums;
using RottenTomatoes.Helpers.IoCContainer;

namespace RottenTomatoes
{
    public class MoviesTableSource : UITableViewSource
    {
        public event Action ReloadSectionNeeded = delegate {};
        public event Action<Movie> MovieSelected = delegate {};

        private SortedDictionary <MoviesType, MovieList> _movies = new SortedDictionary<MoviesType, MovieList>();
        private UIAlertView _alert;

        public void UpdateMovies(Action callback = null)
        {
            Container.Resolve<IServerApi>().GetOpeningThisWeek(movies =>
            {
                if (callback != null)
                    callback();
                if (movies == null)
                {
                    ShowAlert();
                    return;
                }
                UpdateMovieSection(MoviesType.Opening, movies);
            });

            Container.Resolve<IServerApi>().GetBoxOfficeMovies(movies =>
            {
                if (callback != null)
                    callback();
                if (movies == null)
                {
                    ShowAlert();
                    return;
                }
                UpdateMovieSection(MoviesType.BoxOffice, movies);
            });

            Container.Resolve<IServerApi>().GetAlsoInTheaters(movies =>
            {
                if (callback != null)
                    callback();
                if (movies == null)
                {
                    ShowAlert();
                    return;
                }
                UpdateMovieSection(MoviesType.InTheaters, movies);
            });
        }

        private void ShowAlert()
        {
            InvokeOnMainThread(() =>
            {
                if (_alert != null)
                    return;
                _alert = new UIAlertView("Error", "No data recieved. Pull to refresh.", null, "OK", null);
                _alert.Dismissed += (sender, e) =>
                {
                    _alert.Dispose();
                    _alert = null;
                };
                _alert.Show();
            });
        }

        private void UpdateMovieSection(MoviesType type, MovieList movies)
        {
            if (_movies.ContainsKey(type))
                _movies[type].Movies = movies.Movies;
            else
                _movies.Add(type, movies);
            ReloadSectionNeeded();
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
            MovieTableCell cell = (MovieTableCell)tableView.DequeueReusableCell(MovieTableCell.CellId, indexPath);

            cell.UpdateCell(_movies[(MoviesType)indexPath.Section].Movies[indexPath.Row]);
            cell.AccessibilityLabel = string.Format("MovieCell-{0}-{1}", indexPath.Section, indexPath.Row);
            return cell;
        }

        public override float GetHeightForHeader(UITableView tableView, int section)
        {
            return 40;
        }

        public override UIView GetViewForHeader(UITableView tableView, int section)
        {
            var header = new UIView(new CGRect(0, 0, 320, 40));
            header.BackgroundColor = UIColor.Black;

            var headerLabel = new UILabel(new CGRect(10, 0, 320, 40))
            {
                Font = UIFont.FromName("HelveticaNeue-Bold", 15),
                TextColor = UIColor.White
            };

            switch (section)
            {
                case 0: 
                    headerLabel.Text = headerLabel.AccessibilityLabel = "Opening This Week";
                    break;
                case 1: 
                    headerLabel.Text = headerLabel.AccessibilityLabel = "Top Box Office";
                    break;
                case 2: 
                    headerLabel.Text = headerLabel.AccessibilityLabel = "Also in Theaters";
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
}