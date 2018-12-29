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
using DbHelper.Db_Model;
using DbHelper.Sqlite_Db;

namespace Load_Tap_Changer_Test
{
    public partial class FmTestConfig : DevExpress.XtraEditors.XtraForm
    {
        public FmTestConfig()
        {
            InitializeComponent();
        }
        public FmTestConfig(Test_Plan model)
        {
            InitializeComponent();
            oldmodel = model;
        }
        public Test_Plan oldmodel;
        public string id;
        public string DvName;
        public string topnum;
        public string alltopnum;
        /// <summary>
        /// 基础电流
        /// </summary>
        public int I;
        /// <summary>
        /// 判断基础电流
        /// </summary>
        public string COUNT_BASE_C;


        private void FmTestConfig_Load(object sender, EventArgs e)
        {
            Page_Data_Bind();
        }


        #region 执行方法

        /// <summary>
        /// 判断采样信息
        /// </summary>
        private void Get_Info_Type()
        {
            string GetType = this.rdoGETINFO.Text;
            if (GetType == "1")
            {
                this.txtGetUnit.Enabled = false;
                this.txtSA.Enabled = true;
                this.txtEA.Enabled = true;
                this.rdoTestType.Enabled = true;
            }
            else
            {
                this.txtGetUnit.Enabled = true;
                this.txtSA.Enabled = false;
                this.txtEA.Enabled = false;

                this.rdoTestType.SelectedIndex = 1;
                this.rdoTestType.Enabled = false;
                Get_Test_Type();
            }
        }

        /// <summary>
        /// 判断连续测试还是单次测试
        /// </summary>
        private void Get_Test_Type()
        {
            string GetType = this.rdoTestType.Text;
            if (GetType == "1")
            {
                this.gbDOUBLE.Enabled = true;
                this.gbSINGLE.Enabled = false;
            }
            else
            {
                this.gbDOUBLE.Enabled = false;
                this.gbSINGLE.Enabled = true;
                Get_Single();
            }
        }

        private void Get_Single()
        {
            int Place = Convert.ToInt32(this.CmbPlace.Text);
            if (RdoOrder.Text == "1")
            {
                RdoOrder.Properties.Items[0].Description = "前往后" + "(" + Place + "-" + (Place + 1) + ")";
                RdoOrder.Properties.Items[1].Description = "后往前";
            }
            else
            {
                RdoOrder.Properties.Items[0].Description = "前往后";

                RdoOrder.Properties.Items[1].Description = "后往前" + "(" + Place + "-" + (Place - 1) + ")";
            }
        }
        /// <summary>
        /// 初始化绑定页面数据
        /// </summary>
        private void Page_Data_Bind()
        {
            if (oldmodel.ISEDIT == "2")
            {
                ckC1.Checked = false;
                ckV1.Checked = false;

            }


            string dvname = oldmodel.DVNAME;

            this.cmbBaseC.Text = "10";
            int countNum = Convert.ToInt32(oldmodel.CONTACT_NUM);

            for (int i = 1; i <= countNum; i++)
            {
                CmbPlace.Properties.Items.Add(i);
            }
            CmbPlace.SelectedIndex = 0;

            //判断采样信息是那种状态
            Get_Info_Type();
            Get_Test_Type();
            if (oldmodel.GETINFO == "1")
            {
                rdoGETINFO.SelectedIndex = 0;
            }
            else
            {
                rdoGETINFO.SelectedIndex = 1;
            }

            this.txtGetUnit.Text = oldmodel.TIME_UNIT;
            this.txtSA.Text = oldmodel.SCURRENT;
            this.txtEA.Text = oldmodel.ECURRENT;

            if (oldmodel.V1 == "1")
            {
                this.ckV1.Checked = true;
            }
            if (oldmodel.V2 == "1")
            {
                this.ckV2.Checked = true;
            }
            if (oldmodel.V3 == "1")
            {
                this.ckV3.Checked = true;
            }
            if (oldmodel.C1 == "1")
            {
                this.ckC1.Checked = true;
            }
            if (oldmodel.C2 == "1")
            {
                this.ckC2.Checked = true;
            }
            if (oldmodel.C3 == "1")
            {
                this.ckC3.Checked = true;
            }
            this.txtSPlace.Text = "1";
            this.txtEPlace.Text = oldmodel.CONTACT_NUM;

        }

