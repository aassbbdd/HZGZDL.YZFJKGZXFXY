using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Socket_Server.Udp_Event;

namespace Socket_Server
{
    public class ReceiveMessage
    {
        private static UdpClient receiveUdpClient;
        public static event EventHandler<Udp_EventArgs> udp_Event;
        public static string UdpType;//C客户端 S 服务端
        #region 接受消息

        public static void ReceiveStart(string udpType, IPEndPoint point)
        {
            UdpType = udpType;
            if (receiveUdpClient != null)
            {
                CloseReceiveUdpClient();
            }
            else
            {
                receiveUdpClient = new UdpClient(point);
                //启动接受线程
                Thread threadReceive = new Thread(ReceiveMessages);
                threadReceive.IsBackground = true;
                threadReceive.Start();
            }
        }

        /// <summary>
        /// 处理接受数据
        /// </summary>
        private static void ReceiveMessages()
        {
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    //关闭receiveUdpClient时此句会产生异常
                    byte[] receiveBytes = receiveUdpClient.Receive(ref remoteIPEndPoint);

                    string message = Encoding.Default.GetString(receiveBytes, 0, receiveBytes.Length);

                    #region 返回到服务器前端

                    Udp_EventArgs msg = new Udp_EventArgs();


                    #endregion
                    if (UdpType == "S")
                    {
                        #region 收到回复
                        msg.Msg = "收到：" + message; remoteIPEndPoint.Port = 9001;
                        SendMessage.SendMsgStart(msg.Msg, remoteIPEndPoint);

                        #endregion
                    }
                    else
                    {
                        msg.Msg = message;
                    }

                    udp_Event("", msg);

                }
                catch (Exception ex)
                {
                    CloseReceiveUdpClient();

                    return;
                }
            }
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public static void CloseReceiveUdpClient()
        {
            receiveUdpClient.Close();
        }
        #endregion


    }
}
