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
        private UILabel _titleLbl;

        public ActorTableCell(IntPtr handle)
            : base(handle)
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            _titleLbl = new UILabel(new RectangleF(5, 5, 310, 20))
            {
                Font = UIFont.FromName("HelveticaNeue", 13),
                TextColor = UIColor.Black,
            };
            Add(_titleLbl);
        }

        public void UpdateActor(Actor actor)
        {
            var boldAttr = new UIStringAttributes
            {
                Font = UIFont.FromName("HelveticaNeue-Bold", 13)
            };
            var attrStr = new NSMutableAttributedString(string.Format("{0} as {1}", actor.Name, actor.GetFormattedChars()));
            attrStr.SetAttributes(boldAttr, new NSRange(0, actor.Name.Length + 3));
            _titleLbl.AttributedText = attrStr;
        }
    }
}

