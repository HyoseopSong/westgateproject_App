﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.office
{
    public partial class unionParking : ContentPage
	{
		bool backTouched;
		protected override bool OnBackButtonPressed()
		{
			if (!backTouched)
			{
				backTouched = true;
				Navigation.PopAsync();
			}
			return true;
		}
        public unionParking()
        {
			InitializeComponent();
            backTouched = false;
			absL.AnchorX = 0;
			absL.AnchorY = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					absL.Scale = (App.ScreenHeight - 90) / 354;
					break;
				default:
					absL.Scale = (App.ScreenHeight - 70) / 354;
					break;
			}

			var boundaryBox = new BoxView { Color = Color.Red };
			AbsoluteLayout.SetLayoutBounds(boundaryBox, new Rectangle(599 * absL.Scale, App.ScreenWidth, 0, 30));
			absL.Children.Add(boundaryBox);
		}


		//async void goBack(object sender, EventArgs args)
		//{
		//	await Navigation.PopAsync();
		//}
    }
}
