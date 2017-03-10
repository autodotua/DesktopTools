using SHDocVw;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;


namespace 桌面小工具集
{
    /// <summary>
    /// WebView.xaml 的交互逻辑
    /// </summary>
    public partial class WebView : Window
    {
        public WebView()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);

        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        IWebBrowser2 axIWebBrowser2;

        public void refreshWebview()
        {
            wb.Navigate(Resource.WebView_URL);
        }

        private void window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //opened = true;
            IntPtr pWnd = FindWindow("Progman", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SysListView32", null);
            //IntPtr tWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            SetParent(new WindowInteropHelper(this).Handle, pWnd);

            wb.Navigate(Resource.WebView_URL);
            timer.Interval = new TimeSpan(10000000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        string lastURL = Resource.WebView_URL;
        int lastZoom = Resource.WebView_Zoom;
        private void timer_Tick(object sender, EventArgs e)
        {
            if (Resource.WebView_Show == 1)
            {
                this.Visibility = Visibility.Visible;
            }
            else
            {
                this.Visibility = Visibility.Hidden;
                return;
            }

            if (lastZoom != Resource.WebView_Zoom)
            {
                try
                {
                    axIWebBrowser2 = (IWebBrowser2)this.wb.ActiveXInstance;　//关键代码
                    Zoom(axIWebBrowser2, Resource.WebView_Zoom);

                }
                catch (Exception)
                { }
            }

            if (Resource.WebView_URL != lastURL)
            {
                try
                {
                    wb.Navigate(Resource.WebView_URL);
                    lastURL = Resource.WebView_URL;
                }
                catch (Exception)
                { }
            }
        }


        public void Zoom(IWebBrowser2 axIWebBrowser2, int factor)
        {
            object pvaIn = factor;
        

                axIWebBrowser2.ExecWB(OLECMDID.OLECMDID_OPTICAL_ZOOM,
                   OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER,
                   ref pvaIn, IntPtr.Zero);
            }
     
        
        //public static void sleep(int times)//延时，单位毫秒
        //{
        //    int start = Environment.TickCount;
        //    while (Math.Abs(Environment.TickCount - start) < times)
        //    {
        //       // Application.DoEvents();
        //        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,new Action(delegate { }));
        //    }
        //}

        System.Windows.Threading.DispatcherTimer waitTimer = new System.Windows.Threading.DispatcherTimer();
        private void sleep(int millisecond)
        {
            waitTimer.Interval = new TimeSpan(10000 * millisecond);
            waitTimer.Tick += new EventHandler(waitTimer_Tick);
            waitTimer.Start();

        }

        private void waitTimer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("TimeTickStop");
            waitTimer.IsEnabled = false;
            return;
        }

        private void wb_Navigated(object sender, System.Windows.Forms.WebBrowserNavigatedEventArgs e)
        {

            //if (wb.Document.Url.ToString().StartsWith("res://"))
            //{
            //    lastURL = "";
            //    Debug.WriteLine("failed");
            //}

           //MessageBox.Show(wb.Document.Body.InnerText);

            if (wb.Document.Title.Contains("取消") || wb.Document.Title.Contains("无法") || wb.Document.Url.ToString().Contains("res"))
            {
               sleep(10000);
             Debug.WriteLine("Refresh");
                wb.Navigate(Resource.WebView_URL);
                
            }

            axIWebBrowser2 = (IWebBrowser2)this.wb.ActiveXInstance;　//关键代码
            try
            {
                
            Zoom(axIWebBrowser2, Resource.WebView_Zoom);}
catch(Exception)
{
}
        }

        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
            Resource.WebView_Location = this.Left + "|" + this.Top;
        }
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();



    }
}
