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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using Sportner.Dto;

namespace Sportner.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BestSpPage : Page
    {
        public BestSpPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //Gauna duomenis
            UserController uc = new UserController();
            var users = await uc.GetUsers();   

            foreach (var user in users)
            {
                var userInList = new UserInList
                {
                    Email = user.Email,
                    Username = user.Username,
                    UserId = user.UserId,
                    City = user.City,
                    BirthDate = user.BirthDate,
                    PhoneNumber = user.PhoneNumber
                };

                listSportner.Items.Add(userInList);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {     
            this.Frame.Navigate(typeof(ProfilePage), listSportner.SelectedValue);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            //Turi grįžti į menu
            this.Frame.Navigate(typeof(MainPage));
        }

        


    }
}
