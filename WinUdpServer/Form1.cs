using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Socket_Server;
using Socket_Server.Udp_Event;
using Udp_Agreement;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 协议对应参数
        /// </summary>
        Tester_Agreement agreement = new Tester_Agreement();
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.1.5"), 4000);
        public Form1()
        {
            InitializeComponent();


            string ip = GetLocalIP();
            ipep = new IPEndPoint(IPAddress.Parse(ip), 4000);
            //ipep = new IPEndPoint(IPAddress.Any, 0);


            lbIp.Text = ipep.Address.ToString();
            lbPort.Text = ipep.Port.ToString();

            ReceiveMessage.ReceiveStart("S",ipep);

            Event_Bind();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ReceiveMessage.SendMsgStart(agreement._2_CMD_STARTTESTER, ipep);

            //ReceiveMessage.ReceiveStart("S",ipep);
        }

        /// <summary>
        /// 绑定注册事件
        /// </summary>
        private void Event_Bind()
        {
            try
            {
                ReceiveMessage.udp_Event += new EventHandler<Udp_EventArgs>(Run);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 事件回调执行方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run(object sender, Udp_EventArgs e)
        {
            try
            {
                //异步方法
                this.Invoke(new ThreadStart(delegate ()
                {
                    this.listBox.Items.Add(e.Msg);
                }));

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            ReceiveMessage.CloseReceiveUdpClient();
        }
        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public string GetLocalIP()
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.listBox.Items.Clear();
        }
    }
}
