using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Sportner.Messages.UserMessages;
using Sportner.Models;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Runtime.Serialization;
using RestAPIClient;
using System.Threading;
using Sportner.Pages;
using Sportner.Dto;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Sportner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<EventPoint> points = new List<EventPoint>();
        private bool basketToggleTap = true;
        private bool soccerToggleTap = true;
        private bool volleyToggleTap = true;
        private bool tennisToggleTap = true;
        private bool firstTime;
        private TimeSpan showingHours = new TimeSpan(12, 0, 0);
        public DispatcherTimer dt = new DispatcherTimer();

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            firstTime = true;
            dt.Tick += dt_Tick;
            dt.Interval = TimeSpan.FromMilliseconds(2000);
            dt.Start();
        }

        private void dt_Tick(object sender, object e)
        {
            loadPoints();
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            if (firstTime)
            {
                MyMap.MapElements.Clear();
                loadPoints();
                displayAllPins();
                MyMap.Center = new Geopoint(new BasicGeoposition { Latitude = 54.693759, Longitude = 25.275651 });
                MyMap.ZoomLevel = 14;
                firstTime = false;
            }
            base.OnNavigatedTo(e);
        }

        private async void loadPoints()
        {
            //points.Add(new EventPoint(Sports.Basketball, PointType.Game, 147, new DateTime(2014, 12, 9, 18, 30, 0), 54.696536, 25.283834));
            //points.Add(new EventPoint(Sports.Basketball, PointType.Pitch, 148, new DateTime(), 54.697466, 25.288447));
            //points.Add(new EventPoint(Sports.Basketball, PointType.Training, 149, new DateTime(2014, 12, 9, 20, 15, 0), 54.702215, 25.271131));
            //points.Add(new EventPoint(Sports.Soccer, PointType.Game, 150, new DateTime(2014, 12, 10, 9, 0, 0), 54.693486, 25.289520));
            //points.Add(new EventPoint(Sports.Soccer, PointType.OfficialEvent, 153, new DateTime(2014, 12, 10, 10, 0, 0), 54.696229, 25.259359));
            //points.Add(new EventPoint(Sports.Volleyball, PointType.Game, 154, new DateTime(2014, 12, 14, 13, 15, 0), 54.693178, 25.274122));
            //points.Add(new EventPoint(Sports.Volleyball, PointType.Training, 155, new DateTime(2014, 12, 9, 23, 30, 0), 54.692831, 25.274336));
            //points.Add(new EventPoint(Sports.Volleyball, PointType.Training, 156, new DateTime(2014, 12, 31, 23, 59, 0), 54.697481, 25.266805));
            //points.Add(new EventPoint(Sports.Tennis, PointType.Pitch, 161, new DateTime(), 54.700457, 25.251935));

            EventsController eController = new EventsController();
            var getev = await eController.GetAllEventsPoints();
            points.Clear();
            points.AddRange(getev);
            if (trainingsSwitch.IsOn)
            {
                showTrainings();
            }
            if (pitchesSwitch.IsOn)
            {
                showPitches();
            }
            if (basketToggleTap)
            {
                addPins(Sports.Basketball, PointType.Game, showingHours);
                addPins(Sports.Basketball, PointType.OfficialEvent, showingHours);
            }
            if (soccerToggleTap)
            {
                addPins(Sports.Soccer, PointType.Game, showingHours);
                addPins(Sports.Soccer, PointType.OfficialEvent, showingHours);
            }
            if (volleyToggleTap)
            {
                addPins(Sports.Volleyball, PointType.Game, showingHours);
                addPins(Sports.Volleyball, PointType.OfficialEvent, showingHours);
            }
            if (tennisToggleTap)
            {
                addPins(Sports.Tennis, PointType.Game, showingHours);
                addPins(Sports.Tennis, PointType.OfficialEvent, showingHours);
            }
            // Find geopoint from address
            //Geopoint center = new Geopoint(new BasicGeoposition() { Latitude = 54.700457, Longitude = 25.251935 });
            //MapLocationFinderResult result3 = await MapLocationFinder.FindLocationsAsync("Dariaus ir Girėno gatvė 19, Panevėžys", center);
            //if (result3.Status == MapLocationFinderStatus.Success && result3.Locations.Count != 0)
            //{
            //    points.Add(new EventPoint(Sports.Basketball, PointType.Pitch, 7, new DateTime(), result3.Locations[0].Point.Position.Latitude, result3.Locations[0].Point.Position.Longitude));
            //}

        }

        private void displayAllPins()
        {
            addPins(Sports.Basketball, PointType.Game, showingHours);
            addPins(Sports.Basketball, PointType.OfficialEvent, showingHours);
            addPins(Sports.Basketball, PointType.Training, showingHours);
            addPins(Sports.Soccer, PointType.Game, showingHours);
            addPins(Sports.Soccer, PointType.OfficialEvent, showingHours);
            addPins(Sports.Soccer, PointType.Training, showingHours);
            addPins(Sports.Volleyball, PointType.Game, showingHours);
            addPins(Sports.Volleyball, PointType.OfficialEvent, showingHours);
            addPins(Sports.Volleyball, PointType.Training, showingHours);
            addPins(Sports.Tennis, PointType.Game, showingHours);
            addPins(Sports.Tennis, PointType.OfficialEvent, showingHours);
            addPins(Sports.Tennis, PointType.Training, showingHours);
            //MyMap.MapElements.Clear(); remove all pins
        }

        private Geopoint selectedPoint;
        private UserDto selectedUser;

        private async void MyMap_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            int found = sender.FindMapElementsAtOffset(args.Position).Count();

            if (found > 0)
            {
                var resultText = new StringBuilder();
              
                foreach (var mapObject in sender.FindMapElementsAtOffset(args.Position))
                {
                    resultText.AppendLine("Aprašymas: " + mapObject.ReadData<EventPoint>().eventdto.Description);
                    resultText.AppendLine("Pradžios laikas: " + mapObject.ReadData<EventPoint>().eventdto.StartTime.ToString("MM-dd HH:mm"));
                    resultText.AppendLine("Pabaigos laikas: " + mapObject.ReadData<EventPoint>().eventdto.EndTime.ToString("MM-dd HH:mm"));
                    resultText.AppendLine("Adresas: " + mapObject.ReadData<EventPoint>().eventdto.Address + ", " +  mapObject.ReadData<EventPoint>().eventdto.City);
                    resultText.AppendLine("");
                    resultText.AppendLine(mapObject.ReadData<EventPoint>().userdto.Username);
                    selectedUser = mapObject.ReadData<EventPoint>().userdto;

                }
                var dialog = new MessageDialog(resultText.ToString());
                dialog.Commands.Add(new UICommand("kontaktai", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                dialog.Commands.Add(new UICommand("uždaryti", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
            }
            else
            {
                var resultText = new StringBuilder();
                resultText.AppendLine(string.Format("Naujo įvykio sukūrimas koordinatėse: {0:F9}, {1:F9}", args.Location.Position.Latitude, args.Location.Position.Longitude));
                selectedPoint = new Geopoint(new BasicGeoposition{ Latitude = args.Location.Position.Latitude, Longitude = args.Location.Position.Longitude});
               
                MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(new Geopoint(args.Location.Position));
                if (result.Status == MapLocationFinderStatus.Success)
                {
                    string address = result.Locations[0].Address.Street + " " + result.Locations[0].Address.StreetNumber + ", " + result.Locations[0].Address.Town;
                    resultText.AppendLine("Adresas: " + address);
                }
                MessageDialog dialog = new MessageDialog(resultText.ToString());
                dialog.Commands.Add(new UICommand("sukurti", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                dialog.Commands.Add(new UICommand("uždaryti", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
            }
        }
        private void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label.Equals("sukurti"))
            {
                Frame.Navigate(typeof(AddEventPage), selectedPoint);
            }
            if (command.Label.Equals("kontaktai"))
            {
                Frame.Navigate(typeof(ProfilePage), new UserInList { BirthDate = selectedUser.BirthDate, City = selectedUser.City, Email = selectedUser.Email, PhoneNumber = selectedUser.PhoneNumber, Username = selectedUser.Username, UserId = selectedUser.UserId});
            }
        }

        private void addPins(Sports type, PointType ptype, TimeSpan time)
        {
            var filteredPoints = points.Where(p => p.SportsType.Equals(type)).Where(pt => pt.TypeOfPoint.Equals(ptype)).Where(pn => pn.Date <= DateTime.Now.Add(time) && pn.Date > DateTime.Now);
            foreach (EventPoint point in filteredPoints)
            {
                Point anchorPoint = new Point(0.5, 0.5);
                RandomAccessStreamReference icon = point.Icon;
                MapIcon pin = new MapIcon
                {
                    Title = point.Title,
                    Location = point.Point,
                    NormalizedAnchorPoint = anchorPoint,
                    Image = icon,
                    ZIndex = 5
                };
                pin.AddData(point);
                MyMap.MapElements.Add(pin);
            }
        }

        private void removePins(Sports type)
        {
            var pins = MyMap.MapElements.Where(p => p.ReadData<EventPoint>().SportsType.Equals(type));
            if (pins.Any())
            {
                foreach (var pin in pins.ToList())
                {
                    MyMap.MapElements.Remove(pin);
                }
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyMap.ZoomLevel < 20)
            {
                MyMap.ZoomLevel++;
            }
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (MyMap.ZoomLevel > 1)
            {
                MyMap.ZoomLevel--;
            }
        }

        private void basketButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (basketToggleTap == true)
            {
                basketButton.Opacity = 0.3;
                removePins(Sports.Basketball);
                basketToggleTap = false;
            }
            else
            {
                basketButton.Opacity = 1;

                addPins(Sports.Basketball, PointType.Game, showingHours);
                addPins(Sports.Basketball, PointType.OfficialEvent, showingHours);

                if (pitchesSwitch.IsOn)
                {
                    addPins(Sports.Basketball, PointType.Pitch, showingHours);
                }

                if (trainingsSwitch.IsOn)
                {
                    addPins(Sports.Basketball, PointType.Training, showingHours);
                }

                basketToggleTap = true;
            }
        }

        private void soccerButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (soccerToggleTap == true)
            {
                soccerButton.Opacity = 0.3;
                removePins(Sports.Soccer);
                soccerToggleTap = false;
            }
            else
            {
                soccerButton.Opacity = 1;

                addPins(Sports.Soccer, PointType.Game, showingHours);
                addPins(Sports.Soccer, PointType.OfficialEvent, showingHours);

                if (pitchesSwitch.IsOn)
                {
                    addPins(Sports.Soccer, PointType.Pitch, showingHours);
                }

                if (trainingsSwitch.IsOn)
                {
                    addPins(Sports.Soccer, PointType.Training, showingHours);
                }

                soccerToggleTap = true;
            }
        }

        private void volleyButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (volleyToggleTap == true)
            {
                volleyButton.Opacity = 0.3;
                removePins(Sports.Volleyball);
                volleyToggleTap = false;
            }
            else
            {
                volleyButton.Opacity = 1;

                addPins(Sports.Volleyball, PointType.Game, showingHours);
                addPins(Sports.Volleyball, PointType.OfficialEvent, showingHours);

                if (pitchesSwitch.IsOn)
                {
                    addPins(Sports.Volleyball, PointType.Pitch, showingHours);
                }

                if (trainingsSwitch.IsOn)
                {
                    addPins(Sports.Volleyball, PointType.Training, showingHours);
                }

                volleyToggleTap = true;
            }
        }

        private void tennisButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (tennisToggleTap == true)
            {
                tennisButton.Opacity = 0.3;
                removePins(Sports.Tennis);
                tennisToggleTap = false;
            }
            else
            {
                tennisButton.Opacity = 1;

                addPins(Sports.Tennis, PointType.Game, showingHours);
                addPins(Sports.Tennis, PointType.OfficialEvent, showingHours);

                if (pitchesSwitch.IsOn)
                {
                    addPins(Sports.Tennis, PointType.Pitch, showingHours);
                }

                if (trainingsSwitch.IsOn)
                {
                    addPins(Sports.Tennis, PointType.Training, showingHours);
                }

                tennisToggleTap = true;
            }
        }

        private void showPitches()
        {
            if (basketToggleTap)
            {
                addPins(Sports.Basketball, PointType.Pitch, showingHours);
            }

            if (soccerToggleTap)
            {
                addPins(Sports.Soccer, PointType.Pitch, showingHours);
            }

            if (volleyToggleTap)
            {
                addPins(Sports.Volleyball, PointType.Pitch, showingHours);
            }

            if (tennisToggleTap)
            {
                addPins(Sports.Tennis, PointType.Pitch, showingHours);
            }
        }

        private void hidePitches()
        {
            var pins = MyMap.MapElements.Where(p => p.ReadData<EventPoint>().TypeOfPoint.Equals(PointType.Pitch));
            if (pins.Any())
            {
                foreach (var pin in pins.ToList())
                {
                    MyMap.MapElements.Remove(pin);
                }
            }
        }

        private void showTrainings()
        {
            if (basketToggleTap)
            {
                addPins(Sports.Basketball, PointType.Training, showingHours);
            }

            if (soccerToggleTap)
            {
                addPins(Sports.Soccer, PointType.Training, showingHours);
            }

            if (volleyToggleTap)
            {
                addPins(Sports.Volleyball, PointType.Training, showingHours);
            }

            if (tennisToggleTap)
            {
                addPins(Sports.Tennis, PointType.Training, showingHours);
            }
        }

        private void hideTrainings()
        {
            var pins = MyMap.MapElements.Where(p => p.ReadData<EventPoint>().TypeOfPoint.Equals(PointType.Training));
            if (pins.Any())
            {
                foreach (var pin in pins.ToList())
                {
                    MyMap.MapElements.Remove(pin);
                }
            }
        }

        private void hideAccordingTime(TimeSpan hours)
        {
            var pins = MyMap.MapElements.Where(p => p.ReadData<EventPoint>().TypeOfPoint != PointType.Pitch).Where(pt => pt.ReadData<EventPoint>().Date > DateTime.Now.Add(hours));
            if (pins.Any())
            {
                foreach (var pin in pins.ToList())
                {
                    MyMap.MapElements.Remove(pin);
                }
            }
        }

        private void AppBarButton_Click_4(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage), 0);
        }

        private void AppBarButton_Click_5(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage), 1);
        }

        private void AppBarButton_Click_6(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(MenuPage), 2);
            Frame.Navigate(typeof(BestSpPage));
        }

        private void AppBarButton_Click_7(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage), 3);
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage), 4);
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage), 5);
        }

        private void AppBarButton_Click_8(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FirstPage));
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (pitchesSwitch != null)
            {
                if (pitchesSwitch.IsOn)
                {
                    showPitches();
                }
                else
                {
                    hidePitches();
                }
            }
        }

        private void ToggleSwitch_Toggled_1(object sender, RoutedEventArgs e)
        {
            if (trainingsSwitch != null)
            {
                if (trainingsSwitch.IsOn)
                {
                    showTrainings();
                }
                else
                {
                    hideTrainings();
                }
            }
        }

        private void slider2_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (slider2 != null)
            {
                timeInfo.Text = slider2.Value + " h";
                TimeSpan oldShowingHours = showingHours;
                showingHours = new TimeSpan(Int32.Parse(slider2.Value.ToString()), 0, 0);
                if (oldShowingHours > showingHours)
                {
                    hideAccordingTime(showingHours);
                }
                else
                {
                    if (trainingsSwitch.IsOn)
                    {
                        showTrainings();
                    }
                    if (basketToggleTap)
                    {
                        addPins(Sports.Basketball, PointType.Game, showingHours);
                        addPins(Sports.Basketball, PointType.OfficialEvent, showingHours);
                    }
                    if (soccerToggleTap)
                    {
                        addPins(Sports.Soccer, PointType.Game, showingHours);
                        addPins(Sports.Soccer, PointType.OfficialEvent, showingHours);
                    }
                    if (volleyToggleTap)
                    {
                        addPins(Sports.Volleyball, PointType.Game, showingHours);
                        addPins(Sports.Volleyball, PointType.OfficialEvent, showingHours);
                    }
                    if (tennisToggleTap)
                    {
                        addPins(Sports.Tennis, PointType.Game, showingHours);
                        addPins(Sports.Tennis, PointType.OfficialEvent, showingHours);
                    }
                }
            }
        }

        private void slider1_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (slider1 != null)
            {
                distanceInfo.Text = slider1.Value + " km";
            }
        }

    }
}
