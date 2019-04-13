using System;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Util;
using FamilyLocator.Model;

namespace FamilyLocator.Service
{
    [Service(Name = "com.xamarin.FamilyLocator.UpdateLocationJob",
        Permission = "android.permission.BIND_JOB_SERVICE")]
    public class UpdateLocationJob : JobService
    {
        bool isWorking = false;
        bool jobCancelled = false;

        public override bool OnStartJob(JobParameters jobParams)
        {
            Log.Info("UpdateLocationJob: ", "Job started! ");
            isWorking = true;
            Task.Run(() =>
            {
                // Work is happening asynchronously
                UpdateLocationInfo();
                // Have to tell the JobScheduler the work is done. 
                JobFinished(jobParams, false);
            });

            // Return true because of the asynchronous work
            return true;
        }

        public override bool OnStopJob(JobParameters jobParams)
        {
            Log.Info("UpdateLocationJob: ", "Job stopped! ");
            // we don't want to reschedule the job if it is stopped or cancelled.
            return false;
        }

        private async Task UpdateLocationInfo()
        {
            var latLng = await FamilyLocationService.GetUserLatLng();
            var db = new Database();
            var location = new Location()
            {
                LocationLatLng = latLng.Latitude + " " + latLng.Longitude,
                Time = DateTime.UtcNow
            };
            db.insertIntoTable(location);
            string Address_ShortName;
            string Address_country;
            string Address_administrative_area_level_1;
            string Address_administrative_area_level_2;
            string Address_administrative_area_level_3;
            string Address_colloquial_area;
            string Address_locality;
            string Address_sublocality;
            string Address_neighborhood;
            FamilyLocationService.ReverseGeoLoc(latLng.Latitude.ToString(), latLng.Longitude.ToString(), out Address_ShortName, out Address_country,
                out Address_administrative_area_level_1, out Address_administrative_area_level_2, out Address_administrative_area_level_3, 
                out Address_colloquial_area, out Address_locality, out Address_sublocality, out Address_neighborhood);
        }
    }
}