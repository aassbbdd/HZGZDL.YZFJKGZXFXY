using Commons;
using Socket_Server.Udp_Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socket_Server
{
    public class SendMessage
    {

        /// <summary>
        /// UDP服务
        /// </summary>
        private static UdpClient sendUdpClient;
        /// <summary>
        /// IP 和端口
        /// </summary>
        private static IPEndPoint ipPoint;
        public static event EventHandler<Udp_EventArgs> udp_Event;
        private static int outTime = 3; //单位：秒

        private static bool continueLoop = true;//控制接收开关
        private static Thread threadSendStert;//接收协议线程

        #region 发送消息

        /// 发送消息
        /// </summary>
        /// <param name="sendMsg"></param>
        /// <param name="sendIp"></param>
        /// <param name="sendPort"></param>
        public static void SendMsgStart(string sendMsg, IPEndPoint point)
        {
            continueLoop = true;
            //给参数赋值
            ipPoint = point;//选择发送模式
            //固定为匿名模式（套接字绑定的端口由系统自动分配）
            sendUdpClient = new UdpClient(0);//0默认随机 

            if (sendMsg != "03550355")
            {
                //启动发送线程
                Thread threadSend = new Thread(SendMessages);
                threadSend.IsBackground = true;
                threadSend.Start(sendMsg);
                if (sendMsg == "05550555")
                {
                    if (threadSendStert != null)
                    {
                        continueLoop = false;
                        //Thread.Sleep(100);
                        //threadSendStert.Abort();
                    }
                }
            }
            else
            {
                if (threadSendStert != null)
                {
                    Thread.Sleep(100);
                    threadSendStert.Abort();
                }
               
                    //启动发送线程  开始测试时一直保持运行
                    threadSendStert = new Thread(SendMessages1);
                    threadSendStert.IsBackground = true;
                    threadSendStert.Start(sendMsg);
               
            }
        }
        private static void SendMessages(object sendMsg)
        {
            string receiveCmd = string.Empty; //接收到信息
            string sendmessage = (string)sendMsg;
            byte[] sendbytes = ProtocolUtil.strToToHexByte(sendmessage);

            sendUdpClient.Send(sendbytes, sendbytes.Length, ipPoint);
            IPEndPoint receivePoint = new IPEndPoint(IPAddress.Any, 0);
            /* 接收UDP数据，为防止，接收不到数据，添加while循环接收，并判断超时时间 */
            DateTime startTime = DateTime.Now;
            while (continueLoop && DateTimeUtil.DateTimeDiff(startTime, DateTime.Now) <= outTime * 1000)
            {
                Thread.Sleep(20);
                if (sendUdpClient != null)
                {
                    if (sendUdpClient.Client.Available > 0)
                    {
                        /* 接收UPD返回数据，并进行处理 */
                        byte[] recData = sendUdpClient.Receive(ref receivePoint);
                        receiveCmd = ProtocolUtil.byteToHexStr(recData);

                        Udp_EventArgs eventArgs = new Udp_EventArgs();
                        eventArgs.Msg = receiveCmd;
                        udp_Event("", eventArgs);
                        continueLoop = false;
                    }
                    else
                    {
                        receiveCmd = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 发送开始 测试协议
        /// </summary>
        /// <param name="sendMsg"></param>
        private static void SendMessages1(object sendMsg)
        {
            string receiveCmd = string.Empty; //接收到信息

            string sendmessage = (string)sendMsg;
            byte[] sendbytes = ProtocolUtil.strToToHexByte(sendmessage);

            sendUdpClient.Send(sendbytes, sendbytes.Length, ipPoint);
            IPEndPoint receivePoint = new IPEndPoint(IPAddress.Any, 0);

            while (continueLoop)
            {
                if (sendUdpClient.Client.Available > 0)
                {
                    /* 接收UPD返回数据，并进行处理 */
                    byte[] recData = sendUdpClient.Receive(ref receivePoint);
                    receiveCmd = ProtocolUtil.byteToHexStr(recData);

                    Udp_EventArgs eventArgs = new Udp_EventArgs();
                    eventArgs.Msg = receiveCmd;

                    if (!string.IsNullOrEmpty(receiveCmd))//判断 回复包不为空时调用
                    {
                        if (receiveCmd.Substring(0, 4) == "0909")//判断是测试回复协议
                        {
                            eventArgs.Hearder = "0909";
                            sendmessage = "30FF" + receiveCmd.Substring(4, 4);
                            sendbytes = ProtocolUtil.strToToHexByte(sendmessage);
                            sendUdpClient.Send(sendbytes, sendbytes.Length, ipPoint);
                            //string path = AppDomain.CurrentDomain.BaseDirectory;
                            //ListToText.Instance.WriteListToTextFile("", path);
                        }
                    }
                    udp_Event("", eventArgs);
                }
            }
        }

        #endregion

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            try
            {
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
