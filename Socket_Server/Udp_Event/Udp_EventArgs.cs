using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Server.Udp_Event
{
    public class Udp_EventArgs : EventArgs
    {
        //存储一个字符串  
        public string Msg
        {
            get;
            set;
        }

        ////头部  
        public string Hearder
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddDate
        {
            get;
            set;
        }
    }
}
