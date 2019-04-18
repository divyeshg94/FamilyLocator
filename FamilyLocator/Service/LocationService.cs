using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Android.Gms.Maps.Model;
using Plugin.Geolocator;

namespace FamilyLocator.Service
{
    public static class FamilyLocationService
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<LatLng> GetUserLatLng()
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
                return new LatLng(11.016844, 76.955833);
            }
        }

        public static async Task<string> ReverseGeoLoc(string latitude, string longitude)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                var url =
                    "https://maps.googleapis.com/maps/api/geocode/xml?latlng="+ latitude+","+ longitude +"&key=AIzaSyCuP_Dh1pAQuSm4l7gGEzFNUNeB-O4Oo68";

                string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                doc.Load(url);
                var result = doc.SelectSingleNode("GeocodeResponse").SelectSingleNode("result");
                var locationName = result.SelectSingleNode("formatted_address").InnerText;
                return locationName;
            }
            catch (Exception ex)
            {
                return ("(Address lookup failed: ) " + ex.Message);
            }
        }
    }
}