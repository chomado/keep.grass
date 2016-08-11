﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace keep.grass
{
	public class ResponsiveLayout : StackLayout
	{
		public double MaxColumnWidth;
		public double MinColumnWidth;

		public List<Layout> BlockList = new List<Layout>();
		public List<StackLayout> ColumnStackList = new List<StackLayout>();

		public ResponsiveLayout()
		{
			Orientation = StackOrientation.Horizontal;
		}

		public void Response()
		{
			var MaxColumnSize =
				MaxColumnWidth <= Width ?
					1:
					Math.Min((int)(Width /MinColumnWidth), BlockList.Count);
			
			var ColumnSize = 0;
			do
			{
				++ColumnSize;
				ColumnStackList.Clear();
				for (var i = 0; i < ColumnSize; ++i)
				{
					var CurrentStack = new StackLayout();
					foreach (var Block in BlockList.Where((v, index) => i == index % ColumnSize))
					{
						CurrentStack.Children.Add(Block);
					}
					ColumnStackList.Add(CurrentStack);
				}
			}
			while
			(
				Height < ColumnStackList.Select(i => i.Height).Sum() &&
				ColumnSize < MaxColumnSize
			);
		}
	}
}
