using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Sportner
{
    public static class MapController
    {
        static public double DistanceBetween(Geopoint fpoint, Geopoint spoint)
        {
            double rlat1 = Math.PI * fpoint.Position.Latitude / 180;
            double rlat2 = Math.PI * spoint.Position.Latitude / 180;
            double rlon1 = Math.PI * fpoint.Position.Longitude / 180;
            double rlon2 = Math.PI * spoint.Position.Longitude / 180;
            double theta = fpoint.Position.Longitude - spoint.Position.Longitude;
            double rtheta = Math.PI * theta / 180;
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515 * 1.609344;
            return dist;
        }

        static public bool IsInRadius(Geopoint fpoint, Geopoint spoint, double radius)
        {
            return DistanceBetween(fpoint, spoint) <= radius;
        }

        static public async Task<Geoposition> GetMyLocation()
        {
            Geolocator geolocator = new Geolocator();     
            geolocator.DesiredAccuracyInMeters = 50;     
            try     
            {          
                Geoposition geoposition = await geolocator.GetGeopositionAsync(maximumAge: TimeSpan.FromMinutes(5), timeout: TimeSpan.FromSeconds(10));
                return geoposition;
            }     
            catch (UnauthorizedAccessException e)     
            {         
                // the app does not have the right capability or the location master switch is off   
                throw e;
            } 
        }
    }
}
