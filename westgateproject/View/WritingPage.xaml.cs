﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using westgateproject.Helper;
using westgateproject.Models;
using Xamarin.Forms;

namespace westgateproject.View
{
    public partial class WritingPage : TabbedPage
    {
        private MediaFile photoStream;
        string _shopName;
        bool isInitial;
        bool backTouched;
        bool onProcessing;
        bool dataLoading;
        int myPageNumber;
        const int numOfMyPage = 10;
		Stream stream;

		string _shopLocation = "";
		List<UserInfoEntity> userInfo = new List<UserInfoEntity>();
		List<string> blobNameList = new List<string>();
        List<ContentsEntity> MyContentsSource = new List<ContentsEntity>();
        ObservableCollection<ContentsEntity> myContents = new ObservableCollection<ContentsEntity>();
        ObservableCollection<UserInfoEntity> shopListSource = new ObservableCollection<UserInfoEntity>();

        public WritingPage(){}

        public WritingPage(List<UserInfoEntity> userInfoParam)
        {
            InitializeComponent();
            dataLoading = false;
            backTouched = false;

            Title = App.userEmail;
            CameraButton.WidthRequest = App.ScreenWidth / 2;
            PictureButton.WidthRequest = App.ScreenWidth / 2;
            UploadTextEditor.BackgroundColor = Color.Lime;

            userInfo = userInfoParam;
			isInitial = true;

			searchToolbarItem.Text = "";
			searchToolbarItem.Clicked -= OnSearchItemClicked;
			//syncLabel();
			this.CurrentPageChanged += (object sender, EventArgs e) => {
				var i = this.Children.IndexOf(this.CurrentPage);
				if (i > 1)
				{
					searchToolbarItem.Text = "검색";
					searchToolbarItem.Clicked += OnSearchItemClicked;

				}
				else
				{
					searchToolbarItem.Text = "";
					searchToolbarItem.Clicked -= OnSearchItemClicked;
				}
			};
            //syncLabel();
        }
        protected override async void OnAppearing()
        {
            UploadTextEditor.HeightRequest = App.ScreenHeight * 0.2;
            if(!isInitial)
            {
                Debug.WriteLine("OnAppearing if");
                return;
            }
            else
            {
                Debug.WriteLine("OnAppearing else");
                isInitial = false;
                onProcessing = false;
            }


            shopListSource = new ObservableCollection<UserInfoEntity>(userInfo);
            MyShopListView.ItemsSource = shopListSource;

            foreach(var s in shopListSource)
            {
		        var rawBuildingInfo = s.RowKey.Split(':')[0];
                switch (rawBuildingInfo)
		        {
		            case "Dongsan":
                        s.RowKey = s.RowKey.Replace(rawBuildingInfo, "동산상가");
		                break;
					case "FifthBuilding":
						s.RowKey = s.RowKey.Replace(rawBuildingInfo, "5지구");
		                break;
					case "SecondBuilding":
						s.RowKey = s.RowKey.Replace(rawBuildingInfo, "2지구");
		                break;
		        }
                s.RowKey = s.RowKey.Replace(":", " ");
			}

            //foreach (var UserInfo in userInfo)
            //{
            //    if (UserInfo.Paid)
            //    {
            //        _shopName = UserInfo.ShopName;
            //        shopPicker.Items.Add(UserInfo.ShopName);

            //        var shopInfo = UserInfo.RowKey.Split(':');
            //        _shopLocation.Add(UserInfo.ShopName, shopInfo[0] + ":" + shopInfo[1] + ":" + shopInfo[2]);

            //        switch (shopInfo[0])
            //        {
            //            case "Dongsan":
            //                shopInfo[0] = "동산상가";
            //                break;
            //            case "FifthBuilding":
            //                shopInfo[0] = "5지구";
            //                break;
            //            case "SecondBuilding":
            //                shopInfo[0] = "2지구";
            //                break;
            //        }

            //        Label shopName = new Label()
            //        {
            //            Text = "매장 이름 : " + UserInfo.ShopName,
            //            VerticalTextAlignment = TextAlignment.Center
            //        };
            //        Label shopLocation = new Label()
            //        {
            //            Text = "위치 : " + shopInfo[0] + " " + shopInfo[1] + " " + shopInfo[2],
            //            VerticalTextAlignment = TextAlignment.Center
            //        };
            //        Label phoneNumber = new Label()
            //        {
            //            Text = "전화 번호 : " + UserInfo.PhoneNumber,
            //            VerticalTextAlignment = TextAlignment.Center
            //        };
            //        Label servicePeriod = new Label()
            //        {
            //            Text = "만료 날짜 : " + UserInfo.Period,
            //            VerticalTextAlignment = TextAlignment.Center
            //        };
            //        BoxView myBox = new BoxView()
            //        {
            //            HeightRequest = 10,
            //            BackgroundColor = Color.LightGray
            //        };

            //        switch (Device.RuntimePlatform)
            //        {
            //            case Device.iOS:
            //                shopName.HeightRequest = 30;
            //                shopLocation.HeightRequest = 30;
            //                phoneNumber.HeightRequest = 30;
            //                servicePeriod.HeightRequest = 30;
            //                break;
            //            case Device.Android:
            //                shopName.HeightRequest = 40;
            //                shopLocation.HeightRequest = 40;
            //                phoneNumber.HeightRequest = 40;
            //                servicePeriod.HeightRequest = 30;
            //                break;
            //        }

            //        MyInformation.Children.Add(shopName);
            //        MyInformation.Children.Add(shopLocation);
            //        MyInformation.Children.Add(phoneNumber);
            //        MyInformation.Children.Add(servicePeriod);
            //        MyInformation.Children.Add(myBox);
            //    }
            //    else
            //    {
            //        var rawShopInfo = UserInfo.RowKey.Split(':');
            //        switch (rawShopInfo[0])
            //        {
            //            case "Dongsan":
            //                rawShopInfo[0] = "동산상가";
            //                break;
            //            case "FifthBuilding":
            //                rawShopInfo[0] = "5지구";
            //                break;
            //            case "SecondBuilding":
            //                rawShopInfo[0] = "2지구";
            //                break;
            //        }
            //        Label shopInfo = new Label()
            //        {
            //            Text = rawShopInfo[0] + " " + rawShopInfo[1] + " " + rawShopInfo[2] + " " + UserInfo.ShopName + " 등록 대기 중",
            //            VerticalTextAlignment = TextAlignment.Center
            //        };

            //        BoxView myBox = new BoxView()
            //        {
            //            HeightRequest = 10,
            //            BackgroundColor = Color.LightGray
            //        };

            //        MyInformation.Children.Insert(0, shopInfo);
            //    }
            //}



            Dictionary<string, string> getParam = new Dictionary<string, string>
            {
                { "id", App.userEmail},
            };
            MyContentsSource = await App.Client.InvokeApiAsync<List<ContentsEntity>>("upload", System.Net.Http.HttpMethod.Get, getParam);

            MyContentsSource.Reverse();
			MyListView.ItemsSource = myContents;

            int i = 0;
            foreach (var t in MyContentsSource)
            {
                t.RowKey = "https://westgateproject.blob.core.windows.net/" + App.userEmail.Split('@')[0] + "/" + t.RowKey;
                switch (t.LikeMember)
                {
                    case "True":
                        t.LikeMember = "HeartFilled.png";
                        break;
                    case "False":
                        t.LikeMember = "HeartEmpty.png";
                        break;
                }

                if (i++ < numOfMyPage)
                {
                    myContents.Add(t);
                }
            }
            MyListView.ItemAppearing += (object sender, ItemVisibilityEventArgs e) =>
            {
                var item = e.Item as ContentsEntity;
                int index = myContents.IndexOf(item);
                if (myContents.Count - 2 <= index)
                {
                    if (!dataLoading)
                    {
                        dataLoading = true;

                        myPageNumber++;
                        for (i = myPageNumber * numOfMyPage; i < (myPageNumber + 1) * numOfMyPage && i < MyContentsSource.Count; i++)
                        {
                            myContents.Add(MyContentsSource[i]);
                        }

                        dataLoading = false;
                    }
                }
            };

        }

