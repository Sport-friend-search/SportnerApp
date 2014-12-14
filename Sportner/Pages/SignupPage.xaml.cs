using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
using System.Runtime.Serialization.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using Newtonsoft.Json;
using RestSharp.Portable;
using Sportner.Dto;
using Sportner.Messages.UserMessages;

namespace Sportner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignupPage : Page
    {
        public SignupPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            boxFirstName.Background = new SolidColorBrush(Colors.White);
            boxLastName.Background = new SolidColorBrush(Colors.White);
            boxPhoneNumber.Background = new SolidColorBrush(Colors.White);
            boxEmail.Background = new SolidColorBrush(Colors.White);
            boxPassword.Background = new SolidColorBrush(Colors.White);

            if (!System.Text.RegularExpressions.Regex.IsMatch(boxFirstName.Text, "^[A-Z][a-zA-Z]*$"))
            {
                boxFirstName.Text = string.Empty;
                boxFirstName.Background = new SolidColorBrush(Colors.Red);
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(boxLastName.Text, "^[A-Z][a-zA-Z]*$"))
            {
                boxLastName.Text = string.Empty;
                boxLastName.Background = new SolidColorBrush(Colors.Red);
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(boxPhoneNumber.Text, @"^\d*$"))
            {
                boxPhoneNumber.Text = string.Empty;
                boxPhoneNumber.Background = new SolidColorBrush(Colors.Red);
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(boxEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                boxEmail.Text = string.Empty;
                boxEmail.Background = new SolidColorBrush(Colors.Red);
            }
            if (boxFirstName.Text == "" || boxLastName.Text == "" || boxEmail.Text == "" || boxPassword.Password == "")
            {
                if (boxPassword.Password == "")
                {
                    boxPassword.Background = new SolidColorBrush(Colors.Red);
                }
                MessageDialog msg = new MessageDialog("Prašome patikslinti įvestą informaciją.", "Klaida!");
                msg.Commands.Add(new UICommand("uždaryti", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await msg.ShowAsync());
            }
            else
            {
                if (boxBirthDate.Date > DateTime.Now)
                {
                    boxBirthDate.Background = new SolidColorBrush(Colors.Red);
                    MessageDialog msg = new MessageDialog("Gimimo data negali būti ateityje.", "Klaida!");
                    msg.Commands.Add(new UICommand("uždaryti", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await msg.ShowAsync());
                }
                else
                {
                    var userToRegister = new UserDto
                    {
                        Username = boxFirstName.Text + " " + boxLastName.Text,
                        Email = boxEmail.Text,
                        Password = boxPassword.Password,
                        PhoneNumber = Convert.ToInt32(boxPhoneNumber.Text),
                        InterestedSportsIds = GetSportIds(),
                        BirthDate = boxBirthDate.Date.Date
                    };

                    UserController userController = new UserController();
                    
                    userController.InsertUser(userToRegister);
                    // Can be registered
                    this.Frame.Navigate(typeof(LoginPage));
                }    
            }   
        }

        private List<int> GetSportIds()
        {
            List<int> sportIdsList = new List<int>();

            if (boxBasketball.IsChecked.Equals(true))
            {
                sportIdsList.Add((int)SportsEnum.BasketBall);
            }

            if (boxTennis.IsChecked.Equals(true))
            {
                sportIdsList.Add((int)SportsEnum.Tennis);
            }

            if (boxVolleyball.IsChecked.Equals(true))
            {
                sportIdsList.Add((int)SportsEnum.Volleyball);
            }

            return sportIdsList;
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FirstPage));
        }
    }
}
