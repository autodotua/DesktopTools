using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace 桌面小工具集
{
    public static class Resource
    {
        public static string HaveOpened { get; set; }
        public static double Clock_Size { get; set; }
        public static double CustomText_Size { get; set; }
        public static int Clock_Show { get; set; }
        public static int Clock_ShowSecond { get; set; }
        public static int ExtraHours { get; set; }
        public static int CustomText_Show { get; set; }
        public static string CustomText_Text { get; set; }
        public static string Clock_Location { get; set; }
        public static string CustomText_Location { get; set; }
        public static string WebView_Location { get; set; }
        public static int WebView_Zoom { get; set; }
        public static double WebView_Height { get; set; }
        public static double WebView_Width { get; set; }
        public static string WebView_URL { get; set; }
        public static int WebView_Show { get; set; }




        public static void getAllConfig()
        {
            HaveOpened = (getConfig("haveOpened") == null ? "0" : getConfig("haveOpened"));
            Clock_Size = (getConfig("width") == null ? 300d : double.Parse(getConfig("width")));
            Clock_Show = (getConfig("clock_show") == null ? 1 : int.Parse(getConfig("clock_show")));
            Clock_ShowSecond = (getConfig("clock_showSecond") == null ? 1 : int.Parse(getConfig("clock_showSecond")));
            ExtraHours = (getConfig("clock_extraHours") == null ? 8 : int.Parse(getConfig("clock_extraHours")));
            CustomText_Show = (getConfig("customText_show") == null ? 1 : int.Parse(getConfig("customText_show")));
            Clock_Location = (getConfig("clock_location") == null ? "50|50" : getConfig("clock_location"));
            CustomText_Text = (getConfig("customText_text") == null ? @"今天是{Year}年{Month}月{DayOfMonth}日
现在是{Hour}:{Minute}:{Second}
距离2048年还有{a.Days}天
#a,2048,1,1#" : getConfig("customText_text").Replace("#nextLine#", "\r\n"));
            CustomText_Location = (getConfig("customText_location") == null ? "50|50" : getConfig("customText_location"));
            CustomText_Size = (getConfig("customText_size") == null ? 300d : double.Parse(getConfig("customText_size")));
            WebView_Location = (getConfig("webview_location") == null ? "50|50" : getConfig("webview_location"));
            WebView_Zoom = (getConfig("webview_zoom") == null ? 40 : int.Parse(getConfig("webview_zoom")));
            WebView_Height = (getConfig("webview_height") == null ? 200d : double.Parse(getConfig("webview_height")));
            WebView_Width = (getConfig("webview_width") == null ? 400d : double.Parse(getConfig("webview_width")));


            WebView_URL = (getConfig("webView_URL") == null ? "www.baidu.com" : getConfig("webView_URL"));
            WebView_Show = (getConfig("webView_show") == null ? 1 : int.Parse(getConfig("webView_show")));



        }













        private static string getConfig(string key)
        {
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("software\\fz\\desktop tools");
            try
            {
                return rk.GetValue(key).ToString();
            }
            catch (Exception)
            {
                return null;
            }

        }
    }

}
/*2016-08-27
 * 启动项目
 * 完成基本的时钟功能
 * 可以保存配置
 * 
 * 2016-08-28
 * 完成倒计时功能
 * 可以拖动小部件
 * 可以更改小部件大小
 * 增加时钟中心小圆点
 * 时钟边框随时钟放大缩小
 * 时区功能
 * 
 * 2016-10-14、15
 * 使用了快速属性
 * 将“倒计时”改为“自定义文本”并完成其中的倒计时部分
 * 
 * 2016-10-17
 * 增加“自定义文本”的当前时间功能
 * 
 * 2016-10-18
 * 将倒计时负数变成正数
 * 
 * 2016-11-2
 * 修复了设置多个倒计时时欲显示日期崩溃的BUG
 * 
 * 2016-11-13
 * 修复了customText无法设置是否显示的BUG
 * 将开机自启由Button改为CheckBox
 * 修复了放弃设置后无法恢复的BUG
 * 扔掉了附带的图标，改为内嵌
 * 
 * 2016-11-14
 * 修复了即使已经开机启动也不勾选的BUG
 * 
 * 2017-2-17
 * 增加网页框功能
 * 修复多个设置逻辑问题
 * 
 * 2017-2-18
 * 修复开机自启自动关闭的BUG
 * 
 * 2017-2-19
 * 新增了网页打不开以后自动刷新的功能
 */
