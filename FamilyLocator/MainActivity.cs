using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.App.Job;
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
using FamilyLocator.Service;
using Plugin.Geolocator;
using ActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using AlertDialog = Android.App.AlertDialog;

namespace FamilyLocator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener, IOnMapReadyCallback
    {
        private GoogleMap mMap;
        private int _mapType = GoogleMap.MapTypeHybrid;

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
            setJobScheduler();
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

            var location = await FamilyLocationService.GetUserLatLng();
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

            mMap.MapType = _mapType;
            mMap.UiSettings.ZoomControlsEnabled = true;
            mMap.UiSettings.CompassEnabled = true;
            // Do something with the map, i.e. add markers, move to a specific location, etc.
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
            if (drawer.IsDrawerOpen(GravityCompat.Start))
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
            }
            else if (id == Resource.Id.action_changeView)
            {
                Dialog dialog = new Dialog(this);
                dialog.SetContentView(Resource.Layout.custom_dialoge);
                dialog.SetTitle("Select View");
                List<String> stringList = new List<string>();  // here is list 
                stringList.Add("None");
                stringList.Add("Normal");
                stringList.Add("Satellite");
                stringList.Add("Terrain");
                stringList.Add("Hybrid");

                RadioGroup rg = (RadioGroup)dialog.FindViewById(Resource.Id.radio_group);

                foreach(var type in stringList)
                {
                    RadioButton rb1 = new RadioButton(this); // dynamically creating RadioButton and adding to RadioGroup.
                    rb1.Text = type;
                    rg.AddView(rb1);
                }

                dialog.Show();
            }

            return base.OnOptionsItemSelected(item);
        }

        private void setJobScheduler()
        {
            var jobBuilder = this.CreateJobBuilderUsingJobId<UpdateLocationJob>(1);
            var jobInfo = jobBuilder.SetRequiresBatteryNotLow(true).SetPeriodic(900000).Build(); //Minimum 15 minutes https://stackoverflow.com/questions/29492022/job-scheduler-not-running-within-set-interval
            JobScheduler jobScheduler = (JobScheduler)GetSystemService(Context.JobSchedulerService);
            int resultCode = jobScheduler.Schedule(jobInfo);
            if (resultCode == JobScheduler.ResultSuccess)
            {
                Log.Info("Job: ", "Job scheduled!");
            }
            else
            {
                Log.Info("", "Job not scheduled");
            }
        }

        private async void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            var location = await FamilyLocationService.GetUserLatLng();
            updateCamera(location);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            //signIn();
            if (id == Resource.Id.nav_MyListView)
            {
                listView();
            }
            else if (id == Resource.Id.nav_camera)
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

        private void listView()
        {
            StartActivity(typeof(ListViewActivity));
        }

        private void signIn()
        {
            StartActivity(typeof(SignInActivity));
        }
    }
}

