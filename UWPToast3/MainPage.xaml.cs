using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPToast3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationViewTitleBar appTitleBar = ApplicationView.GetForCurrentView().TitleBar;

            // Make the title bar transparent
            appTitleBar.BackgroundColor = Colors.Transparent;
            // Get the core appication view title bar
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            /*
                ExtendViewIntoTitleBar
                    Gets or sets a value that specifies whether this title
                    bar should replace the default window title bar.
            */

            // Extend the core application view into title bar
            coreTitleBar.ExtendViewIntoTitleBar = true;
            bool present = ApiInformation.IsTypePresent("Windows.UI.Notifications.Management.UserNotificationListener");
            double xxx = 120;

            
            void Button_Click(object sender, RoutedEventArgs e)
            {
            }
            
            
        }

        public void setText(string text)
        {
            boxText.Text += text + Environment.NewLine;
        }

        async Task<int> getAccess()
        {
            //await Windows.ApplicationModel.FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();


            // Get the listener
            UserNotificationListener listener = UserNotificationListener.Current;

            // And request access to the user's notifications (must be called from UI thread)
            UserNotificationListenerAccessStatus accessStatus = await listener.RequestAccessAsync();

            switch (accessStatus)
            {
                // This means the user has granted access.
                case UserNotificationListenerAccessStatus.Allowed:

                    if (!BackgroundTaskRegistration.AllTasks.Any(k => k.Value.Name.Equals("UserNotificationChanged")))
                    {
                        // Specify the background task
                        var builder = new BackgroundTaskBuilder()
                        {
                            Name = "UserNotificationChanged"
                        };

                        // Set the trigger for Listener, listening to Toast Notifications
                        builder.SetTrigger(new UserNotificationChangedTrigger(NotificationKinds.Toast));

                        // Register the task
                        builder.Register();
                    }

                    IReadOnlyList<UserNotification> notifs = await listener.GetNotificationsAsync(NotificationKinds.Toast);
                    IReadOnlyList<UserNotification> notifsUn = await listener.GetNotificationsAsync(NotificationKinds.Unknown);
                    double notifCount = notifs.Count;
                    if (notifCount > 0)
                    {
                        UserNotification notif = notifs[0];
                        string message = notif.Notification.ToString();

                        if (notif.AppInfo.DisplayInfo.DisplayName != null)
                        {
                            // Get the app's display name
                            string appDisplayName = notif.AppInfo.DisplayInfo.DisplayName;
                        }

                        // Get the app's logo
                        BitmapImage appLogo = new BitmapImage();
                        RandomAccessStreamReference appLogoStream = notif.AppInfo.DisplayInfo.GetLogo(new Size(16, 16));
                        await appLogo.SetSourceAsync(await appLogoStream.OpenReadAsync());
                    }

                    // Yay! Proceed as normal
                    break;

                    // This means the user has denied access.
                    // Any further calls to RequestAccessAsync will instantly
                    // return Denied. The user must go to the Windows settings
                    // and manually allow access.

                case UserNotificationListenerAccessStatus.Denied:

                    // Show UI explaining that listener features will not
                    // work until user allows access.
                    break;

                // This means the user closed the prompt without
                // selecting either allow or deny. Further calls to
                // RequestAccessAsync will show the dialog again.
                case UserNotificationListenerAccessStatus.Unspecified:

                    // Show UI that allows the user to bring up the prompt again
                    break;
            }
            int i = 1;
            return i;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            getAccess();

        }
    }
}
