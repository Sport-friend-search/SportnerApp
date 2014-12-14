using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sportner.Dto;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;

namespace Sportner
{
    public class EventPoint
    {
        public string Title { get; set; }
        public Geopoint Point { get; set; }
        public int EventID { get; set; }

        public DateTime Date { get; set; }

        private Sports sportsType;
        public Sports SportsType
        { 
            get 
            {
                return this.sportsType;
            } 
        }

        private PointType typeOfPoint;
        public PointType TypeOfPoint 
        {
            get
            {
                return this.typeOfPoint;
            }
        }

        private RandomAccessStreamReference icon;
        public RandomAccessStreamReference Icon
        {
            get
            {
                return this.icon;
            }
        }

        public UserDto userdto { get; set; }
        public EventDto eventdto { get; set; }

        public EventPoint(Sports type, PointType pointType, int eventID, DateTime date, double latitude, double longitude, UserDto user, EventDto eventobj)
        {
            this.typeOfPoint = pointType;
            this.userdto = user;
            this.eventdto = eventobj;
            this.EventID = eventID;
            this.sportsType = type;
            this.Date = date;
            this.Point = new Geopoint(new BasicGeoposition() { Latitude = latitude, Longitude = longitude});

            switch (type)
            {
                case Sports.Basketball:
                    this.icon = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Basketball-icon.png"));
                    break;
                case Sports.Soccer:
                    this.icon = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Soccer-icon.png"));
                    break;
                case Sports.Volleyball:
                    this.icon = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Volleyball-icon.png"));
                    break;
                case Sports.Tennis:
                    this.icon = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Tennis-icon.png"));
                    break;
                default:
                    this.icon = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Default-icon.png"));
                    break;
            }

            switch (pointType)
            {
                case PointType.Game:
                    this.Title = date.ToString("MM-dd HH:mm");
                    break;
                case PointType.OfficialEvent:
                    this.Title = "Renginys";
                    break;
                case PointType.Training:
                    this.Title = "Treniruotė";
                    break;
                case PointType.Pitch:
                    this.Title = "Aikštelė";
                    break;
                default:
                    this.Title = "";
                    break;
            }
        }
    }
}
