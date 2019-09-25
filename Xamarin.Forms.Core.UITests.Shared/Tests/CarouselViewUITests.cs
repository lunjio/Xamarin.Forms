﻿using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;

namespace Xamarin.Forms.Core.UITests
{
	[Category(UITestCategories.CarouselView)]
	internal class CarouselViewUITests : BaseTestFixture
	{
		string _enableCollectionView = "Enable CollectionView";
		string _carouselViewGalleries = "CarouselView Galleries";

		protected override void NavigateToGallery()
		{
			App.NavigateToGallery(GalleryQueries.CollectionViewGallery);

			App.WaitForElement(_enableCollectionView);
			App.Tap(_enableCollectionView);
			App.Tap(_carouselViewGalleries);
		}

		[TestCase("CarouselView (Code, Horizontal)")]
		//[TestCase("CarouselView (XAML, Horizontal)")]
		public void CarouselViewHorizontal(string subgallery)
		{
			VisitSubGallery(subgallery);


			App.WaitForElement("pos:1", "Did start on the correct position");
			var rect = App.Query(c => c.Marked("TheCarouselView")).First().Rect;
			var centerX = rect.CenterX;
			var rightX = rect.X + rect.Width - 1;
			App.DragCoordinates(centerX, rect.CenterY, rightX, rect.CenterY);
			App.WaitForElement("pos:0", "Did not scroll to first position");
			App.DragCoordinates(centerX, rect.CenterY, rect.X + 5, rect.CenterY);
			App.WaitForElement("pos:1", "Did not scroll to second position");

			App.Tap("Item: 1");

			App.WaitForElement("Button works");

			App.Tap(c => c.Marked("Ok"));

			App.Tap("SwipeSwitch");

			// iOS will show the Master page when we try drag
#if __ANDROID__
			App.DragCoordinates(centerX, rect.CenterY, rightX, rect.CenterY);

			App.WaitForNoElement("pos:0", "Swiped while swipe is disabled");
#endif
			App.Back();
		}

		[TestCase("CarouselView (Code, Vertical)")]
		public void CarouselViewVertical(string subgallery)
		{
			VisitSubGallery(subgallery);
			var rect = App.Query(c => c.Marked("TheCarouselView")).First().Rect;

			var centerX = rect.CenterX;
			var centerY = rect.CenterY;
			var bottomY = rect.Y + rect.Height - 1;

			App.DragCoordinates(rect.CenterX, rect.CenterY, rect.CenterX, bottomY);

			App.WaitForElement("pos:0", "Did not scroll to first position");
			var topY = rect.Y - 1;
#if __ANDROID__
			topY = rect.Y - 10;
#endif
			App.DragCoordinates(rect.CenterX, rect.CenterY, rect.CenterX, topY);

			App.WaitForElement("pos:1", "Did not scroll to second position");

			App.Tap("Item: 1");

			App.WaitForElement("Button works");

			App.Tap(c => c.Marked("Ok"));

			App.Tap("SwipeSwitch");

#if __ANDROID__
			App.DragCoordinates(rect.CenterX, rect.CenterY, rect.CenterY, bottomY);

			App.WaitForNoElement("pos:0", "Swiped while swipe is disabled");
#endif
		}

		void VisitSubGallery(string galleryName)
		{
			App.WaitForElement(t => t.Marked(galleryName));
			App.Tap(t => t.Marked(galleryName));
		}
	}
}