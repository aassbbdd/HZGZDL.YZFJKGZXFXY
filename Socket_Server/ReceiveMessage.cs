using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Commons;
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
                Thread threadReceive = new Thread(ReceiveMessages1);
                threadReceive.IsBackground = true;
                threadReceive.Start();
            }
        }
        static IPEndPoint point11= new IPEndPoint(IPAddress.Any, 0);

        // <summary>
        /// UDP服务
        /// </summary>
        private static UdpClient sendUdpClient;
        /// 发送消息
        /// </summary>
        /// <param name="sendMsg"></param>
        /// <param name="sendIp"></param>
        /// <param name="sendPort"></param>
        public static void SendMsgStart(string sendMsg, IPEndPoint point)
        {
            sendUdpClient = new UdpClient(0);
            string receiveCmd = string.Empty; //接收到信息
            string sendmessage = (string)sendMsg;
            byte[] sendbytes = ProtocolUtil.strToToHexByte(sendmessage);

            sendUdpClient.Send(sendbytes, sendbytes.Length, point);
            IPEndPoint receivePoint = new IPEndPoint(IPAddress.Any, 0);
            
            while (true)
            {
                if (sendUdpClient.Client.Available > 0)
                {
                    Udp_EventArgs eventArgs = new Udp_EventArgs();
                    /* 接收UPD返回数据，并进行处理 */
                    byte[] recData = sendUdpClient.Receive(ref receivePoint);
                    receiveCmd = ProtocolUtil.byteToHexStr(recData);

                    if (receiveCmd.Substring(0, 4) == "0909")//判断是测试回复协议
                    {
 
 
                        udp_Event("", eventArgs);

                     }

                }
            }

        }

        /// <summary>
        /// 处理接受数据
        /// </summary>
        private static void ReceiveMessages1()
        {
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                if (receiveUdpClient.Client.Available > 0)
                {
                    //关闭receiveUdpClient时此句会产生异常
                    byte[] receiveBytes = receiveUdpClient.Receive(ref remoteIPEndPoint);

                    string message = Encoding.Default.GetString(receiveBytes, 0, receiveBytes.Length);

                    Udp_EventArgs msg = new Udp_EventArgs();
                    msg.Msg = message;
                    udp_Event("", msg);
                }
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
