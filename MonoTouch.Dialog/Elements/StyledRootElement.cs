﻿using System;
using MonoTouch.UIKit;

namespace MonoTouch.Dialog
{
	public class StyledRootElement : RootElement
	{
		public UIImage Image { get; set; }

		public StyledRootElement(string caption)
			: base(caption)
		{
		}

		public StyledRootElement(string caption, Func<RootElement, UIViewController> createOnSelected)
			: base(caption, createOnSelected)
		{
		}

		public StyledRootElement(string caption, Func<RootElement, UIViewController> createOnSelected, UIImage image)
			: base(caption, createOnSelected)
		{
			this.Image = image;
		}

		public override MonoTouch.UIKit.UITableViewCell GetCell(MonoTouch.UIKit.UITableView tv)
		{
			var cell = base.GetCell(tv);

			cell.ImageView.Image = this.Image;

			return cell;
		}
	}
}