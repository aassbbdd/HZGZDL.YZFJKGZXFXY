using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udp_Agreement
{
    public class Tester_Agreement
    {
        private string heartbeat = "00FF00FF";
        /// <summary>       
        /// 心跳  
        /// </summary>
        public string _1_CMD_HEARTBEAT
        {
            get
            {
                return heartbeat;
            }
        }

        private string starttester = "03550355";
        /// <summary>       
        /// 开始测试  
        /// </summary>
        public string _2_CMD_STARTTESTER
        {
            get
            {
                return starttester;
            }
        }

        private string stoptester = "05550555";
        /// <summary>       
        /// 停止测试  
        /// </summary>
        public string _3_CMD_STOPTESTER
        {
            get
            {
                return stoptester;
            }
        }

        private string receiveend = "5050";
        /// <summary>       
        /// 数据接收完成命令  
        /// </summary>
        public string _4_CMD_RECEIVEEND
        {
            get
            {
                return stoptester;
            }
        }
    }
}
