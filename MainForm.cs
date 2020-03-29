using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptRunner
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.textBoxMsg.MaxLength = Int32.MaxValue;
        }

        private void 调试信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            LoadLog();

        }

        private const int WM_HOTKEY = 0x312; //窗口消息：热键
        private const int WM_CREATE = 0x1; //窗口消息：创建
        private const int WM_DESTROY = 0x2; //窗口消息：销毁

        private const int HotKeyID = 123; //热键ID（自定义）

        protected override void WndProc(ref Message msg)
        {
            base.WndProc(ref msg);
            switch (msg.Msg)
            {
                case WM_HOTKEY: //窗口消息：热键
                    int tmpWParam = msg.WParam.ToInt32();
                    if (tmpWParam == HotKeyID)
                    {
                        // 超时未服务的，自动退出
                        System.Environment.Exit(0);
                    }
                    break;
                case WM_CREATE: //窗口消息：创建
                    AppHotKey.RegisterHotKey(this.Handle, HotKeyID, AppHotKey.KeyModifiers.Ctrl | AppHotKey.KeyModifiers.Shift | AppHotKey.KeyModifiers.Alt, Keys.K);
                    break;
                case WM_DESTROY: //窗口消息：销毁
                    AppHotKey.UnregisterHotKey(this.Handle, HotKeyID); //销毁热键
                    break;
                default:
                    break;
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadLog();
            this.Visible = false;
        }

        private void LoadLog()
        {
            try
            {
                this.textBoxMsg.Text = Log;
            }
            catch (Exception exception)
            {
                MainForm.WriteLine(exception);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
            }
        }


        static StringBuilder sb = new StringBuilder();

        public static void WriteLine(object msg)
        {
            if (sb.Length > Int32.MaxValue / 3)
            {
                sb.Clear();
            }
            Console.WriteLine(msg);
            sb.Insert(0,$@"{msg}");
        }

        public static string Log => sb.ToString();
    }
}
