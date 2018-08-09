using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Socket_Server;
using Socket_Server.Udp_Event;
using Udp_Agreement;
using Udp_Agreement.Model;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.0.143"), 8001);
        Tester_Agreement agreement = new Tester_Agreement();
        List<DataModel> list = new List<DataModel>();
        public Form1()
        {
            InitializeComponent();
            ReceiveStart();//后台起服务
        }

        #region 按键

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.rtBox.Text = "";
        }

        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            sendUdp(this.txtMsg.Text);
        }

        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            sendUdp(agreement._2_CMD_STARTTESTER);
        }

        //停止测试
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            sendUdp(agreement._3_CMD_STOPTESTER);
        }

        /// <summary>
        /// 发送协议
        /// </summary>
        private void sendUdp(string msg)
        {
            if (!string.IsNullOrEmpty(this.txtIp.Text) && !string.IsNullOrEmpty(this.txtPorit.Text))
            {
                ipep = new IPEndPoint(IPAddress.Parse(txtIp.Text), Convert.ToInt32(txtPorit.Text));
            }
            SendMessage.SendMsgStart(msg, ipep);
        }
        #endregion

        #region 调用方法
        /// <summary>
        /// 启动服务
        /// </summary>
        private void ReceiveStart()
        {
            string ip = SendMessage.GetLocalIP();
            IPEndPoint sipep = new IPEndPoint(IPAddress.Parse(ip), 4001);
            ReceiveMessage.ReceiveStart("C", sipep);
            Event_Bind();
        }

        /// <summary>
        /// 绑定注册事件
        /// </summary>
        private void Event_Bind()
        {
            try
            {
                ReceiveMessage.udp_Event += new EventHandler<Udp_EventArgs>(Run);
                SendMessage.udp_Event += new EventHandler<Udp_EventArgs>(Run);
            }
            catch (Exception e)
            {
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
                    if (!string.IsNullOrEmpty(e.Msg))
                    {
                        if (!string.IsNullOrEmpty(e.Hearder))
                        {
                            //截取返回数据
                            string data = e.Msg.Substring(8, e.Msg.Length - 8);
                            string heard = e.Msg.Substring(4, 4);
                            DataModel model = new DataModel();
                            model.id = Convert.ToInt32(heard,16);
                            model.text = e.Msg;
                            model.old_data = new List<Vibration_Current>();
                            model.new_data = new List<Vibration_Current>();
                            for (int i = 0; i < 80; i++)
                            {
                                Vibration_Current vmodel = new Vibration_Current();
                                Vibration_Current vmodel1 = new Vibration_Current();
                                int length = 24 * i;

                                vmodel.Vibration1 = data.Substring(0 + length, 4);
                                vmodel.Vibration2 = data.Substring(4 + length, 4);
                                vmodel.Vibration3 = data.Substring(8 + length, 4);
                                vmodel.Current1 = data.Substring(12 + length, 4);
                                vmodel.Current2 = data.Substring(16 + length, 4);
                                vmodel.Current3 = data.Substring(20 + length, 4);


                                //计算
                                vmodel1.Vibration1 = Algorithm.Instance.Vibration_Algorithm(vmodel.Vibration1);
                                vmodel1.Vibration2 = Algorithm.Instance.Vibration_Algorithm(vmodel.Vibration2);
                                vmodel1.Vibration3 = Algorithm.Instance.Vibration_Algorithm(vmodel.Vibration3);
                                vmodel1.Current1 = Algorithm.Instance.Current_Algorithm(vmodel.Current1);
                                vmodel1.Current2 = Algorithm.Instance.Current_Algorithm(vmodel.Current2);
                                vmodel1.Current3 = Algorithm.Instance.Current_Algorithm(vmodel.Current3);

                                model.old_data.Add(vmodel);
                                model.new_data.Add(vmodel1);
                            }

                            list.Add(model);
                        }
                        this.rtBox.Text = e.Msg + "\r\n" + this.rtBox.Text;
                    }
                    else
                    {
                        this.rtBox.Text = "回复数据为空！\r\n" + this.rtBox.Text;
                    }
                }));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        #endregion

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.rtBox.Text = Convert.ToInt32("FFFF", 16).ToString() + "\r\n" + this.rtBox.Text;
            this.rtBox.Text = Algorithm.Instance.Current_Algorithm(Convert.ToInt32("FFFF", 16)).ToString()
                + "\r\n" + this.rtBox.Text;


        }
    }
}
