using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FamilyLocator.Service
{
    [Service(Name = "com.xamarin.FamilyLocator.UpdateLocationJob",
        Permission = "android.permission.BIND_JOB_SERVICE")]
    public class UpdateLocationJob : JobService
    {
        public override bool OnStartJob(JobParameters jobParams)
        {
            Task.Run(() =>
            {
                // Work is happening asynchronously

                // Have to tell the JobScheduler the work is done. 
                JobFinished(jobParams, false);
            });

            // Return true because of the asynchronous work
            return true;
        }

        public override bool OnStopJob(JobParameters jobParams)
        {
            // we don't want to reschedule the job if it is stopped or cancelled.
            return false;
        }
    }
}