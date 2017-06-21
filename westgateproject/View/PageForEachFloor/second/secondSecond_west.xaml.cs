﻿using System;
using System.Collections.Generic;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View.PageForEachFloor.second
{
    public partial class secondSecond_west : ContentPage
    {
        public secondSecond_west()
        {
			InitializeComponent();
			zoomContainer.Content.AnchorX = 0;
			zoomContainer.Content.AnchorY = 0;
			zoomContainer.min_ty = 0;
			switch (Device.RuntimePlatform)
			{
				case Device.Android:
					zoomContainer.Content.Scale = (App.ScreenHeight - 90) / 295;
					break;
				default:
					zoomContainer.Content.Scale = (App.ScreenHeight - 70) / 295;
					break;
			}
			zoomContainer.min_tx = -((570 * zoomContainer.Content.Scale) - App.ScreenWidth);
			zoomContainer.maxScale = zoomContainer.Content.Scale;
			zoomContainer.minScale = zoomContainer.Content.Scale;
			NavigationPage.SetHasBackButton(this, false);
		}
		async void OnTapped(object sender, EventArgs args)
		{
			ShopInformation infoFromSQLite = new ShopInformation();
			var temp = sender as Label;
			infoFromSQLite = await App.Database.GetShopAsync("2지구", "지하1층", temp.Text);
			await Navigation.PushAsync(new ShopInfoPage(infoFromSQLite));
		}
		async void OnTappedEast(object sender, EventArgs args)
		{
			await Navigation.PushAsync(new secondSecond_east());
			Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

		}
		async void goBack(object sender, EventArgs args)
		{
			await Navigation.PopAsync();
		}
    }
}
