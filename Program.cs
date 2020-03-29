using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptRunner
{
    class Program
    {
        static void Main()
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
                    Console.WriteLine($@"ERROR:加载文件：{exception}");
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

            using (var code = new ScriptRunner())
            {
                code.Code = File.ReadAllText(JsCode,Encoding.UTF8);
                code.Run();
            }

            // 超时未服务的，自动退出
            System.Environment.Exit(0);
        }

        public static string JsCode = "runner.js";


    }
}
