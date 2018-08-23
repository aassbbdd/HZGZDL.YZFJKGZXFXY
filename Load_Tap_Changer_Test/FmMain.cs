﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Socket_Server;
using System.Threading;
using System.Collections.Concurrent;

using Udp_Agreement.Model;
using Socket_Server.Udp_Event;
using Udp_Agreement;
using System.Net;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Export;
using DocDecrypt.Common;
using DbHelper.Sqlite_Db;
using Commons;
using Commons.XmlModel;
using System.Xml.Linq;
using System.Xml;
using Steema.TeeChart.Tools;

namespace Basic_Controls
{
    public partial class FmMain : DevExpress.XtraEditors.XtraForm
    {
        public FmMain()
        {
            InitializeComponent();

        }


        private void FmMain_Load(object sender, EventArgs e)
        {
            Event_Bind();//绑定注册事件
            Chart_Init();//初始化图表
            XmlHelper.Init("测试数据1");
            xmlpath = XmlHelper.xmlpath;
        }
        #region 页面初始化参数

        /// <summary>
        /// xml 存储路径
        /// </summary>
        string xmlpath = "";

        /// <summary>
        /// 协议对应参数
        /// </summary>
        Tester_Agreement agreement = new Tester_Agreement();

        #endregion

        #region 按键

        #region 菜单栏按钮

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        #region 任务栏按钮



        /// <summary>
        /// 新建测试计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int index = 0;
            using (FmTestSelect form = new FmTestSelect())
            {
                form.ShowDialog();
                index = form.index;
            }

            if (index == 1)// 加载一个存在的测试计划
            {

            }
            else if (index == 2)//加载分析测试数据
            {

            }
            else if (index == 3)///新建测试计划
            {
                using (FmAddTest form = new FmAddTest())
                {
                    form.ShowDialog();
                }
            }
            else//使用默认测试计划
            {
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog pSaveFileDialog = new SaveFileDialog
            {
                Title = "保存为:",
                RestoreDirectory = true,
                Filter = "所有文件(*.*)|*.*"
            };//同打开文件，也可指定任意类型的文件
            if (pSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = pSaveFileDialog.FileName;
            }
        }


