using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Sportner.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
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
            var user = (UserInList)e.Parameter;

            blockFirstName.Text = user.Username ?? "-";
            blockLastName.Text = user.Email ?? "-";
            blockBirthdate.Text = user.BirthDate.Value.ToString("yyyy-MM-dd");
            blockPhoneNumber.Text = user.PhoneNumber.ToString();
            if(blockPhoneNumber.Text.Equals("")){
                blockPhoneNumber.Text = "nenurodytas";
                blockPhoneNumber.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BestSpPage));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void blockPhoneNumber_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(!blockPhoneNumber.Text.Equals("nenurodytas")){
                Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(blockPhoneNumber.Text, blockFirstName.Text);
            }
        }
    }
}
