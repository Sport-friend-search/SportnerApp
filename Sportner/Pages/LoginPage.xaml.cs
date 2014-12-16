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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using Sportner.Pages;

namespace Sportner
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            boxName.Background = new SolidColorBrush(Colors.White);
            boxPassword.Background = new SolidColorBrush(Colors.White);
            if (!System.Text.RegularExpressions.Regex.IsMatch(boxName.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                boxName.Text = string.Empty;
                boxName.Background = new SolidColorBrush(Colors.Red);
            }
            if (boxName.Text == "" || boxPassword.Password.Trim() == "")
            {
                if (boxPassword.Password.Trim() == "")
                {
                    boxPassword.Background = new SolidColorBrush(Colors.Red);
                }
                MessageDialog msg = new MessageDialog("Prašome patikslinti įvestą informaciją.", "Klaida!");
                msg.Commands.Add(new UICommand("uždaryti", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await msg.ShowAsync());
            }
            else
            {
                string email = boxName.Text;
                string password = boxPassword.Password;

                // Can be logged in
                var loginobj = await new UserController().Login(email, password);
                if (loginobj != null)
                {
                    if (loginobj.IsAuthorized)
                    {
                        UserInfo.UserId = loginobj.UserId;
                        this.Frame.Navigate(typeof(MainPage));
                    }
                    else
                    {
                        MessageDialog msg = new MessageDialog("Duomenys neteisingi.", "Klaida!");
                        msg.Commands.Add(new UICommand("uždaryti", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await msg.ShowAsync());
                    }
                } else {
                        MessageDialog msg = new MessageDialog("Patikrinkite interneto ryšį.", "Klaida!");
                        msg.Commands.Add(new UICommand("uždaryti", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await msg.ShowAsync());
                }
            }            
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
