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
        Test_Plan oldmodel;
        public string id;
        public string DvName;

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
            }
            else
            {
                this.txtGetUnit.Enabled = true;
                this.txtSA.Enabled = false;
                this.txtEA.Enabled = false;
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
                Test_Plan model = new Test_Plan();
                string TestType = rdoTestType.Text;
                model.PARENTID = oldmodel.ID;
                if (TestType == "1")
                {
                    int SPlace = Convert.ToInt32(this.txtSPlace.Text);
                    int EPlace = Convert.ToInt32(this.txtEPlace.Text);
                    if (EPlace > SPlace)
                    {
                        model.DVNAME = oldmodel.DVNAME + "_" + SPlace + "-" + (SPlace + 1) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                    else
                    {
                        model.DVNAME = oldmodel.DVNAME + "_" + EPlace + "-" + (EPlace - 1) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                }
                else
                {
                    int Place = Convert.ToInt32(this.CmbPlace.Text);
                    if (RdoOrder.Text == "1")
                    {
                        model.DVNAME = oldmodel.DVNAME + "_" + Place + "-" + (Place + 1) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                    else
                    {
                        model.DVNAME = oldmodel.DVNAME + "_" + Place + "-" + (Place - 1) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                }

                DvName = model.DVNAME;
                model.GETINFO = this.rdoGETINFO.Text;
                model.SCURRENT = model.GETINFO == "1" ? this.txtSA.Text : "";
                model.ECURRENT = model.GETINFO == "1" ? this.txtEA.Text : "";
                model.TIME_UNIT = model.GETINFO == "2" ? this.txtGetUnit.Text : "";

                model.V1 = ckV1.Checked ? "1" : "0";
                model.V2 = ckV2.Checked ? "1" : "0";
                model.V3 = ckV3.Checked ? "1" : "0";

                model.C1 = ckC1.Checked ? "1" : "0";
                model.C2 = ckC2.Checked ? "1" : "0";
                model.C3 = ckC3.Checked ? "1" : "0";

                model.TEST_BASE_C = this.cmbBaseC.Text;
                model.TEST_SINGLE_DOUBLE = this.rdoTestType.Text;
                model.DOUBLE_SP = this.txtSPlace.Text;
                model.DOUBLE_EP = this.txtEPlace.Text;
                model.SINGLE_P = this.CmbPlace.Text;

                model.TEST_ORDER = this.RdoOrder.Text;
                model.COUNT_BASE_C = this.RdoBaseC.Text;
                model.ISEDIT = "1";
                int count = 0;

                count = Db_Action.Instance.Test_Confige_Insert(model);

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