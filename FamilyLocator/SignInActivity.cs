
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Android.Preferences;
using Plugin.GoogleClient;

namespace FamilyLocator
{
    [Activity(Label = "SignInActivity")]
    public class SignInActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.signInlayout);

            // Create your application here
        }
    }
}