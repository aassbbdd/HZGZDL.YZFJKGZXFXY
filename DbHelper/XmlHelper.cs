using DbHelper.Db_Model;
using DbHelper.XmlModel;
using DocDecrypt.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Udp_Agreement;

namespace DbHelper
{
    public static class XmlHelper
    {
        #region Fields and Properties
        /// <summary>
        /// 文件夹名字
        /// </summary>
        private static string Xml_File = ConfigurationManager.ConnectionStrings["xml_path"].ConnectionString.ToString();

        /// <summary>
        /// 内存数据
        /// </summary>
        static XElement xele;

        /// <summary>
        /// xml 文件路径
        /// </summary>
        public static string xmlpath = "";
        /// <summary>
        /// 文件名字
        /// </summary>
        static string xmlname = "";
        #endregion

        /// <summary>
        ///     创建XML文档
        /// </summary>
        /// <param name="name">根节点名称</param>
        /// <param name="type">根节点的一个属性值</param>
        /// <returns></returns>
        public static void CreateXmlDocument(string name)
        {
            try
            {
                //文件夹路径
                xmlpath = AppDomain.CurrentDomain.BaseDirectory + Xml_File + "\\";
                if (!FileHelper.IsFileExist(xmlpath))//验证文件是否存在
                {
                    FileHelper.CreateDirectoy(xmlpath);//创建文件夹
                }
                xmlpath = xmlpath + "\\" + name + ".xml"; //拼接文件路径
                if (!FileHelper.IsFileExist(xmlpath))//验证文件是否存在
                {
                    xele = new XElement("root");
                    xele.SetAttributeValue("IsCount", "false");//默认数据没转换好
                    xele.Save(xmlpath);
                }

            }
            catch (Exception exception)
            {
                //LogHelper.LogError("Error! ", exception);
            }
        }

