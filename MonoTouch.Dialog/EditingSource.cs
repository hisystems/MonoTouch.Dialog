using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace MonoTouch.Dialog
{
	/// <summary>
	/// Override GetSizingSource in DialogViewController and return this instance.
	/// The objects are automatically deleted and updated as per the user interface.
	/// Only allows row editing when explicitly in editing mode (disallows inadvertent deletes via a swipe).
	/// </summary>
	public class EditingSource : DialogViewController.Source 
	{
		public EditingSource(DialogViewController viewController) 
			: base(viewController) 
		{
		}

		public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
		{
			// Only allow deletes when in editing mode.
			return tableView.Editing;
		}

		public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
		{
			// Always show a delete button.
			return UITableViewCellEditingStyle.Delete;
		}

		public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			var section = Container.Root[indexPath.Section];
			var element = section[indexPath.Row];
			section.Remove(element);
		}

		public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
		{
			// Always allow a row to be moved.
			return true;
		}

		public override void MoveRow(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
		{
			var section = Container.Root[sourceIndexPath.Section];
			var source = section[sourceIndexPath.Row];

			section.Remove(source);
			section.Insert(destinationIndexPath.Row, source);
		}
	}
}