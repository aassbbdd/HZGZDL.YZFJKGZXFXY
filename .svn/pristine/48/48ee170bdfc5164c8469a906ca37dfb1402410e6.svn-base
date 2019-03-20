using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Socket_Server
{
    public class Udp_Server
    {
        public static void Start()
        {

            int recv;
            byte[] data = new byte[1024];

            //构建UDP服务器

            //得到本机IP，设置UDP端口号         
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 8001);
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //绑定网络地址
            newsock.Bind(ipep);

            //Console.WriteLine("This is a Server, host name is {0}", Dns.GetHostName());

            //等待客户机连接
            //Console.WriteLine("Waiting for a client");

            //得到客户机IP
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            EndPoint Remote = (EndPoint)(sender);
            recv = newsock.ReceiveFrom(data, ref Remote);
            //Console.WriteLine("Message received from {0}: ", Remote.ToString());
            //Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));

            //客户机连接成功后，发送欢迎信息
            string welcome = "Welcome ! ";

            //字符串与字节数组相互转换
            data = Encoding.ASCII.GetBytes(welcome);

            //发送信息
            newsock.SendTo(data, data.Length, SocketFlags.None, Remote);
            while (true)
            {
                data = new byte[1024];
                //发送接受信息
                recv = newsock.ReceiveFrom(data, ref Remote);
                //Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
                newsock.SendTo(data, recv, SocketFlags.None, Remote);
            }
        }
    }
}