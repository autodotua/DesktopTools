using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 桌面小工具集
{
    /// <summary>
    /// clock.xaml 的交互逻辑
    /// </summary>
    public partial class Clock : Window
    {
        public Clock()
        {
            InitializeComponent();
        }
        List<System.Windows.Shapes.Line> lN = new List<Line> { new Line(), new Line(), new Line() };

        List<System.Windows.Shapes.Line> lD = new List<Line>();
        List<System.Windows.Shapes.Line> lD2 = new List<Line>();
        Ellipse[] elps = { null, null };
        private void drawDialLine(SolidColorBrush brush, double tn, double x1, double y1, double x2, double y2, int i)
        {
            System.Windows.Shapes.Line l = new Line();
            lD[i].Stroke = brush;
            lD[i].StrokeThickness = tn;
            lD[i].X1 = x1 * cv.ActualWidth / 2 + cv.ActualWidth / 2;
            lD[i].X2 = x2 * cv.ActualWidth / 2 + cv.ActualWidth / 2;
            lD[i].Y1 = cv.ActualHeight / 2 - y1 * cv.ActualHeight / 2;
            lD[i].Y2 = cv.ActualHeight / 2 - y2 * cv.ActualHeight / 2;
            if (!cv.Children.Contains(lD[i]))
            {
                cv.Children.Add(lD[i]);
            }
        }

        private void drawNeedleLine(SolidColorBrush brush, double tn, double x1, double y1, double x2, double y2, int i)
        {
            lN[i].Stroke = brush;
            lN[i].StrokeThickness = tn;
            lN[i].X1 = x1 * cv.ActualWidth / 2 + cv.ActualWidth / 2;
            lN[i].X2 = x2 * cv.ActualWidth / 2 + cv.ActualWidth / 2;
            lN[i].Y1 = cv.ActualHeight / 2 - y1 * cv.ActualHeight / 2;
            lN[i].Y2 = cv.ActualHeight / 2 - y2 * cv.ActualHeight / 2;
            if (!cv.Children.Contains(lN[i]))
            {
                cv.Children.Add(lN[i]);
            }
        }
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);

        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);





        private void window_Loaded_1(object sender, RoutedEventArgs e)
        {
            // opened = true;
            IntPtr pWnd = FindWindow("Progman", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SysListView32", null);
            //IntPtr tWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            SetParent(new WindowInteropHelper(this).Handle, pWnd);

            for (int i = 1; i <= 16; i++)
            {
                lD.Add(new Line());

            }
            drawDial();
            timer.Interval = new TimeSpan(100000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

        }
        private void timer_Tick(object sender, EventArgs e)
        {

            if (Resource.Clock_Show == 1)
            {
                this.Visibility = Visibility.Visible;
            }
            else
            {
                this.Visibility = Visibility.Hidden;
                return;
            }

            DateTime d = DateTime.UtcNow;
            double h = d.Hour + d.Minute / 60d + Resource.ExtraHours;
            while (h >= 12)
            {
                h -= 12;
            }
            double m = d.Minute + d.Second / 60d;
            double s = d.Second + d.Millisecond / 1000d;
            drawNeedle(h, m, s);

            //if (this.Width != Resource.ClockSize)
            //{
            //    this.Width = Resource.ClockSize;
            //    this.Height = Resource.ClockSize;
            //}
        }

        private void drawDial()
        {
            int count = 0;
            for (double i = 0; i < 2 * Math.PI; i += Math.PI / 6)//general time
            {
                drawDialLine(
                    new SolidColorBrush(Colors.White),
                    0.005 * cv.ActualHeight,
                    0.99 * Math.Sin(i),
                    0.99 * Math.Cos(i),
                    0.85 * Math.Sin(i),
                    0.85 * Math.Cos(i), count++);
            }
            for (double i = 0; i < 2 * Math.PI; i += Math.PI / 2)//0369 o'clock
            {
                drawDialLine(
                    new SolidColorBrush(Colors.White),
                    0.008 * cv.ActualHeight,
                    0.99 * Math.Sin(i),
                    0.99 * Math.Cos(i),
                    0.70 * Math.Sin(i),
                    0.70 * Math.Cos(i),
                count++);


            }
            if (elps[0] == null)
                elps[0] = new Ellipse();
            elps[0].Stroke = new SolidColorBrush(Colors.White);
            elps[0].StrokeThickness = 0.04 * cv.ActualHeight;
            elps[0].Margin = new Thickness(0.48 * cv.ActualHeight);//new Thickness(0.5 * this.ActualWidth, 0.5 * this.ActualHeight, 0.5 * this.ActualWidth, 0.5 * this.ActualHeight);

            if (elps[1] == null)
                elps[1] = new Ellipse();
            elps[1].Stroke = new SolidColorBrush(Colors.White);
            elps[1].StrokeThickness = 0.01 * cv.ActualHeight;
            elps[1].Width = this.ActualWidth;
            elps[1].Height = this.ActualWidth;
            // elps[1].Margin = new Thickness(0);//new Thickness(0.5 * this.ActualWidth, 0.5 * this.ActualHeight, 0.5 * this.ActualWidth, 0.5 * this.ActualHeight);
            cv.Children.Add(elps[0]);
            cv.Children.Add(elps[1]);
        }

        private void drawNeedle(double h, double m, double s)
        {
            if (Resource.Clock_ShowSecond == 1)
            {
                drawNeedleLine(new SolidColorBrush(Colors.White),
    0.005 * cv.ActualHeight,
    0, 0,
    Math.Sin(s / 30 * Math.PI), Math.Cos(s / 30 * Math.PI)
    , 0);
            }
            else
            {
                drawNeedleLine(new SolidColorBrush(Colors.White),
    0.005 * cv.ActualHeight,
    0, 0,
    0, 0
    , 0);
            }
            drawNeedleLine(new SolidColorBrush(Colors.White),
                0.008 * cv.ActualHeight,
                 0, 0,
             0.7 * Math.Sin(m / 30 * Math.PI), 0.7 * Math.Cos(m / 30 * Math.PI)
             , 1);
            drawNeedleLine(new SolidColorBrush(Colors.White),
              0.012 * cv.ActualHeight,
              0, 0,
    0.4 * Math.Sin(h / 6 * Math.PI), 0.4 * Math.Cos(h / 6 * Math.PI),
    2);
        }


        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                drawDial();
            }
            catch (Exception)
            {
            }
        }


        private void cv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
            Resource.Clock_Location = this.Left.ToString() + "|" + this.Top.ToString();
        }
        // bool opened = false;
        private void window_LocationChanged(object sender, EventArgs e)
        {


        }
    }
}
