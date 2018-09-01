using System;
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
using DbHelper;
using System.Configuration;
using DbHelper.Db_Model;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;

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
            Bind_IsDC();
            Event_Bind();//绑定注册事件
            Chart_Init();//初始化图表
            CreateDb();//生成数据库
            Tester_List_Bind();//获取测试计划绑定到页面树
        }

        #region 页面初始化参数

        /// <summary>
        /// xml 存储路径
        /// </summary>
        string xmlpath = "";

        /// <summary>
        /// 接收数据开关
        /// </summary>
        bool IsSaveData = false;

        /// <summary>
        /// 协议对应参数
        /// </summary>
        Tester_Agreement agreement = new Tester_Agreement();
        /// <summary>
        /// s设备IP
        /// </summary>
        string DvIp = ConfigurationManager.ConnectionStrings["DvIp"].ConnectionString.ToString();

        /// <summary>
        /// 获取焦点行
        /// </summary>
        TreeListNode publicnode;
        Test_Plan pub_Test_Plan;
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
            TreeListNode node = treeList.FocusedNode;
            Test_Plan model = new Test_Plan();
            if (node != null)
            {
                model = Test_Plan_Bind(node);
            }
            model.ISEDIT = "1";
            string id = "";
            model.PARENTID = "0";
            using (FmAddTest form = new FmAddTest(model))
            {
                form.ShowDialog();
                id = form.id;
            }
            Tester_List_Bind();

            Set_Foucs(id);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //SaveFileDialog pSaveFileDialog = new SaveFileDialog
            //{
            //    Title = "保存为:",
            //    RestoreDirectory = true,
            //    Filter = "所有文件(*.*)|*.*"
            //};//同打开文件，也可指定任意类型的文件
            //if (pSaveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    string path = pSaveFileDialog.FileName;
            //}
            //IsSaveData = true;
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
                    //  DataTable dt = XmlHelper.Xml_To_DataTable(filepath, cks);
                    XmlHelper.Xml_To_Array(filepath, cks,
                       out vx1, out vy1,
                       out vx2, out vy2,
                       out vx3, out vy3,
                       out cx1, out cy1,
                       out cx2, out cy2,
                       out cx3, out cy3
                        );



                    Chart_DataTable_Init();
                    Chart_Data_Lond_Bind();

                    tChart.Refresh();
                    min = tChart.Axes.Bottom.Minimum;
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
            //Tester_List_Bind();
        }

        /// <summary>
        /// 停止测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sendUdp(agreement._3_CMD_STOPTESTER);
            End_Chart();
            Invoke(new ThreadStart(delegate ()
            {
                panelControl1.Enabled = true;
                pc2.Enabled = true;
                IsSaveData = false;
            }));
        }

        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (publicnode == null)
            {
                MessageBox.Show("请在左侧选择测试计划");
                return;
            }

            End_Chart();
            #region 生成测试配置信息
            Test_Plan model = Test_Plan_Bind(publicnode);
            model.PARENTID = model.ID;
            model.DVNAME += "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            Db_Action.Instance.Test_Confige_Insert(model);
            //生成后刷新树
            treeList.Refresh();
            #endregion
            string filename = model.DVNAME;

            XmlHelper.DeleteXmlDocument(filename);
            XmlHelper.Init(filename);
            xmlpath = XmlHelper.xmlpath;
            Thread.Sleep(10);

            sendUdp(agreement._2_CMD_STARTTESTER);
            //清空Y轴
            tChart.Series.RemoveAllSeries();
            //清空Y轴
            tChart.Axes.Custom.Clear();
            Start_Chart();
            panelControl1.Enabled = false;
            pc2.Enabled = false;
        }

        #endregion

        #region 功能按钮
        /// <summary>
        /// 重新加载图标显示通道线路
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReLond_Click(object sender, EventArgs e)
        {
            //更新波形通道
            Bind_IsDC();
            //清空Y轴
            tChart.Series.RemoveAllSeries();
            //清空Y轴
            tChart.Axes.Custom.Clear();
            Chart_Data_Lond_Bind();
            tChart.Refresh();
        }

        /// <summary>
        /// 放大缩小数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReNew_Click(object sender, EventArgs e)
        {
            tChart.Zoom.Undo();
        }

        /// <summary>
        /// 清空页面图表和数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (tChart != null && linlength > 0)
            {
                Porint_List = new ConcurrentQueue<Udp_EventArgs>();
                Save_Db_Source = new ConcurrentQueue<Udp_EventArgs>();
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

                vline1.Clear();
                vline2.Clear();
                vline3.Clear();
                cline1.Clear();
                cline2.Clear();
                cline3.Clear();

                tChart.Refresh();
            }
            else
            {
                MessageBox.Show("无数据清理！");
            }
        }

        bool[] cks = new bool[6];

        /// <summary>
        /// 保定数据通道是否开启
        /// </summary>
        private void Bind_IsDC()
        {
            v1 = ckV1.Checked;
            v2 = ckV2.Checked;
            v3 = ckV3.Checked;

            c1 = ckC1.Checked;
            c2 = ckC2.Checked;
            c3 = ckC3.Checked;

            cks[1] = v1;
            cks[2] = v2;
            cks[3] = v3;
            cks[1] = c1;
            cks[2] = c2;
            cks[3] = c3;
        }
        #endregion

        #endregion
        #region 调用方法

        /// <summary>
        /// 发送协议
        /// </summary>
        private void sendUdp(string msg)
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(DvIp), 4000);
            //if (!string.IsNullOrEmpty(txtIp.Text) && !string.IsNullOrEmpty(txtPorit.Text))
            //{
            //    ipep = new IPEndPoint(IPAddress.Parse(txtIp.Text), Convert.ToInt32(txtPorit.Text));
            //}
            SendMessage.SendMsgStart(msg, ipep);
        }

        /// <summary>
        /// 绑定UDP协议回调注册事件
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
        /// 事件回调执行方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run_Kind(object sender, Udp_EventArgs e)
        {
            try
            {
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
                Porint_List.Enqueue(e);
                Invoke(new ThreadStart(delegate ()
                {
                    LBPrintCount.Text = Porint_List.Count.ToString();
                }));
             
                //if (IsSaveData)
                //{
                //    //数据存储
                //    Save_Db_Source.Enqueue(e);
                //}
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        #endregion

        #region  多图表 数据处理 图表刷新 功能

        #region teeChart 鼠标事件

        /// <summary>
        /// teechart 图表事件注册
        /// </summary>
        private void Event_Chart_Bind()
        {
            tChart.MouseWheel += new MouseEventHandler(tChart_MouseWheel);
            tChart.MouseMove += new MouseEventHandler(chart_MouseMove);
            tChart.MouseUp += new MouseEventHandler(chart_MouseUp);
            tChart.MouseDown += new MouseEventHandler(chart_MouseDown);
            //  tChart.Zoomed += new EventHandler(chart_Zoomed);//自带 放大缩小 事件
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
        /// 放大次数 只能放大 3次 缩小只能大于1
        /// </summary>
        int aotuzoom = 0;

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (aotuzoom < 3)
            {
                double OldXMin = tChart.Axes.Bottom.Minimum;
                double OldXMax = tChart.Axes.Bottom.Maximum;
                double XMid = (OldXMin + OldXMax) / 2;
                double NewXMin = (XMid * 0.5 + OldXMin) / (1.5);
                double NewXMax = (XMid * 0.5 + OldXMax) / (1.5);
                tChart.Axes.Bottom.Labels.ValueFormat = "0.000000";
                tChart.Axes.Bottom.Increment = 0.000001;//控制X轴 刻度的增量
                tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);
                aotuzoom++;
            }
        }
        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (aotuzoom > 1)
            {
                double OldXMin = tChart.Axes.Bottom.Minimum;
                double OldXMax = tChart.Axes.Bottom.Maximum;
                double XMid = (OldXMin + OldXMax) / 2;
                double NewXMin = (-XMid * 0.5 + OldXMin) / (0.5);
                double NewXMax = (-XMid * 0.5 + OldXMax) / (0.5);
                tChart.Axes.Bottom.Labels.ValueFormat = "0.000000";
                tChart.Axes.Bottom.Increment = 0.000001;//控制X轴 刻度的增量
                tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);
                aotuzoom--;
            }
            else if (aotuzoom == 1)
            {
                tChart.Axes.Bottom.Labels.ValueFormat = "0.00";
                tChart.Axes.Bottom.SetMinMax(min, max);
                aotuzoom = 0;
            }
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
                tChart.Axes.Bottom.Labels.ValueFormat = "0.000000";
                tChart.Axes.Bottom.Increment = 0.000001;//控制X轴 刻度的增量
                if (e.Delta > 0)
                {
                    if (aotuzoom < 3)
                    {
                        double OldXMin = tChart.Axes.Bottom.CalcPosPoint(e.X) - (aotuzoom == 0 ? 0.5 : (0.5 / aotuzoom));
                        double OldXMax = tChart.Axes.Bottom.CalcPosPoint(e.X) + (aotuzoom == 0 ? 0.5 : (0.5 / aotuzoom));

                        double NewXMin = (XMid * 0.5 + OldXMin) / (1.5);
                        double NewXMax = (XMid * 0.5 + OldXMax) / (1.5);

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

                        tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);
                        aotuzoom--;
                    }
                    else if (aotuzoom == 1)
                    {
                        tChart.Axes.Bottom.Labels.ValueFormat = "0.00";
                        tChart.Axes.Bottom.SetMinMax(min, max);
                        aotuzoom = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 鼠标移动点位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            lbX.Text = e.X.ToString();
            lbY.Text = e.Y.ToString();
        }

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

        /// <summary>
        /// 自带放大缩小控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_Zoomed(object sender, EventArgs e)
        {





        }

        #endregion

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
        int LessPoint = 40;//
        /// <summary>
        /// 总的秒数 默认10秒
        /// </summary>
        int alltime = 10;

        /// <summary>
        ///总点数=10秒/（单点长度*偷点数）
        /// </summary>
        static int linlength = 0;

        /// <summary>
        /// 初始化参数
        /// </summary>
        private void init_Chart_Config(int newalltime, int newLessPoint)
        {
            alltime = newalltime;
            LessPoint = newLessPoint;
        }

        /// <summary>
        ///  //计算1秒 点位数量
        /// </summary>
        int pnum = 0;
        /// <summary>
        /// 单个点位的长度
        /// </summary>
        double newXvalue = 0;
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

        /// <summary>
        /// 放大缩小时 开始区域
        /// </summary>
        double min = 0.0;
        /// <summary>
        /// 放大缩小时 结束区域
        /// </summary>
        double max = 0.0;

        #endregion

        /// <summary>
        /// 初始化图表
        /// </summary>
        private void Chart_Init()
        {
            Chart_Config();//初始化图表
            Chart_Data_Bind();//初始化绑定 线line
            pclChart.Controls.Add(tChart);// 绑定图表位置 
        }
        /// <summary>
        /// 图片配置初始化
        /// </summary>
        private void Chart_Config()
        {
            tChart.Series.Clear();
            //总点数=alltime执行秒数  （默认10秒）/（单点长度*偷点数）
            linlength = (alltime * allnum) / num / LessPoint;
            //计算1秒 点位数量
            pnum = (int)(0.1 * allnum) / num / LessPoint;
            //计算单点的距离
            newXvalue = (double)(1 * num * LessPoint) / allnum;

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


            /// <summary>
            /// 颜色开始区域
            /// </summary>
            colorFrom = 0.0;
            /// <summary>
            /// 颜色结束区域
            /// </summary>
            colorTo = 0.0;

            tChart.Dock = DockStyle.Fill;
            tChart.Aspect.View3D = false;
            tChart.Legend.Visible = false;//显示/隐藏线的注释 

            // tChart.Header.Text = "有载调压开关故障诊断系统波形";

            tChart.Legend.LegendStyle = LegendStyles.Series;
            tChart.Axes.Bottom.Labels.ValueFormat = "0.00";

            //默认10秒
            tChart.Axes.Bottom.SetMinMax(0, alltime);

            tChart.Axes.Bottom.Increment = 1;//控制X轴 刻度的增量

            //tChart.Axes.Bottom.Inverted = true;//控制X轴 顺序还是倒序

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

            min = tChart.Axes.Bottom.Minimum;
            max = tChart.Axes.Bottom.Maximum;
        }

        /// <summary>
        /// 初始化重新查看数据
        /// </summary>
        private void Chart_DataTable_Init()
        {
            Bind_IsDC();
            tChart.Series.Clear();
            tChart.Axes.Custom.Clear();
            tChart.Zoom.Allow = false;
            Event_Chart_Bind();
        }

        /// <summary>
        /// 绑定X轴 10秒
        /// </summary>
        //private void X_Bind()
        //{
        //    vx1[0] = 0;
        //    for (int i = 1; i < linlength; i++)
        //    {
        //        vx1[i] = Math.Round((vx1[i - 1] + newXvalue), 4);
        //    }
        //    vx2 = vx1;
        //    vx3 = vx1;
        //    cx1 = vx1;
        //    cx2 = vx1;
        //    cx3 = vx1;
        //}
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
                axis.Title.Angle = 90;//'标题摆放角度
                axis.Maximum = 10;//最大值
                axis.Minimum = -10;//最小值

                axis.Title.Text = tChart.Series[i].Title;
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
                if (v1)
                {
                    //震动1路 vline1
                    vline1.Title = string.Format("震动曲线{0}", 1);
                    tChart.Series.Add(vline1);
                }
                if (v2)
                {
                    //震动2路 vline2
                    //Line vline2 = new Line();
                    vline2.Title = string.Format("震动曲线{0}", 2);
                    tChart.Series.Add(vline2);
                }
                if (v3)
                {

                    //震动3路 vline3
                    //Line vline3 = new Line();
                    vline3.Title = string.Format("震动曲线{0}", 3);
                    tChart.Series.Add(vline3);
                }
                if (c1)
                {
                    //电流1路 cline1
                    cline1.Title = string.Format("电流曲线{0}", 1);
                    tChart.Series.Add(cline1);
                }
                if (c2)
                {

                    //电流2路 cline2
                    //Line cline2 = new Line();
                    cline2.Title = string.Format("电流曲线{0}", 2);
                    tChart.Series.Add(cline2);
                }
                if (c3)
                {

                    //电流3路 cline3
                    cline3.Title = string.Format("电流曲线{0}", 3);
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

        /// <summary>
        /// 重新加载图表
        /// </summary>
        private void Chart_Data_Lond_Bind()
        {
            try
            {
                //震动1路 vline1
                if (v1)
                {
                    vline1 = new Line();
                    tChart.Series.Add(vline1);

                    vline1.Title = string.Format("震动曲线{0}", 1);
                    vline1.Add(vx1, vy1);
                }
                //震动2路 vline2
                if (v2)
                {
                    vline2 = new Line();
                    tChart.Series.Add(vline2);
                    vline2.Title = string.Format("震动曲线{0}", 2);
                    vline2.Add(vx2, vy2);
                }
                //震动3路 vline3
                if (v3)
                {
                    vline3 = new Line();
                    tChart.Series.Add(vline3);
                    vline3.Title = string.Format("震动曲线{0}", 3);
                    vline3.Add(vx3, vy3);
                }
                //电流1路 cline1
                if (c1)
                {

                    cline1 = new Line();
                    tChart.Series.Add(cline1);
                    cline1.Title = string.Format("电流曲线{0}", 1);
                    cline1.Add(cx1, cy1);
                }
                //电流2路 cline2
                if (c2)
                {
                    cline2 = new Line();
                    tChart.Series.Add(cline2);
                    cline2.Title = string.Format("电流曲线{0}", 2);
                    //cline2.YValues.DataMember = "C2";
                    //cline2.XValues.DataMember = "Xwitdh";
                    //cline2.DataSource = dt;

                    cline2.Add(cx2, cy2);
                }
                //电流3路 cline3
                if (c3)
                {
                    cline3 = new Line();
                    tChart.Series.Add(cline3);
                    cline3.Title = string.Format("电流曲线{0}", 3);
                    //cline3.YValues.DataMember = "C3";
                    //cline3.XValues.DataMember = "Xwitdh";
                    //cline3.DataSource = dt;
                    cline3.Add(cx3, cy3);
                }

                //绘制画布
                AddCustomAxis(tChart.Series.Count);
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
        ConcurrentQueue<Udp_EventArgs> Porint_List = new ConcurrentQueue<Udp_EventArgs>();
        /// <summary>
        /// 存储数据队列
        /// </summary>
        ConcurrentQueue<Udp_EventArgs> Save_Db_Source = new ConcurrentQueue<Udp_EventArgs>();

        /// <summary>
        /// 计量当前数据执行次数 （刷新清除横线时使用 重要） 停止发送数据后要归零
        /// </summary>
        int porintadd = 0;
        /// <summary>
        /// 颜色开始区域
        /// </summary>
        double colorFrom = 0.0;
        /// <summary>
        /// 颜色结束区域
        /// </summary>
        double colorTo = 0.0;

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

            //thread_List = new Thread(Job_Queue);
            if (pub_Test_Plan.GETINFO == "2")
            {
                thread_List = new Thread(Job_Queue_01);
            }
            else
            {
                thread_List = new Thread(Job_Queue_02);
            }
            thread_List.IsBackground = true;
            thread_List.Start();//启动线程

            #endregion

            #region 绘图线程

            ReChart = new Thread(Refresh_Server);
            ReChart.IsBackground = true;
            ReChart.Start();//启动线程

            #endregion

            #region 存储数据
            //if (IsSaveData)
            //{
            Db_Save = new Thread(Test_Xml_Insert);
            Db_Save.IsBackground = true;
            Db_Save.Start();//启动线程
            //}
            #endregion

            Chart_Init();
        }

        /// <summary>
        /// 停止处理图表
        /// </summary>
        private void End_Chart()
        {
            istrue = false;
            porintadd = 0;
            isAbort = false;
        }

        #region 队列数据处理

        /// <summary>
        /// 判断是否刚开始 走图形 是为false 否为true 
        /// </summary>
        bool istrue = false;

        /// <summary>
        /// 是否触发开关存储
        /// </summary>
        bool SCURRENT = false;
        /// <summary>
        /// 触头触发数
        /// </summary>
        int topnum = 0;
        /// <summary>
        /// 总触头数
        /// </summary>
        int alltopnum = 0;

        /// <summary>
        /// 前往后走图形 按时间走波形
        /// </summary>
        private void Job_Queue_01()
        {
            while (isAbort)
            {
                Udp_EventArgs e = new Udp_EventArgs();//初始化 实体
                if (Porint_List.Count > 0)
                {
                    Porint_List.TryDequeue(out e);//取出队里数据并删除
                                                  //截取返回数据
                    if (!string.IsNullOrEmpty(e.Hearder))
                    {
                        string data = e.Msg.Substring(8, e.Msg.Length - 8);

                        int Porints = 80 / LessPoint;
                        double x = new double();
                        double y = new double();

                        for (int i = 0; i < Porints; i++)
                        {
                            Vibration_Current vmodel = new Vibration_Current();
                            //  Vibration_Current vmodel1 = new Vibration_Current();
                            int length = 24 * (i + 1);//截取位置 +1 默认不取第一个点位

                            vmodel.Current1 = data.Substring(0 + length, 4);
                            vmodel.Current2 = data.Substring(4 + length, 4);
                            vmodel.Current3 = data.Substring(8 + length, 4);

                            vmodel.Vibration1 = data.Substring(12 + length, 4);
                            vmodel.Vibration2 = data.Substring(16 + length, 4);
                            vmodel.Vibration3 = data.Substring(20 + length, 4);
                            //计算
                            //double Current1 = Algorithm.Instance.Current_Algorithm_Double(vmodel.Current1);
                            //double Current2 = Algorithm.Instance.Current_Algorithm_Double(vmodel.Current2);
                            //double Current3 = Algorithm.Instance.Current_Algorithm_Double(vmodel.Current3);

                            //double Vibration1 = Algorithm.Instance.Vibration_Algorithm_Double(vmodel.Vibration1);
                            //double Vibration2 = Algorithm.Instance.Vibration_Algorithm_Double(vmodel.Vibration2);
                            //double Vibration3 = Algorithm.Instance.Vibration_Algorithm_Double(vmodel.Vibration3);

                            //否 存储
                            #region 判断电流是否启动 先判断是

                            //判断数据是否超标了
                            if (porintadd >= linlength)
                            {//是停止
                                Thread.Sleep(50);
                                btnStopTest_ItemClick(null, null);
                                return;
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    // Save_Db_Source.Enqueue(e);
                                }
                            }

                            #endregion

                            #region 计算保存新数据

                            x = Get_Pub_S_X();
                            if (v1)
                            {
                                //计算
                                double Vibration1 = Algorithm.Instance.Vibration_Algorithm_Double(vmodel.Vibration1);
                                vx1[porintadd] = x;
                                vy1[porintadd] = Vibration1;
                            }
                            if (v2)
                            {
                                double Vibration2 = Algorithm.Instance.Vibration_Algorithm_Double(vmodel.Vibration2);

                                vx2[porintadd] = x;
                                vy2[porintadd] = Vibration2;
                            }
                            if (v3)
                            {
                                double Vibration3 = Algorithm.Instance.Vibration_Algorithm_Double(vmodel.Vibration3);

                                vx3[porintadd] = x;
                                vy3[porintadd] = Vibration3;
                            }
                            if (c1)
                            {
                                double Current1 = Algorithm.Instance.Current_Algorithm_Double(vmodel.Current1);

                                cx1[porintadd] = x;
                                cy1[porintadd] = Current1;
                            }
                            if (c2)
                            {
                                double Current2 = Algorithm.Instance.Current_Algorithm_Double(vmodel.Current2);
                                cx2[porintadd] = x;
                                cy2[porintadd] = Current2;
                            }
                            if (c3)
                            {
                                double Current3 = Algorithm.Instance.Current_Algorithm_Double(vmodel.Current3);
                                cx3[porintadd] = x;
                                cy3[porintadd] = Current3;
                            }
                            #endregion
                            porintadd++;
                        }
                        #region 数据大于10秒时执行   不需要执行这个
                        //if (pub_Test_Plan.GETINFO == "1")
                        //{
                        //    if (porintadd > linlength - 1)
                        //    {
                        //        porintadd = 0;//归零
                        //        istrue = true;//开启进入标识
                        //        double swidth = 0;//初始位置
                        //        double width = swidth;//结束时位置
                        //        for (int i = 0; i < 500; i++)
                        //        {
                        //            width = Math.Round(width + newXvalue, 4);

                        //            Data_Bind(i, width);
                        //        }
                        //        colorFrom = swidth;
                        //        colorTo = width;
                        //    }
                        //    if (istrue)
                        //    {
                        //        double swidth = Math.Round(vx1[porintadd], 4);//初始位置
                        //        double width = swidth;//结束时位置
                        //        for (int i = 0; i < 500; i++)
                        //        {
                        //            int length = porintadd + i + 1;
                        //            if (length < linlength - 1)
                        //            {
                        //                width = Math.Round(width + newXvalue, 4);
                        //                Data_Bind(i, width);
                        //            }
                        //            else
                        //            {
                        //                break;
                        //            }
                        //        }

                        //        colorFrom = swidth;
                        //        colorTo = width;
                        //    }
                        //}
                        #endregion
                    }
                }
            }
        }
        /// <summary>
        /// 获取X轴下一个点位的位置
        /// </summary>
        /// <returns></returns>
        private double Get_Pub_S_X()
        {
            try
            {
                double x = 0;
                if (porintadd == 0)
                {
                    if (v1)
                    {
                        x = vx1[porintadd] + newXvalue;
                    }
                    else if (v2)
                    {
                        x = vx2[porintadd] + newXvalue;
                    }
                    else if (v3)
                    {
                        x = vx3[porintadd] + newXvalue;
                    }
                    if (c1)
                    {
                        x = cx1[porintadd] + newXvalue;
                    }
                    else if (c2)
                    {
                        x = cx2[porintadd] + newXvalue;
                    }
                    else if (c3)
                    {
                        x = cx3[porintadd] + newXvalue;
                    }

                }
                else
                {
                    if (v1)
                    {
                        x = vx1[porintadd - 1] + newXvalue;
                    }
                    else if (v2)
                    {
                        x = vx2[porintadd - 1] + newXvalue;
                    }
                    else if (v3)
                    {
                        x = vx3[porintadd - 1] + newXvalue;
                    }
                    if (c1)
                    {
                        x = cx1[porintadd - 1] + newXvalue;
                    }
                    else if (c2)
                    {
                        x = cx2[porintadd - 1] + newXvalue;
                    }
                    else if (c3)
                    {
                        x = cx3[porintadd - 1] + newXvalue;
                    }
                }


                return x;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 前往后走图形电流触发波形
        /// </summary>
        private void Job_Queue_02()
        {
            while (isAbort)
            {
                Udp_EventArgs e = new Udp_EventArgs();//初始化 实体
                if (Porint_List.Count > 0)
                {
                    Porint_List.TryDequeue(out e);//取出队里数据并删除
                                                  //截取返回数据
                    
                    if (!string.IsNullOrEmpty(e.Hearder))
                    {
                        string data = e.Msg.Substring(8, e.Msg.Length - 8);

                        int Porints = 80 / LessPoint;
                        double x = new double();
                        double y = new double();


                        for (int i = 0; i < Porints; i++)
                        {
                            Vibration_Current vmodel = new Vibration_Current();
                            Vibration_Current vmodel1 = new Vibration_Current();
                            int length = 24 * (i + 1);//截取位置 +1 默认不取第一个点位

                            vmodel.Current1 = data.Substring(0 + length, 4);
                            vmodel.Current2 = data.Substring(4 + length, 4);
                            vmodel.Current3 = data.Substring(8 + length, 4);

                            vmodel.Vibration1 = data.Substring(12 + length, 4);
                            vmodel.Vibration2 = data.Substring(16 + length, 4);
                            vmodel.Vibration3 = data.Substring(20 + length, 4);



                            //否 存储
                            #region 判断电流是否启动 电流存储
                            if (pub_Test_Plan.GETINFO == "1")
                            {
                                //double setSCURRENT = Convert.ToDouble(pub_Test_Plan.SCURRENT);
                                //double setECURRENT = Convert.ToDouble(pub_Test_Plan.ECURRENT);
                                ////开始存数据
                                //if (Current1 >= setSCURRENT && !SCURRENT)
                                //{
                                //    SCURRENT = false;
                                //}
                                ////结束存数据
                                //if (Current1 >= setECURRENT && SCURRENT)
                                //{
                                //    topnum += 1;
                                //    SCURRENT = true;
                                //}

                                //if (SCURRENT)
                                //{
                                //    if (i == 0)
                                //    {
                                //        Save_Db_Source.Enqueue(e);
                                //    }
                                //}

                            }
                            #endregion
                            #region 计算保存新数据
                            //if (porintadd == 0)
                            //{
                            //    x = vx1[porintadd] + newXvalue;//
                            //}
                            //else
                            //{
                            //    x = vx1[porintadd - 1] + newXvalue;//
                            //}
                            x = Get_Pub_S_X();
                            if (v1)
                            {
                                //计算
                                double Vibration1 = Algorithm.Instance.Vibration_Algorithm_Double(vmodel.Vibration1);

                                vx1[porintadd] = x;
                                vy1[porintadd] = Vibration1;
                            }
                            if (v2)
                            {
                                double Vibration2 = Algorithm.Instance.Vibration_Algorithm_Double(vmodel.Vibration2);

                                vx2[porintadd] = x;
                                vy2[porintadd] = Vibration2;
                            }
                            if (v3)
                            {
                                double Vibration3 = Algorithm.Instance.Vibration_Algorithm_Double(vmodel.Vibration3);

                                vx3[porintadd] = x;
                                vy3[porintadd] = Vibration3;
                            }
                            if (c1)
                            {
                                double Current1 = Algorithm.Instance.Current_Algorithm_Double(vmodel.Current1);

                                cx1[porintadd] = x;
                                cy1[porintadd] = Current1;
                            }
                            if (c2)
                            {
                                double Current2 = Algorithm.Instance.Current_Algorithm_Double(vmodel.Current2);
                                cx2[porintadd] = x;
                                cy2[porintadd] = Current2;
                            }
                            if (c3)
                            {
                                double Current3 = Algorithm.Instance.Current_Algorithm_Double(vmodel.Current3);
                                cx3[porintadd] = x;
                                cy3[porintadd] = Current3;
                            }

                            #endregion
                            porintadd++;


                        }


                        #region 数据大于10秒时执行   不需要执行这个

                        if (porintadd > linlength - 1)
                        {
                            porintadd = 0;//归零
                            istrue = true;//开启进入标识
                            double swidth = 0;//初始位置
                            double width = swidth;//结束时位置
                            for (int i = 0; i < 500; i++)
                            {
                                width = Math.Round(width + newXvalue, 4);
                                Data_Bind(i, width);
                            }
                            colorFrom = swidth;
                            colorTo = width;
                        }
                        if (istrue)
                        {

                            //公共初始位置
                            double pubwidth = 0;

                            if (v1)
                            {
                                pubwidth = vx1[porintadd];
                            }
                            else if (v2)
                            {
                                pubwidth = vx2[porintadd];
                            }
                            else if (v3)
                            {
                                pubwidth = vx3[porintadd];
                            }
                            if (c1)
                            {
                                pubwidth = cx1[porintadd];
                            }
                            else if (c2)
                            {
                                pubwidth = cx2[porintadd];
                            }
                            else if (c3)
                            {
                                pubwidth = cx3[porintadd];
                            }
                            //double swidth = Math.Round(vx1[porintadd], 4);//初始位置
                            double swidth = Math.Round(pubwidth, 4);//初始位置
                            double width = swidth;//结束时位置
                            for (int i = 0; i < 500; i++)
                            {
                                int length = porintadd + i + 1;
                                if (length < linlength - 1)
                                {
                                    width = Math.Round(width + newXvalue, 4);
                                    Data_Bind(i, width);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            colorFrom = swidth;
                            colorTo = width;
                        }
                        #endregion
                    }
                }
            }
        }

        private void Data_Bind(int index, double width)
        {
            vx1[porintadd + index + 1] = width;
            vy1[porintadd + index + 1] = 0.0;

            vx2[porintadd + index + 1] = width;
            vy2[porintadd + index + 1] = 0.0;

            vx3[porintadd + index + 1] = width;
            vy3[porintadd + index + 1] = 0.0;

            cx1[porintadd + index + 1] = width;
            cy1[porintadd + index + 1] = 0.0;

            cx2[porintadd + index + 1] = width;
            cy2[porintadd + index + 1] = 0.0;

            cx3[porintadd + index + 1] = width;
            cy3[porintadd + index + 1] = 0.0;

        }

        /// <summary>
        /// 后往前走数据
        /// </summary>
        private void Job_Queue()
        {
            while (isAbort)
            {
                Udp_EventArgs e = new Udp_EventArgs();//初始化 实体
                if (Porint_List.Count > 0)
                {
                    Porint_List.TryDequeue(out e);//取出队里数据并删除
                                                  //截取返回数据
                    if (!string.IsNullOrEmpty(e.Hearder))
                    {
                        string data = e.Msg.Substring(8, e.Msg.Length - 8);
                        //string head = e.Msg.Substring(4, 4);
                        #region 数据大于 alltime 当前秒数 （默认10秒）时执行
                        //数组里面添加
                        if (porintadd >= linlength)//
                        {
                            for (int i = 0; i < 1250; i++)
                            {
                                #region 对数组操作
                                Porint_Bind(i);
                                #endregion

                                double width = alltime;
                                colorTo = alltime;//从底部插入颜色区域

                                vx1[linlength - 1] = width;
                                vy1[linlength - 1] = 0.0;

                                vx2[linlength - 1] = width;
                                vy2[linlength - 1] = 0.0;

                                vx3[linlength - 1] = width;
                                vy3[linlength - 1] = 0.0;

                                cx1[linlength - 1] = width;
                                cy1[linlength - 1] = 0.0;

                                cx2[linlength - 1] = width;
                                cy2[linlength - 1] = 0.0;

                                cx3[linlength - 1] = width;
                                cy3[linlength - 1] = 0.0;

                            }
                            porintadd = 0;//初始化标识

                        }
                        #endregion
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

                            #region 对数组操作
                            Porint_Bind();
                            #endregion

                            double width = alltime;

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
                            porintadd++;
                        }
                    }
                }
            }
        }

        private void Job_Queue1()
        {
            while (isAbort)
            {
                Udp_EventArgs e = new Udp_EventArgs();//初始化 实体
                if (Porint_List.Count > 0)
                {
                    Porint_List.TryDequeue(out e);//取出队里数据并删除
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


                            #region 对数组操作
                            Porint_Bind();
                            #endregion

                            double width = alltime;

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
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绑定数据点
        /// </summary>
        private void Porint_Bind(int nonenum = -1)
        {
            double newPvalue = (double)(1 * num * LessPoint) / allnum;
            if (colorTo > 0)//判断颜色最大区域大于0才执行
            {
                if (colorFrom <= 0 && nonenum >= 0)// 数据输入完毕进入结束 输入
                {
                    colorFrom = colorTo;
                }
                else if (nonenum >= 0)//还在插入空白数据 只修改开始区域
                {
                    colorFrom = colorFrom - newPvalue;//计算开始区域
                }
                else//已经插入完毕开始往前移动
                {
                    if (colorFrom > 0)
                    {
                        colorFrom = colorFrom - newPvalue;//计算开始区域
                    }
                    else if (colorFrom < 0)
                    {
                        colorFrom = 0;
                    }

                    if (colorTo > 0)
                    {
                        colorTo = colorTo - newPvalue;//计算结束区域
                    }

                }
            }

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
                    Thread.Sleep(150);//多图标使用150单图表使用 150/6 15
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
                vline1.XValues.Clear();
                vline1.YValues.Clear();
                vline1.Add(vx1, vy1);
                if (colorTo > 0 || colorFrom > 0)
                {
                    ValueList vlist = vline1.ValuesLists[0];
                    vline1.Colors.Clear();
                    vline1.ColorRange(vlist, colorFrom, colorTo, Color.Red);
                }
                //控制颜色
                //tChart.Refresh();
            }
            if (v2)
            {
                vline2.XValues.Clear();
                vline2.YValues.Clear();
                vline2.Add(vx2, vy2);
                //控制颜色
                if (colorTo > 0 || colorFrom > 0)
                {
                    ValueList vlist = vline2.ValuesLists[0];
                    vline2.Colors.Clear();
                    vline2.ColorRange(vlist, colorFrom, colorTo, Color.Red);
                }
            }
            if (v3)
            {
                vline3.XValues.Clear();
                vline3.YValues.Clear();
                vline3.Add(vx3, vy3);
                //控制颜色
                if (colorTo > 0 || colorFrom > 0)
                {
                    ValueList vlist = vline3.ValuesLists[0];
                    vline3.Colors.Clear();
                    vline3.ColorRange(vlist, colorFrom, colorTo, Color.Red);
                }
            }
            if (c1)
            {
                cline1.XValues.Clear();
                cline1.YValues.Clear();
                cline1.Add(cx1, cy1);
                //控制颜色
                if (colorTo > 0 || colorFrom > 0)
                {
                    ValueList vlist = cline1.ValuesLists[0];
                    cline1.Colors.Clear();
                    cline1.ColorRange(vlist, colorFrom, colorTo, Color.Red);
                }
            }
            if (c2)
            {
                cline2.XValues.Clear();
                cline2.YValues.Clear();
                cline2.Add(cx2, cy2);
                //控制颜色
                if (colorTo > 0 || colorFrom > 0)
                {
                    ValueList vlist = cline2.ValuesLists[0];
                    cline2.Colors.Clear();
                    cline2.ColorRange(vlist, colorFrom, colorTo, Color.Red);
                }
            }
            if (c3)
            {
                cline3.XValues.Clear();
                cline3.YValues.Clear();
                cline3.Add(cx3, cy3);
                //控制颜色
                if (colorTo > 0 || colorFrom > 0)
                {
                    ValueList vlist = cline3.ValuesLists[0];
                    cline3.Colors.Clear();
                    cline3.ColorRange(vlist, colorFrom, colorTo, Color.Red);
                }
            }
        }

        #endregion

        #region SQL 数据库操作作废
        //public void Test_Data_insert()
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            if (Db_Source.Count > 0)
        //            {

        //                Udp_EventArgs e = new Udp_EventArgs();

        //                Db_Source.TryDequeue(out e);//取出队里数据并删除

        //                DataModel model = new DataModel();

        //                model.head = e.Msg.Substring(4, 4); ;
        //                model.text = e.Msg;
        //                model.id = Convert.ToInt32(model.head, 16);
        //                // Db_Action.Instance.Test_Data_insert(model);
        //                ListToText.Instance.WriteListToTextFile1(e.Msg + "|");

        //                Thread.Sleep(1);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 生成数据库
        /// </summary>
        private void CreateDb()
        {
            try
            {
                bool IsCreate = SQLiteHelper.NewDbFile();//生成SQL文件
                DataSet ds = Db_Select.Instance.Get_All_Table();
                if (IsCreate || ds.Tables == null || ds.Tables[0].Rows.Count <= 0)
                {
                    string DbStr = Create_Table.Instance.Create_TEST_CONFIGE();
                    SQLiteHelper.NewTable(DbStr);
                    DbStr = Create_Table.Instance.Create_TEST_DATA();
                    SQLiteHelper.NewTable(DbStr);
                }
                else
                {
                    DataTable dt = ds.Tables[0];
                    DataRow[] newdt = dt.Select(" name='TEST_CONFIGE'");
                    if (newdt.Length == 0)
                    {
                        string DbStr = Create_Table.Instance.Create_TEST_CONFIGE();
                        SQLiteHelper.NewTable(DbStr);
                    }
                    newdt = dt.Select(" name='TEST_DATA'");
                    if (newdt.Length == 0)
                    {
                        string DbStr = Create_Table.Instance.Create_TEST_DATA();
                        SQLiteHelper.NewTable(DbStr);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region Xml 数据操作
        /// <summary>
        /// 保存数据到XML
        /// </summary>
        public void Test_Xml_Insert()
        {
            try
            {
                int i = 1;
                while (isAbort || Save_Db_Source.Count > 0)
                {
                    if (Save_Db_Source.Count > 0)
                    {
                        Udp_EventArgs e = new Udp_EventArgs();

                        Save_Db_Source.TryDequeue(out e);//取出队里数据并删除
                        Xml_Node_Model model = new Xml_Node_Model();
                        model.Id = e.Msg.Substring(4, 4); ;
                        model.DataSource = e.Msg;
                        model.Data = new List<Xml_Element_Model>();

                        XmlHelper.Insert(model);
                        Thread.Sleep(1);
                        if (i == 2000)
                        {
                            XmlHelper.Save();
                            i = 1;
                        };

                        Invoke(new ThreadStart(delegate ()
                        {
                            try
                            {
                                LBxmlCount.Text = Save_Db_Source.Count.ToString();
                            }
                            catch (Exception ex)
                            {

                            }
                        }));

                        i++;
                    }

                }
                if (!isAbort)
                {
                    XmlHelper.Save();
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

        #region 页面控件数据操作

        #region 绑定测试计划
        /// <summary>
        /// 绑定测试计划
        /// </summary>
        public void Tester_List_Bind()
        {
            List<Test_Plan> list = Db_Select.Instance.All_Test_Cofig_Get();
            treeList.DataSource = list;



            treeList.Refresh();
            treeList.ExpandAll();
        }

        #endregion

        /// <summary>
        /// 单击树 行获取 行信息 并修改图表标题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            // treeList.OptionsView.ShowIndicator = false;
            if (e.Node.Selected)
            {
                if (treeList.Appearance.FocusedCell.BackColor != Color.SteelBlue)
                {
                    treeList.Appearance.FocusedCell.BackColor = Color.SteelBlue;
                };
                publicnode = e.Node;
                pub_Test_Plan = Test_Plan_Bind(publicnode);
                tChart.Header.Text = pub_Test_Plan.DVNAME;

                if (pub_Test_Plan.GETINFO == "2")
                {
                    alltime = string.IsNullOrEmpty(pub_Test_Plan.TIME_UNIT) ? alltime : Convert.ToInt32(pub_Test_Plan.TIME_UNIT);
                    init_Chart_Config(alltime, 40);
                    Chart_Init();
                }
                else
                {
                    init_Chart_Config(10, 40);
                    //以电流达到1A存数据 小于等于0.1A结束
                    topnum = Convert.ToInt32(pub_Test_Plan.SPLACE);
                    Chart_Init();
                }
            }
        }

        /// <summary>
        /// 双击修改 该条内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_MouseDown(object sender, MouseEventArgs e)
        {
            //双击左键弹出页面用
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //确定双击区域是否在 treelist范围里面
                TreeListHitInfo hitInfo = (sender as TreeList).CalcHitInfo(new Point(e.X, e.Y));
                TreeListNode node = hitInfo.Node;

                if (node != null)
                {
                    //取得选定行信息  
                    Test_Plan model = Test_Plan_Bind(node);
                    model.ISEDIT = "2";
                    if (model.PARENTID != "0")
                    {
                        return;
                    }
                    using (FmAddTest form = new FmAddTest(model))
                    {
                        form.ShowDialog();
                    }
                    Tester_List_Bind();

                    Set_Foucs(model.ID);
                }
            }

            //以下是鼠标右键删除用
            if (e.Button == MouseButtons.Right)
            {
                treeList.ContextMenuStrip = null;
                TreeListHitInfo hInfo = treeList.CalcHitInfo(new Point(e.X, e.Y));
                TreeListNode node = hInfo.Node;
                treeList.FocusedNode = node;
                if (node != null)
                {
                    Point point = new Point(e.X, e.Y); //右键菜单弹出的位置
                    popupMenu.ShowPopup(barManager1, point);
                }
            }
        }
        /// <summary>
        /// treelist 鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_MouseUp(object sender, MouseEventArgs e)
        {
            //单击选中并且改变背景色
            if (e.Button == MouseButtons.Right
                    && ModifierKeys == Keys.None
                    && treeList.State == TreeListState.Regular)
            {
                TreeList tree = sender as TreeList;
                Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
                TreeListHitInfo hitInfo = tree.CalcHitInfo(e.Location);
                if (hitInfo.HitInfoType == HitInfoType.Cell)
                {
                    tree.SetFocusedNode(hitInfo.Node);
                }
                if (tree.FocusedNode != null)
                {
                    popupMenu.ShowPopup(p);
                }
            }
        }
        /// <summary>
        /// 绑定选中行数据到实体
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Test_Plan Test_Plan_Bind(TreeListNode node)
        {
            try
            {
                Test_Plan model = new Test_Plan();
                model.DVNAME = node.GetValue("DVNAME").ToString();
                model.DVPOSITION = node.GetValue("DVPOSITION").ToString();
                model.DVID = node.GetValue("DVID").ToString();
                model.TESTER = node.GetValue("TESTER").ToString();
                model.OLTC_TS = node.GetValue("OLTC_TS").ToString();

                model.CONTACT_NUM = node.GetValue("CONTACT_NUM").ToString();
                model.TEST_NUM = node.GetValue("TEST_NUM").ToString();
                model.SPLACE = node.GetValue("SPLACE").ToString();
                model.OILTEMP = node.GetValue("OILTEMP").ToString();
                model.TEST_TIME = node.GetValue("TEST_TIME").ToString();

                model.TEST_TYPE = node.GetValue("TEST_TYPE").ToString();
                model.GETINFO = node.GetValue("GETINFO").ToString();
                model.TESTSTAGE = node.GetValue("TESTSTAGE").ToString();
                model.DJUST = node.GetValue("DJUST").ToString();
                model.DESCRIBE = node.GetValue("DESCRIBE").ToString();

                model.SCURRENT = node.GetValue("SCURRENT").ToString();
                model.ECURRENT = node.GetValue("ECURRENT").ToString();
                model.TIME_UNIT = node.GetValue("TIME_UNIT").ToString();
                model.V1 = node.GetValue("V1").ToString();
                model.V2 = node.GetValue("V2").ToString();

                model.V3 = node.GetValue("V3").ToString();
                model.C1 = node.GetValue("C1").ToString();
                model.C2 = node.GetValue("C2").ToString();
                model.C3 = node.GetValue("C3").ToString();
                model.PARENTID = node.GetValue("PARENTID").ToString();
                model.ID = node.GetValue("ID").ToString();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定节点焦点
        /// </summary>
        /// <param name="Id"></param>
        private void Set_Foucs(string Id)
        {
            List<TreeListNode> list = treeList.GetNodeList();
            foreach (TreeListNode n in list)
            {
                if (n.GetValue("ID").ToString() == Id)
                {
                    treeList.SetFocusedNode(n);
                    treeList.Refresh();
                    break;
                }
            }
        }

        #endregion

        private void treeList_GetSelectImage(object sender, GetSelectImageEventArgs e)
        {
            if (e.Node.GetValue("PARENTID").ToString() == "0")
            {
                e.NodeImageIndex = 2;
            }
            else
            {
                e.NodeImageIndex = 3;
            }

        }

        private void treeList_CustomDrawNodeImages(object sender, CustomDrawNodeImagesEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.Expanded)
                {
                    e.SelectImageIndex = 2;
                    return;
                }
                e.SelectImageIndex = 1;
            }
            else
            {
                e.SelectImageIndex = 0;
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Test_Plan model = Test_Plan_Bind(publicnode);
            Db_Action.Instance.Test_Confige_Del(model);

            treeList.Refresh();
        }


    }
}