using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MonoTouch.Dialog
{
	/// <summary>
	/// MUST be used with a DialogViewController.SizingSource() so that the height of the element can be calculated.
	/// </summary>
	public class ProgressViewElement : UIViewElement
	{
		static NSString key = new NSString ("ProgressView");
		
		public ProgressViewElement ()
			: base ("", CreateView(), transparent: false, insets: UIEdgeInsets.Zero)
		{
		}

		public float Progress 
		{
			set 
			{
				((UIProgressView)View).Progress = value;
			}
		}

		private static UIView CreateView()
		{
			var view = new UIProgressView();

			view.Frame = new CGRect(0, 0, 100, 2);
			view.Progress = 0;

			return view;
		}

		protected override NSString CellKey 
		{
			get 
			{
				return key;
			}
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			ContainerView.Subviews[0].Frame = new CGRect (0, 0, tv.Frame.Width, 2);

			return base.GetCell(tv);
		}
	}
}