        #region  保存数据库

        /// <summary>
        /// 保存数据
        /// </summary>
        private void Save()
        {
            try
            {
                string TestType = rdoTestType.Text;

                oldmodel.PARENTID = oldmodel.ID;
                if (TestType == "1")
                {
                    int SPlace = Convert.ToInt32(this.txtSPlace.Text);
                    int EPlace = Convert.ToInt32(this.txtEPlace.Text);
                    if (EPlace > SPlace)
                    {
                        oldmodel.DVNAME = oldmodel.DVNAME + "_" + SPlace + "-" + (SPlace + 1) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                    else
                    {
                        oldmodel.DVNAME = oldmodel.DVNAME + "_" + EPlace + "-" + (EPlace - 1) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                }
                else
                {
                    int Place = Convert.ToInt32(this.CmbPlace.Text);
                    if (RdoOrder.Text == "1")
                    {
                        oldmodel.DVNAME = oldmodel.DVNAME + "_" + Place + "-" + (Place + 1) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                    else
                    {
                        oldmodel.DVNAME = oldmodel.DVNAME + "_" + Place + "-" + (Place - 1) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                }

                DvName = oldmodel.DVNAME;
                oldmodel.GETINFO = this.rdoGETINFO.Text;
                oldmodel.SCURRENT = oldmodel.GETINFO == "1" ? this.txtSA.Text : "";
                oldmodel.ECURRENT = oldmodel.GETINFO == "1" ? this.txtEA.Text : "";
                oldmodel.TIME_UNIT = oldmodel.GETINFO == "2" ? this.txtGetUnit.Text : "";

                oldmodel.V1 = ckV1.Checked ? "1" : "0";
                oldmodel.V2 = ckV2.Checked ? "1" : "0";
                oldmodel.V3 = ckV3.Checked ? "1" : "0";

                oldmodel.C1 = ckC1.Checked ? "1" : "0";
                oldmodel.C2 = ckC2.Checked ? "1" : "0";
                oldmodel.C3 = ckC3.Checked ? "1" : "0";

                oldmodel.TEST_BASE_C = this.cmbBaseC.Text;
                oldmodel.TEST_SINGLE_DOUBLE = this.rdoTestType.Text;
                oldmodel.DOUBLE_SP = this.txtSPlace.Text;
                oldmodel.DOUBLE_EP = this.txtEPlace.Text;
                oldmodel.SINGLE_P = this.CmbPlace.Text;

                oldmodel.TEST_ORDER = this.RdoOrder.Text;
                oldmodel.COUNT_BASE_C = this.RdoBaseC.Text;
                oldmodel.ISEDIT = "1";

                COUNT_BASE_C = oldmodel.COUNT_BASE_C;
                I = Convert.ToInt32(oldmodel.TEST_BASE_C);

                topnum = oldmodel.TEST_SINGLE_DOUBLE == "2" ? "0" : oldmodel.DOUBLE_SP;
                alltopnum = oldmodel.TEST_SINGLE_DOUBLE == "2" ? "0" : oldmodel.DOUBLE_EP;

                int count = 0;

                count = Db_Action.Instance.Test_Confige_Insert(oldmodel);

                if (count >= 1)
                {
                    id = count.ToString();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #endregion

        #region 按键事件
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            this.DialogResult = DialogResult.OK;
            // CreateDb();
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 判断采样信息是那种状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoGetInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //判断采样信息是那种状态
            Get_Info_Type();
        }
        #endregion

        private void rdoTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //判断连续测试还是单次测试
            Get_Test_Type();
        }

        private void RdoOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            Get_Single();
        }

    }
}