using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinUdpServer
{
    public partial class Test_Class : Form
    {
        public Test_Class()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<test1> list = Data_Get();
           // var aa = ConvertList(list);
        }

        private List<test1> Data_Get()
        {
            List<test1> list = new List<test1>();
            for (int i = 0; i < 4; i++)
            {
                test1 model = new test1();
                model.id = i;
                model.name = "name" + i;
                list.Add(model);
            }
            return list;
        }

        //public static T ConvertModel<T>(T list1)
        //{
        //    Type type = typeof(T);
        //    PropertyInfo[] properties = type.GetProperties();
        //    T model = Activator.CreateInstance<T>();

        //    for (int i = 0; i < properties.Length; i++)
        //    {
        //        ////判断属性的名称和字段的名称是否相同
        //        //if (properties[i].Name == sdr.GetName(j))
        //        //{
        //        //    Object value = sdr[j];
        //        //    //将字段的值赋值给User中的属性
        //        //    properties[i].SetValue(model, value, null);
        //        //}

        //        list.Add(model);
        //    }
        //}

        //public static List<T> ConvertList<T>(List<T> list1)
        //{
        //    List<T> list = new List<T>();
        //    Type type = typeof(T);
        //    PropertyInfo[] properties = type.GetProperties();
        //    T model = Activator.CreateInstance<T>();
        //    for (j = 0; j < list1.Count; j++)
        //        for (int i = 0; i < properties.Length; i++)
        //        {
        //            ////判断属性的名称和字段的名称是否相同
        //            //if (properties[i].Name == sdr.GetName(j))
        //            //{
        //            //    Object value = sdr[j];
        //            //    //将字段的值赋值给User中的属性
        //            //    properties[i].SetValue(model, value, null);
        //            //}

        //            list.Add(model);
        //        }
        //    return list;
        //}
        public static List<T> ConvertData<T>(SqlDataReader sdr)
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            while (sdr.Read())
            {
                T model = Activator.CreateInstance<T>();
                for (int i = 0; i < properties.Length; i++)
                {
                    for (int j = 0; j < sdr.FieldCount; j++)
                    {
                        //判断属性的名称和字段的名称是否相同
                        if (properties[i].Name == sdr.GetName(j))
                        {
                            Object value = sdr[j];
                            //将字段的值赋值给User中的属性
                            properties[i].SetValue(model, value, null);
                        }
                    }
                }
                list.Add(model);
            }
            return list;
        }
    }

    public class test1
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}
