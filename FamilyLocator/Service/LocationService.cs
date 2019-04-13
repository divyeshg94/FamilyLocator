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

        public static async Task<string> ReverseGeoLoc(string longitude, string latitude)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                var url =
                    "https://maps.googleapis.com/maps/api/geocode/xml?latlng=40.714224,-73.961452&key=AIzaSyCuP_Dh1pAQuSm4l7gGEzFNUNeB-O4Oo68";
                var responseString = await client.GetStringAsync(url);


                string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                if (url.StartsWith(_byteOrderMarkUtf8))
                {
                    url = url.Remove(0, _byteOrderMarkUtf8.Length);
                }
                doc.Load(url);
                XmlNode element = doc.SelectSingleNode("//GeocodeResponse/status");
                if (element.InnerText == "ZERO_RESULTS")
                {
                    return ("No data available for the specified location");
                }
                else
                {
                    element = doc.SelectSingleNode("//GeocodeResponse/result/formatted_address");

                    string longname = "";
                    string shortname = "";
                    string typename = "";
                    bool fHit = false;


                    XmlNodeList xnList = doc.SelectNodes("//GeocodeResponse/result/address_component");
                    //foreach (XmlNode xn in xnList)
                    //{
                    //    try
                    //    {
                    //        longname = xn["long_name"].InnerText;
                    //        shortname = xn["short_name"].InnerText;
                    //        typename = xn["type"].InnerText;


                    //        fHit = true;
                    //        switch (typename)
                    //        {
                    //            //Add whatever you are looking for below
                    //            case "country":
                    //                {
                    //                    Address_country = longname;
                    //                    Address_ShortName = shortname;
                    //                    break;
                    //                }

                    //            case "locality":
                    //                {
                    //                    Address_locality = longname;
                    //                    //Address_locality = shortname; //Om Longname visar sig innehålla konstigheter kan man använda shortname istället
                    //                    break;
                    //                }

                    //            case "sublocality":
                    //                {
                    //                    Address_sublocality = longname;
                    //                    break;
                    //                }

                    //            case "neighborhood":
                    //                {
                    //                    Address_neighborhood = longname;
                    //                    break;
                    //                }

                    //            case "colloquial_area":
                    //                {
                    //                    Address_colloquial_area = longname;
                    //                    break;
                    //                }

                    //            case "administrative_area_level_1":
                    //                {
                    //                    Address_administrative_area_level_1 = longname;
                    //                    break;
                    //                }

                    //            case "administrative_area_level_2":
                    //                {
                    //                    Address_administrative_area_level_2 = longname;
                    //                    break;
                    //                }

                    //            case "administrative_area_level_3":
                    //                {
                    //                    Address_administrative_area_level_3 = longname;
                    //                    break;
                    //                }

                    //            default:
                    //                fHit = false;
                    //                break;
                    //        }


                    //        if (fHit)
                    //        {
                    //            Console.Write(typename);
                    //            Console.ForegroundColor = ConsoleColor.Green;
                    //            Console.Write("\tL: " + longname + "\tS:" + shortname + "\r\n");
                    //            Console.ForegroundColor = ConsoleColor.Gray;
                    //        }
                    //    }

                    //    catch (Exception e)
                    //    {
                    //        //Node missing either, longname, shortname or typename
                    //        fHit = false;
                    //        Console.Write(" Invalid data: ");
                    //        Console.ForegroundColor = ConsoleColor.Red;
                    //        Console.Write("\tX: " + xn.InnerXml + "\r\n");
                    //        Console.ForegroundColor = ConsoleColor.Gray;
                    //    }
                    //}

                    return (element.InnerText);
                }

            }
            catch (Exception ex)
            {
                return ("(Address lookup failed: ) " + ex.Message);
            }
        }
    }
}