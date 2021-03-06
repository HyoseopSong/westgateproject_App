﻿using System;
using Android.App;
using Android.Content;
using westgateproject.Droid;
using westgateproject.Helper;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(LoginHelper))]
namespace westgateproject.Droid
{
    public class LoginHelper : ILoginHelper
    {
        public LoginHelper()
        {

        }


        public void StartLogin()
        {
            var intent = new Intent(Forms.Context, typeof(SignInActivity));
            intent.PutExtra("action", "login");
			//Forms.Context.StartActivity(intent);
			Activity activity = Forms.Context as Activity;
			activity.StartActivityForResult(intent, 1);
        }

		public void StartLogout()
		{
			var intent = new Intent(Forms.Context, typeof(SignInActivity));
			intent.PutExtra("action", "logout");
			//Forms.Context.StartActivity(intent);
			Activity activity = Forms.Context as Activity;
			activity.StartActivityForResult(intent, 1);
		}
    }
}
