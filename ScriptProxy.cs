using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Effects;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;

namespace ScriptRunner
{
    /// <summary>
    /// 脚本代理
    /// </summary>
    public class ScriptProxy
    {
        public MouseProxy 鼠标 { get; } = new MouseProxy();
        public OsSystem 系统 { get; } = new OsSystem();

        public KeyBoard 键盘 { get; } = new KeyBoard();

        public void 延迟(int delay)
        {
            Thread.Sleep(delay);
        }
    }

    public class MouseProxy
    {
        public void 移动(int x,int y)
        {
            Mouse.MoveTo(new Point(x,y));
        }

        public void 单击(int x,int y)
        {
            Mouse.Click(new Point(x,y));
        }

        public void 双击(int x,int y)
        {
            Mouse.DoubleClick(new Point(x,y));
        }

        public void 拖动(int x,int y,int x2,int y2)
        {
            Mouse.Drag(new Point(x,y),new Point(x2,y2));
        }

        public void 滚屏(int x,int y)
        {
            if (x > 0)
            {
                Mouse.HorizontalScroll(x);
            }
        }
    }

    public class KeyBoard
    {
        
        public void 开始按键(VirtualKeyShort virtualKey)
        {
            Keyboard.Press(virtualKey);
        }

        public void 结束按键(VirtualKeyShort virtualKey)
        {
            Keyboard.Release(virtualKey);
        }

        public void 输入(string text)
        {
            Keyboard.Type(text);
        }
    }

    public class OsSystem
    {
        /// <summary>
        /// 退出
        /// </summary>
        public void 退出()
        {
            Application.Exit();
        }

        /// <summary>
        /// 图片特征
        /// </summary>
        /// <param name="rectVal"></param>
        /// <returns></returns>
        public string 图片特征(string rectVal)
        {
            var img = Img();
            var rect = Helper.GetRectangle(rectVal);
            using (var bmp = img.Clone(rect,img.PixelFormat))
            {
                return HashNormalHelper.Hash(bmp);
            }
        }

        public void 打开(string fileName,string args)
        {
            var process = Process.Start(fileName, args);
        }

        internal Func<Bitmap> Img;
    }
}
