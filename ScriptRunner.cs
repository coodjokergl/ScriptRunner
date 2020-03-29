using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlaUI.Core.Capturing;
using System.Windows;
using FlaUI.Core.WindowsAPI;
using Microsoft.ClearScript;

namespace ScriptRunner
{
    /// <summary>
    /// 启动器
    /// </summary>
    public class ScriptRunner : IDisposable
    {
        public ScriptProxy Proxy = new ScriptProxy();
        public ScriptRunner()
        {
            V8 = new V8ScriptEngine("script code runner");
            V8.AddHostObject("host", new HostFunctions());
            V8.AddHostObject("代理",Proxy);
            V8.AddHostObject("库", new HostTypeCollection("mscorlib", "System.Core"));
            V8.AddHostType("键盘",typeof(VirtualKeyShort));

            V8.AddHostType("Console", typeof(Console));
           
            UiTimer = new System.Threading.Timer(new TimerCallback((state =>
            {
                lock (this)
                {
                    if (IsExecuting)
                    {
                        MainForm.WriteLine("上一任务未执行完成，此次轮空");
                        return;
                    }
                    IsExecuting = true;
                }

                try
                {
                    StartUp();
                    IsExecuting = false;
                }
                catch (Exception exception)
                {
                    MainForm.WriteLine($@"ERROR runner script:{exception}");
                    IsExecuting = false;
                    IsRun = false;
                }
            })), null, 0, 300);
        }

        V8ScriptEngine V8;

        Timer UiTimer;
        public void Dispose()
        {
            V8.Dispose();
        }

        public void StartUp()
        {   
            MainForm.WriteLine("加载脚本...");
            V8.Execute(Code); //加载代码

            CaptureImage screen = Capture.Screen(V8.Script.屏幕());
            var 当前工作区域 = 工作区域 == System.Drawing.Rectangle.Empty ?
                new System.Drawing.Rectangle(System.Drawing.Point.Empty,screen.Bitmap.Size) : 工作区域;
            using (var bmp = screen.Bitmap.Clone(当前工作区域, screen.Bitmap.PixelFormat))
            {
                try
                {
                    Proxy.系统.Img = () => bmp;
                    MainForm.WriteLine("脚本执行中...");
                    V8.Script.运行();
                    MainForm.WriteLine("脚本执行结束");
                }
                catch (Exception exception)
                {
                    MainForm.WriteLine($@"ERROR runner caller : {exception}");
                }
                finally
                {
                    Proxy.系统.Img = null;
                }
            }
        }

        public System.Drawing.Rectangle 工作区域 => Helper.GetRectangle(V8.Script.工作区域());

        public bool IsExecuting { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 启动
        /// </summary>
        public void Run()
        {
            IsRun = true;
            while (IsRun)
            {
                Thread.Sleep(1000);
            }

            MainForm.WriteLine("即将退出...");
            Thread.Sleep(5000);
        }

        public static bool IsRun { get; set; }
    }
}
