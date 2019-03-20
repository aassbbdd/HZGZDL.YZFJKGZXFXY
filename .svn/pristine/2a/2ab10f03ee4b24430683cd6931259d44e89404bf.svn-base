using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udp_Agreement.Model
{
    public class DataModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 头部数据序号
        /// </summary>
        public string head { get; set; }
        /// <summary>
        /// 完整协议
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 未处理过的解析数据
        /// </summary>
        public List<Vibration_Current> old_data { get; set; }

        /// <summary>
        /// 处理过的解析数据
        /// </summary>
        public List<Vibration_Current> new_data { get; set; }

    }

    /// <summary>
    /// 震动电流实体
    /// </summary>
    public class Vibration_Current
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public string width { get; set; }

        /// <summary>
        /// 震动1
        /// </summary>
        public string Vibration1 { get; set; }
        /// <summary>
        /// 震动2
        /// </summary>
        public string Vibration2 { get; set; }
        /// <summary>
        /// 震动3
        /// </summary>
        public string Vibration3 { get; set; }

        /// <summary>
        /// 电流1
        /// </summary>
        public string Current1 { get; set; }
        /// <summary>
        /// 电流2
        /// </summary>
        public string Current2 { get; set; }
        /// <summary>
        /// 电流3
        /// </summary>
        public string Current3 { get; set; }

    }
}
