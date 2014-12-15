using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI;
using Windows.UI.Core;
using Sportner.Dto;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Sportner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private Geopoint evPoint;
        public GamePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            evPoint = (Geopoint)e.Parameter;
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(evPoint);
            if (result.Status == MapLocationFinderStatus.Success)
            {
                boxAddress.Text = (result.Locations[0].Address.Street + " " + result.Locations[0].Address.StreetNumber).Trim();
                boxCity.Text = result.Locations[0].Address.Town.Trim();
            }
        }

        private async void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            boxCity.Background = new SolidColorBrush(Colors.White);
            boxAddress.Background = new SolidColorBrush(Colors.White);

            if (boxCity.Text.Trim() == "" || boxAddress.Text.Trim() == "")
            {
                if (boxCity.Text.Trim() == "")
                {
                    boxCity.Background = new SolidColorBrush(Colors.Red);
                }
                if (boxAddress.Text.Trim() == "")
                {
                    boxAddress.Background = new SolidColorBrush(Colors.Red);
                }
                MessageDialog msg = new MessageDialog("Prašome patikslinti įvestą informaciją.", "Klaida!");
                msg.Commands.Add(new UICommand("uždaryti", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await msg.ShowAsync());
            }
            else
            {
                if (startDate.Date > endDate.Date)
                {
                    MessageDialog msg = new MessageDialog("Renginio pabaigos data turi būti vėlesnė nei pradžios.", "Klaida!");
                    msg.Commands.Add(new UICommand("uždaryti", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await msg.ShowAsync());
                }
                else
                {
                    if (startDate.Date.ToString("MM/dd/yyyy").Equals(endDate.Date.ToString("MM/dd/yyyy")) && startTime.Time >= endTime.Time)
                    {
                        MessageDialog msg = new MessageDialog("Renginio pabaigos laikas turi būti vėlesnis nei pradžios.", "Klaida!");
                        msg.Commands.Add(new UICommand("uždaryti", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await msg.ShowAsync());
                    }
                    else {
                        // Can be added
                        DateTime startTimeObj = startDate.Date.DateTime;
                        TimeSpan timeStart = startTime.Time;
                        startTimeObj = startTimeObj.Date + timeStart;

                        DateTime endTimeObj = endDate.Date.DateTime;
                        TimeSpan timeEnd = endTime.Time;
                        endTimeObj = endTimeObj.Date + timeEnd;

                        int selectedSportId = 0;

                        if ((bool)buttonBasketball.IsChecked)
                        {
                            selectedSportId = (int)Sports.Basketball;
                        }
                        else if ((bool)buttonFootball.IsChecked)
                        {
                            selectedSportId = (int)Sports.Soccer;
                        }
                        else if ((bool)buttonVolleyball.IsChecked)
                        {
                            selectedSportId = (int)Sports.Volleyball;
                        }
                        else if ((bool)buttonTennis.IsChecked)
                        {
                            selectedSportId = (int)Sports.Tennis;
                        }
                        EventsController eController = new EventsController();
                        await eController.AddEvent(new EventDto { Address = boxAddress.Text, City = boxCity.Text, Description = boxDescription.Text, StartTime = startTimeObj, EndTime = endTimeObj, SportId = selectedSportId, UserId = UserInfo.UserId, Latitude = evPoint.Position.Latitude, Longitude = evPoint.Position.Longitude, EventTypeId = (int)PointType.Game });
                        this.Frame.Navigate(typeof(MainPage));
                    }
                }                
            }
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            //
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
