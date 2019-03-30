using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Plugin.Geolocator;
using ActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using AlertDialog = Android.App.AlertDialog;

namespace FamilyLocator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener, IOnMapReadyCallback
    {
        private GoogleMap mMap;

        public readonly string[] PermissionsLocation =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        const int RequestLocationId = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            permissionCheck();
            SetupMap();
        }

        private void SetupMap()
        {
            if (mMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }

        public async void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;

            var location = await getUserLatLng();
            updateCamera(location);
            addMarker(mMap, location);
        }

        private void updateCamera(LatLng location)
        {
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(18);
            builder.Bearing(155);
            builder.Tilt(65);

            CameraPosition cameraPosition = builder.Build();

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            mMap.MoveCamera(cameraUpdate);

            mMap.MapType = GoogleMap.MapTypeHybrid;
            mMap.UiSettings.ZoomControlsEnabled = true;
            mMap.UiSettings.CompassEnabled = true;
            // Do something with the map, i.e. add markers, move to a specific location, etc.
        }

        private async Task<LatLng> getUserLatLng()
        {
            var locator = CrossGeolocator.Current;
            if (locator.IsGeolocationEnabled)
            {
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(timeout: TimeSpan.FromSeconds(10));
                return new LatLng(position.Latitude, position.Longitude);
            }
            else
            {
                NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
                Snackbar.Make(navigationView, "Geo Location is not enabled", Snackbar.LengthLong)
                    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();

                return new LatLng(11.016844, 76.955833);
            }
        }

        private void addMarker(GoogleMap map, LatLng location)
        {
            MarkerOptions markerOpt1 = new MarkerOptions();
            markerOpt1.SetPosition(location);
            markerOpt1.SetTitle("My Location!");

            var bmDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan);
            markerOpt1.InvokeIcon(bmDescriptor);

            map.AddMarker(markerOpt1);
        }

        private void permissionCheck()
        {
            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
            {
                // Provide an additional rationale to the user if the permission was not granted
                // and the user would benefit from additional context for the use of the permission.
                // For example if the user has previously denied the permission.
                Log.Info("", "Displaying camera permission rationale to provide additional context.");

                var requiredPermissions = new String[] { Manifest.Permission.AccessFineLocation };

                Snackbar.Make((View)Resource.Layout.activity_main, "Location access is required to show coffee shops nearby.", Snackbar.LengthIndefinite)
                    .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
                    .Show();

                return;
            }
        }

        public override void OnBackPressed()
        {
            var drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            } else if (id == Resource.Id.action_changeView)
            {
                //methodInvokeBaseAlertDialog();
                var alertDialog = new AlertDialog.Builder(this);
                alertDialog.SetTitle("Select the View");
                var radioView = FindViewById<DrawerLayout>(Resource.Id.radio_group);
                alertDialog.SetView(radioView);
                alertDialog.Show();
            }

            return base.OnOptionsItemSelected(item);
        }

        //private void methodInvokeBaseAlertDialog()
        //{
        //    Dialog dialog = new Dialog(this);
        //    dialog.SetContentView(Resource.Layout.content_main);
        //    dialog.SetTitle("Dialog with Radio Button");
        //    dialog.SetCancelable(true);

        //    RadioButton rd1 = (RadioButton)dialog.FindViewById(Resource.Id.rd1);
        //    RadioButton rd2 = (RadioButton)dialog.FindViewById(Resource.Id.rd2);

        //    dialog.Show();
        //}

        //void handllerNotingButton(object sender, DialogClickEventArgs e)
        //{
        //    AlertDialog objAlertDialog = sender as AlertDialog;
        //    Button btnClicked = objAlertDialog.GetButton(e.Which);
        //    Toast.MakeText(this, "you clicked on " + btnClicked.Text, ToastLength.Long).Show();
        //}

        private async void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            var location = await getUserLatLng();
            updateCamera(location);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                // Handle the camera action
            }
            else if (id == Resource.Id.nav_gallery)
            {

            }
            else if (id == Resource.Id.nav_slideshow)
            {

            }
            else if (id == Resource.Id.nav_manage)
            {

            }
            else if (id == Resource.Id.nav_share)
            {

            }
            else if (id == Resource.Id.nav_send)
            {

            }

            DrawerLayout drawer = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
    }
}

