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
        bool isCharAbort = true;// 控制图表刷新


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
            isCharAbort = true;
            isAbort = true;
            #region 处理队列数据
            //thread_List = new Thread(Job_Queue);
            //thread_List = new Thread(Job_Queue1);
            thread_List = new Thread(Job_Queue2);


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


                //if (vline1.Count > 13500 && isAbort)//取12500个包感觉不对
                //{
                //    //isAbort = false;
                //    //sendUdp(agreement._3_CMD_STOPTESTER);
                //}
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
        int allnum = 100000;//将显示宽度转换为整数
        int num = 1;//数据显示宽度  1秒=0.000001秒* 800微秒  0.0008毫秒 0.000001 * 800
                    //1个数据包 800微秒 0.0008  一个包80个数据点  每个点 10微秒 0.00001秒 

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

        Line vline1 = new Line();//震动1
        Line vline2 = new Line();//震动2
        Line vline3 = new Line();//震动3
        Line cline1 = new Line();//电流1
        Line cline2 = new Line();//电流2
        Line cline3 = new Line();//电流3
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

                axis.Maximum = 5;//最大值
                axis.Minimum = -5;//最小值
                tChart.Axes.Custom.Add(axis);
                listBaseLine[i].CustomVertAxis = axis;
            }
        }

        /// <summary>
        /// 初始化图表
        /// </summary>
        private void Chart_Init()
        {
            //总点数=10秒/（单点长度*偷点数）
            linlength = (10 * allnum) / num / LessPoint;
            //计算1秒 点位数量
            pnum =(int)(0.1 * allnum) / num / LessPoint;

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

                //vline1.Title = string.Format("震动曲线{0}", 1);
                //vline1.YValues.DataMember = "Vibration1";
                //vline1.XValues.DataMember = "width";
                // vline1.DataSource = vcdt1;

                //震动2路 vline2
                //Line vline2 = new Line();
                tChart.Series.Add(vline2);
                //vline2.Title = string.Format("震动曲线{0}", 2);
                //vline2.YValues.DataMember = "vibration2";
                //vline2.XValues.DataMember = "width";
                //vline2.DataSource = dt;

                //震动3路 vline3
                //Line vline3 = new Line();
                tChart.Series.Add(vline3);
                //vline3.Title = string.Format("震动曲线{0}", 3);
                //vline3.YValues.DataMember = "vibration3";
                //vline3.XValues.DataMember = "width";
                //vline3.DataSource = dt;

                //电流1路 cline1
                tChart.Series.Add(cline1);
                //cline1.Title = string.Format("电流曲线{0}", 1);
                //cline1.YValues.DataMember = "Current1";
                //cline1.XValues.DataMember = "width";
                //  cline1.DataSource = vcdt1;

                //电流2路 cline2
                //Line cline2 = new Line();
                tChart.Series.Add(cline2);
                //cline2.Title = string.Format("电流曲线{0}", 2);
                //cline2.YValues.DataMember = "current2";
                //cline2.XValues.DataMember = "width";
                //cline2.DataSource = dt;

                //电流3路 cline3
                //Line cline3 = new Line();
                tChart.Series.Add(cline3);
                //cline3.Title = string.Format("电流曲线{0}", 3);
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

        #region 全局XY数据

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
        static int jsq = 0;//计数器 当存储等于1秒时刷一次

        bool islength = true;//数据是否走完 true否 false是

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
                                                 //截取返回数据
                    if (!string.IsNullOrEmpty(e.Hearder))
                    {
                        string data = e.Msg.Substring(8, e.Msg.Length - 8);
                        string head = e.Msg.Substring(4, 4);
                        DataModel model = new DataModel();
                        model.id = Convert.ToInt32(head, 16);
                        model.head = head;
                        model.text = e.Msg;
                        model.old_data = new List<Vibration_Current>();
                        model.new_data = new List<Vibration_Current>();

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

                            model.old_data.Add(vmodel);
                            model.new_data.Add(vmodel1);


                            if (vline1.Count < linlength && islength)
                            {
                                string vwidth1 = Algorithm.Instance.Less_Porint_Algorithm(vline1.Count, num, LessPoint, allnum);//震动1长度
                                string vwidth2 = Algorithm.Instance.Less_Porint_Algorithm(vline2.Count, num, LessPoint, allnum);//震动2长度
                                string vwidth3 = Algorithm.Instance.Less_Porint_Algorithm(vline3.Count, num, LessPoint, allnum);//震动3长度
                                string cwidth1 = Algorithm.Instance.Less_Porint_Algorithm(cline1.Count, num, LessPoint, allnum);//电流1长度
                                string cwidth2 = Algorithm.Instance.Less_Porint_Algorithm(cline2.Count, num, LessPoint, allnum);//电流2长度
                                string cwidth3 = Algorithm.Instance.Less_Porint_Algorithm(cline3.Count, num, LessPoint, allnum);//电流3长度
                                #region line 动态刷新图表数据源

                                if (this.v1.Checked)
                                {
                                    vline1.Add(Convert.ToDouble(vwidth1), Convert.ToDouble(vmodel1.Vibration1));
                                }
                                if (this.v2.Checked)
                                {

                                    vline2.Add(Convert.ToDouble(vwidth2), Convert.ToDouble(vmodel1.Vibration2));
                                }
                                if (this.v3.Checked)
                                {
                                    vline3.Add(Convert.ToDouble(vwidth3), Convert.ToDouble(vmodel1.Vibration3));
                                }
                                if (this.c1.Checked)
                                {
                                    cline1.Add(Convert.ToDouble(cwidth1), Convert.ToDouble(vmodel1.Current1));
                                }
                                if (this.c2.Checked)
                                {
                                    cline2.Add(Convert.ToDouble(cwidth2), Convert.ToDouble(vmodel1.Current2));
                                }
                                if (this.c3.Checked)
                                {
                                    cline3.Add(Convert.ToDouble(cwidth3), Convert.ToDouble(vmodel1.Current3));
                                }
                                #endregion
                            }
                            else //数据走满后 重新增加数据
                            {
                                // pnum = num * LessPoint;

                                islength = false;
                                isCharAbort = false;
                                //for (int j = 0; j < pnum; j++)
                                //{
                                #region 清除过时数据
                                //vline1.Delete(0);
                                //vline2.Delete(0);
                                //vline3.Delete(0);
                                //cline1.Delete(0);
                                //cline2.Delete(0);
                                //cline3.Delete(0);
                                #endregion
                                #region 计算保存新数据

                                //string vwidth1 = Algorithm.Instance.Less_Porint_Algorithm(linlength - pnum + j, num, LessPoint, allnum);//震动1长度
                                //string vwidth2 = Algorithm.Instance.Less_Porint_Algorithm(linlength - pnum + j, num, LessPoint, allnum);//震动2长度
                                //string vwidth3 = Algorithm.Instance.Less_Porint_Algorithm(linlength - pnum + j, num, LessPoint, allnum);//震动3长度
                                //string cwidth1 = Algorithm.Instance.Less_Porint_Algorithm(linlength - pnum + j, num, LessPoint, allnum);//电流1长度
                                //string cwidth2 = Algorithm.Instance.Less_Porint_Algorithm(linlength - pnum + j, num, LessPoint, allnum);//电流2长度
                                //string cwidth3 = Algorithm.Instance.Less_Porint_Algorithm(linlength - pnum + j, num, LessPoint, allnum);//电流3长度

                                string width = "10.00";


                                double vx1 = Convert.ToDouble(width);
                                double vy1 = Convert.ToDouble(vmodel1.Vibration1);

                                double vx2 = Convert.ToDouble(width);
                                double vy2 = Convert.ToDouble(vmodel1.Vibration2);

                                double vx3 = Convert.ToDouble(width);
                                double vy3 = Convert.ToDouble(vmodel1.Vibration3);

                                double cx1 = Convert.ToDouble(width);
                                double cy1 = Convert.ToDouble(vmodel1.Current1);

                                double cx2 = Convert.ToDouble(width);
                                double cy2 = Convert.ToDouble(vmodel1.Current2);

                                double cx3 = Convert.ToDouble(width);
                                double cy3 = Convert.ToDouble(vmodel1.Current3);

                                #endregion
                                //}
                                #region 直接操作line线
                                for (int j = 0; j < vline1.Count - 1; j++)//先将已有数据平移，再将新增数据加入
                                {
                                    //往前平移距离 当前序号 j 乘以 num 位移距离 乘以偷点数 LessPoint  除以 到整数位的数量 allnum
                                    //double range = Math.Round((double)(j * num * LessPoint) / allnum, allnum.ToString().Length);
                                    double newXvalue = (double)(j * num * LessPoint) / allnum;
                                    if (this.v1.Checked)
                                    {
                                        var dd = vline1.XValues[j];
                                        vline1.XValues[j] = newXvalue;
                                        vline1.YValues[j] = vline1.YValues[j + 1];
                                    }
                                    if (this.v2.Checked)
                                    {
                                        vline2.XValues[j] = newXvalue;
                                        vline2.YValues[j] = vline2.YValues[j + 1];
                                    }
                                    if (this.v3.Checked)
                                    {
                                        vline3.XValues[j] = newXvalue;
                                        vline3.YValues[j] = vline3.YValues[j + 1];
                                    }
                                    if (this.c1.Checked)
                                    {
                                        cline1.XValues[j] = newXvalue;
                                        cline1.YValues[j] = cline1.YValues[j + 1];
                                    }
                                    if (this.c2.Checked)
                                    {
                                        cline2.XValues[j] = newXvalue;
                                        cline2.YValues[j] = cline2.YValues[j + 1];
                                    }
                                    if (this.c3.Checked)
                                    {
                                        cline3.XValues[j] = newXvalue;
                                        cline3.YValues[j] = cline3.YValues[j + 1];
                                    }
                                }
                                #endregion


                                //再将新增数据加入
                                if (this.v1.Checked)
                                {
                                    vline1.XValues[vline1.Count - 1] = vx1;
                                    vline1.YValues[vline1.Count - 1] = vy1;
                                }
                                if (this.v2.Checked)
                                {
                                    //vline2.Add(vx2, vy2);
                                    vline2.XValues[vline2.Count - 1] = vx2;
                                    vline2.YValues[vline2.Count - 1] = vy2;

                                }
                                if (this.v3.Checked)
                                {
                                    // vline3.Add(vx3, vy3);
                                    vline3.XValues[vline3.Count - 1] = vx3;
                                    vline3.YValues[vline3.Count - 1] = vy3;
                                }
                                if (this.c1.Checked)
                                {
                                    //cline1.Add(cx1, cy1);
                                    cline1.XValues[cline1.Count - 1] = cx1;
                                    cline1.YValues[cline1.Count - 1] = cy1;
                                }
                                if (this.c2.Checked)
                                {
                                    //cline2.Add(cx2, cy2);
                                    cline2.XValues[cline2.Count - 1] = cx2;
                                    cline2.YValues[cline2.Count - 1] = cy2;
                                }
                                if (this.c3.Checked)
                                {
                                    //cline3.Add(cx3, cy3);
                                    cline3.XValues[cline3.Count - 1] = cx3;
                                    cline3.YValues[cline3.Count - 1] = cy3;
                                }
                                //异步方法
                                this.Invoke(new ThreadStart(delegate ()
                                {
                                    tChart.Refresh();
                                    string path = AppDomain.CurrentDomain.BaseDirectory;
                                    ListToText.Instance.WriteListToTextFile(DateTime.Now.ToString("O"), path);
                                }));

                            }

                            //list.Add(model);
                        }
                    }

                }
            }
        }
        private void Job_Queue1(object time)
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
                        string head = e.Msg.Substring(4, 4);
                        DataModel model = new DataModel();
                        model.id = Convert.ToInt32(head, 16);
                        model.head = head;
                        model.text = e.Msg;
                        model.old_data = new List<Vibration_Current>();
                        model.new_data = new List<Vibration_Current>();

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

                            model.old_data.Add(vmodel);
                            model.new_data.Add(vmodel1);


                            if (vline1.Count < linlength && islength)
                            {
                                string vwidth1 = Algorithm.Instance.Less_Porint_Algorithm(vline1.Count, num, LessPoint, allnum);//震动1长度
                                string vwidth2 = Algorithm.Instance.Less_Porint_Algorithm(vline2.Count, num, LessPoint, allnum);//震动2长度
                                string vwidth3 = Algorithm.Instance.Less_Porint_Algorithm(vline3.Count, num, LessPoint, allnum);//震动3长度
                                string cwidth1 = Algorithm.Instance.Less_Porint_Algorithm(cline1.Count, num, LessPoint, allnum);//电流1长度
                                string cwidth2 = Algorithm.Instance.Less_Porint_Algorithm(cline2.Count, num, LessPoint, allnum);//电流2长度
                                string cwidth3 = Algorithm.Instance.Less_Porint_Algorithm(cline3.Count, num, LessPoint, allnum);//电流3长度
                                #region line 动态刷新图表数据源
                                int line = vline1.Count;//定位
                                if (this.v1.Checked)
                                {
                                    vline1.Add(Convert.ToDouble(vwidth1), Convert.ToDouble(vmodel1.Vibration1));

                                    var dd = vx1.Length;
                                    vx1[line] = Convert.ToDouble(vwidth1);
                                    vy1[line] = Convert.ToDouble(vmodel1.Vibration1);

                                }
                                if (this.v2.Checked)
                                {
                                    vline2.Add(Convert.ToDouble(vwidth2), Convert.ToDouble(vmodel1.Vibration2));
                                    vx2[line] = Convert.ToDouble(vwidth2);
                                    vy2[line] = Convert.ToDouble(vmodel1.Vibration2);
                                }
                                if (this.v3.Checked)
                                {
                                    vline3.Add(Convert.ToDouble(vwidth3), Convert.ToDouble(vmodel1.Vibration3));
                                    vx3[line] = Convert.ToDouble(vwidth3);
                                    vy3[line] = Convert.ToDouble(vmodel1.Vibration3);
                                }
                                if (this.c1.Checked)
                                {
                                    cline1.Add(Convert.ToDouble(cwidth1), Convert.ToDouble(vmodel1.Current1));
                                    cx1[line] = Convert.ToDouble(cwidth1);
                                    cy1[line] = Convert.ToDouble(vmodel1.Current1);
                                }
                                if (this.c2.Checked)
                                {
                                    cline2.Add(Convert.ToDouble(cwidth2), Convert.ToDouble(vmodel1.Current2));
                                    cx2[line] = Convert.ToDouble(cwidth2);
                                    cy2[line] = Convert.ToDouble(vmodel1.Current2);
                                }
                                if (this.c3.Checked)
                                {
                                    cline3.Add(Convert.ToDouble(cwidth3), Convert.ToDouble(vmodel1.Current3));
                                    cx3[line] = Convert.ToDouble(cwidth3);
                                    cy3[line] = Convert.ToDouble(vmodel1.Current3);

                                }
                                #endregion



                            }
                            else //数据走满后 重新增加数据
                            {
                                islength = false;
                                isCharAbort = false;
                                #region 计算保存新数据

                                string width = "10.00";

                                double vxd1 = Convert.ToDouble(width);
                                double vyd1 = Convert.ToDouble(vmodel1.Vibration1);

                                double vxd2 = Convert.ToDouble(width);
                                double vyd2 = Convert.ToDouble(vmodel1.Vibration2);

                                double vxd3 = Convert.ToDouble(width);
                                double vyd3 = Convert.ToDouble(vmodel1.Vibration3);

                                double cxd1 = Convert.ToDouble(width);
                                double cyd1 = Convert.ToDouble(vmodel1.Current1);

                                double cxd2 = Convert.ToDouble(width);
                                double cyd2 = Convert.ToDouble(vmodel1.Current2);

                                double cxd3 = Convert.ToDouble(width);
                                double cyd3 = Convert.ToDouble(vmodel1.Current3);


                                vx1[linlength - 1] = vxd1;
                                vy1[linlength - 1] = vyd1;

                                vx2[linlength - 1] = vxd2;
                                vy2[linlength - 1] = vyd2;

                                vx3[linlength - 1] = vxd3;
                                vy3[linlength - 1] = vyd3;

                                cx1[linlength - 1] = cxd1;
                                cy1[linlength - 1] = cyd1;

                                cx2[linlength - 1] = cxd2;
                                cy2[linlength - 1] = cyd2;

                                cx3[linlength - 1] = cxd3;
                                cy3[linlength - 1] = cyd3;


                                #endregion

                                #region 对数组操作

                                for (int j = 0; j < vx1.Length - 1; j++)//先将已有数据平移，再将新增数据加入
                                {
                                    //往前平移距离 当前序号 j 乘以 num 位移距离 乘以偷点数 LessPoint  除以 到整数位的数量 allnum
                                    //double range = Math.Round((double)(j * num * LessPoint) / allnum, allnum.ToString().Length);
                                    double newXvalue = (double)(j * num * LessPoint) / allnum;
                                    if (this.v1.Checked)
                                    {
                                        vx1[j] = newXvalue;
                                        vy1[j] = vy1[j + 1];
                                    }
                                    if (this.v2.Checked)
                                    {
                                        vx2[j] = newXvalue;
                                        vy2[j] = vy2[j + 1];
                                    }

                                    if (this.v3.Checked)
                                    {
                                        vx3[j] = newXvalue;
                                        vy3[j] = vy3[j + 1];
                                    }

                                    if (this.c1.Checked)
                                    {
                                        cx1[j] = newXvalue;
                                        cy1[j] = cy1[j + 1];
                                    }

                                    if (this.c2.Checked)
                                    {
                                        cx2[j] = newXvalue;
                                        cy2[j] = cy2[j + 1];
                                    }

                                    if (this.c3.Checked)
                                    {
                                        cx3[j] = newXvalue;
                                        cy3[j] = cy3[j + 1];
                                    }
                                }



                                vline1.Add(vx1, vy1);
                                vline2.Add(vx2, vy2);
                                vline3.Add(vx3, vy3);
                                cline1.Add(cx1, cy1);
                                cline2.Add(cx2, cy2);
                                cline3.Add(cx3, cy3);

                                #endregion



                                //异步方法
                                this.Invoke(new ThreadStart(delegate ()
                                {
                                    tChart.Refresh();
                                    string path = AppDomain.CurrentDomain.BaseDirectory;
                                    ListToText.Instance.WriteListToTextFile(DateTime.Now.ToString("O"), path);
                                }));

                            }

                            //list.Add(model);
                        }
                    }

                }
            }
        }
        private void Job_Queue2(object time)
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
                        string head = e.Msg.Substring(4, 4);
                        DataModel model = new DataModel();
                        model.id = Convert.ToInt32(head, 16);
                        model.head = head;
                        model.text = e.Msg;
                        model.old_data = new List<Vibration_Current>();
                        model.new_data = new List<Vibration_Current>();

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

                            model.old_data.Add(vmodel);
                            model.new_data.Add(vmodel1);


                            if (vline1.Count < linlength && islength)
                            {
                                string vwidth1 = Algorithm.Instance.Less_Porint_Algorithm(vline1.Count, num, LessPoint, allnum);//震动1长度
                                string vwidth2 = Algorithm.Instance.Less_Porint_Algorithm(vline2.Count, num, LessPoint, allnum);//震动2长度
                                string vwidth3 = Algorithm.Instance.Less_Porint_Algorithm(vline3.Count, num, LessPoint, allnum);//震动3长度
                                string cwidth1 = Algorithm.Instance.Less_Porint_Algorithm(cline1.Count, num, LessPoint, allnum);//电流1长度
                                string cwidth2 = Algorithm.Instance.Less_Porint_Algorithm(cline2.Count, num, LessPoint, allnum);//电流2长度
                                string cwidth3 = Algorithm.Instance.Less_Porint_Algorithm(cline3.Count, num, LessPoint, allnum);//电流3长度
                                #region line 动态刷新图表数据源
                                int line = vline1.Count;//定位
                                if (this.v1.Checked)
                                {
                                    vline1.Add(Convert.ToDouble(vwidth1), Convert.ToDouble(vmodel1.Vibration1));
                                    vx1[line] = Convert.ToDouble(vwidth1);
                                    vy1[line] = Convert.ToDouble(vmodel1.Vibration1);

                                }
                                if (this.v2.Checked)
                                {
                                    vline2.Add(Convert.ToDouble(vwidth2), Convert.ToDouble(vmodel1.Vibration2));
                                    vx2[line] = Convert.ToDouble(vwidth2);
                                    vy2[line] = Convert.ToDouble(vmodel1.Vibration2);
                                }
                                if (this.v3.Checked)
                                {
                                    vline3.Add(Convert.ToDouble(vwidth3), Convert.ToDouble(vmodel1.Vibration3));
                                    vx3[line] = Convert.ToDouble(vwidth3);
                                    vy3[line] = Convert.ToDouble(vmodel1.Vibration3);
                                }
                                if (this.c1.Checked)
                                {
                                    cline1.Add(Convert.ToDouble(cwidth1), Convert.ToDouble(vmodel1.Current1));
                                    cx1[line] = Convert.ToDouble(cwidth1);
                                    cy1[line] = Convert.ToDouble(vmodel1.Current1);
                                }
                                if (this.c2.Checked)
                                {
                                    cline2.Add(Convert.ToDouble(cwidth2), Convert.ToDouble(vmodel1.Current2));
                                    cx2[line] = Convert.ToDouble(cwidth2);
                                    cy2[line] = Convert.ToDouble(vmodel1.Current2);
                                }
                                if (this.c3.Checked)
                                {
                                    cline3.Add(Convert.ToDouble(cwidth3), Convert.ToDouble(vmodel1.Current3));
                                    cx3[line] = Convert.ToDouble(cwidth3);
                                    cy3[line] = Convert.ToDouble(vmodel1.Current3);

                                }
                                #endregion



                            }
                            else //数据走满后 重新增加数据
                            {

                                islength = false;
                                isCharAbort = false;
                                #region 计算保存新数据

                                //for(int j=0;j< pnum;j++)
                                //{
                                string width = "10.00";

                                vx1[linlength - 1] = Convert.ToDouble(width);
                                vy1[linlength - 1] = Convert.ToDouble(vmodel1.Vibration1);

                                vx2[linlength - 1] = Convert.ToDouble(width);
                                vy2[linlength - 1] = Convert.ToDouble(vmodel1.Vibration2);

                                vx3[linlength - 1] = Convert.ToDouble(width);
                                vy3[linlength - 1] = Convert.ToDouble(vmodel1.Vibration3);

                                cx1[linlength - 1] = Convert.ToDouble(width);
                                cy1[linlength - 1] = Convert.ToDouble(vmodel1.Current1);

                                cx2[linlength - 1] = Convert.ToDouble(width);
                                cy2[linlength - 1] = Convert.ToDouble(vmodel1.Current2);

                                cx3[linlength - 1] = Convert.ToDouble(width);
                                cy3[linlength - 1] = Convert.ToDouble(vmodel1.Current3);
                                //}
                                #endregion

                                #region 对数组操作

                                for (int j = 0; j < vx1.Length - 1; j++)//先将已有数据平移，再将新增数据加入
                                {
                                    //往前平移距离 当前序号 j 乘以 num 位移距离 乘以偷点数 LessPoint  除以 到整数位的数量 allnum
                                    double newXvalue = (double)(j * num * LessPoint) / allnum;
                                    if (this.v1.Checked)
                                    {
                                        vx1[j] = newXvalue;
                                        vy1[j] = vy1[j + 1];
                                    }
                                    if (this.v2.Checked)
                                    {
                                        vx2[j] = newXvalue;
                                        vy2[j] = vy2[j + 1];
                                    }

                                    if (this.v3.Checked)
                                    {
                                        vx3[j] = newXvalue;
                                        vy3[j] = vy3[j + 1];
                                    }

                                    if (this.c1.Checked)
                                    {
                                        cx1[j] = newXvalue;
                                        cy1[j] = cy1[j + 1];
                                    }

                                    if (this.c2.Checked)
                                    {
                                        cx2[j] = newXvalue;
                                        cy2[j] = cy2[j + 1];
                                    }

                                    if (this.c3.Checked)
                                    {
                                        cx3[j] = newXvalue;
                                        cy3[j] = cy3[j + 1];
                                    }
                                }
                                if (jsq == pnum)
                                {
                                    vline1.Add(vx1, vy1);
                                    //vline2.Add(vx2, vy2);
                                    //vline3.Add(vx3, vy3);
                                    //cline1.Add(cx1, cy1);
                                    //cline2.Add(cx2, cy2);
                                    //cline3.Add(cx3, cy3);
                                    jsq = 0;
                                }
                                #endregion
                                //异步方法
                                //this.Invoke(new ThreadStart(delegate ()
                                //{
                                //    tChart.Refresh();
                                //    string path = AppDomain.CurrentDomain.BaseDirectory;
                                //    ListToText.Instance.WriteListToTextFile(DateTime.Now.ToString("O"), path);
                                //}));
                                jsq++;
                            }

                            //list.Add(model);

                        }
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
            //int Sleep_Time = (int)time;
            //while (isAbort & isCharAbort)
            //{
            //    try
            //    {
            //        //Thread.Sleep(Sleep_Time);
            //        // 异步方法
            //        this.Invoke(new ThreadStart(delegate ()
            //    {
            //        tChart.Refresh();

            //    }));
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
        }
        #endregion

        #endregion
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            simpleButton3_Click(sender, e);
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton9_Click(object sender, EventArgs e)
        {
            simpleButton2_Click(sender, e);
        }
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            simpleButton4_Click(sender, e);
        }
    }
}
