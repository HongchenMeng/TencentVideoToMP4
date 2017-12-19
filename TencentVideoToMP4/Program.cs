using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace TencentVideoToMP4
{
    class Program
    {
        static void Main(string[] args)
        {
         try
            {
                //获取和设置当前目录（即该进程从中启动的目录）的完全限定路径。
                string pathString = System.Environment.CurrentDirectory;

                //获取该路径下所有文件
                string[] files = Directory.GetFiles(pathString, "*.tdl", SearchOption.TopDirectoryOnly);

                //获取盘符并，加上:号
                string name1 = files[0].Substring(0, 1) + ":";

                string name2 = Path.GetFileNameWithoutExtension(files[0]);
                name2 = name2.Substring(0, name2.Length - 4);
                //获取文件的第一个字符
                string name3 = name2.Substring(0, 1);
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < files.Count(); i++)
                {
                    stringBuilder.Append("*" + i.ToString().PadLeft(3, '0') + ".tdl");
                    stringBuilder.Append("+");
                }
                stringBuilder.Remove(stringBuilder.Length - 1, 1);

                string strtdl = stringBuilder.ToString();
                string str = string.Format(@"copy/B {0} {1}.mp4", strtdl, name2);

                string str2 = Path.Combine(pathString, name2 + ".mp4");
                if (File.Exists(str2))
                {
                    File.Delete(str2);
                }
                Process p = new Process();
                //设置要启动的应用程序
                p.StartInfo.FileName = "cmd.exe";
                //是否使用操作系统shell启动
                p.StartInfo.UseShellExecute = false;
                // 接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardInput = true;
                //输出信息
                p.StartInfo.RedirectStandardOutput = true;
                // 输出错误
                p.StartInfo.RedirectStandardError = true;
                //不显示程序窗口
                p.StartInfo.CreateNoWindow = true;
                //启动程序
                p.Start();

                //向cmd窗口发送输入信息
                p.StandardInput.WriteLine("cd " + pathString);
                p.StandardInput.WriteLine(name1);
                p.StandardInput.WriteLine(str);
                p.StandardInput.WriteLine(string.Format("已将缓存文件转换为MP4格式，文件名为:{0}", name2));
                p.StandardInput.WriteLine("按任意键退出&exit");

                p.StandardInput.AutoFlush = true;

                //获取输出信息
                string strOuput = p.StandardOutput.ReadToEnd();
                //等待程序执行完退出进程
                p.WaitForExit();
                p.Close();

                Console.WriteLine(strOuput);

                Console.ReadKey();
            }
            catch
            {

            }


        }

        }
}