        /// <summary>
        /// 删除XML文档
        /// </summary>
        /// <returns></returns>
        public static void DeleteXmlDocument(string name)
        {
            try
            {
                //文件夹路径
                xmlpath = AppDomain.CurrentDomain.BaseDirectory + Xml_File + "\\";
                if (!FileHelper.IsFileExist(xmlpath))//验证文件是否存在
                {
                    FileHelper.CreateDirectoy(xmlpath);//创建文件夹
                }
                xmlpath = xmlpath + "\\" + name + ".xml"; //拼接文件路径
                if (FileHelper.IsFileExist(xmlpath))//验证文件是否存在
                {
                    FileHelper.DeleteFile(xmlpath);
                }

            }
            catch (Exception exception)
            {
                //LogHelper.LogError("Error! ", exception);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name"></param>
        public static void Init(string name, Test_Plan model)
        {
            xmlname = name;
            CreateXmlDocument(name);
            xele = XElement.Load(xmlpath);
            model.ISEDIT = "";
            Init_Config(model);
        }

        #region 新增节点

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>        
        /// <param name="xmlNode">要插入的Xml节点</param>
        public static void Insert(Xml_Node_Model model)
        {
            if (model.Id == "1")
            {
                XElement element = new XElement("fgx", "--------------------------" + model.DataSource + "--------------------------");
                xele.Add(element);
            }
            else
            {
                XElement element = ToXElement(model);
                xele.Add(element);
            }
        }

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>        
        /// <param name="xmlNode">要插入的Xml节点</param>
        public static void Init_Config(Test_Plan model)
        {
            //插入数据时检查xml文件是否存在
            XElement element = ToXElement(model);
            xele.Add(element);
        }

        /// <summary>
        /// 保存到文档
        /// </summary>
        public static void Save()
        {
            if (xele != null)
            {
                xele.Save(xmlpath);
                // Init();
            }
        }

        #endregion

        #region 实体列表转换成 XElement 或string 字符串
        public static string ToXml<T>(IList<T> entities, string rootName = "") where T : new()
        {
            if (entities == null || entities.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("未使用");//XmlResource.XmlHeader

            XElement element = ToXElement<T>(entities, rootName);
            builder.Append(element.ToString());

            return builder.ToString();
        }

        public static XmlDocument ToXmlDocument<T>(IList<T> entities, string rootName = "") where T : new()
        {
            if (entities == null || entities.Count == 0)
            {
                return null;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(ToXml<T>(entities, rootName));

            return xmlDocument;
        }

        public static XDocument ToXDocument<T>(IList<T> entities, string rootName = "") where T : new()
        {
            if (entities == null || entities.Count == 0)
            {
                return null;
            }

            return XDocument.Parse(ToXml<T>(entities, rootName));
        }

        public static XElement ToXElement<T>(IList<T> entities, string rootName = "") where T : new()
        {
            if (entities == null || entities.Count == 0)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(rootName))
            {
                rootName = typeof(T).Name;
            }

            XElement element = new XElement(rootName);

            foreach (T entity in entities)
            {
                element.Add(ToXElement<T>(entity));
            }

            return element;
        }

        /// <summary>
        /// 实体转换成 string 字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string ToXml<T>(T entity) where T : new()
        {
            if (entity == null)
            {
                return string.Empty;
            }

            XElement element = ToXElement<T>(entity);

            return element.ToString();
        }

        /// <summary>
        /// 实体转换成 XElement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static XElement ToXElement<T>(T entity)
        {
            if (entity == null)
            {
                return null;
            }
            PropertyInfo[] properties = typeof(T).GetProperties();
            object id = properties[0].GetValue(entity, null);
            XElement element = new XElement(typeof(T).Name);
            XElement innerElement = null;
            object propertyValue = null;

            foreach (PropertyInfo property in properties)
            {
                string type = property.GetValue(entity).GetType().Name;
                if (type.ToLower() == "string" || type.ToLower() == "int32" || type.ToLower() == "double")//判断属性是否是基本类型比如strin int double 若是，则建立节点并赋值
                {
                    propertyValue = property.GetValue(entity, null);
                    innerElement = new XElement(property.Name, propertyValue);
                    element.Add(innerElement);
                }
                //else //否则，递归运行Process函数
                //{
                //    var t= typeof(T);
                //    var dd= t.GetProperty("Data");

                //    //string colType = property.PropertyType.GenericParameterAttributes.ToString();
                //    //if (colType == typeof(List<Xml_Node_Model>))
                //    //{
                //        object obj = property.GetValue(entity, null);
                //        List<Xml_Node_Model> iml = (List<Xml_Node_Model>)obj;
                //        element.Add(ToXElement<Xml_Node_Model>(iml));
                //    //}

                //}

            }

            return element;
        }

        public static XElement ToXElement(Type type)
        {
            if (type == null)
            {
                return null;
            }

            XElement element = new XElement(type.Name);
            PropertyInfo[] properties = type.GetProperties();
            XElement innerElement = null;

            foreach (PropertyInfo property in properties)
            {
                innerElement = new XElement(property.Name, null);
                element.Add(innerElement);
            }

            return element;
        }
        #endregion

        #region xml 转换成实体

        /// <summary>
        /// xml转实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="path">xml路径</param>
        /// <returns></returns>
        public static Test_Plan Xml_To_Model(string path)
        {
            try
            {
                XDocument document;
                try
                {
                    document = XDocument.Load(path);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                XElement root = document.Root;
                XElement ele = root.Element("Test_Plan");
                Test_Plan model = new Test_Plan();
                model = (Test_Plan)Deserialize(typeof(Test_Plan), ele.ToString());
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// xml转列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="path">xml路径</param>
        /// <returns></returns>
        public static List<Xml_Node_Model> Xml_To_List(string path)
        {
            try
            {
                List<Xml_Node_Model> list = new List<Xml_Node_Model>();

                XDocument document;
                try
                {
                    document = XDocument.Load(path);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                XElement root = document.Root;
                //判断数据是否已经解析完成
                string IsCount = root.Attribute("IsCount").Value;

                IEnumerable<XElement> eles = root.Elements("Xml_Node_Model");

                int index = 0; //index 为索引值
                foreach (XElement item in eles)
                {
                    Xml_Node_Model model = new Xml_Node_Model();
                    model.Id = item.Element("Id").Value;
                    model.DataSource = item.Element("DataSource").Value;

                    if (IsCount == "true")
                    {
                        model.Data = (from data in item.Element("Data").Descendants("Xml_Element_Model")
                                      select new Xml_Element_Model
                                      {
                                          Xwitdh = data.Element("Xwitdh").Value != null ? String.Empty : data.Element("Xwitdh").Value,
                                          V1 = data.Element("V1").Value != null ? String.Empty : data.Element("V1").Value,
                                          V2 = data.Element("V2").Value != null ? String.Empty : data.Element("V2").Value,
                                          V3 = data.Element("V3").Value != null ? String.Empty : data.Element("V3").Value,
                                          C1 = data.Element("C1 ").Value != null ? String.Empty : data.Element("C1 ").Value,
                                          C2 = data.Element("C2 ").Value != null ? String.Empty : data.Element("C2 ").Value,
                                          C3 = data.Element("C3 ").Value != null ? String.Empty : data.Element("C3 ").Value,
                                          Id = data.Element("Id ").Value != null ? String.Empty : data.Element("Id ").Value
                                      }).ToList();
                    }
                    else
                    {
                        model.Data = Algorithm_Data(model, index);
                    }
                    list.Add(model);
                    index++;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static DataTable dt = new DataTable();
        /// <summary>
        /// xml转DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="path">xml路径</param>
        /// <returns></returns>
        public static DataTable Xml_To_DataTable(string path, bool[] cks)
        {
            try
            {
                dt = new DataTable();
                dt.Columns.Add("V1", typeof(string));
                dt.Columns.Add("V2", typeof(string));
                dt.Columns.Add("V3", typeof(string));
                dt.Columns.Add("C1", typeof(string));
                dt.Columns.Add("C2", typeof(string));
                dt.Columns.Add("C3", typeof(string));
                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("Xwitdh", typeof(string));
                XDocument document;
                try
                {
                    document = XDocument.Load(path);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                XElement root = document.Root;
                string IsCount = root.Attribute("IsCount").Value;

                IEnumerable<XElement> eles = root.Elements("Xml_Node_Model");

                int index = 0; //index 为索引值
                foreach (XElement item in eles)
                {
                    if (IsCount == "true")
                    {

                        IEnumerable<XElement> eChild = item.Element("Data").Descendants("Xml_Element_Model");

                        foreach (XElement xe in eChild)
                        {
                            DataRow dr = dt.NewRow();

                            dr["V1"] = xe.Element("V1").Value != null ? String.Empty : xe.Element("V1").Value;
                            dr["V2"] = xe.Element("V2").Value != null ? String.Empty : xe.Element("V2").Value;
                            dr["V3"] = xe.Element("V3").Value != null ? String.Empty : xe.Element("V3").Value;
                            dr["C1"] = xe.Element("C1").Value != null ? String.Empty : xe.Element("C1").Value;
                            dr["C2"] = xe.Element("C2").Value != null ? String.Empty : xe.Element("C2").Value;
                            dr["C3"] = xe.Element("C3").Value != null ? String.Empty : xe.Element("C3").Value;

                            dr["Xwitdh"] = xe.Element("Xwitdh").Value != null ? String.Empty : xe.Element("Xwitdh").Value;
                            dr["Id"] = xe.Element("Id").Value != null ? String.Empty : xe.Element("Id").Value;

                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        Algorithm_To_DataTable(item, index);
                    }
                    index++;
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 计算点位 存储为list
        /// </summary>
        private static List<Xml_Element_Model> Algorithm_Data(Xml_Node_Model model, int jinex)
        {
            //转成毫秒除数
            int allnum = 100000;
            //基本宽度
            int num = 1;
            //一组数据总计算次数
            int count = 80;
            string data = model.DataSource.Substring(8, model.DataSource.Length - 8);
            List<Xml_Element_Model> list = new List<Xml_Element_Model>();
            for (int i = 0; i < count; i++)
            {
                Xml_Element_Model newmodel = new Xml_Element_Model();
                int length = 24 * i;//截取位置

                string Current1 = data.Substring(0 + length, 4);
                string Current2 = data.Substring(4 + length, 4);
                string Current3 = data.Substring(8 + length, 4);

                string Vibration1 = data.Substring(12 + length, 4);
                string Vibration2 = data.Substring(16 + length, 4);
                string Vibration3 = data.Substring(20 + length, 4);
                //计算
                newmodel.C1 = Algorithm.Instance.Current_Algorithm(Current1);
                newmodel.C2 = Algorithm.Instance.Current_Algorithm(Current2);
                newmodel.C3 = Algorithm.Instance.Current_Algorithm(Current3);

                newmodel.V1 = Algorithm.Instance.Vibration_Algorithm(Vibration1);
                newmodel.V2 = Algorithm.Instance.Vibration_Algorithm(Vibration2);
                newmodel.V3 = Algorithm.Instance.Vibration_Algorithm(Vibration3);

                newmodel.Id = ((jinex * count) + i).ToString();
                //单点宽度计算公式  当前包的 ((序号* 包截取个数) +当前截取序号)/转成毫秒除数
                double newXvalue = (double)((jinex * count) + num) / allnum;

                newmodel.Xwitdh = newXvalue.ToString();

                list.Add(newmodel);
            }
            return list;
        }

        /// <summary>
        /// 存储为DataTable
        /// </summary>
        private static void Algorithm_To_DataTable(XElement model, int jinex)
        {
            //转成毫秒除数
            int allnum = 100000;
            //基本宽度
            int num = 1;
            //一组数据总计算次数
            int count = 80;

            string DataSource = model.Element("DataSource").Value;
            string data = DataSource.Substring(8, DataSource.Length - 8);

            for (int i = 0; i < count; i++)
            {
                DataRow dr = dt.NewRow();

                int length = 24 * i;//截取位置

                string Current1 = data.Substring(0 + length, 4);
                string Current2 = data.Substring(4 + length, 4);
                string Current3 = data.Substring(8 + length, 4);

                string Vibration1 = data.Substring(12 + length, 4);
                string Vibration2 = data.Substring(16 + length, 4);
                string Vibration3 = data.Substring(20 + length, 4);
                //计算
                dr["C1"] = Algorithm.Instance.Current_Algorithm(Current1);
                dr["C2"] = Algorithm.Instance.Current_Algorithm(Current2);
                dr["C3"] = Algorithm.Instance.Current_Algorithm(Current3);

                dr["V1"] = Algorithm.Instance.Vibration_Algorithm(Vibration1);
                dr["V2"] = Algorithm.Instance.Vibration_Algorithm(Vibration2);
                dr["V3"] = Algorithm.Instance.Vibration_Algorithm(Vibration3);

                dr["Id"] = ((jinex * count) + i).ToString();
                //单点宽度计算公式  当前包的 ((序号* 包截取个数) +当前截取序号)/转成毫秒除数
                double newXvalue = (double)((jinex * count) + num + i) / allnum;

                dr["Xwitdh"] = newXvalue.ToString();
                dt.Rows.Add(dr);
            }
        }

        /// <summary>        
        /// 存储为数组 arrey
        /// </summary>
        private static void Algorithm_To_Arrey(XElement model, int jinex
            )
        {
            //转成毫秒除数
            int allnum = 100000;
            //基本宽度
            int num = 1;
            //一组数据总计算次数
            int count = 80;

            string DataSource = model.Element("DataSource").Value;
            string data = DataSource.Substring(8, DataSource.Length - 8);

            for (int i = 0; i < count; i++)
            {

                int length = 24 * i;//截取位置

                string Current1 = data.Substring(0 + length, 4);
                string Current2 = data.Substring(4 + length, 4);
                string Current3 = data.Substring(8 + length, 4);

                string Vibration1 = data.Substring(12 + length, 4);
                string Vibration2 = data.Substring(16 + length, 4);
                string Vibration3 = data.Substring(20 + length, 4);
                int id = (jinex * count) + i;

                //计算
                newcy1[id] = double.Parse(Algorithm.Instance.Current_Algorithm(Current1));
                newcy2[id] = double.Parse(Algorithm.Instance.Current_Algorithm(Current2));
                newcy3[id] = double.Parse(Algorithm.Instance.Current_Algorithm(Current3));

                newvy1[id] = double.Parse(Algorithm.Instance.Vibration_Algorithm(Vibration1));
                newvy2[id] = double.Parse(Algorithm.Instance.Vibration_Algorithm(Vibration2));
                newvy3[id] = double.Parse(Algorithm.Instance.Vibration_Algorithm(Vibration3));


                //单点宽度计算公式  当前包的 ((序号* 包截取个数) +当前截取序号)/转成毫秒除数
                double newXvalue = (double)((jinex * count) + num + i) / allnum;


                newvx1[id] = newvx2[id] = newvx3[id]
              = newcx1[id] = newcx2[id] = newcx3[id]
              = newXvalue;

            }
        }
        static double[] newvx1; static double[] newvy1;
        static double[] newvx2; static double[] newvy2;
        static double[] newvx3; static double[] newvy3;
        static double[] newcx1; static double[] newcy1;
        static double[] newcx2; static double[] newcy2;
        static double[] newcx3; static double[] newcy3;


        /// <summary>
        /// xml转DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="path">xml路径</param>
        /// <returns></returns>
        public static void Xml_To_Array(string path, bool[] cks,
            out double[] vx1, out double[] vy1,
            out double[] vx2, out double[] vy2,
            out double[] vx3, out double[] vy3,
            out double[] cx1, out double[] cy1,
            out double[] cx2, out double[] cy2,
            out double[] cx3, out double[] cy3
            )
        {
            try
            {
                XDocument document;
                try
                {
                    document = XDocument.Load(path);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                XElement root = document.Root;
                string IsCount = root.Attribute("IsCount").Value;

                IEnumerable<XElement> eles = root.Elements("Xml_Node_Model");

                int count = eles.Count() * 80;
                newvx1 = new double[count]; newvy1 = new double[count];
                newvx2 = new double[count]; newvy2 = new double[count];
                newvx3 = new double[count]; newvy3 = new double[count];

                newcx1 = new double[count]; newcy1 = new double[count];
                newcx2 = new double[count]; newcy2 = new double[count];
                newcx3 = new double[count]; newcy3 = new double[count];


                vx1 = new double[count]; vy1 = new double[count];
                vx2 = new double[count]; vy2 = new double[count];
                vx3 = new double[count]; vy3 = new double[count];

                cx1 = new double[count]; cy1 = new double[count];
                cx2 = new double[count]; cy2 = new double[count];
                cx3 = new double[count]; cy3 = new double[count];


                int index = 0; //index 为索引值
                foreach (XElement item in eles)
                {
                    if (IsCount == "true")
                    {

                        IEnumerable<XElement> eChild = item.Element("Data").Descendants("Xml_Element_Model");
                        int i = 0;
                        foreach (XElement xe in eChild)
                        {
                            newvx1[i] = newvx2[i] = newvx3[i]
                          = newcx1[i] = newcx2[i] = newcx3[i]
                          = item.Element("Xwitdh").Value != null ? 0.000 : double.Parse(xe.Element("Xwitdh").Value);

                            newvy1[i] = xe.Element("V1").Value != null ? 0 : double.Parse(xe.Element("V1").Value);
                            newvy3[i] = xe.Element("V2").Value != null ? 0 : double.Parse(xe.Element("V2").Value);
                            newvy3[i] = xe.Element("V3").Value != null ? 0 : double.Parse(xe.Element("V3").Value);
                            newcy1[i] = xe.Element("C1").Value != null ? 0 : double.Parse(xe.Element("C1").Value);
                            newcy3[i] = xe.Element("C2").Value != null ? 0 : double.Parse(xe.Element("C2").Value);
                            newcy3[i] = xe.Element("C3").Value != null ? 0 : double.Parse(xe.Element("C3").Value);
                            i++;
                        }
                    }
                    else
                    {
                        Algorithm_To_Arrey(item, index);
                    }
                    index++;
                }

                vx1 = newvx1; vy1 = newvy1;
                vx2 = newvx2; vy2 = newvy2;
                vx3 = newvx3; vy3 = newvy3;

                cx1 = newcx1; cy1 = newcy1;
                cx2 = newcx2; cy2 = newcy2;
                cx3 = newcx3; cy3 = newcy3;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        #endregion
        #endregion
    }
}