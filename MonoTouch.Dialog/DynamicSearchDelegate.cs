using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace MonoTouch.Dialog
{
	/// <summary>
	/// Used for dynamic searching as opposed to the default implementation of only
	/// searching the already existing objects in the table view.
	/// Required when there is too much data to load into memory so this allows dynamically
	/// adding search results that are not already in the table view.
	/// Typically, inheritor overrides the TextChanged delegate callback and hooks this 
	/// constructor via ViewDidLoad.
	/// </summary>
	public abstract class DynamicSearchDelegate : UISearchBarDelegate
	{
		private readonly DialogViewController container;

		/// <summary>
		/// This will also override the existing search delegate on the controller.
		/// </summary>
		public DynamicSearchDelegate(DialogViewController container)
		{
			this.container = container;

			if (this.container.TableView.TableHeaderView == null)
				throw new InvalidOperationException(
					"Override and call after DialogViewController.ViewDidLoad() and ensure DialogViewController.EnableSearch = true");

			((UISearchBar)this.container.TableView.TableHeaderView).Delegate = this;
		}

		public override void OnEditingStarted(UISearchBar searchBar)
		{
			searchBar.ShowsCancelButton = true;
			this.container.StartSearch();
		}

		public override void OnEditingStopped(UISearchBar searchBar)
		{
			searchBar.ShowsCancelButton = false;
			this.container.FinishSearch();
		}

		/// <summary>
		/// Updates the view controller with the new search results.
		/// Ensure that the call is marshalled to UI thread before calling!
		/// </summary>
		public void SetResults(Section[] searchResults)
		{
			// This will be reset on FinishSearch() back to the original root stored
			// via StartSearch().
			// CANNOT re-assign the Root OR Clear the root because it will clear the section elements from the 
			// current root by explicitly calling Dispose().
			while (this.container.Root.Count > 0)
				this.container.Root.RemoveAt(0);

			this.container.Root.Add(searchResults);
			this.container.ReloadData();
		}

		public override void CancelButtonClicked(UISearchBar searchBar)
		{
			searchBar.ShowsCancelButton = false;
			searchBar.Text = string.Empty;
			this.container.FinishSearch();
		}

		public override void SearchButtonClicked(UISearchBar searchBar)
		{
			this.container.SearchButtonClicked(searchBar.Text);
		}
	}
}