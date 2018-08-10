using System;
using System.Collections.Concurrent;
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
        Thread ReChart;//刷新图表
        Thread thread_List;//处理队列线程
        bool isAbort = true;// 全局 控制 图表数据停止接收  刷新图表和 处理队列是否继续进行
        List<DataModel> list = new List<DataModel>();
        List<Vibration_Current> VClist = new List<Vibration_Current>();

        public Form1()
        {
            InitializeComponent();
            //   ReceiveStart();//后台起服务
            Event_Bind();//绑定注册事件
            Chart_Init();//初始化图表
        }

        /// <summary>
        /// 开始处理图表
        /// </summary>
        private void Start_Chart()
        {
            isAbort = true;
            #region 处理队列数据
            thread_List = new Thread(Job_Queue);

            thread_List.IsBackground = true;
            thread_List.Start(1000);//启动线程

            #endregion

            #region 绘图线程

            ReChart = new Thread(Refresh_Server);
            ReChart.IsBackground = true;
            ReChart.Start(100);//启动线程

            #endregion
        }

        /// <summary>
        /// 停止处理图表
        /// </summary>
        private void End_Chart()
        {
            isAbort = false;
            #region 处理队列数据
            //if (thread_List != null)
            //{
            //    thread_List.Abort();
            //}
            #endregion

            #region 绘图线程

            //if (ReChart != null)
            //{
            //    ReChart.Abort();
            //}
            #endregion
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
            vline1 = new Line();
            vline2 = new Line();
            vline3 = new Line();
            cline1 = new Line();
            cline2 = new Line();
            cline3 = new Line();
            tChart.Series.Clear();
            Chart_Data_Bind();
            tChart.Refresh();
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
            Start_Chart();
        }

        //停止测试
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            sendUdp(agreement._3_CMD_STOPTESTER);
            this.simpleButton6.Enabled = true;
            End_Chart();
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
            tChart.Series.Clear();
            Chart_Data_Bind();

            //AddCustomAxis(tChart.Series.Count);
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
                list_Event.Enqueue(e);


                if (list.Count > 13500 && isAbort)//取12500个包感觉不对
                {
                    isAbort = false;
                    sendUdp(agreement._3_CMD_STOPTESTER);
                    this.Invoke(new ThreadStart(delegate ()
                    {
                        Thread.Sleep(1000);//暂停1秒大数据为处理完全
                        tChart.Refresh();
                    }));

                    //  End_Chart();

                }
                //异步方法
                //this.Invoke(new ThreadStart(delegate ()
                //{
                //    if (!string.IsNullOrEmpty(e.Msg))
                //    {

                //        if (list.Count > 12502)//取12500个包感觉不对
                //        {
                //            sendUdp(agreement._3_CMD_STOPTESTER);
                //            this.simpleButton6.Enabled = true;
                //        }
                //        //this.rtBox.Text = e.Msg + "\r\n" + this.rtBox.Text;
                //    }
                //    else
                //    {
                //        this.rtBox.Text = "回复数据为空！\r\n" + this.rtBox.Text;
                //    }
                //}));
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
        double num = 0.00001;//数据显示宽度  1秒=0.000001秒* 800微秒  0.0008毫秒 0.000001 * 800
                             //1个数据包 800微秒 0.0008  一个包80个数据点  每个点 10微秒 0.00001秒
        double databoxnum = (10 * 1000) / 8;

        #endregion

        //10秒
        //换算数据包个数 = 10秒/800微秒 10/0.0008
        /// <summary>
        /// 添加若干个自定义坐标轴 绘制 图表画布
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
            Axis axis;
            for (int i = 0; i < count; i++)
            {
                axis = new Axis();
                startPosition = endPosition + space;
                endPosition = startPosition + single;
                axis.StartPosition = startPosition;
                axis.EndPosition = endPosition;
                axis.AutomaticMaximum = false;//最大刻度禁用
                axis.AutomaticMinimum = false;//最小刻度禁用

                if (i == 0)
                {
                    axis.Title.Angle = 90;//'标题摆放角度
                    axis.Title.Text = "震动1";
                }
                else if (i == 1)
                {
                    axis.Title.Angle = 90;//'标题摆放角度
                    axis.Title.Text = "震动1";
                }
                else if (i == 2)
                {
                    axis.Title.Angle = 90;//'标题摆放角度
                    axis.Title.Text = "震动1";
                }
                else if (i == 3)
                {
                    axis.Title.Text = "电流1";
                    axis.Title.Angle = 90;//'标题摆放角度
                }
                else if (i == 4)
                {
                    axis.Title.Text = "电流2";
                    axis.Title.Angle = 90;//'标题摆放角度

                }
                else if (i == 5)
                {
                    axis.Title.Text = "电流3";
                    axis.Title.Angle = 90;//'标题摆放角度

                }

                axis.Maximum = 20;//最大值
                axis.Minimum = -20;//最小值
                tChart.Axes.Custom.Add(axis);
                listBaseLine[i].CustomVertAxis = axis;
            }
        }

        /// <summary>
        /// 初始化图表
        /// </summary>
        private void Chart_Init()
        {
            tChart.Dock = DockStyle.Fill;
            tChart.Aspect.View3D = false;
            tChart.Legend.Visible = false;//显示/隐藏线的注释 

            //tChart.Panel.Gradient.Visible = true;

            tChart.Legend.LegendStyle = LegendStyles.Series;
            tChart.Axes.Bottom.Labels.ValueFormat = "0.00";

            tChart.Axes.Bottom.AutomaticMaximum = true;//禁用自增长 
            tChart.Axes.Bottom.SetMinMax(0, 10);
            tChart.Axes.Bottom.Increment = 1;//控制X轴 刻度的增量

            tChart.Panel.MarginLeft = 10;
            tChart.Panel.MarginRight = 10;
            tChart.Panel.MarginBottom = 10;
            tChart.Panel.MarginTop = 10;

            panel1.Controls.Add(tChart);

            Chart_Data_Bind();//初始化绑定 线line
        }


        Line vline1 = new Line();//震动1
        Line vline2 = new Line();//震动2
        Line vline3 = new Line();//震动3
        Line cline1 = new Line();//电流1
        Line cline2 = new Line();//电流2
        Line cline3 = new Line();//电流3
        /// <summary>
        /// 绑定图表 line 线
        /// </summary>
        private void Chart_Data_Bind()
        {
            try
            {
                //DataTable dt = dtnew.Copy();
                //震动1路 vline1
                tChart.Series.Add(vline1);

                vline1.Title = string.Format("震动曲线{0}", 1);
                //vline1.YValues.DataMember = "Vibration1";
                //vline1.XValues.DataMember = "width";
                // vline1.DataSource = vcdt1;

                //震动2路 vline2
                //Line vline2 = new Line();
                tChart.Series.Add(vline2);
                vline2.Title = string.Format("震动曲线{0}", 2);
                //vline2.YValues.DataMember = "vibration2";
                //vline2.XValues.DataMember = "width";
                //vline2.DataSource = dt;

                //震动3路 vline3
                //Line vline3 = new Line();
                tChart.Series.Add(vline3);
                vline3.Title = string.Format("震动曲线{0}", 3);
                //vline3.YValues.DataMember = "vibration3";
                //vline3.XValues.DataMember = "width";
                //vline3.DataSource = dt;

                //电流1路 cline1
                tChart.Series.Add(cline1);
                cline1.Title = string.Format("电流曲线{0}", 1);
                //cline1.YValues.DataMember = "Current1";
                //cline1.XValues.DataMember = "width";
                //  cline1.DataSource = vcdt1;

                //电流2路 cline2
                //Line cline2 = new Line();
                tChart.Series.Add(cline2);
                cline2.Title = string.Format("电流曲线{0}", 2);
                //cline2.YValues.DataMember = "current2";
                //cline2.XValues.DataMember = "width";
                //cline2.DataSource = dt;

                //电流3路 cline3
                //Line cline3 = new Line();
                tChart.Series.Add(cline3);
                cline3.Title = string.Format("电流曲线{0}", 3);
                //cline3.YValues.DataMember = "current3";
                //cline3.XValues.DataMember = "width";
                //cline3.DataSource = dt;

                AddCustomAxis(tChart.Series.Count);//绘制画布
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        #endregion

        #region 队列刷新动态图表

        ConcurrentQueue<Udp_EventArgs> list_Event = new ConcurrentQueue<Udp_EventArgs>();

        /// <summary>
        /// 处理队列
        /// </summary>
        private void Job_Queue(object time)
        {
            while (isAbort)
            {
                Udp_EventArgs e = new Udp_EventArgs();//初始化 实体
                if (list_Event.Count > 0)
                {
                    list_Event.TryDequeue(out e);//取出队里数据并删除
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
                            int length = 24 * i;//截取位置

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


                            #region vcdt1 动态刷新图表数据源

                            //DataRow dr1 = vcdt1.NewRow();

                            //Vibration_Current model1 = new Vibration_Current();

                            //model1.id= VClist.Count + 1;
                            //model1.Vibration1 = vmodel1.Vibration1;
                            //model1.Vibration2 = vmodel1.Vibration2;
                            //model1.Vibration3 = vmodel1.Vibration3;

                            //model1.Current1= vmodel1.Current1;
                            //model1.Current2= vmodel1.Current2;
                            //model1.Current3 = vmodel1.Current3;
                            //model1.width = ((vcdt1.Rows.Count * num) + num).ToString();

                            ////while (Is_Refresh_Chart)//暂停处理
                            ////{

                            ////    //Thread.Sleep(100);
                            ////}

                            //VClist.Add(model1);

                            #endregion

                            #region vcdt1 动态刷新图表数据源

                            //DataRow dr1 = vcdt1.NewRow();
                            //dr1["id"] = vcdt1.Rows.Count + 1;
                            //dr1["Vibration1"] = vmodel1.Vibration1;
                            //dr1["Vibration2"] = vmodel1.Vibration2;
                            //dr1["Vibration3"] = vmodel1.Vibration3;

                            //dr1["Current1"] = vmodel1.Current1;
                            //dr1["Current2"] = vmodel1.Current2;
                            //dr1["Current3"] = vmodel1.Current3;
                            //dr1["width"] = (vcdt1.Rows.Count * num) + num;

                            ////while (Is_Refresh_Chart)//暂停处理
                            ////{

                            ////    //Thread.Sleep(100);
                            ////}

                            //vcdt1.Rows.Add(dr1);

                            #endregion

                            #region line 动态刷新图表数据源

                            if (this.v1.Checked)
                            {
                                string vwidth1 = ((vline1.Count * num) + num).ToString();//震动1长度
                                vline1.Add(Convert.ToDouble(vwidth1), Convert.ToDouble(vmodel1.Vibration1));
                            }
                            if (this.v2.Checked)
                            {

                                string vwidth2 = ((vline2.Count * num) + num).ToString();//震动2长度
                                vline2.Add(Convert.ToDouble(vwidth2), Convert.ToDouble(vmodel1.Vibration2));
                            }
                            if (this.v3.Checked)
                            {
                                string vwidth3 = ((vline3.Count * num) + num).ToString();//震动3长度
                                vline3.Add(Convert.ToDouble(vwidth3), Convert.ToDouble(vmodel1.Vibration3));
                            }
                            if (this.c1.Checked)
                            {
                                string cwidth1 = ((cline1.Count * num) + num).ToString();//电流1长度
                                cline1.Add(Convert.ToDouble(cwidth1), Convert.ToDouble(vmodel1.Current1));
                            }
                            if (this.c2.Checked)
                            {
                                string cwidth2 = ((cline2.Count * num) + num).ToString();//电流2长度
                                cline2.Add(Convert.ToDouble(cwidth2), Convert.ToDouble(vmodel1.Current2));
                            }
                            if (this.c3.Checked)
                            {
                                string cwidth3 = ((cline3.Count * num) + num).ToString();//电流3长度
                                cline3.Add(Convert.ToDouble(cwidth3), Convert.ToDouble(vmodel1.Current3));
                            }
                            #endregion
                        }
                        list.Add(model);
                    }
                }
            }
        }

        #region  刷新绘图代码

        /// <summary>
        /// 刷新图表
        /// </summary>
        /// <param name="time"></param>
        private void Refresh_Server(object time)
        {
            int Sleep_Time = (int)time;
            while (isAbort)
            {
                try
                {
                    //Thread.Sleep(Sleep_Time);
                    // 异步方法
                    this.Invoke(new ThreadStart(delegate ()
                {
                    tChart.Refresh();

                }));
                }
                catch (Exception ex)
                {

                }
            }
        }


        #endregion 

        #endregion
    }
}
