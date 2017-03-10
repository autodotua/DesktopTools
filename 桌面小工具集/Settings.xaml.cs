using Microsoft.Win32;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using IWshRuntimeLibrary;
using System.IO;

namespace 桌面小工具集
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string tempFileName = System.IO.Path.GetTempFileName();
        //设置图表
        public MainWindow()
        {
            //this.Icon=new BitmapImage(new Uri(
            InitializeComponent();
            FileStream fs = new FileStream(tempFileName, FileMode.Create);
            Properties.Resources.icon.Save(fs);
            fs.Close();
            this.Icon = new BitmapImage(new Uri(tempFileName));
        }

        private System.Windows.Forms.NotifyIcon notifyIcon = null;
        Window w_c;
        Window w_ct;
        Window w_wv;
        //设置托盘图标
        private void InitialTray()
        {

            //设置托盘的各个属性
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = "设置界面在托盘";
            notifyIcon.Text = "桌面小工具集";

            // System.IO.File.WriteAllBytes("D:\hello.ico",  Encoding.Default.GetBytes(Encoding.Default.GetString(Properties.Resources.icon.ToString()));


            //notifyIcon.Icon = 
            //try
            //{
            notifyIcon.Icon = new System.Drawing.Icon(tempFileName);
            //}
            //catch
            //{
            //    System.Windows.Forms.MessageBox.Show("找不到图标文件！");
            //}
            notifyIcon.Visible = true;
            if (Resource.HaveOpened != "1")
            {

                notifyIcon.ShowBalloonTip(2000);
            }
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

            //设置菜单项
            //System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem("菜单", new System.Windows.Forms.MenuItem[] { menu1 , menu2 });

            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(exit_Click);

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //窗体状态改变时候触发
            //this.StateChanged += new EventHandler(SysTray_StateChanged);

        }
        //设置托盘图标鼠标动作
        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    this.Activate();
                }
            }
        }
        //设置托盘图标鼠标右键退出
        private void exit_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            setAllConfig();
            Application.Current.Shutdown();
        }
        //获取配置、更改内核版本、打开各个窗口、应用配置到设置控件
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Resource.getAllConfig();//获取所有配置项
            //更改IE内核版本
            try
            {
                RegistryKey rk = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION");
                //rk.SetValue(new FileInfo(Process.GetCurrentProcess().MainModule.FileName).Name, "11001");
                rk.SetValue(new FileInfo(Process.GetCurrentProcess().MainModule.FileName).Name, "11001", RegistryValueKind.DWord);
            }
            catch (Exception)
            {
                if (Resource.HaveOpened != "1")
                {
                    MessageBox.Show(@"请注意：
若您要使用webview功能，请以管理员权限运行一次本软件，
否则，网页显示可能会有兼容性问题。
更名或改变位置后需要重新管理员运行。
本提示仅出现一次。
仅适用于64位Windows10及以上操作系统。
", "注意");
                }
            }




            //图标问题
            //System.IO.File.WriteAllBytes("",Properties.Resources.icon));
            //this.Icon = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\desktop_icon.ico"));
            //this.Icon = new BitmapImage(new Uri("icon.ico"));
            //opened = true;
            w_c = new Clock();
            w_ct = new CustomText();
            w_wv = new WebView();
            w_c.Show();
            w_ct.Show();
            w_wv.Show();
            if (Resource.Clock_Show == 0)
            {
                clockShow.IsChecked = false;
                w_c.Hide();
            }
            if (Resource.CustomText_Show == 0)
            {
                customTextShow.IsChecked = false;
                w_ct.Hide();
            }
            if (Resource.WebView_Show == 0)
            {

                webviewShow.IsChecked = false;
                w_wv.Hide();
            }


            clock_ssecond.IsChecked = (Resource.Clock_ShowSecond == 1 ? true : false);
            clockSize.Value = Resource.Clock_Size;

            eh.Text = Resource.ExtraHours.ToString();
            ud.Text = Resource.CustomText_Text;

            customTextSize.Value = Resource.CustomText_Size;
            webviewURL.Text = Resource.WebView_URL;
            webviewZoom.Value = Resource.WebView_Zoom;
            webviewHeight.Value = Resource.WebView_Height;
            webviewWidth.Value = Resource.WebView_Width;


            w_c.Left = double.Parse(Resource.Clock_Location.Split('|')[0]);
            w_c.Top = double.Parse(Resource.Clock_Location.Split('|')[1]);
            w_ct.Left = double.Parse(Resource.CustomText_Location.Split('|')[0]);
            w_ct.Top = double.Parse(Resource.CustomText_Location.Split('|')[1]);
            w_wv.Left = double.Parse(Resource.WebView_Location.Split('|')[0]);
            w_wv.Top = double.Parse(Resource.WebView_Location.Split('|')[1]);


            string Path = Environment.GetFolderPath(Environment.SpecialFolder.Startup);// System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\新建文件夹 (3)"; //"%USERPROFILE%\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup";

           // System.IO.File.Delete(Path + "\\DesktopTools.lnk");
            if (System.IO.File.Exists(Path + "\\DesktopTools.lnk"))
            {
                startup.IsChecked = true;
            }

            if (Resource.HaveOpened == "1")
            {
                this.Visibility = Visibility.Hidden;
            }
            InitialTray();
        }
        //设置点右上角叉叉隐藏而非关闭窗口
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        //将所有的配置写到注册表
        private void setAllConfig()
        {

            setConfig("width", Resource.Clock_Size.ToString());

            setConfig("clock_show", Resource.Clock_Show.ToString());

            setConfig("clock_showSecond", Resource.Clock_ShowSecond.ToString());

            setConfig("haveOpened", "1");

            setConfig("extraHours", Resource.ExtraHours.ToString());

            setConfig("customText_show", Resource.CustomText_Show.ToString());

            setConfig("customText_text", Resource.CustomText_Text.Replace("\r\n", "#nextLine#"));

            setConfig("clock_location", Resource.Clock_Location);

            setConfig("customText_location", Resource.CustomText_Location);

            setConfig("customText_size", Resource.CustomText_Size.ToString());

            setConfig("webview_zoom", Resource.WebView_Zoom.ToString());

            setConfig("webview_height", Resource.WebView_Height.ToString());

            setConfig("webview_width", Resource.WebView_Width.ToString());

            setConfig("webview_location", Resource.WebView_Location);

            setConfig("webview_URL", Resource.WebView_URL);

            setConfig("webview_show", Resource.WebView_Show.ToString());
        }
        //写注册表的具体方法
        private static void setConfig(string key, string value)
        {
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("software\\fz\\desktop tools");
            try
            {
                rk.SetValue(key, value);
            }
            catch (Exception)
            {

            }
        }
        // 将所有配置保存到Resource类中
        private void saveAllConfig()
        {
            Resource.CustomText_Text = ud.Text;
            Resource.ExtraHours = int.Parse(eh.Text);
            Resource.Clock_Show = (clockShow.IsChecked == true ? 1 : 0);
            Resource.CustomText_Show = (customTextShow.IsChecked == true ? 1 : 0);
            Resource.Clock_Size = clockSize.Value;
            Resource.CustomText_Size = customTextSize.Value;
            Resource.Clock_ShowSecond = (clock_ssecond.IsChecked == true ? 1 : 0);
            Resource.WebView_Width = webviewWidth.Value;
            Resource.WebView_Height = webviewHeight.Value;
            Resource.WebView_Zoom = (int)webviewZoom.Value;
            Resource.WebView_URL = webviewURL.Text;
            Resource.WebView_Show = (webviewShow.IsChecked == true ? 1 : 0);
        }


        // 按下确定按钮
        private void saveAllConfig(object sender, RoutedEventArgs e)
        {
            saveAllConfig();
            setAllConfig();
            this.Hide();
        }
        // 按下应用按钮
        private void SaveWithoutClosing(object sender, RoutedEventArgs e)
        {
            saveAllConfig();
        }


        // 时钟的尺寸
        private void clockSize_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (w_c != null)
            {
                w_c.Width = ((Slider)sender).Value;
                w_c.Height = ((Slider)sender).Value;
            }
        }
        // 自定义文本的尺寸
        private void customTextSize_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (w_ct != null)
            {
                w_ct.Width = ((Slider)sender).Value;
                w_ct.Height = ((Slider)sender).Value;
            }
        }



        //网页的缩放
        private void webviewZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (w_wv != null)

                Resource.WebView_Zoom = (int)webviewZoom.Value;
        }
        //网页的尺寸
        private void webviewHeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (w_wv != null)
                w_wv.Height = webviewHeight.Value;
        }
        private void webviewWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (w_wv != null)
                w_wv.Width = webviewWidth.Value;
        }
        //网页的地址
        private void webviewURL_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                Resource.WebView_URL = webviewURL.Text;

            }

        }
            //开机自启设置
        private void startup_Click(object sender, RoutedEventArgs e)
        {
            if (startup.IsChecked == true)
            {
                string Path = Environment.GetFolderPath(Environment.SpecialFolder.Startup);// System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\新建文件夹 (3)"; //"%USERPROFILE%\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup";

                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshShortcut sc = (IWshShortcut)shell.CreateShortcut(Path + "\\DesktopTools.lnk");
                sc.TargetPath = Process.GetCurrentProcess().MainModule.FileName;
                sc.WorkingDirectory = Environment.CurrentDirectory;
                sc.Save();
                Debug.WriteLine(Path);
                if (System.IO.File.Exists(Path + "\\DesktopTools.lnk"))
                {
                    System.Windows.Forms.MessageBox.Show("Succeed");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Fail");
                }
            }
            else
            {
                string Path = Environment.GetFolderPath(Environment.SpecialFolder.Startup);// System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\新建文件夹 (3)"; //"%USERPROFILE%\\AppData\\Roaming\\Microsoft\\Windows\\Start Menu\\Programs\\Startup";

                System.IO.File.Delete(Path + "\\DesktopTools.lnk");
                if (System.IO.File.Exists(Path + "\\DesktopTools.lnk"))
                {
                    System.Windows.Forms.MessageBox.Show("Fail");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Succeed");
                }

            }
        }


    }
}