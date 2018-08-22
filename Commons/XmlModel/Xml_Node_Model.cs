using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Commons.XmlModel
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
        /// 数据
        /// </summary>
        [XmlElement(ElementName = "Data")]
        public IList<Xml_Element_Model> Data { get; set; }
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
        /// 震动1
        /// </summary>
        /// 
        [XmlAttribute("V1")]
        public string V1 { get; set; }
        /// <summary>
        /// 震动2
        /// </summary>
        /// 
        [XmlAttribute("V2")]
        public string V2 { get; set; }
        /// <summary>
        /// 震动3
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
}
