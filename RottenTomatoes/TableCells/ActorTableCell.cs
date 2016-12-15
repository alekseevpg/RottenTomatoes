using System;
using UIKit;
using RottenApi;
using Foundation;
using CoreGraphics;

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
            _titleLbl = new UILabel(new CGRect(5, 5, 270, 20))
            {
                Font = UIFont.FromName("HelveticaNeue-bold", 13),
                TextColor = UIColor.Black,
            };
            Add(_titleLbl);

            _charactersLbl = new UILabel(new CGRect(5, 25, 270, 20))
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