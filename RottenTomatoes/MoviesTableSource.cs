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
    public class MoviesTableSource : UITableViewSource
    {
        public event Action ReloadSectionNeeded = delegate {};
        public event Action<Movie> MovieSelected = delegate {};

        private SortedDictionary <MoviesType, MovieList> _movies = new SortedDictionary<MoviesType, MovieList>();

        public MoviesTableSource()
        {
        }

        public void UpdateMovies(Action callback = null)
        {
            Container.Resolve<IServerApi>().GetOpeningThisWeek(movies =>
            {
                UpdateMovieSection(MoviesType.Opening, movies);
                if (callback != null)
                    callback();
            });

            Container.Resolve<IServerApi>().GetBoxOfficeMovies(movies =>
            {
                UpdateMovieSection(MoviesType.BoxOffice, movies);
                if (callback != null)
                    callback();
            });

            Container.Resolve<IServerApi>().GetAlsoInTheaters(movies =>
            {
                UpdateMovieSection(MoviesType.InTheaters, movies);
                if (callback != null)
                    callback();
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
            return cell;
        }

        public override float GetHeightForHeader(UITableView tableView, int section)
        {
            return 40;
        }

        public override UIView GetViewForHeader(UITableView tableView, int section)
        {
            var header = new UIView(new RectangleF(0, 0, 320, 40));
            header.BackgroundColor = UIColor.Black;

            var headerLabel = new UILabel(new RectangleF(10, 0, 320, 40))
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
}

