using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FamilyLocator
{
    public class Constants
    {
        public const string AppName = "Family Locator";
        public const string UserInfoUrl = "";
        public const string Scope = "profile";
        public const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public const string AccessTokenUrl = "https://oauth2.googleapis.com/token";
    }
}