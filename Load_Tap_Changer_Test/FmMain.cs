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

using System.Xml.Linq;
using System.Xml;
using Steema.TeeChart.Tools;
using DbHelper;
using System.Configuration;
using DbHelper.Db_Model;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using DbHelper.XmlModel;
using System.IO;
using System.Drawing.Drawing2D;
using Load_Tap_Changer_Test;

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
            //barManager1.ToolTipController.ca
            //this.MaximizeBox = false;//使最大化窗口失效
            // this.MinimizeBox = false;//使最小化窗口失效
            //更新波形通道
            if (Bind_IsDC())
            {
                Event_Bind();//绑定注册事件
                Chart_Init();//初始化图表
                CreateDb();//生成数据库
                Tester_List_Bind();//获取测试计划绑定到页面树

            }
            //测试连接通信
            // sendUdp(agreement._1_CMD_HEARTBEAT);
        }

        #region 页面初始化参数

        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == 0x112)
        //    {
        //        switch ((int)m.WParam)
        //        {
        //            //禁止双击标题栏关闭窗体
        //            case 0xF063:
        //            case 0xF093:
        //                m.WParam = IntPtr.Zero;
        //                break;

        //            //禁止拖拽标题栏还原窗体
        //            case 0xF012:
        //            case 0xF010:
        //                m.WParam = IntPtr.Zero;
        //                break;

        //            //禁止双击标题栏
        //            case 0xf122:
        //                m.WParam = IntPtr.Zero;
        //                break;

        //            ////禁止关闭按钮
        //            //case 0xF060:
        //            //    m.WParam = IntPtr.Zero;
        //            //    break;

        //            ////禁止最小化按钮
        //            //case 0xf020:
        //            //    m.WParam = IntPtr.Zero;
        //            //    break;

        //            ////禁止最大化按钮
        //            //case 0xf030:
        //            //    m.WParam = IntPtr.Zero;
        //            //    break;

        //            ////禁止还原按钮
        //            //case 0xf120:
        //            //    m.WParam = IntPtr.Zero;

        //            //    //this.WindowState = FormWindowState.Maximized;
        //            //    break;
        //        }
        //    }
        //    base.WndProc(ref m);
        //}

        /// <summary>
        /// xml 存储路径
        /// </summary>
      //  string xmlpath = "";

        /// <summary>
        /// 接收数据开关
        /// </summary>
       // bool IsSaveData = false;

        /// <summary>
        /// 协议对应参数
        /// </summary>
        Tester_Agreement agreement = new Tester_Agreement();
        /// <summary>
        /// s设备IP
        /// </summary>
        string DvIp = ConfigurationManager.ConnectionStrings["DvIp"].ConnectionString.ToString();

        /// <summary>
        /// 存储TxT数据用
        /// </summary>
        List<DataModel> list = new List<DataModel>();
        /// <summary>
        /// 获取焦点行
        /// </summary>
        TreeListNode publicnode;

        /// <summary>
        /// 焦点行实体
        /// </summary>
        Test_Plan pub_Test_Plan = new Test_Plan();
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
            if (model.PARENTID == "0" || this.treeList.DataSource == null)
            {

                using (FmAddTest form = new FmAddTest(model))
                {
                    form.ShowDialog();
                    if (form.DialogResult == DialogResult.OK)
                    {
                        id = form.id;
                        Tester_List_Bind();
                        Set_Foucs(id);
                    }
                }
            }
            else
            {
                using (FmAddTest form = new FmAddTest())
                {
                    form.ShowDialog();
                    if (form.DialogResult == DialogResult.OK)
                    {
                        id = form.id;
                        Tester_List_Bind();
                        Set_Foucs(id);
                    }
                }
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (list.Count > 0)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    ListToText.Instance.WriteListToTextFile(list, path);
                    //ListToText.Instance.WriteListToTextFile1(list, path);
                }
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());

                MessageBox.Show("ex:" + ex.ToString());
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
                Chart_Lond(filepath);
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());

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
            string path = FileHelper.Local_Path_Get() + "TestLineImage";
            FileHelper.CreateDirectoy(path);
            //文件夹名字
            string filename = DateTime.Now.ToString("d");
            path = path + "\\" + filename;
            FileHelper.CreateDirectoy(path);
            string imgname = "\\" + DateTime.Now.ToString("HHmmss") + ".png";
            tChart.Export.Image.PNG.Save(path + "\\" + imgname);

            #region 截屏 暂时不用
            //Rectangle ScreenArea = System.Windows.Forms.Screen.GetBounds(this);
            //int width1 = ScreenArea.Width; //屏幕宽度 
            //int height1 = ScreenArea.Height; //屏幕高度

            //System.Drawing.Rectangle rec = Screen.GetWorkingArea(this);

            //int SH = rec.Height;

            //int SW = rec.Width;

            //int SH1 = Screen.PrimaryScreen.Bounds.Height;

            //int SW1 = Screen.PrimaryScreen.Bounds.Width;


            //Bitmap bit = new Bitmap(width1, height1);//实例化一个和窗体一样大的bitmap
            //Graphics g = Graphics.FromImage(bit);
            //g.CompositingQuality = CompositingQuality.HighQuality;//质量设为最高
            //g.CopyFromScreen(this.Left, this.Top, 0, 0, new Size(width1, height1));//保存整个窗体为图片
            //                                                                                                       //g.CopyFromScreen(panel游戏区 .PointToScreen(Point.Empty), Point.Empty, panel游戏区.Size);//只保存某个控件（这里是panel游戏区）
            //bit.Save(path + "weiboTemp.png");//默认保存格式为PNG，保存成jpg格式质量不是很好

            #endregion
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
        /// 停止测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Stop_Test(false);
        }
        /// <summary>
        /// 停止测试功能
        /// </summary>
        /// <param name="isIN">是否 跳转档位</param>
        /// <param name="ShowType">显示状态 0，不操作 1,跳转到下一个档位 2，结束提醒</param>
        private void Stop_Test(bool isIN, int ShowType = 1)
        {
            try
            {
                if (sendUdp(agreement._3_CMD_STOPTESTER))// 返回成功才执行
                {
                    End_Chart();
                    if (ShowType == 2)
                    {
                        #region 删除未采集到数据的 测试
                        topnum = 0;//当前档位必须 设置为 0。
                        Db_Action.Instance.Test_Confige_Del(pub_Test_Plan);
                        string filepath = FileHelper.Local_Path_Get() + "Xml_Data\\" + pub_Test_Plan.DVNAME + ".xml";
                        FileHelper.DeleteFile(filepath);

                        #endregion

                        Tester_List_Bind();
                        XtraMessageBox.Show("电流未触发数据采集，请检查电流！");
                        return;
                    }
                    else if (ShowType == 1)
                    {
                        //Invoke(new ThreadStart(delegate ()
                        //                          {
                        Show_Open("数据保存中");

                        //结束采样 同时采样存储 图形处理异常慢 所以分开 存储数据
                        //考虑是否加个 转圈提示在存储数据不进行别的操作
                        Test_Xml_Insert();

                        if (allv > 0 && vcount > 0)
                        {
                            pub_Test_Plan.VOLTAGE = (allv / vcount).ToString();
                        }
                        else
                        {
                            pub_Test_Plan.VOLTAGE = "0";
                        }
                        Db_Action.Instance.Test_Confige_VOLTAGE_Edit(pub_Test_Plan);

                        XmlHelper.Edit_Voltage(pub_Test_Plan);

                        splashScreenManager.CloseWaitForm();
                        // Show_End();

                        //}));
                        if (isIN)
                        {
                            if (thread_List != null)
                            {
                                thread_List.Abort();
                            }
                            if (IsOpensTest && topnum <= alltopnum)
                            {
                                int time = 5;
                                splashScreenManager.ShowWaitForm();
                                splashScreenManager.SetWaitFormCaption("请切换档位");
                                splashScreenManager.SetWaitFormDescription(time + " 秒后 开始采集数据...");

                                Thread.Sleep(time * 1000);
                                splashScreenManager.CloseWaitForm();
                                // 通信超时后还原页面属性
                                Invoke(new ThreadStart(delegate ()
                                {
                                    Send_Config(2);
                                }));
                            }
                            else
                            {
                                isIN = false;
                                Invoke(new ThreadStart(delegate ()
                                {
                                    Tester_List_Bind();
                                    MessageBox.Show("数据采集完毕！");
                                    btnSTest.Enabled = true;

                                }));
                            }
                        }
                        else
                        {
                            btnSTest.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
            }
        }

        /// <summary>
        /// 按钮开始测试标识 用来阻断 点击计划树触发事件
        /// </summary>
        bool isBtnTest = true;

        /// <summary>
        /// 标识是连续测试还是单次测试
        /// </summary>
        bool IsOpensTest = false;
        /// <summary>
        /// 开始测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Send_Config();
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
            }
        }
        /// <summary>
        /// 开始测试基本配置
        /// </summary>
        private void Send_Config(int SINGLE_DOUBLE = 1)
        {
            if (publicnode == null)
            {
                MessageBox.Show("请在左侧选择测试计划");
                return;
            }
            if (Bind_IsDC())
            {
                End_Chart();
                #region 生成测试配置信息

                Test_Plan model = new Test_Plan();

                if (pub_Test_Plan.PARENTID == "0")
                {
                    model = Test_Plan_Bind(publicnode);
                }
                else
                {
                    TreeListNode node = Select_Top_Node(pub_Test_Plan.PARENTID);
                    model = Test_Plan_Bind(node);
                }

                pub_Test_Plan.ISEDIT = "1";
                string id = "";
                if (SINGLE_DOUBLE == 1)
                {
                    using (FmTestConfig form = new FmTestConfig(model))
                    {
                        form.ShowDialog();
                        if (form.DialogResult == DialogResult.OK)
                        {
                            id = form.id;
                            pub_Test_Plan.DVNAME = form.DvName;

                            I = form.I;
                            COUNT_BASE_C = form.COUNT_BASE_C;
                            if (!string.IsNullOrEmpty(form.topnum))
                            {
                                topnum = Convert.ToInt32(form.topnum);
                                alltopnum = Convert.ToInt32(form.alltopnum);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    //model.TIME_UNIT
                    pub_Test_Plan.PARENTID = model.ID;
                    pub_Test_Plan.DVNAME = model.DVNAME + "_" + topnum.ToString() + "--" + (topnum + 1).ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    pub_Test_Plan.ID = Db_Action.Instance.Test_Confige_Insert(pub_Test_Plan).ToString();
                    id = pub_Test_Plan.ID;
                }

                isBtnTest = false;
                tChart.Header.Text = pub_Test_Plan.DVNAME;
                Tester_List_Bind();
                Set_Foucs(id);

                #endregion
                //保存电压值


                XmlHelper.DeleteXmlDocument(pub_Test_Plan.DVNAME);
                XmlHelper.Init(pub_Test_Plan.DVNAME, pub_Test_Plan);
                btnSTest.Enabled = false;

                if (sendUdp(agreement._2_CMD_STARTTESTER))
                {
                    Start_Chart();

                };
            }
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
            if (Bind_IsDC())
            {
                if (pub_Test_Plan != null && pub_Test_Plan.PARENTID != "0")
                {
                    string filepath = FileHelper.Local_Path_Get() + "Xml_Data\\" + pub_Test_Plan.DVNAME + ".xml";
                    bool isdd = FileHelper.IsFileExist(filepath);
                    if (FileHelper.IsFileExist(filepath))
                    {
                        Chart_Lond(filepath);
                    }
                    else
                    {
                        MessageBox.Show("未找到该图形数据！");
                    }
                }
                else
                {
                    Chart_Init();
                }
            }
        }

        /// <summary>
        /// 放大缩小数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReNew_Click(object sender, EventArgs e)
        {
            //tChart.Zoom.Undo();
            tChart.Axes.Bottom.Labels.ValueFormat = "0.00";
            tChart.Axes.Bottom.SetMinMax(min, max);
            aotuzoom = 0;
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
                Porint_List = new ConcurrentQueue<byte[]>();
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
        private bool Bind_IsDC()
        {
            v1 = ckV1.Checked;
            v2 = ckV2.Checked;
            v3 = ckV3.Checked;

            c1 = ckC1.Checked;
            c2 = ckC2.Checked;
            c3 = ckC3.Checked;


            cks[0] = c1;
            cks[1] = c2;
            cks[2] = c3;

            cks[3] = v1;
            cks[4] = v2;
            cks[5] = v3;

            if (v1 || v2 || v3 || c1 || c2 || c3)
            {
                return true;
            }
            else
            {
                MessageBox.Show("请勾选左侧电流震动");
                return false;
            }

        }
        #endregion

        #region 树形右键

        /// <summary>
        /// 删除计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Test_Plan model = Test_Plan_Bind(publicnode);
            Db_Action.Instance.Test_Confige_Del(model);
            Tester_List_Bind();
        }

        #endregion

        #endregion

        #region 协议调用方法

        /// <summary>
        /// 发送协议
        /// </summary>
        private bool sendUdp(string msg)
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(DvIp), 4000);
            string ip = GetIpAddress();

            if (PingIP(DvIp))
            {
                if (ip == DvIp)
                {
                    MessageBox.Show("本机IP和设备IP相同，请修改本机IP。");
                    btnSTest.Enabled = true;

                    return false;
                }
                else
                {
                    SendMessage.SendMsgStart(msg, ipep);
                    return true;
                }
            }
            else
            {
                if (msg == agreement._2_CMD_STARTTESTER)//发送 采集数据是 检查设备未连接 删除该条数据 和xml文件
                {
                    Db_Action.Instance.Test_Confige_Del(pub_Test_Plan);
                    string filepath = FileHelper.Local_Path_Get() + "Xml_Data\\" + pub_Test_Plan.DVNAME + ".xml";
                    FileHelper.DeleteFile(filepath);
                }
                btnSTest.Enabled = true;
                MessageBox.Show("设备IP: 【" + DvIp + "】 未连接");
                return false;
            }
        }
        /// <summary>
        /// 检测IP
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns></returns>
        public static bool PingIP(string strIP)
        {

            System.Net.NetworkInformation.Ping psender = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply prep = psender.Send(strIP, 500, Encoding.Default.GetBytes("afdafdadfsdacareqretrqtqeqrq8899tu"));
            if (prep.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        private string GetIpAddress()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
                                                                    //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            IPAddress localaddr = localhost.AddressList[0];

            return localaddr.ToString();
        }

        /// <summary>
        /// 绑定UDP协议回调注册事件
        /// </summary>
        private void Event_Bind()
        {
            //波形数据
            SendMessage.udp_Event += new EventHandler<byte[]>(Run);
            //一般协议数据
            SendMessage.udp_Event_Kind += new EventHandler<Udp_EventArgs>(Run_Kind);
            Event_Chart_Bind();
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
                    ListToText.Instance.WriteListToTextFile1(e.Hearder);
                    if (e.Hearder == "00FF00FF")
                    {
                        Open_Type("1");
                        MessageBox.Show("设备连接成功");
                    }
                    else if (e.Hearder == "-1")
                    {
                        // 通信超时后停止操作 
                        End_Chart();
                        Open_Type("0");
                        MessageBox.Show("设备通信超时！");

                    }
                }));
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
            }
        }

        /// <summary>
        /// 修改页面通信状态
        /// </summary>
        /// <param name="type"></param>
        private void Open_Type(string type)
        {
            string str = FileHelper.Local_Path_Get();
            if (type == "0")
            {
                this.pcOpen.Load(str + "ImgIcon\\Favorite.bmp");
                this.lbOpen.Text = "设备未连接";
            }
            else
            {
                this.pcOpen.Load(str + "ImgIcon\\项目1525.bmp");
                this.lbOpen.Text = "设备已连接";
            }
        }

        /// <summary>
        /// 事件回调执行方法 存储接收到的 udp数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run(object sender, byte[] e)
        {
            Porint_List.Enqueue(e);

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
        /// 放大缩小 初始时间 防止鼠标触发 速度过快 放大次数用完
        /// </summary>
        DateTime startTime = DateTime.Now;

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //if (aotuzoom < 3)
            //{
            double OldXMin = tChart.Axes.Bottom.Minimum;
            double OldXMax = tChart.Axes.Bottom.Maximum;
            double XMid = (OldXMin + OldXMax) / 2;
            double NewXMin = (XMid * 0.5 + OldXMin) / (1.5);
            double NewXMax = (XMid * 0.5 + OldXMax) / (1.5);
            tChart.Axes.Bottom.Labels.ValueFormat = "0.000000";
            tChart.Axes.Bottom.Increment = 0.000001;//控制X轴 刻度的增量
            tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);

            tChart.Axes.Top.Labels.ValueFormat = "0.000000";
            tChart.Axes.Top.Increment = 0.000001;//控制X轴 刻度的增量
            tChart.Axes.Top.SetMinMax(NewXMin, NewXMax);
            aotuzoom++;
            //}
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


                tChart.Axes.Top.Labels.ValueFormat = "0.000000";
                tChart.Axes.Top.Increment = 0.000001;//控制X轴 刻度的增量
                tChart.Axes.Top.SetMinMax(NewXMin, NewXMax);
                aotuzoom--;
            }
            else if (aotuzoom == 1)
            {
                tChart.Axes.Bottom.Labels.ValueFormat = "0.00";
                tChart.Axes.Bottom.SetMinMax(min, max);

                tChart.Axes.Top.Labels.ValueFormat = "0.00";
                tChart.Axes.Top.SetMinMax(min, max);
                aotuzoom = 0;
            }
        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnlarge_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            double OldXMin = tChart.Axes.Bottom.Minimum;
            double OldXMax = tChart.Axes.Bottom.Maximum;
            double XMid = (OldXMin + OldXMax) / 2;
            double NewXMin = (XMid * 0.5 + OldXMin) / (1.5);
            double NewXMax = (XMid * 0.5 + OldXMax) / (1.5);

            tChart.Axes.Bottom.Labels.ValueFormat = "0.000000";
            tChart.Axes.Bottom.Increment = 0.000001;//控制X轴 刻度的增量
            tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);

            OldXMin = tChart.Axes.Top.Minimum;
            OldXMax = tChart.Axes.Top.Maximum;
            XMid = (OldXMin + OldXMax) / 2;
            NewXMin = (XMid * 0.5 + OldXMin) / (1.5);
            NewXMax = (XMid * 0.5 + OldXMax) / (1.5);

            tChart.Axes.Top.Labels.ValueFormat = "0.000000";
            tChart.Axes.Top.Increment = 0.000001;//控制X轴 刻度的增量
            tChart.Axes.Top.SetMinMax(NewXMin, NewXMax);
            Add_CursorTool();
            //if (aotuzoom < 7)
            //{
            aotuzoom++;
            //}
        }

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNarrow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (aotuzoom > 0)
            {
                double OldXMin = tChart.Axes.Top.Minimum;
                double OldXMax = tChart.Axes.Top.Maximum;
                double XMid = (OldXMin + OldXMax) / 2;
                double NewXMin = (-XMid * 0.5 + OldXMin) / (0.5);
                double NewXMax = (-XMid * 0.5 + OldXMax) / (0.5);
                tChart.Axes.Top.Labels.ValueFormat = "0.000000";
                tChart.Axes.Top.Increment = 0.000001;//控制X轴 刻度的增量
                double min = 0;
                if (NewXMin < 0)
                {
                    min = Math.Abs(NewXMin);
                    NewXMin = 0;
                }

                tChart.Axes.Top.SetMinMax(NewXMin, NewXMax);


                OldXMin = tChart.Axes.Bottom.Minimum;
                OldXMax = tChart.Axes.Bottom.Maximum;
                XMid = (OldXMin + OldXMax) / 2;
                NewXMin = (-XMid * 0.5 + OldXMin) / (0.5);
                NewXMax = (-XMid * 0.5 + OldXMax) / (0.5);
                tChart.Axes.Bottom.Labels.ValueFormat = "0.000000";
                tChart.Axes.Bottom.Increment = 0.000001;//控制X轴 刻度的增量
                NewXMin = NewXMin + min;
                if (NewXMin < Offset)
                {
                    NewXMin = Offset;
                }
                tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);
                aotuzoom--;

                if (tChart.Axes.Bottom.Maximum > max)
                {
                    tChart.Axes.Bottom.SetMinMax(min, max);
                    aotuzoom = 0;
                    return;
                }
            }
        }
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (tChart != null && linlength > 0)
            {
                Porint_List = new ConcurrentQueue<byte[]>();
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
        /// <summary>
        /// 还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //tChart.Zoom.Undo();
            tChart.Axes.Bottom.Labels.ValueFormat = "0.00";
            tChart.Axes.Bottom.SetMinMax(min, max);
            tChart.Axes.Top.Labels.ValueFormat = "0.00";
            tChart.Axes.Top.SetMinMax(min, max);
            aotuzoom = 0;
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

                tChart.Axes.Top.Labels.ValueFormat = "0.000000";
                tChart.Axes.Top.Increment = 0.000001;//控制X轴 刻度的增量

                if (e.Delta > 0)
                {
                    //  if (aotuzoom < 3 && DateTimeUtil.DateTimeDiff(startTime, DateTime.Now) > 1000)
                    if (DateTimeUtil.DateTimeDiff(startTime, DateTime.Now) > 1000)
                    {


                        double OldXMin = tChart.Axes.Bottom.CalcPosPoint(e.X) - 0.5;//(aotuzoom == 0 ? 0.5 : (0.5 / aotuzoom));
                        double OldXMax = tChart.Axes.Bottom.CalcPosPoint(e.X) + 0.5;//(aotuzoom == 0 ? 0.5 : (0.5 / aotuzoom));

                        double NewXMin = (XMid * 0.5 + OldXMin) / (1.5);
                        double NewXMax = (XMid * 0.5 + OldXMax) / (1.5);
                        startTime = DateTime.Now;
                        //if (NewXMax - NewXMin < 0.25)
                        //{
                        //    NewXMin = NewXMin + 0.1;
                        //    NewXMax = NewXMax - 0.1;
                        //}
                        //else if (NewXMax - NewXMin < 0.5)
                        //{
                        //    NewXMin = NewXMin + 0.25;
                        //    NewXMax = NewXMax - 0.25;
                        //}
                        tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);
                        tChart.Axes.Top.SetMinMax(NewXMin, NewXMax);
                        aotuzoom++;
                    }
                }
                else
                {
                    if (aotuzoom > 1)
                    {
                        double OldXMin = tChart.Axes.Bottom.CalcPosPoint(e.X) - 0.5;//(aotuzoom == 0 ? 0.5 : (0.5 / aotuzoom));
                        double OldXMax = tChart.Axes.Bottom.CalcPosPoint(e.X) + 0.5;//(aotuzoom == 0 ? 0.5 : (0.5 / aotuzoom));
                        double NewXMin = (-XMid * 0.5 + OldXMin) / (0.5);
                        double NewXMax = (-XMid * 0.5 + OldXMax) / (0.5);

                        tChart.Axes.Bottom.SetMinMax(NewXMin, NewXMax);
                        tChart.Axes.Top.SetMinMax(NewXMin, NewXMax);

                        aotuzoom--;
                    }
                    else if (aotuzoom == 1)
                    {
                        tChart.Axes.Bottom.Labels.ValueFormat = "0.00";
                        tChart.Axes.Bottom.SetMinMax(min, max);
                        tChart.Axes.Top.SetMinMax(min, max);

                        aotuzoom = 0;
                    }
                }
            }
        }
        int MouseX = 0;

        /// <summary>
        /// 鼠标移动点位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            //lbX.Text = e.X.ToString();
            //lbY.Text = e.Y.ToString();
            //if (vline1.Count > 0)
            //{
            //var dd = cursorTool1.XValue;
            ////       var dd = vline1.XValues[10000];
            //int num = vline1.XValues.Count;
            //int num1 = vline2.XValues.Count;
            //int num2 = cline1.XValues.Count;
            //int num3 = cline2.XValues.Count;

            MouseX = e.X;
            double BottomNum = tChart.Axes.Bottom.CalcPosPoint(e.X);
            double TopNum = tChart.Axes.Top.CalcPosPoint(e.X);

            //lbX.Text = BottomNum.ToString();
            //lbY.Text = TopNum.ToString();
            //}
        }

        /// 点击鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_MouseDown(object sender, MouseEventArgs e)
        {
            sx = e.X;
            sy = e.Y;
            startTime1 = DateTime.Now;
        }

        /// <summary>
        /// 松开鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_MouseUp(object sender, MouseEventArgs e)
        {
            // lbtest.Text = DateTimeUtil.DateTimeDiff(OpenCursortTime, startTime1) + " " + OpenCursortTime + "" + startTime1;
            if (tChart != null && tChart.Series.Count > 0 && DateTimeUtil.DateTimeDiff(OpenCursortTime, startTime1) > 200)
            {
                if (e.Button == MouseButtons.Left && e.Clicks == 1)
                {
                    // 开始位置大于结束位置    开始位置大于0  是鼠标左键
                    if ((sx < e.X || sy < e.Y) && sx > 0 && sy > 0)
                    {
                        //if (aotuzoom < 3 && DateTimeUtil.DateTimeDiff(startTime, DateTime.Now) > 1000)
                        if (DateTimeUtil.DateTimeDiff(startTime, DateTime.Now) > 1000)
                        {
                            double min = tChart.Axes.Bottom.CalcPosPoint(sx);
                            double max = tChart.Axes.Bottom.CalcPosPoint(e.X);
                            tChart.Axes.Bottom.Labels.ValueFormat = "0.000000";
                            tChart.Axes.Bottom.Increment = 0.000001;//控制X轴 刻度的增量
                            tChart.Axes.Bottom.SetMinMax(min, max);

                            double mint = tChart.Axes.Top.CalcPosPoint(sx);
                            double maxt = tChart.Axes.Top.CalcPosPoint(e.X);
                            tChart.Axes.Top.Labels.ValueFormat = "0.000000";
                            tChart.Axes.Top.Increment = 0.000001;//控制X轴 刻度的增量
                            tChart.Axes.Top.SetMinMax(mint, maxt);

                            aotuzoom = 7;
                            startTime = DateTime.Now;
                            Add_CursorTool();
                        }
                    }
                    else
                    {
                        tChart.Axes.Bottom.Labels.ValueFormat = "0.00";
                        tChart.Axes.Bottom.SetMinMax(min, max);


                        tChart.Axes.Top.Labels.ValueFormat = "0.00";
                        tChart.Axes.Top.SetMinMax(min, max);


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

        /// <summary>
        /// 左移右移标尺
        /// </summary>
        int LeftOrRightNum = 0;
        /// <summary>
        /// 移动标尺
        /// </summary>
        int MoveNum = 10000;
        /// <summary>
        /// 偏移量 用来计算缩小是 样本数据的 开始位置
        /// </summary>
        double Offset = 0;
        /// <summary>
        /// 左移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeftShift_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LeftOrRightNum++;
            Offset = tChart.Axes.Bottom.Minimum + (0.01 * Math.Abs(LeftOrRightNum));
            //if(Offset>0)
            //{
            //    tChart.Axes.Bottom.SetMinMax(Offset, tChart.Axes.Top.Maximum - Offset);
            //}
            //else
            //{
            tChart.Axes.Bottom.SetMinMax(Offset, tChart.Axes.Top.Maximum + Offset);
            //}

            //int num = MoveNum * LeftOrRightNum;
            //int allnum = cx2.Length - num;

            //if (allnum > 0)
            //{
            //    //double[] newvx = new double[allnum];
            //    //double[] newvy = new double[allnum];

            //    //double[] newcx = new double[allnum];
            //    //double[] newcy = new double[allnum];

            //    //for (int i = 0; i <= allnum - 1; i++)
            //    //{
            //    //    double x = i * 0.00001;
            //    //    newvx[i] = x;
            //    //    newvy[i] = vy2[num + i];

            //    //    newcx[i] = x;
            //    //    newcy[i] = cy2[num + i];
            //    //}

            //    //vline2.Add(newvx, newvy);
            //    //cline2.Add(newcx, newcy);

            //    tChart.Axes.Bottom.SetMinMax(tChart.Axes.Bottom.Minimum + (0.01 * LeftOrRightNum), tChart.Axes.Bottom.Maximum);
            //}
            //else
            //{
            //    LeftOrRightNum--;
            //    double[] xy = new double[1] { 0 };
            //    vline2.Add(xy, xy);
            //    cline2.Add(xy, xy);
            //}
        }

        /// <summary>
        /// 右移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnRightShift_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LeftOrRightNum--;
            //tChart.Axes.Bottom.SetMinMax(tChart.Axes.Bottom.Minimum - (0.01 * Math.Abs(LeftOrRightNum)), tChart.Axes.Bottom.Maximum);
            Offset = tChart.Axes.Bottom.Minimum - (0.01 * Math.Abs(LeftOrRightNum));
            //if (Offset > 0)
            //{
            //    tChart.Axes.Bottom.SetMinMax(Offset, tChart.Axes.Top.Maximum + Offset);
            //}
            //else
            //{
            tChart.Axes.Bottom.SetMinMax(Offset, tChart.Axes.Top.Maximum + Offset);
            //  }

            //int num = MoveNum * LeftOrRightNum;
            //int allnum = cx2.Length - num;

            //if (cx2.Length > vline2.Count)
            //{
            //    //double[] newvx = new double[allnum];
            //    //double[] newvy = new double[allnum];

            //    //double[] newcx = new double[allnum];
            //    //double[] newcy = new double[allnum];

            //    //for (int i = 0; i <= allnum - 1; i++)
            //    //{
            //    //    double x = i * 0.00001;
            //    //    newvx[i] = x;
            //    //    newvy[i] = vy2[num + i];

            //    //    newcx[i] = x;
            //    //    newcy[i] = cy2[num + i];
            //    //}

            //    //vline2.Add(newvx, newvy);
            //    //cline2.Add(newcx, newcy);
            //    tChart.Axes.Bottom.SetMinMax(tChart.Axes.Bottom.Minimum - (0.01 * LeftOrRightNum), tChart.Axes.Bottom.Maximum);

            //}
            //else if (cx2.Length == vline2.Count)
            //{
            //    //allnum = cx2.Length;
            //    //double[] newx = new double[allnum];
            //    //for (int i = 0; i <= allnum - 1; i++)
            //    //{
            //    //    double x = (Math.Abs(LeftOrRightNum) * MoveNum * 0.00001) + (i * 0.00001);
            //    //    newx[i] = x;
            //    //}
            //    //vline2.Add(newx, vy2);
            //    //cline2.Add(newx, cy2);
            //    tChart.Axes.Bottom.SetMinMax(tChart.Axes.Bottom.Minimum - (0.01 * Math.Abs(LeftOrRightNum)), tChart.Axes.Bottom.Maximum);

            //}
            //else
            //{

            //}
        }

        #endregion

        #region 图表初始化参数配置

        /// <summary>
        /// 全局 图表 队列 数据 处理 开关 ture 开 false 关
        /// </summary>
        bool isAbort = true;

        ///// <summary>
        ///// 刷新图表
        ///// </summary>
        //Thread ReChart;

        ///// <summary>
        ///// 存储数据
        ///// </summary>
        //Thread Db_Save;

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
        private int space = 6;

        /// <summary>
        /// //将显示宽度转换为整数
        /// </summary>
        int allnum = 100000;

        /// <summary>
        /// 单点距离 乘以 allnum 转换系数（100000） 最后转换为点位是 除去 allnum 
        /// //数据显示宽度  1秒=0.000002秒* 800微秒  0.0008毫秒 0.000001 * 800
        ///1个数据包 800微秒 0.0008  一个包80个数据点  每个点 10微秒 0.00001秒 
        /// </summary>
        int num = 1;

        /// <summary>
        /// 偷点数量 80能除尽数量 进行偷点
        /// </summary>
        int LessPoint = 80;

        /// <summary>
        /// 总的秒数 默认10秒
        /// </summary>
        int alltime = 10;

        /// <summary>
        ///总点数=10秒/（单点长度*偷点数）
        /// </summary>
        static int linlength = 0;

        /// <summary>
        /// 初始化参数 图表部分参数
        /// </summary>
        /// <param name="newalltime">Y轴长度以秒为换算单位</param>
        /// <param name="newLessPoint">漏点数</param>
        private void init_Chart_Config(int newalltime)
        {
            alltime = newalltime;
            // LessPoint = newLessPoint;
        }

        /// <summary>
        ///  //计算1秒 点位数量
        /// </summary>
        //int pnum = 0;
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
            plLinePath.Enabled = true;
            Offset = 0;
            aotuzoom = 0;
            tChart.Series.Clear();
            tChart.Axes.Custom.Clear();

            Chart_Config();//初始化图表
            Chart_Data_Bind();//初始化绑定 线line
            pclChart.Controls.Add(tChart);// 绑定图表位置 
        }
        /// <summary>
        /// 图表配置初始化
        /// </summary>
        private void Chart_Config(string time = "10")
        {
            //总点数=alltime执行秒数  （默认10秒）/（单点长度*偷点数）
            linlength = (alltime * allnum) / num / LessPoint;
            //计算1秒 点位数量
            // pnum = (int)(0.1 * allnum) / num / LessPoint;
            //计算单点的距离
            newXvalue = (double)(1 * num * LessPoint) / allnum;

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
            tChart.Axes.Bottom.Title.Text = "时间单位:" + time + "秒(sec)";


            //tChart.Axes.Bottom.Labels.Tag = "sec";
            //tChart.Axes.Bottom.Labels.


            //默认10秒
            tChart.Axes.Bottom.SetMinMax(0, alltime);

            tChart.Axes.Bottom.Increment = 1;//控制X轴 刻度的增量

            //tChart.Axes.Bottom.Inverted = true;//控制X轴 顺序还是倒序

            tChart.Panel.MarginLeft = 10;
            tChart.Panel.MarginRight = 10;
            tChart.Panel.MarginBottom = 5;
            tChart.Panel.MarginTop = 10;

            tChart.Zoom.Allow = false;


            min = tChart.Axes.Bottom.Minimum;
            max = tChart.Axes.Bottom.Maximum;

        }
        /// <summary>
        /// 十字光标
        /// </summary>
        CursorTool cursorTool;
        /// <summary>
        /// 松开十字光标的时间
        /// </summary>
        DateTime OpenCursortTime;
        /// <summary>
        /// 鼠标左键松开的时间
        /// </summary>
        DateTime startTime1;

        /// <summary>
        /// 增加十字光标
        /// </summary>
        private void Add_CursorTool()
        {
            tChart.Tools.Clear();
            cursorTool = new CursorTool();
            cursorTool.Active = true;
            cursorTool.FollowMouse = false;
            cursorTool.Series = tChart.Series[0];
            cursorTool.Style = CursorToolStyles.Both;

            tChart.Tools.Add(cursorTool);
            cursorTool.Change += (CursorChangeEventHandler)delegate
            {
                try
                {
                    int length = Convert.ToInt32(allnum * cursorTool.XValue) / firstAverageNum;

                    if (length < vline1.YValues.Count)// && ckV1.Checked
                    {
                        lbv1.Text = Rounding(vline1.YValues[length]);
                    }
                    if (Offset == 0)
                    {
                        if (length < vline2.YValues.Count)// && ckV1.Checked
                        {
                            lbv2.Text = Rounding(vline2.YValues[length]);
                        }
                    }
                    else
                    {
                        int vl2Num = vline2.YValues.Count + (-(int)(allnum * Offset));
                        if (length < vl2Num)//&& ckV2.Checked
                        {
                            if (length + (int)(allnum * Offset) > 0)
                            {
                                lbv2.Text = Rounding(vline2.YValues[length + (int)(allnum * Offset)]);
                            }
                        }
                    }

                    if (length < vline3.YValues.Count)//&& ckV3.Checked
                    {
                        lbv3.Text = vline3.YValues[length].ToString();
                    }

                    if (length < cline1.YValues.Count)//&& ckC1.Checked
                    {
                        lbc1.Text = Rounding(cline1.YValues[length]);
                    }
                    if (Offset == 0)
                    {
                        if (length < cline2.YValues.Count)// && ckV1.Checked
                        {
                            lbc2.Text = Rounding(cline2.YValues[length]);
                        }
                    }
                    else
                    {
                        int cl2Num = cline2.YValues.Count + (-(int)(allnum * Offset));
                        if (length < cl2Num)//&& ckV2.Checked
                        {
                            if (length + (int)(allnum * Offset) > 0)
                            {
                                lbc2.Text = Rounding(cline2.YValues[length + (int)(allnum * Offset)]);
                            }
                        }
                    }
                    if (length < cline3.YValues.Count)//&& ckC3.Checked
                    {
                        lbc3.Text = Rounding(cline3.YValues[length]);
                    }
                    tChart.Axes.Bottom.CalcPosPoint(MouseX);
                    lbTopTime.Text = Rounding(tChart.Axes.Top.CalcPosPoint(MouseX));
                    lbBottomTime.Text = Rounding(tChart.Axes.Bottom.CalcPosPoint(MouseX));

                    OpenCursortTime = DateTime.Now;
                }
                catch
                {
                }
            };
        }

        private string Rounding(double num)
        {
            string remsg = Math.Round(num, 3, MidpointRounding.AwayFromZero).ToString();
            return remsg;
        }

        /// <summary>
        /// 初始化对比页面
        /// </summary>
        private void Chart_Config_Contrast(string time1, string time2)
        {
            vline1 = new Line();//震动1
            vline2 = new Line();//震动2
            cline1 = new Line();//电流1
            cline2 = new Line();//电流2

            tChart.Dock = DockStyle.Fill;
            tChart.Aspect.View3D = false;
            tChart.Legend.Visible = false;//显示/隐藏线的注释 
            tChart.Axes.Visible = true;

            tChart.Legend.LegendStyle = LegendStyles.Series;
            tChart.Axes.Bottom.Labels.ValueFormat = "0.00";
            tChart.Axes.Bottom.Title.Text = "对比数据时间单位:" + time2 + "秒(sec)";

            //默认10秒
            tChart.Axes.Bottom.SetMinMax(0, alltime);
            tChart.Axes.Bottom.Increment = 1;//控制X轴 刻度的增量
            tChart.Axes.Bottom.Grid.Visible = false;//网格 显示 true  不显示 false


            tChart.Axes.Top.Visible = true;

            tChart.Axes.Top.Labels.ValueFormat = "0.00";
            tChart.Axes.Top.Title.Text = "样本数据时间单位:" + time1 + "秒(sec)";
            tChart.Axes.Top.SetMinMax(0, alltime);
            tChart.Axes.Top.Increment = 1;//控制X轴 刻度的增量
                                          // tChart.Axes.Top.Automatic = false;
                                          //  tChart.Axes.Top.Grid.Visible = false;

            tChart.Panel.MarginLeft = 10;
            tChart.Panel.MarginRight = 10;
            tChart.Panel.MarginBottom = 5;
            tChart.Panel.MarginTop = 10;

            tChart.Zoom.Allow = false;


            min = tChart.Axes.Bottom.Minimum;
            max = tChart.Axes.Bottom.Maximum;
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
            double single = (100 / count) - space;
            //if (count != 1)
            //{
            //    single = (100 - space * (count - 2)) / (count + 1);//单个坐标轴的百分比
            //}

            tChart.Axes.Left.StartPosition = space;
            tChart.Axes.Left.EndPosition = tChart.Axes.Left.EndPosition = tChart.Axes.Left.StartPosition + single;
            tChart.Axes.Left.StartEndPositionUnits = PositionUnits.Percent;

            listBaseLine[0].CustomVertAxis = tChart.Axes.Left;
            double startPosition = tChart.Axes.Left.StartPosition;
            double endPosition = tChart.Axes.Left.EndPosition;
            startPosition = 0;
            endPosition = 0;
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
                //axis.Maximum = 10;//最大值
                //axis.Minimum = -10;//最小值

                string title = tChart.Series[i].Title;

                axis.Title.Text = title;
                if (title.Substring(0, 2) == "电流")
                {
                    axis.Maximum = 5;//最大值
                    axis.Minimum = -5;//最小值
                }
                else
                {
                    axis.Maximum = 10;//最大值
                    axis.Minimum = -10;//最小值
                }


                tChart.Axes.Custom.Add(axis);
                listBaseLine[i].CustomVertAxis = axis;
            }
        }

        /// <summary>
        /// 添加对比数据线
        /// </summary>
        /// <param name="count"></param>
        private void AddCustomAxis_Contrast(int count)
        {

            double single = (100 / count) - space;
            List<BaseLine> listBaseLine = new List<BaseLine>();
            for (int i = 0; i < tChart.Series.Count; i++)
            {
                listBaseLine.Add((BaseLine)tChart.Series[i]);
            }
            double startPosition = tChart.Axes.Left.StartPosition;
            double endPosition = tChart.Axes.Left.EndPosition;
            startPosition = 0;
            endPosition = 0;
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

                string title = tChart.Series[i * 2].Title;

                axis.Title.Text = title;
                axis.Title.Color = Color.Red;
                axis.Maximum = 5;//最大值
                axis.Minimum = -5;//最小值

                tChart.Axes.Custom.Add(axis);
                listBaseLine[i * 2].CustomVertAxis = axis;
                listBaseLine[(i * 2) + 1].CustomVertAxis = axis;
            }
        }

        /// <summary>
        /// 绑定图表 line 线
        /// </summary>
        private void Chart_Data_Bind()
        {
            try
            {
                tChart.Axes.Custom.Clear();
                if (v1)
                {
                    //震动1路 vline1
                    vline1.Title = string.Format("震动曲线{0}", 1);
                    //    vline1.Color = Color.Green;
                    tChart.Series.Add(vline1);

                    //vline1_1 = new Line();
                    //vline1_1.Color = Color.Red;
                    // tChart.Series.Add(vline1_1);
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
                //AddCustomAxis1(tChart.Series.Count);//绘制画布
                AddCustomAxis(tChart.Series.Count);//绘制画布

            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
            }
        }

        #region 重新加载波形图代码

        /// <summary>
        /// 初始化加载波形参数
        /// </summary>
        private void Chart_DataTable_Init()
        {
            //更新波形通道
            if (Bind_IsDC())
            {
                tChart.Series.Clear();
                tChart.Axes.Custom.Clear();
                tChart.Zoom.Allow = false;
                Event_Chart_Bind();
            }
        }

        /// <summary>
        /// 平均次数
        /// </summary>
        int firstAverageNum = 100;
        /// <summary>
        /// 加载波形图配置
        /// </summary>
        /// <param name="filepath"></param>
        private void Chart_Lond(string filepath)
        {
            if (!string.IsNullOrEmpty(filepath))
            {
                Show_Open();
                bool IsNotNull;



                //XmlHelper.Xml_To_Array(filepath, cks,
                //   out vx1, out vy1,
                //   out vx2, out vy2,
                //   out vx3, out vy3,
                //   out cx1, out cy1,
                //   out cx2, out cy2,
                //   out cx3, out cy3,
                //   out IsNotNull
                //    );


                //取平均数据组
                XmlHelper.Xml_To_Average_Array(filepath, cks,
                   out vx1, out vy1,
                   out vx2, out vy2,
                   out vx3, out vy3,
                   out cx1, out cy1,
                   out cx2, out cy2,
                   out cx3, out cy3,
                   out IsNotNull, firstAverageNum
                    );
                //XmlHelper.Xml_To_Average_Array(filepath, cks,
                //   out linex, out liney,

                //   out IsNotNull, firstAverageNum
                //    );

                //取 参数有问题
                Test_Plan modelNew = XmlHelper.Xml_To_Model(filepath);

                if (modelNew != null)
                {
                }
                if (!IsNotNull)
                {
                    MessageBox.Show("未找到该图形数据！");
                    Show_End();
                    return;
                }
                string time = vx1[vx1.Length - 1].ToString();
                tChart.Axes.Bottom.Title.Text = "时间单位:" + time + "秒(sec)";
                alltime = 20;
                Chart_DataTable_Init();
                Chart_Data_Lond();
                Show_End();
            }
        }

        /// <summary>
        /// 重新加载图表
        /// </summary>
        private void Chart_Data_Lond()
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
                    cline3.Add(cx3, cy3);
                }

                //绘制画布
                AddCustomAxis(tChart.Series.Count);

                tChart.Refresh();
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
            }
        }

        /// <summary>
        /// 对比数据加载加载图表
        /// </summary>
        private void Chart_Data_Contrast_Lond()
        {
            try
            {
                vline1 = new Line();
                vline1.HorizAxis = HorizontalAxis.Top;
                vline1.Title = string.Format("【震动】");

                vline1.Add(vx1, vy1);
                vline1.Color = Color.Green;

                tChart.Series.Add(vline1);

                vline2 = new Line();
                vline2.HorizAxis = HorizontalAxis.Bottom;
                //  vline2.Title = string.Format("【震动】22");
                vline2.Add(vx2, vy2);
                vline2.Color = Color.Red;
                tChart.Series.Add(vline2);

                cline1 = new Line();
                cline1.HorizAxis = HorizontalAxis.Top;
                cline1.Title = string.Format("【电流】");
                cline1.Add(cx1, cy1);
                cline1.Color = Color.Orange;

                tChart.Series.Add(cline1);


                cline2 = new Line();
                cline2.HorizAxis = HorizontalAxis.Bottom;
                //cline2.Title = string.Format("【电流】对比数据");
                cline2.Add(cx2, cy2);
                cline2.Color = Color.Blue;
                tChart.Series.Add(cline2);

                //绘制画布
                AddCustomAxis_Contrast(2);
                //AddCustomAxis_Contrast(tChart.Series.Count);
                tChart.Refresh();
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
            }
        }

        /// <summary>
        /// 对比数据加载加载图表
        /// </summary>
        private void Chart_Data_Envelope_Lond(double[] Out_x, double[] Out_y, double[] Out_x1 = null
            , double[] Out_y1 = null, bool Single = false)
        {
            try
            {
                vline1 = new Line();

                vline1.Title = string.Format("包络线");

                vline1.Add(Out_x, Out_y);
                vline1.Color = Color.Green;

                tChart.Series.Add(vline1);

                vline2 = new Line();
                tChart.Series.Add(vline2);
                if (Single)
                {
                    vline1.Title = string.Format("包络线 (绿色【样本数据】   红色【对比数据】)");

                    vline1.HorizAxis = HorizontalAxis.Top;
                    vline2.HorizAxis = HorizontalAxis.Bottom;

                    vline2.Add(Out_x1, Out_y1);
                    vline2.Color = Color.Red;
                }
                //绘制画布
                AddCustomAxis_Contrast(1);

                tChart.Refresh();
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
            }
        }

        #endregion

        #region 队列刷新动态图表

        /// <summary>
        /// 绘图队列
        /// </summary>
        ConcurrentQueue<byte[]> Porint_List = new ConcurrentQueue<byte[]>();
        /// <summary>
        /// 存储数据队列
        /// </summary>
        ConcurrentQueue<Udp_EventArgs> Save_Db_Source = new ConcurrentQueue<Udp_EventArgs>();

        /// <summary>
        /// 存储头尾展示线1秒
        /// </summary>
        ConcurrentQueue<Udp_EventArgs> Top_End_Data = new ConcurrentQueue<Udp_EventArgs>();

        /// <summary>
        /// 计量当前数据执行次数 （刷新清除横线时使用 重要） 停止发送数据后要归零
        /// </summary>
        int porintadd = 0;

        /// <summary>
        /// 计量 电流触发 10秒触发量 1 秒 1250   10秒  12500
        /// </summary>
        int CurrentNum = 1;
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

        double[][] linex;
        double[][] liney;

        #endregion

        /// <summary>
        /// 开始处理图表
        /// </summary>
        private void Start_Chart()
        {
            allv = 0;
            vcount = 0;
            lbVoltage.Text = "0";
            panelControl1.Enabled = false;
            pc2.Enabled = false;

            isAbort = true;

            Porint_List = new ConcurrentQueue<byte[]>();

            Save_Db_Source = new ConcurrentQueue<Udp_EventArgs>();

            Top_End_Data = new ConcurrentQueue<Udp_EventArgs>();

            #region 处理队列数据

            //thread_List = new Thread(Job_Queue);
            if (pub_Test_Plan.GETINFO == "2")
            {
                thread_List = new Thread(Job_Queue_01);
                // Xml_Save_ON_OFF();
            }
            else
            {
                thread_List = new Thread(Job_Queue_03);
                //thread_List = new Thread(Job_Queue_04);


            }
            thread_List.IsBackground = true;
            thread_List.Start();//启动线程

            #endregion

            #region 绘图线程

            //if (pub_Test_Plan.GETINFO == "1")
            //{
            //    ReChart = new Thread(Refresh_Server);
            //    ReChart.IsBackground = true;
            //    ReChart.Start();//启动线程
            //}

            #endregion
            #region 存储数据


            #endregion
            Chart_Init();
        }

        /// <summary>
        /// 停止处理图表
        /// </summary>
        private void End_Chart()
        {
            btnSTest.Enabled = true;//通信未超时是回复
            istrue = false;
            porintadd = 0;
            endprint = 0;
            isAbort = false;
            Current_Config();
            WinRefreshNum = 0;
            // 通信超时后还原页面属性
            Invoke(new ThreadStart(delegate ()
            {
                panelControl1.Enabled = true;

                pc2.Enabled = true;
                //IsSaveData = false;
            }));
            //Tester_List_Bind();
            isBtnTest = true;
        }

        #region 队列数据处理

        /// <summary>
        /// 判断是否刚开始 走图形 是为false 否为true  
        /// 电流触发 启动时使用
        /// </summary>
        bool istrue = false;

        /// <summary>
        /// 是否触发开关存储
        /// </summary>
        bool SCURRENT = false;

        /// <summary>
        /// 当前档位
        /// </summary>
        int topnum = 0;

        /// <summary>
        /// 总档位数
        /// </summary>
        int alltopnum = 0;

        /// <summary>
        /// 当前存储数据量刷新图形用
        /// </summary>
        int addNum = 0;
        /// <summary>
        /// 存储数量的最大值刷新图形用
        /// </summary>
        int alladdnum = 150;
        /// <summary>
        /// 总电压
        /// </summary>
        int allv = 0;
        /// <summary>
        /// 电压返回次数
        /// </summary>
        int vcount = 0;

        string[] addv = new string[1000];
        int[] addvv = new int[1000];

        double[] c1x;
        double[] c1y;

        double[] c2x;
        double[] c2y;

        double[] c3x;
        double[] c3y;

        double[] v1x;
        double[] v1y;

        double[] v2x;
        double[] v2y;

        double[] v3x;
        double[] v3y;

        /// <summary>
        /// 前往后走图形 按时间走波形
        /// </summary>
        private void Job_Queue_01()
        {
            addNum = 0;
            c1x = new double[alladdnum];
            c1y = new double[alladdnum];

            c2x = new double[alladdnum];
            c2y = new double[alladdnum];

            c3x = new double[alladdnum];
            c3y = new double[alladdnum];

            v1x = new double[alladdnum];
            v1y = new double[alladdnum];

            v2x = new double[alladdnum];
            v2y = new double[alladdnum];

            v3x = new double[alladdnum];
            v3y = new double[alladdnum];

            while (isAbort)
            {
                if (Porint_List.Count > 0)
                {
                    byte[] bytes;
                    Porint_List.TryDequeue(out bytes);//取出队里数据并删除
                                                      //截取返回数据
                    Udp_EventArgs e = new Udp_EventArgs();//初始化 实体
                    e.DataBaty = bytes;
                    e.Msg = ProtocolUtil.byteToHexStr(e.DataBaty);
                    if (bytes.Length > 300)
                    {
                        string data = e.Msg.Substring(8, e.Msg.Length - 8);
                        int Porints = 80 / LessPoint;
                        double x = new double();
                        double y = new double();

                        for (int i = 0; i < Porints; i++)
                        {
                            int length = 24 * (i + 1);//截取位置 +1 默认不取第一个点位
                            #region 计算保存新点位数据

                            x = porintadd * newXvalue;
                            if (v1)
                            {
                                string V = data.Substring(0 + length, 4);
                                double Vd = Algorithm.Instance.Vibration_Algorithm_Double(V);

                                v1x[addNum] = x;
                                v1y[addNum] = Vd;
                                if (addNum == alladdnum - 1 || porintadd >= linlength)
                                {
                                    vline1.Add(v1x, v1y, true);
                                    v1x = new double[alladdnum];
                                    v1y = new double[alladdnum];
                                }
                            }
                            if (v2)
                            {
                                string V = data.Substring(4 + length, 4);
                                double Vd = Algorithm.Instance.Vibration_Algorithm_Double(V);

                                v2x[addNum] = x;
                                v2y[addNum] = Vd;
                                if (addNum == alladdnum - 1 || porintadd >= linlength)
                                {
                                    vline2.Add(v2x, v2y, true);
                                    v2x = new double[alladdnum];
                                    v2y = new double[alladdnum];
                                }
                            }
                            if (v3)
                            {
                                string V = data.Substring(8 + length, 4);
                                double Vd = Algorithm.Instance.Vibration_Algorithm_Double(V);

                                v3x[addNum] = x;
                                v3y[addNum] = Vd;

                                if (addNum == alladdnum - 1 || porintadd >= linlength)
                                {
                                    vline3.Add(v3x, v3y, true);
                                    v3x = new double[alladdnum];
                                    v3y = new double[alladdnum];
                                }
                            }
                            if (c1)
                            {
                                string C = data.Substring(12 + length, 4);
                                double Cd = Algorithm.Instance.Current_Algorithm_Double(C);

                                c1x[addNum] = x;
                                c1y[addNum] = Cd;

                                if (addNum == alladdnum - 1 || porintadd >= linlength)
                                {
                                    cline1.Add(c1x, c1y, true);
                                    c1x = new double[alladdnum];
                                    c1y = new double[alladdnum];
                                }
                            }
                            if (c2)
                            {
                                string C = data.Substring(16 + length, 4);
                                double Cd = Algorithm.Instance.Current_Algorithm_Double(C);

                                c2x[addNum] = x;
                                c2y[addNum] = Cd;
                                if (addNum == alladdnum - 1 || porintadd >= linlength)
                                {
                                    cline2.Add(c2x, c2y, true);
                                    c2x = new double[alladdnum];
                                    c2y = new double[alladdnum];
                                }

                            }
                            if (c3)
                            {
                                string C = data.Substring(20 + length, 4);
                                double Cd = Algorithm.Instance.Current_Algorithm_Double(C);

                                c3x[addNum] = x;
                                c3y[addNum] = Cd;
                                if (addNum == alladdnum - 1 || porintadd >= linlength)
                                {
                                    cline3.Add(c3x, c3y, true);
                                    c3x = new double[alladdnum];
                                    c3y = new double[alladdnum];
                                }
                            }
                            #endregion

                            if (addNum == alladdnum - 1)
                            {
                                addNum = 0;
                            }
                            else
                            {
                                addNum++;
                            }
                            #region 存储数据

                            //判断数据是否超标了
                            if (porintadd >= linlength)
                            {
                                //是停止
                                this.BeginInvoke(new MethodInvoker(() =>
                                {
                                    tChart.Refresh();
                                    Stop_Test(false);

                                }));
                                return;
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    Save_Db_Source.Enqueue(e);
                                }
                            }
                            #endregion

                            porintadd++;
                        }
                    }
                    else
                    {
                        //Udp_EventArgs e = new Udp_EventArgs();//初始化 实体

                        e.Hearder = e.Msg.Substring(0, 4);
                        if (e.Hearder == "5050")//返回电压只 要存储 
                        {
                            string vv = e.Msg.Substring(4, 4);
                            int v = Convert.ToInt16(vv, 16);

                            addv[vcount] = vv;
                            addvv[vcount] = v;
                            if (v > 0)
                            {
                                allv = allv + v;

                                vcount++;
                                Invoke(new ThreadStart(delegate ()
                                {
                                    lbVoltage.Text = (allv / vcount).ToString();
                                }));
                            }
                        }
                    }
                }
            }
            Print_End_Bind(addNum);
        }

        /// <summary>
        /// 绑定所有数据加载完毕后 未绑定到线上的数据
        /// </summary>
        /// <param name="addNum"></param>
        private void Print_End_Bind(int addNum)
        {
            for (int i = 0; i <= addNum; i++)
            {
                if (v1)
                {
                    if (v1x[i] > 0)
                    {
                        double[] x = { v1x[i] };
                        double[] y = { v1y[i] };
                        vline1.Add(x, y, true);
                    }
                }
                if (v2)
                {
                    if (v2x[i] > 0)
                    {
                        double[] x = { v2x[i] };
                        double[] y = { v2y[i] };
                        vline2.Add(x, y, true);
                    }
                }
                if (v3)
                {
                    if (v3x[i] > 0)
                    {
                        double[] x = { v3x[i] };
                        double[] y = { v3y[i] };
                        vline3.Add(x, y, true);
                    }
                }
                if (c1)
                {
                    if (c1x[i] > 0)
                    {
                        double[] x = { c1x[i] };
                        double[] y = { c1y[i] };
                        cline1.Add(x, y, true);
                    }
                }
                if (c2)
                {
                    if (c2x[i] > 0)
                    {
                        double[] x = { c2x[i] };
                        double[] y = { c2y[i] };
                        cline2.Add(x, y, true);
                    }
                }
                if (c3)
                {
                    if (c3x[i] > 0)
                    {
                        double[] x = { c3x[i] };
                        double[] y = { c3y[i] };
                        cline3.Add(x, y, true);
                    }
                }
            }

        }

        /// <summary>
        /// 开始前后存1秒数据 前 1250 后1250 *2 =2500
        /// </summary>
        //int around = 1;
        /// <summary>
        /// 开始前后是否存数据开关 true 开 false 关
        /// </summary>
        //bool isaround = true;
        /// <summary>
        /// 到达2000是 进行一次运算
        /// </summary>
        int operation = 0;

        /// <summary>
        /// 累计的电流平方
        /// </summary>
        double countCurrent1 = 0.0;

        /// <summary>
        /// 电流触发前后秒数 刚开始存1秒  后面结束一次 存2秒
        /// </summary>
        int AroundSecond = 1;

        /// <summary>
        /// 基本电流
        /// </summary>
        int I = 10;
        /// <summary>
        /// 判定电流是否到达触发值 运算参照数据
        /// </summary>
        string COUNT_BASE_C = "1";

        /// <summary>
        /// 波形图 满屏后刷新次数 
        /// </summary>
        int WinRefreshNum = 0;

        /// <summary>
        /// 结束后再保存2秒
        /// </summary>
        int endprint = 0;

        /// <summary>
        /// 前往后走图形电流触发波形
        /// </summary>
        private void Job_Queue_03()
        {
            addNum = 0;
            c1x = new double[alladdnum];
            c1y = new double[alladdnum];

            c2x = new double[alladdnum];
            c2y = new double[alladdnum];

            c3x = new double[alladdnum];
            c3y = new double[alladdnum];

            v1x = new double[alladdnum];
            v1y = new double[alladdnum];

            v2x = new double[alladdnum];
            v2y = new double[alladdnum];

            v3x = new double[alladdnum];
            v3y = new double[alladdnum];
            bool isFist = false;

            while (isAbort)
            {
                if (Porint_List.Count > 0)
                {
                    byte[] bytes;
                    Porint_List.TryDequeue(out bytes);//取出队里数据并删除
                                                      //截取返回数据
                    Udp_EventArgs e = new Udp_EventArgs();//初始化 实体
                    e.DataBaty = bytes;
                    e.Msg = ProtocolUtil.byteToHexStr(e.DataBaty);

                    if (bytes.Length > 300)
                    {

                        if (Top_End_Data.Count < 1250)
                        {
                            Top_End_Data.Enqueue(e);
                        }

                        string data = e.Msg.Substring(8, e.Msg.Length - 8);
                        int Porints = 80 / LessPoint;
                        double x = new double();
                        double y = new double();
                        for (int i = 0; i < Porints; i++)
                        {
                            int length = 24 * (i + 1);//截取位置 +1 默认不取第一个点位
                            #region 计算保存新点位数据

                            x = porintadd * newXvalue;
                            if (v1)
                            {
                                string V = data.Substring(0 + length, 4);
                                double Vd = Algorithm.Instance.Vibration_Algorithm_Double(V);
                                if (isFist)
                                {
                                    vline1.YValues[porintadd] = Vd;
                                }
                                else
                                {
                                    v1x[addNum] = x;
                                    v1y[addNum] = Vd;
                                    if (addNum == alladdnum - 1)
                                    {
                                        vline1.Add(v1x, v1y, true);
                                        v1x = new double[alladdnum];
                                        v1y = new double[alladdnum];
                                    }
                                }
                            }
                            if (v2)
                            {
                                string V = data.Substring(4 + length, 4);
                                double Vd = Algorithm.Instance.Vibration_Algorithm_Double(V);

                                if (isFist)
                                {
                                    vline2.YValues[porintadd] = Vd;
                                }
                                else
                                {
                                    v2x[addNum] = x;
                                    v2y[addNum] = Vd;
                                    if (addNum == alladdnum - 1)
                                    {
                                        vline2.Add(v2x, v2y, true);
                                        v2x = new double[alladdnum];
                                        v2y = new double[alladdnum];
                                    }
                                }
                            }
                            if (v3)
                            {
                                string V = data.Substring(8 + length, 4);
                                double Vd = Algorithm.Instance.Vibration_Algorithm_Double(V);
                                if (isFist)
                                {
                                    vline3.YValues[porintadd] = Vd;
                                }
                                else
                                {
                                    v3x[addNum] = x;
                                    v3y[addNum] = Vd;

                                    if (addNum == alladdnum - 1)
                                    {
                                        vline3.Add(v3x, v3y, true);
                                        v3x = new double[alladdnum];
                                        v3y = new double[alladdnum];
                                    }
                                }
                            }
                            if (c1)
                            {
                                string C = data.Substring(12 + length, 4);
                                double Cd = Algorithm.Instance.Current_Algorithm_Double(C, I);

                                if (isFist)
                                {
                                    cline1.YValues[porintadd] = Cd;
                                }
                                else
                                {
                                    c1x[addNum] = x;
                                    c1y[addNum] = Cd;

                                    if (addNum == alladdnum - 1)
                                    {
                                        cline1.Add(c1x, c1y, true);

                                        c1x = new double[alladdnum];
                                        c1y = new double[alladdnum];
                                    }
                                }
                            }
                            if (c2)
                            {
                                string C = data.Substring(16 + length, 4);
                                double Cd = Algorithm.Instance.Current_Algorithm_Double(C, I);
                                if (isFist)
                                {
                                    cline2.YValues[porintadd] = Cd;
                                }
                                else
                                {
                                    c2x[addNum] = x;
                                    c2y[addNum] = Cd;
                                    if (addNum == alladdnum - 1)
                                    {
                                        cline2.Add(c2x, c2y, true);
                                        c2x = new double[alladdnum];
                                        c2y = new double[alladdnum];
                                    }
                                }
                            }
                            if (c3)
                            {
                                string C = data.Substring(20 + length, 4);
                                double Cd = Algorithm.Instance.Current_Algorithm_Double(C, I);
                                if (isFist)
                                {
                                    cline3.YValues[porintadd] = Cd;
                                }
                                else
                                {
                                    c3x[addNum] = x;
                                    c3y[addNum] = Cd;
                                    if (addNum == alladdnum - 1)
                                    {
                                        cline3.Add(c3x, c3y, true);
                                        c3x = new double[alladdnum];
                                        c3y = new double[alladdnum];
                                    }
                                }
                            }
                            #endregion

                            if (addNum == alladdnum - 1)
                            {
                                addNum = 0;
                            }
                            else
                            {
                                addNum++;
                            }
                            porintadd++;
                        }
                        // 处理数据线程结束后 触发
                        if (!isAbort && Porint_List.Count == 0)
                        {
                            Print_End_Bind(addNum);
                        }
                        #region 数据大于10秒时执行 横条删除效果
                        if (porintadd > linlength - 1 && addNum == 0)
                        {
                            if (WinRefreshNum > 5)
                            {
                                WinRefreshNum = 0;
                                BeginInvoke(new MethodInvoker(() =>
                                {
                                    Stop_Test(false, 2);
                                }));
                                return;
                            }

                            isFist = true;
                            porintadd = 0;//归零

                            WinRefreshNum++;
                            istrue = true;//开启进入标识
                            double swidth = 0;//初始位置
                            double width = swidth;//结束时位置

                            for (int i = 0; i < 500; i++)
                            {
                                width = (porintadd + i) * newXvalue;
                                //Print_Bind((porintadd + i));
                            }
                            colorFrom = swidth;
                            colorTo = width;
                            Print_Color_Bind();

                        }
                        else if (istrue && addNum == 0)
                        {
                            double swidth = Math.Round((porintadd + 1) * newXvalue, 2, MidpointRounding.AwayFromZero);//初始位置
                            double width = swidth;//结束时位置
                            for (int i = 0; i < 500; i++)
                            {
                                if (porintadd + 1 + i <= linlength - 1)
                                {
                                    width = (porintadd + 1 + i) * newXvalue;
                                    //Print_Bind((porintadd + 1 + i));
                                }
                                else
                                {
                                    break;
                                }
                            }
                            colorFrom = Math.Round(swidth, 2, MidpointRounding.AwayFromZero);
                            colorTo = width;
                            Print_Color_Bind();

                            //Invoke(new ThreadStart(delegate ()
                            //{
                            //    try
                            //    {
                            //        tChart.Refresh();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        ListToText.Instance.WriteListToTextFile1(ex.ToString());
                            //    }
                            //}));
                        }
                        #endregion
                        //运算 电流是否达到开启值
                        Current_Open(data);

                        if (SCURRENT)//true 电流到达开启值  
                        {

                            //储存数量 10秒  
                            Save_Db_Source.Enqueue(e);
                            ////到达10秒后 SCURRENT=false isaround=true
                            //Invoke(new ThreadStart(delegate ()
                            //{
                            //    try
                            //    {
                            //        blSavePorint.Text = CurrentNum.ToString();
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        ListToText.Instance.WriteListToTextFile1(ex.ToString());
                            //    }
                            //}));
                            CurrentNum++;
                            if (CurrentNum > 23750) //存储达到 12500条 10秒的数据时 17500
                            {
                                SCURRENT = false;//关闭有效波段存储
                                topnum++;
                                this.BeginInvoke(new MethodInvoker(() =>
                                {
                                    Stop_Test(true, 1);
                                }));
                                break;
                            }
                        }
                        else
                        {
                            if (AroundSecond == 2)//电流到达关闭值
                            {
                                SCURRENT = false;//关闭有效波段存储
                                                 // 异步方法
                                if (endprint < 2500)
                                {
                                    Save_Db_Source.Enqueue(e);
                                    endprint++;
                                }
                                else
                                {
                                    this.BeginInvoke(new MethodInvoker(() =>
                                    {
                                        topnum++;
                                        Stop_Test(true, 1);
                                    }));
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        e.Hearder = e.Msg.Substring(0, 4);
                        if (e.Hearder == "5050")//返回电压只 要存储 
                        {
                            string vv = e.Msg.Substring(4, 4);
                            int v = Convert.ToInt16(vv, 16);

                            addv[vcount] = vv;
                            addvv[vcount] = v;
                            if (v > 0)
                            {
                                allv = allv + v;

                                vcount++;
                                Invoke(new ThreadStart(delegate ()
                                {
                                    lbVoltage.Text = (allv / vcount).ToString();
                                }));
                            }
                        }
                    }

                }
            }

        }

        /// <summary>
        /// 初始化电流配置
        /// </summary>
        private void Current_Config()
        {
            operation = 0;
            SCURRENT = false;

            countCurrent1 = 0;
            // around = 1;
            CurrentNum = 1;
            AroundSecond = 1;
        }

        /// <summary>
        /// 判断电流是否开始或结束
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private void Current_Open(string data)
        {

            //数据需要重新计算
            /*
             运算公式   (x1*x1)+(x2*x2)+...(xn*xn)   //计算平方 n为2000
             根运算 M Math.Sqrt((x1*x1)+(x2*x2)+...(xn*xn) /2000）           
             */
            for (int i = 0; i < 80; i++)
            {
                double Current = 0.0;
                int lengtht = data.Length;
                Vibration_Current vmodel = new Vibration_Current();
                int length = 24 * i;//截取位置 +1 默认不取第一个点位

                string Curren = "";
                //新板子
                if (COUNT_BASE_C == "1")
                {
                    Curren = data.Substring(12 + length, 4);
                }
                if (COUNT_BASE_C == "2")
                {
                    Curren = data.Substring(16 + length, 4);
                }
                if (COUNT_BASE_C == "3")
                {
                    Curren = data.Substring(20 + length, 4);
                }

                Current = Algorithm.Instance.Current_Algorithm_Double(Curren, I);

                countCurrent1 += Current * Current;

                operation++;
                if (operation >= 2000)
                {

                    //否 存储
                    // 判断电流是否启动 电流存储
                    double setSCURRENT = Convert.ToDouble(pub_Test_Plan.SCURRENT);
                    double setECURRENT = Convert.ToDouble(pub_Test_Plan.ECURRENT);

                    double Current1 = Math.Abs(Math.Sqrt(countCurrent1 / 2000));

                    if (Current1 >= setSCURRENT && !SCURRENT)
                    {
                        SCURRENT = true;
                    }
                    //结束存数据   条件1： 判断是否达到电流关闭 ，条件2：判断是电流关闭
                    if (Current1 <= setECURRENT && SCURRENT || CurrentNum == 12500)
                    {
                        SCURRENT = false;
                        AroundSecond = 2;//第二次开始存2秒电流数据
                    }
                    //运算完成后恢复初始值
                    operation = 1;
                    countCurrent1 = 0;
                }
            }
        }

        /// <summary>
        /// 绑定数数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="width"></param>
        private void Print_Bind(int index)
        {
            if (v1)
            {
                vline1.YValues[index] = 0;
            }
            if (v2)
            {
                vline2.YValues[index] = 0;
            }
            if (v3)
            {
                vline3.YValues[index] = 0;
            }
            if (c1)
            {
                cline1.YValues[index] = 0;
            }
            if (c2)
            {
                cline2.YValues[index] = 0;
            }
            if (c3)
            {
                cline3.YValues[index] = 0;
            }
        }
        /// <summary>
        /// 绑定数数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="width"></param>
        private void Print_Color_Bind()
        {
            if (colorTo > 0 || colorFrom > 0)
            {
                colorTo = 0;
                colorFrom = 0;

                Color c = new Color();
                if (v1)
                {

                    //  vline1.Colors.Clear();

                    //if (vline1.Colors.Count <= 500)
                    //{

                    //    //Clear 程序会崩溃
                    //    vline1.Colors.RemoveRange(0, vline1.Colors.Count);
                    //}
                    //else if (vline1.Colors.Count > 2000)
                    //{
                    //    vline1.Colors.RemoveRange(0, 2000);
                    //}
                    //else
                    //{
                    //    //for (int i = vline1.Colors.Count; i > 0; i--)
                    //    //{
                    //    //    if (vline1.Colors[i].Name == "Red")
                    //    //    {
                    //    //        vline1.Colors.Remove(vline1.Colors[i]);
                    //    //    }
                    //    //}

                    //    // vline1.Colors.RemoveRange(100, vline2.Colors.Count - 100);
                    //    vline1.Colors.RemoveRange(vline2.Colors.Count - 500, 500);

                    //}


                    //if (vline1.Colors.Count == 0)
                    //{
                    //vline1.Colors.RemoveRange(0, vline1.Colors.Count);
                    //vline1.ColorRange(vlist, 0, 0, c);

                    //if (vline1.Colors.Count == 500)
                    //{
                    //    vline1.Colors.RemoveRange(0, vline1.Colors.Count);
                    //}
                    //else if (vline1.Colors.Count > 600)
                    //{
                    //    vline1.Colors.RemoveRange(100, vline1.Colors.Count - 100);
                    //}

                    vline1.Colors.Remove(c);
                    // vline1.Colors.Add(c);
                    ValueList vlist = vline1.ValuesLists[0];

                    vline1.ColorRange(vlist, colorFrom, colorTo, c);

                    //}
                    //else
                    //{
                    //    vline1.Colors.RemoveAt(0);
                    //    vline1.Colors.Add(vline1.ValueColor(0));
                    //}
                }
                if (v2)
                {
                    vline2.Colors.Remove(c);
                    vline2.Colors.Add(c);
                    //ValueList vlist = vline2.ValuesLists[0];

                    //vline2.ColorRange(vlist, colorFrom, colorTo, c);
                }
                if (v3)
                {
                    vline3.Colors.Remove(c);
                    vline3.Colors.Add(c);
                    //ValueList vlist = vline3.ValuesLists[0];

                    //vline3.ColorRange(vlist, colorFrom, colorTo, c);
                }
                if (c1)
                {
                    cline1.Colors.Remove(c);
                    cline1.Colors.Add(c);

                    //ValueList vlist = cline1.ValuesLists[0];
                    //cline1.ColorRange(vlist, colorFrom, colorTo, c);
                }
                if (c2)
                {
                    cline2.Colors.Remove(c);
                    cline2.Colors.Add(c);
                    //ValueList vlist = cline2.ValuesLists[0];
                    //cline2.ColorRange(vlist, colorFrom, colorTo, c);
                }
                if (c3)
                {
                    cline3.Colors.Remove(c);
                    cline3.Colors.Add(c);
                    //ValueList vlist = cline3.ValuesLists[0];
                    //cline3.ColorRange(vlist, colorFrom, colorTo, c);
                }
            }
        }

        #endregion

        #endregion

        #region SQL 查询是否有数据库,没有生产数据库

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
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
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
                int i = 0;
                Xml_Node_Model model = new Xml_Node_Model();
                #region 开始插入分割线
                if (pub_Test_Plan.GETINFO == "1")
                {
                    while (Top_End_Data.Count < 1250)// 有进入死循环的 概率秒开秒关
                    {
                        if (!isAbort)//如果线程都关闭 了，还在循环则 结束循环
                        {
                            break;
                        }
                    }
                    model.DataSource = "分割线开始";
                    model.Id = "1";
                    XmlHelper.Insert(model);
                    for (int j = 0; j < AroundSecond; j++)
                    {
                        foreach (Udp_EventArgs e in Top_End_Data)
                        {
                            model = new Xml_Node_Model();
                            //  model.AddDate = e.AddDate;
                            model.Id = e.Msg.Substring(4, 4);
                            model.DataSource = e.Msg;
                            model.Data = new List<Xml_Element_Model>();
                            XmlHelper.Insert(model);
                        }
                    }
                    model = new Xml_Node_Model();
                    model.Id = "1";
                    model.DataSource = "分割线结束";
                    XmlHelper.Insert(model);

                    XmlHelper.Save();
                    // isaround = false;
                }
                #endregion
                //插入波形数据 条件一  一直插入 或 条件二 只要还有数据一直插入
                while (isAbort || Save_Db_Source.Count > 0)
                {
                    if (Save_Db_Source.Count > 0)
                    {
                        Udp_EventArgs e = new Udp_EventArgs();
                        Save_Db_Source.TryDequeue(out e);//取出队里数据并删除

                        model = new Xml_Node_Model();
                        model.Id = e.Msg.Substring(4, 4);
                        model.AddDate = e.AddDate;
                        model.DataSource = e.Msg;
                        model.Data = new List<Xml_Element_Model>();
                        XmlHelper.Insert(model);
                        if (i == 2000)
                        {
                            XmlHelper.Save();
                            i = 0;
                        };
                        i++;
                    }
                }
                //if (pub_Test_Plan.GETINFO == "1")
                //{
                //    #region 结束后插入分割线

                //    model = new Xml_Node_Model();
                //    model.DataSource = "分割线开始";
                //    model.Id = "1";
                //    XmlHelper.Insert(model);
                //    for (int j = 0; j < AroundSecond; j++)
                //    {
                //        foreach (Udp_EventArgs e in Top_End_Data)
                //        {
                //            model = new Xml_Node_Model();
                //            model.Id = e.Msg.Substring(4, 4);
                //            model.DataSource = e.Msg;
                //            model.Data = new List<Xml_Element_Model>();
                //            XmlHelper.Insert(model);
                //        }
                //    }
                //    Top_End_Data = new ConcurrentQueue<Udp_EventArgs>();
                //    model = new Xml_Node_Model();
                //    model.Id = "1";
                //    model.DataSource = "分割线结束";
                //    XmlHelper.Insert(model);
                //    //}
                //    #endregion
                //}
                //结束后保存下防止 漏数据
                XmlHelper.Save();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("");
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
            }
        }

        #endregion

        #endregion

        #region 页面控件数据操作

        #region 绑定测试计划
        /// <summary>
        /// 绑定测试计划 刷新左边树
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
            btnEnvelope.Enabled = false;

            // treeList.OptionsView.ShowIndicator = false;
            if (e.Node.Selected)
            {
                lbVoltage.Text = "0";//单击时电压清空
                if (treeList.Appearance.FocusedCell.BackColor != Color.SteelBlue)//选中测试计划 改变颜色
                {
                    treeList.Appearance.FocusedCell.BackColor = Color.SteelBlue;
                };
                publicnode = e.Node;
                pub_Test_Plan = Test_Plan_Bind(publicnode);
                tChart.Header.Text = pub_Test_Plan.DVNAME;

                if (pub_Test_Plan.GETINFO == "2")//判断是 按时长采集数据
                {
                    if (!string.IsNullOrEmpty(pub_Test_Plan.TIME_UNIT))
                    {
                        alltime = string.IsNullOrEmpty(pub_Test_Plan.TIME_UNIT) ? alltime : Convert.ToInt32(pub_Test_Plan.TIME_UNIT);
                    }
                    init_Chart_Config(alltime);
                    btnCTest.Enabled = false;
                }
                else//按电流触发采集数据
                {
                    alltime = 10;
                    init_Chart_Config(alltime);
                    btnCTest.Enabled = true;
                    if (isBtnTest)//只有不执行的时候才能进去
                    {
                        if (pub_Test_Plan.PARENTID != "0")
                        {
                            //if (!string.IsNullOrEmpty(pub_Test_Plan.DOUBLE_SP) &&
                            //    !string.IsNullOrEmpty(pub_Test_Plan.DOUBLE_EP))
                            //{
                            //    topnum = Convert.ToInt32(pub_Test_Plan.DOUBLE_SP);
                            //    alltopnum = Convert.ToInt32(pub_Test_Plan.DOUBLE_EP);
                            //}
                            IsOpensTest = true;
                        }
                    }
                }
                if (isBtnTest)
                {
                    if (pub_Test_Plan.V1 == "1")
                    {
                        this.ckV1.Checked = true;
                        v1 = true;
                    }
                    else
                    {
                        this.ckV1.Checked = false;
                        v1 = false;
                    }
                    if (pub_Test_Plan.V2 == "1")
                    {
                        this.ckV2.Checked = true;
                        v2 = true;
                    }
                    else
                    {
                        this.ckV2.Checked = false;
                        v2 = false;
                    }

                    if (pub_Test_Plan.V3 == "1")
                    {
                        this.ckV3.Checked = true;
                        v3 = true;
                    }
                    else
                    {
                        this.ckV3.Checked = false;
                        v3 = false;
                    }

                    if (pub_Test_Plan.C1 == "1")
                    {
                        this.ckC1.Checked = true;
                        c1 = true;
                    }
                    else
                    {
                        this.ckC1.Checked = false;
                        c1 = false;
                    }
                    if (pub_Test_Plan.C2 == "1")
                    {
                        this.ckC2.Checked = true;
                        c2 = true;
                    }
                    else
                    {
                        this.ckC2.Checked = false;
                        c2 = false;
                    }

                    if (pub_Test_Plan.C3 == "1")
                    {
                        this.ckC3.Checked = true;
                        c3 = true;
                    }
                    else
                    {
                        this.ckC3.Checked = false;
                        c3 = false;
                    }
                }
                Chart_Init();
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
                    lbVoltage.Text = model.VOLTAGE;
                    model.ISEDIT = "2";
                    if (model.PARENTID != "0")
                    {
                        Add_CursorTool();
                        string filepath = FileHelper.Local_Path_Get() + "Xml_Data\\" + pub_Test_Plan.DVNAME + ".xml";
                        bool isdd = FileHelper.IsFileExist(filepath);
                        if (FileHelper.IsFileExist(filepath))
                        {
                            alltime = 20;
                            Chart_Init();
                            Chart_Lond(filepath);

                            btnEnvelope.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("未找到该图形数据！");
                        }
                    }
                    else
                    {
                        using (FmAddTest form = new FmAddTest(model))
                        {
                            form.ShowDialog();
                        }
                        Tester_List_Bind();

                        Set_Foucs(model.ID);
                    }
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

                model.TEST_BASE_C = node.GetValue("TEST_BASE_C").ToString();
                model.TEST_SINGLE_DOUBLE = node.GetValue("TEST_SINGLE_DOUBLE").ToString();
                model.DOUBLE_SP = node.GetValue("DOUBLE_SP").ToString();
                model.DOUBLE_EP = node.GetValue("DOUBLE_EP").ToString();

                model.SINGLE_P = node.GetValue("SINGLE_P").ToString();
                model.TEST_ORDER = node.GetValue("TEST_ORDER").ToString();
                model.COUNT_BASE_C = node.GetValue("COUNT_BASE_C").ToString();
                model.VOLTAGE = node.GetValue("VOLTAGE").ToString();

                return model;
            }
            catch (Exception ex)
            {
                ListToText.Instance.WriteListToTextFile1(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 获取指定节点
        /// </summary>
        /// <param name="Id"></param>
        private TreeListNode Select_Top_Node(string Id)
        {
            TreeListNode node = null;
            List<TreeListNode> list = treeList.GetNodeList();
            foreach (TreeListNode n in list)
            {
                if (n.GetValue("ID").ToString() == Id)
                {
                    node = n;
                    break;
                }
            }
            return node;
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

        /// <summary>
        /// 树形图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 树形图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #endregion

        #region 包络线相关转换

        /// <summary>
        /// 包络线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnvelope_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Show_Open();

                double[] Out_x;
                double[] Out_y;
                Re_Envelope_Data(vx1, vy1, out Out_x, out Out_y);
                Chart_DataTable_Init();
                string time = Out_x[Out_x.Length - 1].ToString();
                Chart_Config(time);
                Chart_Data_Envelope_Lond(Out_x, Out_y);
                plLinePath.Enabled = false;
                Add_CursorTool();

                Show_End();

            }
            catch (Exception ex)
            {
                string ex1 = ex.ToString();
            }

        }
        /// <summary>
        /// 转换包络线数据
        /// </summary>
        /// <param name="Out_x">返回X轴数据</param>
        /// <param name="Out_y">返回Y轴数据</param>
        private void Re_Envelope_Data(double[] In_x, double[] In_y, out double[] Out_x, out double[] Out_y)
        {
            double spacing = (double)(1 * num) / allnum; ;
            double rc_up = txtRcUp.Text != "0" ? Convert.ToDouble(txtRcUp.Text) : 0.0001;
            double rc_dn = txtRcDn.Text != "0" ? Convert.ToDouble(txtRcDn.Text) : 0.01;
            Envelope_Algorithm.Instance.Envelope(spacing, In_x, In_y
                , out Out_x, out Out_y
                , rc_dn, rc_up);
        }

        /// <summary>
        /// 对比
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContrast_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FmContrast form = new FmContrast())
            {
                form.ShowDialog();
                if (form.DialogResult == DialogResult.OK)
                {
                    Show_Open();
                    int c = Convert.ToInt32(form.C);
                    int v = Convert.ToInt32(form.V);
                    var Node1 = Test_Plan_Bind(form.node1);
                    var Node2 = Test_Plan_Bind(form.node2);
                    ckChecked();
                    if (c == 1)
                    {
                        ckC1.Checked = true;
                        cks[0] = true;
                    }
                    else if (c == 2)
                    {
                        ckC2.Checked = true;
                    }
                    else if (c == 3)
                    {
                        ckC3.Checked = true;
                    }

                    if (v == 1)
                    {
                        ckV1.Checked = true;
                    }
                    else if (v == 2)
                    {
                        ckV2.Checked = true;
                    }
                    else if (v == 3)
                    {
                        ckV3.Checked = true;
                    }
                    Bind_IsDC();
                    string filepath = FileHelper.Local_Path_Get() + "Xml_Data\\" + Node1.DVNAME + ".xml";
                    if (FileHelper.IsFileExist(filepath))
                    {
                        if (!string.IsNullOrEmpty(filepath))
                        {
                            XmlHelper.Xml_To_Array_Contrast(filepath, cks,
                               out vx1, out vy1,
                               out cx1, out cy1
                                );
                        }
                    }
                    else
                    {
                        MessageBox.Show("样本数据 " + Node1.DVNAME + "  未找到该图形数据！");
                    }
                    filepath = FileHelper.Local_Path_Get() + "Xml_Data\\" + Node2.DVNAME + ".xml";
                    if (FileHelper.IsFileExist(filepath))
                    {
                        if (!string.IsNullOrEmpty(filepath))
                        {
                            XmlHelper.Xml_To_Array_Contrast(filepath, cks,
                               out vx2, out vy2,
                               out cx2, out cy2
                                );
                        }
                    }
                    else
                    {
                        MessageBox.Show("对比数据 " + Node2.DVNAME + "  未找到该图形数据！");
                    }
                    if (vx1.Length == 0)
                    {

                        Show_End();
                        MessageBox.Show("样本数据 " + Node1.DVNAME + "  未找到该图形数据！");
                        return;
                    }
                    if (vx2.Length == 0)
                    {
                        Show_End();
                        MessageBox.Show("对比数据 " + Node2.DVNAME + "  未找到该图形数据！");

                        return;
                    }
                    double time1 = vx1[vx1.Length - 1];
                    double time2 = vx2[vx2.Length - 1];

                    lbybname.Text = Node1.DVNAME;
                    lbybtime.Text = time1.ToString() + " 秒(sec)";

                    lbdbdata.Text = Node2.DVNAME;
                    lbdbtime.Text = time2.ToString() + " 秒(sec)";

                    alltime = 20;
                    min = 0;
                    max = 20;
                    plLinePath.Enabled = false;

                    Chart_DataTable_Init();
                    Chart_Config_Contrast(time1.ToString(), time2.ToString());

                    if (form.IsBlx)
                    {
                        double[] Out_x;
                        double[] Out_y;
                        double[] Out_x1;
                        double[] Out_y1;

                        Re_Envelope_Data(vx1, vy1, out Out_x, out Out_y);
                        Re_Envelope_Data(vx2, vy2, out Out_x1, out Out_y1);
                        Chart_Data_Envelope_Lond(Out_x, Out_y, Out_x1, Out_y1, true);
                    }
                    else
                    {
                        Chart_Data_Contrast_Lond();
                    }
                    Add_CursorTool();
                    //cursorTool.Active = true;


                    Show_End();
                }
            }
        }
        private void ckChecked()
        {
            ckC1.Checked = false;
            ckC2.Checked = false;
            ckC3.Checked = false;

            ckV1.Checked = false;
            ckV2.Checked = false;
            ckV3.Checked = false;
        }

        #endregion

        #region 显示动画加载
        private void Show_Open(string Msg = "正在加载图形...")
        {
            splashScreenManager.ShowWaitForm();
            splashScreenManager.SetWaitFormCaption("请稍后");
            splashScreenManager.SetWaitFormDescription(Msg);

        }
        private void Show_End()
        {
            splashScreenManager.SetWaitFormDescription("加载完成。");
            splashScreenManager.CloseWaitForm();
        }

        #endregion

    }
}