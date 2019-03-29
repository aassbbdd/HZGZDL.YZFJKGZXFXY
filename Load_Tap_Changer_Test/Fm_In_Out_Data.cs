using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DbHelper.Db_Model;

using Common;
using Newtonsoft.Json;
using System.Data;
using Commons;
using DbHelper.Sqlite_Db;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Linq;

namespace Load_Tap_Changer_Test
{
    public partial class Fm_In_Out_Data : DevExpress.XtraEditors.XtraForm
    {
        public Fm_In_Out_Data()
        {
            InitializeComponent();
        }
        public Fm_In_Out_Data(TreeList tree)
        {
            InitializeComponent();
            treeList.DataSource = tree.DataSource;
        }
        TreeListNode node;
        private void btnOutData_Click(object sender, EventArgs e)
        {
           
            string savedata = "[]";
            string savePath = "";
            string path = "\\Xml_Data\\";
            List<Test_Plan> list = new List<Test_Plan>();
            //导出单个测试
            if (rdo_In_Out_Select.SelectedIndex == 0)
            {
                if (node == null)
                {
                    MessageBox.Show("请选择单个文件!");
                    return;
                }

                Test_Plan model = Test_Plan_Bind(node.ParentNode);
                list.Add(model);

                model = Test_Plan_Bind(node);
                model.PARENTNAME = node.ParentNode.GetValue("DVNAME").ToString();
                list.Add(model);

                FolderBrowserDialog openFileDialog = new FolderBrowserDialog();

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    savePath = openFileDialog.SelectedPath;
                }
                else
                {
                    return;
                }
                Show_Open();
                string sourcePath = Environment.CurrentDirectory + path + model.DVNAME + ".xml";
                string filename = model.DVNAME + ".xml";
                string remessage = FileHelper.CopyFile(sourcePath, savePath + path, filename);

                if (!string.IsNullOrEmpty(remessage))
                {
                    MessageBox.Show(remessage);
                    return;
                }

                savedata = JsonConvert.SerializeObject(list);
            }
            else//导出所有测试
            {
                FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    savePath = openFileDialog.SelectedPath;
                }
                else
                {
                    return;
                }
                Show_Open();
                string sourcePath = Environment.CurrentDirectory + path;
                FileHelper.CopyOldLabFilesToNewLab(sourcePath, savePath + path);
                list = Db_Select.Instance.All_Test_Cofig_Get();
                //测试数据
                savedata = JsonConvert.SerializeObject(list);
            }
            string json = "{\"Data\":" + savedata + " }";
            ListToText.Instance.WriteListToTextFile_New(savePath + path, json);
           // MessageBox.Show("导出成功");
            Show_End();
        }

        /// <summary>
        /// 单击树 行获取 行信息 并修改图表标题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node.Selected)
            {
                if (treeList.Appearance.FocusedCell.BackColor != Color.SteelBlue)//选中测试计划 改变颜色
                {
                    treeList.Appearance.FocusedCell.BackColor = Color.SteelBlue;
                };
                node = e.Node;
            }
        }
        private void btnInData_Click(object sender, EventArgs e)
        {
            Select_Path();
        }
        /// <summary>
        /// 存放测试数据文件夹名字
        /// </summary>
        string xmlpathfile = "\\Xml_Data\\";

        public void Select_Path()
        {
            try
            {
                

                #region 保存复制文件
                string OpenPath = "";
                FolderBrowserDialog openFileDialog = new FolderBrowserDialog();

                openFileDialog.Description = "请选择【TestData】文件夹";
                openFileDialog.Tag = "请选择【TestData】文件夹";
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    OpenPath = openFileDialog.SelectedPath;
                   
                }
                else
                {
                    return;
                }
                Show_Open("正在导入...");
                string infopath = OpenPath + "\\info.txt";

                if (!File.Exists(infopath))
                {
                    MessageBox.Show("请选择正确的导入文件夹!");
                    return;
                }

                string savePath = Environment.CurrentDirectory + xmlpathfile;
                FileHelper.CopyOldLabFilesToNewLab(OpenPath, savePath);

                #endregion

                #region 插入数据

                JObject json = (JObject)JsonConvert.DeserializeObject(Read(infopath));
                //原始数据
                List<Test_Plan> list = new List<Test_Plan>();
                list = JsonConvert.DeserializeObject<List<Test_Plan>>(json["Data"].ToString());
                //父级测试
                List<Test_Plan> parintlist = new List<Test_Plan>();
                //子集测试
                List<Test_Plan> sonlist = new List<Test_Plan>();

                parintlist = list.Where(x => x.PARENTID == "0").ToList();
                sonlist = list.Where(x => x.PARENTID != "0").ToList();
                foreach (Test_Plan item in parintlist)
                {
                    Test_Plan model = Db_Select.Instance.Single_Test_Cofig_Get(item.DVNAME);
                    if (model == null)
                    {
                        Db_Action.Instance.Test_Confige_Insert(item);
                    }
                }

                foreach (Test_Plan item in sonlist)
                {
                    Test_Plan model = Db_Select.Instance.Single_Test_Cofig_Get(item.PARENTNAME);
                    if (model != null)
                    {
                        Test_Plan son_model = Db_Select.Instance.Single_Test_Cofig_Get(item.DVNAME);
                        if (son_model != null)
                        {
                            continue;
                        }
                        string sourcePath = Environment.CurrentDirectory + xmlpathfile + item.DVNAME + ".xml";
                        if (File.Exists(sourcePath))
                        {
                            item.PARENTID = model.ID;
                            Db_Action.Instance.Test_Confige_Insert(item);
                        }
                    }
                }
                #endregion

                //MessageBox.Show("导入成功");

                #region 刷新数据

                treeList.DataSource = Db_Select.Instance.All_Test_Cofig_Get();

                #endregion

                Show_End("导入完毕！");

            }
            catch (Exception ex)
            {
                // LogHelper.Log_Write(LogHelper.Log_Level_e._0_Error, ex.ToString());
                //MessageBox.Show("导入失败");
                splashScreenManager.CloseWaitForm();
            }
        }

        /// <summary>
        /// 读取txt
        /// </summary>
        public string Read(string path)
        {
            string str_txt = "";
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                str_txt += line.ToString();
            }
            return str_txt;
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
                // ListToText.Instance.WriteListToTextFile1(ex.ToString());
                return null;
            }
        }
        private void Show_Open(string Msg = "正在导出...")
        {
            splashScreenManager.ShowWaitForm();
            splashScreenManager.SetWaitFormCaption("请稍后");
            splashScreenManager.SetWaitFormDescription(Msg);
        }
        private void Show_End(string msg= "导出完毕！")
        {
            splashScreenManager.SetWaitFormDescription(msg);
            splashScreenManager.CloseWaitForm();
        }
    }
}