using Commons;
using Socket_Server.Udp_Event;
using System;
using System.Collections.Concurrent;
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
        /// <summary>
        /// 波形图数据
        /// </summary>
        public static event EventHandler<byte[]> udp_Event;
        /// <summary>
        /// 波形图数据
        /// </summary>
        public static event EventHandler<Udp_EventArgs> udp_Event2;
        /// <summary>
        /// 一般协议发送回调
        /// </summary>
        public static event EventHandler<Udp_EventArgs> udp_Event_Kind;
        private static int outTime = 3; //单位：秒

        private static bool continueLoop = true;//控制接收开关
        private static Thread threadSendStert;//接收协议线程
        public static ConcurrentQueue<byte[]> Porint_List = new ConcurrentQueue<byte[]>();
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
            sendUdpClient = new UdpClient(0);//0默认随机  1024*1024; 665535 1048576
            sendUdpClient.Client.ReceiveBufferSize = 1024 * 1024;
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
                //threadSendStert = new Thread(SendMessages1);
                threadSendStert = new Thread(SendMessages2);

                threadSendStert.Priority = ThreadPriority.Highest;

                threadSendStert.IsBackground = true;
                threadSendStert.Start(sendMsg);



            }
        }
        /// <summary>
        /// 一般发送协议使用
        /// </summary>
        /// <param name="sendMsg"></param>
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
                        eventArgs.Hearder = receiveCmd.Substring(0, 8);
                        eventArgs.Msg = receiveCmd;
                        udp_Event_Kind("", eventArgs);
                        continueLoop = false;
                    }
                    else
                    {
                        receiveCmd = string.Empty;
                    }
                }
            }
            if (continueLoop && DateTimeUtil.DateTimeDiff(startTime, DateTime.Now) > outTime * 1000)
            {
                Udp_EventArgs eventArgs = new Udp_EventArgs();
                eventArgs.Msg = "连接超时";
                eventArgs.Hearder = "-1";
                udp_Event_Kind("", eventArgs);
                return;
            }
        }

        /// <summary>
        /// 测试前发送停止协议
        /// </summary>
        /// <param name="sendMsg"></param>
        public static void SendMessagesStop(string sendMsg, IPEndPoint ipPoint)
        {
            try
            {
                byte[] sendbytes = ProtocolUtil.strToToHexByte(sendMsg);
                sendUdpClient.Send(sendbytes, sendbytes.Length, ipPoint);
                if (sendUdpClient.Client.Available > 0)
                {
                    IPEndPoint receivePoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] recData = sendUdpClient.Receive(ref receivePoint);
                    string reMessage = ProtocolUtil.byteToHexStr(recData);
                    ListToText.Instance.WriteListToTextFile1(reMessage);
                }
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
            }
        }

        /// <summary>
        /// 发送开始 测试协议（震动 电流数据 用）
        /// </summary>
        /// <param name="sendMsg"></param>
        private static void SendMessages1(object sendMsg)
        {
            try
            {
                string receiveCmd = string.Empty; //接收到信息

                string sendmessage = (string)sendMsg;
                byte[] sendbytes = ProtocolUtil.strToToHexByte(sendmessage);

                sendUdpClient.Send(sendbytes, sendbytes.Length, ipPoint);
                IPEndPoint receivePoint = new IPEndPoint(IPAddress.Any, 0);
                DateTime startTime = DateTime.Now;
                int i = 0;
                while (continueLoop && DateTimeUtil.DateTimeDiff(startTime, DateTime.Now) <= outTime * 1000)
                {
                    if (sendUdpClient.Client.Available > 0)
                    {
                        Udp_EventArgs eventArgs = new Udp_EventArgs();
                        /* 接收UPD返回数据，并进行处理 */
                        byte[] recData = sendUdpClient.Receive(ref receivePoint);
                        receiveCmd = ProtocolUtil.byteToHexStr(recData);

                        if (receiveCmd.Substring(0, 4) == "0909")//判断是测试回复协议
                        {
                            sendmessage = "30FF" + receiveCmd.Substring(4, 4);
                            sendbytes = ProtocolUtil.strToToHexByte(sendmessage);
                            sendUdpClient.Send(sendbytes, sendbytes.Length, ipPoint);

                            eventArgs.Msg = receiveCmd;
                            startTime = DateTime.Now;
                            eventArgs.Hearder = "0909";
                            eventArgs.AddDate = startTime.ToString("yyyyMMdd HH:mm:ss.fff");
                            udp_Event2("", eventArgs);

                            i++;
                            //ListToText.Instance.WriteListToTextFile1(sendmessage);
                        }

                    }
                }
                if (continueLoop && DateTimeUtil.DateTimeDiff(startTime, DateTime.Now) > outTime * 1000)
                {
                    Udp_EventArgs eventArgs = new Udp_EventArgs();
                    eventArgs.Msg = "连接超时";
                    eventArgs.Hearder = "-1";
                    udp_Event_Kind("", eventArgs);
                }
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
            }
        }

        /// <summary>
        /// 发送开始 测试协议（震动 电流数据 用）
        /// </summary>
        /// <param name="sendMsg"></param>
        private static void SendMessages2(object sendMsg)
        {
            try
            {
                string receiveCmd = string.Empty; //接收到信息

                string sendmessage = (string)sendMsg;
                byte[] sendbytes = ProtocolUtil.strToToHexByte(sendmessage);

                IPEndPoint receivePoint = new IPEndPoint(IPAddress.Any, 0);

                //确认收到数据
                byte[] confirm = ProtocolUtil.strToToHexByte("30FF");
                int i = 0;

                Udp_EventArgs eventArgs = new Udp_EventArgs();
                //开始发送送钱先发停止命令
                SendMessagesStop("05550555", ipPoint);
                DateTime startTime = DateTime.Now;
                sendUdpClient.Send(sendbytes, sendbytes.Length, ipPoint);
                int j = 0;
                while (continueLoop)// && DateTimeUtil.DateTimeDiff(startTime, DateTime.Now) <= outTime * 1000)
                {
                    if (sendUdpClient.Client.Available > 0)
                    {
                        byte[] recData = sendUdpClient.Receive(ref receivePoint);
                        //高位左移8位拼成 16进制数字
                        int num = ((recData[0] << 8) | recData[1]);
                        switch (num)
                        {
                            case 0x00000909:
                                {
                                    udp_Event("", recData);
                                    i++;
                                    j++;
                                    if (i == 100)// 设备刚开启时，很容易断开，设备运行一段时间后，又正常了。
                                    {
                                        sendUdpClient.Send(confirm, confirm.Length, ipPoint);
                                        i = 0;//重置
                                        startTime = DateTime.Now;
                                    }
                                    continue;
                                }
                            //case 0x00000355:
                            //    continue;
                            //case 0x00000555:
                            //    continueLoop = false;
                            //    //停止命令
                            //    break; 
                            case 0x00005050:
                                continue;
                            default:
                                udp_Event("", recData);
                                continue;
                        }
                        //if (recData.Length > 10)
                        //{
                        //    udp_Event("", recData);
                        //    i++;
                        //    if (i == 100)// 设备刚开启时，很容易断开，设备运行一段时间后，又正常了。
                        //    {
                        //        sendUdpClient.Send(confirm, confirm.Length, ipPoint);
                        //        i = 0;//重置
                        //        startTime = DateTime.Now;
                        //    }
                        //}
                        //else
                        //{
                        //    udp_Event("", recData);
                        //    continue;
                        //}
                    }
                    else if (DateTimeUtil.DateTimeDiff(startTime, DateTime.Now) > outTime * 1000)
                    {
                        //ListToText.Instance.WriteListToTextFile1("超时退出");
                        //ListToText.Instance.WriteListToTextFile1("次数：" + i.ToString());
                        eventArgs.Msg = "连接超时";
                        eventArgs.Hearder = "-1";
                        udp_Event_Kind("", eventArgs);
                        sendUdpClient.Close();
                        break;
                    }
                }
                //if (continueLoop && DateTimeUtil.DateTimeDiff(startTime, DateTime.Now) > outTime * 1000)
                //{
                //    ListToText.Instance.WriteListToTextFile1("次数：" + i.ToString());
                //    eventArgs.Msg = "连接超时";
                //    eventArgs.Hearder = "-1";
                //    udp_Event_Kind("", eventArgs);
                //    sendUdpClient.Close();
                //}
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
                sendUdpClient.Close();

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
