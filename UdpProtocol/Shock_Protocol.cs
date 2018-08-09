using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpProtocol
{
    /// <summary>
    /// 震动协议
    /// </summary>
    public class Shock_Protocol
    {

        /// <summary>       
        /// 握手命令，连接仪器时     
        /// </summary>
        private byte[] handshake = BitConverter.GetBytes(0x80000001);
        public byte[] _1_CMD_HANDSHAKE
        {
            get
            {
                return handshake;
            }
        }
        /// <summary>
        /// 开始测试, 回读INT 型状态数据 
        /// </summary>
        private byte[] start_measure = BitConverter.GetBytes(0x80000002);
        public byte[] _2_CMD_STARTMEASURE
        {
            get
            {
                return start_measure;
            }
        }

        /// <summary>   
        /// 停止测试, 回读INT 型状态数据  
        /// </summary>
        private byte[] stop_measure = BitConverter.GetBytes(0x80000003);
        public byte[] _3_CMD_STOPMEASURE
        {
            get
            {
                return stop_measure;
            }
        }

        /// <summary>
        /// 暂停测试, 回读INT 型状态数据  
        /// </summary>
        private byte[] pause_measure = BitConverter.GetBytes(0x80000004);
        public byte[] _4_CMD_PAUSEMEASUER
        {
            get
            {
                return pause_measure;
            }
        }

        /// <summary>
        /// 讲触发波形的一些参数返回给PC 
        /// </summary>
        private byte[] get_trig_param = BitConverter.GetBytes(0x80000007);
        public byte[] _5_CMD_GETTRIGPARAM
        {
            get
            {
                return get_trig_param;
            }
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        private byte[] vision_type = BitConverter.GetBytes(0x80000008);
        public byte[] _6_CMD_GETVERSION
        {
            get
            {
                return vision_type;
            }
        }
        /// <summary>
        /// 轮询获取状态信息
        /// </summary>
        private byte[] state_type = BitConverter.GetBytes(0x80000006);
        public byte[] _6_CMD_GETSTATE
        {
            get
            {
                return state_type;
            }
        }
        /// <summary>
        /// 轮询获取状态信息
        /// </summary>
        private byte[] lose_page = BitConverter.GetBytes(0x80000009);
        public byte[] _7_CMD_RECALL
        {
            get
            {
                return lose_page;
            }
        }
        /// <summary>
        /// 轮询直流游标位置信息
        /// </summary>
        private byte[] DC_position = BitConverter.GetBytes(0x8000000A);
        public byte[] CMD_GETDCTRIGPOS
        {
            get
            {
                return DC_position;
            }
        }

        public byte[] AllCMD { get; set; }

    }
}
