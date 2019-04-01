using DbHelper.Db_Model;
using DbHelper.XmlModel;
using Common;
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

        static double[] newvx1; static double[] newvy1;
        static double[] newvx2; static double[] newvy2;
        static double[] newvx3; static double[] newvy3;
        static double[] newcx1; static double[] newcy1;
        static double[] newcx2; static double[] newcy2;
        static double[] newcx3; static double[] newcy3;
        static int I;
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
            catch (Exception ex)
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
        public static void Edit_Voltage(Test_Plan model)
        {
            try
            {
                XDocument document;
                try
                {
                    document = XDocument.Load(xmlpath);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                XElement root = document.Root;
                XElement ele = root.Element("Test_Plan");

                if (ele == null)
                {
                    return;
                }

                ele.Element("VOLTAGE").SetValue(model.VOLTAGE);

                document.Save(xmlpath);
            }
            catch (Exception ex)
            {
                throw ex;
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
                var IsVauleNull = property.GetValue(entity);
                if (IsVauleNull == null)
                {
                    propertyValue = property.GetValue(entity, null);
                    innerElement = new XElement(property.Name, propertyValue);
                    element.Add(innerElement);
                }
                else
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
                if (ele == null)
                {
                    return model;
                }

                model.ID = ele.Element("ID").Value;
                model.PARENTID = ele.Element("PARENTID").Value;
                model.DVNAME = ele.Element("DVNAME").Value;
                model.DVPOSITION = ele.Element("DVPOSITION").Value;
                model.DVID = ele.Element("DVID").Value;
                model.TESTER = ele.Element("TESTER").Value;
                model.OLTC_TS = ele.Element("OLTC_TS").Value;
                model.CONTACT_NUM = ele.Element("CONTACT_NUM").Value;
                model.TEST_NUM = ele.Element("TEST_NUM").Value;
                model.SPLACE = ele.Element("SPLACE").Value;
                model.OILTEMP = ele.Element("OILTEMP").Value;
                model.TEST_TIME = ele.Element("TEST_TIME").Value;
                model.TEST_TYPE = ele.Element("TEST_TYPE").Value;
                model.GETINFO = ele.Element("GETINFO").Value;
                model.TESTSTAGE = ele.Element("TESTSTAGE").Value;
                model.DJUST = ele.Element("DJUST").Value;
                model.DESCRIBE = ele.Element("DESCRIBE").Value;
                model.SCURRENT = ele.Element("SCURRENT").Value;
                model.ECURRENT = ele.Element("ECURRENT").Value;
                model.TIME_UNIT = ele.Element("TIME_UNIT").Value;
                model.V1 = ele.Element("V1").Value;
                model.V2 = ele.Element("V2").Value;
                model.V3 = ele.Element("V3").Value;
                model.C1 = ele.Element("C1").Value;
                model.C2 = ele.Element("C2").Value;
                model.C3 = ele.Element("C3").Value;
                model.TEST_BASE_C = ele.Element("TEST_BASE_C").Value;
                model.TEST_SINGLE_DOUBLE = ele.Element("TEST_SINGLE_DOUBLE").Value;
                model.DOUBLE_SP = ele.Element("DOUBLE_SP").Value;
                model.DOUBLE_EP = ele.Element("DOUBLE_EP").Value;
                model.SINGLE_P = ele.Element("SINGLE_P").Value;
                model.TEST_ORDER = ele.Element("TEST_ORDER").Value;
                model.COUNT_BASE_C = ele.Element("COUNT_BASE_C").Value;
                model.VOLTAGE = ele.Element("VOLTAGE").Value;

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

                string Current1 = data.Substring(12 + length, 4);
                string Current2 = data.Substring(16 + length, 4);
                string Current3 = data.Substring(20 + length, 4);

                string Vibration1 = data.Substring(0 + length, 4);
                string Vibration2 = data.Substring(4 + length, 4);
                string Vibration3 = data.Substring(8 + length, 4);
                int id = (jinex * count) + i;

                //计算
                newcy1[id] = Algorithm.Instance.Current_Algorithm_Double(Current1, I);
                newcy2[id] = Algorithm.Instance.Current_Algorithm_Double(Current2, I);
                newcy3[id] = Algorithm.Instance.Current_Algorithm_Double(Current3, I);

                newvy1[id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration1);
                newvy2[id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration2);
                newvy3[id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration3);

                //单点宽度计算公式  当前包的 ((序号* 包截取个数) +当前截取序号)/转成毫秒除数
                double newXvalue = (double)((jinex * count) + num + i) / allnum;

                newvx1[id] = newvx2[id] = newvx3[id]
              = newcx1[id] = newcx2[id] = newcx3[id]
              = newXvalue;

            }
        }


        static double v1 = 0;
        static double v2 = 0;
        static double v3 = 0;

        static double c1 = 0;
        static double c2 = 0;
        static double c3 = 0;
        static int AverageCoun = 0;//累加次数
        static int j = 0;//数组所在位置

        /// <summary>
        ///  //转成毫秒除数
        /// </summary>
        static int allnum = 100000;
        /// <summary>        
        /// 存储为数组 arrey
        /// </summary>
        private static void Algorithm_To_Arrey1(XElement model, int jinex, int AverageNum
            )
        {
            //基本宽度
            int num = 1;
            //一组数据总计算次数
            int count = 80;

            string DataSource = model.Element("DataSource").Value;
            string data = DataSource.Substring(8, DataSource.Length - 8);

            for (int i = 0; i < count; i++)
            {
                int length = 24 * i;//截取位置

                string Current1 = data.Substring(12 + length, 4);
                string Current2 = data.Substring(16 + length, 4);
                string Current3 = data.Substring(20 + length, 4);

                string Vibration1 = data.Substring(0 + length, 4);
                string Vibration2 = data.Substring(4 + length, 4);
                string Vibration3 = data.Substring(8 + length, 4);

                #region 计算全部点数部分
                int id = (jinex * count) + i;

                //计算
                newy[0][id] = Algorithm.Instance.Current_Algorithm_Double(Current1, I);
                newy[1][id] = Algorithm.Instance.Current_Algorithm_Double(Current2, I);
                newy[2][id] = Algorithm.Instance.Current_Algorithm_Double(Current3, I);

                newy[3][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration1);
                newy[4][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration2);
                newy[5][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration3);

                if (newy[0][id] > leftMax[0])
                {
                    leftMax[0] = newy[0][id];
                }
                if (newy[1][id] > leftMax[1])
                {
                    leftMax[1] = newy[1][id];
                }
                if (Math.Abs(newy[2][id]) > leftMax[2])
                {
                    leftMax[2] = newy[2][id];
                }
                if (newy[3][id] > leftMax[3])
                {
                    leftMax[3] = newy[3][id];
                }
                if (newy[4][id] > leftMax[4])
                {
                    leftMax[4] = newy[4][id];
                }
                if (newy[5][id] > leftMax[5])
                {
                    leftMax[5] = newy[5][id];
                }

                //单点宽度计算公式  当前包的 ((序号* 包截取个数) +当前截取序号)/转成毫秒除数
                double newXvalue = (double)((jinex * count) + num + i) / allnum;

                newx[0][id]
              = newx[1][id]
              = newx[2][id]
              = newx[3][id]
              = newx[4][id]
              = newx[5][id]
              = newXvalue;
                #endregion

                #region 计算求平均部分

                c1 += newy[0][id];
                c2 += newy[1][id];
                c3 += newy[2][id];

                v1 += newy[3][id];
                v2 += newy[4][id];
                v3 += newy[5][id];

                if (AverageCoun >= AverageNum)
                {
                    newy[6][j] = c1 / AverageNum;
                    newy[7][j] = c2 / AverageNum;
                    newy[8][j] = c3 / AverageNum;
                    newy[9][j] = v1 / AverageNum;
                    newy[10][j] = v2 / AverageNum;
                    newy[11][j] = v3 / AverageNum;

                    double newXvalue1 = (double)(j * AverageCoun) / allnum;
                    newx[6][j]
                  = newx[7][j]
                  = newx[8][j]
                  = newx[9][j]
                  = newx[10][j]
                  = newx[11][j]
                  = newXvalue1;

                    AverageCoun = 0;
                    c1 = 0;
                    c2 = 0;
                    c3 = 0;
                    v1 = 0;
                    v2 = 0;
                    v3 = 0;

                    j++;
                }
                AverageCoun++;

                #endregion
            }
        }


        /// <summary>
        /// 存储转换为数组 
        /// </summary>
        /// <param name="model">存储数据</param>
        /// <param name="jinex">数据在数据集合位置</param>
        /// <param name="LeakNum">偷点数</param>
        private static void Algorithm_To_Arrey2(XElement model, int jinex, int LeakNum
            )
        {
            //基本宽度
            int num = 1;
            //一组数据总计算次数
            int count = 80;

            string DataSource = model.Element("DataSource").Value;
            string data = DataSource.Substring(8, DataSource.Length - 8);

            for (int i = 0; i < count; i++)
            {
                int length = 24 * i;//截取位置

                string Current1 = data.Substring(12 + length, 4);
                string Current2 = data.Substring(16 + length, 4);
                string Current3 = data.Substring(20 + length, 4);

                string Vibration1 = data.Substring(0 + length, 4);
                string Vibration2 = data.Substring(4 + length, 4);
                string Vibration3 = data.Substring(8 + length, 4);

                #region 计算全部点数部分
                int id = (jinex * count) + i;

                //计算
                newy[0][id] = Algorithm.Instance.Current_Algorithm_Double(Current1, I);
                newy[1][id] = Algorithm.Instance.Current_Algorithm_Double(Current2, I);
                newy[2][id] = Algorithm.Instance.Current_Algorithm_Double(Current3, I);

                newy[3][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration1);
                newy[4][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration2);
                newy[5][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration3);

                if (Math.Abs(newy[0][id]) > Math.Abs(leftMax[0]))
                {
                    leftMax[0] = newy[0][id];
                }
                if (Math.Abs(newy[1][id]) > Math.Abs(leftMax[1]))
                {
                    leftMax[1] = newy[1][id];
                }
                if (Math.Abs(newy[2][id]) > Math.Abs(leftMax[2]))
                {
                    leftMax[2] = newy[2][id];
                }
                if (Math.Abs(newy[3][id]) > Math.Abs(leftMax[3]))
                {
                    leftMax[3] = newy[3][id];
                }
                if (Math.Abs(newy[4][id]) > Math.Abs(leftMax[4]))
                {
                    leftMax[4] = newy[4][id];
                }
                if (Math.Abs(newy[5][id]) > Math.Abs(leftMax[5]))
                {
                    leftMax[5] = newy[5][id];
                }

                //单点宽度计算公式  当前包的 ((序号* 包截取个数) +当前截取序号)/转成毫秒除数
                double newXvalue = (double)((jinex * count) + num + i) / allnum;

                newx[0][id]
              = newx[1][id]
              = newx[2][id]
              = newx[3][id]
              = newx[4][id]
              = newx[5][id]
              = newXvalue;

                #endregion

                #region 计算求平均部分

                if (Math.Abs(newy[0][id]) > Math.Abs(c1))
                {
                    c1 = newy[0][id];
                }

                if (Math.Abs(newy[1][id]) > Math.Abs(c2))
                {
                    c2 = newy[1][id];
                }

                if (Math.Abs(newy[2][id]) > Math.Abs(c3))
                {
                    c3 = newy[2][id];
                }

                if (Math.Abs(newy[3][id]) > Math.Abs(v1))
                {
                    v1 = newy[3][id];
                }

                if (Math.Abs(newy[4][id]) > Math.Abs(v2))
                {
                    v2 = newy[4][id];
                }

                if (Math.Abs(newy[5][id]) > Math.Abs(v3))
                {
                    v3 = newy[5][id];
                }

                if (AverageCoun >= LeakNum)
                {
                    newy[6][j] = c1;
                    newy[7][j] = c2;
                    newy[8][j] = c3;
                    newy[9][j] = v1;
                    newy[10][j] = v2;
                    newy[11][j] = v3;

                    double newXvalue1 = (double)(j * AverageCoun) / allnum;
                    newx[6][j]
                 = newx[7][j]
                 = newx[8][j]
                 = newx[9][j]
                 = newx[10][j]
                 = newx[11][j]
                 = newXvalue1;

                    AverageCoun = 0;

                    c1 = 0;
                    c2 = 0;
                    c3 = 0;
                    v1 = 0;
                    v2 = 0;
                    v3 = 0;

                    j++;
                }
                AverageCoun++;

                #endregion
            }
        }

        /// <summary>
        /// xml转数组
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
            out double[] cx3, out double[] cy3,
            out bool IsNotNull
            )
        {
            try
            {
                IsNotNull = true;
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

                XElement ele = root.Element("Test_Plan");

                if (ele == null)
                {
                    I = 10;
                }
                else
                {
                    I = string.IsNullOrEmpty(ele.Element("TEST_BASE_C").Value) ? 10 : Convert.ToInt32(ele.Element("TEST_BASE_C").Value);
                }
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

                if (count == 0)
                {
                    IsNotNull = false;
                }
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

        /// <summary>
        /// xml转平均数组
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cks"></param>
        /// <param name="AverageNum">平均次数</param>
        /// <param name="vx1"></param>
        /// <param name="vy1"></param>
        /// <param name="vx2"></param>
        /// <param name="vy2"></param>
        /// <param name="vx3"></param>
        /// <param name="vy3"></param>
        /// <param name="cx1"></param>
        /// <param name="cy1"></param>
        /// <param name="cx2"></param>
        /// <param name="cy2"></param>
        /// <param name="cx3"></param>
        /// <param name="cy3"></param>
        /// <param name="IsNotNull"></param>
        public static void Xml_To_Average_Array(string path, bool[] cks,
            out double[][] linex, out double[][] liney,
            out bool IsNotNull, int AverageNum = 100
            )
        {
            try
            {
                IsNotNull = true;
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

                IEnumerable<XElement> eles = root.Elements("Xml_Node_Model");

                XElement ele = root.Element("Test_Plan");

                if (ele == null)
                {
                    I = 10;
                }
                else
                {
                    I = string.IsNullOrEmpty(ele.Element("TEST_BASE_C").Value) ? 10 : Convert.ToInt32(ele.Element("TEST_BASE_C").Value);
                }

                //int AverageNum = 100;//多少次去平均
                var dd = (eles.Count() * 80);
                var dd11 = eles.Count();
                int count = (eles.Count() * 80) / AverageNum;
                newvx1 = new double[count]; newvy1 = new double[count];
                newvx2 = new double[count]; newvy2 = new double[count];
                newvx3 = new double[count]; newvy3 = new double[count];

                newcx1 = new double[count]; newcy1 = new double[count];
                newcx2 = new double[count]; newcy2 = new double[count];
                newcx3 = new double[count]; newcy3 = new double[count];


                linex = new double[24][];
                liney = new double[24][];
                if (count == 0)
                {
                    IsNotNull = false;
                }
                int index = 0; //index 为 振动电流 每个点的 索引位置值

                //转成毫秒除数
                int allnum = 100000;
                //基本宽度
                int num = 1;
                //一组数据总计算次数
                int dcount = 80;

                double v1 = 0;
                double v2 = 0;
                double v3 = 0;

                double c1 = 0;
                double c2 = 0;
                double c3 = 0;
                int AverageCoun = 0;//累加次数
                int j = 0;//数组所在位置

                foreach (XElement item in eles)
                {
                    // Algorithm_To_Arrey(item, index);
                    string DataSource = item.Element("DataSource").Value;
                    string data = DataSource.Substring(8, DataSource.Length - 8);

                    for (int i = 0; i < dcount; i++)
                    {
                        int length = 24 * i;//截取位置

                        string Current1 = data.Substring(12 + length, 4);
                        string Current2 = data.Substring(16 + length, 4);
                        string Current3 = data.Substring(20 + length, 4);

                        string Vibration1 = data.Substring(0 + length, 4);
                        string Vibration2 = data.Substring(4 + length, 4);
                        string Vibration3 = data.Substring(8 + length, 4);
                        int id = (index * dcount) + i;

                        //计算
                        c1 += Algorithm.Instance.Current_Algorithm_Double(Current1, I);
                        c2 += Algorithm.Instance.Current_Algorithm_Double(Current2, I);
                        c3 += Algorithm.Instance.Current_Algorithm_Double(Current3, I);

                        v1 += Algorithm.Instance.Vibration_Algorithm_Double(Vibration1);
                        v2 += Algorithm.Instance.Vibration_Algorithm_Double(Vibration2);
                        v3 += Algorithm.Instance.Vibration_Algorithm_Double(Vibration3);

                        //单点宽度计算公式  当前包的 ((序号* 包截取个数) +当前截取序号)/转成毫秒除数
                        AverageCoun++;

                        if (AverageCoun >= AverageNum)
                        {
                            newvy1[j] = v1 / AverageNum;
                            newvy2[j] = v2 / AverageNum;
                            newvy3[j] = v3 / AverageNum;
                            newcy1[j] = c1 / AverageNum;
                            newcy2[j] = c2 / AverageNum;
                            newcy3[j] = c3 / AverageNum;

                            double newXvalue = (double)(j * AverageCoun) / allnum;
                            newvx1[j] = newvx2[j] = newvx3[j]
                          = newcx1[j] = newcx2[j] = newcx3[j]
                          = newXvalue;

                            AverageCoun = 0;
                            c1 = 0;
                            c2 = 0;
                            c3 = 0;
                            v1 = 0;
                            v2 = 0;
                            v3 = 0;

                            j++;
                        }
                        index++;
                    }
                }
                if (AverageCoun > 0)
                {
                    newvy1[j + 1] = v1 / AverageCoun;
                    newvy2[j + 1] = v2 / AverageCoun;
                    newvy3[j + 1] = v3 / AverageCoun;
                    newcy1[j + 1] = c1 / AverageCoun;
                    newcy2[j + 1] = c2 / AverageCoun;
                    newcy3[j + 1] = c3 / AverageCoun;

                    newvx1[j + 1] = newvx2[j + 1] = newvx3[j + 1]
                  = newcx1[j + 1] = newcx2[j + 1] = newcx3[j + 1]
                  = (double)(index * AverageCoun) / allnum;
                }
                linex[0] = newvx1; liney[0] = newvy1;
                linex[1] = newvx2; liney[1] = newvy2;
                linex[2] = newvx3; liney[2] = newvy3;

                linex[3] = newcx1; liney[3] = newcy1;
                linex[4] = newcx2; liney[4] = newcy2;
                linex[5] = newcx3; liney[5] = newcy3;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// xml转平均数组
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cks"></param>
        /// <param name="AverageNum">平均次数</param>
        /// <param name="vx1"></param>
        /// <param name="vy1"></param>
        /// <param name="vx2"></param>
        /// <param name="vy2"></param>
        /// <param name="vx3"></param>
        /// <param name="vy3"></param>
        /// <param name="cx1"></param>
        /// <param name="cy1"></param>
        /// <param name="cx2"></param>
        /// <param name="cy2"></param>
        /// <param name="cx3"></param>
        /// <param name="cy3"></param>
        /// <param name="IsNotNull"></param>
        public static void Xml_To_Average_Array(string path, bool[] cks,
            out double[] vx1, out double[] vy1,
            out double[] vx2, out double[] vy2,
            out double[] vx3, out double[] vy3,
            out double[] cx1, out double[] cy1,
            out double[] cx2, out double[] cy2,
            out double[] cx3, out double[] cy3,
            out bool IsNotNull, int AverageNum = 100
            )
        {
            try
            {
                IsNotNull = true;
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

                IEnumerable<XElement> eles = root.Elements("Xml_Node_Model");

                XElement ele = root.Element("Test_Plan");

                if (ele == null)
                {
                    I = 10;
                }
                else
                {
                    I = string.IsNullOrEmpty(ele.Element("TEST_BASE_C").Value) ? 10 : Convert.ToInt32(ele.Element("TEST_BASE_C").Value);
                }

                //int AverageNum = 100;//多少次去平均
                var dd = (eles.Count() * 80);
                var dd11 = eles.Count();
                int count = (eles.Count() * 80) / AverageNum;
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

                if (count == 0)
                {
                    IsNotNull = false;
                }
                int index = 0; //index 为 振动电流 每个点的 索引位置值

                //转成毫秒除数
                int allnum = 100000;
                //基本宽度
                int num = 1;
                //一组数据总计算次数
                int dcount = 80;

                double v1 = 0;
                double v2 = 0;
                double v3 = 0;

                double c1 = 0;
                double c2 = 0;
                double c3 = 0;
                int AverageCoun = 0;//累加次数
                int j = 0;//数组所在位置

                foreach (XElement item in eles)
                {
                    // Algorithm_To_Arrey(item, index);
                    string DataSource = item.Element("DataSource").Value;
                    string data = DataSource.Substring(8, DataSource.Length - 8);

                    for (int i = 0; i < dcount; i++)
                    {
                        int length = 24 * i;//截取位置

                        string Current1 = data.Substring(12 + length, 4);
                        string Current2 = data.Substring(16 + length, 4);
                        string Current3 = data.Substring(20 + length, 4);

                        string Vibration1 = data.Substring(0 + length, 4);
                        string Vibration2 = data.Substring(4 + length, 4);
                        string Vibration3 = data.Substring(8 + length, 4);
                        int id = (index * dcount) + i;

                        //计算
                        c1 += Algorithm.Instance.Current_Algorithm_Double(Current1, I);
                        c2 += Algorithm.Instance.Current_Algorithm_Double(Current2, I);
                        c3 += Algorithm.Instance.Current_Algorithm_Double(Current3, I);

                        v1 += Algorithm.Instance.Vibration_Algorithm_Double(Vibration1);
                        v2 += Algorithm.Instance.Vibration_Algorithm_Double(Vibration2);
                        v3 += Algorithm.Instance.Vibration_Algorithm_Double(Vibration3);

                        //单点宽度计算公式  当前包的 ((序号* 包截取个数) +当前截取序号)/转成毫秒除数
                        AverageCoun++;

                        if (AverageCoun >= AverageNum)
                        {
                            newvy1[j] = v1 / AverageNum;
                            newvy2[j] = v2 / AverageNum;
                            newvy3[j] = v3 / AverageNum;
                            newcy1[j] = c1 / AverageNum;
                            newcy2[j] = c2 / AverageNum;
                            newcy3[j] = c3 / AverageNum;

                            double newXvalue = (double)(j * AverageCoun) / allnum;
                            newvx1[j] = newvx2[j] = newvx3[j]
                          = newcx1[j] = newcx2[j] = newcx3[j]
                          = newXvalue;

                            AverageCoun = 0;
                            c1 = 0;
                            c2 = 0;
                            c3 = 0;
                            v1 = 0;
                            v2 = 0;
                            v3 = 0;

                            j++;
                        }
                        index++;
                    }
                }
                if (AverageCoun > 0)
                {
                    newvy1[j + 1] = v1 / AverageCoun;
                    newvy2[j + 1] = v2 / AverageCoun;
                    newvy3[j + 1] = v3 / AverageCoun;
                    newcy1[j + 1] = c1 / AverageCoun;
                    newcy2[j + 1] = c2 / AverageCoun;
                    newcy3[j + 1] = c3 / AverageCoun;

                    newvx1[j + 1] = newvx2[j + 1] = newvx3[j + 1]
                  = newcx1[j + 1] = newcx2[j + 1] = newcx3[j + 1]
                  = (double)(index * AverageCoun) / allnum;
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
        static double[][] newx;
        static double[][] newy;

        /// <summary>
        /// 左边轴高低 0 电流1 ，1 电流2 ，2 电流3，3 振动1 ，4 振动2 5 振动3
        /// </summary>
        static double[] leftMax;


        /// <summary>
        /// xml转 平均数组和完整数组
        /// 0 ：完整数据电流1;  1 ：完整数据电流2 ; 2：完整数据电流3;  3：完整数据振动1; 4 ：完整数据振动2;5：完整数据振动3;
        /// 6 ：平均数据电流1;7：平均数据电流2; 8 ：平均数据电流3;9 ：平均数据振动1;10 ：平均数据振动2;11：平均数据振动3;
        /// 12： 平均包络振动1 13：平均包络振动2 14： 完整包络振动1 15：完整包络振动2
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="path">xml路径</param>
        /// <returns></returns>
        public static void Xml_To_AverageAndArray(string path, bool[] cks,
            out double[][] linex, out double[][] liney,
            out bool IsNotNull, out double[] leftMaxs, int AverageNum = 100
            )
        {
            try
            {
                IsNotNull = true;
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

                IEnumerable<XElement> eles = root.Elements("Xml_Node_Model");

                XElement ele = root.Element("Test_Plan");

                if (ele == null)
                {
                    I = 10;
                }
                else
                {
                    I = string.IsNullOrEmpty(ele.Element("TEST_BASE_C").Value) ? 10 : Convert.ToInt32(ele.Element("TEST_BASE_C").Value);
                }

                linex = new double[24][];
                liney = new double[24][];
                #region  完全数据组
                int count = eles.Count() * 80;

                linex[0] = new double[count];
                linex[1] = new double[count];
                linex[2] = new double[count];
                linex[3] = new double[count];
                linex[4] = new double[count];
                linex[5] = new double[count];

                liney[0] = new double[count];
                liney[1] = new double[count];
                liney[2] = new double[count];
                liney[3] = new double[count];
                liney[4] = new double[count];
                liney[5] = new double[count];
                #endregion

                #region 平均数据组

                count = (eles.Count() * 80) / AverageNum;

                linex[6] = new double[count];
                linex[7] = new double[count];
                linex[8] = new double[count];
                linex[9] = new double[count];
                linex[10] = new double[count];
                linex[11] = new double[count];

                liney[6] = new double[count];
                liney[7] = new double[count];
                liney[8] = new double[count];
                liney[9] = new double[count];
                liney[10] = new double[count];
                liney[11] = new double[count];

                #endregion

                newx = linex;
                newy = liney;

                #region 初始化

                v1 = 0;
                v2 = 0;
                v3 = 0;

                c1 = 0;
                c2 = 0;
                c3 = 0;
                AverageCoun = 0;//累加次数
                j = 0;//数组所在位置

                #endregion
                if (count == 0)
                {
                    IsNotNull = false;
                }
                int index = 0; //index 为 振动电流 每个点的 索引位置值

                leftMax = new double[6];

                foreach (XElement item in eles)
                {
                    Algorithm_To_Arrey1(item, index, AverageNum);
                    index++;
                }
                if (AverageCoun > 0)
                {
                    liney[6][j] = c1 / AverageNum;
                    liney[7][j] = c2 / AverageNum;
                    liney[8][j] = c3 / AverageNum;
                    liney[9][j] = v1 / AverageNum;
                    liney[10][j] = v2 / AverageNum;
                    liney[11][j] = v3 / AverageNum;

                    linex[6][j]
                  = linex[7][j]
                  = linex[8][j]
                  = linex[9][j]
                  = linex[10][j]
                  = linex[11][j]
                  = (double)(j * AverageCoun) / allnum;
                }
                leftMaxs = leftMax;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// xml转 偷点数组和完整数组
        /// 0 ：完整数据电流1;  1 ：完整数据电流2 ; 2：完整数据电流3;  3：完整数据振动1; 4 ：完整数据振动2;5：完整数据振动3;
        /// 6 ：平均数据电流1;7：平均数据电流2; 8 ：平均数据电流3;9 ：平均数据振动1;10 ：平均数据振动2;11：平均数据振动3;
        /// 12： 平均包络振动1 13：平均包络振动2 14： 完整包络振动1 15：完整包络振动2
        /// </summary>
        /// <param name="path">储存路径</param>
        /// <param name="cks">展示那几个通道</param>
        /// <param name="linex">X轴数组集合 1 2 3 电路和振动</param>
        /// <param name="liney">Y轴数组集合 1 2 3 电路和振动</param>
        /// <param name="IsNotNull">数据是否为空</param>
        /// <param name="leftMaxs">Y轴坐标卡尺</param>
        /// <param name="LeakNum">偷点漏点数</param>
        public static void Xml_To_Leak_And_Array(string path, bool[] cks,
            out double[][] linex, out double[][] liney,
            out bool IsNotNull, out double[] leftMaxs, int LeakNum = 10
            )
        {
            try
            {
                IsNotNull = true;
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

                IEnumerable<XElement> eles = root.Elements("Xml_Node_Model");

                XElement ele = root.Element("Test_Plan");


                //计算好的数据
                XElement AllEleData = root.Element("Xml_Line_Data");
                if (ele == null)
                {
                    I = 10;
                }
                else
                {
                    //  I = string.IsNullOrEmpty(ele.Element("TEST_BASE_C").Value) ? 10 : Convert.ToInt32(ele.Element("TEST_BASE_C").Value);
                    I = 100; //string.IsNullOrEmpty(ele.Element("TEST_BASE_C").Value) ? 10 : Convert.ToInt32(ele.Element("TEST_BASE_C").Value);
                }

                linex = new double[24][];
                liney = new double[24][];
                #region  完全数据组
                int count = eles.Count() * 80;

                linex[0] = new double[count];
                linex[1] = new double[count];
                linex[2] = new double[count];
                linex[3] = new double[count];
                linex[4] = new double[count];
                linex[5] = new double[count];

                liney[0] = new double[count];
                liney[1] = new double[count];
                liney[2] = new double[count];
                liney[3] = new double[count];
                liney[4] = new double[count];
                liney[5] = new double[count];
                #endregion

                #region 偷点数组

                count = (eles.Count() * 80) / LeakNum;

                linex[6] = new double[count];
                linex[7] = new double[count];
                linex[8] = new double[count];
                linex[9] = new double[count];
                linex[10] = new double[count];
                linex[11] = new double[count];

                liney[6] = new double[count];
                liney[7] = new double[count];
                liney[8] = new double[count];
                liney[9] = new double[count];
                liney[10] = new double[count];
                liney[11] = new double[count];

                #endregion

                newx = linex;
                newy = liney;

                #region 初始化

                v1 = 0;
                v2 = 0;
                v3 = 0;

                c1 = 0;
                c2 = 0;
                c3 = 0;
                AverageCoun = 0;//累加次数
                j = 0;//数组所在位置

                #endregion
                if (count == 0)
                {
                    IsNotNull = false;
                }
                int index = 0; //index 为 振动电流 每个点的 索引位置值

                leftMax = new double[6];
                Xml_Line_Data model = new Xml_Line_Data();

                if (AllEleData == null)
                {

                    foreach (XElement item in eles)
                    {
                        Algorithm_To_Arrey2(item, index, LeakNum);
                        index++;
                    }
                    if (AverageCoun > 0)
                    {
                        liney[6][j] = c1 / LeakNum;
                        liney[7][j] = c2 / LeakNum;
                        liney[8][j] = c3 / LeakNum;
                        liney[9][j] = v1 / LeakNum;
                        liney[10][j] = v2 / LeakNum;
                        liney[11][j] = v3 / LeakNum;

                        linex[6][j]
                      = linex[7][j]
                      = linex[8][j]
                      = linex[9][j]
                      = linex[10][j]
                      = linex[11][j]
                      = (double)(j * AverageCoun) / allnum;
                    }
                    leftMaxs = leftMax;

                    model.leftMaxs = string.Join(",", leftMaxs);
                    model.CY1 = string.Join(",", liney[0]);
                    model.CY2 = string.Join(",", liney[1]);
                    model.CY3 = string.Join(",", liney[2]);

                    model.VY1 = string.Join(",", liney[3]);
                    model.VY2 = string.Join(",", liney[4]);
                    model.VY3 = string.Join(",", liney[5]);
                    model.ALLX = string.Join(",", linex[0]);

                    AllEleData = ToXElement(model);


                    root.Add(AllEleData);
                    root.Save(path);
                }
                else
                {
                    leftMaxs = new double[6];

                    //  model = Deserialize(typeof(Xml_Line_Data), AllEleData.ToString()) as Xml_Line_Data;



                    model.CY1 = AllEleData.Element("CY1").Value;
                    model.CY2 = AllEleData.Element("CY2").Value;
                    model.CY3 = AllEleData.Element("CY3").Value;


                    model.VY1 = AllEleData.Element("VY1").Value;
                    model.VY2 = AllEleData.Element("VY2").Value;
                    model.VY3 = AllEleData.Element("VY3").Value;
                    model.ALLX = AllEleData.Element("ALLX").Value;
                    model.leftMaxs = AllEleData.Element("leftMaxs").Value;
                    if (string.IsNullOrEmpty(model.CY1)
                        || string.IsNullOrEmpty(model.CY2)
                        || string.IsNullOrEmpty(model.CY3))
                    {
                        IsNotNull = false;
                        return;
                    }

                    liney[0] = Array.ConvertAll(model.CY1.Split(','), s => double.Parse(s));
                    liney[1] = Array.ConvertAll(model.CY2.Split(','), s => double.Parse(s));
                    liney[2] = Array.ConvertAll(model.CY3.Split(','), s => double.Parse(s));

                    liney[3] = Array.ConvertAll(model.VY1.Split(','), s => double.Parse(s));
                    liney[4] = Array.ConvertAll(model.VY2.Split(','), s => double.Parse(s));
                    liney[5] = Array.ConvertAll(model.VY3.Split(','), s => double.Parse(s));


                    linex[0] =
                    linex[1] =
                    linex[2] =

                    linex[3] =
                    linex[4] =
                    linex[5] = Array.ConvertAll(model.ALLX.Split(','), s => double.Parse(s));

                    leftMaxs = Array.ConvertAll(model.leftMaxs.Split(','), s => double.Parse(s));

                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        ///// <summary>
        ///// xml转数组  对比数据转换方法
        ///// </summary>
        ///// <typeparam name="T">实体类型</typeparam>
        ///// <param name="path">xml路径</param>
        ///// <returns></returns>
        //public static void Xml_To_Array_Contrast(string path, bool[] cks,
        //    out double[] vx1, out double[] vy1,
        //     out double[] cx1, out double[] cy1
        //    )
        //{
        //    try
        //    {
        //        XDocument document;
        //        try
        //        {
        //            document = XDocument.Load(path);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        XElement root = document.Root;
        //        IEnumerable<XElement> eles = root.Elements("Xml_Node_Model");

        //        XElement ele = root.Element("Test_Plan");

        //        if (ele == null)
        //        {
        //            I = 10;
        //        }
        //        else
        //        {
        //            I = string.IsNullOrEmpty(ele.Element("TEST_BASE_C").Value)
        //                ? 10 : Convert.ToInt32(ele.Element("TEST_BASE_C").Value);
        //        }
        //        int count = eles.Count() * 80;
        //        newvx1 = new double[count]; newvy1 = new double[count];
        //        newcx1 = new double[count]; newcy1 = new double[count];
        //        vx1 = new double[count]; vy1 = new double[count];
        //        cx1 = new double[count]; cy1 = new double[count];

        //        int index = 0; //index 为索引值
        //        foreach (XElement item in eles)
        //        {
        //            Algorithm_To_Arrey_Contrast(item, cks, index);
        //            index++;
        //        }
        //        vx1 = newvx1; vy1 = newvy1;
        //        cx1 = newcx1; cy1 = newcy1;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 多维数组 储存数据之间 间距
        /// </summary>
        static int newDatalength;
        /// <summary>
        /// xml转数组   平均数组和完整数组 对比数据转换方法  
        /// 0， 电流1； 1，电流2；
        /// 4， 振动1； 5，振动2；  
        /// </summary>                                      
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="path">xml路径</param>
        /// <param name="Datalength">0，第一条， 1 第二条 </param>
        /// <returns></returns>
        public static void Xml_To_Array_AverageAndContrast(string path, bool[] cks,
             double[][] linex, double[][] liney, out double[] leftMaxs
            , int Datalength, int AverageNum = 100
            )
        {
            try
            {
                leftMax = new double[6];
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
                IEnumerable<XElement> eles = root.Elements("Xml_Node_Model");

                XElement ele = root.Element("Test_Plan");

                if (ele == null)
                {
                    I = 10;
                }
                else
                {
                    I = string.IsNullOrEmpty(ele.Element("TEST_BASE_C").Value)
                        ? 10 : Convert.ToInt32(ele.Element("TEST_BASE_C").Value);
                }
                newDatalength = Datalength;
                int count = eles.Count() * 80;

                linex[0 + Datalength] = new double[count];
                linex[3 + Datalength] = new double[count];
                liney[0 + Datalength] = new double[count];
                liney[3 + Datalength] = new double[count];

                //count = count / AverageNum;

                //linex[2 + Datalength] = new double[count];
                //linex[3 + Datalength] = new double[count];
                //liney[2 + Datalength] = new double[count];
                //liney[3 + Datalength] = new double[count];

                newx = linex;
                newy = liney;
                AverageCoun = 0;//累加次数
                j = 0;//数组所在位置
                int index = 0; //index 为索引值
                foreach (XElement item in eles)
                {
                    Algorithm_To_Arrey_AverageAndContrast(item, cks, index, AverageNum);
                    index++;
                }
                if (AverageCoun > 0)
                {
                    //  var dd = newy[2 + newDatalength][j-1];
                    // // var dd = newy[2 + newDatalength][j];
                    //  newy[2 + newDatalength][j] = c1 / AverageNum;
                    //  newy[3 + newDatalength][j] = v1 / AverageNum;

                    //  double newXvalue1 = (double)(j * AverageCoun) / allnum;
                    //  newx[2 + newDatalength][j]
                    //= newx[3 + newDatalength][j]
                    //= newXvalue1;
                }
                leftMaxs = leftMax;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// <summary>        
        ///// 对比度提取算法
        ///// </summary>
        //private static void Algorithm_To_Arrey_Contrast(XElement model, bool[] cks, int jinex
        //    )
        //{
        //    //转成毫秒除数
        //    int allnum = 100000;
        //    //基本宽度
        //    int num = 1;
        //    //一组数据总计算次数
        //    int count = 80;

        //    string DataSource = model.Element("DataSource").Value;
        //    string data = DataSource.Substring(8, DataSource.Length - 8);

        //    for (int i = 0; i < count; i++)
        //    {
        //        int length = 24 * i;//截取位置

        //        int id = (jinex * count) + i;
        //        if (cks[0])
        //        {
        //            //计算
        //            string Current1 = data.Substring(12 + length, 4);
        //            newcy1[id] = Algorithm.Instance.Current_Algorithm_Double(Current1, I);
        //        }
        //        else if (cks[1])
        //        {
        //            string Current2 = data.Substring(16 + length, 4);
        //            newcy1[id] = Algorithm.Instance.Current_Algorithm_Double(Current2, I);
        //        }
        //        else if (cks[2])
        //        {
        //            string Current3 = data.Substring(20 + length, 4);
        //            newcy1[id] = Algorithm.Instance.Current_Algorithm_Double(Current3, I);
        //        }

        //        if (cks[3])
        //        {
        //            string Vibration1 = data.Substring(0 + length, 4);
        //            newvy1[id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration1);
        //        }
        //        else if (cks[4])
        //        {
        //            string Vibration2 = data.Substring(4 + length, 4);
        //            newvy1[id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration2);
        //        }
        //        else if (cks[5])
        //        {
        //            string Vibration3 = data.Substring(8 + length, 4);
        //            newvy1[id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration3);
        //        }
        //        //单点宽度计算公式  当前包的 ((序号* 包截取个数) +当前截取序号)/转成毫秒除数
        //        double newXvalue = (double)((jinex * count) + num + i) / allnum;
        //        newvx1[id]
        //      = newcx1[id]
        //      = newXvalue;








        //    }
        //}

        /// <summary>        
        /// 对比度提取算法 平均和完整
        /// </summary>
        private static void Algorithm_To_Arrey_AverageAndContrast(XElement model, bool[] cks, int jinex, int AverageNum)
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

                int id = (jinex * count) + i;
                if (cks[0])
                {
                    //计算
                    string Current1 = data.Substring(12 + length, 4);
                    newy[0 + newDatalength][id] = Algorithm.Instance.Current_Algorithm_Double(Current1, I);
                    if (leftMax[0] < Math.Abs(newy[0 + newDatalength][id]))
                    {
                        leftMax[0] = Math.Abs(newy[0 + newDatalength][id]);
                    }
                }
                else if (cks[1])
                {
                    string Current2 = data.Substring(16 + length, 4);
                    newy[0 + newDatalength][id] = Algorithm.Instance.Current_Algorithm_Double(Current2, I);
                    if (leftMax[1] < Math.Abs(newy[0 + newDatalength][id]))
                    {
                        leftMax[1] = Math.Abs(newy[0 + newDatalength][id]);
                    }
                }
                else if (cks[2])
                {
                    string Current3 = data.Substring(20 + length, 4);
                    newy[0 + newDatalength][id] = Algorithm.Instance.Current_Algorithm_Double(Current3, I);
                    if (leftMax[2] < newy[0 + newDatalength][id])
                    {
                        leftMax[2] = newy[0 + newDatalength][id];
                    }
                }

                if (cks[3])
                {
                    string Vibration1 = data.Substring(0 + length, 4);
                    newy[3 + newDatalength][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration1);
                    if (leftMax[3] < newy[3 + newDatalength][id])
                    {
                        leftMax[3] = newy[3+ newDatalength][id];
                    }
                }
                else if (cks[4])
                {
                    string Vibration2 = data.Substring(4 + length, 4);
                    newy[3 + newDatalength][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration2);
                    if (leftMax[4] < Math.Abs(newy[3 + newDatalength][id]))
                    {
                        leftMax[4] = Math.Abs(newy[3+ newDatalength][id]);
                    }
                }
                else if (cks[5])
                {
                    string Vibration3 = data.Substring(8 + length, 4);
                    newy[3 + newDatalength][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration3);
                    if (leftMax[5] < Math.Abs(newy[3 + newDatalength][id]))
                    {
                        leftMax[5] = Math.Abs(newy[3 + newDatalength][id]);
                    }
                }
                //单点宽度计算公式  当前包的 ((序号* 包截取个数) +当前截取序号)/转成毫秒除数
                double newXvalue = (double)((jinex * count) + num + i) / allnum;
                newx[0 + newDatalength][id]
              = newx[3 + newDatalength][id]
              = newXvalue;

                #region 计算求平均部分

                //c1 += newy[0 + newDatalength][id];
                //v1 += newy[1 + newDatalength][id];

                //if (AverageCoun >= AverageNum)
                //{
                //    newy[2 + newDatalength][j] = c1 / AverageNum;
                //    newy[3 + newDatalength][j] = v1 / AverageNum;

                //    double newXvalue1 = (double)(j * AverageCoun) / allnum;
                //    newx[2 + newDatalength][j]
                //  = newx[3 + newDatalength][j]
                //  = newXvalue1;

                //    AverageCoun = 0;
                //    c1 = 0;
                //    v1 = 0;
                //    j++;
                //}
                //  AverageCoun++;

                #endregion
            }
        }

        /// <summary>        
        /// 对比度提取算法 平均和完整
        /// </summary>
        private static void Algorithm_To_Arrey_AverageAndContrast_Copy(XElement model, bool[] cks, int jinex, int AverageNum)
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

                int id = (jinex * count) + i;
                if (cks[0])
                {
                    //计算
                    string Current1 = data.Substring(12 + length, 4);
                    newy[0 + newDatalength][id] = Algorithm.Instance.Current_Algorithm_Double(Current1, I);

                    var ddd = Math.Abs(newy[0 + newDatalength][id]);
                    if (leftMax[0] < Math.Abs(newy[0 + newDatalength][id]))
                    {
                        leftMax[0] = Math.Abs(newy[0 + newDatalength][id]);
                    }

                }
                else if (cks[1])
                {
                    string Current2 = data.Substring(16 + length, 4);
                    newy[0 + newDatalength][id] = Algorithm.Instance.Current_Algorithm_Double(Current2, I);

                    if (leftMax[1] < Math.Abs(newy[0 + newDatalength][id]))
                    {
                        leftMax[1] = Math.Abs(newy[0 + newDatalength][id]);
                    }

                }
                else if (cks[2])
                {
                    string Current3 = data.Substring(20 + length, 4);
                    newy[0 + newDatalength][id] = Algorithm.Instance.Current_Algorithm_Double(Current3, I);
                    //if (leftMax[2] > Math.Abs(newy[0 + newDatalength][id]))
                    //{
                    //    leftMax[2] = Math.Abs(newy[0 + newDatalength][id]);
                    //}
                    if (leftMax[2] < newy[0 + newDatalength][id])
                    {
                        leftMax[2] = newy[0 + newDatalength][id];
                    }
                }

                if (cks[3])
                {
                    string Vibration1 = data.Substring(0 + length, 4);
                    newy[1 + newDatalength][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration1);
                    //if (leftMax[3] > Math.Abs(newy[1 + newDatalength][id]))
                    //{
                    //    leftMax[3] = Math.Abs(newy[1 + newDatalength][id]);
                    //}

                    if (leftMax[3] < newy[1 + newDatalength][id])
                    {
                        leftMax[3] = newy[1 + newDatalength][id];
                    }
                }
                else if (cks[4])
                {
                    string Vibration2 = data.Substring(4 + length, 4);
                    newy[1 + newDatalength][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration2);
                    if (leftMax[4] < Math.Abs(newy[1 + newDatalength][id]))
                    {
                        leftMax[4] = Math.Abs(newy[1 + newDatalength][id]);
                    }
                }
                else if (cks[5])
                {
                    string Vibration3 = data.Substring(8 + length, 4);
                    newy[1 + newDatalength][id] = Algorithm.Instance.Vibration_Algorithm_Double(Vibration3);
                    if (leftMax[5] < Math.Abs(newy[1 + newDatalength][id]))
                    {
                        leftMax[5] = Math.Abs(newy[1 + newDatalength][id]);
                    }
                }
                //单点宽度计算公式  当前包的 ((序号* 包截取个数) +当前截取序号)/转成毫秒除数
                double newXvalue = (double)((jinex * count) + num + i) / allnum;
                newx[0 + newDatalength][id]
              = newx[1 + newDatalength][id]
              = newXvalue;

                #region 计算求平均部分

                c1 += newy[0 + newDatalength][id];
                v1 += newy[1 + newDatalength][id];

                if (AverageCoun >= AverageNum)
                {
                    newy[2 + newDatalength][j] = c1 / AverageNum;
                    newy[3 + newDatalength][j] = v1 / AverageNum;

                    double newXvalue1 = (double)(j * AverageCoun) / allnum;
                    newx[2 + newDatalength][j]
                  = newx[3 + newDatalength][j]
                  = newXvalue1;

                    AverageCoun = 0;
                    c1 = 0;
                    v1 = 0;
                    j++;
                }
                AverageCoun++;

                #endregion
            }
        }

        #region 反序列化


        /// <summary>
        /// 从某一XML文件反序列化到某一类型
        /// </summary>
        /// <param name="filePath">待反序列化的XML文件名称</param>
        /// <param name="type">反序列化出的</param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                    throw new ArgumentNullException(filePath + " not Exists");

                using (System.IO.StreamReader reader = new System.IO.StreamReader(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    T ret = (T)xs.Deserialize(reader);
                    return ret;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }



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