        /// <summary>
        /// 载入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnLond_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string filepath = FileHelper.GetOpenFilePath();
                if (!string.IsNullOrEmpty(filepath))
                {
                    DataTable dt = XmlHelper.Xml_To_DataTable(filepath);
                    Chart_DataTable_Init();
                    Chart_Data_Lond_Bind(dt);
                    tChart.Refresh();
                    min = tChart.Axes.Bottom.Minimum;
                    max = tChart.Axes.Bottom.Maximum;
                    max = tChart.Axes.Bottom.MaxXValue;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSaveImg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "TestLineImage";
            FileHelper.CreateDirectoy(path);
            //文件夹名字
            string filename = DateTime.Now.ToString("d");
            path = path + "\\" + filename;
            FileHelper.CreateDirectoy(path);
            string imgname = "\\" + DateTime.Now.ToString("HHmmss") + ".png";

            tChart.Export.Image.PNG.Width = 800;
            tChart.Export.Image.PNG.Height = 600;

            tChart.Export.Image.PNG.Save(path + "\\" + imgname);
        }
        /// <summary>
        /// 心跳测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPant_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sendUdp(agreement._1_CMD_HEARTBEAT);
        }
        /// <summary>
        /// 连续测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 停止测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnETest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sendUdp(agreement._3_CMD_STOPTESTER);
            End_Chart();
        }

        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sendUdp(agreement._2_CMD_STARTTESTER);
            Start_Chart();
        }

        #endregion

        #endregion

        #region 调用方法
        /// <summary>
        /// 发送协议
        /// </summary>
        private void sendUdp(string msg)
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.1.5"), 4000);
            //if (!string.IsNullOrEmpty(txtIp.Text) && !string.IsNullOrEmpty(txtPorit.Text))
            //{
            //    ipep = new IPEndPoint(IPAddress.Parse(txtIp.Text), Convert.ToInt32(txtPorit.Text));
            //}
            SendMessage.SendMsgStart(msg, ipep);
        }

        /// <summary>
        /// 绑定注册事件
        /// </summary>
        private void Event_Bind()
        {
            try
            {
                //波形数据
                SendMessage.udp_Event += new EventHandler<Udp_EventArgs>(Run);
                //一般协议数据
                SendMessage.udp_Event_Kind += new EventHandler<Udp_EventArgs>(Run_Kind);
                Event_Chart_Bind();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// teechart 图表事件注册
        /// </summary>
        private void Event_Chart_Bind()
        {
            this.tChart.MouseWheel += new MouseEventHandler(tChart_MouseWheel);
            this.tChart.MouseMove += new MouseEventHandler(chart_MouseMove);
            this.tChart.MouseUp += new MouseEventHandler(chart_MouseUp);
            this.tChart.MouseDown += new MouseEventHandler(chart_MouseDown);
        }


        /// <summary>
        /// 事件回调执行方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run_Kind(object sender, Udp_EventArgs e)
        {
            try
            {
                string data = e.Msg;
                string head = e.Msg;
                Invoke(new ThreadStart(delegate ()
                {
                    MessageBox.Show("设备连接成功");
                }));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
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
                //绘图
                list_Event.Enqueue(e);
                //数据存储
                Db_Source.Enqueue(e);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        #endregion

        #region  多图表 数据处理 图表刷新 功能

        #region 图表初始化参数配置

        /// <summary>
        /// 全局 图表 队列 数据 处理 开关 ture 开 false 关
        /// </summary>
        bool isAbort = true;

        /// <summary>
        /// 刷新图表
        /// </summary>
        Thread ReChart;

        /// <summary>
        /// 存储数据
        /// </summary>
        Thread Db_Save;

        /// <summary>
        /// 处理队列线程
        /// </summary>
        Thread thread_List;

        /// <summary>
        /// 刷新时间图表
        /// </summary>
        int Sleep_Time = 0;

        /// <summary>
        /// TChart 控件初始化
        /// </summary>
        private TChart tChart = new TChart();

        /// <summary>
        /// //计算坐标百分比参数
        /// </summary>
        private int space = 5;

        /// <summary>
        /// //将显示宽度转换为整数
        /// </summary>
        int allnum = 100000;

        /// <summary>
        /// 单点距离 乘以 allnum 转换系数（100000） 最后转换为点位是 除去 allnum 
        /// //数据显示宽度  1秒=0.000001秒* 800微秒  0.0008毫秒 0.000001 * 800
        ///1个数据包 800微秒 0.0008  一个包80个数据点  每个点 10微秒 0.00001秒 
        /// </summary>
        int num = 1;

        /// <summary>
        /// 偷点数量 80能除尽数量 进行偷点
        /// </summary>
        int LessPoint = 80;//

        /// <summary>
        ///总点数=10秒/（单点长度*偷点数）
        /// </summary>
        static int linlength = 0;

        /// <summary>
        ///  //计算1秒 点位数量
        /// </summary>
        int pnum = 0;

        Line vline1;//震动1
        Line vline2;//震动2
        Line vline3;//震动3
        Line cline1;//电流1
        Line cline2;//电流2
        Line cline3;//电流3
        #endregion

        #region 控制显示几路波形
        /*震动3路*/
        bool v1 = true;
        bool v2 = false;
        bool v3 = false;

        /*电流3路*/
        bool c1 = false;
        bool c2 = false;
        bool c3 = false;

        #endregion

        /// <summary>
        /// 初始化图表
        /// </summary>
        private void Chart_Init()
        {
            tChart.Series.Clear();
            //总点数=10秒/（单点长度*偷点数）
            linlength = (10 * allnum) / num / LessPoint;
            //计算1秒 点位数量
            pnum = (int)(0.1 * allnum) / num / LessPoint;

            vx1 = new double[linlength];
            vy1 = new double[linlength];

            vx2 = new double[linlength];
            vy2 = new double[linlength];

            vx3 = new double[linlength];
            vy3 = new double[linlength];

            cx1 = new double[linlength];
            cy1 = new double[linlength];

            cx2 = new double[linlength];
            cy2 = new double[linlength];

            cx3 = new double[linlength];
            cy3 = new double[linlength];

            vline1 = new Line();//震动1
            vline2 = new Line();//震动2
            vline3 = new Line();//震动3
            cline1 = new Line();//电流1
            cline2 = new Line();//电流2
            cline3 = new Line();//电流3

            tChart.Dock = DockStyle.Fill;
            tChart.Aspect.View3D = false;
            tChart.Legend.Visible = false;//显示/隐藏线的注释 

            //tChart.Panel.Gradient.Visible = true;

            tChart.Legend.LegendStyle = LegendStyles.Series;
            tChart.Axes.Bottom.Labels.ValueFormat = "0.00";

            tChart.Axes.Bottom.SetMinMax(0, 10);
            tChart.Axes.Bottom.Increment = 1;//控制X轴 刻度的增量

            tChart.Panel.MarginLeft = 10;
            tChart.Panel.MarginRight = 10;
            tChart.Panel.MarginBottom = 10;
            tChart.Panel.MarginTop = 10;

            tChart.Zoom.Allow = false;
            ////轴标线
            //Points pointSeries1 = new Points(tChart.Chart);
            //CursorTool cursorTool1 = new CursorTool(tChart.Chart);
            //pointSeries1.FillSampleValues(20);
            //cursorTool1.Active = true;
            //cursorTool1.FollowMouse = true;
            //cursorTool1.Series = pointSeries1;
            //cursorTool1.Style = CursorToolStyles.Both;



            pclChart.Controls.Add(tChart);// 绑定图表位置 
            Chart_Data_Bind();//初始化绑定 线line

            min = tChart.Axes.Bottom.Minimum;
            max = tChart.Axes.Bottom.Maximum;

        }

        double min = 0.0;
        double max = 0.0;
        /// <summary>
        /// 初始化重新查看数据
        /// </summary>
        private void Chart_DataTable_Init()
        {
            tChart = new TChart();

            tChart.Dock = DockStyle.Fill;
            tChart.Aspect.View3D = false;
            tChart.Legend.Visible = false;//显示/隐藏线的注释 

            //tChart.Panel.Gradient.Visible = true;

            tChart.Legend.LegendStyle = LegendStyles.Series;
            tChart.Axes.Bottom.Labels.ValueFormat = "0.00";

            tChart.Axes.Bottom.Minimum = 0.00;
            tChart.Axes.Bottom.AutomaticMaximum = true;
            tChart.Axes.Bottom.AutomaticMinimum = true;
            tChart.Axes.Bottom.Increment = 1;//控制X轴 刻度的增量

            tChart.Panel.MarginLeft = 10;
            tChart.Panel.MarginRight = 10;
            tChart.Panel.MarginBottom = 10;
            tChart.Panel.MarginTop = 10;

            tChart.Zoom.Allow = false;

            pclChart.Controls.Clear();
            pclChart.Controls.Add(tChart);// 绑定图表位置 
            Event_Chart_Bind();
        }

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
            // tChart.Axes.Custom.Clear();//清除原先画布
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
                    axis.Title.Text = "震动2";
                }
                else if (i == 2)
                {
                    axis.Title.Angle = 90;//'标题摆放角度
                    axis.Title.Text = "震动3";
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

                axis.Maximum = 10;//最大值
                axis.Minimum = -10;//最小值

                tChart.Axes.Custom.Add(axis);
                listBaseLine[i].CustomVertAxis = axis;
            }
        }

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

                //震动2路 vline2
                //Line vline2 = new Line();
                tChart.Series.Add(vline2);


                //震动3路 vline3
                //Line vline3 = new Line();
                tChart.Series.Add(vline3);


                //电流1路 cline1
                tChart.Series.Add(cline1);


                //电流2路 cline2
                //Line cline2 = new Line();
                tChart.Series.Add(cline2);


                //电流3路 cline3
                //Line cline3 = new Line();
                tChart.Series.Add(cline3);

                AddCustomAxis(tChart.Series.Count);//绘制画布
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 重新加载图表
        /// </summary>
        private void Chart_Data_Lond_Bind(DataTable dt)
        {
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    tChart.Series.Clear();

                    tChart.Refresh();
                    //震动1路 vline1
                    if (v1)
                    {
                        Line vline1 = new Line();
                        tChart.Series.Add(vline1);

                        vline1.Title = string.Format("震动曲线{0}", 1);
                        vline1.YValues.DataMember = "V1";
                        vline1.XValues.DataMember = "Xwitdh";
                        vline1.DataSource = dt;
                    }
                    //震动2路 vline2
                    if (v2)
                    {
                        Line vline2 = new Line();
                        tChart.Series.Add(vline2);
                        vline2.Title = string.Format("震动曲线{0}", 2);
                        vline2.YValues.DataMember = "V2";
                        vline2.XValues.DataMember = "Xwitdh";
                        vline2.DataSource = dt;
                    }
                    //震动3路 vline3
                    if (v3)
                    {
                        Line vline3 = new Line();
                        tChart.Series.Add(vline3);
                        vline3.Title = string.Format("震动曲线{0}", 3);
                        vline3.YValues.DataMember = "V3";
                        vline3.XValues.DataMember = "Xwitdh";
                        vline3.DataSource = dt;
                    }
                    //电流1路 cline1
                    if (c1)
                    {
                        tChart.Series.Add(cline1);
                        cline1.Title = string.Format("电流曲线{0}", 1);
                        cline1.YValues.DataMember = "C1";
                        cline1.XValues.DataMember = "Xwitdh";
                        cline1.DataSource = dt;
                    }
                    //电流2路 cline2
                    if (c2)
                    {
                        Line cline2 = new Line();
                        tChart.Series.Add(cline2);
                        cline2.Title = string.Format("电流曲线{0}", 2);
                        cline2.YValues.DataMember = "C2";
                        cline2.XValues.DataMember = "Xwitdh";
                        cline2.DataSource = dt;
                    }
                    //电流3路 cline3
                    if (c3)
                    {
                        Line cline3 = new Line();
                        tChart.Series.Add(cline3);
                        cline3.Title = string.Format("电流曲线{0}", 3);
                        cline3.YValues.DataMember = "C3";
                        cline3.XValues.DataMember = "Xwitdh";
                        cline3.DataSource = dt;
                    }

                    //绘制画布
                    AddCustomAxis(tChart.Series.Count);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绑定DataTable图表 line 线
        /// </summary>
        private void Chart_DataTable_Bind()
        {
            try
            {
                if (v1)
                {
                    //DataTable dt = dtnew.Copy();
                    //震动1路 vline1
                    tChart.Series.Add(vline1);
                }
                if (v2)
                {
                    //震动2路 vline2
                    //Line vline2 = new Line();
                    tChart.Series.Add(vline2);
                }
                if (v3)
                {
                    //震动3路 vline3
                    //Line vline3 = new Line();
                    tChart.Series.Add(vline3);
                }

                if (c1)
                {
                    //电流1路 cline1
                    tChart.Series.Add(cline1);
                }
                if (c2)
                {
                    //电流2路 cline2
                    //Line cline2 = new Line();
                    tChart.Series.Add(cline2);
                }
                if (c3)
                {
                    //电流3路 cline3
                    //Line cline3 = new Line();
                    tChart.Series.Add(cline3);
                }
                AddCustomAxis(tChart.Series.Count);//绘制画布
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 队列刷新动态图表

        /// <summary>
        /// 绘图队列
        /// </summary>
        ConcurrentQueue<Udp_EventArgs> list_Event = new ConcurrentQueue<Udp_EventArgs>();
        /// <summary>
        /// 存储数据队列
        /// </summary>
        ConcurrentQueue<Udp_EventArgs> Db_Source = new ConcurrentQueue<Udp_EventArgs>();

        #region  line x y轴 数据源

        double[] vx1;
        double[] vy1;

        double[] vx2;
        double[] vy2;

        double[] vx3;
        double[] vy3;



        double[] cx1;
        double[] cy1;

        double[] cx2;
        double[] cy2;

        double[] cx3;
        double[] cy3;
        #endregion

        /// <summary>
        /// 开始处理图表
        /// </summary>
        private void Start_Chart()
        {
            isAbort = true;

            #region 处理队列数据

            thread_List = new Thread(Job_Queue);
            thread_List.IsBackground = true;
            thread_List.Start();//启动线程

            #endregion

            #region 绘图线程

            ReChart = new Thread(Refresh_Server);
            ReChart.IsBackground = true;
            ReChart.Start();//启动线程

            #endregion

            #region 存储数据

            Db_Save = new Thread(Test_Xml_Insert);
            Db_Save.IsBackground = true;
            Db_Save.Start();//启动线程

            #endregion

            Chart_Init();
        }

        /// <summary>
        /// 停止处理图表
        /// </summary>
        private void End_Chart()
        {
            isAbort = false;
        }

        #region 队列数据处理

        private void Job_Queue()
        {
            while (isAbort)
            {
                Udp_EventArgs e = new Udp_EventArgs();//初始化 实体
                if (list_Event.Count > 0)
                {
                    list_Event.TryDequeue(out e);//取出队里数据并删除
                                                 //截取返回数据
                    if (!string.IsNullOrEmpty(e.Hearder))
                    {
                        string data = e.Msg.Substring(8, e.Msg.Length - 8);
                        //string head = e.Msg.Substring(4, 4);

                        int Porints = 80 / LessPoint;

                        for (int i = 0; i < Porints; i++)
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

                            #region 计算保存新数据

                            double width = 10.00;

                            vx1[linlength - 1] = width;
                            vy1[linlength - 1] = Convert.ToDouble(vmodel1.Vibration1);

                            vx2[linlength - 1] = width;
                            vy2[linlength - 1] = Convert.ToDouble(vmodel1.Vibration2);

                            vx3[linlength - 1] = width;
                            vy3[linlength - 1] = Convert.ToDouble(vmodel1.Vibration3);

                            cx1[linlength - 1] = width;
                            cy1[linlength - 1] = Convert.ToDouble(vmodel1.Current1);

                            cx2[linlength - 1] = width;
                            cy2[linlength - 1] = Convert.ToDouble(vmodel1.Current2);

                            cx3[linlength - 1] = width;
                            cy3[linlength - 1] = Convert.ToDouble(vmodel1.Current3);
                            #endregion

                            #region 对数组操作

                            for (int j = 0; j < vx1.Length - 1; j++)//先将已有数据平移，再将新增数据加入
                            {
                                //往前平移距离 当前序号 j 乘以 num 位移距离 乘以偷点数 LessPoint  除以 到整数位的数量 allnum
                                double newXvalue = (double)(j * num * LessPoint) / allnum;
                                if (v1)
                                {
                                    vx1[j] = newXvalue;
                                    vy1[j] = vy1[j + 1];
                                }
                                if (v2)
                                {
                                    vx2[j] = newXvalue;
                                    vy2[j] = vy2[j + 1];
                                }

                                if (v3)
                                {
                                    vx3[j] = newXvalue;
                                    vy3[j] = vy3[j + 1];
                                }

                                if (c1)
                                {
                                    cx1[j] = newXvalue;
                                    cy1[j] = cy1[j + 1];
                                }

                                if (c2)
                                {
                                    cx2[j] = newXvalue;
                                    cy2[j] = cy2[j + 1];
                                }

                                if (c3)
                                {
                                    cx3[j] = newXvalue;
                                    cy3[j] = cy3[j + 1];
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        #endregion

        #region  刷新绘图代码

        /// <summary>
        /// 刷新图表
        /// </summary>
        /// <param name="time"></param>
        private void Refresh_Server()
        {
            while (isAbort)
            {
                try
                {
                    // 异步方法
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        Line_Bind();
                    }));
                    //死循环
                    Thread.Sleep(150);
                }
                catch (Exception ex)
                {
                }
            }
        }

        /// <summary>
        /// 绑定 line线
        /// </summary>
        private void Line_Bind()
        {
            if (v1)
            {
                vline1.Add(vx1, vy1);
            }
            if (v2)
            {
                vline2.Add(vx2, vy2);
            }
            if (v3)
            {
                vline3.Add(vx3, vy3);
            }
            if (c1)
            {
                cline1.Add(cx1, cy1);
            }
            if (c2)
            {
                cline2.Add(cx2, cy2);
            }
            if (c3)
            {
                cline3.Add(cx3, cy3);
            }
        }

        #endregion

        #region SQL 数据库操作
        public void Test_Data_insert()
        {
            try
            {
                while (true)
                {
                    if (Db_Source.Count > 0)
                    {

                        Udp_EventArgs e = new Udp_EventArgs();

                        Db_Source.TryDequeue(out e);//取出队里数据并删除

                        DataModel model = new DataModel();

                        model.head = e.Msg.Substring(4, 4); ;
                        model.text = e.Msg;
                        model.id = Convert.ToInt32(model.head, 16);
                        // Db_Action.Instance.Test_Data_insert(model);
                        ListToText.Instance.WriteListToTextFile1(e.Msg + "|");

                        Thread.Sleep(1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Xml 数据操作

        public void Test_Xml_Insert()
        {
            try
            {
                int i = 1;
                while (true)
                {
                    if (Db_Source.Count > 0)
                    {
                        Udp_EventArgs e = new Udp_EventArgs();

                        Db_Source.TryDequeue(out e);//取出队里数据并删除
                        Xml_Node_Model model = new Xml_Node_Model();
                        model.Id = e.Msg.Substring(4, 4); ;
                        model.DataSource = e.Msg;
                        model.Data = new List<Xml_Element_Model>();

                        XmlHelper.Insert(model);
                        Thread.Sleep(1);
                        if (i == 100)
                        {
                            XmlHelper.Save();
                            i = 1;
                        };
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #endregion

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            double OldXMin = tChart.Axes.Bottom.Minimum;
            double OldXMax = tChart.Axes.Bottom.Maximum;
            double XMid = (OldXMin + OldXMax) / 2;
            double NewXMin = (XMid * 0.5 + OldXMin) / (1.5);
            double NewXMax = (XMid * 0.5 + OldXMax) / (1.5);
            tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);

        }
        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            double OldXMin = tChart.Axes.Bottom.Minimum;
            double OldXMax = tChart.Axes.Bottom.Maximum;
            double XMid = (OldXMin + OldXMax) / 2;
            double NewXMin = (-XMid * 0.5 + OldXMin) / (0.5);
            double NewXMax = (-XMid * 0.5 + OldXMax) / (0.5);
            tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);
        }
        /// <summary>
        /// 中键放大缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tChart_MouseWheel(object sender, MouseEventArgs e)
        {
            if (tChart != null)
            {
                double XMid = tChart.Axes.Bottom.CalcPosPoint(e.X);

                if (e.Delta > 0)
                {
                    if (aotuzoom < 3)
                    {
                        double OldXMin = tChart.Axes.Bottom.CalcPosPoint(e.X) - (aotuzoom == 0 ? 0.5 : (0.5 / aotuzoom));
                        double OldXMax = tChart.Axes.Bottom.CalcPosPoint(e.X) + (aotuzoom == 0 ? 0.5 : (0.5 / aotuzoom));

                        double NewXMin = (XMid * 0.5 + OldXMin) / (1.5);
                        double NewXMax = (XMid * 0.5 + OldXMax) / (1.5);

                        tChart.Axes.Bottom.Labels.ValueFormat = "0.000000";
                        tChart.Axes.Bottom.Increment = 0.000001;//控制X轴 刻度的增量

                        tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);
                        aotuzoom++;
                    }
                }
                else
                {
                    if (aotuzoom > 1)
                    {
                        double OldXMin = tChart.Axes.Bottom.CalcPosPoint(e.X) + (aotuzoom == 0 ? 0.5 : (0.5 / aotuzoom));
                        double OldXMax = tChart.Axes.Bottom.CalcPosPoint(e.X) - (aotuzoom == 0 ? 0.5 : (0.5 / aotuzoom));
                        double NewXMin = (-XMid * 0.5 + OldXMin) / (0.5);
                        double NewXMax = (-XMid * 0.5 + OldXMax) / (0.5);
                        tChart.Axes.Bottom.Labels.ValueFormat = "0.000000";
                        tChart.Axes.Bottom.Increment = 0.000001;//控制X轴 刻度的增量
                        tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);
                        aotuzoom--;
                    }else if(aotuzoom==1)
                    {
                        tChart.Axes.Bottom.Labels.ValueFormat = "0.00";
                        tChart.Axes.Bottom.SetMinMax(min, max);
                        aotuzoom = 0;
                    }
                }
            }
        }

        private void chart_MouseMove(object sender, MouseEventArgs e)
        {

            lbX.Text = e.X.ToString();
            lbY.Text = e.Y.ToString();
        }
        /// <summary>
        /// 鼠标点击时X位置
        /// </summary>
        int sx = 0;
        /// <summary>
        /// 鼠标点击时Y位置
        /// </summary>
        int sy = 0;
        /// <summary>
        /// 放大次数 只能放大缩小3次
        /// </summary>
        int aotuzoom = 0;
        /// <summary>
        /// 点击鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_MouseDown(object sender, MouseEventArgs e)
        {
            sx = e.X;
            sy = e.Y;
        }
        /// <summary>
        /// 松开鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_MouseUp(object sender, MouseEventArgs e)
        {

            if (tChart != null && tChart.Series.Count > 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    // 开始位置大于结束位置    开始位置大于0  是鼠标左键
                    if ((sx < e.X || sy < e.Y) && sx > 0 && sy > 0)
                    {
                        if (aotuzoom < 3)
                        {
                            double min = tChart.Axes.Bottom.CalcPosPoint(sx);
                            double max = tChart.Axes.Bottom.CalcPosPoint(e.X);
                            tChart.Axes.Bottom.Labels.ValueFormat = "0.000000";
                            tChart.Axes.Bottom.Increment = 0.000001;//控制X轴 刻度的增量
                            tChart.Axes.Bottom.SetMinMax(min, max);
                            aotuzoom++;
                        }
                    }
                    else
                    {
                        tChart.Axes.Bottom.Labels.ValueFormat = "0.00";
                        tChart.Axes.Bottom.SetMinMax(min, max);
                        aotuzoom = 0;
                    }
                }
            }


        }
    }
}