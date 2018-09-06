using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udp_Agreement.Model;

namespace Commons
{
    public class ListToText
    {
        /// <summary>
        /// 获取唯一实例
        /// </summary>
        private static ListToText instance = null;
        public static ListToText Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ListToText();
                }
                return instance;
            }
        }

        //读取文本文件转换为List
        public List<string> ReadTextFileToList(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            List<string> list = new List<string>();
            StreamReader sr = new StreamReader(fs);
            //使用StreamReader类来读取文件
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            // 从数据流中读取每一行，直到文件的最后一行
            string tmp = sr.ReadLine();
            while (tmp != null)
            {
                list.Add(tmp);
                tmp = sr.ReadLine();
            }
            //关闭此StreamReader对象
            sr.Close();
            fs.Close();
            return list;
        }

        //将List转换为TXT文件
        public void WriteListToTextFile(List<DataModel> list, string txtFile)
        {
            try
            {
                FileStream fs;
                if (File.Exists(txtFile))
                {
                    // MessageBox.Show("文件存在");
                    fs = new FileStream(txtFile + "\\TestTxt.txt", FileMode.Open, FileAccess.Write);
                }
                else
                {
                    // MessageBox.Show("无此文件");
                    fs = new FileStream(txtFile + "\\TestTxt.txt", FileMode.OpenOrCreate, FileAccess.Write);
                }
                //创建一个文件流，用以写入或者创建一个StreamWriter

                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);//转码
                sw.Flush();
                //使用StreamWriter来往文件中写入内容
                sw.BaseStream.Seek(0, SeekOrigin.Begin);

                var nlist = list.OrderByDescending(x => x.id);

                foreach (DataModel item in nlist)
                {
                    sw.WriteLine("序号：" + item.id);
                    //sw.WriteLine("序号16：" + item.head);

                    sw.WriteLine("文本：" + item.text);
                    sw.WriteLine("开始解析---------------------------------------------------------------------------------");

                    for (int i = 0; i < item.old_data.Count; i++)
                    {
                        sw.WriteLine("电流1: " + item.old_data[i].Current1 + " :" + item.new_data[i].Current1);
                        //sw.WriteLine("电流2: " + item.old_data[i].Current2 + " :" + item.new_data[i].Current2);
                        //sw.WriteLine("电流3: " + item.old_data[i].Current3 + " :" + item.new_data[i].Current3);

                        sw.WriteLine("震动1: " + item.old_data[i].Vibration1 + " :" + item.new_data[i].Vibration1);
                        //sw.WriteLine("震动2: " + item.old_data[i].Vibration2 + " :" + item.new_data[i].Vibration2);
                        //sw.WriteLine("震动3: " + item.old_data[i].Vibration3 + " :" + item.new_data[i].Vibration3);
                    };
                    sw.WriteLine("结束解析---------------------------------------------------------------------------------");

                }

                //关闭此文件
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }


        //将List转换为TXT文件
        public void WriteListToTextFile1(List<DataModel> list, string txtFile)
        {
            FileStream fs;
            if (File.Exists(txtFile))
            {
                // MessageBox.Show("文件存在");
                fs = new FileStream(txtFile + "\\TestTxtId.txt", FileMode.Open, FileAccess.Write);
            }
            else
            {
                // MessageBox.Show("无此文件");
                fs = new FileStream(txtFile + "\\TestTxtId.txt", FileMode.OpenOrCreate, FileAccess.Write);
            }
            //创建一个文件流，用以写入或者创建一个StreamWriter

            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);//转码
            sw.Flush();
            //使用StreamWriter来往文件中写入内容
            sw.BaseStream.Seek(0, SeekOrigin.Begin);

            var nlist = list.OrderByDescending(x => x.id);

            foreach (DataModel item in nlist)
            {
                sw.WriteLine("开始解析---------------------------------------------------------------------------------");

                sw.WriteLine("序号：" + item.id);
                sw.WriteLine("序号16：" + item.head);

                sw.WriteLine("结束解析---------------------------------------------------------------------------------");

            }

            //关闭此文件
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        //将List转换为TXT文件
        public void WriteListToTextFile(string text, string txtFile)
        {
            FileStream fs;
            StreamWriter sw;

            string path = txtFile + "TestTxttext.txt";
            if (File.Exists(path))
            {
                sw = new StreamWriter(path, true, Encoding.UTF8);//转码

                //创建一个文件流，用以写入或者创建一个StreamWriter
                sw.Flush();
                //使用StreamWriter来往文件中写入内容
                //   sw.BaseStream.Seek(0, SeekOrigin.Begin);
                sw.WriteLine(DateTime.Now.ToString("O") + " : " + text);
                //关闭此文件
                sw.Flush();
                sw.Close();
            }
            else
            {
                // MessageBox.Show("无此文件");
                fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                sw = new StreamWriter(fs, Encoding.UTF8);//转码

                //创建一个文件流，用以写入或者创建一个StreamWriter
                sw.Flush();
                //使用StreamWriter来往文件中写入内容
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                sw.WriteLine(DateTime.Now.ToString("O") + " : " + text);
                //关闭此文件
                sw.Flush();
                sw.Close();
                fs.Close();
            }

        }
        //将List转换为TXT文件
        public void WriteListToTextFile1(string text)
        {
            FileStream fs;
            StreamWriter sw;
            string path = AppDomain.CurrentDomain.BaseDirectory + "log.txt";


            if (File.Exists(path))
            {
                sw = new StreamWriter(path, true, Encoding.UTF8);//转码

                //创建一个文件流，用以写入或者创建一个StreamWriter
                sw.Flush();
                //使用StreamWriter来往文件中写入内容
                //   sw.BaseStream.Seek(0, SeekOrigin.Begin);
                sw.WriteLine(DateTime.Now.ToString()+":  "+ text);
                //关闭此文件
                sw.Flush();
                sw.Close();
            }
            else
            {
                // MessageBox.Show("无此文件");
                fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                sw = new StreamWriter(fs, Encoding.UTF8);//转码

                //创建一个文件流，用以写入或者创建一个StreamWriter
                sw.Flush();
                //使用StreamWriter来往文件中写入内容
                sw.BaseStream.Seek(0, SeekOrigin.Begin);
                sw.WriteLine(text);
                //关闭此文件
                sw.Flush();
                sw.Close();
                fs.Close();
            }

        }

    }
}
