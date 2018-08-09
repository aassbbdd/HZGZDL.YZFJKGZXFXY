using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Commons;
using Socket_Server;
using Socket_Server.Udp_Event;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
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
            Init();//初始化图表
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
            list = new List<DataModel>();
            vcdt.Rows.Clear();
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
            this.simpleButton6.Enabled = true;
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
            this.simpleButton6.Enabled = false;
        }

        /// <summary>
        /// 查看转换换算数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.rtBox.Text = Convert.ToInt32("FFFF", 16).ToString() + "\r\n" + this.rtBox.Text;
            this.rtBox.Text = Algorithm.Instance.Current_Algorithm(Convert.ToInt32("FFFF", 16)).ToString()
                + "\r\n" + this.rtBox.Text;


        }

        /// <summary>
        /// 生成 txt 文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                if (list.Count > 0)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    ListToText.Instance.WriteListToTextFile(list, path);
                    ListToText.Instance.WriteListToTextFile1(list, path);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ex:" + ex.ToString());
            }
        }
        /// <summary>
        /// 动态刷新图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = GetData2();
            foreach (DataRow dr in dt.Rows)
            {
                DataRow dr1 = vcdt.NewRow();
                dr1["id"] = vcdt.Rows.Count + 1;
                dr1["vibration1"] = dr["vibration1"];
                dr1["vibration2"] = dr["vibration2"];
                dr1["vibration3"] = dr["vibration3"];

                dr1["current1"] = dr["current1"];
                dr1["current2"] = dr["current2"];
                dr1["current3"] = dr["current3"];
                dr1["width"] = (vcdt.Rows.Count * num) + num;
                vcdt.Rows.Add(dr1);
            }
            tChart.Series.Clear();
            Chart_Data_Bind(vcdt);

            AddCustomAxis(tChart.Series.Count);
            tChart.Refresh();
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
                            string head = e.Msg.Substring(4, 4);
                            DataModel model = new DataModel();
                            model.id = Convert.ToInt32(head, 16);
                            model.head = head;
                            model.text = e.Msg;
                            model.old_data = new List<Vibration_Current>();
                            model.new_data = new List<Vibration_Current>();
                            for (int i = 0; i < 80; i++)
                            {
                                Vibration_Current vmodel = new Vibration_Current();
                                Vibration_Current vmodel1 = new Vibration_Current();
                                int length = 24 * i;

                                vmodel.Current1 = data.Substring(0 + length, 4);
                                vmodel.Current2 = data.Substring(4 + length, 4);
                                vmodel.Current3 = data.Substring(8 + length, 4);
                                vmodel.Vibration1 = data.Substring(12 + length, 4);
                                vmodel.Vibration2 = data.Substring(16 + length, 4);
                                vmodel.Vibration3 = data.Substring(20 + length, 4);


                                //计算
                                vmodel1.Current1 = Algorithm.Instance.Current_Algorithm(vmodel.Current1);
                                vmodel1.Current2 = Algorithm.Instance.Current_Algorithm(vmodel.Current2);
                                vmodel1.Current3 = Algorithm.Instance.Current_Algorithm(vmodel.Current3);

                                vmodel1.Vibration1 = Algorithm.Instance.Vibration_Algorithm(vmodel.Vibration1);
                                vmodel1.Vibration2 = Algorithm.Instance.Vibration_Algorithm(vmodel.Vibration2);
                                vmodel1.Vibration3 = Algorithm.Instance.Vibration_Algorithm(vmodel.Vibration3);

                                model.old_data.Add(vmodel);
                                model.new_data.Add(vmodel1);
                            }

                            list.Add(model);

                            if (list.Count > 12502)//取12500个包感觉不对
                            {
                                sendUdp(agreement._3_CMD_STOPTESTER);
                                this.simpleButton6.Enabled = true;
                            }

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

        #region  多图表代码
        #region 图表参数配置
        private TChart tChart = new TChart();
        private int space = 5;//计算坐标百分比参数
        DataTable vcdt;//电流震动数据源

        DataTable vdt;//震动数据源
        DataTable cdt;//电流数据源

        double num = 0.00001;//数据显示宽度  1秒=0.000001秒* 800微秒  0.0008毫秒 0.000001 * 800
         //1个数据包 800微秒 0.0008  一个包80个数据点  每个点 10微秒 0.00001秒
        double databoxnum = (10 * 1000) / 8;

        #endregion

        //10秒
        //换算数据包个数 = 10秒/800微秒 10/0.0008
        /// <summary>
        /// 添加若干个自定义坐标轴
        /// </summary>
        /// <param name="count"></param>
        private void AddCustomAxis(int count)
        {
            List<BaseLine> listBaseLine = new List<BaseLine>();
            for (int i = 0; i < tChart.Series.Count; i++)
            {
                listBaseLine.Add((BaseLine)tChart.Series[i]);
            }

            double single = (100 - space * (count + 2)) / (count + 1);//单个坐标轴的百分比
            tChart.Axes.Left.StartPosition = space;
            tChart.Axes.Left.EndPosition = tChart.Axes.Left.EndPosition = tChart.Axes.Left.StartPosition + single;
            tChart.Axes.Left.StartEndPositionUnits = PositionUnits.Percent;

            listBaseLine[0].CustomVertAxis = tChart.Axes.Left;

            double startPosition = tChart.Axes.Left.StartPosition;
            double endPosition = tChart.Axes.Left.EndPosition;
            Steema.TeeChart.Axis axis;
            for (int i = 0; i < count; i++)
            {
                axis = new Steema.TeeChart.Axis();
                startPosition = endPosition + space;
                endPosition = startPosition + single;
                axis.StartPosition = startPosition;
                axis.EndPosition = endPosition;
                axis.AutomaticMaximum = false;//最大刻度禁用
                axis.AutomaticMinimum = false;//最小刻度禁用

                axis.Maximum = 20;
                axis.Minimum = -20;
              

                tChart.Axes.Custom.Add(axis);

 
                listBaseLine[i].CustomVertAxis = axis;
            }
        }

        private void Init()
        {
            tChart.Dock = DockStyle.Fill;
            tChart.Aspect.View3D = false;
            tChart.Legend .Visible = true;//显示/隐藏线的注释 
            //tChart.Panel.Gradient.Visible = true;
          
            tChart.Legend.LegendStyle = LegendStyles.Series;
            tChart.Axes.Bottom.Labels.ValueFormat = "0.00";

            tChart.Axes.Bottom.AutomaticMaximum = true;//禁用自增长 
            tChart.Axes.Bottom.SetMinMax(0, 10);
            tChart.Axes.Bottom.Increment = 1;//控制X轴 刻度的增量

            //tChart.Axes.Bottom.Labels.ExactDateTime = true;
            tChart.Axes.Bottom.Labels.Angle = 100;
           // tChart.Axes.Animated = false;
            panel1.Controls.Add(tChart);

            vcdt = GetData2();
            Chart_Data_Bind(vcdt);
        }

        /// <summary>
        /// 绑定图表
        /// </summary>
        private void Chart_Data_Bind(DataTable dt)
        {
            //震动1路 vline1
            Line vline1 = new Line();
            tChart.Series.Add(vline1);
            vline1.Title = string.Format("震动曲线{0}", 1);
            vline1.YValues.DataMember = "vibration1";
            vline1.XValues.DataMember = "width";
            vline1.DataSource = dt;

            ////震动2路 vline2
            //Line vline2 = new Line();
            //tChart.Series.Add(vline2);
            //vline2.Title = string.Format("震动曲线{0}", 2);
            //vline2.YValues.DataMember = "vibration2";
            //vline2.XValues.DataMember = "width";
            //vline2.DataSource = dt;

            ////震动3路 vline3
            //Line vline3 = new Line();
            //tChart.Series.Add(vline3);
            //vline3.Title = string.Format("震动曲线{0}", 3);
            //vline3.YValues.DataMember = "vibration3";
            //vline3.XValues.DataMember = "width";
            //vline3.DataSource = dt;

            //电流1路 cline1
            Line cline1 = new Line();
            tChart.Series.Add(cline1);
            cline1.Title = string.Format("电流曲线{0}", 1);
            cline1.YValues.DataMember = "current1";
            cline1.XValues.DataMember = "width";
            cline1.DataSource = dt;

            ////电流2路 cline2
            //Line cline2 = new Line();
            //tChart.Series.Add(cline2);
            //cline2.Title = string.Format("电流曲线{0}", 2);
            //cline2.YValues.DataMember = "current2";
            //cline2.XValues.DataMember = "width";
            //cline2.DataSource = dt;

            ////电流3路 cline3
            //Line cline3 = new Line();
            //tChart.Series.Add(cline3);
            //cline3.Title = string.Format("电流曲线{0}", 3);
            //cline3.YValues.DataMember = "current3";
            //cline3.XValues.DataMember = "width";
            //cline3.DataSource = dt;

            AddCustomAxis(tChart.Series.Count);
        }

        private List<BaseLine> GetVisibleSeries()
        {
            List<BaseLine> visibleSeries = new List<BaseLine>();
            for (int i = 0; i < tChart.Series.Count; i++)
            {
                tChart.Series[i].CustomVertAxis = null;
                if (tChart.Series[i].Visible)
                {
                    visibleSeries.Add((BaseLine)tChart.Series[i]);
                }
            }
            return visibleSeries;
        }

        private DataTable GetData()
        {


            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("width");
            dt.Columns.Add("vibration");
            // double num = 0.01;
            int cs = 1000;
            for (int i = 0; i < cs; i++)
            {

                DataRow dr = dt.NewRow();
                dr["id"] = dt.Rows.Count + 1;

                dr["width"] = (dt.Rows.Count * num) + num;
                double b;
                if (i % 2 > 0)
                {
                    b = 2;
                }
                else
                {
                    b = -2;
                }


                dr["vibration"] = b;
                dt.Rows.Add(dr);

            }
            for (int i = 0; i < cs; i++)
            {

                DataRow dr = dt.NewRow();
                dr["id"] = dt.Rows.Count + 1;

                dr["width"] = (dt.Rows.Count * num) + num;
                double b;
                if (i % 2 > 0)
                {
                    b = 5;
                }
                else
                {
                    b = -5;
                }


                dr["vibration"] = b;
                dt.Rows.Add(dr);

            }
            for (int i = 0; i < cs; i++)
            {

                DataRow dr = dt.NewRow();
                dr["id"] = dt.Rows.Count + 1;

                dr["width"] = (dt.Rows.Count * num) + num;
                double b;
                if (i % 2 > 0)
                {
                    b = 1;
                }
                else
                {
                    b = 1;
                }


                dr["vibration"] = b;
                dt.Rows.Add(dr);

            }
            for (int i = 0; i < cs; i++)
            {

                DataRow dr = dt.NewRow();
                dr["id"] = dt.Rows.Count + 1;

                dr["width"] = (dt.Rows.Count * num) + num;
                double b;
                if (i % 2 > 0)
                {
                    b = 6;
                }
                else
                {
                    b = -6;
                }


                dr["vibration"] = b;
                dt.Rows.Add(dr);

            }
            for (int i = 0; i < cs; i++)
            {

                DataRow dr = dt.NewRow();
                dr["id"] = dt.Rows.Count + 1;

                dr["width"] = (dt.Rows.Count * num) + num;
                double b;
                if (i % 2 > 0)
                {
                    b = 2;
                }
                else
                {
                    b = -2;
                }


                dr["vibration"] = b;
                dt.Rows.Add(dr);

            }
            return dt;
        }

        private DataTable GetData2()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("width");
            dt.Columns.Add("vibration1");//震动1
            dt.Columns.Add("vibration2");//震动2
            dt.Columns.Add("vibration3");//震动3
            dt.Columns.Add("current1");//电流1
            dt.Columns.Add("current2");//电流2
            dt.Columns.Add("current3");//电流3

            if (list.Count > 0)
            {
                foreach (DataModel item in list)
                {
                    for (int i = 0; i < item.new_data.Count; i++)
                    {
                        Vibration v = new Vibration();
                        DataRow dr = dt.NewRow();

                        double d1 = (dt.Rows.Count * num);//计算间距

                        dr["width"] = d1 + num;// 计算间距

                        dr["vibration1"] = item.new_data[i].Vibration1;
                        dr["current1"] = item.new_data[i].Current1;

                        dr["vibration2"] = item.new_data[i].Vibration2;
                        dr["current2"] = item.new_data[i].Current2;

                        dr["vibration3"] = item.new_data[i].Vibration3;
                        dr["current3"] = item.new_data[i].Current3;

                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        #endregion
    }
}
