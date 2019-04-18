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
using FamilyLocator.Model;

namespace FamilyLocator.Service
{
    public class ListViewAdaptor : BaseAdapter
    {
        private Activity activity;
        private List<Location> listLocation;

        public ListViewAdaptor(Activity activity, List<Location> listLocation)
        {
            this.activity = activity;
            this.listLocation = listLocation;
        }

        public override int Count
        {
            get { return listLocation.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return listLocation[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_view, parent, false);
            var txtId= view.FindViewById<TextView>(Resource.Id.txtView_Id);
            var txtLatLng= view.FindViewById<TextView>(Resource.Id.txtView_LatLng);
            var txtLocationName= view.FindViewById<TextView>(Resource.Id.txtView_Name);
            var txtDate= view.FindViewById<TextView>(Resource.Id.txtView_Date);
            txtId.Text = listLocation[position].Id.ToString();
            txtLatLng.Text = listLocation[position].LocationLatLng;
            txtDate.Text = listLocation[position].Time.ToLocalTime().ToString();
            if (string.IsNullOrEmpty(listLocation[position].LocationName))
            {
                var latLng = listLocation[position].LocationLatLng.Split(",");
                var locationName = FamilyLocationService.ReverseGeoLoc(latLng[0], latLng[1]).Result;
                listLocation[position].LocationName = locationName;
            }

            txtLocationName.Text = listLocation[position].LocationName.ToString();
            return view;
        }
    }
}