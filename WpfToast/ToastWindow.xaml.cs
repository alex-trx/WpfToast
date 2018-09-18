using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Dcolor = System.Drawing.Color;
using Mcolor = System.Windows.Media.Color;

namespace WpfToast
{
    /// <summary>
    /// Interaction logic for ToastWindow.xaml
    /// </summary>
    public partial class ToastWindow : Window
    {
        private Storyboard myStoryboard;
        private double windowOpacity = 1;
        private double movInSpeed = 0.5;
        private double moveOutSpeed = 0.3;
        private MainWindow parentWindow;
        private string mainMessage;
        private bool movingOut = false;
        private string imgPath = "";
        private Mcolor stateColor;
        private Mcolor accentColor;
        private SolidColorBrush stateColorBrush;
        private SolidColorBrush accentColorBrush;
        private string state = "";
        private double windowLeft = 0;

        public ToastWindow(MainWindow parentWindow, string mainMessage, string additionalMessage = "", double? windowTop = null, double? windowLeft = null, string imgPath = "", string state = "")
        {
            //this.Left = windowLeft.GetValueOrDefault();
            this.Left = 4600;
            this.Top = 100;
            InitializeComponent();
            //this.Topmost = true;
            //this.Top = -100;
            //this.Left = 2560;
            this.parentWindow = parentWindow;
            this.mainMessage = mainMessage;
            mainText.Content = mainMessage;
            textAdditional.Content = additionalMessage;
            if(windowTop != null)
            {
                this.Top = windowTop.GetValueOrDefault();
            }
            if (windowLeft != null)
            {
                //this.Left = windowLeft.GetValueOrDefault();
                this.windowLeft = windowLeft.GetValueOrDefault();
            }
            this.imgPath = imgPath;
            if(imgPath != "")
            {
                notificationImage.Source = new BitmapImage(new Uri(imgPath));
            }
            this.state = state;
            //MoveIn();
            //this.Left = 4500;
            //this.windowLeft = 4500;
            this.Left = 2600;
            this.Left = 2600;
            this.Left = 2600;
            this.Left = 2600;
            this.Left = 2600;
        }

        public bool setColor(Dcolor color)
        {
            Mcolor mcolor = Mcolor.FromArgb(color.A, color.R, color.G, color.B);
            stateColor = mcolor;
            double x = 0.5;
            accentColor = Mcolor.FromArgb(color.A, Convert.ToByte((Convert.ToInt32(color.R) * x)),
                Convert.ToByte((Convert.ToInt32(color.G) * x)), Convert.ToByte((Convert.ToInt32(color.B) * x)));
            stateColorBrush = new SolidColorBrush(mcolor);
            accentColorBrush = new SolidColorBrush(accentColor);
            border1.Background = stateColorBrush;
            border1.BorderBrush = stateColorBrush;
            if (state.ToLower().Equals("error"))
            {
                Blink();
            }
            return true;
        }

        public void Blink()
        {
            var storyboard = new Storyboard
            {
                Duration = TimeSpan.FromSeconds(2),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            
            ColorAnimation statusBlinkAnimation = new ColorAnimation();
            //statusBlinkAnimation.From = accentColor;
            //statusBlinkAnimation.From = Mcolor.FromRgb(255,255,255);
            statusBlinkAnimation.To = Mcolor.FromRgb(63, 81, 181);
            //statusBlinkAnimation.To = Mcolor.FromRgb(255,255,255);
            //statusBlinkAnimation.To = stateColor;
            statusBlinkAnimation.From = stateColor;
            statusBlinkAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            statusBlinkAnimation.AutoReverse = true;
            //statusBlinkAnimation.RepeatBehavior = RepeatBehavior.

            this.RegisterName("animatedBrush", stateColorBrush);
            Storyboard.SetTargetName(statusBlinkAnimation, "animatedBrush");
            Storyboard.SetTargetProperty(statusBlinkAnimation, new PropertyPath(SolidColorBrush.ColorProperty));


            storyboard.Children.Add(statusBlinkAnimation);
            storyboard.Begin(this);
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
            if (movingOut) return;
            movingOut = true;
            parentWindow.childClosing();
            DoubleAnimation windowPositionAnimation = new DoubleAnimation();
            windowPositionAnimation.From = this.Top;
            windowPositionAnimation.To = -100;
            windowPositionAnimation.Duration = new Duration(TimeSpan.FromSeconds(moveOutSpeed));
            windowPositionAnimation.Completed += (o, i) =>
            {
                //parentWindow.Close();
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

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MoveOut();
            //MessageBox.Show(this.Left.ToString());
            //this.Left = this.windowLeft;
            //Window parentWindow = Window.GetWindow(this);
            //parentWindow.Close();
        }
        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            textAdditional.Content = this.Left;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(this.Left.ToString());
            //this.Left = this.windowLeft;
            this.Left = 2565;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //this.Activate();
            //MessageBox.Show(this.mainMessage + " " + this.Left.ToString());

            this.Left = this.windowLeft;
            this.Top = -100;
            this.Visibility = Visibility.Visible;
            this.Show();
            //this.Left = this.windowLeft;
            //System.Threading.Thread.Sleep(5000);
            //this.Top = 100;
            //this.Left = 4000;
            if (this.mainMessage.StartsWith("2"))
            {
                //this.Left = 2562;
                //this.Top = 300;
            }
           if (this.mainMessage.StartsWith("3"))
            {
                //this.Top = 600;
                //this.Left = 5702;
                //MessageBox.Show(this.mainMessage + " " + this.Left.ToString());
                //this.Left = this.windowLeft;
                //this.Left = 2000;
                //this.Top = 100;
            }
            MoveIn();
        }
    }
}
