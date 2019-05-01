
using System;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Support.V7.App;

namespace FamilyLocator
//https://forums.xamarin.com/discussion/123468/google-sign-in-not-working-in-android
{
    [Activity(Label = "Sign In")]

    public class SignInActivity : AppCompatActivity, GoogleApiClient.IOnConnectionFailedListener
    {
        //GoogleSignInClient mGoogleSignInClient;
        private GoogleApiClient mGoogleApiClient;
        private static int RC_SIGN_IN = 9001;
        private GoogleProfile googleProfile;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.signInlayout);
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken("417371590404-chu45uoru1r4vg1u3qro73aanbt5k2eq.apps.googleusercontent.com")
                .RequestEmail()
                .Build();

            mGoogleApiClient = new GoogleApiClient.Builder(this)
                .EnableAutoManage(this, this)
                .AddOnConnectionFailedListener(this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                .Build();
            SignInButton sib = FindViewById<SignInButton>(Resource.Id.sign_in_button);
            sib.Click += SibCLick;
        }

        private void SibCLick(object sender, EventArgs e)
        {
            SignIn();
        }

        private void SignIn()
        {
            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
            StartActivityForResult(signInIntent, RC_SIGN_IN);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == RC_SIGN_IN)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                var profile = HandleSignInResult(result);
            }
        }

        private GoogleProfile HandleSignInResult(GoogleSignInResult result)
        {
            if (result.IsSuccess)
            {
                // Signed in successfully, show authenticated UI.
                GoogleSignInAccount acct = result.SignInAccount;
                googleProfile = new GoogleProfile()
                {
                    GivenName = acct.GivenName,
                    FamilyName = acct.FamilyName,
                    Email = acct.Email,
                    Id = acct.Id
                };
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("The resul was not success");
            }

            return googleProfile;
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            throw new System.NotImplementedException();
        }
    }

    public class GoogleProfile
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
    }
}