        async void UploadButton_Clicked(object sender, EventArgs e)
        {

            var senderButton = sender as Button;
            senderButton.IsEnabled = false;
            string result = "";

            if(PhotoImage.Source != null && UploadTextEditor.Text != null && uploadShopName.Text != "")
            {
                Debug.WriteLine(UploadTextEditor.Text + " " + _shopName + " " + _shopLocation);
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        result = await SyncData.UploadContents(photoStream, UploadTextEditor.Text, _shopName, _shopLocation);
                        break;
                    case Device.iOS:
                        result = await SyncData.UploadByteArrayContents(stream, UploadTextEditor.Text, _shopName, _shopLocation);
                        break;
                }
				result += ".jpg";
				ContentsEntity tempEntity = new ContentsEntity(App.userEmail, result, _shopName, UploadTextEditor.Text);

				tempEntity.RowKey = "https://westgateproject.blob.core.windows.net/" + App.userEmail.Split('@')[0] + "/" + tempEntity.RowKey;
                tempEntity.LikeMember = "HeartEmpty.png";
                MyContentsSource.Insert(0, tempEntity);

                myContents.Insert(0, tempEntity);

                PhotoImage.IsVisible = false;
                AlterText.IsVisible = true;
                PhotoImage.Source = null;
                UploadTextEditor.Text = null;
                MyShopListView.IsVisible = true;
            }
            else
            {
                await DisplayAlert("빈 칸 있음", "매장과 사진, 글 중 하나 이상이 비어있습니다.", "확인");
            }

