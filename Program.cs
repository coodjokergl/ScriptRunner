using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptRunner
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Task.Run(Run);
            Application.Run(new MainForm());
        }


        public static void Run()
        {
            var jsFiles = new List<string>();
            var args = Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                try
                {
                    if (Path.GetExtension(arg) == ".js" && File.Exists(arg))
                    {
                        jsFiles.Add(arg);
                    }

                    if (Directory.Exists(arg))
                    {
                        jsFiles.AddRange(Directory.GetFiles(arg,"*.js",SearchOption.AllDirectories));
                    }
                }
                catch (Exception exception)
                {
                    MainForm.WriteLine($@"ERROR:加载文件：{exception}");
                }
            }

            if (jsFiles.OrderBy(q=>q).Any())
            {
                var allCode = new StringBuilder();
                foreach (var jsFile in jsFiles)
                {
                    allCode.AppendLine($@"//{jsFile}");
                    allCode.AppendLine(File.ReadAllText(jsFile));
                    allCode.AppendLine();
                }

                if (File.Exists(JsCode))
                {
                    File.Delete(JsCode);
                }

                File.WriteAllText(JsCode,allCode.ToString(),Encoding.UTF8);
            }
            FileStream fs = new FileStream("runner.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.AutoFlush = true;
            Console.SetOut(sw);

            using (var code = new ScriptRunner())
            {
                code.Code = File.ReadAllText(JsCode,Encoding.UTF8);
                code.Run();
            }


            // 超时未服务的，自动退出
            Application.Exit();
        }

        public static string JsCode = "runner.js";


    }
}
