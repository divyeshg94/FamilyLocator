using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace FamilyLocator.Model
{
    [Table("Location")]
    public class Location
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [MaxLength(30)]
        public string LocationLatLng { get; set; }

        public DateTime Time { get; set; }

        public string LocationName { get; set; }

        public Location()
        {

        }

        //public Location(LatLng locationLatLan)
        //{
        //    var location = new Location()
        //    {
        //        LocationLatLng = locationLatLan.Latitude + " " + locationLatLan.Longitude,
        //        Time = DateTime.UtcNow
        //    };
        //}
    }
}