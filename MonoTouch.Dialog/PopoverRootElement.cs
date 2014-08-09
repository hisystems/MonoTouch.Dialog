using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace MonoTouch.Dialog
{
	/// <summary>
	/// Displays the root element as a popover (iPad only) rather than as a sub-table element.
	/// </summary>
	public class PopoverRootElement : RootElement
	{
		/// <summary>
		/// Sets the detail text for the cell.
		/// </summary>
		/// <remarks>>
		/// Typically in MonoTouch Dialog this is sourced from a summary section.
		/// But because a dialog is being displayed this approach does not work.
		/// </remarks>
		private string value = string.Empty;

		private UIPopoverController popoverViewController;
		private readonly Func<UIPopoverArrowDirection> getArrowDirection;

		public PopoverRootElement(
			string caption, 
			string value, 
			Func<RootElement, UIViewController> createOnSelected, 
			UIPopoverArrowDirection arrowDirection = UIPopoverArrowDirection.Any
		)
			: this(caption, value, createOnSelected, () => arrowDirection)
		{
		}

		public PopoverRootElement(
			string caption, 
			string value, 
			Func<RootElement, UIViewController> createOnSelected, 
			Func<UIPopoverArrowDirection> getArrowDirection
		)
			: base(caption, createOnSelected)
		{
			this.value = value;
			this.getArrowDirection = getArrowDirection;
		}

		public string Value
		{
			get
			{
				return this.value;
			}

			set
			{
				this.value = value;

				var root = base.GetImmediateRootElement();

				if (root != null)
					root.Reload(this, UITableViewRowAnimation.None);
			}
		}

		/// <summary>
		/// Dismiss the popover.
		/// </summary>
		public void DismissPopover(bool animated)
		{
			this.popoverViewController.Dismiss(animated);
		}

		public override UITableViewCell GetCell(UITableView tableView)
		{
			const string CellID = "PopoverElement";

			var cell = tableView.DequeueReusableCell(CellID);

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Value1, CellID);
				cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
			}

			cell.DetailTextLabel.Text = Value;
			cell.TextLabel.Text = this.Caption;
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

			return cell;
		}

		public override void Selected(DialogViewController dvc, MonoTouch.UIKit.UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
			tableView.DeselectRow(path, animated: true);

			var newViewController = this.MakeViewController();
			this.PrepareDialogViewController(newViewController);

			this.popoverViewController = new UIPopoverController(newViewController);

			var selectedCell = tableView.CellAt(path);

			popoverViewController.PresentFromRect(selectedCell.Frame, dvc.View, this.getArrowDirection(), animated: true);
		}
	}
}