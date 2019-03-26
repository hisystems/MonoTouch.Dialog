using System;
using UIKit;

namespace MonoTouch.Dialog
{
	/// <summary>
	/// Displays caption and text(summary).
	/// </summary>
	public class SummaryElement : RootElement
	{
		private string value = string.Empty;

		public SummaryElement (
			string caption, 
			string value, 
			Func<RootElement, UIViewController> createOnSelected 
		)
			: base(caption, createOnSelected)
		{
			this.value = value;
		}

		public override string Summary ()
		{
			return value;
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

		public override UITableViewCell GetCell(UITableView tableView)
		{
			const string CellID = "SummaryElement";

			var cell = tableView.DequeueReusableCell(CellID);

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Value1, CellID);
				cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
			}

			cell.DetailTextLabel.Text = Value;
			cell.TextLabel.Text = this.Caption;
			cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;

			SetCellEnabledState (cell);

			return cell;
		}
	}
}