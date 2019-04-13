using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using FamilyLocator.Model;
using FamilyLocator.Service;


//Refer https://www.c-sharpcorner.com/article/xamarin-android-sqlite-database/
namespace FamilyLocator
{
    [Activity(Label = "ListViewActivity")]
    public class ListViewActivity : Activity
    {
        ListView lstViewData;
        List<Location> listSource = new List<Location>();
        Database db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.ListView);
            //Create Database  
            db = new Database();
            db.createDatabase();
            lstViewData = FindViewById<ListView>(Resource.Id.listView);
            var edtLocation = FindViewById<EditText>(Resource.Id.edtLocation);
            var edtTime = FindViewById<EditText>(Resource.Id.edtTime);
            var btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            //var btnEdit = FindViewById<Button>(Resource.Id.btnEdit);
            //var btnRemove = FindViewById<Button>(Resource.Id.btnRemove);
            //Load Data  
            LoadData();
            //Event  
            btnAdd.Click += delegate
            {
                Location location = new Location()
                {
                    LocationLatLng = edtLocation.Text,
                    //Time = DateTime.Parse(edtTime.Text)
                    Time = DateTime.UtcNow
                };
                db.insertIntoTable(location);
                LoadData();
            };
            lstViewData.ItemClick += (s, e) =>
            {
                //Set Backround for selected item  
                for (int i = 0; i < lstViewData.Count; i++)
                {
                    if (e.Position == i)
                        lstViewData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.AntiqueWhite);
                    else
                        lstViewData.GetChildAt(i).SetBackgroundColor(Android.Graphics.Color.Transparent);
                }
                //Binding Data  
                var txtView_Id = e.View.FindViewById<TextView>(Resource.Id.txtView_Id);
                var txtView_LatLng = e.View.FindViewById<TextView>(Resource.Id.txtView_LatLng);
                var txtView_Date = e.View.FindViewById<TextView>(Resource.Id.txtView_Date);
                edtLocation.Text = txtView_LatLng.Text;
                txtView_Date.Text = txtView_Date.Text;
            };
        }

        private void LoadData()
        {
            listSource = db.selectTable();
            var adapter = new ListViewAdaptor(this, listSource);
            lstViewData.Adapter = adapter;
        }
    }
}