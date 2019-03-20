using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DbHelper.Db_Model
{
    /// <summary>
    /// 测试计划
    /// </summary>
    [Serializable]
    [XmlRoot("Test_Plan")]
    public class Test_Plan
    {
        /// <summary>
        /// 序号
        /// </summary>
        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }

        /// <summary>
        /// 子元素
        /// </summary>
        [XmlAttribute(AttributeName = "PARENTID")]
        public string PARENTID { get; set; }

        /// <summary>
        /// 设备名字
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "DVNAME")]
        public string DVNAME { get; set; }

        /// <summary>
        /// 设备位置
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "DVPOSITION")]
        public string DVPOSITION { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "DVID")]
        public string DVID { get; set; }

        /// <summary>
        /// 有载调压操作手
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "TESTER")]
        public string TESTER { get; set; }

        /// <summary>
        /// OLTC型号
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "OLTC_TS")]
        public string OLTC_TS { get; set; }

        /// <summary>
        /// 档位总数量
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "CONTACT_NUM")]
        public string CONTACT_NUM { get; set; }

        /// <summary>
        /// 操作次数
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "TEST_NUM")]
        public string TEST_NUM { get; set; }

        /// <summary>
        /// 开始位置
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "SPLACE")]
        public string SPLACE { get; set; }

        /// <summary>
        /// 油温（摄氏度）
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "OILTEMP")]
        public string OILTEMP { get; set; }

        /// <summary>
        /// 测试时间
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "TEST_TIME")]
        public string TEST_TIME { get; set; }

        /// <summary>
        /// 测试类型 1 在线,2,离线（使用所有类型开关触头）3，离线（使用部分类型开关触头）
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "TEST_TYPE")]
        public string TEST_TYPE { get; set; }

        /// <summary>
        /// 测试阶段 1,检修前测试2,临时测试3,检修后测试4，故障后测试
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "TESTSTAGE")]
        public string TESTSTAGE { get; set; }

        /// <summary>
        /// 技术调整 1,注油2,机械调节3,触头维护
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "DJUST")]
        public string DJUST { get; set; }

        /// <summary>
        /// 详情描述
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "DESCRIBE")]
        public string DESCRIBE { get; set; }

        /// <summary>
        /// 采样信息1，触发后采样2，一秒为单位采样
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "GETINFO")]
        public string GETINFO { get; set; }

        /// <summary>
        /// 开始电流
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "SCURRENT")]
        public string SCURRENT { get; set; }
        /// <summary>
        /// 结束电流
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "ECURRENT")]
        public string ECURRENT { get; set; }

        /// <summary>
        /// 以秒为单位采样（时间单位  秒）
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "TIME_UNIT")]
        public string TIME_UNIT { get; set; }

        [XmlAttribute(AttributeName = "V1")]
        public string V1 { get; set; }
        [XmlAttribute(AttributeName = "V2")]
        public string V2 { get; set; }
        [XmlAttribute(AttributeName = "V3")]
        public string V3 { get; set; }
        [XmlAttribute(AttributeName = "C1")]
        public string C1 { get; set; }
        [XmlAttribute(AttributeName = "C2")]
        public string C2 { get; set; }
        [XmlAttribute(AttributeName = "C3")]
        public string C3 { get; set; }
        /// <summary>
        /// 修改添加标识 1新增2修改
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "ISEDIT")]
        public string ISEDIT { get; set; }

        /// <summary>
        ///测试基准电流
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "TEST_BASE_C")]
        public string TEST_BASE_C { get; set; }


        /// <summary>
        /// 连续测试开始位置
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "DOUBLE_SP")]
        public string DOUBLE_SP { get; set; }

        /// <summary>
        /// 连续测试结束位置
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "DOUBLE_EP")]
        public string DOUBLE_EP { get; set; }

        /// <summary>
        /// 单次测试位置
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "SINGLE_P")]
        public string SINGLE_P { get; set; }

        /// <summary>
        /// 单次测试顺序 0前往后 1 后往前
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "TEST_ORDER")]
        public string TEST_ORDER { get; set; }

        /// <summary>
        /// 计算基准电流  1 电流1 2 电流2  3 电流3
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "COUNT_BASE_C")]
        public string COUNT_BASE_C { get; set; }

        /// <summary>
        /// 电压
        /// </summary>
        [XmlAttribute(AttributeName = "VOLTAGE")]
        public string VOLTAGE { get; set; }

        /// <summary>
        /// 单侧还是连续测试 1连续 2单次
        /// </summary>
        /// 
        [XmlAttribute(AttributeName = "TEST_SINGLE_DOUBLE")]
        public string TEST_SINGLE_DOUBLE { get; set; }

    }
}