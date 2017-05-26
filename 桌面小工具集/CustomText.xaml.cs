using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace 桌面小工具集
{
    /// <summary>
    /// CountDown.xaml 的交互逻辑
    /// </summary>
    public partial class CustomText : Window
    {
        public CustomText()
        {
            InitializeComponent();
        }

        private void Window_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {

        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow([MarshalAs(UnmanagedType.LPTStr)] string lpClassName, [MarshalAs(UnmanagedType.LPTStr)] string lpWindowName);

        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        Dictionary<string, DateTime> setTime = new Dictionary<string, DateTime>();
        string textPart = "", raw = "";
        int digitOfDicimal = 0;
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            IntPtr pWnd = FindWindow("Progman", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SHELLDLL_DefVIew", null);
            pWnd = FindWindowEx(pWnd, IntPtr.Zero, "SysListView32", null);
            //IntPtr tWnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

            SetParent(new WindowInteropHelper(this).Handle, pWnd);
            //this.AllowsTransparency = false;
            // this.Foreground = Brushes.White;


            initializeText();


            timer.Interval = new TimeSpan(10000000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

        }
        public void initializeText()
        {
            setTime.Clear();

            string temp = Resource.CustomText_Text;
            Debug.WriteLine(temp);
            raw = temp;
            // t.Text=temp;
            if (temp == null)
            {
                return;
            }
            string[] split1 = temp.Split(new string[] { "\r\n#" }, StringSplitOptions.RemoveEmptyEntries);
            if (split1 == null)
            {
                return;
            }
            textPart = "";
            foreach (var item in split1)
            {
                if (item[item.Length - 1] != '#')//如果是普通文本
                {
                    textPart += item + "\r\n";
                }
                else//如果是倒计时的设置项
                {
                    //分割
                    string[] split2 = item.Remove(item.Length - 1)/*删除最后一个井号*/.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (split2.Length == 4)
                    {
                        setTime.Add(split2[0], new DateTime(int.Parse(split2[1]), int.Parse(split2[2]), int.Parse(split2[3])));
                        
                    }
                    else if (split2.Length == 5)
                    {
                        setTime.Add(split2[0], new DateTime(int.Parse(split2[1]), int.Parse(split2[2]), int.Parse(split2[3])));
                        digitOfDicimal = int.Parse(split2[4]);
                    }
                    else
                        if (split2.Length == 7)
                        {
                            setTime.Add(split2[0], new DateTime(int.Parse(split2[1]), int.Parse(split2[2]), int.Parse(split2[3]), int.Parse(split2[4]), int.Parse(split2[5]), int.Parse(split2[6])));
                            
                        }
                    else
                         if (split2.Length == 8)
                    {
                        setTime.Add(split2[0], new DateTime(int.Parse(split2[1]), int.Parse(split2[2]), int.Parse(split2[3]), int.Parse(split2[4]), int.Parse(split2[5]), int.Parse(split2[6])));
                        digitOfDicimal = int.Parse(split2[7]);
                    }
                }
            }
        }

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        private void timer_Tick(object sender, EventArgs e)
        {
            if (Resource.CustomText_Show == 1)
            {
                this.Visibility = Visibility.Visible;
            }
            else
            {
                this.Visibility = Visibility.Hidden;
                return;
            }
            Dictionary<string, TimeSpan> countDown = new Dictionary<string, TimeSpan>();
            DateTime now = DateTime.UtcNow.Add(new TimeSpan(Resource.ExtraHours, 0, 0));
            foreach (var item in setTime)
            {
                //Debug.WriteLine(item.ToString());
                countDown.Add(item.Key, (item.Value - now).Duration());

            }
            if (raw != Resource.CustomText_Text)
            {
                //  Debug.WriteLine("Yes");
                initializeText();
            }
            string target = textPart;
            // Debug.WriteLine(textPart);
            Regex r = new Regex(@"{\w+\.\w+}");
            int n = r.Matches(target).Count;
            for (int i = 0; i < n; i++)
            {
                // Debug.WriteLine(r.Matches(target)[i]);
                //  Debug.WriteLine(i);
                if (r.IsMatch(target) == false)
                {
                    break;
                }
                string name = r.Matches(target)[0].Value.Replace("}", "").Replace("{", "").Split('.')[0];
                string prop = r.Matches(target)[0].Value.Replace("}", "").Replace("{", "").Split('.')[1];
                foreach (var j in countDown)
                {
                    if (name == j.Key)
                    {
                        //Debug.WriteLine(r.Match(target).Captures[i].Value);
                        switch (prop)
                        {
                            case "Days":
                                target = target.Replace(r.Match(target).Captures[0].Value, j.Value.Days.ToString());
                                break;
                            case "TotalDays":
                                target = target.Replace(r.Match(target).Captures[0].Value, Math.Round(j.Value.TotalDays, digitOfDicimal).ToString());
                                break;
                            case "Hours":
                                target = target.Replace(r.Match(target).Captures[0].Value, j.Value.Hours.ToString());
                                break;
                            case "TotalHours":
                                target = target.Replace(r.Match(target).Captures[0].Value, Math.Round(j.Value.TotalHours,digitOfDicimal).ToString());
                                break;
                            case "Minutes":
                                target = target.Replace(r.Match(target).Captures[0].Value, j.Value.Minutes.ToString());
                                break;
                            case "TotalMinutes":
                                target = target.Replace(r.Match(target).Captures[0].Value, Math.Round(j.Value.TotalMinutes, digitOfDicimal).ToString());
                                break;
                            case "Seconds":
                                target = target.Replace(r.Match(target).Captures[0].Value, j.Value.Seconds.ToString());
                                break;
                            case "TotalSeconds":
                                target = target.Replace(r.Match(target).Captures[0].Value, Math.Round(j.Value.TotalSeconds, digitOfDicimal).ToString());
                                break;

                        }


                    }
                }

            }
            target = target.Replace("{DayOfMonth}", now.Day.ToString())
.Replace("{DayOfWeek}", now.DayOfWeek.ToString())
.Replace("{DayOfYear}", now.DayOfYear.ToString())
.Replace("{Month}", now.Month.ToString())
.Replace("{Year}", now.Year.ToString())
.Replace("{Date}", now.Date.ToString().Replace(" 0:00:00", ""))
.Replace("{Hour}", now.Hour.ToString())//.Length==1?"0"+now.Hour.ToString():now.Hour.ToString())
.Replace("{Minute}", now.Minute.ToString().Length == 1 ? "0" + now.Minute.ToString() : now.Minute.ToString())
.Replace("{Second}", now.Second.ToString().Length == 1 ? "0" + now.Second.ToString() : now.Second.ToString());

            t.Text = target;


            //       setTime = new DateTime(Resource.CountDown_Time);
            //       //int y = dt.Year;
            //       //int mo = dt.Month;
            //       //int d = dt.Day;
            //       //int h = dt.Hour;
            //       //int mi = dt.Minute;
            //       //int s = dt.Second;
            //       if (Resource.CountDown_ShowCountDown == 1)
            //       {
            //           this.Visibility = Visibility.Visible;
            //       }
            //       else
            //       {
            //           this.Visibility = Visibility.Hidden;
            //       }
            //       DateTime now = DateTime.UtcNow.Add(new TimeSpan(Resource.ExtraHours, 0, 0));

            //       TimeSpan d = setTime - now;
            //     //  DateTime d=new DateTime((setTime-now).Ticks);
            //       string temp = "";
            //       temp =//d.
            //           // d.Year.ToString()+"年"+
            //           // d.Month.ToString()+"月"+
            //d.Days.ToString() + "天";


            //           if (Resource.CountDown_OnlyShowDays!= 1)
            //           {
            //               temp+=//d.
            //                   // d.Year.ToString()+"年"+
            //                   // d.Month.ToString()+"月"+
            //d.Hours.ToString() + "小时" +
            //d.Minutes.ToString() + "分钟";
            //           }
            //           if(Resource.CountDown_HideSecond!=1 && Resource.CountDown_OnlyShowDays!=1)
            //           {
            //              temp+=//d.
            //                   // d.Year.ToString()+"年"+
            //                   // d.Month.ToString()+"月"+
            //          d.Seconds.ToString() + "秒";
            //           }

            //       t.Text = Resource.CountDown_UD.Replace("%t", temp);
            //       if (this.Width != Resource.ClockSize)
            //       {
            //           this.Width = Resource.ClockSize;
            //       }
            //   }
        }
        private void Viewbox_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
            Resource.CustomText_Location = this.Left + "|" + this.Top;
        }

    }
}

