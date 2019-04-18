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

            var locationName = FamilyLocationService.ReverseGeoLoc(latLng.Latitude.ToString(), latLng.Longitude.ToString()).Result;

            var db = new Database();
            var location = new Location()
            {
                LocationLatLng = latLng.Latitude + " " + latLng.Longitude,
                Time = DateTime.UtcNow,
                LocationName = locationName
            };
            db.insertIntoTable(location);
        }
    }
}