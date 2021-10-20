using NotificationTestApp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotificationTestApp
{
    public partial class MainPage : ContentPage
    {
        INotificationManager notificationManager;

        public MainPage()
        {
            InitializeComponent();

            notificationManager = DependencyService.Get<INotificationManager>(); 
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string title = "Test Title";
            string msg = "Test Msg";

            notificationManager.SendNotification(title, msg);
        }
    }
}
