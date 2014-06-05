using System;
using MonoTouch.UIKit;
using System.Drawing;
using RottenApi;
using MonoTouch.Foundation;

namespace RottenTomatoes.TableCells
{
    public class ActorTableCell : UITableViewCell
    {
        public static readonly NSString CellId = new NSString("ActorTableCell");

        private UILabel _titleLbl, _charactersLbl;

        public ActorTableCell(IntPtr handle)
            : base(handle)
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            _titleLbl = new UILabel(new RectangleF(5, 5, 270, 20))
            {
                Font = UIFont.FromName("HelveticaNeue-bold", 13),
                TextColor = UIColor.Black,
            };
            Add(_titleLbl);

            _charactersLbl = new UILabel(new RectangleF(5, 25, 270, 20))
            {
                Font = UIFont.FromName("HelveticaNeue", 13),
                TextColor = UIColor.Black,
            };
            Add(_charactersLbl);
        }

        public void UpdateActor(Actor actor)
        {
            _titleLbl.Text = actor.Name;
            _charactersLbl.Text = actor.GetFormattedChars();
        }
    }
}

