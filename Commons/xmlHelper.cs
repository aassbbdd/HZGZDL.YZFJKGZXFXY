using Commons.XmlModel;
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

namespace Commons
{
    public static class XmlHelper
    {
        #region Fields and Properties
        /// <summary>
        /// 文件夹名字
        /// </summary>
        public static string Xml_Path = ConfigurationManager.ConnectionStrings["xml_path"].ConnectionString.ToString();
        public enum XmlType
        {
            File,
            String
        }
        static XElement xele;
        static string path = "";
        #endregion

        #region  Methods

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
                string path = AppDomain.CurrentDomain.BaseDirectory + Xml_Path + "\\";
                if (!FileHelper.IsFileExist(path))//验证文件是否存在
                {
                    FileHelper.CreateDirectoy(path);//创建文件夹
                }
                path = path + "\\" + name + ".xml"; //拼接文件路径
                if (!FileHelper.IsFileExist(path))//验证文件是否存在
                {
                    xele = new XElement("root");
                    xele.Save(path);
                }
            }
            catch (Exception exception)
            {
                //LogHelper.LogError("Error! ", exception);
            }
        }

        public static void Init()
        {
             path = AppDomain.CurrentDomain.BaseDirectory + XmlHelper.Xml_Path + "\\测试1.xml";
            if (!FileHelper.IsFileExist(path))//验证文件是否存在
            {
                FileHelper.CreateDirectoy(path);//创建文件夹
            }
             xele = XElement.Load(path);
        }


        #region 新增节点


        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>        
        /// <param name="xmlNode">要插入的Xml节点</param>
        public static void In(Xml_Node_Model model)
        {

            XElement root = xele.Element("root");
            XElement element = ToXElement(model);
            XElement data = new XElement("Data");
            foreach (var item in model.Data)
            {
                XElement e = new XElement("Id", item.Id);
                data.Add(e);
                data.Add(new XElement("V1", item.V1));
                data.Add(new XElement("V2", item.V2));
                data.Add(new XElement("V3", item.V3));

                data.Add(new XElement("C1", item.C1));
                data.Add(new XElement("C2", item.C2));
                data.Add(new XElement("C3", item.C3));

            }
            element.Add(data);
            xele.Add(element);
          //  xele.Save(path);
        }
        public static void Save()
        {
            if (xele != null)
            {
                xele.Save(path);
                Init();
            }
        }


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


        public static string ToXml<T>(T entity) where T : new()
        {
            if (entity == null)
            {
                return string.Empty;
            }

            XElement element = ToXElement<T>(entity);

            return element.ToString();
        }

        public static XElement ToXElement<T>(T entity)
        {
          
            if (entity == null)
            {
                return null;
            }
            PropertyInfo[] properties = typeof(T).GetProperties();
            object id = properties[0].GetValue(entity, null);
            XElement element = new XElement(typeof(T).Name+id.ToString());
           
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

        #endregion
    }
}