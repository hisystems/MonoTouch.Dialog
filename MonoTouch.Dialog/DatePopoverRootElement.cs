using System;
using MonoTouch.Dialog;
using UIKit;
using Foundation;

namespace MonoTouch.Dialog
{
	/// <summary>
	/// Displays a date picker within a popover for a root element.
	/// </summary>
	public class DatePopoverRootElement : PopoverRootElement
	{
		public event Action<DateTime> DateSelected;

		private DateTime initialDate;
		private UIDatePicker datePicker;
		private DateTime? dateSelected;

		public DatePopoverRootElement(string caption, DateTime initialDate)
			: base(caption, initialDate.ToLongDateString(), createOnSelected: null)
		{
			this.initialDate = initialDate;
		}

		public DatePopoverRootElement(string caption, DateTime initialDate, Action<DateTime> dateSelected = null)
			: this(caption, initialDate)
		{
			if (dateSelected != null)
				this.DateSelected += dateSelected;
		}

		public DateTime DateValue
		{
			get
			{
				return this.dateSelected.HasValue ? this.dateSelected.Value : this.initialDate;
			}
		}

		protected override UIViewController MakeViewController()
		{
			this.datePicker = new UIDatePicker();
			this.datePicker.Mode = UIDatePickerMode.Date;
			this.datePicker.Date = (NSDate)DateTime.SpecifyKind(initialDate, DateTimeKind.Local);

			var pickerViewController = new UIViewController();
			pickerViewController.Title = "Date";
			pickerViewController.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, e) => Cancel());
			pickerViewController.NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, e) => Done());
			pickerViewController.View = this.datePicker;

			var navigationController = new UINavigationController();
			navigationController.PreferredContentSize = this.datePicker.IntrinsicContentSize;
			navigationController.PushViewController(pickerViewController, animated: false);

			return navigationController;
		}

		private void Done()
		{
			var dateValue = ((DateTime)this.datePicker.Date).ToLocalTime();

			// Reset so that future selections will default to the new value.
			this.initialDate = dateValue;

			this.Value = dateValue.ToLongDateString();
			this.DismissPopover(animated: true);

			this.dateSelected = dateValue;
			OnDateSelected(dateValue);
		}

		private void OnDateSelected(DateTime date)
		{
			if (DateSelected != null)
				DateSelected(date);
		}

		private void Cancel()
		{
			this.DismissPopover(animated: true);
		}
	}
}