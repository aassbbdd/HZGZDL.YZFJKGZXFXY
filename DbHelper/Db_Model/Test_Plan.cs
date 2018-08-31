using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHelper.Db_Model
{
    /// <summary>
    /// 测试计划
    /// </summary>
    public class Test_Plan
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 子元素
        /// </summary>
        public string PARENTID { get; set; }

        /// <summary>
        /// 设备名字
        /// </summary>
        public string DVNAME { get; set; }

        /// <summary>
        /// 设备位置
        /// </summary>
        public string DVPOSITION { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DVID { get; set; }

        /// <summary>
        /// 有载调压操作手
        /// </summary>
        public string TESTER { get; set; }

        /// <summary>
        /// OLTC型号
        /// </summary>
        public string OLTC_TS { get; set; }

        /// <summary>
        /// 触头数量
        /// </summary>
        public string CONTACT_NUM { get; set; }

        /// <summary>
        /// 操作次数
        /// </summary>
        public string TEST_NUM { get; set; }

        /// <summary>
        /// 开始位置
        /// </summary>
        public string SPLACE { get; set; }

        /// <summary>
        /// 油温（摄氏度）
        /// </summary>
        public string OILTEMP { get; set; }

        /// <summary>
        /// 测试时间
        /// </summary>
        public string TEST_TIME { get; set; }

        /// <summary>
        /// 测试类型 1 在线,2,离线（使用所有类型开关触头）3，离线（使用部分类型开关触头）
        /// </summary>
        public string TEST_TYPE { get; set; }


        /// <summary>
        /// 测试阶段 1,检修前测试2,临时测试3,检修后测试4，故障后测试
        /// </summary>
        public string TESTSTAGE { get; set; }

        /// <summary>
        /// 技术调整 1,注油2,机械调节3,触头维护
        /// </summary>
        public string DJUST { get; set; }

        /// <summary>
        /// 详情描述
        /// </summary>
        public string DESCRIBE { get; set; }

        /// <summary>
        /// 采样信息1，触发后采样2，一秒为单位采样
        /// </summary>
        public string GETINFO { get; set; }

        /// <summary>
        /// 开始电流
        /// </summary>
        public string SCURRENT { get; set; }
        /// <summary>
        /// 结束电流
        /// </summary>
        public string ECURRENT { get; set; }

        /// <summary>
        /// 以秒为单位采样（时间单位  秒）
        /// </summary>
        public string TIME_UNIT { get; set; }

        public string V1 { get; set; }
        public string V2 { get; set; }
        public string V3 { get; set; }
        public string C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }
        /// <summary>
        /// 修改添加标识 1新增2修改
        /// </summary>
        public string ISEDIT { get; set; }

    }
}

