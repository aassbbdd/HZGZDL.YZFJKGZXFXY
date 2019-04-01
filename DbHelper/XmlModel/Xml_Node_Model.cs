using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbHelper.XmlModel
{
    /// <summary>
    ///  节点模型
    /// </summary>
    [Serializable]
    [XmlRoot("Xml_Node_Model")]
    public class Xml_Node_Model
    {
        /// <summary>
        /// 序号
        /// </summary>
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// 原始数据
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "DataSource")]
        public string DataSource { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [XmlAttribute(AttributeName = "AddDate")]
        public string AddDate { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        [XmlElement(ElementName = "Data")]
        public IList<Xml_Element_Model> Data { get; set; }


        /// <summary>
        /// 原始数据数据
        /// </summary>
        [XmlElement(ElementName = "Data")]
        public Xml_Line_Data Line_Data { get; set; }

    }

    /// <summary>
    /// 解析数据
    /// </summary>
    public class Xml_Element_Model
    {
        /// <summary>
        /// 组序号
        /// </summary>
        /// 
        [XmlAttribute("Id")]
        public string Id { get; set; }
        /// <summary>
        /// 振动1
        /// </summary>
        /// 
        [XmlAttribute("V1")]
        public string V1 { get; set; }
        /// <summary>
        /// 振动2
        /// </summary>
        /// 
        [XmlAttribute("V2")]
        public string V2 { get; set; }
        /// <summary>
        /// 振动3
        /// </summary>
        /// 
        [XmlAttribute("V3")]
        public string V3 { get; set; }

        /// <summary>
        /// 电流1
        /// </summary>
        /// 
        [XmlAttribute("C1")]
        public string C1 { get; set; }

        /// <summary> 
        /// 电流2     
        /// </summary>   
        /// 
        [XmlAttribute("C2")]
        public string C2 { get; set; }

        /// <summary> 
        /// 电流3     
        /// </summary>    
        /// 
        [XmlAttribute("C3")]
        public string C3 { get; set; }

        /// <summary> 
        /// Xwitdh     X轴宽度 
        /// </summary>    
        /// 
        [XmlAttribute("Xwitdh")]
        public string Xwitdh { get; set; }
    }

    /// <summary>
    /// 完成的数据数据
    /// </summary>
    public class Xml_Line_Data
    {
        #region 原始数据
        /// <summary>
        /// 振动Y1
        /// </summary>
        [XmlAttribute("VY1")]
        public string VY1 { get; set; }

        /// <summary>
        /// 振动Y2
        /// </summary>
        [XmlAttribute("VY2")]
        public string VY2 { get; set; }

        /// <summary>
        /// 振动Y3
        /// </summary>
        [XmlAttribute("VY3")]
        public string VY3 { get; set; }

        /// <summary>
        /// 电流Y1
        /// </summary>

        [XmlAttribute("CY1")]
        public string CY1 { get; set; }

        /// <summary> 
        /// 电流Y2     
        /// </summary>   
        /// 
        [XmlAttribute("CY2")]
        public string CY2 { get; set; }

        /// <summary> 
        /// 电流Y3     
        /// </summary>    
        /// 
        [XmlAttribute("CY3")]
        public string CY3 { get; set; }

        #endregion


        #region 包络数据
        /// <summary>
        /// 振动Y1
        /// </summary>
        [XmlAttribute("Envelope_VY1")]
        public string Envelope_VY1 { get; set; }

        /// <summary>
        /// 振动Y2
        /// </summary>
        [XmlAttribute("Envelope_VY2")]
        public string Envelope_VY2 { get; set; }

        /// <summary>
        /// 振动Y3
        /// </summary>
        [XmlAttribute("Envelope_VY3")]
        public string Envelope_VY3 { get; set; }

        /// <summary>
        /// 电流Y1
        /// </summary>

        [XmlAttribute("Envelope_CY1")]
        public string Envelope_CY1 { get; set; }

        /// <summary> 
        /// 电流Y2     
        /// </summary>   
        /// 
        [XmlAttribute("Envelope_CY2")]
        public string Envelope_CY2 { get; set; }

        /// <summary> 
        /// 电流Y3     
        /// </summary>    
        /// 
        [XmlAttribute("Envelope_CY3")]
        public string Envelope_CY3 { get; set; }

        #endregion 

        /// <summary>
        /// 所有X轴
        /// </summary>
        [XmlAttribute("ALLX")]
        public string ALLX { get; set; }

        /// <summary>
        /// 左边最高值
        /// </summary>
        [XmlAttribute("leftMaxs")]
        public string leftMaxs { get; set; }
     
    }
}
