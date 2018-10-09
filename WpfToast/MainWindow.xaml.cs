using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Drawing;
using System.Runtime.InteropServices;
using Dcolor = System.Drawing.Color;
using Mcolor = System.Windows.Media.Color;
using System.Threading;
using System.Diagnostics;

[assembly: DisableDpiAwareness]

namespace WpfToast
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard myStoryboard;
        private Double windowOpacity = 1;
        private Double movInSpeed = 0.5;
        private Double moveOutSpeed = 0.3;
        private double defaultDpi = 96;
        private List<ToastWindow> toastWidnows = new List<ToastWindow>();
        private string imgPath = "";
        private string title = "";
        private string mainMessage = "";
        private string subLine = "";
        private string state = "";
        private Dcolor stateColor = Dcolor.White;
        private int windowCounter = 1;
        private bool toastsClosing = false;

        public MainWindow()
        {
            InitializeComponent();

            //UserNotificationListener listener = UserNotificationListener.Current;

            this.Opacity = 0;
            this.Visibility = Visibility.Hidden;
            this.Top = 200;
            //this.Left = (1706 / 2) - (this.Width / 2);
            this.Left = 1706 + (1920 / 144 * 96);
            this.Left = 1706 + 1920;
            //this.Left = (2560 / 144 * 96) + (1920 / 144 * 96);
            this.Left = (2560 / 144 * 96) + (1920 / 144 * 96);
            this.Left = 2560;
            this.Left = 5000;

            //this.Left = 1706;
            //this.Left = 1800;
            //this.WindowState = WindowState.Normal;


            //this.Left = 0;
            string[] args = Environment.GetCommandLineArgs();
            List<string[]> actions = new List<string[]>();
            if (args.Length >= 2) {
                this.Hide();
                //if(args.Length == 2)
                //{
                //    mainMessage = args[1];
                //}
                this.ShowInTaskbar = false;
                mainText.Content = args[1];
                textAdditional.Content = this.Left;
                //ToastWindow tw = new ToastWindow(this, args[1]);
                //tw.Left = System.Windows.SystemParameters.VirtualScreenWidth / 2;
                //tw.Left = 2560 + 1920 + (1920 / 2);
                //tw.Show();
                bool namedp = false;
                for(int i = 1; i <= args.Length - 1; i++)//counts up one!
                {
                    string arg = args[i];
                    if (!namedp && !arg.StartsWith("-"))
                    {
                        if (i == 1)
                        {
                            title = arg;
                            mainMessage = arg;
                        }
                        else if (i == 2)
                        {
                            title = args[i - 1];
                            mainMessage = arg;
                        }
                        else if (i == 3)
                        {
                            if (File.Exists(arg))
                            {
                                imgPath = arg;
                            }
                        }
                    }
                    else if (arg.StartsWith("-"))
                    {
                        namedp = true;
                        if (arg.Length >= i + 1)
                        {
                            string nextS = args[i + 1];
                            switch (arg.ToLower())
                            {
                                case "-title":
                                    title = nextS;
                                    break;
                                case "-message":
                                case "-mainmessage":
                                case "-msg":
                                case "-text":
                                    mainMessage = nextS;
                                    break;
                                case "-image":
                                    if (File.Exists(nextS))
                                    {
                                        imgPath = nextS;
                                    }
                                    break;
                                case "-color":
                                    //stateColor = System.Drawing.Color.FromName(nextS);
                                    //var mcolor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.
                                    try
                                    {
                                        stateColor = ColorTranslator.FromHtml(nextS);
                                        setColor(stateColor);
                                    }
                                    catch (Exception cx)
                                    {

                                    }
                                    break;
                                case "-state":
                                    state = nextS;
                                    processState(state);
                                    break;
                                case "-action":

                                case "-actionname":
                                    if(actions.Count > 0)
                                    {
                                        if(actions[actions.Count - 1][0] != null && actions[actions.Count - 1][1] == null)
                                        {
                                            actions[actions.Count - 1][0] = nextS;
                                        }
                                    }
                                    else
                                    {
                                        string[] newAction = new string[3];
                                        newAction[0] = nextS;
                                        actions.Add(newAction);
                                    }
                                    break;

                                case "-actionparams":
                                    if (!string.IsNullOrEmpty(nextS))
                                    {
                                        if(actions != null && actions.Count > 0 && actions[actions.Count - 1][2] == null)
                                        {
                                            actions[actions.Count - 1][2] = nextS;
                                        }
                                        break;
                                    }
                                    break;
                                case "-actionpath":
                                    if (!File.Exists(nextS))
                                    {
                                        break;
                                    }
                                    if (actions.Count > 0)
                                    {
                                        if (actions[actions.Count - 1][0] != null && actions[actions.Count - 1][1] == null)
                                        {
                                            actions[actions.Count - 1][1] = nextS;
                                        }
                                    }
                                    else
                                    {
                                        string[] newAction = new string[3];
                                        newAction[0] = nextS;
                                        actions.Add(newAction);
                                    }
                                    break;
                            }
                            i += 1;// only has to count up one more
                        }
                    }
                }

                //if (args[args.Length - 2].ToLower().Equals("-image") && File.Exists(args[args.Length - 1]))
                //{
                //    notificationImage.Source = new BitmapImage(new Uri(args[args.Length - 1]));
                //    imgPath = args[args.Length - 1];
                //}
                if(String.IsNullOrWhiteSpace(imgPath) && File.Exists(@"Y:\Users\Admin\Pictures\lion.jpg"))
                {
                    imgPath = @"Y:\Users\Admin\Pictures\lion.jpg";
                }
            }
            else if(File.Exists(@"Y:\Users\Admin\Pictures\lion.jpg"))
            {
                imgPath = @"Y:\Users\Admin\Pictures\lion.jpg";
            }
            string screens = "";
            List<double> screenX = new List<double>();
            List<double> screensWidth = new List<double>();
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                screens += screen.WorkingArea.ToString() + "\r\n";
                uint x, y;
                screen.GetDpi(DpiType.Angular, out x, out y);
                screens += x + " " + y + "\r\n";
                screen.GetDpi(DpiType.Effective, out x, out y);
                double dpi = x;
                double thisScreenX = (screen.WorkingArea.Width / 2);// - (this.Width / 2);
                double thisWidth = screen.WorkingArea.Width;
                if (dpi != 96)
                {
                    //defaultDpi = dpi;
                    thisScreenX = thisScreenX / dpi * 96;
                    //thisWidth = thisWidth / dpi * 96;
                }
                //thisScreenX = thisScreenX / defaultDpi * 96;
                //thisWidth = thisWidth / defaultDpi * 96;
                //thisScreenX = thisScreenX - (this.Width / 2);
                thisScreenX = thisScreenX - (this.Width / 2);
                if (screenX.Count == 0)
                {
                    screenX.Add(thisScreenX);
                    screensWidth.Add(thisWidth);
                }
                else
                {
                    double last = screensWidth.Last();
                    //screenX.Add(thisScreenX + screensWidth.Last());
                    double screenSum = 0;
                    screensWidth.ForEach(a => screenSum += a);

                    screenX.Add(thisScreenX + screenSum);
                    screensWidth.Add(thisWidth);
                }
                screens += x + " " + y + "\r\n";
                screen.GetDpi(DpiType.Raw, out x, out y);
                screens += x + " " + y + "\r\n";
                //screen.
                
            }

            if (args.Length > 1)
            {
                //screenX.Reverse();
                //screenX[0] = 0;
                //screenX[1] = 2562;
                //screenX[2] = 5000;
                //screenX = new List<double>();
                //screenX.Add(5000);
                //screenX.Add(4500);
                foreach (var s in screenX)
                {
                    //Thread thread = new Thread(() =>{
                        ToastWindow tw = new ToastWindow(this, args[1], windowLeft: s, imgPath: imgPath, additionalMessage: mainMessage, state: state, actions: actions);
                        toastWidnows.Add(tw);
                        tw.setColor(stateColor);
                        //tw.Left = System.Windows.SystemParameters.VirtualScreenWidth / 2;
                        //tw.Left = 2560 + 1920 + (1920 / 2);
                        //tw.Left = s;
                        //tw.Show();

                        

                        //System.Windows.Threading.Dispatcher.Run();

                    //});
                    //thread.SetApartmentState(ApartmentState.STA);
                    //thread.Start();
                }
                foreach(ToastWindow tw in toastWidnows)
                {
                    tw.Show();
                }
            }
            //MessageBox.Show(screens);

            //MoveIn();
            //MessageBox.Show(System.Windows.SystemParameters.VirtualScreenWidth.ToString());
            //this.Left = System.Windows.SystemParameters.VirtualScreenWidth / 2;
            //VirtualScreenWidth


            //MainWindow mw2 = new MainWindow();
            //mw2.Show();
        }

        private void processState(string state)
        {
            switch (state.ToLower())
            {
                case "error":
                    stateColor = ColorTranslator.FromHtml("#F44336");
                    break;
            }
        }

        private bool setColor(Dcolor color)
        {
            Mcolor mcolor = Mcolor.FromArgb(color.A, color.R, color.G, color.B);
            var mbrush = new SolidColorBrush(mcolor);
            border1.Background = mbrush;
            border1.BorderBrush = mbrush;
            return true;
        }

        public async Task<int> gett()
        {
            //var mediaCapture = new Windows.Media.Capture.MediaCapture();
            //await mediaCapture.InitializeAsync();

            //UserNotificationListener listener = UserNotificationListener.Current;
            //UserNotificationListenerAccessStatus accessStatus = listener.RequestAccessAsync();

            int i = 1;
            return i;
        }

        public async Task<int> getAccess()
        {
            //// You need to add a reference to System.Net.Http to declare client.
            //System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            //// GetStringAsync returns a Task<string>. That means that when you await the 
            //// task you'll get a string (urlContents).
            //Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            //string urlContents = await getStringTask;

            //UserNotificationListener listener = UserNotificationListener.Current;
            //UserNotificationListenerAccessStatus accessStatus = await listener.RequestAccessAsync();
            ////await listener.RequestAccessAsync();

            int i = 1;
            return i;
        }

        public void MoveIn()
        {
            DoubleAnimation windowPositionAnimation = new DoubleAnimation();
            windowPositionAnimation.From = -100;
            windowPositionAnimation.To = 20;
            windowPositionAnimation.Duration = new Duration(TimeSpan.FromSeconds(movInSpeed));

            DoubleAnimation windowTransparancyAnimation = new DoubleAnimation();
            windowTransparancyAnimation.From = 0;
            windowTransparancyAnimation.To = windowOpacity;
            windowTransparancyAnimation.Duration = new Duration(TimeSpan.FromSeconds(movInSpeed));

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(windowPositionAnimation);
            //Storyboard.SetTargetName(myDoubleAnimation, "Window");
            //Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Window.TopProperty));

            // Use the Loaded event to start the Storyboard.
            //myRectangle.Loaded += new RoutedEventHandler(myRectangleLoaded);
            //myPanel.Children.Add(myRectangle);
            //this.Content = myPanel;
            //myStoryboard.Begin(this);
            this.BeginAnimation(Window.TopProperty, windowPositionAnimation);
            this.BeginAnimation(Window.OpacityProperty, windowTransparancyAnimation);
        }

        public void MoveOut()
        {
            DoubleAnimation windowPositionAnimation = new DoubleAnimation();
            windowPositionAnimation.From = this.Top;
            windowPositionAnimation.To = -100;
            windowPositionAnimation.Duration = new Duration(TimeSpan.FromSeconds(moveOutSpeed));
            windowPositionAnimation.Completed += (o, i) =>
            {
                this.Close();
                App.Current.Shutdown();
            };

            DoubleAnimation windowTransparancyAnimation = new DoubleAnimation();
            windowTransparancyAnimation.From = windowOpacity;
            windowTransparancyAnimation.To = 0;
            windowTransparancyAnimation.Duration = new Duration(TimeSpan.FromSeconds(moveOutSpeed));

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(windowPositionAnimation);
            //Storyboard.SetTargetName(myDoubleAnimation, "Window");
            //Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Window.TopProperty));

            // Use the Loaded event to start the Storyboard.
            //myRectangle.Loaded += new RoutedEventHandler(myRectangleLoaded);
            //myPanel.Children.Add(myRectangle);
            //this.Content = myPanel;
            //myStoryboard.Begin(this);
            this.BeginAnimation(Window.TopProperty, windowPositionAnimation);
            this.BeginAnimation(Window.OpacityProperty, windowTransparancyAnimation);
        }

        public void childClosing(string execute = null, string executeParams = null)
        {
            if (this.toastsClosing) { return; }
            this.toastsClosing = true;
            foreach(ToastWindow tw in toastWidnows)
            {
                tw.MoveOut();
            }
            if(execute != null && executeParams == null && File.Exists(execute))
            {
                Process.Start(execute);
            }
            if (execute != null && executeParams != null && File.Exists(execute))
            {
                Process.Start(execute, executeParams);
            }
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            textAdditional.Content = this.Left;
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MoveOut();
        }
    }

    public static class ScreenExtensions
    {
        public static void GetDpi(this System.Windows.Forms.Screen screen, DpiType dpiType, out uint dpiX, out uint dpiY)
        {
            var pnt = new System.Drawing.Point(screen.Bounds.Left + 1, screen.Bounds.Top + 1);
            var mon = MonitorFromPoint(pnt, 2/*MONITOR_DEFAULTTONEAREST*/);
            GetDpiForMonitor(mon, dpiType, out dpiX, out dpiY);
        }

        //https://msdn.microsoft.com/en-us/library/windows/desktop/dd145062(v=vs.85).aspx
        [DllImport("User32.dll")]
        private static extern IntPtr MonitorFromPoint([In]System.Drawing.Point pt, [In]uint dwFlags);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510(v=vs.85).aspx
        [DllImport("Shcore.dll")]
        private static extern IntPtr GetDpiForMonitor([In]IntPtr hmonitor, [In]DpiType dpiType, [Out]out uint dpiX, [Out]out uint dpiY);
    }

    //https://msdn.microsoft.com/en-us/library/windows/desktop/dn280511(v=vs.85).aspx
    public enum DpiType
    {
        Effective = 0,
        Angular = 1,
        Raw = 2,
    }
}
