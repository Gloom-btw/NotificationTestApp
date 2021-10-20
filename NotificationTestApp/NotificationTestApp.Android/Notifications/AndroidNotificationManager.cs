using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using AndroidX.Core.App;
using NotificationTestApp.Interfaces;
using Xamarin.Forms;


[assembly: Dependency(typeof(NotificationTestApp.Droid.Notifications.AndroidNotificationManager))]
namespace NotificationTestApp.Droid.Notifications
{
    public class AndroidNotificationManager : INotificationManager
    {

        const string channelId = "default";

        const string channelName = "Default";

        const string channelDescription = "The default channel for notifications.";

        public const string TitleKey = "title";

        public const string MessageKey = "message";

        bool channelInitialized = false;

        int messageId = 0;

        int pendingIntentId = 0;

        NotificationManager manager;

        public static AndroidNotificationManager Instance { get; private set; }

        public event EventHandler NotificationReceived;

        public AndroidNotificationManager() => Initialize();

        public void Initialize()
        {
            if (Instance == null)
            {
                CreateNotificationChannel();
                Instance = this;
            }
        }

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)Android.App.Application.Context.GetSystemService(Android.App.Application.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            else
            {
                Show(title, message);
            }
        }

        public void Show(string title, string message)
        {
            Intent intent = new Intent(Android.App.Application.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            var color = Android.Graphics.Color.Red;
                var notificationLayout = new RemoteViews(Android.App.Application.Context.PackageName, 2131427378);
            PendingIntent pendingIntent = PendingIntent.GetActivity(Android.App.Application.Context, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);
            NotificationCompat.Builder builder = new NotificationCompat.Builder(Android.App.Application.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetColor(color)
                .SetColorized(true)
                .SetStyle(new AndroidX.Media.App.NotificationCompat.MediaStyle())
                .SetCustomContentView(notificationLayout)
                .SetCustomBigContentView(notificationLayout)
                .SetLargeIcon(BitmapFactory.DecodeResource(Android.App.Application.Context.Resources, Resource.Drawable.notification_template_icon_bg))
                .SetSmallIcon(Resource.Drawable.notification_template_icon_bg)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetPriority((int)NotificationPriority.Max);
            

            
            Notification notification = builder.Build();
            manager.Notify(messageId++, notification);
        }
    }
}