            senderButton.IsEnabled = true;

        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            photoStream = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {

                PhotoSize = PhotoSize.Custom,
                CustomPhotoSize = 25,
                CompressionQuality = 50,
                RotateImage = true

            });


            if (photoStream != null)
			{
				PhotoImage.IsVisible = true;
                AlterText.IsVisible = false;
                PhotoImage.HeightRequest = App.ScreenWidth;

                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        PhotoImage.Source = ImageSource.FromStream(photoStream.GetStream);
                        break;
                    case Device.iOS:
                        byte[] resizedImageByteArray = DependencyService.Get<IImageResizeHelper>().ResizeImageIOS(photoStream.Path);
                        stream = new MemoryStream(resizedImageByteArray);
                        var thisStream = new MemoryStream(resizedImageByteArray);
                        PhotoImage.Source = ImageSource.FromStream(() => thisStream);

                        break;
                }

            }

        }


        private async void PicturePicker_Clicked(object sender, EventArgs e)
        {
            photoStream = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Custom,
                CustomPhotoSize = 25,
                CompressionQuality = 50
            });


            if (photoStream != null)
            {
				PhotoImage.HeightRequest = App.ScreenWidth;
				PhotoImage.IsVisible = true;
				AlterText.IsVisible = false;

                switch(Device.RuntimePlatform)
                {
                    case Device.Android:
                        PhotoImage.Source = ImageSource.FromStream(photoStream.GetStream);
                        break;
                    case Device.iOS:
                        byte[] resizedImageByteArray = DependencyService.Get<IImageResizeHelper>().ResizeImageIOS(photoStream.Path);
                        stream = new MemoryStream(resizedImageByteArray);
                        var thisStream = new MemoryStream(resizedImageByteArray);
                        PhotoImage.Source = ImageSource.FromStream(() => thisStream);

                        break;
                }

            }
        }

		void OnSearchItemClicked(object sender, EventArgs args)
		{
			if (mySearchWindow.IsVisible)
			{
				mySearchWindow.IsVisible = false;
			}
			else
			{
				mySearchWindow.IsVisible = true;
			}
		}

        async void OnMyContentSelection(object sender, SelectedItemChangedEventArgs e)
        {
            Debug.WriteLine("onProcessing : " + onProcessing);
            if (!onProcessing)
            {
                onProcessing = true;
				if (e.SelectedItem == null)
				{
					onProcessing = false;
					return;
				}

				var item = (ContentsEntity)e.SelectedItem;

				var answer = await DisplayAlert("게시물 삭제", "이 게시물을 삭제하시겠습니까?", "삭제", "무시");
				if (answer)
				{
					myContents.Remove(item);

					var splitedRowKey = item.RowKey.Split('/');
					var result = await SyncData.DeleteContents(splitedRowKey[splitedRowKey.Length - 1]);
					onProcessing = false;
				}
                else
                {
                    onProcessing = false;
                }
            }

			((ListView)sender).SelectedItem = null;
        }


        protected override bool OnBackButtonPressed()
        {
            if(!backTouched)
            {
                backTouched = true;
                Navigation.PopAsync();
            }
            return true;
        }
		void MySearch(object sender, TextChangedEventArgs e)
		{

			myComplete.IsVisible = true;
			myCancel.IsVisible = false;
			if (e.NewTextValue == "")
			{
				MyListView.ItemsSource = myContents;
			}
			else
			{
				ObservableCollection<ContentsEntity> mySearchResult = new ObservableCollection<ContentsEntity>();
				MyListView.ItemsSource = mySearchResult;

				foreach (var r in MyContentsSource)
				{
					if (r.Context.Contains(e.NewTextValue) || r.ShopName.Contains(e.NewTextValue))
					{
						mySearchResult.Add(r);
					}

				}
			}
		}

		void MyCancelClicked(object sender, EventArgs e)
		{
			mySearchEntry.Text = "";
			myCancel.IsVisible = false;
			myComplete.IsVisible = true;
		}

		void MyCompleteClicked(object sender, EventArgs e)
		{
			if (mySearchEntry.Text != "")
			{
				myCancel.IsVisible = true;
				myComplete.IsVisible = false;
			}
		}

		async void OnMyShopListSelection(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
			//Debug.WriteLine("item.PartitionKey : " + item.PartitionKey);
			//Debug.WriteLine("item.RowKey : " + item.RowKey);
            //Debug.WriteLine("item.Paid : " + item.Paid);
    //        if (item.Paid)
    //        {
    //            if(item.RowKey.Contains("동산상가"))
				//{
				//	item.RowKey = item.RowKey.Replace("동산상가", "Dongsan");
    //            }
    //            else if(item.RowKey.Contains("5지구"))
    //            {
    //                item.RowKey = item.RowKey.Replace("5지구", "FifthBuilding");
    //            }
    //            else if(item.RowKey.Contains("2지구"))
    //            {
    //                item.RowKey = item.RowKey.Replace("2지구", "SecondBuilding");
    //            }
				//item.RowKey = item.RowKey.Replace(" ", ":");
				//Debug.WriteLine("item.RowKey2 : " + item.RowKey);
				//var index = userInfo.IndexOf(item);
				//Debug.WriteLine("index : " + index);
				//uploadShopName.Text = userInfo[index].ShopName;
				//_shopLocation = userInfo[index].RowKey;
            //    this.CurrentPage = this.Children[1];
            //}
            //else
            //{
            //    await DisplayAlert("승인되지 않은 매장", "승인된 후 게시물을 등록 할 수 있습니다.", "확인");
            //}


			if (!onProcessing)
			{
				onProcessing = true;
				if (e.SelectedItem == null)
				{
					onProcessing = false;
					return;
				}

                var item = (UserInfoEntity)e.SelectedItem;
                if(item.Paid)
                {
					Debug.WriteLine("item.Paid is true");
					if (item.RowKey.Contains("동산상가"))
    				{
                        item.RowKey = item.RowKey.Replace("동산상가", "Dongsan");
                    }
                    else if(item.RowKey.Contains("5지구"))
                    {
                        item.RowKey = item.RowKey.Replace("5지구", "FifthBuilding");
                    }
                    else if(item.RowKey.Contains("2지구"))
                    {
                        item.RowKey = item.RowKey.Replace("2지구", "SecondBuilding");
                    }
                    item.RowKey = item.RowKey.Replace(" ", ":");
                    Debug.WriteLine("item.RowKey2 : " + item.RowKey);
                    var index = userInfo.IndexOf(item);
                    Debug.WriteLine("index : " + index);
                    uploadShopName.Text = "게시할 매장 : " + userInfo[index].ShopName;
                    _shopName = userInfo[index].ShopName;
                    _shopLocation = userInfo[index].RowKey;
                    this.CurrentPage = this.Children[1];
				}
                else
                {
                    await DisplayAlert("승인되지 않은 매장", "승인되면 내 소식을 게시할 수 있습니다.", "확인");
                }
				onProcessing = false;
			}
		}
    }
}
